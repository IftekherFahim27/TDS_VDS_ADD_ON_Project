using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDS_VDS_ADD_ON_FINAL.Helper
{
    class Global
    {
        public static SAPbouiCOM.Application G_UI_Application;
        public static SAPbobsCOM.Company oComp; // Varible for company 
        public static GlobalFunction GFunc = new GlobalFunction();
    }
}
