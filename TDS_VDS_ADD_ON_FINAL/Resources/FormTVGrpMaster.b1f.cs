using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDS_VDS_ADD_ON_FINAL.Helper;

namespace TDS_VDS_ADD_ON_FINAL.Resources
{
    [FormAttribute("TDS_VDS_ADD_ON_FINAL.Resources.FormTVGrpMaster", "Resources/FormTVGrpMaster.b1f")]
    class FormTVGrpMaster : UserFormBase
    {
        public FormTVGrpMaster()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.STGRCODE = ((SAPbouiCOM.StaticText)(this.GetItem("STGRCODE").Specific));
            this.STGPDESC = ((SAPbouiCOM.StaticText)(this.GetItem("STGPDESC").Specific));
            this.STACTIVE = ((SAPbouiCOM.StaticText)(this.GetItem("STACTIVE").Specific));
            this.CBACTIVE = ((SAPbouiCOM.CheckBox)(this.GetItem("CBACTIVE").Specific));
            this.ETGPCODE = ((SAPbouiCOM.EditText)(this.GetItem("ETGPCODE").Specific));
            this.ETGPDESC = ((SAPbouiCOM.EditText)(this.GetItem("ETGPDESC").Specific));
            this.MATGRPRW = ((SAPbouiCOM.Matrix)(this.GetItem("MATGRPRW").Specific));
            this.MATGRPRW.LostFocusAfter += new SAPbouiCOM._IMatrixEvents_LostFocusAfterEventHandler(this.MATGRPRW_LostFocusAfter);
            this.MATGRPRW.ChooseFromListAfter += new SAPbouiCOM._IMatrixEvents_ChooseFromListAfterEventHandler(this.MATGRPRW_ChooseFromListAfter);
            this.ETDOCTRY = ((SAPbouiCOM.EditText)(this.GetItem("ETDOCTRY").Specific));
            this.AddButton = ((SAPbouiCOM.Button)(this.GetItem("1").Specific));
            this.AddButton.PressedBefore += new SAPbouiCOM._IButtonEvents_PressedBeforeEventHandler(this.AddButton_PressedBefore);
            this.CancelButton = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
        }

        private SAPbouiCOM.StaticText STGRCODE;

        private void OnCustomInitialize()
        {

        }

        private SAPbouiCOM.StaticText STGPDESC;
        private SAPbouiCOM.StaticText STACTIVE;
        private SAPbouiCOM.CheckBox CBACTIVE;
        private SAPbouiCOM.EditText ETGPCODE;
        private SAPbouiCOM.EditText ETGPDESC;
        private SAPbouiCOM.Matrix MATGRPRW;
        private SAPbouiCOM.EditText ETDOCTRY;
        private SAPbouiCOM.Button AddButton;
        private SAPbouiCOM.Button CancelButton;

