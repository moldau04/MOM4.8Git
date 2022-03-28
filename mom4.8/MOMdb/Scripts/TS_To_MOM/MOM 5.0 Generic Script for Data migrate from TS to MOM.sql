 /* MOM 5.0  Generic Script for Data migrate from TS to MOM5.0
             

         =========> Notes:---  Before run this script Please make sure is identity

		 Chart.ID , 
		 Estimatei.ID ,  
		 Inv.ID , 
		 Rol.ID ,
		 Emp.ID,
		 tblWork.ID,
		 Route.ID,
		 Terr.ID
		 Phone.ID,
		 JobTItem.ID,
		 JobT.ID,
		 Elev.ID,
		 PRwage.ID,
		 service and owner
		 column(ID) is identity .
		  

------> Elev AID newID()
		 =========> Please This script run only on time in TS databse to migrate data.

 */
----------------
---------------- Start Script for $$$ TS Schedule Frequency TO MOM Schedule Frequency  Convertion $$$
----------------
 
              

------ NEVER   ----   Step 1

UPDATE contract SET    scycle = -1 WHERE  scycle = 13

------ DAILTY   -----  Step 2
UPDATE contract SET    scycle = 13 WHERE  scycle = 0

-----  Monthly   ----- Step 3
UPDATE contract SET    scycle = 0 WHERE  scycle = 6


-----  Bi-Weekly ---   $$$$$NEW  -- 4
UPDATE contract SET    scycle = 6 WHERE  scycle = 4


----   Annually  ------ Step 5
UPDATE contract SET    scycle = 4 WHERE  scycle = 12 

------ Bi-Monthly ------Step 6
UPDATE contract SET    scycle = 1 WHERE  scycle = 8

-----  Quarterly -----Step 7
UPDATE contract SET    scycle = 2 WHERE  scycle = 9


-----   Semi-monthly--   $$$$$New---Step 8
UPDATE contract SET    scycle = 14 WHERE  scycle = 5

---- Weekly Condition ---$$$$$$$New-----Step 9
UPDATE contract SET    scycle = 5 WHERE  scycle = 3
 

----  Semi-Annually-----------------10
UPDATE contract SET    scycle = 3 WHERE  scycle = 11
 

----  Every 13 weeks New ----------11
UPDATE contract SET    scycle = 7 WHERE  scycle = 18


update c set c.status=2 from Contract c inner join job j on j.ID=c.Job WHERE j.Status=0 and c.Status=1 and c.OffService is not null


----------------
---------------- END Script for $$$ TS Schedule Frequency TO MOM Schedule Frequency  Convertion $$$
----------------



 ----------------
 ---------------- Start Script for Sales Module
 ----------------

 BEGIN
 print('---- Step 1  $$$$$ Update AssignedTo and Fuser For Existing Opportunity  $$$$$')

UPDATE lead SET lead.AssignedToID=Terr.ID,lead.fuser=terr.SDesc FROM lead
INNER JOIN tbluser ON tbluser.ID = lead.AssignedToID and tbluser.fUser=lead.fuser
INNER JOIN emp ON tbluser.fUser=emp.CallSign
INNER JOIN terr ON terr.SMan=emp.ID
WHERE lead.fuser <> (Terr.SDesc) and lead.AssignedToID <> (Terr.ID)
AND isnull(lead.fuser,'') <> '' and isnull(lead.AssignedToID,'') <> ''

print('----Step 2  $$$$ if AssignedTo is null then Update AssignedTo and Fuser For Existing Opportunity From Loc.Terr')
UPDATE l SET l.AssignedToID=terr.ID   ,l.fuser=terr.SDesc
FROM Lead l
INNER JOIN TicketD t on t.ID=l.TicketID
INNER JOIN loc  on t.Loc=loc.Loc
INNER JOIN terr on terr.ID=loc.Terr
WHERE isnull(l.AssignedToID,'')=''

 print('--- Step 3 $$$ Update EstimateUserId  For Existing Estimate From Opportunity table')
