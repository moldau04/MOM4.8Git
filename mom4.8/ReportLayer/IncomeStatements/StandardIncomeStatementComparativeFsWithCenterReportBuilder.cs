namespace ReportLayer.IncomeStatements
{
    using BusinessEntity.Reports.IncomeStatements;
    using BusinessLayer;
    using Stimulsoft.Base.Drawing;
    using Stimulsoft.Report;
    using Stimulsoft.Report.Components;
    using Stimulsoft.Report.Components.TextFormats;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Web;

    public class StandardIncomeStatementComparativeFsWithCenterReportBuilder : IReportBuilder<SISComparativeWithCenterInput>
    {
        public string FilePath { get { return "~/StimulsoftReports/StandardIncomeStatementComparativeFsWithCenter.mrt"; } }
        private BL_Report _objBLReport = new BL_Report();
        public string ConnConfig { get; set; }

        public StiReport GetReportTemplate()
        {
            var reportPath = System.Web.Hosting.HostingEnvironment.MapPath(FilePath);
            StiReport report = new StiReport();
            report.Load(reportPath);
            report.Compile();
            return report;
        }

        public void Build(StiReport report, SISComparativeWithCenterInput input, IReadOnlyDictionary<string, object> reportParams)
        {
            if (reportParams != null && reportParams.Any())
            {
                foreach (var paramItem in reportParams)
                {
                    report[paramItem.Key] = paramItem.Value;
                }
            }

            var lastDateOfMonth = DateTime.DaysInMonth(input.EndDate.Year - 1, input.YearEnd + 1);
            var startFiscalDate = new DateTime(input.EndDate.Year - 1, input.YearEnd + 1, lastDateOfMonth).AddDays(1);

            var startFishCalDatePreviousYear = startFiscalDate.AddYears(-1);


            var inputParams = new ComparativeFSWithCenterParam
            {
                Departments = input.Departments,
                MonthStartDateCurrentYear = input.StartDate,
                MonthEndDateCurrentYear = input.EndDate,
                MonthStartDatePreviousYear = input.StartDate.AddYears(-1),
                MonthEndDatePreviousYear = input.EndDate.AddYears(-1),
                YTDStartDateCurrentYear = startFiscalDate,
                YTDEndDateCurrentYear = input.EndDate,
                YTDStartDatePreviousYear = startFishCalDatePreviousYear,
                YTDEndDatePreviousYear = input.EndDate.AddYears(-1),
            };

            var departmentsList = input.Departments.Split(',').Select(t => int.Parse(t)).ToList();
            if (string.IsNullOrWhiteSpace(input.OfficeCenter))
            {
                throw new ArgumentNullException();
            }
            var officeCenterId = int.Parse(input.OfficeCenter);

            if (!departmentsList.Contains(officeCenterId))
            {
                departmentsList.Add(officeCenterId);
                var addedDepartments = string.Join(",", departmentsList);
                inputParams.Departments = addedDepartments;
            }

            var page = report.Pages[0];

            var dataSet = _objBLReport.StandardIncomeStatementComparativeFsWithCenter(ConnConfig, inputParams);

            if (string.IsNullOrWhiteSpace(input.Departments))
            {
                report.RegData("dsCompany", input.DsCompany.Tables[0]);
                report.Dictionary.Synchronize();
                page.CanGrow = true;
                report.Render();
                return;
            }

            var targetDs = new DataSet();
            var stringDateTimeFormat = "MM/dd/yyyy";
            var currencyFormat = new StiCurrencyFormatService();
            var percentFormat = new StiPercentageFormatService();

            var nameMapping = new Dictionary<string, ColumnSource>
            {
                { "fDesc",new ColumnSource("",null,2) },
                { "MonthTotalAmountCurrentYear", new ColumnSource(inputParams.MonthEndDateCurrentYear.ToString(stringDateTimeFormat),currencyFormat,1.3) },
                { "MonthPercentOfSaleCurrentYear",new ColumnSource("% of Sales",percentFormat,0.7)},
                { "MonthTotalAmountPreviousYear", new ColumnSource(inputParams.MonthEndDatePreviousYear.ToString(stringDateTimeFormat),currencyFormat,1.3)},
                { "MonthPercentOfSalePreviousYear",new ColumnSource("% of Sales",percentFormat,0.7)},
                { "MonthDifference",new ColumnSource("Difference",currencyFormat,1.3)},
                { "MonthTotalPercentOfSale",new ColumnSource("% of Sales",percentFormat,0.7)},

                { "YTDTotalAmountCurrentYear",new ColumnSource(inputParams.YTDEndDateCurrentYear.ToString(stringDateTimeFormat),currencyFormat,1.3)},
                { "YTDPercentOfSaleCurrentYear",new ColumnSource("% of Sales",percentFormat,0.7)},
                { "YTDTotalAmountPreviousYear",new ColumnSource(inputParams.YTDEndDatePreviousYear.ToString(stringDateTimeFormat),currencyFormat,1.3) },
                { "YTDPercentOfSalePreviousYear",new ColumnSource("% of Sales",percentFormat,0.7)},
                { "YTDDifference",new ColumnSource("Difference",currencyFormat,1.3)},
                { "YTDTotalPercentOfSale",new ColumnSource("% of Sales",percentFormat,0.7)},
            };

            var dataTable = dataSet.Tables[0];
            var newColumns = new DataColumn[]
            {
                new DataColumn("MonthPercentOfSalePreviousYear",typeof(decimal)),
                new DataColumn("MonthPercentOfSaleCurrentYear",typeof(decimal)),
                new DataColumn("MonthDifference",typeof(decimal)),
                new DataColumn("MonthTotalPercentOfSale",typeof(decimal)),


                new DataColumn("YTDPercentOfSalePreviousYear",typeof(decimal)),
                new DataColumn("YTDPercentOfSaleCurrentYear",typeof(decimal)),
                new DataColumn("YTDDifference",typeof(decimal)),
                new DataColumn("YTDTotalPercentOfSale",typeof(decimal)),
            };

            dataTable.Columns.AddRange(newColumns);
            var urlColumn = new DataColumn("Url", typeof(string));
            dataTable.Columns.Add(urlColumn);
            var request = HttpContext.Current.Request;
            dataTable.AsEnumerable().ToList().ForEach(b => b["Url"] = (request.Url.Scheme +
                                              (Uri.SchemeDelimiter +
                                                  (request.Url.Authority +
                                                      (request.ApplicationPath + "/accountledger.aspx?id=" + b["AcctId"].ToString() + "&s="
                                                      + System.Web.HttpUtility.UrlEncode(input.StartDate.ToShortDateString()).ToString()
                                                        + "&e=" + System.Web.HttpUtility.UrlEncode(input.EndDate.ToShortDateString()).ToString()
                                                      )
                                                  )
                                              )
                                           )
                                       );

            var listOfCenters = new List<CenterConsolidateFS>();
            foreach (var departement in departmentsList)
            {
                var centralNameRow = input.Centrals.Tables[0].AsEnumerable().First(t => t.Field<int>("ID") == departement);
                var centralname = centralNameRow.Field<string>("CentralName");
                ProcessDataByCentral(departement, centralname, nameMapping, listOfCenters, dataSet, targetDs, page);
            }

            var consolidateSourceDic = new Dictionary<string, string>
            {
                { "YTD","Year To Date" },
                { "Month","For The Month" },
            };

            foreach (var source in consolidateSourceDic)
            {
                DataTable schema;
                var listOfConsolidateRows = CreateConsolidateDatatable(listOfCenters, source.Key, officeCenterId, out schema);
                AppendConsolidateHeaderBand(page, listOfCenters, source.Value);

                foreach (var row in listOfConsolidateRows)
                {
                    var sourceName = string.Format("Consolidate_{0}_{1}", source.Key, Guid.NewGuid().ToString("N"));
                    var dataRowTable = CreateDataTableFromRow(schema, row.DataRow, row.DataRow.Field<string>("fDesc"));
                    dataRowTable.TableName = sourceName;
                    targetDs.Tables.Add(dataRowTable);
                    AppendConsolidateDataBand(page, listOfCenters, sourceName, row.Format);
                }
            }

            report.RegData("main", targetDs);
            report.RegData("dsCompany", input.DsCompany.Tables[0]);
            report.Dictionary.Synchronize();
            page.CanGrow = true;
            report.Render();
        }

        private const int ContractRevenue = 3;
        private const int CostOfSale = 4;
        private const int Expense = 5;
        private void ProcessDataByCentral(int centralId, string centralName, Dictionary<string, ColumnSource> nameMapping, List<CenterConsolidateFS> listOfCenters,
                                            DataSet rawDataSet, DataSet targetDs, StiPage page)
        {


            var columnWidth = page.Width / nameMapping.Sum(t => t.Value.Weight);
            AppendCentralColumnHeader(0, page.Width, page, centralName);
            AppendGroupColumnHeader(columnWidth * 2, columnWidth * 6, page);

            AppendColumnHeader(nameMapping, columnWidth, page);

            var dataRows = rawDataSet.Tables[0].AsEnumerable()
                                      .Where(t => t.Field<int>("CentralId") == centralId);
            if (!dataRows.Any())
            {
                var dataTable = rawDataSet.Tables[0];
                var source = new DataBandSource(centralId, CreateDataTableFromRow(dataTable, rawDataSet.Tables[0].NewRow(), string.Empty), "PageBreak", true, true);
                AppendDataBand(source, nameMapping, columnWidth, page);
                var emptyRow = dataTable.NewRow();

                foreach (DataColumn column in dataTable.Columns)
                {
                    if (column.DataType == typeof(decimal))
                    {
                        emptyRow[column] = 0.0m;
                    }
                }

                listOfCenters.Add(new CenterConsolidateFS
                {
                    ColumnName = "Center" + centralId,
                    CenterId = centralId,
                    CenterName = centralName,
                    ContractRevenue = emptyRow,
                    TotalCostOfGoodSolds = emptyRow,
                    TotalExpenses = emptyRow,
                    GrossProfit = emptyRow,
                    ProfitLoss = emptyRow
                });
                return;
            }

            var dataRowsTable = dataRows.CopyToDataTable();
            var listDataBand = new List<StiDataBand>();
            var revenueSummary = GetSummaryByAccountType(dataRowsTable, ContractRevenue);
            var totalCostOfGoodSoldSummary = GetSummaryByAccountType(dataRowsTable, CostOfSale);
            var totalOfExpenseSummary = GetSummaryByAccountType(dataRowsTable, Expense);
            var listTotal = new List<SummaryByAccountType>
            {
                totalCostOfGoodSoldSummary,
                totalOfExpenseSummary
            };

            var contractRevenueRow = revenueSummary.SummaryRow;

            foreach (var accountType in listTotal)
            {
                var totalMonthPercentOfSalePreviousYear = 0.0m;
                var totalMonthPercentOfSaleCurrentYear = 0.0m;
                var totalMonthDifference = 0.0m;
                var totalMonthTotalPercentOfSale = 0.0m;
                var totalYTDPercentOfSalePreviousYear = 0.0m;
                var totalYTDPercentOfSaleCurrentYear = 0.0m;
                var totalYTDDifference = 0.0m;
                var totalYTDTotalPercentOfSale = 0.0m;
                foreach (DataRow item in accountType.AccountData.Rows)
                {
                    item["MonthPercentOfSalePreviousYear"] = contractRevenueRow.Field<decimal>("MonthTotalAmountPreviousYear") == 0.0m ? 0 :
                                                            item.Field<decimal>("MonthTotalAmountPreviousYear")
                                                            / contractRevenueRow.Field<decimal>("MonthTotalAmountPreviousYear");
                    totalMonthPercentOfSalePreviousYear += item.Field<decimal>("MonthPercentOfSalePreviousYear");

                    item["MonthPercentOfSaleCurrentYear"] = contractRevenueRow.Field<decimal>("MonthTotalAmountCurrentYear") == 0.0m ? 0 :
                                                            item.Field<decimal>("MonthTotalAmountCurrentYear")
                                                             / contractRevenueRow.Field<decimal>("MonthTotalAmountCurrentYear");
                    totalMonthPercentOfSaleCurrentYear += item.Field<decimal>("MonthPercentOfSaleCurrentYear");

                    item["MonthDifference"] = item.Field<decimal>("MonthTotalAmountCurrentYear")
                                              - item.Field<decimal>("MonthTotalAmountPreviousYear");

                    totalMonthDifference += item.Field<decimal>("MonthDifference");
                    item["MonthTotalPercentOfSale"] = item.Field<decimal>("MonthPercentOfSaleCurrentYear")
                                                      - item.Field<decimal>("MonthPercentOfSalePreviousYear");
                    totalMonthTotalPercentOfSale += item.Field<decimal>("MonthTotalPercentOfSale");

                    item["YTDPercentOfSalePreviousYear"] = contractRevenueRow.Field<decimal>("YTDTotalAmountPreviousYear") == 0.0m ? 0 :
                                                           item.Field<decimal>("YTDTotalAmountPreviousYear")
                                                        / contractRevenueRow.Field<decimal>("YTDTotalAmountPreviousYear");
                    totalYTDPercentOfSalePreviousYear += item.Field<decimal>("YTDPercentOfSalePreviousYear");

                    item["YTDPercentOfSaleCurrentYear"] = contractRevenueRow.Field<decimal>("YTDTotalAmountCurrentYear") == 0.0m ? 0 :
                                                            item.Field<decimal>("YTDTotalAmountCurrentYear")
                                                             / contractRevenueRow.Field<decimal>("YTDTotalAmountCurrentYear");
                    totalYTDPercentOfSaleCurrentYear += item.Field<decimal>("YTDPercentOfSaleCurrentYear");
                    item["YTDDifference"] = item.Field<decimal>("YTDTotalAmountCurrentYear")
                                            - item.Field<decimal>("YTDTotalAmountPreviousYear");
                    totalYTDDifference += item.Field<decimal>("YTDDifference");
                    item["YTDTotalPercentOfSale"] = item.Field<decimal>("YTDPercentOfSaleCurrentYear")

                                                    - item.Field<decimal>("YTDPercentOfSalePreviousYear");
                    totalYTDTotalPercentOfSale = item.Field<decimal>("YTDTotalPercentOfSale");
                }

                accountType.SummaryRow["MonthPercentOfSalePreviousYear"] = totalMonthPercentOfSalePreviousYear;
                accountType.SummaryRow["MonthPercentOfSaleCurrentYear"] = totalMonthPercentOfSaleCurrentYear;
                accountType.SummaryRow["MonthDifference"] = totalMonthDifference;
                accountType.SummaryRow["MonthTotalPercentOfSale"] = totalMonthTotalPercentOfSale;

                accountType.SummaryRow["YTDPercentOfSalePreviousYear"] = totalYTDPercentOfSalePreviousYear;
                accountType.SummaryRow["YTDPercentOfSaleCurrentYear"] = totalYTDPercentOfSaleCurrentYear;
                accountType.SummaryRow["YTDDifference"] = totalYTDDifference;
                accountType.SummaryRow["YTDTotalPercentOfSale"] = totalYTDTotalPercentOfSale;
            }

            var grossProfit = GetGrossProfit(dataRowsTable, totalCostOfGoodSoldSummary.SummaryRow, contractRevenueRow);
            var profitLoss = GetProfitLoss(dataRowsTable, totalOfExpenseSummary.SummaryRow, grossProfit, contractRevenueRow);
            listOfCenters.Add(new CenterConsolidateFS
            {
                ColumnName = "Center" + centralId,
                CenterId = centralId,
                CenterName = centralName,
                ContractRevenue = contractRevenueRow,
                TotalCostOfGoodSolds = totalCostOfGoodSoldSummary.SummaryRow,
                TotalExpenses = totalOfExpenseSummary.SummaryRow,
                GrossProfit = grossProfit,
                ProfitLoss = profitLoss
            });

            var dataBandSources = new List<DataBandSource>
            {
                new DataBandSource(centralId,CreateDataTableFromRow(dataRowsTable,revenueSummary.SummaryRow,"Contract Revenue"),"ContractRevenueSummary",true),
                new DataBandSource(centralId,CreateDataTableFromRow(dataRowsTable,dataRowsTable.NewRow(),string.Empty),"ContractRevenueSummaryPadding",true),
                new DataBandSource(centralId,CreateTitleDataTable(dataRowsTable,"Cost of Goods Sold"),"CostofGoodsSoldTitle",true),
                new DataBandSource(centralId,totalCostOfGoodSoldSummary.AccountData,"CostofGoodsSold",false),
                new DataBandSource(centralId,CreateDataTableFromRow(dataRowsTable,totalCostOfGoodSoldSummary.SummaryRow,"Total Cost of Goods Sold"),"CostofGoodsSoldSummary",true),
                new DataBandSource(centralId,CreateDataTableFromRow(dataRowsTable,dataRowsTable.NewRow(),string.Empty),"CostofGoodsSoldSummaryPadding",true),
                new DataBandSource(centralId,CreateDataTableFromRow(dataRowsTable,grossProfit,"Gross Profit"),"GrossProfit",true),
                new DataBandSource(centralId,CreateDataTableFromRow(dataRowsTable,dataRowsTable.NewRow(),string.Empty),"GrossProfitPadding",true),


                new DataBandSource(centralId,CreateTitleDataTable(dataRowsTable,"General & Administrative"),"ExpenseTitle",true),
                new DataBandSource(centralId,totalOfExpenseSummary.AccountData,"Expense",false),
                new DataBandSource(centralId,CreateDataTableFromRow(dataRowsTable,totalOfExpenseSummary.SummaryRow,"Total General & Administrative"),"ExpenseSummary",true),
                new DataBandSource(centralId,CreateDataTableFromRow(dataRowsTable,dataRowsTable.NewRow(),string.Empty),"ExpenseSummaryPadding",true),
                new DataBandSource(centralId,CreateDataTableFromRow(dataRowsTable,profitLoss,"Profit (Loss)"),"ProfitLoss",true),
                new DataBandSource(centralId,CreateDataTableFromRow(dataRowsTable,dataRowsTable.NewRow(),string.Empty),"ProfitLossPadding",true,true),
            };

            foreach (var source in dataBandSources)
            {
                AppendDataBand(source, nameMapping, columnWidth, page);
                targetDs.Tables.Add(source.Source);
            }
        }

        private DataRow GetGrossProfit(DataTable schema, DataRow totalCost, DataRow contractRevenueRow)
        {
            var row = schema.NewRow();

            row["MonthTotalAmountPreviousYear"] = contractRevenueRow.Field<decimal>("MonthTotalAmountPreviousYear")
                                                  - totalCost.Field<decimal>("MonthTotalAmountPreviousYear");

            row["MonthPercentOfSalePreviousYear"] = contractRevenueRow.Field<decimal>("MonthTotalAmountPreviousYear") == 0.0m ? 0 :
                                                            row.Field<decimal>("MonthTotalAmountPreviousYear")
                                                            / contractRevenueRow.Field<decimal>("MonthTotalAmountPreviousYear");

            row["MonthTotalAmountCurrentYear"] = contractRevenueRow.Field<decimal>("MonthTotalAmountCurrentYear")
                                                  - totalCost.Field<decimal>("MonthTotalAmountCurrentYear");
            row["MonthPercentOfSaleCurrentYear"] = contractRevenueRow.Field<decimal>("MonthTotalAmountCurrentYear") == 0.0m ? 0 :
                                                    row.Field<decimal>("MonthTotalAmountCurrentYear")
                                                     / contractRevenueRow.Field<decimal>("MonthTotalAmountCurrentYear");

            row["MonthDifference"] = contractRevenueRow.Field<decimal>("MonthDifference")
                                      - totalCost.Field<decimal>("MonthDifference");

            row["MonthTotalPercentOfSale"] = row.Field<decimal>("MonthPercentOfSaleCurrentYear")
                                              - row.Field<decimal>("MonthPercentOfSalePreviousYear");


            row["YTDTotalAmountPreviousYear"] = contractRevenueRow.Field<decimal>("YTDTotalAmountPreviousYear")
                                                - totalCost.Field<decimal>("YTDTotalAmountPreviousYear");
            row["YTDPercentOfSalePreviousYear"] = contractRevenueRow.Field<decimal>("YTDTotalAmountPreviousYear") == 0.0m ? 0 :
                                                   row.Field<decimal>("YTDTotalAmountPreviousYear")
                                                / contractRevenueRow.Field<decimal>("YTDTotalAmountPreviousYear");
            row["YTDTotalAmountCurrentYear"] = contractRevenueRow.Field<decimal>("YTDTotalAmountCurrentYear")
                                                  - totalCost.Field<decimal>("YTDTotalAmountCurrentYear");
            row["YTDPercentOfSaleCurrentYear"] = contractRevenueRow.Field<decimal>("YTDTotalAmountCurrentYear") == 0.0m ? 0 :
                                                    row.Field<decimal>("YTDTotalAmountCurrentYear")
                                                     / contractRevenueRow.Field<decimal>("YTDTotalAmountCurrentYear");

            row["YTDDifference"] = contractRevenueRow.Field<decimal>("YTDDifference")
                                    - totalCost.Field<decimal>("YTDDifference");

            row["YTDTotalPercentOfSale"] = row.Field<decimal>("YTDPercentOfSaleCurrentYear")
                                            - row.Field<decimal>("YTDPercentOfSalePreviousYear");
            return row;
        }

        private DataRow GetProfitLoss(DataTable schema, DataRow totalExpense, DataRow grossProfit, DataRow contractRevenueRow)
        {
            var row = schema.NewRow();

            row["MonthTotalAmountPreviousYear"] = grossProfit.Field<decimal>("MonthTotalAmountPreviousYear")
                                                  - totalExpense.Field<decimal>("MonthTotalAmountPreviousYear");

            row["MonthPercentOfSalePreviousYear"] = contractRevenueRow.Field<decimal>("MonthTotalAmountPreviousYear") == 0.0m ? 0 :
                                                            row.Field<decimal>("MonthTotalAmountPreviousYear")
                                                            / contractRevenueRow.Field<decimal>("MonthTotalAmountPreviousYear");

            row["MonthTotalAmountCurrentYear"] = grossProfit.Field<decimal>("MonthTotalAmountCurrentYear")
                                                  - totalExpense.Field<decimal>("MonthTotalAmountCurrentYear");
            row["MonthPercentOfSaleCurrentYear"] = contractRevenueRow.Field<decimal>("MonthTotalAmountCurrentYear") == 0.0m ? 0 :
                                                    row.Field<decimal>("MonthTotalAmountCurrentYear")
                                                     / contractRevenueRow.Field<decimal>("MonthTotalAmountCurrentYear");

            row["MonthDifference"] = grossProfit.Field<decimal>("MonthDifference")
                                      - totalExpense.Field<decimal>("MonthDifference");

            row["MonthTotalPercentOfSale"] = row.Field<decimal>("MonthPercentOfSaleCurrentYear")
                                              - row.Field<decimal>("MonthPercentOfSalePreviousYear");


            row["YTDTotalAmountPreviousYear"] = grossProfit.Field<decimal>("YTDTotalAmountPreviousYear")
                                                 - totalExpense.Field<decimal>("YTDTotalAmountPreviousYear");

            row["YTDPercentOfSalePreviousYear"] = contractRevenueRow.Field<decimal>("YTDTotalAmountPreviousYear") == 0.0m ? 0 :
                                                            row.Field<decimal>("YTDTotalAmountPreviousYear")
                                                            / contractRevenueRow.Field<decimal>("YTDTotalAmountPreviousYear");

            row["YTDTotalAmountCurrentYear"] = grossProfit.Field<decimal>("YTDTotalAmountCurrentYear")
                                                  - totalExpense.Field<decimal>("YTDTotalAmountCurrentYear");
            row["YTDPercentOfSaleCurrentYear"] = contractRevenueRow.Field<decimal>("YTDTotalAmountCurrentYear") == 0.0m ? 0 :
                                                    row.Field<decimal>("YTDTotalAmountCurrentYear")
                                                     / contractRevenueRow.Field<decimal>("YTDTotalAmountCurrentYear");

            row["YTDDifference"] = grossProfit.Field<decimal>("YTDDifference")
                                      - totalExpense.Field<decimal>("YTDDifference");

            row["YTDTotalPercentOfSale"] = row.Field<decimal>("YTDPercentOfSaleCurrentYear")
                                              - row.Field<decimal>("YTDPercentOfSalePreviousYear");
            return row;
        }


        private static DataTable CreateDataTableFromRow(DataTable schemaDt, DataRow row, string desc)
        {
            var dataTable = schemaDt.Clone();
            dataTable.LoadDataRow(row.ItemArray, true);
            dataTable.Rows[0]["fDesc"] = desc;
            return dataTable;
        }

        private static DataTable CreateTitleDataTable(DataTable schemaDt, string title)
        {
            var row = schemaDt.NewRow();
            row["fDesc"] = title;
            return CreateDataTableFromRow(schemaDt, row, title);
        }

        private void AppendCentralColumnHeader(double x, double width, StiPage page, string title)
        {
            StiHeaderBand headerBand = new StiHeaderBand
            {
                Height = 0.24,
                Name = "HeaderBand",
            };

            headerBand.Border.Style = StiPenStyle.None;
            headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            headerBand.PrintIfEmpty = true;
            page.Components.Add(headerBand);

            var hText = new StiText(new RectangleD(x, 0, width, 0.30))
            {
                Text = title,
                HorAlignment = StiTextHorAlignment.Left,
                VertAlignment = StiVertAlignment.Center,
                Name = "HeaderText" + new Guid().ToString("N"),
                Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213))
            };

            hText.Border.Side = StiBorderSides.Bottom;
            hText.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            hText.Border.Style = StiPenStyle.Solid;
            hText.Border.Color = Color.White;
            hText.TextBrush = new StiSolidBrush(Color.White);
            hText.WordWrap = true;
            hText.HorAlignment = StiTextHorAlignment.Center;
            headerBand.Components.Add(hText);
        }

        private void AppendGroupColumnHeader(double x, double width, StiPage page)
        {
            StiHeaderBand headerBand = new StiHeaderBand();
            headerBand.Height = 0.24;
            headerBand.Name = "HeaderBand";
            headerBand.Border.Style = StiPenStyle.None;
            headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            headerBand.PrintIfEmpty = true;
            page.Components.Add(headerBand);

            var monthHText = CreateHeaderText(x, 0, width, "Month");
            var yTDhText = CreateHeaderText(x + width, 0, width, "Year To Date");
            headerBand.Components.Add(monthHText);
            headerBand.Components.Add(yTDhText);
        }

        private StiText CreateHeaderText(double x, double y, double width, string title)
        {
            var hText = new StiText(new RectangleD(x, y, width, 0.24))
            {
                Text = title,
                //hText.SetText(columnName.Value);
                HorAlignment = StiTextHorAlignment.Left,
                VertAlignment = StiVertAlignment.Center,
                Name = "HeaderText" + new Guid().ToString("N"),
                Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213))
            };

            hText.Border.Side = StiBorderSides.All;
            hText.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            hText.Border.Style = StiPenStyle.Solid;
            hText.Border.Color = Color.White;
            hText.TextBrush = new StiSolidBrush(Color.White);
            hText.WordWrap = true;
            hText.HorAlignment = StiTextHorAlignment.Center;
            return hText;
        }

        private void AppendColumnHeader(Dictionary<string, ColumnSource> nameMapping, double columnWidth, StiPage page)
        {
            StiHeaderBand headerBand = new StiHeaderBand();
            headerBand.Height = 0.24;
            headerBand.Name = "HeaderBand";
            headerBand.Border.Style = StiPenStyle.None;
            headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            headerBand.PrintIfEmpty = true;
            page.Components.Add(headerBand);
            var pos = 0.0;
            var nameIndex = 0;

            foreach (var column in nameMapping)
            {
                StiText hText = null;
                hText = new StiText(new RectangleD(pos, 0, columnWidth * column.Value.Weight, 0.24))
                {
                    Text = column.Value.DisplayName,
                    //hText.SetText(columnName.Value);
                    HorAlignment = StiTextHorAlignment.Left,
                    VertAlignment = StiVertAlignment.Center,
                    Name = "HeaderText" + nameIndex.ToString(),
                    Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213))
                };
                hText.Border.Side = StiBorderSides.All;
                hText.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
                hText.Border.Style = StiPenStyle.None;
                hText.TextBrush = new StiSolidBrush(Color.White);
                hText.WordWrap = true;
                headerBand.Components.Add(hText);

                pos = pos + columnWidth * column.Value.Weight;
                nameIndex++;
            }
        }

        private void AppendDataBand(DataBandSource source, Dictionary<string, ColumnSource> nameMapping, double columnWidth, StiPage page)
        {
            StiDataBand dataBand = new StiDataBand
            {
                DataSourceName = source.SourceName,

                Height = 0.24,
                Name = "DataBand" + source.SourceName,
            };

            if (source.IsNewPageAfter)
            {
                dataBand.NewPageAfter = source.IsNewPageAfter;
            }


            dataBand.Border.Style = StiPenStyle.None;
            page.Components.Add(dataBand);

            double pos = 0;

            int nameIndex = 1;
            var dataTable = source.Source;
            foreach (var column in nameMapping)
            {
                var dataColumn = dataTable.Columns[column.Key];

                StiText dataText = null;
                dataText = new StiText(new RectangleD(pos, 0, columnWidth * column.Value.Weight, 0.24))
                {
                    Font = new System.Drawing.Font("Arial", 7F, source.IsFDescBold ? FontStyle.Bold : FontStyle.Regular)
                };

                //dataText.Text.Value = "{" + source.SourceName + "." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";         
                dataText.Text.Value = string.Format(@"{{IIF({0}[""{1}""] == DBNull.Value, null, {0}[""{1}""])}}", source.SourceName, Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName));
                dataText.TextFormat = column.Value.Format;

                dataText.HorAlignment = StiTextHorAlignment.Left;
                dataText.VertAlignment = StiVertAlignment.Center;
                dataText.Name = "DataText" + nameIndex.ToString();
                dataText.Border.Style = StiPenStyle.None;
                dataText.OnlyText = false;
                dataText.Border.Side = StiBorderSides.All;
                dataText.WordWrap = true;
                dataText.Margins = new StiMargins(0, 1, 0, 0);

                if (column.Key == "fDesc")
                {
                    dataText.Hyperlink = new StiHyperlinkExpression(string.Format(@"{{{0}.{1}}}", source.SourceName, "Url"));
                }
                //dataText.CanShrink = true;

                dataBand.Components.Add(dataText);

                pos = pos + columnWidth * column.Value.Weight;
                nameIndex++;
            }
            dataBand.PrintIfDetailEmpty = true;
            //dataBand.CanShrink = true;
        }

        private SummaryByAccountType GetSummaryByAccountType(DataTable source, int accountType)
        {
            var summaryRow = source.NewRow();
            var accountRows = source.AsEnumerable().Where(t => t.Field<short>("Type") == accountType);

            foreach (DataColumn dataColumn in source.Columns)
            {
                if (dataColumn.DataType == typeof(decimal))
                {
                    //
                    var total = 0.0m;
                    foreach (var row in accountRows)
                    {
                        if (row[dataColumn.ColumnName] == DBNull.Value)
                        {
                            row[dataColumn.ColumnName] = 0.0m;
                        }
                        else
                        {
                            total += row.Field<decimal>(dataColumn.ColumnName);
                        }
                    }

                    summaryRow[dataColumn.ColumnName] = total;
                }
            }

            var dataTable = accountRows.Any() ? accountRows.CopyToDataTable() : source.Clone();

            //if (!accountRows.Any())
            //{
            //    dataTable.Rows.Add(dataTable.NewRow());
            //}

            return new SummaryByAccountType
            {
                SummaryRow = summaryRow,
                AccountData = dataTable,
                AccountType = accountType
            };
        }

        private List<ConsolidateRow> CreateConsolidateDatatable(List<CenterConsolidateFS> listOfCenters, string prefix, int officeCenterId, out DataTable schema)
        {
            var dataTable = new DataTable();
            var listOfConsolidateRows = new List<ConsolidateRow>();
            var currencyFormat = new StiCurrencyFormatService();
            var percentFormat = new StiPercentageFormatService();
            dataTable.Columns.Add(new DataColumn("fDesc", typeof(string)));

            foreach (var center in listOfCenters)
            {
                dataTable.Columns.Add(new DataColumn(center.ColumnName, typeof(decimal)));
            }

            dataTable.Columns.Add(new DataColumn("Total", typeof(decimal)));

            schema = dataTable;

            var contractRevenuePercentRow = dataTable.NewRow();
            contractRevenuePercentRow["fDesc"] = string.Empty;
            listOfConsolidateRows.Add(new ConsolidateRow(contractRevenuePercentRow, percentFormat));

            var contractRevenueRow = dataTable.NewRow();
            contractRevenueRow["fDesc"] = "Contract, Service and Repair Revenue";
            listOfConsolidateRows.Add(new ConsolidateRow(contractRevenueRow, currencyFormat));

            var earnedRevenueRow = dataTable.NewRow();
            earnedRevenueRow["fDesc"] = "Cost of Earned Revenue";
            listOfConsolidateRows.Add(new ConsolidateRow(earnedRevenueRow, currencyFormat));

            var grossProfitRow = dataTable.NewRow();
            grossProfitRow["fDesc"] = "Gross Profit";
            listOfConsolidateRows.Add(new ConsolidateRow(grossProfitRow, currencyFormat));

            var grossProfitPercentageRow = dataTable.NewRow();
            grossProfitPercentageRow["fDesc"] = "Gross Profit %";
            listOfConsolidateRows.Add(new ConsolidateRow(grossProfitPercentageRow, percentFormat));

            var gAndADivFsRow = dataTable.NewRow();
            gAndADivFsRow["fDesc"] = "G&A (Div f/s)";
            listOfConsolidateRows.Add(new ConsolidateRow(gAndADivFsRow, currencyFormat));

            var gAndAAllocRow = dataTable.NewRow();
            gAndAAllocRow["fDesc"] = "G&A (Alloc)";
            listOfConsolidateRows.Add(new ConsolidateRow(gAndAAllocRow, currencyFormat));

            var gAndAAllocPercentRow = dataTable.NewRow();
            gAndAAllocPercentRow["fDesc"] = "G&A (Alloc %)";
            listOfConsolidateRows.Add(new ConsolidateRow(gAndAAllocPercentRow, percentFormat));

            var otherIncomeRow = dataTable.NewRow();
            otherIncomeRow["fDesc"] = "Other Income (Expense)";
            listOfConsolidateRows.Add(new ConsolidateRow(otherIncomeRow, currencyFormat));

            var incomeBeforeTaxRow = dataTable.NewRow();
            incomeBeforeTaxRow["fDesc"] = "Income Before Taxes";
            listOfConsolidateRows.Add(new ConsolidateRow(incomeBeforeTaxRow, currencyFormat));

            var provisionsForTaxesRow = dataTable.NewRow();
            provisionsForTaxesRow["fDesc"] = "Provisions for Taxes";
            listOfConsolidateRows.Add(new ConsolidateRow(provisionsForTaxesRow, currencyFormat));

            var netIncomeRow = dataTable.NewRow();
            netIncomeRow["fDesc"] = "NET INCOME";
            listOfConsolidateRows.Add(new ConsolidateRow(netIncomeRow, currencyFormat));

            var netIncomePercentRow = dataTable.NewRow();
            netIncomePercentRow["fDesc"] = "Net Income %";
            listOfConsolidateRows.Add(new ConsolidateRow(netIncomePercentRow, percentFormat));

            var salesBreakEvenPointRow = dataTable.NewRow();
            salesBreakEvenPointRow["fDesc"] = "Sales-Break-Even Point";
            listOfConsolidateRows.Add(new ConsolidateRow(salesBreakEvenPointRow, currencyFormat));

            var totalContractRevenue = listOfCenters.Select(t => t.ContractRevenue.Field<decimal>(prefix + "TotalAmountCurrentYear")).Sum();
            var totalEarnedRevenue = 0.0m;
            var totalGrossProfit = 0.0m;
            var totalGAndA = 0.0m;
            foreach (var center in listOfCenters)
            {
                var centerColumn = center.ColumnName;
                contractRevenueRow[centerColumn] = center.ContractRevenue[prefix + "TotalAmountCurrentYear"];
                contractRevenuePercentRow[centerColumn] = totalContractRevenue == 0.0m ? 0.0m :
                                                          contractRevenueRow.Field<decimal>(centerColumn) / totalContractRevenue;
                earnedRevenueRow[centerColumn] = center.TotalCostOfGoodSolds[prefix + "TotalAmountCurrentYear"];
                totalEarnedRevenue += earnedRevenueRow.Field<decimal>(centerColumn);
                grossProfitRow[centerColumn] = contractRevenueRow.Field<decimal>(centerColumn) - earnedRevenueRow.Field<decimal>(centerColumn);
                totalGrossProfit += grossProfitRow.Field<decimal>(centerColumn);
                var contractRevenue = contractRevenueRow.Field<decimal>(centerColumn);
                grossProfitPercentageRow[centerColumn] = contractRevenue == 0 ? 0 :
                                                         grossProfitRow.Field<decimal>(centerColumn) / contractRevenueRow.Field<decimal>(centerColumn);
                gAndADivFsRow[centerColumn] = center.TotalExpenses[prefix + "TotalAmountCurrentYear"];
                totalGAndA += gAndADivFsRow.Field<decimal>(centerColumn);
                //G&A (Alloc %) is blocked
            }

            contractRevenueRow["Total"] = totalContractRevenue;
            earnedRevenueRow["Total"] = totalEarnedRevenue;
            grossProfitRow["Total"] = totalGrossProfit;
            gAndADivFsRow["Total"] = totalGAndA;


            var officeCenterColumn = listOfCenters.FirstOrDefault(t => t.CenterId == officeCenterId);

            if (officeCenterColumn != null)
            {
                gAndAAllocRow[officeCenterColumn.ColumnName] = earnedRevenueRow.Field<decimal>(officeCenterColumn.ColumnName)
                                                         - gAndADivFsRow.Field<decimal>(officeCenterColumn.ColumnName);
                gAndAAllocPercentRow[officeCenterColumn.ColumnName] = contractRevenueRow.Field<decimal>(officeCenterColumn.ColumnName) == 0.0m ? 0.0m :
                                                                      (gAndADivFsRow.Field<decimal>(officeCenterColumn.ColumnName)
                                                                      + gAndAAllocRow.Field<decimal>(officeCenterColumn.ColumnName)) /
                                                                      contractRevenueRow.Field<decimal>(officeCenterColumn.ColumnName);
            }

            //TODO Quant: Completely blocked

            return listOfConsolidateRows;
        }

        private void AppendConsolidateDataBand(StiPage page, List<CenterConsolidateFS> listOfCenters, string sourceName, StiFormatService format)
        {
            var listOfCenterDic = listOfCenters.ToDictionary(key => key.ColumnName, value => value);

            var width = page.Width / (listOfCenters.Count + 3);

            var dataBand = new StiDataBand
            {
                //TODO Quant: Add later on
                DataSourceName = sourceName,
                Height = 0.24,
                Name = "DataBand" + Guid.NewGuid().ToString("N"),
            };

            page.Components.Add(dataBand);

            var descband = CreateCell(0, 0, width * 2, false); ;
            descband.Text.Value = string.Format(@"{{{0}.{1}}}", sourceName, "fDesc");
            dataBand.Components.Add(descband);
            var offset = width * 2;

            foreach (var center in listOfCenters)
            {

                var cellText = CreateCell(offset, 0, width, false);
                cellText.TextFormat = format;
                cellText.Text.Value = string.Format(@"{{{0}.{1}}}", sourceName, center.ColumnName);
                dataBand.Components.Add(cellText);
                offset += width;
            }

            //TODO Quant; later on
            var totalCellText = CreateCell(offset, 0, width, false);
            totalCellText.Text.Value = string.Format(@"{{{0}.{1}}}", sourceName, "Total");
            totalCellText.TextFormat = format;
            dataBand.Components.Add(totalCellText);
        }

        private void AppendConsolidateHeaderBand(StiPage page, List<CenterConsolidateFS> listOfCenters, string bandTitle)
        {
            AppendCentralColumnHeader(0, page.Width, page, bandTitle);
            var width = page.Width / (listOfCenters.Count + 3);
            var listOfCenterDic = listOfCenters.ToDictionary(key => key.ColumnName, value => value);
            StiHeaderBand headerBand = new StiHeaderBand();
            headerBand.Height = 0.24;
            headerBand.Name = "HeaderBand";
            headerBand.Border.Style = StiPenStyle.None;
            headerBand.Brush = new StiSolidBrush(Color.FromArgb(91, 155, 213));
            headerBand.PrintIfEmpty = true;
            page.Components.Add(headerBand);
            var descHeader = CreateHeaderText(0, 0, width * 2, string.Empty);
            headerBand.Components.Add(descHeader);
            var offset = width * 2;

            foreach (var center in listOfCenterDic)
            {
                var headerText = CreateHeaderText(offset, 0, width, listOfCenterDic[center.Key].CenterName);
                headerBand.Components.Add(headerText);
                offset += width;
            }

            var totalHeaderText = CreateHeaderText(offset, 0, width, "Total");
            headerBand.Components.Add(totalHeaderText);
        }

        public StiText CreateCell(double x, double y, double width, bool isBold)
        {
            StiText dataText = null;
            dataText = new StiText(new RectangleD(x, y, width, 0.24))
            {
                Font = new System.Drawing.Font("Arial", 7F, isBold ? FontStyle.Bold : FontStyle.Regular)
            };

            //dataText.Text.Value = "{" + source.SourceName + "." + Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName) + "}";         
            // dataText.Text.Value = string.Format(@"{{IIF({0}[""{1}""] == DBNull.Value, null, {0}[""{1}""])}}", source.SourceName, Stimulsoft.Report.CodeDom.StiCodeDomSerializator.ReplaceSymbols(dataColumn.ColumnName));
            // dataText.TextFormat = column.Value.Format;

            dataText.HorAlignment = StiTextHorAlignment.Left;
            dataText.VertAlignment = StiVertAlignment.Center;
            dataText.Name = "DataText" + Guid.NewGuid().ToString("N");
            dataText.Border.Style = StiPenStyle.None;
            dataText.OnlyText = false;
            dataText.Border.Side = StiBorderSides.All;
            dataText.WordWrap = true;
            dataText.Margins = new StiMargins(0, 1, 0, 0);
            return dataText;
        }

        private class ConsolidateRow
        {
            public ConsolidateRow(DataRow row, StiFormatService format)
            {
                DataRow = row;
                Format = format;
            }

            public DataRow DataRow { get; set; }
            public StiFormatService Format { get; set; }
        }

        private class CenterConsolidateFS
        {
            public string ColumnName { get; set; }
            public int CenterId { get; set; }
            public string CenterName { get; set; }
            public DataRow ContractRevenue { get; set; }
            public DataRow TotalCostOfGoodSolds { get; set; }
            public DataRow GrossProfit { get; set; }
            public DataRow TotalExpenses { get; set; }
            public DataRow ProfitLoss { get; set; }
        }

        private class SummaryByAccountType
        {
            public DataRow SummaryRow { get; set; }
            public DataTable AccountData { get; set; }
            public int AccountType { get; set; }
        }

        private class DataBandSource
        {
            public DataBandSource(int centralId, DataTable source, string sourceName, bool isFDescBold, bool isNewPageAfter = false)
            {
                Source = source;
                IsFDescBold = isFDescBold;
                IsNewPageAfter = isNewPageAfter;
                Source.TableName = "Central" + centralId + "_" + sourceName;
                SourceName = Source.TableName;
            }

            public DataTable Source { get; private set; }
            public string SourceName { get; private set; }
            public bool IsFDescBold { get; private set; }
            public bool IsNewPageAfter { get; private set; }
        }

        private class ColumnSource
        {
            public ColumnSource(string displayName, StiFormatService format, double weight)
            {
                DisplayName = displayName;
                Format = format;
                Weight = weight;
            }

            public string DisplayName { get; set; }
            public StiFormatService Format { get; set; }
            public double Weight { get; set; }
        }
    }
}