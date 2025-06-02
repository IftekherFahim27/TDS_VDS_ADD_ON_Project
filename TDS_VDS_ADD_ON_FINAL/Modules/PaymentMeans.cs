using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbouiCOM.Framework;
using TDS_VDS_ADD_ON_FINAL.Helper;

namespace TDS_VDS_ADD_ON_FINAL.Modules
{
    class PaymentsMeans
    {
        public PaymentsMeans()
        {
            Application.SBO_Application.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
        }

        public double amtbank;
        public double amtcash;
        public double baseamt;
        //int bankfast=0;
        //int cashfast=0;
        public double basebank;
        public double remainbank;

        double overallAmount, totalTdsVds;

        bool isBtnClicked = false;
        bool isFirstFocusSet = false;
        string firstFocusedItem = "";
        string secondItem = "";
        double paymentDifference = 0.0;
        double remainingAmount = 0.0;
        bool isSecondValuePending = false;



        private void SBO_Application_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                if (pVal.FormTypeEx == "196" && pVal.EventType != SAPbouiCOM.BoEventTypes.et_FORM_UNLOAD)
                {
                    //define a form in 3 ways 
                    SAPbouiCOM.Form oform = Application.SBO_Application.Forms.GetFormByTypeAndCount(pVal.FormType, pVal.FormTypeCount);

                    if (pVal.EventType == SAPbouiCOM.BoEventTypes.et_FORM_LOAD && pVal.BeforeAction == true)
                    {
                        SAPbouiCOM.Item oNewItemN;
                        SAPbouiCOM.Item oItem;
                        //Adding button
                        oNewItemN = oform.Items.Add("btn1", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                        oItem = oform.Items.Item("8");

                        oNewItemN.Left = oItem.Left + 140;
                        oNewItemN.Width = 80;
                        oNewItemN.Height = oItem.Height;
                        oNewItemN.Top = oItem.Top;
                        oNewItemN.Visible = true;
                        SAPbouiCOM.Button btn = (SAPbouiCOM.Button)oNewItemN.Specific;
                        btn.Caption = "Load TDS VDS";



                    }

                    if (pVal.EventType == SAPbouiCOM.BoEventTypes.et_CLICK && pVal.BeforeAction == false && pVal.ItemUID == "btn1")
                    {
                        try
                        {
                            // === Step 0: Get values from Form 426 ===
                            double tdsAmount = 0.0;
                            double vdsAmount = 0.0;

                            // Loop through all open forms to find Form 426
                            for (int i = 0; i < Application.SBO_Application.Forms.Count; i++)
                            {
                                SAPbouiCOM.Form otherForm = Application.SBO_Application.Forms.Item(i);
                                if (otherForm.TypeEx == "426")
                                {
                                    // Get EditText values
                                    SAPbouiCOM.EditText etTDS = (SAPbouiCOM.EditText)otherForm.Items.Item("ET_TDS").Specific;
                                    SAPbouiCOM.EditText etVDS = (SAPbouiCOM.EditText)otherForm.Items.Item("ET_VDS").Specific;

                                    double.TryParse(etTDS.Value.Trim(), out tdsAmount);
                                    double.TryParse(etVDS.Value.Trim(), out vdsAmount);
                                    break;
                                }
                            }

                            // === Step 1: VDS Calculation & Set on Matrix 112 ===
                            SAPbouiCOM.Matrix oMatrix112 = (SAPbouiCOM.Matrix)oform.Items.Item("112").Specific;

                            SAPbouiCOM.ComboBox oCombo = (SAPbouiCOM.ComboBox)oMatrix112.Columns.Item("41").Cells.Item(1).Specific;
                            oCombo.Select("TDS", SAPbouiCOM.BoSearchKey.psk_ByValue);
                            string cc = "TDS";

                            SAPbobsCOM.Recordset oRS = (SAPbobsCOM.Recordset)Global.oComp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                            string qstr = string.Format(@"SELECT {0}AcctCode{0} FROM {0}OCRC{0} WHERE {0}CardName{0} ='" + cc + "'", '"');
                            oRS.DoQuery(qstr);

                            string tdsgl = Convert.ToString(oRS.Fields.Item("AcctCode").Value);
                            SAPbouiCOM.EditText oEdittdsgl = (SAPbouiCOM.EditText)oMatrix112.Columns.Item("67").Cells.Item(1).Specific;
                            oEdittdsgl.Value = tdsgl;

                            SAPbouiCOM.EditText oEdit = (SAPbouiCOM.EditText)oMatrix112.Columns.Item("46").Cells.Item(1).Specific;
                            oEdit.Value = tdsAmount.ToString("F2");


                            // === Step 2: Insert into Matrix 55 - First Row ===
                            SAPbouiCOM.Matrix oMatrix55 = (SAPbouiCOM.Matrix)oform.Items.Item("55").Specific;
                            int newRow1 = oMatrix55.RowCount;

                            SAPbouiCOM.EditText col22Row1 = (SAPbouiCOM.EditText)oMatrix55.Columns.Item("22").Cells.Item(newRow1).Specific;
                            col22Row1.Value = "Define New";

                            // === Step 3: Switch ComboBox to VDS and update amount ===
                            oCombo.Select("VDS", SAPbouiCOM.BoSearchKey.psk_ByValue);
                            cc = "VDS";

                            SAPbobsCOM.Recordset oRS2 = (SAPbobsCOM.Recordset)Global.oComp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                            string qstr2 = string.Format(@"SELECT {0}AcctCode{0} FROM {0}OCRC{0} WHERE {0}CardName{0} ='" + cc + "'", '"');
                            oRS2.DoQuery(qstr2);

                            string vdsgl = Convert.ToString(oRS2.Fields.Item("AcctCode").Value);
                            SAPbouiCOM.EditText oEditvdsgl = (SAPbouiCOM.EditText)oMatrix112.Columns.Item("67").Cells.Item(1).Specific;
                            oEditvdsgl.Value = vdsgl;

                            oEdit.Value = vdsAmount.ToString("F2");

                            SAPbouiCOM.EditText balance = (SAPbouiCOM.EditText)oform.Items.Item("12").Specific;
                            string bal = balance.Value.Trim();
                            string bal2 = bal.StartsWith("BDT") ? bal.Substring(4).Trim() : bal;
                            baseamt = double.TryParse(bal2, out double parsedAmt) ? parsedAmt : 0;

                            overallAmount = baseamt;

                            SAPbouiCOM.EditText tv = (SAPbouiCOM.EditText)oform.Items.Item("37").Specific;
                            string tvbal = tv.Value.Trim();
                            string tvbal2 = tvbal.StartsWith("BDT") ? tvbal.Substring(4).Trim() : tvbal;
                            totalTdsVds = double.TryParse(tvbal2, out double parsedAmt2) ? parsedAmt2 : 0;



                            paymentDifference = overallAmount - totalTdsVds;
                            if (paymentDifference < 0) paymentDifference = 0;

                            // Initialize
                            isBtnClicked = true;
                            isFirstFocusSet = false;
                            isSecondValuePending = false;
                            firstFocusedItem = "";
                            secondItem = "";
                            remainingAmount = 0.0;

                        }
                        catch (Exception ex)
                        {
                            Application.SBO_Application.SetStatusBarMessage("Error: " + ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, true);
                        }
                    }

                    //if (pVal.EventType == SAPbouiCOM.BoEventTypes.et_GOT_FOCUS && pVal.BeforeAction == false && pVal.ItemUID == "34")
                    //{


                    //    SAPbouiCOM.EditText balance2 = (SAPbouiCOM.EditText)oform.Items.Item("37").Specific;
                    //    string bal3 = balance2.Value.Trim();
                    //    string bal4 = bal3.StartsWith("BDT") ? bal3.Substring(4).Trim() : bal3;
                    //    amtbank = double.TryParse(bal4, out double parsedAmt2) ? parsedAmt2 : 0;

                    //    basebank = amtbank;

                    //    double remainBalance = baseamt-amtbank ;
                    //    SAPbouiCOM.EditText oEditamt = (SAPbouiCOM.EditText)oform.Items.Item("34").Specific;
                    //    oEditamt.Value = remainBalance.ToString("F2");

                    //    remainbank = remainBalance;


                    //}

                    //if (pVal.EventType == SAPbouiCOM.BoEventTypes.et_LOST_FOCUS && pVal.BeforeAction == false && pVal.ItemUID == "34")
                    //{
                    //    SAPbouiCOM.EditText balance2 = (SAPbouiCOM.EditText)oform.Items.Item("34").Specific;
                    //    string bal3 = balance2.Value.Trim();
                    //    string bal4 = bal3.StartsWith("BDT") ? bal3.Substring(4).Trim() : bal3;
                    //    amtbank = double.TryParse(bal4, out double parsedAmt2) ? parsedAmt2 : 0;

                    //    amtcash = baseamt - (amtbank + basebank);

                    //}




                    //if (pVal.EventType == SAPbouiCOM.BoEventTypes.et_GOT_FOCUS && pVal.BeforeAction == false && pVal.ItemUID == "38")
                    //{

                    //    double remainBalance = amtcash;
                    //    SAPbouiCOM.EditText oEditamt = (SAPbouiCOM.EditText)oform.Items.Item("38").Specific;
                    //    oEditamt.Value = remainBalance.ToString("F2");

                    //    //baseamt = remainBalance;

                    //}
                    // First Time Got Focus After btn1
                    // 2. Handle First Field Got Focus (Bank or Cash)

                    if (pVal.EventType == SAPbouiCOM.BoEventTypes.et_GOT_FOCUS && pVal.BeforeAction == false && isBtnClicked && (pVal.ItemUID == "34" || pVal.ItemUID == "38"))
                    {
                        // Detect active pane
                        int currentPane = oform.PaneLevel;

                        // Skip if wrong pane
                        if ((pVal.ItemUID == "34" && currentPane != 2) || (pVal.ItemUID == "38" && currentPane != 3))
                            return;

                        // Handle First Focus
                        if (!isFirstFocusSet)
                        {
                            firstFocusedItem = pVal.ItemUID;               // "34" or "38"
                            secondItem = (firstFocusedItem == "34") ? "38" : "34";
                            isFirstFocusSet = true;

                            // Set full difference in first field
                            SAPbouiCOM.EditText txtFirst = (SAPbouiCOM.EditText)oform.Items.Item(firstFocusedItem).Specific;
                            txtFirst.Value = paymentDifference.ToString("F2");

                            // Don't set second yet (may not be visible)
                            remainingAmount = 0.0;
                        }
                        else if (pVal.ItemUID == secondItem && isSecondValuePending)
                        {
                            // Set remaining in second field when it gets focus
                            SAPbouiCOM.EditText txtSecond = (SAPbouiCOM.EditText)oform.Items.Item(secondItem).Specific;
                            txtSecond.Value = remainingAmount.ToString("F2");

                            isSecondValuePending = false; // Done
                        }
                    }

                    // 3. Handle First Field Lost Focus: Compute Remaining
                    if (pVal.EventType == SAPbouiCOM.BoEventTypes.et_LOST_FOCUS && isBtnClicked && isFirstFocusSet && pVal.ItemUID == firstFocusedItem)
                    {
                        //SAPbouiCOM.EditText txtFirst = (SAPbouiCOM.EditText)oform.Items.Item(firstFocusedItem).Specific;
                        double firstValue = 0.0;


                        SAPbouiCOM.EditText balance2 = (SAPbouiCOM.EditText)oform.Items.Item(firstFocusedItem).Specific;
                        string bal3 = balance2.Value.Trim();
                        string bal4 = bal3.StartsWith("BDT") ? bal3.Substring(4).Trim() : bal3;
                        firstValue = double.TryParse(bal4, out double parsedAmt2) ? parsedAmt2 : 0;



                        remainingAmount = paymentDifference - firstValue;
                        if (remainingAmount < 0) remainingAmount = 0.0;

                        isSecondValuePending = true;
                    }

















                }




            }
            catch (Exception ex)
            {
                Application.SBO_Application.SetStatusBarMessage("Error in Itemevnt for SAP Screen - " + ex.ToString(), SAPbouiCOM.BoMessageTime.bmt_Medium, true);
            }
        }


    }
}