UPDATE Estimate set Estimate.EstimateUserId=lead.AssignedToID FROM Estimate 
INNER JOIN lead on lead.id=Estimate.Opportunity
WHERE Estimate.EstimateUserId is Null

  UPDATE  Estimate set EstimateUserId=(select top 1 ID from terr  where SMan=EmpID)  
  where EstimateUserId is Null and EmpID is not Null
   
PRINT('---------- Update ESTIMATE Contact Name')

UPDATE ESTIMATE SET Contact = ( SELECT TOP 1 ID FROM PHONE WHERE FDESC = Contact) where Contact not in ( SELECT cast(ID as varchar) FROM PHONE)
    
print('--------$$$$$$$$$  Billing Tab Script  $$$$$$$$$$$')



UPDATE EstimateI Set Type=1,Code=100

SET IDENTITY_INSERT BOM ON

INSERT INTO BOM(Type,BudgetExt,BudgetUnit,EstimateIId,ID,LabItem,LabRate,QtyRequired,UM)
SELECT 1 AS Type, EstimateI.Cost,EstimateI.Price,EstimateI.ID,(ROW_NUMBER() OVER (order by (select 1))+
(SELECT COALESCE(MAX(ID),0) FROM BOM)) AS ID,
(SELECT Wage FROM JobT WhERE ID=(SELECT Template FROM Estimate WHERE ID=EstimateI.Estimate)) 
AS LabItem,EstimateI.Rate,EstimateI.Quan,'Each' AS UM
FROM EstimateI

SET IDENTITY_INSERT BOM OFF


PRINT('--------$$$$$$$$$$$  MilestoneName Tab Script  $$$$$$$$$')

DECLARE db_cursor CURSOR FOR Select ID,Price from Estimate ; 
DECLARE @ID INT;
DECLARE @Price FLOAT;
DECLARE @EstimateItemID INT
OPEN db_cursor;
FETCH NEXT FROM db_cursor INTO @ID, @Price;
WHILE @@FETCH_STATUS = 0 
BEGIN 
 
PRINT('------insert EstimateI-----------')
INSERT INTO EstimateI (Estimate, Line, fDesc, Code, Type, Amount,AmountPer, OrderNo)	
VALUES  (@ID, 1, 'Revenue', 100, 0, @Price,100, 1)

SET @EstimateItemID = SCOPE_IDENTITY()

 print('------insert Milestone-----------')
INSERT INTO [dbo].[Milestone] ([EstimateIId], [MilestoneName],[CreationDate], [Type], [Amount])
VALUES (@EstimateItemID, 'Revenue', GETDATE (), 1, @Price)

FETCH NEXT FROM db_cursor INTO @ID, @Price;
END;
CLOSE db_cursor;
DEALLOCATE db_cursor;
 

print('------- For Estimate Company Name')
 
 
print('-------------  ACCOUNT:-')

 UPDATE
Estimate
SET
Estimate.CompanyName =(SELECT Name FROM ROL WHERE ID=(SELECT Rol FROM Owner WHERE ID=(SELECT TOP 1 Owner FROM Loc Where Loc=Estimate.LocID))),
Estimate.EstimateAddress = (SELECT Name FROM ROL WHERE ID=(SELECT Rol FROM Loc Where Loc= Estimate.LocID))
WHERE
Estimate.LocID!=0  and  CompanyName is null

print('-------------update   PROSPECT:-')
 UPDATE
Estimate
SET
Estimate.CompanyName = (SELECT Name FROM ROL WHERE ID=Estimate.RolID),
Estimate.EstimateAddress = (SELECT Name FROM ROL WHERE ID=Estimate.RolID)
WHERE
Estimate.LocID=0  and  CompanyName is null


print('--------------- Lead ')

 UPDATE Prospect SET Prospect.CustomerName = Rol.Name
FROM Prospect AS Prospect
INNER JOIN Rol AS Rol ON Prospect.Rol = Rol.ID
WHERE Prospect.CustomerName IS NULL OR Prospect.CustomerName=''


 print('----- This script will fix the Profit Issue.')

UPDATE Estimate Set MarkupPer=0.00 Where MarkupPer IS NULL

UPDATE EstimateI SET MMUAmt=Cost,LMUAmt=Labor where  MMUAmt<>Cost and LMUAmt<>Labor


