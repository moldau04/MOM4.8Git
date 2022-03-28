CREATE PROCEDURE [dbo].[spGetVendorTransactionHistory]
	@Ref INT,
	@Type VARCHAR(50),
	@vendor INT, 
	@loc INT,
    @status VARCHAR(50),
	@transID INT
AS
BEGIN
	Declare @page varchar(100);
	DECLARE @CreditDisc NUMERIC(19,2)
--Declare @page varchar(100);
--DECLARE @Ref INT=4046;
--DECLARE @Type VARCHAR(50)='Bill';
--DECLARE @vendor INT=318;
--DECLARE @loc INT;
--DECLARE @status VARCHAR(50);
--DECLARE @transID INT=76655

DECLARE @BillTable Table (
line int,
fDate DateTime,
Ref VARCHAR(100),
fDesc VARCHAR(500),
Amount NUMERIC(19,2),
Type VARCHAR(50),
LinkTo NVARCHAR(250)
)


IF @Type='Bill'
	BEGIN	
	INSERT INTO @BillTable (line ,fDate ,Ref ,fDesc ,Amount ,Type ,LinkTo )
select  1 as line,fDate as fDate
		,CONVERT(VARCHAR(50), ref) as Ref ,
		case when CONVERT(VARCHAR(200), fDesc)='' then 'Input Only' else CONVERT(VARCHAR(Max), fDesc) End as fDesc,
		Amount as Amount,
		'Bill' as Type,
		CONCAT('addbills.aspx?id=',ID,@page) AS LinkTo 		
		FROM PJ i
		where Id=@Ref
		Union	
		
		SELECT 2 AS line,c.fDate
		,CONVERT(VARCHAR(50), c.ref) as Ref ,
		CONVERT(VARCHAR(Max), c.fDesc) as fDesc,
		p.paid *(-1) as Amount,
		'Check' AS Type,
		CONCAT('editcheck.aspx?id=', c.ID,@page) AS LinkTo
		 from CD as c   INNER JOIN Paid p ON c.ID = p.PITR
		 WHERE p.TRID = @transID
		 
		 --///////
		 DECLARE @BillAmount decimal(19,3)
		 SELECT @BillAmount = Amount FROM PJ WHERE ID = @Ref
		 IF @BillAmount > 0
		 BEGIN
			SELECT @CreditDisc = DISC FROM CreditPaid p WHERE FromPJID = (SELECT FromPJID FROM CreditPaid WHERE ToPJID = @Ref) AND ToPJID = @Ref
			
			INSERT INTO @BillTable (line ,fDate ,Ref ,fDesc ,Amount ,Type ,LinkTo )
			SELECT 3 AS line,p.fDate 
			--,CONVERT(VARCHAR(50), p.ref) as Ref ,
			,(SELECT CONVERT(VARCHAR(Max), Ref) FROM PJ WHERE ID = (SELECT FromPJID FROM CreditPaid WHERE ToPJID = @Ref) AND ToPJID = @Ref)  as Ref,
			CONVERT(VARCHAR(Max), p.fDesc) as fDesc,
			p.paid*-1  as Amount,
			'Bill' AS Type,
			CONCAT('addbills.aspx?id=',p.FromPJID,@page) AS LinkTo 
			FROM CreditPaid p WHERE FromPJID = (SELECT FromPJID FROM CreditPaid WHERE ToPJID = @Ref) AND ToPJID = @Ref

			IF ISNULL(@CreditDisc,0) >0
			BEGIN
				INSERT INTO @BillTable (line ,fDate ,Ref ,fDesc ,Amount ,Type ,LinkTo )
				SELECT 4 AS line,p.fDate 
				,CONVERT(VARCHAR(50), p.ref) as Ref ,
				--,(SELECT CONVERT(VARCHAR(Max), Ref) FROM PJ WHERE ID = (SELECT FromPJID FROM CreditPaid WHERE ToPJID = @Ref) AND ToPJID = @Ref)  as Ref,
				--CONVERT(VARCHAR(Max), p.fDesc) as fDesc,
				'Discount Taken',
				p.Disc*-1  as Amount,
				'Bill' AS Type,
				CONCAT('addbills.aspx?id=',p.ToPJID,@page) AS LinkTo 
				FROM CreditPaid p WHERE FromPJID = (SELECT FromPJID FROM CreditPaid WHERE ToPJID = @Ref) AND ToPJID = @Ref
			END
			SELECT * FROM @BillTable ORDER BY line
		 END
		 --///////
		 IF @BillAmount < 0
		 BEGIN
			SELECT * FROM @BillTable
			UNION
			SELECT 4 AS line,p.fDate 
			,CONVERT(VARCHAR(50), p.ref) as Ref ,
			CONVERT(VARCHAR(Max), p.fDesc) as fDesc,
			p.paid  as Amount,
			'Bill' AS Type,
			CONCAT('addbills.aspx?id=',p.FromPJID,@page) AS LinkTo 
			 FROM CreditPaid p WHERE FromPJID = @Ref AND ToPJID <> 0
		 END
END



IF @Type='Check'
	BEGIN

--Declare @page varchar(100);
--DECLARE @Ref INT=1091;
--DECLARE @Type VARCHAR(50)='Check';
--DECLARE @vendor INT=318;
--DECLARE @loc INT;
--DECLARE @status VARCHAR(50);
--DECLARE @transID INT=76655

select  1 AS line,c.fDate
		,CONVERT(VARCHAR(50), c.ref) as Ref ,
		CONVERT(VARCHAR(Max), c.fDesc) as fDesc,
		c.Amount *(-1) as Amount,
		'Check' AS Type,
		CONCAT('editcheck.aspx?id=', c.ID,@page) AS LinkTo
		 from CD as c   INNER JOIN Paid p ON c.ID = p.PITR
		 WHERE c.ID = @Ref
		 UNION
		 SELECT 2 as line,i.fDate as fDate
		,CONVERT(VARCHAR(50), i.ref) as Ref ,
		case when CONVERT(VARCHAR(200), i.fDesc)='' then 'Input Only' else CONVERT(VARCHAR(Max), i.fDesc) End as fDesc,
		Amount as Amount,
		'Bill' as Type,
		CONCAT('addbills.aspx?id=',i.ID,@page) AS LinkTo 		
		FROM PJ i INNER JOIN Paid p ON i.TRID = p.TRID
		where p.PITR=@Ref
END
END
  
		