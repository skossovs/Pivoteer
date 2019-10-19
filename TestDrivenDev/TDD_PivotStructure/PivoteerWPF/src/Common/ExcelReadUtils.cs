using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PivoteerWPF.Common
{
    class ExcelReadUtils<T>
    {
        public static IEnumerable<T> RetrieveSheetData(string fullPath, string sheetName, string valueFieldName)
        {
            var result = new List<T>();
            var dataTable = GetDataFromExcel(fullPath, sheetName);
            result = ConvertToList<T>(dataTable, valueFieldName);
            return result;
        }

        public static DataTable GetDataFromExcel(string path, string sheetName)
        {
            DataTable dt = new DataTable();
            //Open the Excel file using ClosedXML.
            using (XLWorkbook workBook = new XLWorkbook(path))
            {
                //Read the first Sheet from Excel file.
                var workSheet = workBook.Worksheets.Where(s => s.Name == sheetName).FirstOrDefault();
                //Create a new DataTable.
                //Loop through the Worksheet rows.
                bool firstRow = true;
                foreach (IXLRow row in workSheet.Rows())
                {
                    //Use the first row to add columns to DataTable.
                    if (firstRow)
                    {
                        foreach (IXLCell cell in row.Cells())
                        {
                            if (!string.IsNullOrEmpty(cell.Value.ToString()))
                                dt.Columns.Add(cell.Value.ToString());
                            else
                                break;
                        }
                        firstRow = false;
                    }
                    else
                    {
                        int i = 0;
                        DataRow toInsert = dt.NewRow();
                        foreach (IXLCell cell in row.Cells(1, dt.Columns.Count))
                        {
                            try
                            {
                                toInsert[i] = cell.Value.ToString();
                            }
                            catch (Exception ex)
                            {
                                // TODO: handle exceptions
                            }
                            i++;
                        }
                        dt.Rows.Add(toInsert);
                    }
                }
                return dt;
            }

        }

        private static List<T> ConvertToList<T>(DataTable dt, string valueFieldName)
        {
            var columnNames = dt.Columns.Cast<DataColumn>()
                .Select(c => c.ColumnName)
                .ToList();

            var fields = typeof(T).GetFields();

            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();

                foreach (var pro in fields)
                {
                    if (columnNames.Contains(pro.Name))
                        if (pro.Name == valueFieldName)
                            pro.SetValue(objT, new Decimal?(Convert.ToDecimal(row[pro.Name] == string.Empty ? null : row[pro.Name])));
                        else
                            pro.SetValue(objT, row[pro.Name]);
                }

                return objT;
            }).ToList();
        }
    }
}
