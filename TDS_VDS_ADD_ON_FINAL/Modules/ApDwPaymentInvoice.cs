using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDS_VDS_ADD_ON_FINAL.Helper;
using SAPbouiCOM.Framework;

namespace TDS_VDS_ADD_ON_FINAL.Modules
{
    class ApDwPaymentInvoice
    {
        public ApDwPaymentInvoice()
        {
            Application.SBO_Application.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
        }

        public string formnum = "65301";
        public string spos = "34";
        public string epos = "33";
        public string db = "ODPO";

        private void SBO_Application_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            FormModifier.SBO_Application_ItemEvent(FormUID, ref pVal, out BubbleEvent,
                                                      formnum, spos, epos, db);


        }
    }
}
