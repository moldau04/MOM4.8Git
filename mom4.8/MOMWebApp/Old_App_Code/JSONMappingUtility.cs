using BusinessEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

/// <summary>
/// Summary description for JSONMappingUtility
/// </summary>
public static class JSONMappingUtility
{

    public static RequestQuoteVendorJSON RequestQuoteVendorMappingJSON(DataSet dsinv)
    {
        RequestQuoteVendorJSON datapackge = new RequestQuoteVendorJSON();
        RequestQuoteManufacutrerJSON invManufactInfo = new RequestQuoteManufacutrerJSON();
        List<RequestQuoteManufacutrerJSON> lstinvManufactInfo = new List<RequestQuoteManufacutrerJSON>();

        if (dsinv != null)
        {
            if (dsinv.Tables.Count > 0)
            {
                if (dsinv.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsinv.Tables[0].Rows.Count; i++)
                    {
                        invManufactInfo = new RequestQuoteManufacutrerJSON();
                        invManufactInfo.InventoryManufacturerInformationId = dsinv.Tables[0].Rows[i]["ID"] != DBNull.Value ? Convert.ToInt32(dsinv.Tables[0].Rows[i]["ID"]) : 0;
                        invManufactInfo.MPN = dsinv.Tables[0].Rows[i]["MPN"] != DBNull.Value ? Convert.ToString(dsinv.Tables[0].Rows[i]["MPN"]) : string.Empty;
                        invManufactInfo.Manufacturer = dsinv.Tables[0].Rows[i]["ApprovedManufacturer"] != DBNull.Value ? Convert.ToString(dsinv.Tables[0].Rows[i]["ApprovedManufacturer"]) : string.Empty;
                        invManufactInfo.Vendorid = dsinv.Tables[0].Rows[i]["ApprovedVendor"] != DBNull.Value ? Convert.ToInt32(dsinv.Tables[0].Rows[i]["ApprovedVendor"]) : 0;
                        lstinvManufactInfo.Add(invManufactInfo);
                    }

                    datapackge.ManufacturerInfo = lstinvManufactInfo;

                    for (int i = 0; i < dsinv.Tables[1].Rows.Count; i++)
                    {
                        datapackge.Email = dsinv.Tables[1].Rows[i]["Email"] != DBNull.Value ? Convert.ToString(dsinv.Tables[1].Rows[i]["Email"]) : string.Empty;
                    }
                }
            }
        }

