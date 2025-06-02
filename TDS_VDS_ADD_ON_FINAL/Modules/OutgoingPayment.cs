using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbouiCOM.Framework;
using TDS_VDS_ADD_ON_FINAL.Helper;


namespace TDS_VDS_ADD_ON_FINAL.Modules
{
    class OutgoingPayment
    {
        public OutgoingPayment()
        {
            Application.SBO_Application.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);

        }

        public double Amt;
        public string docEntry;

        private void SBO_Application_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                if (pVal.FormTypeEx == "426" && pVal.EventType != SAPbouiCOM.BoEventTypes.et_FORM_UNLOAD)
                {
                    //define a form in 3 ways 
                    SAPbouiCOM.Form oform = Application.SBO_Application.Forms.GetFormByTypeAndCount(pVal.FormType, pVal.FormTypeCount);

                    if (pVal.EventType == SAPbouiCOM.BoEventTypes.et_FORM_LOAD && pVal.BeforeAction == true)
                    {
                        //Total TDS Amount

                        SAPbouiCOM.Item StTDS = oform.Items.Add("ST_TDS", SAPbouiCOM.BoFormItemTypes.it_STATIC); // we are going to create
                        SAPbouiCOM.Item osrc = oform.Items.Item("53");
                        StTDS.Top = osrc.Top + 15;
                        StTDS.Height = osrc.Height;
                        StTDS.Width = osrc.Width;
                        StTDS.Left = osrc.Left;


                        //property static field
                        SAPbouiCOM.StaticText ostds = ((SAPbouiCOM.StaticText)(StTDS.Specific));
                        ostds.Caption = "Total TDS";


                        SAPbouiCOM.Item EtTDS = oform.Items.Add("ET_TDS", SAPbouiCOM.BoFormItemTypes.it_EDIT); // we are going to create
                        SAPbouiCOM.Item orsc1 = oform.Items.Item("52");
                        EtTDS.Top = StTDS.Top;
                        EtTDS.Height = orsc1.Height;
                        EtTDS.Width = orsc1.Width;
                        EtTDS.Left = orsc1.Left;

                        //specific property for edit text.
                        SAPbouiCOM.EditText oedtds = (SAPbouiCOM.EditText)(EtTDS.Specific);
                        oedtds.Item.Enabled = false; // to disABLE A FIELD.
                        oedtds.DataBind.SetBound(true, "OVPM", "U_TTDSAMT"); //TO SAVE THE VALUE IN TABLE


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
                        oedvds.DataBind.SetBound(true, "OVPM", "U_TVDSAMT"); //TO SAVE THE VALUE IN TABLE



                        SAPbouiCOM.DBDataSource ds = oform.DataSources.DBDataSources.Item("OVPM");
                        docEntry = ds.GetValue("DocEntry", 0).Trim();

                    }

                    if (pVal.EventType == SAPbouiCOM.BoEventTypes.et_CLICK && pVal.ItemUID == "20" && pVal.ColUID == "10000127" && pVal.BeforeAction == false)
                    {
                        try
                        {
                            CalculateMatrixTDSVDS(oform);
                        }
                        catch (Exception ex)
                        {
                            System.Windows.Forms.MessageBox.Show("Error: " + ex.Message);
                        }
                    }
                    if (pVal.EventType == SAPbouiCOM.BoEventTypes.et_LOST_FOCUS && pVal.ItemUID == "20" && pVal.ColUID == "24" && pVal.BeforeAction == false)
                    {
                        // Inside an event handler
                        CalculateMatrixTDSVDS(oform);


                    }






                }


            }
            catch (Exception ex)
            {
                Application.SBO_Application.SetStatusBarMessage("Error in Itemevnt for SAP Screen - " + ex.ToString(), SAPbouiCOM.BoMessageTime.bmt_Medium, true);
            }
        }

        public static (double newTDS, double newVDS) CalculateAdjustedTDSVDS(double originalAmount, double newAmount, double originalTDS, double originalVDS)
        {
            if (originalAmount == 0)
                return (0.0, 0.0); // Avoid division by zero

            if (originalAmount == newAmount)
            {
                return (originalTDS, originalVDS);
            }

            double ratio = newAmount / originalAmount;

            double newTDS = Math.Round(ratio * originalTDS, 2);
            double newVDS = Math.Round(ratio * originalVDS, 2);

            return (newTDS, newVDS);
        }

        public void CalculateMatrixTDSVDS(SAPbouiCOM.Form oForm)
        {
            SAPbouiCOM.Matrix oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item("20").Specific;

            double totalTDS = 0.0;
            double totalVDS = 0.0;

            for (int i = 1; i <= oMatrix.RowCount; i++)
            {
                SAPbouiCOM.CheckBox chk = (SAPbouiCOM.CheckBox)oMatrix.Columns.Item("10000127").Cells.Item(i).Specific;
                if (chk.Checked)
                {
                    SAPbouiCOM.EditText txtDoc = (SAPbouiCOM.EditText)oMatrix.Columns.Item("1").Cells.Item(i).Specific;
                    string docnum = txtDoc.Value.Trim();

                    SAPbouiCOM.EditText txtdoctype = (SAPbouiCOM.EditText)oMatrix.Columns.Item("45").Cells.Item(i).Specific;
                    string doctype = txtdoctype.Value.Trim();

                    SAPbouiCOM.EditText txttotal = (SAPbouiCOM.EditText)oMatrix.Columns.Item("7").Cells.Item(i).Specific;
                    string atotal = txttotal.Value.Trim();
                    string total = atotal.StartsWith("BDT") ? atotal.Substring(4).Trim() : atotal;
                    double amt = double.TryParse(total, out double parsedAmt) ? parsedAmt : 0;

                    SAPbouiCOM.EditText txtnewtotal = (SAPbouiCOM.EditText)oMatrix.Columns.Item("24").Cells.Item(i).Specific;
                    string anewtotal = txtnewtotal.Value.Trim();
                    string newtotal = anewtotal.StartsWith("BDT") ? anewtotal.Substring(4).Trim() : anewtotal;
                    double newamt = double.TryParse(newtotal, out double parsednewAmt) ? parsednewAmt : 0;

                    SAPbouiCOM.EditText txtCardCode = (SAPbouiCOM.EditText)oForm.Items.Item("5").Specific;
                    string code = txtCardCode.Value.Trim();

                    SAPbouiCOM.ComboBox compi = (SAPbouiCOM.ComboBox)oForm.Items.Item("87").Specific;
                    string period = compi.Selected.Description;
                    string numberPart = period.Substring(2); // e.g. "2425"
                    string part1 = numberPart.Substring(0, 2); // "24"
                    string part2 = numberPart.Substring(2, 2); // "25"
                    string result = part1 + "-" + part2;

                    string query = "";
                    SAPbobsCOM.Recordset oRS = (SAPbobsCOM.Recordset)Global.oComp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                    if (doctype == "18")
                    {
                        query = @"SELECT ""U_TTDSAMT"", ""U_TVDSAMT"" FROM ""OPCH"" 
                          WHERE ""DocNum"" = '" + docnum + @"' 
                            AND ""CardCode"" = '" + code + @"' 
                            AND ""DocTotal"" = '" + amt + @"' 
                            AND ""PIndicator"" = '" + result + @"'";
                    }
                    else if (doctype == "204")
                    {
                        query = @"SELECT ""U_TTDSAMT"", ""U_TVDSAMT"" FROM ""ODPO"" 
                          WHERE ""DocNum"" = '" + docnum + @"' 
                            AND ""CardCode"" = '" + code + @"' 
                            AND ""DocTotal"" = '" + amt + @"' 
                            AND ""PIndicator"" = '" + result + @"'";
                    }

                    oRS.DoQuery(query);

                    if (!oRS.EoF)
                    {
                        double tdsAmt = Convert.ToDouble(oRS.Fields.Item("U_TTDSAMT").Value);
                        double vdsAmt = Convert.ToDouble(oRS.Fields.Item("U_TVDSAMT").Value);

                        var (tds, vds) = CalculateAdjustedTDSVDS(amt, newamt, tdsAmt, vdsAmt);

                        totalTDS += tds;
                        totalVDS += vds;
                    }
                }
            }

            // Set totals to EditTexts
            SAPbouiCOM.EditText etTDS = (SAPbouiCOM.EditText)oForm.Items.Item("ET_TDS").Specific;
            SAPbouiCOM.EditText etVDS = (SAPbouiCOM.EditText)oForm.Items.Item("ET_VDS").Specific;

            etTDS.Value = totalTDS.ToString("F2");
            etVDS.Value = totalVDS.ToString("F2");
        }




    }
}
