CREATE PROCEDURE [dbo].[spAddCompany]

@CompanyName varchar(15),
@Address varchar(8000),
@City varchar(50),
@State varchar(2),
@Zip varchar(10),
@Tel varchar(22),
@fax varchar(20),
@Email varchar(50),
@Web varchar(50),
@MSM varchar(15),
@DSN varchar(100),
@DBname varchar(50),
@UserName varchar(50),	
@Password varchar(50),
@Contact varchar(50),
@Remarks varchar(200),
@Lat varchar(50),
@Lng varchar(50)

as
BEGIN TRANSACTION

if not exists (select 1 from Control where DBName=@DBname)
begin insert into Control (Name,Address,City,State,Zip,Phone,Fax,Email,WebAddress,msm,Dsn,DBname,username,password,GeoLock,Contact,Remarks,Custweb,Lat,Lng) values(@CompanyName,     @Address,@City,@State,@Zip,@Tel,@fax,@Email,@Web,@MSM,@DSN,@DBname,@UserName,@Password,0,   @Contact,@Remarks,0,@Lat,@Lng)
end else begin RAISERROR ('Database name already exixts, please use different database name!', 16, 1)  RETURN end

IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN  	RAISERROR ('Error Occured', 16, 1)     ROLLBACK TRANSACTION      RETURN END
  
INSERT [dbo].[tblUser] ([fUser], [Password], [Status], [Access], [fStart], [fEnd], [Since], [Last], [Remarks], [Owner], [Location], [Elevator], [Invoice], [Deposit], [Apply], [WriteOff], [ProcessC], [ProcessT], [Interest], [Collection], [ARViewer], [AROther], [Vendor], [Bill], [BillSelect], [BillPay], [PO], [APViewer], [APOther], [Chart], [GLAdj], [GLViewer], [IReg], [CReceipt], [PJournal], [YE], [Service], [Financial], [Item], [InvViewer], [InvAdj], [Job], [JobViewer], [JobTemplate], [JobClose], [JobResult], [Dispatch], [Ticket], [Resolve], [TestDate], [TC], [Human], [DispReport], [Violation], [UserS], [Control], [Bank], [BankRec], [BankViewer], [Manual], [Log], [Code], [STax], [Zone], [Territory], [Commodity], [Employee], [Crew], [PRProcess], [PRRemit], [PRRegister], [PRReport], [Diary], [TTD], [Document], [Phone], [ToDo], [Sales], [ToDoC], [EN], [Proposal], [Convert], [POLimit], [FU], [POApprove], [Tool], [Vehicle], [Estimates], [AwardEstimates], [BidResults], [Competitors], [JobHours], [Totals], [fDate], [PDA], [Tech], [MassResolvePDATickets], [ListsAdmin], [UserType]) 
VALUES (N'ADMIN', N'PASSWORD', 0, 2, CAST(0x00009A1600000000 AS DateTime), CAST(0x00009FCA00000000 AS DateTime), CAST(0x000092F400000000 AS DateTime), CAST(0x000092F400000000 AS DateTime), N'', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', 0, N'YYYYYY', 0, 0, N'YYYYYY', N'YYYYYY', CAST(0.00 AS Numeric(30, 2)), N'YYYYYY', 1, N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', N'YYYYYY', NULL, 1, CAST(0x00009A5200000000 AS DateTime), NULL, NULL, 1, 0, NULL)
  

DECLARE @Rol INT ;DECLARE @Chart INT ; SET IDENTITY_INSERT [dbo].[Chart] ON 

INSERT [dbo].[Chart] ([ID],[Acct], [fDesc], [Balance], [Type], [Sub], [Remarks], [Control], [InUse], [Detail], [CAlias], [Status], [Sub2], [DAT], [Branch], [CostCenter], [AcctRoot], [QBAccountID], [LastUpdateDate], [DefaultNo]) 
VALUES (1, N'D1000', N'Cash in Bank', CAST(0.00 AS Numeric(30, 2)), 6, N'', N'', 0, 1, 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'D1000')

