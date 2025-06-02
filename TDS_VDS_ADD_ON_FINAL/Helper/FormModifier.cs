using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbouiCOM.Framework;

namespace TDS_VDS_ADD_ON_FINAL.Helper
{
    class FormModifier
    {
        public static void SBO_Application_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent, string formnum, string spos, string epos, string db)
        {

            BubbleEvent = true;
            try
            {

                if (pVal.FormTypeEx == formnum && pVal.EventType != SAPbouiCOM.BoEventTypes.et_FORM_UNLOAD)
                {
                    
                    SAPbouiCOM.Form oform = Application.SBO_Application.Forms.GetFormByTypeAndCount(pVal.FormType, pVal.FormTypeCount);

                    if (pVal.EventType == SAPbouiCOM.BoEventTypes.et_FORM_LOAD && pVal.BeforeAction == true)
                    {
                        //Total TDS Amount

                        SAPbouiCOM.Item StTDS = oform.Items.Add("ST_TDS", SAPbouiCOM.BoFormItemTypes.it_STATIC); // we are going to create
                        SAPbouiCOM.Item osrc = oform.Items.Item(spos);
                        StTDS.Top = osrc.Top + 15;
                        StTDS.Height = osrc.Height;
                        StTDS.Width = osrc.Width;
                        StTDS.Left = osrc.Left;


                        //property static field
                        SAPbouiCOM.StaticText ostds = ((SAPbouiCOM.StaticText)(StTDS.Specific));
                        ostds.Caption = "Total TDS";


                        SAPbouiCOM.Item EtTDS = oform.Items.Add("ET_TDS", SAPbouiCOM.BoFormItemTypes.it_EDIT); // we are going to create
                        SAPbouiCOM.Item orsc1 = oform.Items.Item(epos);
                        EtTDS.Top = StTDS.Top;
                        EtTDS.Height = orsc1.Height;
                        EtTDS.Width = orsc1.Width;
                        EtTDS.Left = orsc1.Left;

                        //specific property for edit text.
                        SAPbouiCOM.EditText oedtds = (SAPbouiCOM.EditText)(EtTDS.Specific);
                        oedtds.Item.Enabled = false; // to disABLE A FIELD.
                        oedtds.DataBind.SetBound(true, db, "U_TTDSAMT"); //TO SAVE THE VALUE IN TABLE


                        // Totol VDS Amount
                        SAPbouiCOM.Item StVDS = oform.Items.Add("ST_VDS", SAPbouiCOM.BoFormItemTypes.it_STATIC); // we are going to create
                        SAPbouiCOM.Item osrc2 = oform.Items.Item("ST_TDS");
                        StVDS.Top = osrc2.Top + 15;
                        StVDS.Height = osrc2.Height;
                        StVDS.Width = osrc2.Width;
                        StVDS.Left = osrc2.Left;


                        //property static field
                        SAPbouiCOM.StaticText osvds = ((SAPbouiCOM.StaticText)(StVDS.Specific));
                        osvds.Caption = "Total VDS";


                        SAPbouiCOM.Item EtVDS = oform.Items.Add("ET_VDS", SAPbouiCOM.BoFormItemTypes.it_EDIT); // we are going to create
                        SAPbouiCOM.Item orsc3 = oform.Items.Item("ET_TDS");
                        EtVDS.Top = StVDS.Top;
                        EtVDS.Height = orsc3.Height;
                        EtVDS.Width = orsc3.Width;
                        EtVDS.Left = orsc3.Left;

                        //specific property for edit text.
                        SAPbouiCOM.EditText oedvds = (SAPbouiCOM.EditText)(EtVDS.Specific);
                        oedvds.Item.Enabled = false; // to disABLE A FIELD.
                        oedvds.DataBind.SetBound(true, db, "U_TVDSAMT"); //TO SAVE THE VALUE IN TABLE



                      

                        // Adding CFL 
                        // === Step 1: Add CFL ===
                        SAPbouiCOM.ChooseFromListCollection oCFLs = oform.ChooseFromLists;
                        SAPbouiCOM.ChooseFromListCreationParams oCFLParams = (SAPbouiCOM.ChooseFromListCreationParams)Application.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams);

                        oCFLParams.MultiSelection = false;
                        oCFLParams.ObjectType = "FIL_M_TVGRPM";  // UDO object type
                        oCFLParams.UniqueID = "CFL_TV";

                        SAPbouiCOM.ChooseFromList oCFL = oCFLs.Add(oCFLParams);

                        // === Step 2: Bind CFL to Matrix Column ===
                        SAPbouiCOM.Matrix oMatrix = (SAPbouiCOM.Matrix)oform.Items.Item("38").Specific;
                        SAPbouiCOM.Column oColumn = oMatrix.Columns.Item("U_TVCODE");

                        SAPbouiCOM.Column oColumn1 = oMatrix.Columns.Item("U_TDSAMT");
                        //oColumn1.Editable = true;

                        SAPbouiCOM.Column oColumn2 = oMatrix.Columns.Item("U_VDSAMT");
                        //oColumn2.Editable = true;
                      

                        oColumn.ChooseFromListUID = "CFL_TV";
                        oColumn.ChooseFromListAlias = "Code";  // This must match the field in the UDO

                        Application.SBO_Application.SetStatusBarMessage("CFL successfully added to U_TVCODE", SAPbouiCOM.BoMessageTime.bmt_Short, false);

                      

                    }

                    if (pVal.FormTypeEx == formnum && pVal.EventType == SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST && pVal.BeforeAction == false && pVal.ItemUID == "38" && pVal.ColUID == "U_TVCODE")
                    {
                        int selectedRow = pVal.Row;

                        SAPbouiCOM.Form oForm = Application.SBO_Application.Forms.Item(pVal.FormUID);
                        SAPbouiCOM.Matrix oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item("38").Specific;
                        SAPbouiCOM.EditText item = (SAPbouiCOM.EditText)oMatrix.Columns.Item("1").Cells.Item(selectedRow).Specific;
                        string itemCode = item.Value.Trim();

                        if (!string.IsNullOrWhiteSpace(itemCode))
                        {
                            //selectedRow = SelectedMatrixRow;
                            try
                            {

                                // Get selected row from CFL
                                SAPbouiCOM.ChooseFromListEvent cflEvent = (SAPbouiCOM.ChooseFromListEvent)pVal;
                                SAPbouiCOM.DataTable dt = cflEvent.SelectedObjects;

                                if (dt != null)
                                {
                                    string grpCode = dt.GetValue("Code", 0).ToString();
                                    int row = pVal.Row;

                                    CollectData(grpCode, oMatrix, row, out double amt, out double tdsRate, out string tdsRank, out double vdsRate, out string vdsRank);

                                    SAPbouiCOM.ComboBox inclu = (SAPbouiCOM.ComboBox)oMatrix.Columns.Item("U_TAXINCL").Cells.Item(row).Specific;
                                    string icluStr = inclu.Value.Trim();


                                    // Calculate TDS and VDS
                                    (double tdsAmt, double vdsAmt) = TDSVDSCalculator.CalculateTDSVDS(amt, tdsRate , tdsRank , vdsRate , vdsRank , icluStr);

                                    // Set TDSAMT and VDSAMT in matrix (POR1 UDFs)
                                    ((SAPbouiCOM.EditText)oMatrix.Columns.Item("U_TDSAMT").Cells.Item(row).Specific).Value = tdsAmt.ToString("F2");
                                    ((SAPbouiCOM.EditText)oMatrix.Columns.Item("U_VDSAMT").Cells.Item(row).Specific).Value = vdsAmt.ToString("F2");
                                   
                                    ((SAPbouiCOM.EditText)oMatrix.Columns.Item("U_TVCODE").Cells.Item(row).Specific).Value = grpCode;


                                    TDSVDSCalculator.CalculateTotalTDSVDS(oForm);

                                }
                            }

                            catch (Exception ex)
                            {
                                Application.SBO_Application.SetStatusBarMessage("Error: " + ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, true);
                            }
                        }
                        else
                        {
                            Application.SBO_Application.SetStatusBarMessage("Select Item First ", SAPbouiCOM.BoMessageTime.bmt_Short, true);
                        }
                    }

                    //if (pVal.FormTypeEx == "141" && pVal.EventType == SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST && pVal.BeforeAction == false && pVal.ItemUID == "38" && pVal.ColUID == "1")
                    //{
                    //    int selectedRow = pVal.Row;
                    //    ItemCFLSelection = true;
                    //    SelectedMatrixRow = selectedRow;
                    //}

                    if (pVal.FormTypeEx == formnum && pVal.ItemUID == "38" && pVal.EventType == SAPbouiCOM.BoEventTypes.et_LOST_FOCUS && pVal.BeforeAction == false)
                    {
                        if (pVal.ColUID == "11" || pVal.ColUID == "14" || pVal.ColUID == "15")
                        {

                            try
                            {
                                SAPbouiCOM.Form oForm = Application.SBO_Application.Forms.Item(pVal.FormUID);
                                SAPbouiCOM.Matrix oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item("38").Specific;
                                int row = pVal.Row;
                                SAPbouiCOM.EditText etv = (SAPbouiCOM.EditText)oMatrix.Columns.Item("U_TVCODE").Cells.Item(row).Specific;
                                string tvCode = etv.Value.Trim();

                                if (!string.IsNullOrWhiteSpace(tvCode))
                                {
                                    CollectData(tvCode, oMatrix, row, out double amt, out double tdsRate, out string tdsRank, out double vdsRate, out string vdsRank);

                                    SAPbouiCOM.ComboBox inclu = (SAPbouiCOM.ComboBox)oMatrix.Columns.Item("U_TAXINCL").Cells.Item(row).Specific;
                                    string icluStr = inclu.Value.Trim();

                                    // Calculate TDS and VDS
                                    (double tdsAmt, double vdsAmt) = TDSVDSCalculator.CalculateTDSVDS(amt, tdsRate, tdsRank, vdsRate, vdsRank, icluStr);



                                    SAPbouiCOM.EditText TDS = (SAPbouiCOM.EditText)oMatrix.Columns.Item("U_TDSAMT").Cells.Item(row).Specific;
                                    TDS.Value = tdsAmt.ToString("F2");

                                    SAPbouiCOM.EditText VDS = (SAPbouiCOM.EditText)oMatrix.Columns.Item("U_VDSAMT").Cells.Item(row).Specific;
                                    VDS.Value = vdsAmt.ToString("F2");


                                    TDSVDSCalculator.CalculateTotalTDSVDS(oForm);


                                }
                            }
                            catch (Exception ex)
                            {
                                Application.SBO_Application.SetStatusBarMessage("Error: " + ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, true);
                            }

                        }
                        if (pVal.ColUID == "U_TVCODE")
                        {
                            SAPbouiCOM.Form oForm = Application.SBO_Application.Forms.Item(pVal.FormUID);
                            SAPbouiCOM.Matrix oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item("38").Specific;
                            int row = pVal.Row;
                            SAPbouiCOM.EditText etv = (SAPbouiCOM.EditText)oMatrix.Columns.Item("U_TVCODE").Cells.Item(row).Specific;
                            string tvCode = etv.Value.Trim();
                            if (string.IsNullOrWhiteSpace(tvCode))
                            {

                                //(double tdsAmt, double vdsAmt) = CalculateTDSVDS(0.0, 0.0, 0.0);
                                (double tdsAmt, double vdsAmt) = TDSVDSCalculator.CalculateTDSVDS(0.0, 0.0,"1", 0.0, "1", "Y");

                                SAPbouiCOM.EditText TDS = (SAPbouiCOM.EditText)oMatrix.Columns.Item("U_TDSAMT").Cells.Item(row).Specific;
                                TDS.Value = tdsAmt.ToString("F2");

                                SAPbouiCOM.EditText VDS = (SAPbouiCOM.EditText)oMatrix.Columns.Item("U_VDSAMT").Cells.Item(row).Specific;
                                VDS.Value = vdsAmt.ToString("F2");

                                

                                TDSVDSCalculator.CalculateTotalTDSVDS(oForm);

                                //CalculateTotalTDSVDS(oForm);

                            }
                        }
                    }

                    if (pVal.FormTypeEx == formnum && pVal.ItemUID == "38" && pVal.ColUID == "U_TAXINCL" && pVal.EventType == SAPbouiCOM.BoEventTypes.et_COMBO_SELECT && pVal.BeforeAction == false)
                    {
                        SAPbouiCOM.Form oForm = Application.SBO_Application.Forms.Item(pVal.FormUID);
                        SAPbouiCOM.Matrix oMatrix = (SAPbouiCOM.Matrix)oform.Items.Item("38").Specific;

                        // Access the ComboBox control in the specific cell
                        SAPbouiCOM.ComboBox cbCell = (SAPbouiCOM.ComboBox)oMatrix.Columns.Item("U_TAXINCL").Cells.Item(pVal.Row).Specific;
                        string selectedValue = cbCell.Selected.Value;

                        SAPbouiCOM.EditText etv = (SAPbouiCOM.EditText)oMatrix.Columns.Item("U_TVCODE").Cells.Item(pVal.Row).Specific;
                        string grpCode = etv.Value.Trim();

                        // Example: Update another column based on selection
                        if (selectedValue == "Y")
                        {
                            CollectData(grpCode, oMatrix, pVal.Row, out double amt, out double tdsRate, out string tdsRank, out double vdsRate, out string vdsRank);
                           
                            (double tdsAmt, double vdsAmt) = TDSVDSCalculator.CalculateTDSVDS(amt, tdsRate, tdsRank, vdsRate, vdsRank, selectedValue);

                            SAPbouiCOM.EditText TDS = (SAPbouiCOM.EditText)oMatrix.Columns.Item("U_TDSAMT").Cells.Item(pVal.Row).Specific;
                            TDS.Value = tdsAmt.ToString("F2");

                            SAPbouiCOM.EditText VDS = (SAPbouiCOM.EditText)oMatrix.Columns.Item("U_VDSAMT").Cells.Item(pVal.Row).Specific;
                            VDS.Value = vdsAmt.ToString("F2");


                            TDSVDSCalculator.CalculateTotalTDSVDS(oForm);
                        }
                        else if (selectedValue == "N")
                        {
                            CollectData(grpCode, oMatrix, pVal.Row, out double amt, out double tdsRate, out string tdsRank, out double vdsRate, out string vdsRank);
                            
                            (double tdsAmt, double vdsAmt) = TDSVDSCalculator.CalculateTDSVDS(amt, tdsRate, tdsRank, vdsRate, vdsRank, selectedValue);

                            SAPbouiCOM.EditText TDS = (SAPbouiCOM.EditText)oMatrix.Columns.Item("U_TDSAMT").Cells.Item(pVal.Row).Specific;
                            TDS.Value = tdsAmt.ToString("F2");

                            SAPbouiCOM.EditText VDS = (SAPbouiCOM.EditText)oMatrix.Columns.Item("U_VDSAMT").Cells.Item(pVal.Row).Specific;
                            VDS.Value = vdsAmt.ToString("F2");


                            TDSVDSCalculator.CalculateTotalTDSVDS(oForm);
                        }
                    }





                }

            }
            catch (Exception ex)
            {
                Application.SBO_Application.SetStatusBarMessage("Error in Itemevnt for SAP Screen - " + ex.ToString(), SAPbouiCOM.BoMessageTime.bmt_Medium, true);
            }
        }


        public static void CollectData(string grpCode, SAPbouiCOM.Matrix oMatrix, int row, out double amt, out double tdsRate, out string tdsRank, out double vdsRate, out string vdsRank)
        {
            // Initialize output variables
            amt = 0.0;
            tdsRate = 0.0;
            vdsRate = 0.0;
            tdsRank = "";
            vdsRank = "";

            try
            {
                // Get updated value from column 21
                SAPbouiCOM.EditText oAmtCell = (SAPbouiCOM.EditText)oMatrix.Columns.Item("21").Cells.Item(row).Specific;
                string amtStr = oAmtCell.Value.Trim();
                string numericValue = amtStr.StartsWith("BDT") ? amtStr.Substring(4).Trim() : amtStr;

                amt = double.TryParse(numericValue, out double parsedAmt) ? parsedAmt : 0.0;

                // Query for TDS and VDS rates
                SAPbobsCOM.Recordset oRS = (SAPbobsCOM.Recordset)Global.oComp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string query = $@"
            SELECT T1.""U_RANK"", T0.""U_WHLDTYPE"", T0.""U_RATE""
            FROM ""@FIL_MH_TVM"" T0
            INNER JOIN ""@FIL_MR_TVGRPM"" T1 ON T0.""Code"" = T1.""U_TAXCODE""
            WHERE T1.""Code"" = '{grpCode}'";

                oRS.DoQuery(query);

                while (!oRS.EoF)
                {
                    string type = oRS.Fields.Item("U_WHLDTYPE").Value.ToString();
                    string rank = oRS.Fields.Item("U_RANK").Value.ToString();
                    double rate = Convert.ToDouble(oRS.Fields.Item("U_RATE").Value);

                    if (type == "T") // TDS
                    {
                        tdsRate = rate;
                        tdsRank = rank;
                    }
                    else if (type == "V") // VDS
                    {
                        vdsRate = rate;
                        vdsRank = rank;
                    }

                    oRS.MoveNext();
                }
            }
            catch (Exception ex)
            {
                Application.SBO_Application.MessageBox("Error in CollectData: " + ex.Message);
            }
        }



    }
}