        return datapackge;

    }

    public static List<ProjectItemsJSON> ProjectItemsMappingJSON(DataSet dsinv, string sortdir, string sortcol)
    {
        List<ProjectItemsJSON> datapackge = new List<ProjectItemsJSON>();


        if (dsinv != null)
        {
            if (dsinv.Tables.Count > 0)
            {
                if (dsinv.Tables[0].Rows.Count > 0)
                {
                    DataView dv = dsinv.Tables[0].DefaultView;
                    dv.Sort = sortcol + " " + sortdir;
                    DataTable sortedDT = dv.ToTable();
                    for (int i = 0; i < sortedDT.Rows.Count; i++)
                    {
                        ProjectItemsJSON invManufactInfo = new ProjectItemsJSON();
                        
                        invManufactInfo.ID = sortedDT.Rows[i]["ID"] != DBNull.Value ? Convert.ToString(sortedDT.Rows[i]["ID"]) : "0";
                        invManufactInfo.Location = sortedDT.Rows[i]["Tag"] != DBNull.Value ? Convert.ToString(sortedDT.Rows[i]["Tag"]) : string.Empty;
                        invManufactInfo.Description = sortedDT.Rows[i]["fdesc"] != DBNull.Value ? Convert.ToString(sortedDT.Rows[i]["fdesc"]) : string.Empty;
                        invManufactInfo.Status = sortedDT.Rows[i]["Status"] != DBNull.Value ? Convert.ToString(sortedDT.Rows[i]["Status"]) : string.Empty;
                        if (invManufactInfo.Status == "InActive")
                        {
                            invManufactInfo.Statuscolor = "Red";

                        }

                        else
                        {
                            invManufactInfo.Statuscolor = "black";
                        }

                        invManufactInfo.Company = sortedDT.Rows[i]["Company"] != DBNull.Value ? Convert.ToString(sortedDT.Rows[i]["Company"]) : string.Empty;
                        invManufactInfo.DateCreated = sortedDT.Rows[i]["fDate"] != DBNull.Value ? Convert.ToDateTime(sortedDT.Rows[i]["fDate"]).ToString("MM/dd/yy") : string.Empty;
                        invManufactInfo.Hours = sortedDT.Rows[i]["NHour"] != DBNull.Value ? Convert.ToString(sortedDT.Rows[i]["NHour"]) : "0.00";
                        invManufactInfo.TotalOnOrder = sortedDT.Rows[i]["NComm"] != DBNull.Value ? "$" + string.Format("{0:n}", Convert.ToDouble(sortedDT.Rows[i]["NComm"])) : "$0.00";

                        invManufactInfo.TotalBilled = sortedDT.Rows[i]["NRev"] != DBNull.Value ? Convert.ToString(sortedDT.Rows[i]["NRev"]) : "0.00";
                        if (Convert.ToDouble(invManufactInfo.TotalBilled) < 0)
                        {
                            invManufactInfo.TotalBilledcolor = "Red";
                            invManufactInfo.TotalBilledText = string.Format("{0:c}", Convert.ToDouble(invManufactInfo.TotalBilled)).Replace("-", "");
                        }
                        else
                        {
                            invManufactInfo.TotalBilledcolor = "Black";
                            invManufactInfo.TotalBilledText = "$" + string.Format("{0:n}", Convert.ToDouble(invManufactInfo.TotalBilled));
                        }

                        invManufactInfo.LaborExpense = sortedDT.Rows[i]["NLabor"] != DBNull.Value ? Convert.ToString(sortedDT.Rows[i]["NLabor"]) : "0.00";
                        if (Convert.ToDouble(invManufactInfo.LaborExpense) < 0)
                        {
                            invManufactInfo.LaborExpensecolor = "Red";
                            invManufactInfo.LaborExpenseText = string.Format("{0:c}", Convert.ToDouble(invManufactInfo.LaborExpense)).Replace("-", "");
                        }
                        else
                        {
                            invManufactInfo.LaborExpensecolor = "Black";
                            invManufactInfo.LaborExpenseText = "$" + string.Format("{0:n}", Convert.ToDouble(invManufactInfo.LaborExpense));
                        }

                        invManufactInfo.MaterialExpense = sortedDT.Rows[i]["NMat"] != DBNull.Value ? Convert.ToString(sortedDT.Rows[i]["NMat"]) : "0.00";
                        if (Convert.ToDouble(invManufactInfo.MaterialExpense) < 0)
                        {
                            invManufactInfo.MaterialExpensecolor = "Red";
                            invManufactInfo.MaterialExpenseText = string.Format("{0:c}", Convert.ToDouble(invManufactInfo.MaterialExpense)).Replace("-", "");
                        }
                        else
                        {
                            invManufactInfo.MaterialExpensecolor = "Black";
                            invManufactInfo.MaterialExpenseText = "$" + string.Format("{0:n}", Convert.ToDouble(invManufactInfo.MaterialExpense));
                        }

                        invManufactInfo.Expenses = sortedDT.Rows[i]["NOMat"] != DBNull.Value ? Convert.ToString(sortedDT.Rows[i]["NOMat"]) : "0.00";
                        if (Convert.ToDouble(invManufactInfo.Expenses) < 0)
                        {
                            invManufactInfo.Expensecolor = "Red";
                            invManufactInfo.ExpenseText = string.Format("{0:c}", Convert.ToDouble(invManufactInfo.Expenses)).Replace("-", "");
                        }
                        else
                        {
                            invManufactInfo.Expensecolor = "Black";
                            invManufactInfo.ExpenseText = "$" + string.Format("{0:n}", Convert.ToDouble(invManufactInfo.Expenses));
                        }

                        invManufactInfo.TotalExpenses = sortedDT.Rows[i]["NCost"] != DBNull.Value ? Convert.ToString(sortedDT.Rows[i]["NCost"]) : "0.00";
                        if (Convert.ToDouble(invManufactInfo.TotalExpenses) < 0)
                        {
                            invManufactInfo.TotalExpensescolor = "Red";
                            invManufactInfo.TotalExpensesText = string.Format("{0:c}", Convert.ToDouble(invManufactInfo.TotalExpenses)).Replace("-", "");
                        }
                        else
                        {
                            invManufactInfo.TotalExpensescolor = "Black";
                            invManufactInfo.TotalExpensesText = "$" + string.Format("{0:n}", Convert.ToDouble(invManufactInfo.TotalExpenses));
                        }


                        invManufactInfo.Net = sortedDT.Rows[i]["NProfit"] != DBNull.Value ? Convert.ToString(sortedDT.Rows[i]["NProfit"]) : "0.00";

                        invManufactInfo.Profit = sortedDT.Rows[i]["NRatio"] != DBNull.Value ? Convert.ToString(sortedDT.Rows[i]["NRatio"]) : "0.00";
                        if (Convert.ToDouble(invManufactInfo.Net) < 0)
                        {
                            invManufactInfo.Netcolor = "Red";
                            invManufactInfo.NetText = string.Format("{0:c}", Convert.ToDouble(invManufactInfo.Net)).Replace("-", "");
                        }
                        else
                        {
                            invManufactInfo.Netcolor = "Black";
                            invManufactInfo.NetText = "$" + string.Format("{0:n}", Convert.ToDouble(invManufactInfo.Net));
                        }
                        invManufactInfo.Url = sortedDT.Rows[i]["Url"].ToString();
                        invManufactInfo.Customer = sortedDT.Rows[i]["Customer"].ToString();
                        datapackge.Add(invManufactInfo);
                    }

                    ProjectItemsJSON totals = new ProjectItemsJSON();
                    totals.ID = "0";
                    totals.HoursTotal = string.Format("{0:0.00}", Convert.ToDouble(sortedDT.Compute("SUM(NHour)", string.Empty)));
                    totals.TotalBilledTotal = string.Format("{0:c}", Convert.ToDouble(sortedDT.Compute("SUM(NRev)", string.Empty)));
                    totals.TotalExpensesTotal = string.Format("{0:c}", Convert.ToDouble(sortedDT.Compute("SUM(NCost)", string.Empty)));
                    var chkData = Convert.ToDouble(sortedDT.Compute("SUM(NProfit)", string.Empty));
                    if(chkData<0)
                    {
                        totals.NetTotalColor = "Red";
                    }
                    else
                    {
                        totals.NetTotalColor = "Black";
                    }
                    totals.NetTotal = string.Format("{0:c}", Convert.ToDouble(sortedDT.Compute("SUM(NProfit)", string.Empty)));
                   
                    totals.LaborExpenseTotal = string.Format("{0:c}", Convert.ToDouble(sortedDT.Compute("SUM(NLabor)", string.Empty)));
                    totals.TotalExpenses = string.Format("{0:c}", Convert.ToDouble(sortedDT.Compute("SUM(NOMat)", string.Empty)));
                    totals.MaterialExpenseTotal = string.Format("{0:c}", Convert.ToDouble(sortedDT.Compute("SUM(NMat)", string.Empty)));
                    totals.TotalOnOrderTotal = string.Format("{0:c}", Convert.ToDouble(sortedDT.Compute("SUM(NComm)", string.Empty)));
                    if (Convert.ToDouble(sortedDT.Compute("SUM(NRev)", string.Empty)) != 0)
                    {
                        totals.ProfitTotal = string.Format("{0:n}", ((Convert.ToDouble(sortedDT.Compute("SUM(NProfit)", string.Empty)) / Convert.ToDouble(sortedDT.Compute("SUM(NRev)", string.Empty))) * 100));
                    }
                    else
                    {
                        totals.ProfitTotal = string.Format("{0:n}", 0.00);
                    }

                    //lblHourFooter.Text = string.Format("{0:0.00}", dblHours);
                    //lblRevFooter.Text = string.Format("{0:c}", dblBilled);
                    //lblTotalExpFooter.Text = string.Format("{0:c}", dblTotalExp);
                    //lblNetFooter.Text = string.Format("{0:c}", dblNet);
                    //lblTotalLaborFooter.Text = string.Format("{0:c}", dblLabor);
                    //lblExpensesFooter.Text = string.Format("{0:c}", dblExp);
                    //lblPercentFooter.Text = string.Format("{0:n}", dblPercent);
                    //lblTotalMatFooter.Text = string.Format("{0:c}", dblMat);
                    //lblTotalOrderFooter.Text = string.Format("{0:c}", dblOrder);
                    datapackge.Add(totals);
                }
            }
        }

        return datapackge.ToList();

    }

    public static List<VendorItemsJSON> VendorItemsMappingJSON(DataSet dsinv, string sortdir, string sortcol)
    {
        List<VendorItemsJSON> datapackge = new List<VendorItemsJSON>();


        if (dsinv != null)
        {
            if (dsinv.Tables.Count > 0)
            {
                if (dsinv.Tables[0].Rows.Count > 0)
                {
                    DataView dv = dsinv.Tables[0].DefaultView;
                    dv.Sort = sortcol + " " + sortdir;
                    DataTable sortedDT = dv.ToTable();
                    for (int i = 0; i < sortedDT.Rows.Count; i++)
                    {
                        VendorItemsJSON invManufactInfo = new VendorItemsJSON();
                        invManufactInfo.ID = sortedDT.Rows[i]["ID"] != DBNull.Value ? Convert.ToString(sortedDT.Rows[i]["ID"]) : "0";
                        invManufactInfo.Rol = sortedDT.Rows[i]["Rol"] != DBNull.Value ? Convert.ToString(sortedDT.Rows[i]["Rol"]) : "0";
                        invManufactInfo.Acct = sortedDT.Rows[i]["Acct"] != DBNull.Value ? Convert.ToString(sortedDT.Rows[i]["Acct"]) : string.Empty;
                        invManufactInfo.Name = sortedDT.Rows[i]["Name"] != DBNull.Value ? Convert.ToString(sortedDT.Rows[i]["Name"]) : string.Empty;

                        invManufactInfo.Status = sortedDT.Rows[i]["Status"] != DBNull.Value ? Convert.ToString(sortedDT.Rows[i]["Status"]) : string.Empty;
                        if (invManufactInfo.Status == "InActive")
                        {
                            invManufactInfo.Statuscolor = "Red";

                        }
                        else if (invManufactInfo.Status == "Active")
                        {
                            invManufactInfo.Statuscolor = "Green";
                        }
                        else
                        {
                            invManufactInfo.Statuscolor = "black";
                        }

                        
                        invManufactInfo.Company = sortedDT.Rows[i]["Company"] != DBNull.Value ? Convert.ToString(sortedDT.Rows[i]["Company"]) : string.Empty;
                        invManufactInfo.Type = sortedDT.Rows[i]["Type"] != DBNull.Value ? Convert.ToString(sortedDT.Rows[i]["Type"]) : string.Empty;

                        invManufactInfo.Balance = sortedDT.Rows[i]["Balance"] != DBNull.Value ? "$" + Convert.ToString(sortedDT.Rows[i]["Balance"]) : "$0.00";


                        datapackge.Add(invManufactInfo);
                    }

                    VendorItemsJSON totals = new VendorItemsJSON();
                    totals.ID = "0";
                    totals.TotalBalance = string.Format("{0:c}", Convert.ToDouble(sortedDT.Compute("SUM(Balance)", string.Empty)));
                    datapackge.Add(totals);
                }
            }
        }

        return datapackge.ToList();

    }
}