INSERT INTO Rol (Name, Type, GeoLock) VALUES ('Cash in Bank', 2, 0)

SET @Rol = SCOPE_IDENTITY();

DECLARE @MAXBank INT = ISNULL((SELECT MAX(ISNULL(ID,0)) FROM Bank),0) + 1

INSERT [dbo].[Bank]	([ID], [fDesc], [Rol], [NBranch], [NAcct], [NRoute], [NextC], [NextD],   [NextE], [Rate], [CLimit], [Warn], [Recon], [Balance], [Status], [InUse],  [ACHFileHeaderStringA], [ACHFileHeaderStringB], [ACHFileHeaderStringC], [ACHCompanyHeaderString1],  [ACHCompanyHeaderString2], [ACHBatchControlString1], [ACHBatchControlString2], [ACHBatchControlString3],  [ACHFileControlString1], [Chart], [LastReconDate]) 
VALUES (@MAXBank, 'Cash in Bank', @Rol, '',	 '', '', 0, 0,   0, CAST(0.00 AS Numeric(30, 2)), CAST(0.00 AS Numeric(30, 2)), 1,  CAST(0.00 AS Numeric(30, 2)), CAST(0.00 AS Numeric(30, 2)), 0, 0, 			   NULL, NULL, NULL, NULL, 			   NULL, NULL, NULL, NULL, 			   NULL, 1, NULL)


