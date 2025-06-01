using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDS_VDS_ADD_ON_FINAL.Helper;

namespace TDS_VDS_ADD_ON_FINAL.Resources
{
    [FormAttribute("TDS_VDS_ADD_ON_FINAL.Resources.FormTVMaster", "Resources/FormTVMaster.b1f")]
    class FormTVMaster : UserFormBase
    {
        public FormTVMaster()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.STTXCODE = ((SAPbouiCOM.StaticText)(this.GetItem("STTXCODE").Specific));
            this.STTXDESC = ((SAPbouiCOM.StaticText)(this.GetItem("STTXDESC").Specific));
            this.STGLACCT = ((SAPbouiCOM.StaticText)(this.GetItem("STGLACCT").Specific));
            this.STGLDESC = ((SAPbouiCOM.StaticText)(this.GetItem("STGLDESC").Specific));
            this.STEFDATE = ((SAPbouiCOM.StaticText)(this.GetItem("STEFDATE").Specific));
            this.STETDATE = ((SAPbouiCOM.StaticText)(this.GetItem("STETDATE").Specific));
            this.STRATE = ((SAPbouiCOM.StaticText)(this.GetItem("STRATE").Specific));
            this.STACTIVE = ((SAPbouiCOM.StaticText)(this.GetItem("STACTIVE").Specific));
            this.STREMARK = ((SAPbouiCOM.StaticText)(this.GetItem("STREMARK").Specific));
            this.ETTXCODE = ((SAPbouiCOM.EditText)(this.GetItem("ETTXCODE").Specific));
            this.ETTXDESC = ((SAPbouiCOM.EditText)(this.GetItem("ETTXDESC").Specific));
            this.ETGLACCT = ((SAPbouiCOM.EditText)(this.GetItem("ETGLACCT").Specific));
            this.ETGLACCT.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.ETGLACCT_ChooseFromListAfter);
            this.ETGLDESC = ((SAPbouiCOM.EditText)(this.GetItem("ETGLDESC").Specific));