        private void MATGRPRW_ChooseFromListAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            if (pVal.ColUID == "COLTCODE") // Ensure CFL is triggered on COLTCODE
            {
                SAPbouiCOM.ISBOChooseFromListEventArg cflArg = (SAPbouiCOM.ISBOChooseFromListEventArg)pVal;
                SAPbouiCOM.DataTable dt = cflArg.SelectedObjects;
                SAPbouiCOM.Form oform = Application.SBO_Application.Forms.Item(pVal.FormUID);
                if (dt != null && dt.Rows.Count > 0)
                {
                    string code = dt.GetValue("Code", 0).ToString();   // CFL selected code
                    string name = dt.GetValue("Name", 0).ToString();   // CFL selected name
                    string wtype = dt.GetValue("U_WHLDTYPE", 0).ToString();   // CFL selected WTYPE


                    SAPbobsCOM.Recordset oRS = (SAPbobsCOM.Recordset)Global.oComp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                    string query = string.Format(@"SELECT ""Descr"" FROM UFD1 WHERE ""TableID"" = '@FIL_MH_TVM' AND ""FieldID"" = '2' AND ""FldValue"" = '{0}'", wtype);
                    oRS.DoQuery(query);
                    string wt = Convert.ToString(oRS.Fields.Item("Descr").Value);


                    SAPbouiCOM.Matrix oMatrix = (SAPbouiCOM.Matrix)this.GetItem("MATGRPRW").Specific;

                    // Set code (already done by system, but we ensure it's in the correct row)
                    SAPbouiCOM.EditText txtCode = (SAPbouiCOM.EditText)oMatrix.Columns.Item("COLTCODE").Cells.Item(pVal.Row).Specific;
                    txtCode.Value = code;

                    // Set name/description
                    SAPbouiCOM.EditText txtDesc = (SAPbouiCOM.EditText)oMatrix.Columns.Item("COLTDESC").Cells.Item(pVal.Row).Specific;
                    txtDesc.Value = name;

                    SAPbouiCOM.EditText txtwtype = (SAPbouiCOM.EditText)oMatrix.Columns.Item("COLWTYPE").Cells.Item(pVal.Row).Specific;
                    txtwtype.Value = wt;
                   
                    

                }
            }
        }



        private void AddButton_PressedBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            SAPbouiCOM.Form oform = Application.SBO_Application.Forms.Item(pVal.FormUID);
            if (oform.Mode == SAPbouiCOM.BoFormMode.fm_ADD_MODE || oform.Mode == SAPbouiCOM.BoFormMode.fm_UPDATE_MODE)
            {
                ValidateForm(ref oform, ref BubbleEvent);
            }

        }

        private bool ValidateForm(ref SAPbouiCOM.Form pForm, ref bool BubbleEvent)
        {
            string GrpCode = pForm.DataSources.DBDataSources.Item("@FIL_MH_TVGRPM").GetValue("Code", 0);
            string GrpDesc = pForm.DataSources.DBDataSources.Item("@FIL_MH_TVGRPM").GetValue("U_TVGMDESC", 0);
          
            string taxcode = pForm.DataSources.DBDataSources.Item("@FIL_MR_TVGRPM").GetValue("U_TAXCODE", 0);
            string taxDesc = pForm.DataSources.DBDataSources.Item("@FIL_MR_TVGRPM").GetValue("U_TAXDESC", 0);
            string rank = pForm.DataSources.DBDataSources.Item("@FIL_MR_TVGRPM").GetValue("U_RANK", 0);
            string whldtype = pForm.DataSources.DBDataSources.Item("@FIL_MR_TVGRPM").GetValue("U_WHLDTYPE", 0);


            if (GrpCode == "")
            {
                Global.GFunc.ShowError("Enter Group Code");
                pForm.ActiveItem = "ETGPCODE";
                return BubbleEvent = false;
            }
            else if (GrpDesc == "")
            {
                Global.GFunc.ShowError("Enter Group Description");
                pForm.ActiveItem = "ETGPDESC";
                return BubbleEvent = false;
            }
            else if (taxcode == "")
            {
                Global.GFunc.ShowError("Choose the TaxCode");
                pForm.ActiveItem = "COLTCODE";
                return BubbleEvent = false;
            }
            else if (taxDesc == "")
            {
                Global.GFunc.ShowError("Enter GL Name");
                pForm.ActiveItem = "COLTDESC";
                return BubbleEvent = false;
            }

            else if (rank == "")
            {
                Global.GFunc.ShowError("Enter Section ");
                pForm.ActiveItem = "COLRANK";
                return BubbleEvent = false;
            }

            else if (whldtype == "")
            {
                Global.GFunc.ShowError("Select The With Hold Type");
                pForm.ActiveItem = "COLWTYPE";
                return BubbleEvent = false;
            }

            // Preventing Empty Row to Add in the DB
            SAPbouiCOM.DBDataSource oDBDetail = pForm.DataSources.DBDataSources.Item("@FIL_MR_TVGRPM");

            int rowCount = MATGRPRW.VisualRowCount;

            if (rowCount > 0)
            {
                string lasttaxCode = oDBDetail.GetValue("U_TAXCODE", rowCount - 1).Trim();
                string lasttaxName = oDBDetail.GetValue("U_TAXDESC", rowCount - 1).Trim();
                string lastrank = oDBDetail.GetValue("U_RANK", rowCount - 1).Trim();
                string lastwtype = oDBDetail.GetValue("U_WHLDTYPE", rowCount - 1).Trim();

                if (string.IsNullOrEmpty(lasttaxCode) && string.IsNullOrEmpty(lasttaxName) && string.IsNullOrEmpty(lastrank) && string.IsNullOrEmpty(lastwtype))
                {
                    MATGRPRW.DeleteRow(rowCount);
                    oDBDetail.RemoveRecord(rowCount - 1);
                    rowCount--;  // Adjust row count after deletion
                }
            }


            return BubbleEvent;
        }

        private void MATGRPRW_LostFocusAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            if (pVal.ColUID == "COLRANK")
            {
                SAPbouiCOM.Form oform = Application.SBO_Application.Forms.Item(pVal.FormUID);
                SAPbouiCOM.Matrix oMatrix = (SAPbouiCOM.Matrix)oform.Items.Item("MATGRPRW").Specific;

                // Get total rows in the matrix
                int totalRows = oMatrix.RowCount;

                // Proceed only if the row that lost focus is the last row
                if (pVal.Row == totalRows)
                {
                    SAPbouiCOM.EditText txtRank = (SAPbouiCOM.EditText)oMatrix.Columns.Item("COLRANK").Cells.Item(pVal.Row).Specific;
                    string rankValue = txtRank.Value.Trim();

                    if (!string.IsNullOrEmpty(rankValue))
                    {
                        // Add a new line only if the current last row has value
                        SAPbouiCOM.DBDataSource DBDataSourceLine = oform.DataSources.DBDataSources.Item("@FIL_MR_TVGRPM");
                        Global.GFunc.SetNewLine(oMatrix, DBDataSourceLine, 1, "");
                    }
                }
            }
        }



    }
}
