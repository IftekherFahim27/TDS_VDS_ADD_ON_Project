using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbouiCOM.Framework;
using TDS_VDS_ADD_ON_FINAL.Helper;

namespace TDS_VDS_ADD_ON_FINAL.Modules
{
    class PurchaseOrder
    {
        public PurchaseOrder()
        {
            Application.SBO_Application.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
        }

        public string formnum = "142";
        public string spos = "30";
        public string epos = "29";
        public string db = "OPOR";

        private void SBO_Application_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            FormModifier.SBO_Application_ItemEvent(FormUID, ref pVal, out BubbleEvent,
                                                      formnum, spos, epos, db);

        }


    }
}