public class ProjectItemsJSON
{
    public string Location { get; set; }
    public string ID { get; set; }
    public string Description { get; set; }

    public string Status { get; set; }  
    public string Statuscolor { get; set; }
    public string Company { get; set; }
    public string DateCreated { get; set; }
    public string Hours { get; set; }
    public string HoursTotal { get; set; }
    public string TotalOnOrder { get; set; }
    public string TotalOnOrderTotal { get; set; }

    public string TotalBilled { get; set; }
    public string TotalBilledTotal { get; set; }
    public string TotalBilledText { get; set; }
    public string TotalBilledcolor { get; set; }
    public string LaborExpense { get; set; }
    public string LaborExpenseTotal { get; set; }
    public string LaborExpenseText { get; set; }
    public string LaborExpensecolor { get; set; }
    public string MaterialExpense { get; set; }
    public string MaterialExpenseTotal { get; set; }
    public string MaterialExpenseText { get; set; }
    public string MaterialExpensecolor { get; set; }
    public string Expenses { get; set; }
    public string ExpensesTotal { get; set; }
    public string ExpenseText { get; set; }
    public string Expensecolor { get; set; }
    public string TotalExpenses { get; set; }
    public string TotalExpensesTotal { get; set; }
    public string TotalExpensesText { get; set; }
    public string TotalExpensescolor { get; set; }
    public string NetText { get; set; }
    public string Net { get; set; }
    public string NetTotal { get; set; }
    public string NetTotalColor { get; set; }
    public string Netcolor { get; set; }

    public string Profit { get; set; }
    public string ProfitTotal { get; set; }
    public string Url { get; set; }
    public string Customer { get; set; }





}

public class VendorItemsJSON
{

    public string ID { get; set; }
    public string Rol { get; set; }
    public string Acct { get; set; }

    public string Status { get; set; }
    public string Statuscolor { get; set; }

    public string Name { get; set; }
    public string Type { get; set; }
    public string Balance { get; set; }
    public string TotalBalance { get; set; }
    public string Company { get; set; }
   


}