            this.ETEFDATE = ((SAPbouiCOM.EditText)(this.GetItem("ETEFDATE").Specific));
            this.ETETDATE = ((SAPbouiCOM.EditText)(this.GetItem("ETETDATE").Specific));
            this.ETRATE = ((SAPbouiCOM.EditText)(this.GetItem("ETRATE").Specific));
            this.ETREMARK = ((SAPbouiCOM.EditText)(this.GetItem("ETREMARK").Specific));
            this.CHKACTVE = ((SAPbouiCOM.CheckBox)(this.GetItem("CHKACTVE").Specific));
            this.ADD_Button = ((SAPbouiCOM.Button)(this.GetItem("1").Specific));
            this.ADD_Button.PressedBefore += new SAPbouiCOM._IButtonEvents_PressedBeforeEventHandler(this.ADD_Button_PressedBefore);
            this.Cancel_Button = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.ETDOCTRY = ((SAPbouiCOM.EditText)(this.GetItem("ETDOCTRY").Specific));
            this.STWHTYPE = ((SAPbouiCOM.StaticText)(this.GetItem("STWHTYPE").Specific));
            this.CBWHTYPE = ((SAPbouiCOM.ComboBox)(this.GetItem("CBWHTYPE").Specific));
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
        }

       

        private void OnCustomInitialize()
        {
            this.ETGLDESC.Item.Enabled = false;
        }

        private SAPbouiCOM.StaticText STTXCODE;
        private SAPbouiCOM.StaticText STTXDESC;
        private SAPbouiCOM.StaticText STGLACCT;
        private SAPbouiCOM.StaticText STGLDESC;
        private SAPbouiCOM.StaticText STEFDATE;
        private SAPbouiCOM.StaticText STETDATE;
        private SAPbouiCOM.StaticText STRATE;
        private SAPbouiCOM.StaticText STACTIVE;
        private SAPbouiCOM.StaticText STREMARK;
        private SAPbouiCOM.EditText ETTXCODE;
        private SAPbouiCOM.EditText ETTXDESC;
        private SAPbouiCOM.EditText ETGLACCT;
        private SAPbouiCOM.EditText ETGLDESC;
        private SAPbouiCOM.EditText ETEFDATE;
        private SAPbouiCOM.EditText ETETDATE;
        private SAPbouiCOM.EditText ETRATE;
        private SAPbouiCOM.EditText ETREMARK;
        private SAPbouiCOM.CheckBox CHKACTVE;
        private SAPbouiCOM.Button ADD_Button;
        private SAPbouiCOM.Button Cancel_Button;
        private SAPbouiCOM.EditText ETDOCTRY;
        private SAPbouiCOM.StaticText STWHTYPE;
        private SAPbouiCOM.ComboBox CBWHTYPE;

        private void ADD_Button_PressedBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
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
            string Code = pForm.DataSources.DBDataSources.Item("@FIL_MH_TVM").GetValue("Code", 0);
            string Desc = pForm.DataSources.DBDataSources.Item("@FIL_MH_TVM").GetValue("Name", 0);
            string GLAcct = pForm.DataSources.DBDataSources.Item("@FIL_MH_TVM").GetValue("U_ACCTCODE", 0);
            string GLDesc = pForm.DataSources.DBDataSources.Item("@FIL_MH_TVM").GetValue("U_ACCTNAME", 0);
            string efd = pForm.DataSources.DBDataSources.Item("@FIL_MH_TVM").GetValue("U_EFRMDATE", 0);
            string etd = pForm.DataSources.DBDataSources.Item("@FIL_MH_TVM").GetValue("U_ETODATE", 0);
            string rate = pForm.DataSources.DBDataSources.Item("@FIL_MH_TVM").GetValue("U_RATE", 0);
            string remarks = pForm.DataSources.DBDataSources.Item("@FIL_MH_TVM").GetValue("U_REMARKS", 0);
            string whldtype = pForm.DataSources.DBDataSources.Item("@FIL_MH_TVM").GetValue("U_WHLDTYPE", 0);
          

            if (Code == "")
            {
                Global.GFunc.ShowError("Enter Tax Code");
                pForm.ActiveItem = "ETTXCODE";
                return BubbleEvent = false;
            }
            else if (Desc == "")
            {
                Global.GFunc.ShowError("Enter Tax Description");
                pForm.ActiveItem = "ETTXDESC";
                return BubbleEvent = false;
            }
            else if (GLAcct == "")
            {
                Global.GFunc.ShowError("Enter GL Account");
                pForm.ActiveItem = "ETGLACCT";
                return BubbleEvent = false;
            }
            else if (GLDesc == "")
            {
                Global.GFunc.ShowError("Enter GL Name");
                pForm.ActiveItem = "ETTXDESC";
                return BubbleEvent = false;
            }
            else if (efd == "")
            {
                Global.GFunc.ShowError("Enter Effective From Date ");
                pForm.ActiveItem = "ETEFDATE";
                return BubbleEvent = false;
            }
            else if (etd == "")
            {
                Global.GFunc.ShowError("Enter Effective To Date");
                pForm.ActiveItem = "ETETDATE";
                return BubbleEvent = false;
            }
            else if (rate == "")
            {
                Global.GFunc.ShowError("Enter Section ");
                pForm.ActiveItem = "ETRATE";
                return BubbleEvent = false;
            }
            else if (whldtype == "")
            {
                Global.GFunc.ShowError("Select The With Hold Type");
                pForm.ActiveItem = "CBWHTYPE";
                return BubbleEvent = false;
            }

            return BubbleEvent;
        }

        private void ETGLACCT_ChooseFromListAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                SAPbouiCOM.ISBOChooseFromListEventArg cflArg = (SAPbouiCOM.ISBOChooseFromListEventArg)pVal;

                SAPbouiCOM.DataTable dt = cflArg.SelectedObjects;
                if (dt == null || dt.Rows.Count == 0)
                    return;

                string acctCode = dt.GetValue("AcctCode", 0).ToString();
                string acctName = dt.GetValue("CardName", 0).ToString();

                ETGLACCT.Value = acctCode;
                ETGLDESC.Value = acctName;

                

               
            }
            catch (Exception e)
            {
                Application.SBO_Application.MessageBox("Error in ChooseFromListAfter: " + e.Message);
            }
        }
    }
}
