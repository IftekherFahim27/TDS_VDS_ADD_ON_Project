using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDS_VDS_ADD_ON_FINAL.Helper
{
    class GlobalFunction
    {
        public bool setComboBoxValue(SAPbouiCOM.ComboBox oComboBox, string strQry)
        {
            bool flag;
            try
            {

                int count = oComboBox.ValidValues.Count;//0
                if (count > 0)
                {
                    while (true)
                    {
                        if (count <= 0)
                        {
                            break;
                        }
                        oComboBox.ValidValues.Remove(count - 1, SAPbouiCOM.BoSearchKey.psk_Index);
                        count--;
                    }
                }
                //IN VS-CORE DOTNET WE USE DATASET- AS LIKE SAME FUNCTIONALITY, IT WILL USING RECORDSET IN SAP
                SAPbobsCOM.Recordset businessObject = (SAPbobsCOM.Recordset)Global.oComp.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string str = Convert.ToString(oComboBox.ValidValues.Count);
                if (oComboBox.ValidValues.Count == 0)
                {

                    businessObject.DoQuery(strQry); //doquery means- executinga query
                    businessObject.MoveFirst();
                    int num2 = businessObject.RecordCount - 1; //linelevel count 
                    int num = 0;
                    while (true)
                    {
                        int num3 = num2;
                        if (num > num3)
                        {
                            break;
                        }
                        oComboBox.ValidValues.Add(Convert.ToString(businessObject.Fields.Item(0).Value), Convert.ToString(businessObject.Fields.Item(1).Value));

                        businessObject.MoveNext(); // it will move to next cursor to recordset
                        num++;
                    }
                }
                oComboBox.ExpandType = SAPbouiCOM.BoExpandType.et_ValueDescription;
                oComboBox.Item.DisplayDesc = true;
                flag = true;
            }
            catch (Exception exception1)
            {

                Application.SBO_Application.StatusBar.SetText("setComboBoxValue Function Failed:" + exception1.ToString(), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
                flag = true;

            }
            return flag;
        }
        SAPbouiCOM.EditText omatcol;
        SAPbouiCOM.ComboBox omatcolb;
        public void SetNewLine(SAPbouiCOM.Matrix oMatrix, SAPbouiCOM.DBDataSource oDBDSDetail, int RowID = 1, string ColumnUID = "")
        {
            try
            {

                if (ColumnUID != "")
                {
                    omatcolb = (SAPbouiCOM.ComboBox)oMatrix.Columns.Item(ColumnUID).Cells.Item(RowID).Specific;
                }

                if (ColumnUID.Equals(""))  //no column assign ; eventhough no values exist in previous column then also can add new lines.
                {
                    oMatrix.FlushToDataSource();
                    oMatrix.AddRow(1, -1);
                    oDBDSDetail.InsertRecord(oDBDSDetail.Size);
                    oDBDSDetail.Offset = oMatrix.VisualRowCount - 1;
                    oDBDSDetail.SetValue("LineId", oDBDSDetail.Offset, Convert.ToString(oMatrix.VisualRowCount));
                    oMatrix.SetLineData(oMatrix.VisualRowCount);
                    oMatrix.FlushToDataSource();
                }
                else if (oMatrix.VisualRowCount <= 0)  //1st time row creation
                {
                    oMatrix.FlushToDataSource();
                    oMatrix.AddRow(1, -1);//1-1=4
                    oDBDSDetail.InsertRecord(oDBDSDetail.Size);
                    oDBDSDetail.Offset = oMatrix.VisualRowCount - 1; //starting index from 0
                    oDBDSDetail.SetValue("LineId", oDBDSDetail.Offset, Convert.ToString(oMatrix.VisualRowCount));
                    oMatrix.SetLineData(oMatrix.VisualRowCount);
                    oMatrix.FlushToDataSource();
                }
                else if (!(omatcolb.Value).Equals("") && (RowID == oMatrix.VisualRowCount))  // column assigned ; only add a row when present column value is not null.
                {
                    oMatrix.FlushToDataSource();
                    oMatrix.AddRow(1, -1);
                    oDBDSDetail.InsertRecord(oDBDSDetail.Size);
                    oDBDSDetail.Offset = oMatrix.VisualRowCount - 1;
                    oDBDSDetail.SetValue("LineId", oDBDSDetail.Offset, Convert.ToString(oMatrix.VisualRowCount));
                    oMatrix.SetLineData(oMatrix.VisualRowCount);
                    //oMatrix.LoadFromDataSource();
                    oMatrix.FlushToDataSource();
                }
            }
            catch (Exception exception1)
            {

            }
        }
        public void ShowError(string ErrorMessage)
        {
            Application.SBO_Application.StatusBar.SetText(ErrorMessage, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
        }
        public void ShowSuccess(string ErrorMessage)
        {
            Application.SBO_Application.StatusBar.SetText(ErrorMessage, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
        }
        public void AddRow(SAPbouiCOM.Matrix oMatrix, SAPbouiCOM.DBDataSource oDataSource)
        {

            oMatrix.FlushToDataSource();
            oDataSource.InsertRecord(oDataSource.Size);
            if (oDataSource.Size > 1)
                for (int i = oDataSource.Size - 2; i >= 0; i--)
                {
                    if (oDataSource.GetValue("U_CHARCODE", i) == "")
                        oDataSource.RemoveRecord(i);
                    else
                        break;
                }
            for (int i = 0; i < oDataSource.Size; i++)
            {
                oDataSource.SetValue("LineId", i, (i + 1).ToString());
            }
            oMatrix.Clear();
            oMatrix.LoadFromDataSource();
        }
    }
}
