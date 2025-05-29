using TDS_VDS_ADD_ON_FINAL.Resources;
using TDS_VDS_ADD_ON_FINAL.Helper;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TDS_VDS_ADD_ON_FINAL
{
    class Menu
    {
        public void BasicStart()
        {
            CompanyConnection(); //1)Company connection 
            CreateMainMenu("43520", "FIL_MN_TDS_VDS", "TDS VDS ", 15, 2, false);//parent 2 step
            //CreateMainMenu("FIL_MN_TARGET", "FIL_MASTER", "Master", 0, 2, false);
            //String Menu
            CreateMainMenu("FIL_MN_TDS_VDS", "FIL_TDS_VDS", " TDS VDS Master", 0, 1, false);
            CreateMainMenu("FIL_MN_TDS_VDS", "FIL_TVGRPMASTER", "TDS VDS Group Master", 1, 1, false);
            //CreateMainMenu("FIL_MASTER", "FIL_LOCATION", "Location", 1, 1, false);
            //CreateMainMenu("FIL_MASTER", "FIL_LOCEMPMAPPING", "Location Employee Mapping", 2, 1, false);
            ////
            //CreateMainMenu("FIL_MN_TARGET", "FIL_TRANSACTION", "Transaction", 1, 2, false);
            ////String Menu
            //CreateMainMenu("FIL_TRANSACTION", "FIL_AREATARGET", "Area Target", 0, 1, false);
            //CreateMainMenu("FIL_TRANSACTION", "FIL_DEALERTARGET", "Dealer Target", 1, 1, false);
            //CreateMainMenu("SSM", "SSMS", "Setup", 0, 2, false);//parent 2 step
            //CreateMainMenu("SSMS", "VEHM", "Addon Setup", 0, 1, false);  //setup(No UDO)
            //string loggedInUser = Global.ocomp.UserName;
            //int loggedInUser2 = Global.ocomp.UserSignature;
        }


        public void SBO_Application_MenuEvent(ref SAPbouiCOM.MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {

                if (pVal.BeforeAction && pVal.MenuUID == "FIL_TDS_VDS")
                {
                    FormTVMaster activeForm = new FormTVMaster();
                    activeForm.Show();
                    //SAPbouiCOM.Form oform = (SAPbouiCOM.Form)Application.SBO_Application.Forms.Item("FIL_FRM_MH_TDSVDS");
                    //SAPbouiCOM.DBDataSource DBDataSourceLine = (SAPbouiCOM.DBDataSource)oform.DataSources.DBDataSources.Item("@FIL_MH_TVM");


                }
                else if (pVal.BeforeAction && pVal.MenuUID == "FIL_TVGRPMASTER")
                {
                    FormTVGrpMaster activeForm = new FormTVGrpMaster();
                    activeForm.Show();
                    //SAPbouiCOM.Form oform = (SAPbouiCOM.Form)Application.SBO_Application.Forms.Item("FIL_FRM_MH_TVGRP");
                   // SAPbouiCOM.DBDataSource DBDataSourceLine = (SAPbouiCOM.DBDataSource)oform.DataSources.DBDataSources.Item("@FIL_MH_TVGRPM");


                }




            }
            catch (Exception ex)
            {
                Application.SBO_Application.MessageBox(ex.ToString(), 1, "Ok", "", "");
            }
        }

        public bool IsFormOpen(string formUID)
        {
            try
            {
                foreach (SAPbouiCOM.Form form in Application.SBO_Application.Forms)
                {
                    if (form.UniqueID == formUID)
                    {
                        return true; // Form is already open (SAPbouiCOM.Form)Application.SBO_Application.Forms
                    }
                }
            }
            catch (Exception ex)
            {
                Global.G_UI_Application.StatusBar.SetText("Error checking form: " + ex.Message,
                   SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
            return false; // Form is not open
        }

        private void CompanyConnection()
        {

            try
            {
                string sErrorMsg;
                string cookie;
                string connStr;
                // Global.ocomp.
                Global.oComp = new SAPbobsCOM.Company();
                cookie = Global.oComp.GetContextCookie();
                //    Global.oCompany = new SAPbobsCOM.Company();
                //   cookie =Global.oCompany.GetContextCookie();
                connStr = Application.SBO_Application.Company.GetConnectionContext(cookie);
                Global.oComp.SetSboLoginContext(connStr);
                ////   if (Global.CF.IsSAPHANA())
                ////  {
                ////   Global.oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_HANADB;
                //// }
                //// else
                //// {
                //Global.ocomp.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2019;
                // }
                // Global.oCompany.Connect();
                Global.G_UI_Application = Application.SBO_Application;
                Global.oComp = (SAPbobsCOM.Company)Application.SBO_Application.Company.GetDICompany(); // Reassign the ocomp with the session we conencted with sap b1
                                                                                                       // sErrorMsg = Global.oCompany.GetLastErrorDescription();
                Application.SBO_Application.StatusBar.SetText("TDS VDS Add-On Connected Successfully", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            }
            catch
            {
                Application.SBO_Application.MessageBox(Global.oComp.GetLastErrorDescription().ToString(), 1, "OK", "", "");
            }
        }

        public void CreateMainMenu(string ParentMenuID, string MenuID, string MenuName, int Position, int imenutype, bool flgimg) // POP UP- PARENT
        {
            try
            {
                SAPbouiCOM.Menus oMenus = null; // Define a variable to "menus"
                SAPbouiCOM.MenuItem oMenuItem = null; // Define a variable to MenuItem

                oMenus = Application.SBO_Application.Menus;  // Assign a SAP menu

                SAPbouiCOM.MenuCreationParams oCreationPackage = null;   //Define a variable to menu creating parameter
                oCreationPackage = ((SAPbouiCOM.MenuCreationParams)(Application.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams)));
                oMenuItem = Application.SBO_Application.Menus.Item(ParentMenuID); // "43520" moudles'  //assign a Parent menu




                switch (imenutype)
                {
                    case 2:
                        {
                            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_POPUP;
                            break;
                        }
                    case 1:
                        {
                            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                            break;
                        }
                    case 3:
                        {
                            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_SEPERATOR;
                            break;
                        }
                }

                oCreationPackage.UniqueID = MenuID;
                oCreationPackage.String = MenuName;
                oCreationPackage.Enabled = true;
                oCreationPackage.Position = Position;  //postion is integer and it start from 0 value

                //string path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath).ToString();
                string path = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath).ToString();
                //string Img = string.Concat(path, @"\BANKREC1.png");
                //oCreationPackage.Image = Img;
                if (flgimg == true)
                {
                    

                }
                oMenus = oMenuItem.SubMenus;

                try
                {
                    //  If the menu already exists this code will fail
                    oMenus.AddEx(oCreationPackage);
                }
                catch (Exception ex)
                {

                }
            }
            catch
            {

            }
        }


    }
}
