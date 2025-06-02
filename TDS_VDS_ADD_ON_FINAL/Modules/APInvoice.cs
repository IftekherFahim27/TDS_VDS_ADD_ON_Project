using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbouiCOM.Framework;
using TDS_VDS_ADD_ON_FINAL.Helper;

namespace TDS_VDS_ADD_ON_FINAL.Modules
{
    class APInvoice
    {
        public APInvoice()
        {
            Application.SBO_Application.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
        }

        public string formnum = "141";
        public string spos = "34";
        public string epos = "33";
        public string db = "OPCH";
        private void SBO_Application_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            FormModifier.SBO_Application_ItemEvent(FormUID, ref pVal, out BubbleEvent,
                                                      formnum, spos, epos, db);
        }
    }
}