INSERT [dbo].[Chart] ([ID], [Acct], [fDesc], [Balance], [Type], [Sub], [Remarks], [Control], [InUse], [Detail], [CAlias], [Status], [Sub2], [DAT], [Branch], [CostCenter], [AcctRoot], [QBAccountID], [LastUpdateDate], [DefaultNo]) VALUES (2, N'D1100', N'Undeposited Funds', CAST(0.00 AS Numeric(30, 2)), 0, N'', N'', 0, 1, 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'D1100')
INSERT [dbo].[Chart] ([ID], [Acct], [fDesc], [Balance], [Type], [Sub], [Remarks], [Control], [InUse], [Detail], [CAlias], [Status], [Sub2], [DAT], [Branch], [CostCenter], [AcctRoot], [QBAccountID], [LastUpdateDate], [DefaultNo]) VALUES (3, N'D1200', N'Accounts Receivable', CAST(0.00 AS Numeric(30, 2)), 0, N'', N'', 0, 1, 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'D1200')
INSERT [dbo].[Chart] ([ID], [Acct], [fDesc], [Balance], [Type], [Sub], [Remarks], [Control], [InUse], [Detail], [CAlias], [Status], [Sub2], [DAT], [Branch], [CostCenter], [AcctRoot], [QBAccountID], [LastUpdateDate], [DefaultNo]) VALUES (4, N'D2000', N'Accounts Payable', CAST(0.00 AS Numeric(30, 2)), 1, N'', N'', 0, 1, 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'D2000')
INSERT [dbo].[Chart] ([ID], [Acct], [fDesc], [Balance], [Type], [Sub], [Remarks], [Control], [InUse], [Detail], [CAlias], [Status], [Sub2], [DAT], [Branch], [CostCenter], [AcctRoot], [QBAccountID], [LastUpdateDate], [DefaultNo]) VALUES (5, N'D2100', N'Sales Tax Payable', CAST(0.00 AS Numeric(30, 2)), 1, N'', N'', 0, 1, 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'D2100')
INSERT [dbo].[Chart] ([ID], [Acct], [fDesc], [Balance], [Type], [Sub], [Remarks], [Control], [InUse], [Detail], [CAlias], [Status], [Sub2], [DAT], [Branch], [CostCenter], [AcctRoot], [QBAccountID], [LastUpdateDate], [DefaultNo]) VALUES (6, N'D3110', N'Stock', CAST(0.00 AS Numeric(30, 2)), 2, N'', N'', 0, 1, 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'D3110')
INSERT [dbo].[Chart] ([ID], [Acct], [fDesc], [Balance], [Type], [Sub], [Remarks], [Control], [InUse], [Detail], [CAlias], [Status], [Sub2], [DAT], [Branch], [CostCenter], [AcctRoot], [QBAccountID], [LastUpdateDate], [DefaultNo]) VALUES (7, N'D3130', N'Current Earnings', CAST(0.00 AS Numeric(30, 2)), 2, N'', N'', 0, 1, 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'D3130')
INSERT [dbo].[Chart] ([ID], [Acct], [fDesc], [Balance], [Type], [Sub], [Remarks], [Control], [InUse], [Detail], [CAlias], [Status], [Sub2], [DAT], [Branch], [CostCenter], [AcctRoot], [QBAccountID], [LastUpdateDate], [DefaultNo]) VALUES (8, N'D3140', N'Distribution', CAST(0.00 AS Numeric(30, 2)), 2, N'', N'', 0, 1, 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'D3140')
INSERT [dbo].[Chart] ([ID], [Acct], [fDesc], [Balance], [Type], [Sub], [Remarks], [Control], [InUse], [Detail], [CAlias], [Status], [Sub2], [DAT], [Branch], [CostCenter], [AcctRoot], [QBAccountID], [LastUpdateDate], [DefaultNo]) VALUES (9, N'D3920', N'Retained Earnings', CAST(0.00 AS Numeric(30, 2)), 2, N'', N'', 0, 1, 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'D3920')
INSERT [dbo].[Chart] ([ID], [Acct], [fDesc], [Balance], [Type], [Sub], [Remarks], [Control], [InUse], [Detail], [CAlias], [Status], [Sub2], [DAT], [Branch], [CostCenter], [AcctRoot], [QBAccountID], [LastUpdateDate], [DefaultNo]) VALUES (10, N'D6000', N'Bank Charges', CAST(0.00 AS Numeric(30, 2)), 5, N'', N'', 0, 1, 2, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'D6000')
INSERT [dbo].[Chart] ([ID], [Acct], [fDesc], [Balance], [Type], [Sub], [Remarks], [Control], [InUse], [Detail], [CAlias], [Status], [Sub2], [DAT], [Branch], [CostCenter], [AcctRoot], [QBAccountID], [LastUpdateDate], [DefaultNo]) VALUES (11, N'D9991', N'Purchase Orders', CAST(0.00 AS Numeric(30, 2)), 7, N'', N'', 0, 1, 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'D9991')
INSERT [dbo].[Chart] ([ID], [Acct], [fDesc], [Balance], [Type], [Sub], [Remarks], [Control], [InUse], [Detail], [CAlias], [Status], [Sub2], [DAT], [Branch], [CostCenter], [AcctRoot], [QBAccountID], [LastUpdateDate], [DefaultNo]) VALUES (12, N'D3100', N'Payroll Withholdings & Contributions (DF)', CAST(0.00 AS Numeric(30, 2)), 1, N'', N'', 0, 1, 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'D3100')
INSERT [dbo].[Chart] ([ID], [Acct], [fDesc], [Balance], [Type], [Sub], [Remarks], [Control], [InUse], [Detail], [CAlias], [Status], [Sub2], [DAT], [Branch], [CostCenter], [AcctRoot], [QBAccountID], [LastUpdateDate], [DefaultNo]) VALUES (13, N'D3101', N'Inventory Asset', CAST(0.00 AS Numeric(30, 2)), 0, N'', N'', 0, 1, 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'D3101')

SET IDENTITY_INSERT [dbo].[Chart] OFF
 

IF @@ERROR <> 0 AND @@TRANCOUNT > 0 BEGIN  	RAISERROR ('Error Occured', 16, 1)     ROLLBACK TRANSACTION      RETURN END

  COMMIT TRANSACTION