print('--Update the OH Percentage')

UPDATE Estimate SET OHPer= CAST(ROUND(SubTotal1/Overhead,2) AS NUMERIC(36,2)) FROM Estimate Where Overhead!=0.00


END


 ----------------
 ----------------END Sales Module
 ----------------



 -----------------
 ---------------- Start Script for PO Module
 ----------------


 print('---------  Open PO ----------->')
update POItem 
set BalanceQuan=Quan, 
Balance=Amount  ,SelectedQuan=0,Selected=0 
where po not in (select PO from ReceivePO ) 
and BalanceQuan is null and Balance is null and SelectedQuan is null and Selected is null 
and po in (select PO from PO where Status=0) 

 ----------------
 ----------------END PO Module
 ----------------
  
print('loc set default terms')
UPDATE loc set DefaultTerms = Terms ;


UPDATE control set DBName = (SELECT DB_NAME());

print('Chart')
---------------- Chart table
  
if not exists (select 1 from chart where acct='D1000') INSERT [dbo].[Chart] ([Acct], [fDesc], [Balance], [Type], [Sub], [Remarks], [Control], [InUse], [Detail], [CAlias], [Status], [Sub2], [DAT], [Branch], [CostCenter], [AcctRoot], [QBAccountID], [LastUpdateDate], [DefaultNo]) 
VALUES (N'D1000', N'Cash in Bank', CAST(0.00 AS Numeric(30, 2)), 6, N'', N'', 0, 1, 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'D1000')

if not exists (select 1 from chart where acct='D1100') INSERT [dbo].[Chart] ([Acct], [fDesc], [Balance], [Type], [Sub], [Remarks], [Control], [InUse], [Detail], [CAlias], [Status], [Sub2], [DAT], [Branch], [CostCenter], [AcctRoot], [QBAccountID], [LastUpdateDate], [DefaultNo]) 
VALUES (N'D1100', N'Undeposited Funds', CAST(0.00 AS Numeric(30, 2)), 0, N'', N'', 0, 1, 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'D1100')

if not exists (select 1 from chart where acct='D1200') INSERT [dbo].[Chart] ([Acct], [fDesc], [Balance], [Type], [Sub], [Remarks], [Control], [InUse], [Detail], [CAlias], [Status], [Sub2], [DAT], [Branch], [CostCenter], [AcctRoot], [QBAccountID], [LastUpdateDate], [DefaultNo]) 
VALUES (N'D1200', N'Accounts Receivable', CAST(0.00 AS Numeric(30, 2)), 0, N'', N'', 0, 1, 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'D1200')

if not exists (select 1 from chart where acct='D2000') INSERT [dbo].[Chart] ([Acct], [fDesc], [Balance], [Type], [Sub], [Remarks], [Control], [InUse], [Detail], [CAlias], [Status], [Sub2], [DAT], [Branch], [CostCenter], [AcctRoot], [QBAccountID], [LastUpdateDate], [DefaultNo]) 
VALUES (N'D2000', N'Accounts Payable', CAST(0.00 AS Numeric(30, 2)), 1, N'', N'', 0, 1, 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'D2000')

if not exists (select 1 from chart where acct='D2100') INSERT [dbo].[Chart] ([Acct], [fDesc], [Balance], [Type], [Sub], [Remarks], [Control], [InUse], [Detail], [CAlias], [Status], [Sub2], [DAT], [Branch], [CostCenter], [AcctRoot], [QBAccountID], [LastUpdateDate], [DefaultNo]) 
VALUES (N'D2100', N'Sales Tax Payable', CAST(0.00 AS Numeric(30, 2)), 1, N'', N'', 0, 1, 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'D2100')

if not exists (select 1 from chart where acct='D3110') INSERT [dbo].[Chart] ([Acct], [fDesc], [Balance], [Type], [Sub], [Remarks], [Control], [InUse], [Detail], [CAlias], [Status], [Sub2], [DAT], [Branch], [CostCenter], [AcctRoot], [QBAccountID], [LastUpdateDate], [DefaultNo]) 
VALUES (N'D3110', N'Stock', CAST(0.00 AS Numeric(30, 2)), 2, N'', N'', 0, 1, 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'D3110')

