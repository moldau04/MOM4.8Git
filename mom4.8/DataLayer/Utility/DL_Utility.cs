using BusinessEntity.Utility;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace DataLayer.Utility
{

    public class DL_Utility
    {
        int totalRecordcountXlxs = 0;
        double totalTimeXlxs = 0;
        public DataSet SpGet_Core_Session_Data(Core_Session_Data _Core_Session_Data, String ConnectionString)
        {

            try
            {


                SqlParameter[] para = new SqlParameter[5];
                para[0] = new SqlParameter
                {
                    ParameterName = "@User_ID",
                    SqlDbType = SqlDbType.Int,
                    Value = _Core_Session_Data.User_ID
                };

                para[1] = new SqlParameter
                {
                    ParameterName = "@User_Token",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _Core_Session_Data.User_Token
                };

                para[2] = new SqlParameter
                {
                    ParameterName = "@Session_Key",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _Core_Session_Data.Session_Key
                };

                para[3] = new SqlParameter
                {
                    ParameterName = "@Session_Data",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _Core_Session_Data.Session_Data
                };


                return SqlHelper.ExecuteDataset(ConnectionString, "Sp_GetCore_Session_Data", para);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet ADD_Updatet_Core_Session_Data(Core_Session_Data _Core_Session_Data, String ConnectionString)
        {

            try
            {


                SqlParameter[] para = new SqlParameter[5];
                para[0] = new SqlParameter
                {
                    ParameterName = "@User_ID",
                    SqlDbType = SqlDbType.Int,
                    Value = _Core_Session_Data.User_ID
                };

                para[1] = new SqlParameter
                {
                    ParameterName = "@User_Token",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _Core_Session_Data.User_Token
                };

                para[2] = new SqlParameter
                {
                    ParameterName = "@Session_Key",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _Core_Session_Data.Session_Key
                };

                para[3] = new SqlParameter
                {
                    ParameterName = "@Session_Data",
                    SqlDbType = SqlDbType.VarChar,
                    Value = _Core_Session_Data.Session_Data
                };


                return SqlHelper.ExecuteDataset(ConnectionString, "Sp_Core_Session_Data", para);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #region Export excel with large data
        //private int rowsPerSheet = 500;
        private DataTable ResultsData = new DataTable();

        public string ExportToExcel(string ConnectionString, string queryString, string fileName, int rowsPerSheet = 500)
        {
            // Max rowsPerSheet should be 65535 only
            // Excel 2003 supports a maximum of 65,536 rows per worksheet.
            if (rowsPerSheet > 65535) rowsPerSheet = 65535;
            //const string queryString = "select  ID, Job, Loc, fDesc, DescRes from TicketD;";
            try
            {
                //string fileName = @"~/TempPDF/ExportExcel/MyExcel.xlsx";
                using (var connection = new SqlConnection(ConnectionString))
                {
                    var command = new SqlCommand(queryString, connection);
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    int c = 0;
                    bool firstTime = true;

                    //Get the Columns names, types, this will help when we need to format the cells in the excel sheet.
                    DataTable dtSchema = reader.GetSchemaTable();
                    var listCols = new List<DataColumn>();
                    if (dtSchema != null)
                    {
                        foreach (DataRow drow in dtSchema.Rows)
                        {
                            string columnName = Convert.ToString(drow["ColumnName"]);
                            var column = new DataColumn(columnName, (Type)(drow["DataType"]));
                            column.Unique = (bool)drow["IsUnique"];
                            column.AllowDBNull = (bool)drow["AllowDBNull"];
                            column.AutoIncrement = (bool)drow["IsAutoIncrement"];
                            listCols.Add(column);
                            ResultsData.Columns.Add(column);
                        }
                    }

                    // Call Read before accessing data. 
                    while (reader.Read())
                    {
                        DataRow dataRow = ResultsData.NewRow();
                        for (int i = 0; i < listCols.Count; i++)
                        {
                            dataRow[(listCols[i])] = reader[i];
                        }
                        ResultsData.Rows.Add(dataRow);
                        c++;
                        if (c == rowsPerSheet)
                        {
                            c = 0;
                            ExportToOxml(firstTime, fileName);
                            ResultsData.Clear();
                            firstTime = false;
                        }
                    }
                    if (ResultsData != null)
                    {
                        ExportToOxml(firstTime, fileName);
                        ResultsData.Clear();
                    }
                    // Call Close when done reading.
                    reader.Close();
                }

                return fileName;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public string ExportToExcelFromDT(DataTable exportData, string fileName)
        {
            // Max rowsPerSheet should be 65535 only
            // Excel 2003 supports a maximum of 65,536 rows per worksheet.
            //if (rowsPerSheet > 65535) rowsPerSheet = 65535;
            //const string queryString = "select  ID, Job, Loc, fDesc, DescRes from TicketD;";
            try
            {
                //string fileName = @"~/TempPDF/ExportExcel/MyExcel.xlsx";
                //using (var connection = new SqlConnection(ConnectionString))
                //{
                //    var command = new SqlCommand(queryString, connection);
                //    connection.Open();

                //    SqlDataReader reader = command.ExecuteReader();

                //    int c = 0;
                //    bool firstTime = true;

                //    //Get the Columns names, types, this will help when we need to format the cells in the excel sheet.
                //    DataTable dtSchema = reader.GetSchemaTable();
                //    var listCols = new List<DataColumn>();
                //    if (dtSchema != null)
                //    {
                //        foreach (DataRow drow in dtSchema.Rows)
                //        {
                //            string columnName = Convert.ToString(drow["ColumnName"]);
                //            var column = new DataColumn(columnName, (Type)(drow["DataType"]));
                //            column.Unique = (bool)drow["IsUnique"];
                //            column.AllowDBNull = (bool)drow["AllowDBNull"];
                //            column.AutoIncrement = (bool)drow["IsAutoIncrement"];
                //            listCols.Add(column);
                //            ResultsData.Columns.Add(column);
                //        }
                //    }

                //    // Call Read before accessing data. 
                //    while (reader.Read())
                //    {
                //        DataRow dataRow = ResultsData.NewRow();
                //        for (int i = 0; i < listCols.Count; i++)
                //        {
                //            dataRow[(listCols[i])] = reader[i];
                //        }
                //        ResultsData.Rows.Add(dataRow);
                //        c++;
                //        if (c == rowsPerSheet)
                //        {
                //            c = 0;
                //            ExportToOxml(firstTime, fileName);
                //            ResultsData.Clear();
                //            firstTime = false;
                //        }
                //    }
                //    if (ResultsData != null)
                //    {
                //        ExportToOxml(firstTime, fileName);
                //        ResultsData.Clear();
                //    }
                //    // Call Close when done reading.
                //    reader.Close();
                //}

                ResultsData = exportData.Copy();

                if (ResultsData != null)
                {
                    ExportToOxml(true, fileName);
                    ResultsData.Clear();
                }

                return fileName;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        private void ExportToOxml(bool firstTime, string fileName)
        {
            //const string fileName = @"F:\Turlock\ExportToExcel\MyExcel.xlsx";

            //Delete the file if it exists. 
            if (firstTime && File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            uint sheetId = 1; //Start at the first sheet in the Excel workbook.

            if (firstTime)
            {
                //This is the first time of creating the excel file and the first sheet.
                // Create a spreadsheet document by supplying the filepath.
                // By default, AutoSave = true, Editable = true, and Type = xlsx.
                SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.
                    Create(fileName, SpreadsheetDocumentType.Workbook);

                // Add a WorkbookPart to the document.
                WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
                workbookpart.Workbook = new Workbook();


                // Add a WorksheetPart to the WorkbookPart.
                var worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
                var sheetData = new SheetData();
                worksheetPart.Worksheet = new Worksheet(sheetData);

                AutoFilter autoFilter1 = new AutoFilter() { Reference = "A1:Z1" };

                worksheetPart.Worksheet.Append(autoFilter1);

                var bold1 = new Bold();
                CellFormat cf = new CellFormat();


                // Add Sheets to the Workbook.
                Sheets sheets;
                sheets = spreadsheetDocument.WorkbookPart.Workbook.
                    AppendChild<Sheets>(new Sheets());

                // Append a new worksheet and associate it with the workbook.
                var sheet = new Sheet()
                {
                    Id = spreadsheetDocument.WorkbookPart.
                        GetIdOfPart(worksheetPart),
                    SheetId = sheetId,
                    Name = "Sheet" + sheetId
                };
                sheets.Append(sheet);

                //Add Header Row.
                var headerRow = new Row();
                foreach (DataColumn column in ResultsData.Columns)
                {
                    var cell = new Cell { DataType = CellValues.String, CellValue = new CellValue(column.ColumnName) };
                    headerRow.AppendChild(cell);
                }
                sheetData.AppendChild(headerRow);

                foreach (DataRow row in ResultsData.Rows)
                {
                    var newRow = new Row();                    
                    foreach (DataColumn col in ResultsData.Columns)
                    {
                        //var cell = new Cell
                        //{
                        //    DataType = CellValues.String,
                        //    CellValue = new CellValue(row[col].ToString())
                        //};

                        var cell = new Cell();
                        //if (col.DataType == typeof(DateTime))
                        //    cell.DataType = CellValues.Date;
                        //else 
                        if (col.DataType == typeof(Decimal)
                            || col.DataType == typeof(Double)
                            || col.DataType == typeof(Int16)
                            || col.DataType == typeof(Int32)
                            || col.DataType == typeof(Int64)
                            || col.DataType == typeof(Single)
                            || col.DataType == typeof(UInt16)
                            || col.DataType == typeof(UInt32)
                            || col.DataType == typeof(UInt64)
                            )
                            cell.DataType = CellValues.Number;
                        else
                            cell.DataType = CellValues.String;

                        cell.CellValue = new CellValue(row[col].ToString());
                        if (col.ColumnName == "Ticket#")
                        {
                            totalRecordcountXlxs++;
                        }
                        if (col.ColumnName == "TotalTime")
                        {
                            totalTimeXlxs += Convert.ToDouble(row[col]);
                        }
                        newRow.AppendChild(cell);
                    }

                    sheetData.AppendChild(newRow);
                }

                //footer Row.
                var footer = new Row();
                foreach (DataColumn column in ResultsData.Columns)
                {
                    if (column.ColumnName == "Ticket#")
                    {
                        var cell = new Cell { DataType = CellValues.String, CellValue = new CellValue("Total :"+ totalRecordcountXlxs) };
                        footer.AppendChild(cell);
                    }
                    else if (column.ColumnName == "TotalTime")
                    {
                        var cell = new Cell { DataType = CellValues.String, CellValue = new CellValue(totalTimeXlxs.ToString()) };
                        footer.AppendChild(cell);
                    }
                    else
                    {
                        var cell = new Cell { DataType = CellValues.String, CellValue = new CellValue("") };
                        footer.AppendChild(cell);
                    }
                }

                sheetData.AppendChild(footer);

                spreadsheetDocument.Close();
            }
            else
            {
                // Open the Excel file that we created before, and start to add sheets to it.
                var spreadsheetDocument = SpreadsheetDocument.Open(fileName, true);

                var workbookpart = spreadsheetDocument.WorkbookPart;
                if (workbookpart.Workbook == null)
                    workbookpart.Workbook = new Workbook();

                var worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
                var sheetData = new SheetData();
                worksheetPart.Worksheet = new Worksheet(sheetData);
                var sheets = spreadsheetDocument.WorkbookPart.Workbook.Sheets;

                if (sheets.Elements<Sheet>().Any())
                {
                    //Set the new sheet id
                    sheetId = sheets.Elements<Sheet>().Max(s => s.SheetId.Value) + 1;
                }
                else
                {
                    sheetId = 1;
                }

                // Append a new worksheet and associate it with the workbook.
                var sheet = new Sheet()
                {
                    Id = spreadsheetDocument.WorkbookPart.
                        GetIdOfPart(worksheetPart),
                    SheetId = sheetId,
                    Name = "Sheet" + sheetId
                };
                sheets.Append(sheet);

                //Add the header row here.
                var headerRow = new Row();

                foreach (DataColumn column in ResultsData.Columns)
                {
                    var cell = new Cell { DataType = CellValues.String, CellValue = new CellValue(column.ColumnName) };
                    headerRow.AppendChild(cell);
                }
                sheetData.AppendChild(headerRow);

                foreach (DataRow row in ResultsData.Rows)
                {
                    var newRow = new Row();

                    foreach (DataColumn col in ResultsData.Columns)
                    {
                        //var cell = new Cell
                        //{
                        //    DataType = CellValues.String,
                        //    CellValue = new CellValue(row[col].ToString())
                        //};
                        var cell = new Cell();
                        //if (col.DataType == typeof(DateTime))
                        //    cell.DataType = CellValues.Date;
                        //else 
                        if (col.DataType == typeof(Decimal)
                            || col.DataType == typeof(Double)
                            || col.DataType == typeof(Int16)
                            || col.DataType == typeof(Int32)
                            || col.DataType == typeof(Int64)
                            || col.DataType == typeof(Single)
                            || col.DataType == typeof(UInt16)
                            || col.DataType == typeof(UInt32)
                            || col.DataType == typeof(UInt64)
                            )
                            cell.DataType = CellValues.Number;
                        else
                            cell.DataType = CellValues.String;

                        cell.CellValue = new CellValue(row[col].ToString());
                        newRow.AppendChild(cell);
                    }

                    sheetData.AppendChild(newRow);
                }

                workbookpart.Workbook.Save();

                // Close the document.
                spreadsheetDocument.Close();
            }
        }
        #endregion
    }
}
