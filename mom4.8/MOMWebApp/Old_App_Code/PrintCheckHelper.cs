using System;
using Microsoft.Office.Interop.Word;
using BusinessEntity;
using System.Globalization;
using System.Data;

/// <summary>
/// Summary description for PrintCheckHelper
/// </summary>
public static class PrintCheckHelper
{
    public static string WordCheck(GenerateCheck _objCheck)
    {
        try
        {
            Microsoft.Office.Interop.Word.Application winword = new Microsoft.Office.Interop.Word.Application();

            //Set status for word application is to be visible or not.
            winword.Visible = false;

            //Create a missing variable for missing value
            object missing = System.Reflection.Missing.Value;

            //Create a new document
            Microsoft.Office.Interop.Word.Document document = winword.Documents.Add(ref missing, ref missing, ref missing, ref missing);

            Microsoft.Office.Interop.Word.Paragraph para6 = document.Content.Paragraphs.Add(ref missing);
            object styleHeading5 = "Heading 1";
            para6.Range.set_Style(ref styleHeading5);
            para6.Range.Text = _objCheck.CheckDate.ToString("MMMM dd, yyyy"); //MM/dd/yyyy
            para6.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
            para6.Range.InsertParagraphAfter();

            //Add paragraph with Heading 1 style
            Microsoft.Office.Interop.Word.Paragraph para1 = document.Content.Paragraphs.Add(ref missing);
            object styleHeading1 = "Heading 1";
            para1.Range.set_Style(ref styleHeading1);
            para1.Range.Text = _objCheck.VendorName + "                                                                                    " + _objCheck.VendorName;
            para1.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;

            para1.Range.InsertParagraphAfter();

            Microsoft.Office.Interop.Word.Paragraph para2 = document.Content.Paragraphs.Add(ref missing);
            para2.Range.Text = _objCheck.TotalAmountWords;//"***one thousand five hundred and xx / 100****************";
            para2.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            para2.Range.InsertParagraphAfter();

            //Add paragraph with Heading 1 style
            Microsoft.Office.Interop.Word.Paragraph para3 = document.Content.Paragraphs.Add(ref missing);
            object styleHeading2 = "Heading 1";
            para3.Range.set_Style(ref styleHeading2);
            para3.Range.Text = _objCheck.VendorName;//"Middleton, Ian";
            para3.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            para3.Range.InsertParagraphAfter();

            Microsoft.Office.Interop.Word.Paragraph para4 = document.Content.Paragraphs.Add(ref missing);
            para4.Range.Text = "";//"1813 Kristy Ct";
            para4.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            para4.Range.InsertParagraphAfter();

            Microsoft.Office.Interop.Word.Paragraph para5 = document.Content.Paragraphs.Add(ref missing);
            para5.Range.Text = "";// "LONGMONT, CO 80504";
            para5.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            para5.Range.InsertParagraphAfter();

            Microsoft.Office.Interop.Word.Table firstTable = document.Tables.Add(para1.Range, 2, 4, ref missing, ref missing);
            firstTable.Borders.Enable = 1;
            foreach (Microsoft.Office.Interop.Word.Row row in firstTable.Rows)
            {
                foreach (Cell cell in row.Cells)
                {
                    //other format properties goes here
                    cell.Range.Font.Name = "verdana";
                    cell.Range.Font.Size = 10;

                    //Center alignment for the Header cells
                    cell.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                    cell.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    if (cell.RowIndex == 1)
                    {
                        switch (cell.ColumnIndex)
                        {
                            case 1:
                                cell.Range.Text = "Payee";
                                cell.Range.Font.Bold = 1;
                                //cell.Range.Font.ColorIndex = WdColorIndex.wdGray25;                            
                                cell.Shading.BackgroundPatternColor = WdColor.wdColorGray25;
                                break;
                            case 2:
                                cell.Range.Text = _objCheck.VendorName; //"Middleton, Ian";

                                break;
                            case 3:
                                cell.Range.Text = "Date";
                                cell.Range.Font.Bold = 1;
                                //cell.Range.Font.ColorIndex = WdColorIndex.wdGray25;                            
                                cell.Shading.BackgroundPatternColor = WdColor.wdColorGray25;
                                break;
                            case 4:
                                cell.Range.Text = _objCheck.CheckDate.ToString("MM/dd/yyyy");//"09/26/2015";

                                break;
                        }
                    }
                    else
                    {
                        switch (cell.ColumnIndex)
                        {
                            case 1:
                                cell.Range.Text = "Account #";
                                cell.Range.Font.Bold = 1;
                                //cell.Range.Font.ColorIndex = WdColorIndex.wdGray25;                            
                                cell.Shading.BackgroundPatternColor = WdColor.wdColorGray25;
                                break;
                            case 2:
                                cell.Range.Text = "";
                                break;
                            case 3:
                                cell.Range.Text = "Amount";
                                cell.Range.Font.Bold = 1;
                                //cell.Range.Font.ColorIndex = WdColorIndex.wdGray25;                            
                                cell.Shading.BackgroundPatternColor = WdColor.wdColorGray25;
                                break;
                            case 4:
                                cell.Range.Text = _objCheck.TotalAmount.ToString("0.00", CultureInfo.InvariantCulture);//"1,500.00";
                                break;
                        }
                    }
                }
            }
            para1.Range.InsertParagraphAfter();
            //Create a 5X5 table and insert some dummy record
            int _count = _objCheck.dtOpenAP.Rows.Count + 1;
            Microsoft.Office.Interop.Word.Table secondTable = document.Tables.Add(para2.Range, _count, 7, ref missing, ref missing);

            secondTable.Borders.Enable = 1;
            int i = 0;
            //DataRow _dr = _objCheck.dtOpenAP.Rows[0];
            foreach (Microsoft.Office.Interop.Word.Row row in secondTable.Rows)
            {
                //foreach(DataRow _dr i
                //if(i < _objCheck.dtOpenAP.Rows.Count)
                //{
                    //for (int j = 0; j < _objCheck.dtOpenAP.Rows.Count; j++)
                    //{
                        DataRow _dr = _objCheck.dtOpenAP.Rows[i];
                        foreach (Cell cell in row.Cells)
                        {
                            //Header row
                            if (cell.RowIndex == 1)
                            {
                                switch (cell.ColumnIndex)
                                {
                                    case 1:
                                        cell.Range.Text = "Date";

                                        break;
                                    //case 2:
                                    //    cell.Range.Text = "Type";
                                    //    break;
                                    case 2:
                                        cell.Range.Text = "Ref";
                                        break;
                                    case 3:
                                        cell.Range.Text = "Description";
                                        break;
                                    case 4:
                                        cell.Range.Text = "Original";
                                        break;
                                    case 5:
                                        cell.Range.Text = "Balance";
                                        break;
                                    case 6:
                                        cell.Range.Text = "Disc";
                                        break;
                                    case 7:
                                        cell.Range.Text = "Paid";
                                        break;
                                }
                                cell.Range.Font.Bold = 1;
                                //other format properties goes here
                                cell.Range.Font.Name = "verdana";
                                cell.Range.Font.Size = 10;
                                //cell.Range.Font.ColorIndex = WdColorIndex.wdGray25;                            
                                cell.Shading.BackgroundPatternColor = WdColor.wdColorGray25;
                                //Center alignment for the Header cells
                                cell.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                                cell.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                            }
                            //Data row
                            else
                            {
                               
                                switch (cell.ColumnIndex)
                                {
                                    case 1:
                                        cell.Range.Text = Convert.ToDateTime(_dr["Date"]).ToString("MM/dd/yyyy"); //"10/28/2008";
                                        break;
                                    //case 2:
                                    //    cell.Range.Text = "Other";
                                    //    break;
                                    case 2:
                                        cell.Range.Text = _dr["Ref"].ToString(); //"Rent";
                                        break;
                                    case 3:
                                        cell.Range.Text = _dr["Description"].ToString();//"Invoice";
                                        break;
                                    case 4:
                                        cell.Range.Text = Convert.ToDouble(_dr["Original"]).ToString("0.00", CultureInfo.InvariantCulture); //"$1,500.00";
                                        break;
                                    case 5:
                                        cell.Range.Text = Convert.ToDouble(_dr["Balance"]).ToString("0.00", CultureInfo.InvariantCulture); //"$1,500.00";
                                        break;
                                    case 6:
                                        cell.Range.Text = Convert.ToDouble(_dr["Disc"]).ToString("0.00", CultureInfo.InvariantCulture); //"$0.00";
                                        break;
                                    case 7:
                                        cell.Range.Text = Convert.ToDouble(_dr["Paid"]).ToString("0.00", CultureInfo.InvariantCulture); //"$1,500.00";
                                        break;
                                }
                            }
                        }
                  
                        if (row.Index > 1)
                            i++;
                
            }
            

            //Save the document
           
            string _filename = _objCheck.CheckNum.ToString() + "_" + Guid.NewGuid(); //_"+_objCheck.VendorName+"
            string serverPath = System.Web.HttpContext.Current.Server.MapPath("PrintChecks/" + _filename);
            object filename = @serverPath;
            document.SaveAs(ref filename);
            
            //OpenWord(filename); //Open file

            //object objFalse = false;
            //object objTrue = true;
            //object missing1 = System.Reflection.Missing.Value;
            //object emptyData = string.Empty;
            //ApplicationClass wordApp = new Microsoft.Office.Interop.Word.ApplicationClass();

            //string fName = filename.ToString() + ".docx";
            //wordApp.Visible = true;
            //Microsoft.Office.Interop.Word.Document aDoc = wordApp.Documents.Open(fName, ref objFalse, ref objFalse,
            //        ref objFalse, ref missing1, ref missing1, ref missing1, ref missing1, ref missing1, ref missing1, ref missing1, ref objTrue,
            //        ref missing1, ref missing1, ref missing1);
            //aDoc.Activate();

            //object fileName = wordFileName;
            
            document.Close(ref missing, ref missing, ref missing);
            document = null;
            winword.Quit(ref missing, ref missing, ref missing);
            winword = null;

            //object filename = @"D:\temp1.docx"; 
            //document.Open(filename); 
            //MessageBox.Show("Document created successfully !");
            //System.IO.StringWriter stringWriter = new System.IO.StringWriter();
            //stringWriter = document.ToString();

            return serverPath;
        }
        catch (Exception ex)
        {

            //MessageBox.Show("Error occurred while executing code : " + ex.Message);
        }
        //finally
        //{
        //    //you can dispose object here
        //}
        return "";
    }

    public static void OpenWord(object wordFileName)
    {
        object fileName = wordFileName;
        object objFalse = false;
        object objTrue = true;
        object missing = System.Reflection.Missing.Value;
        object emptyData = string.Empty;
        ApplicationClass wordApp = new Microsoft.Office.Interop.Word.ApplicationClass();

        wordApp.Visible = true;
        Microsoft.Office.Interop.Word.Document aDoc = wordApp.Documents.Open(ref fileName, ref objFalse, ref objFalse,
                ref objFalse, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref objTrue,
                ref missing, ref missing, ref missing);
        aDoc.Activate();

    }

}