if not exists (select 1 from chart where acct='D3130') INSERT [dbo].[Chart] ([Acct], [fDesc], [Balance], [Type], [Sub], [Remarks], [Control], [InUse], [Detail], [CAlias], [Status], [Sub2], [DAT], [Branch], [CostCenter], [AcctRoot], [QBAccountID], [LastUpdateDate], [DefaultNo])
 VALUES (N'D3130', N'Current Earnings', CAST(0.00 AS Numeric(30, 2)), 2, N'', N'', 0, 1, 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'D3130')

if not exists (select 1 from chart where acct='D3140') INSERT [dbo].[Chart] ([Acct], [fDesc], [Balance], [Type], [Sub], [Remarks], [Control], [InUse], [Detail], [CAlias], [Status], [Sub2], [DAT], [Branch], [CostCenter], [AcctRoot], [QBAccountID], [LastUpdateDate], [DefaultNo])
 VALUES (N'D3140', N'Distribution', CAST(0.00 AS Numeric(30, 2)), 2, N'', N'', 0, 1, 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'D3140')

if not exists (select 1 from chart where acct='D3920') INSERT [dbo].[Chart] ([Acct], [fDesc], [Balance], [Type], [Sub], [Remarks], [Control], [InUse], [Detail], [CAlias], [Status], [Sub2], [DAT], [Branch], [CostCenter], [AcctRoot], [QBAccountID], [LastUpdateDate], [DefaultNo])
 VALUES (N'D3920', N'Retained Earnings', CAST(0.00 AS Numeric(30, 2)), 2, N'', N'', 0, 1, 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'D3920')

if not exists (select 1 from chart where acct='D6000') INSERT [dbo].[Chart] ([Acct], [fDesc], [Balance], [Type], [Sub], [Remarks], [Control], [InUse], [Detail], [CAlias], [Status], [Sub2], [DAT], [Branch], [CostCenter], [AcctRoot], [QBAccountID], [LastUpdateDate], [DefaultNo]) 
VALUES (N'D6000', N'Bank Charges', CAST(0.00 AS Numeric(30, 2)), 5, N'', N'', 0, 1, 2, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'D6000')

if not exists (select 1 from chart where acct='D9991')INSERT [dbo].[Chart] ([Acct], [fDesc], [Balance], [Type], [Sub], [Remarks], [Control], [InUse], [Detail], [CAlias], [Status], [Sub2], [DAT], [Branch], [CostCenter], [AcctRoot], [QBAccountID], [LastUpdateDate], [DefaultNo]) 
VALUES (N'D9991', N'Purchase Orders', CAST(0.00 AS Numeric(30, 2)), 7, N'', N'', 0, 1, 0, NULL, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'D9991')

-------------


print('Trans update')
UPDATE Trans
SET 	Trans.Acct = Chart.ID FROM
Bank INNER JOIN Chart ON Bank.Chart = Chart.ID 
INNER JOIN Trans ON Trans.AcctSub = Bank.ID
WHERE Trans.Acct IN (SELECT ID FROM Chart WHERE DefaultNo = 'D1000') 
  
print('/** mass-insert-bankadj-for-ts-db **/')
     declare @batch int
     declare @fdate datetime
     declare @Internal int
     declare @fDesc varchar(8000)

	 DECLARE db_cursor CURSOR FOR 
 
	 SELECT Batch, Min(fDate) as fDate, Ref as Internal, fDesc 
					FROM Trans WHERE Type = 30
					GROUP BY Batch, Ref, fDesc
					ORDER BY batch
	 OPEN db_cursor  
	 FETCH NEXT FROM db_cursor INTO  @batch, @fDate, @Internal, @fDesc

	 WHILE @@FETCH_STATUS = 0
	 BEGIN

	 
	 INSERT INTO [dbo].[GLA] ([Ref],[fDate],[Internal],[fDesc],[Batch])
     VALUES ((SELECT ISNULL(MAX(Ref),0)+1 FROM GLA),@fdate,@Internal,@fDesc,@batch)

	 FETCH NEXT FROM db_cursor INTO @batch, @fDate, @Internal, @fDesc
	 END

	 CLOSE db_cursor  
	 DEALLOCATE db_cursor



	 --<----------Project Modules--------->


