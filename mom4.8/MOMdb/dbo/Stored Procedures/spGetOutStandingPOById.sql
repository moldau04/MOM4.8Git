CREATE PROCEDURE [dbo].[spGetOutStandingPOById]
	@PO int
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @count int;

    CREATE TABLE #temp
	(
		RowID int IDENTITY(1,1),
		ID int,
		Line smallint,
		AcctID int,
		fDesc varchar(max),
		TotalQuan numeric(30,2),
		Price numeric(30,2),
		TotalAmount numeric(30,2),
		Amount numeric(30,2),
		Quan numeric(30,2),
		JobID int,
		JobName varchar(150),
		PhaseID smallint,
		Phase varchar(255),
		Inv int,
		Freight numeric(30,2),
		Rquan numeric(30,2),
		Billed int,
		Ticket int,
		Loc varchar(150),
		AcctNo varchar(150),
		Due Datetime, 
		Usetax numeric(30,2), 
		UName varchar(20),
		UtaxGL varchar(20),
		ItemID int,
		ItemDesc varchar(max), 
		TypeID int,
		LocName varchar(75),
		WarehouseID varchar(5),
		LocationID int,
		OpSq varchar(150),
		PrvIn numeric(30,2), 
		PrvInQuan numeric(30,2), 
		OutstandQuan numeric(30,2),
		OutstandBalance numeric(30,2),
		[STax] [int] NULL,
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
	SELECT	p.PO, p.fDate, p.fDesc, p.Amount, p.Vendor, p.Status, convert(varchar(50),p.Due, 101) as Due, 
			p.ShipVia, p.Terms as PaymentTerms, p.FOB, p.ShipTo, p.Approved, p.Custom1, p.Custom2, p.ApprovedBy,
			p.ReqBy, p.fBy, p.PORevision, p.CourrierAcct, p.POReasonCode, r.Name AS VendorName,
			r.Address +', '+ CHAR(13)+ r.City +', '+ r.State+', '+ r.Zip as Address,
			r.City as VendorCity, r.State as VendorState, r.Zip as VendorZip, r.Address as VendorAddress, t.Name As Terms,
			r.State as [State],
			(CASE p.Status 
						WHEN 0 THEN 'Open'         
						WHEN 1 THEN 'Closed'                 
						WHEN 2 THEN 'Void'     
						WHEN 3 THEN 'Partial-Quantity'
						WHEN 4 THEN 'Partial-Amount' 
						WHEN 5 THEN 'Closed At Received PO'		END) AS StatusName , '' AS TC 
			,v.Days as [Days],CASE WHEN ISNUMERIC(REPLACE(REPLACE(t.Name,'Net',''),'Days','')) <> 1 THEN 30 ELSE (REPLACE(REPLACE(t.Name,'Net',''),'Days','')) END as [Term],
			v.Type VendorType
	FROM PO AS p, Vendor AS v, Rol AS r, tblterms As t 
		WHERE p.Vendor = v.ID AND v.Rol = r.ID AND t.ID = p.Terms AND p.PO=@PO  
						   
	DECLARE @IsReceived int = 0	
	SET @IsReceived= ISNULL((select TOP 1 1 from Receivepo rpo where rpo.Po = @PO), 0)
	--select @ReceivePOID
	--IF @IsReceived = 0	
	--BEGIN
	INSERT INTO #temp (ID, Line, AcctID, fDesc, TotalQuan, Price, TotalAmount,Amount,Quan, JobID, JobName, PhaseID, Phase, Inv, Freight,
	Rquan, Billed, Ticket, Loc, AcctNo, Due, Usetax, UName, UtaxGL, ItemID, ItemDesc, TypeID, LocName,WarehouseID,LocationID,OpSq,PrvIn,
	PrvInQuan,OutstandQuan,OutstandBalance,STax ,STaxName ,STaxRate ,STaxAmt ,STaxGL ,GSTRate ,GTaxAmt ,GSTTaxGL,Warehouse , 
	WHLocID,Warehousefdesc ,Locationfdesc,OrderedQuan ,	Ordered )
	SELECT DISTINCT p.PO as ID
		, p.Line
		, p.GL as AcctID
		, p.fDesc as fDesc
		, p.Quan  As TotalQuan
		, p.Price
		, p.Amount As TotalAmount 
		, CASE WHEN @IsReceived = 0 THEN isnull(p.Amount,0)
			ELSE
				(isnull(p.Amount,0)
				- isnull((select Sum(rpi.Amount)  from RPOItem rpi
										inner join Receivepo rpo on rpo.ID = rpi.ReceivePO
										where rpo.Po = @PO and rpi.POLine=p.Line
										group by rpi.POLine),0))
			END as Amount
		, CASE WHEN @IsReceived = 0 THEN isnull(p.Quan,0)
			ELSE
				(isnull(p.Quan,0)
				- isnull((select Sum(rpi.Quan)  from RPOItem rpi
										inner join Receivepo rpo on rpo.ID = rpi.ReceivePO
										where rpo.Po = @PO and rpi.POLine=p.Line
										group by rpi.POLine),0))
			END as Quan
		--, (isnull( p.Quan,0)-isnull((select Sum(Quan)  from RPOItem where ReceivePO  in (select ID from  Receivepo rr where po =@PO ) and RPOItem.POLine=p.Line group by RPOItem.POLine ),0) ) AS Quan
		, p.Job as JobID
		, convert(nvarchar(50),p.Job)+', '+j.fdesc as JobName, p.Phase as PhaseID
		, CASE ISNULL(p.TypeID,0) When 0 THEN (select Type from BOMT where ID = bom.TypeID)  ELSE (select Type from BOMT where ID = p.TypeID) END AS Phase --bom.Phase
		, p.Inv
		, p.Freight
		, p.Rquan
		, p.Billed
		,  CASE p.Ticket WHEN 0 THEN null ELSE p.Ticket END AS Ticket
		, r.Name as Loc, c.Acct+' - '+c.fDesc as AcctNo, p.Due, 0.00 as UseTax
		, '' as Uname
		, '' as UtaxGL
		, Inv as ItemID
		, CASE WHEN bom.Item is null or  bom.Item = '' THEN i.Name Else bom.Item  end AS Item
		, ISNULL(p.TypeID, bom.TypeID) As TypeID
		, (SELECT top 1 tag 
			FROM   Loc 
			WHERE  Loc = j.Loc) AS locname
		, p.WarehouseID
		, p.LocationID
		, bom.Code
		, isnull(p.Selected,0.00) as PrvIn,
		isnull(p.SelectedQuan,0.00) as PrvInQuan,
		isnull(p.BalanceQuan,p.Quan) as OutstandQuan,
		isnull(p.Balance, P.Amount) as OutstandBalance,
		0 ,Null ,0 ,0 ,0 ,0 ,0 ,0,
		--0,v.STax ,
		--CASE WHEN ISNULL(v.STax,'') ='' THEN 0 WHEN ISNULL(v.STax,'') <> '' THEN (SELECT Rate FROM STax WHERE Name = v.STax AND Utype = 0)  END as STaxRate,0,
		--ISNULL((SELECT GL FROM STax WHERE Name = v.STax AND Utype = 0),0) AS STaxGL,0,0,0,


		P.WarehouseID,p.LocationID,
		isnull((SELECT NAME FROM Warehouse WHERE ID IN (P.WarehouseID)),'') as Warehousefdesc,
		isnull((SELECT Tag FROM Loc WHERE Loc IN (p.LocationID)),'') as Locationfdesc,
		p.Quan ,p.Amount 
	FROM POItem as p 
		LEFT JOIN PO as POS ON POS.PO = p.PO
		LEFT JOIN Vendor AS v ON POS.Vendor = v.ID
		LEFT JOIN Job as j ON p.Job=j.ID  
		LEFT JOIN Chart as c ON c.ID = p.GL 
		LEFT JOIN Loc as l ON l.Loc = j.Loc 
		LEFT JOIN Rol as r ON l.Rol = r.ID
		LEFT JOIN Inv as i on i.ID = p.Inv
		LEFT JOIN (SELECT distinct jt.Line
						, jt.Job
						, CASE ISNULL(p.TypeID,0) When 0 THEN (select Type from BOMT where ID = b.Type) 
							ELSE (select Type from BOMT where ID = p.TypeID) 
							END AS Phase
						, CASE b.type WHEN 1 THEN isnull(i.Name,'') WHEN 2 THEN isnull(pr.fdesc,'') 
							ELSE isnull(i.Name,'')  
							END AS Item
						, isnull(p.TypeID ,b.Type) as TypeID
						, jt.Code
					FROM POItem as p LEFT JOIN Job as j ON p.Job=j.ID 
						LEFT JOIN JobTItem as jt ON jt.Line = p.Phase and isnull(jt.Job,0) = isnull(j.ID,0)
						INNER JOIN BOM as b ON b.JobTItemID = jt.ID  AND ISNULL(Jt.Type,0) != 2
						LEFT JOIN Inv as i on i.ID = p.Inv and b.Matitem=i.ID
						LEFT JOIN PRWage as pr ON pr.ID = p.Inv
					WHERE p.PO = @PO) AS bom ON bom.Line = p.Phase and isnull(bom.Job,0) = isnull(j.ID,0)
	WHERE p.PO=@PO  AND ISNULL(ForceClose,0) <> 1 And (isnull(p.BalanceQuan,0) <> 0 OR isnull(p.Balance,0) <> 0)
	--isnull(p.BalanceQuan,0) <> 0 And isnull(p.Balance,0) <> 0
	ORDER BY p.Line 

	SELECT @count = count(*) FROM #temp
 
	If (@count = 0)
	BEGIN
		INSERT INTO #temp (ID, Line, AcctID, fDesc, 
		TotalQuan, Price, TotalAmount,Amount,Quan, JobID, JobName, PhaseID, Phase, 
		Inv, Freight, Rquan, Billed, Ticket, Loc, AcctNo, Due, UseTax,
			Uname, UtaxGL, ItemID, ItemDesc, TypeID, locname,WarehouseID,LocationID,STax ,STaxName ,STaxRate ,STaxAmt ,STaxGL ,GSTRate ,GTaxAmt ,GSTTaxGL,Warehouse , 
			WHLocID,Warehousefdesc ,	Locationfdesc,OrderedQuan ,	Ordered)
		VALUES(0,(@count+1),0,'',
		null,null,null,null,null,0,'',0,'',
		0,0,0,0,null,'','',null, null
		,'','', null, '', 0,'','',0,0,NuLL,0,0,0,0,0,0,'',0,'','',0,0);
	END
	SELECT * FROM #temp 
END
GO

