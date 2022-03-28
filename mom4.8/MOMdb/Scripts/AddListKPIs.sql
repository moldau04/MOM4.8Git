DELETE FROM [dbo].[KPI];

SET IDENTITY_INSERT [dbo].[KPI] ON 
INSERT [dbo].[KPI] ([ID], [Name], [Module], [Screen], [Type], [UserControl]) VALUES (1, N'120+ Accounts Receivable', N'Customer', N'Customer', NULL, N'~/NewKPI/Components/Contents/OneTwentyDayAccountsReceivable.ascx')
INSERT [dbo].[KPI] ([ID], [Name], [Module], [Screen], [Type], [UserControl]) VALUES (2, N'90+ Accounts Receivable', N'Customer', N'Customer', NULL, N'~/NewKPI/Components/Contents/NinetyDayAccountsReceivable.ascx')
INSERT [dbo].[KPI] ([ID], [Name], [Module], [Screen], [Type], [UserControl]) VALUES (3, N'60+ Accounts Receivable', N'Customer', N'Customer', NULL, N'~/NewKPI/Components/Contents/SixtyDayAccountsReceivable.ascx')
INSERT [dbo].[KPI] ([ID], [Name], [Module], [Screen], [Type], [UserControl]) VALUES (4, N'Avg Estimate Conversion Rate', N'Sales', N'Estimate', NULL, N'~/NewKPI/Components/Contents/AvgEstimateConversionRate.ascx')
INSERT [dbo].[KPI] ([ID], [Name], [Module], [Screen], [Type], [UserControl]) VALUES (5, N'Equipment by Type', N'Customer', N'Equipment', NULL, N'~/NewKPI/Components/EquipmentTypeChart.ascx')
INSERT [dbo].[KPI] ([ID], [Name], [Module], [Screen], [Type], [UserControl]) VALUES (6, N'Equipment By Building', N'Customer', N'Equipment', NULL, N'~/NewKPI/Components/EquipmentBuildingChart.ascx')
INSERT [dbo].[KPI] ([ID], [Name], [Module], [Screen], [Type], [UserControl]) VALUES (7, N'Converted Estimates By Salesperson Avg. Days', N'Sales', N'Estimate', NULL, N'~/NewKPI/Components/RecurringHoursChart.ascx')
INSERT [dbo].[KPI] ([ID], [Name], [Module], [Screen], [Type], [UserControl]) VALUES (8, N'Monthly Recurring Open vs Completed', N'Schedule', N'Ticket list', NULL, N'~/NewKPI/Components/TicketRecurringChart.ascx')
INSERT [dbo].[KPI] ([ID], [Name], [Module], [Screen], [Type], [UserControl]) VALUES (9, N'Actual vs Budgeted Revenue', N'Statements', N'Profit and Loss', NULL, N'~/NewKPI/Components/ActualBudgetedRevenueChart.ascx')
INSERT [dbo].[KPI] ([ID], [Name], [Module], [Screen], [Type], [UserControl]) VALUES (10, N'Recurring Hours Remaining for Current Month by Route', N'Schedule', N'Ticket list', NULL, N'~/NewKPI/Components/RecurringHoursRemaining.ascx')
INSERT [dbo].[KPI] ([ID], [Name], [Module], [Screen], [Type], [UserControl]) VALUES (11, N'Monthly Revenue by Department', N'Statements', N'Profit and Loss', NULL, N'~/NewKPI/Components/MonthlyRevenueByDeptChart.ascx')
INSERT [dbo].[KPI] ([ID], [Name], [Module], [Screen], [Type], [UserControl]) VALUES (12, N'Trouble Calls by Equipment', N'Schedule', N'Ticket list', NULL, N'~/NewKPI/Components/TroubleCallsByEquipment.ascx')
SET IDENTITY_INSERT [dbo].[KPI] OFF