UPDATE JobTItem set OrderNo=Line where OrderNo is null

INSERT INTO BOM(JobTItemID,Type)

select   ID,Type from JobTItem where Job is not null and type=1 and isnull(id,0) not in (select isnull(JobTItemID,0) from BOM where JobTItemID is not null )


INSERT INTO Milestone(JobTItemID,Type,Amount,MilestoneName) 

select   ID,Type , Budget,fDesc from JobTItem where Job is not null and type=0  and isnull(id,0) not in (select isnull(JobTItemID,0) from Milestone where JobTItemID is not null )

update jt set jt.Code=jc.code  from JobTItem  jt inner join  JobCode jc  on cast(jc.id as varchar)=jt.Code  

update j set j.Custom20=l.Route from job j  inner join Contract c on c.job = j.id inner join loc l on l.loc=c.Loc


update  jobtitem set code =100 where  isnull(code,'')=''

update  jobtitem set code =100 where  code not in (select code from JobCode)
 

update m set m.MilestoneName=jc.fDesc

from Milestone  m inner join jobtitem jc on m.JobTItemID=jc.ID 

where jc.type=0 and  jc.fDesc='Revenue' and m.MilestoneName is null

--Reason 1: some invoice have status =1 but value Original not equal selected value in OpenAR
--Script to update status for Invoice

Update Invoice
set Status =3
Where status=1 and Ref in (select Ref from OpenAR where Original != Selected)
GO
Update OpenAR
set Selected=Original - ISNULL(Balance,0)
where Ref in (select ref from Invoice where status =3)

--Reason 2: some invoice have status =0 but does not have record in OpenAR and Trans has status not null.In this case invoice is paid
--Need to update status =1
BEGIN TRY
BEGIN TRANSACTION
DECLARE @c_tranID int
DECLARE @c_Ref int
DECLARE @Amount NUMERIC(30,2)
DECLARE @PayAmount NUMERIC(30,2)
DECLARE @Status varchar(30)

DECLARE cur_Inv CURSOR FOR 	
	select Ref,TransID from Invoice where status =0 
	and (select count(1) from OpenAR where Ref=Invoice.Ref)=0  and amount !=0 order by Loc
OPEN cur_Inv  
FETCH NEXT FROM cur_Inv INTO @c_Ref, @c_tranID
WHILE @@FETCH_STATUS = 0  
	BEGIN
		select @Amount=Amount ,@Status=Status from Trans where ID=@c_tranID
		select @PayAmount=sum(Amount)*-1 from Trans where status =@Status and  Type =6
		print @PayAmount
		if @PayAmount >=@Amount
		begin
			Update Invoice
			set Status=1
			where REf=@c_Ref
		end		
	FETCH NEXT FROM cur_Inv INTO @c_Ref, @c_tranID
	END	
CLOSE cur_Inv  
DEALLOCATE cur_Inv  
COMMIT
END TRY
BEGIN CATCH

CLOSE cur_Inv  
DEALLOCATE cur_Inv 

SELECT ERROR_MESSAGE() AS ErrorMessage; 
IF @@TRANCOUNT>0
ROLLBACK	
RAISERROR ('An error has occurred on this page.',16,1)
RETURN
END CATCH

--Case 3: Update status for these Invoice have Amount=0 and status=0
Update Invoice 
set status=1 
where status =0
and (select count(1) from OpenAR where Ref=Invoice.Ref) =0


 

update m set    m.LabRate = (jc.ETC /jc.BHours )
 
from bom  m INNER JOIN jobtitem jc on m.JobTItemID=jc.ID 

WHERE jc.type=1 

AND  isnull(jc.ETC,0) <> 0 AND  isnull(jc.BHours,0) <> 0  

 

 -----------------------------

 update m  set m.UM='Each',m.QtyRequired=1 , m.BudgetUnit= jc.Budget  , m.BudgetExt= jc.Budget 
 from bom  m INNER JOIN jobtitem jc on m.JobTItemID=jc.ID 
