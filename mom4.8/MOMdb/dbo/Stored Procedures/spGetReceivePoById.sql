CREATE PROCEDURE [dbo].[spGetReceivePoById]
	@RID int
AS
BEGIN
	SET NOCOUNT ON;

	CREATE TABLE #temp
	(
		RowID int IDENTITY(1,1),
		ID int,
		Line smallint,
		AcctID int,
		AcctNo varchar(120),
		Account varchar(8000),
		fDesc varchar(8000),
		Quan numeric(30,2),
		Price numeric(30,2),
		Amount numeric(30,2),
		JobID int,
		JobName varchar(100),
		PhaseID smallint,
		Phase varchar(255),
		Loc varchar(120),
		Usetax numeric(30,2), 
		UName varchar(100),
		UtaxGL varchar(100), 
		ReceivePO int,
		TypeID int,
		ItemID int,
		ItemDesc varchar(500),
		Ticket int,
		OpSq varchar(150),
		PrvIn numeric(30,2), 
		PrvInQuan numeric(30,2), 
		OutstandQuan numeric(30,2),
		OutstandBalance numeric(30,2),[STax] [int] NULL,
		[STaxName] [varchar](50) NULL,
		[STaxRate] [numeric](30, 4) NULL,
		[STaxAmt] [numeric](30, 4) NULL,
		[STaxGL] [int] NULL,
		[GSTRate] [numeric](30, 4) NULL,
		[GTaxAmt] [numeric](30, 4) NULL,
		[GSTTaxGL] [int] NULL,
		Warehouse varchar(50) ,
		WHLocID int,
		Warehousefdesc VARCHAR(500) NULL,
		Locationfdesc VARCHAR(500) NULL,
		OrderedQuan numeric(30,2),
		Ordered numeric(30,2)
	  )
	--DECLARE @count int

    SELECT p.PO, p.fDate, p.fDesc, p.Amount, p.Vendor, ro.Name As VendorName, p.Status, p.Due, p.ShipVia,           
			p.Terms, p.FOB, p.ShipTo, p.Approved, p.Custom1, p.Custom2, p.ApprovedBy, p.ReqBy, p.fBy, p.PORevision,    
			p.CourrierAcct, p.POReasonCode, r.ID, r.Ref, r.WB, r.Comments, r.Amount as ReceivedAmount, r.fDate as ReceiveDate, 
			ro.Address +', '+ CHAR(13)+ ro.City +', '+ ro.State+', '+ ro.Zip as Address   
			,v.Days as [Days],CASE WHEN ISNUMERIC(REPLACE(REPLACE(t.Name,'Net',''),'Days','')) <> 1 THEN 30 ELSE (REPLACE(REPLACE(t.Name,'Net',''),'Days','')) END as [Term]
			,ro.State as [State]
	FROM ReceivePO as r                             
		INNER JOIN PO as p ON r.PO=p.PO                 
		INNER JOIN Vendor as v ON p.Vendor = v.ID       
		INNER JOIN Rol as ro ON v.Rol = ro.ID 
		INNER JOIN tblTerms as t ON t.ID = p.Terms
	WHERE r.ID = @RID      

	INSERT INTO #temp(ID, Line, AcctID, AcctNo, fDesc, Quan, Price, Amount, JobID, JobName, PhaseID, Phase, Loc, Usetax, UName, UtaxGL, 
		ReceivePO, TypeID, ItemID, ItemDesc,Ticket,OpSq,PrvIn,PrvInQuan,OutstandQuan,OutstandBalance,STax ,STaxName ,STaxRate ,STaxAmt ,STaxGL ,GSTRate ,GTaxAmt ,GSTTaxGL,
		Warehouse,WHLocID,Warehousefdesc,Locationfdesc,OrderedQuan ,	Ordered )
	SELECT DISTINCT p.PO as ID, 
			rp.POLine as Line,
			p.GL As AcctID, 
			c.Acct +' - '+ c.fDesc As AcctNo, 
			p.fDesc,
			-- Thomas: replaced this code for fixing ES-563 in case of Ap-bills page
			--CASE rp.Quan 
			--	WHEN 0 THEN p.Quan 
			--	ELSE rp.Quan 
			--END AS Quan, 
			rp.Quan,
			p.Price, 
			CASE rp.Amount 
				WHEN 0 THEN (rp.Quan * p.Price) 
				ELSE rp.Amount 
			END as Amount,
			p.Job As JobID, 
			convert(nvarchar(50), p.Job) +', '+ j.fdesc as JobName,	
			p.Phase AS PhaseID, 
			CASE ISNULL(p.TypeID,0) When 0 THEN (select Type from BOMT where ID = b.Type)  ELSE (select Type from BOMT where ID = p.TypeID) END AS Phase,--bt.Type as Phase 
			rol.Name as Loc, 
			0 As Usetax, 
			'' As UName, 
			'' As UtaxGL, 
			rp.ReceivePO,
			ISNULL(p.TypeID,b.Type) as TypeID,
			p.Inv as ItemID,
			case 
				when b.Type = 1 then isnull(i.Name,'') 
				WHEN b.Type= 8 THEN isnull(i.Name,'')
				when b.Type = 2 then isnull(pr.fdesc,'') 
				else isnull(i.Name,'') 
			end as ItemDesc,
			CASE p.Ticket WHEN 0 THEN null ELSE p.Ticket END AS Ticket,
			jt.Code,
			isnull(p.Selected,0.00) as PrvIn,
			isnull(p.SelectedQuan,0.00) as PrvInQuan,
			isnull(p.BalanceQuan,p.Quan) as OutstandQuan,
			isnull(p.Balance, P.Amount) as OutstandBalance,
			0 ,Null ,0 ,0 ,0 ,0 ,0 ,0,
		--	0,v.STax ,
		--CASE WHEN ISNULL(v.STax,'') ='' THEN 0 WHEN ISNULL(v.STax,'') <> '' THEN (SELECT Rate FROM STax WHERE Name = v.STax AND Utype = 0)  END as STaxRate,0,
		--ISNULL((SELECT GL FROM STax WHERE Name = v.STax AND Utype = 0),0) AS STaxGL,0,0,0,
			P.WarehouseID,p.LocationID,

			isnull((SELECT NAME FROM Warehouse WHERE ID IN (P.WarehouseID)),'') as Warehousefdesc,
			isnull((SELECT Tag FROM Loc WHERE Loc IN (p.LocationID)),'') as Locationfdesc,
			p.Quan ,p.Amount 
		FROM ReceivePO AS r 
			RIGHT JOIN RPOItem AS rp on rp.ReceivePO = r.ID                 
			INNER JOIN POItem AS p ON p.Line = rp.POLine     
			LEFT JOIN Job as j ON p.Job=j.ID  
			LEFT JOIN JobTItem as jt ON jt.Line = p.Phase AND jt.Job = p.Job AND jt.Type in (1)
			LEFT JOIN BOM as b ON b.JobTItemID = jt.ID
			LEFT JOIN Chart as c ON c.ID = p.GL 
			LEFT JOIN Loc as l ON l.Loc = j.Loc 
			LEFT JOIN Rol as rol ON l.Rol = rol.ID 
			LEFT JOIN BOMT as bt ON bt.ID = b.Type
			LEFT JOIN Inv as i ON i.ID = p.Inv
			LEFT JOIN PRWage as pr ON pr.ID = b.Item
		WHERE r.ID = @RID and p.PO = r.PO          		           
		 
	--SELECT @count = count(*) FROM #temp
	
	--WHILE (@count < 4)
	--BEGIN
		--INSERT INTO #temp (ID, Line, AcctID, AcctNo, fDesc, Quan, Price, Amount, JobID, JobName, PhaseID, Phase, Loc, Usetax, UName, UtaxGL, ReceivePO,Ticket)
		--VALUES(0,(@count+1),0,'','', null,null,null,0,'',0,'','',null,'','',0,null);
		--SET @count = @count +1;
	--END
	SELECT * FROM #temp
END
GO