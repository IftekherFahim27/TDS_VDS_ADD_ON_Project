using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDS_VDS_ADD_ON_FINAL.Helper
{
    using SAPbouiCOM;
    class TDSVDSCalculator
    {
        public static void CalculateTotalTDSVDS(Form oForm)
        {
            try
            {
                Matrix oMatrix = (Matrix)oForm.Items.Item("38").Specific;

                double totalTDS = 0.0;
                double totalVDS = 0.0;
                

                for (int i = 1; i <= oMatrix.RowCount; i++)
                {
                    string tdsValStr = ((EditText)oMatrix.Columns.Item("U_TDSAMT").Cells.Item(i).Specific).Value;
                    string vdsValStr = ((EditText)oMatrix.Columns.Item("U_VDSAMT").Cells.Item(i).Specific).Value;
                   

                    if (double.TryParse(tdsValStr, out double tdsRow))
                        totalTDS += tdsRow;

                    if (double.TryParse(vdsValStr, out double vdsRow))
                        totalVDS += vdsRow;

                    
                }

                ((EditText)oForm.Items.Item("ET_TDS").Specific).Value = totalTDS.ToString("F2");
                ((EditText)oForm.Items.Item("ET_VDS").Specific).Value = totalVDS.ToString("F2");
              
            }
            catch (Exception ex)
            {
                Global.G_UI_Application.SetStatusBarMessage("Error in total TDS/VDS calculation: " + ex.Message, BoMessageTime.bmt_Short, true);
            }
        }

        public static (double tdsAmt, double vdsAmt) CalculateTDSVDS(double amount, double tdsPerc, string tdsrnk, double vdsPerc, string vdsrank, string inclu)
        {
            double tdsAmt = 0.0;
            double vdsAmt = 0.0;
            double famt = 0.0;

            if (inclu == "Y")
            {
                if (tdsrnk == "1" && vdsrank == "2")
                {
                    tdsAmt = (amount * tdsPerc) / (100 + tdsPerc);
                    famt = amount - tdsAmt;
                    vdsAmt = famt * vdsPerc / 100;
                 
                }
                else if (tdsrnk == "2" && vdsrank == "1")
                {
                    vdsAmt = (amount * vdsPerc) / ( 100 + vdsPerc );
                    famt = amount - vdsAmt;
                    tdsAmt = famt * tdsPerc / 100;
                   
                }
                else if (tdsrnk == "1" && vdsrank == "1")
                {
                    tdsAmt =   (amount * tdsPerc) / (100 + tdsPerc);
                    vdsAmt =   (amount * vdsPerc) / (100 + vdsPerc);
                   
                }
            }


            if (inclu == "N") { 

                if (tdsrnk == "1" && vdsrank == "2")
                {
                    tdsAmt = amount * tdsPerc / 100;
                    famt = amount - tdsAmt;
                    vdsAmt = famt * vdsPerc / 100;
                   
                }
                else if (tdsrnk == "2" && vdsrank == "1")
                {
                    vdsAmt = amount * vdsPerc / 100;
                    famt = amount - vdsAmt;
                    tdsAmt = famt * tdsPerc / 100;
                   
                }
                else if (tdsrnk == "1" && vdsrank == "1")
                {
                    tdsAmt = amount * tdsPerc / 100;
                    vdsAmt = amount * vdsPerc / 100;
                   
                }

            }

            return (tdsAmt, vdsAmt);
        }
    }
}