WHERE jc.type=1 AND  isnull(m.BudgetUnit,0)=0 AND  isnull(jc.Budget,0) <> 0 AND  isnull(m.QtyRequired,0) = 0
 

update m set m.Type=2 from bom  m INNER JOIN jobtitem jc on m.JobTItemID=jc.ID WHERE jc.type=1  AND   jc.fDesc  ='Labor' 

--- Transfer serv To InvID

update LType set InvID=Serv where serv is not null and InvID is null

update Vendor set Balance=0 where Balance is null
--02062020 --Import Collection Note ES -4478
insert into CollectionNotes (Notes,CreatedDate,CreatedBy,OwnerID,LocID)
select ColRemarks,GETDATE(), 'Admin',Owner,Loc from Loc where ColRemarks<>' '

Update Rol
set Country='US'
where type =0



--May 4, 2020, 12:08 PM

--fix issue for Lead

Update Rol
set Country='US'
where type =3 and Country='United States'
-- jaya acahrya 22/06/2020
--BEGIN TRANSACTION
Declare @quoteid int
DECLARE db_cursor CURSOR FOR 
			SELECT Ref
			FROM Quote
OPEN db_cursor
FETCH NEXT FROM db_cursor INTO @quoteid
WHILE @@FETCH_STATUS = 0
BEGIN  	

	Declare @EstimateNo int = 0
	Declare @MarkupVal numeric(30,2) = 0
	Declare @MarkupPer numeric(30,4) = 0
	Declare @TotalCost numeric(30,2) = 0

	select @MarkupVal = ISNULL(@MarkupVal, 0) + (ManualMarkup/100)*(Quan*Price), @TotalCost = @TotalCost + (Quan*Price)  from Quotei where ref = @quoteid
	if(@TotalCost != 0)
		select @MarkupPer = @MarkupVal/@TotalCost
	else select @MarkupPer = 0

	--SET @EstimateNo = (SELECT (MAX(ISNULL(ID,0))+1) FROM Estimate)
	--SET IDENTITY_INSERT Estimate ON  
	INSERT INTO Estimate
	(
	RolID,Name,fDesc,fDate,BDate
	,CompanyName,Type,Status,EmpID,Template
	,Remarks,LocID,Category,fFor,Cost
	,Hours,Labor,SubTotal1,Overhead,Profit
	,SubTotal2,Price,Job,Phone,Fax
	,Contact,EstTemplate,STaxRate,STax,SExpense
	,Quoted,Phase,Probability,Custom1,Custom2
	,CADExchange,EstimateNo,EstimateDate,EstimateBillAddress,EstimateAddress,EstimateEmail,EstimateCell
	,Cont,Opportunity,OHPer,MarkupPer,MarkupVal,CommissionVal
	,STaxName,STaxVal,MatExp,LabExp,OtherExp
	,SubToalVal,TotalCostVal,PretaxTotalVal,Discounted,Amount,BillRate
	,OT,RateTravel,DT,RateMileage,RateNT
	)
	select --@EstimateNo id
		--, 
		q.Rol rolid, '' name, 'Quote #' + Convert(varchar(50),q.Ref), fdate, fdate bdate
		, (select r.name from loc l inner join owner o on o.id = l.owner inner join rol r on r.id = o.rol where l.loc = q.loc) companyname, type, 7 Status, null Empid, Template
		, q.fdesc Remarks, loc locid, '' Category, 'ACCOUNT' ffor, null Cost
		, null Hours, null Labor, null SubTotal1, 0 OverHead, null Profit
		, null SubTotal2, Total price, Job, null Phone, null Fax
		, null Contact, 0 EstTemplate, TaxRate staxrate	, null	, null
		, 0	, null, null,null,null
		, null CADExchange, null, getdate() EstimateDate, (select l.Address from loc l  where l.loc = q.loc) EstimateBillAddress
		--, null
		, (select l.Tag from loc l  where l.loc = q.loc) EstimateAddress, null, null, 0 Cont, null opportunity
		, 0	, @MarkupPer*100 MarkupPer, @MarkupVal MarkupVal, 0
		--, 0
		, TaxRegion + ' / ' + Convert(varchar(10), TaxRate) STaxName, q.STax StaxVal, @TotalCost MatExp, 0, 0
		, @TotalCost SubToalVal	, @TotalCost TotalCostVal, q.Amount PreTaxTotalVal, null, Total Amount
		--, 0,0,0,0,0,0,0,0
		, 0,0,0,0,0,0
		--, ''
		--, null
		--, 0
	from Quote q 
	where q.ref = @quoteid

	select @EstimateNo = Scope_Identity() 
	--SET IDENTITY_INSERT Estimate off 
	
	DECLARE @JfDesc VARCHAR(255)
	DECLARE @JCode VARCHAR(10)
	DECLARE @BType SMALLINT
	DECLARE @QtyReq NUMERIC(30,2)
	DECLARE @UM VARCHAR(50)
	DECLARE @BudgetUnit NUMERIC(30,2)
	DECLARE @BudgetExt NUMERIC(30,2)
	DECLARE @MatItem INT
	DECLARE @MatMod NUMERIC(30,2)
	DECLARE @MatPrice NUMERIC(30,2)
	DECLARE @MatMarkup NUMERIC(30,2)
	DECLARE @STax TINYINT
	DECLARE @Currency VARCHAR(10)
	DECLARE @LabItem INT
	DECLARE @LabMod NUMERIC(30,2)
	DECLARE @LabExt NUMERIC(30,2)
	DECLARE @LabRate NUMERIC(30,2)
	DECLARE @LabHours NUMERIC(30,2)
	DECLARE @LabPrice NUMERIC(30,2)
	DECLARE @LabMarkup NUMERIC(30,2)
	DECLARE @LSTax SMALLINT
	DECLARE @SDate DATETIME
	DECLARE @VendorID INT
	DECLARE @Vendor VARCHAR(100)
	DECLARE @JLine INT = 0 
	DECLARE @OrderNo INT
	DECLARE @TotalExt NUMERIC(30,2)
	DECLARE @EstimateItemID INT

	DECLARE db_cursor_BOM CURSOR FOR 
				select
					fDesc--@JfDesc
					, 100 Code--@JCode
					, line + 1 line--@JLine
					, Phase Type--@BType
					, Quan--@QtyReq
					, Measure --@UM
					, Price--@BudgetUnit
					, Quan*Price Cost--@BudgetExt
					, 0--@MatMod
					, Amount MMUAmt--@MatPrice
					, ManualMarkup MMU--@MatMarkup
					, STax--@STax
					, null--@Currency
					, null--@MatItem
					, null--@Vendor 
					, null --@LabItem
					, 0--@LabMod
					, 0--@LabExt
					, 0--@LabRate
					, 0--@LabHours
					, null --@SDate
					, Quan*Price Amount--@TotalExt
					, 0 Labor--@LabPrice
					, 0--@LabMarkup
					, 0 --@LSTax
					, line--@OrderNo
				From Quotei qi 
				where qi.ref = @quoteid
			OPEN db_cursor_BOM  
			FETCH NEXT FROM db_cursor_BOM INTO 
					@JfDesc
					, @JCode
					, @JLine
					, @BType
					, @QtyReq
					, @UM
					, @BudgetUnit
					, @BudgetExt
					, @MatMod
					, @MatPrice
					, @MatMarkup
					, @STax
					, @Currency
					, @MatItem
					, @Vendor
					, @LabItem
					, @LabMod
					, @LabExt
					, @LabRate
					, @LabHours
					, @SDate
					, @TotalExt
					, @LabPrice
					, @LabMarkup
					, @LSTax
					, @OrderNo
			WHILE @@FETCH_STATUS = 0
			BEGIN  	
				SET @JLine = @JLine
				SET @OrderNo = @OrderNo
					
				INSERT INTO EstimateI
					(	Estimate, Line, fDesc, 
						Quan, Cost, Price, Hours, 
						Rate, Labor, Amount, STax, 
						Code, Vendor, Currency, Type, 
						MMU, MMUAmt, LMU, LMUAmt, 
						LStax, LMod, MMod, OrderNo
					)	
				VALUES
					(	@EstimateNo, @JLine, LEFT(@JfDesc,150), 
						@QtyReq, @BudgetExt, @BudgetUnit, @LabHours, 
						@LabRate, @LabExt, @TotalExt, @STax,
						@JCode,	@Vendor, @Currency, 1, 
						@MatMarkup, @MatPrice, @LabMarkup, @LabPrice, 
						@LSTax, @LabMod, @MatMod, @OrderNo
					)
					
				SET @EstimateItemID = SCOPE_IDENTITY()
				INSERT INTO [dbo].[BOM]
						([EstimateIId],[Type],[QtyRequired],[UM],[BudgetUnit],[BudgetExt],
							[LabItem],[MatItem],[LabRate],[SDate],[Vendor])
				VALUES (@EstimateItemID, @Btype, @QtyReq, @UM, @BudgetUnit, @BudgetExt, 
							@LabItem, @MatItem, @LabRate,@SDate,@Vendor)

				--------- Insert into tblInventoryWHTrans-------------
				--DECLARE @INV_WarehouseID varchar(50) = 'OFC';
				---- Inventory
				--IF EXISTS (SELECT 1 FROM Inv Where Type = 0 AND ID =@MatItem)
				--BEGIN
				--	INSERT INTO tblInventoryWHTrans(InvID, WarehouseID,LocationID ,Hand,Balance,fOrder,[Committed],[Available],Screen,ScreenID,Mode,Date,TransType,Batch,FDate)
				--	VALUES (@MatItem,'OFC',0,0,0,0,@QtyReq,0,'Estimate',@EstimateNo,'Add',GETDATE(),'In',0,GETDATE())
				--END
				--------- End Insert into tblInventoryWHTrans----------
				set @JfDesc          = null
				set @JCode		 = null
				set @JLine		 = null
				set @BType		 = null
				set @QtyReq		 = null
				set @UM			 = null
				set @BudgetUnit	 = null
				set @BudgetExt	 = null
				set @MatMod		 = null
				set @MatPrice		 = null
				set @MatMarkup	 = null
				set @STax			 = null
				set @Currency		 = null
				set @MatItem		 = null
				set @Vendor		 = null
				set @LabItem		 = null
				set @LabMod		 = null
				set @LabExt		 = null
				set @LabRate		 = null
				set @LabHours		 = null
				set @SDate		 = null
				set @TotalExt		 = null
				set @LabPrice		 = null
				set @LabMarkup	 = null
				set @LSTax		 = null
				set @OrderNo		 = null


				FETCH NEXT FROM db_cursor_BOM INTO
				@JfDesc
					, @JCode
					, @JLine
					, @BType
					, @QtyReq
					, @UM
					, @BudgetUnit
					, @BudgetExt
					, @MatMod
					, @MatPrice
					, @MatMarkup
					, @STax
					, @Currency
					, @MatItem
					, @Vendor
					, @LabItem
					, @LabMod
					, @LabExt
					, @LabRate
					, @LabHours
					, @SDate
					, @TotalExt
					, @LabPrice
					, @LabMarkup
					, @LSTax
					, @OrderNo
					
				END  
			CLOSE db_cursor_BOM  
			DEALLOCATE db_cursor_BOM
	FETCH NEXT FROM db_cursor INTO @quoteid
END  
CLOSE db_cursor 
DEALLOCATE db_cursor
--Rollback
--Commit
 
 ---- MID West ISSUE

    update custom set Label=0 where Name='InvGL' and Label=1

    update job set custom19 =null

    Update Invoice
    set BillTo= (select r.Address +', '+CHAR(13) + r.City + ', ' +r.State +' '+r.Zip from Loc l
    inner join Rol r on r.ID=l.Rol
    where l.loc=Invoice.Loc and r.type=4) where BillTo=''


    Update Chart set DefaultNo='D1200'where ID=1

    ALTER TABLE Ticketd alter column Mileage numeric(30, 2) NULL

	ALTER TABLE Ticketd alter column NT numeric(30, 2) NULL

	ALTER TABLE Ticketd alter column idRolCustomContact int NULL

    ALTER TABLE Ticketd alter column downtime numeric(30, 2) NULL
