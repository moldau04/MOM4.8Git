CREATE PROCEDURE  [dbo].[spInvoicingFromTicketScreen] (
@TicketID INT,  
@Workorderonly  INT=0,
@Project  INT=0,
@Reviewonly  INT=1,
@Combind  INT=0
)
 
AS   
	DECLARE @DefaultBillcode  VARCHAR(100);
	DECLARE @Defaultprice  VARCHAR(100);
	DECLARE @INVID  INT; 
	DECLARE @WorkOrder VARCHAR(100);
	DECLARE @JobId    INT;

	SELECT @WorkOrder=WorkOrder , @JobId=Job
	FROM  TicketD
    WHERE ID = @TicketID  

	  --------fetch Default Bill code And Rate
	SELECT @DefaultBillcode=i.NAME,
	@Defaultprice=isnull(i.Price1,0),
	@INVID=i.ID
	FROM   Job j
	INNER JOIN Inv i
	ON j.GLRev = i.ID
	WHERE  j.id = @JobId 


    DECLARE @Ctype varchar(100);
	DECLARE @Reg int ;
	DECLARE @OT int ;
	DECLARE @NT int ;
	DECLARE @DT int ;

	SELECT @Ctype=CType FROM   job  WHERE  id = @JobId
    SELECT @Reg=Reg FROM LType  WHERE  TYPE = (@Ctype);
	SELECT @OT=OT FROM LType  WHERE  TYPE = (@Ctype);
	SELECT @DT=DT FROM LType  WHERE  TYPE = (@Ctype);
	SELECT @NT=NT FROM LType  WHERE  TYPE = (@Ctype);  
	 
IF( isnull(@JobId,0) = 0)
	BEGIN
	
	------RT
    SELECT 0                      AS Ref,
           0                      AS line,
           0                      AS acct,
           0.00                   AS Quan,
           'RT'                   AS fDesc,
           0.00                   AS price,
           0.00                   AS amount,
           0.00                   AS stax,
           0                      AS Job,
           0                      AS JobItem,
           0                      AS TransID,
           ''                     AS Measure,
           0                      AS Disc,
           0.00                   AS STaxAmt,
           0.00                   AS pricequant,
           ''                     AS billcode,
           0                      AS code,
           0.00                   AS GTaxAmt,
		    cast( 0 as smallint)  AS INVType,
		   '' Warehouse,
		   0 WHLocID,
		  cast( 0 as smallint) InvStatus,
		  0 AS AStatus
     

    ------OT
 SELECT 0                         AS Ref,
           0                      AS line,
           0                      AS acct,
           0.00                   AS Quan,
           'OT'                   AS fDesc,
           0.00                   AS price,
           0.00                   AS amount,
           0.00                   AS stax,
           0                      AS Job,
           0                      AS JobItem,
           0                      AS TransID,
           ''                     AS Measure,
           0                      AS Disc,
           0.00                   AS STaxAmt,
           0.00                   AS pricequant,
           ''                     AS billcode,
           0                      AS code,
           0.00                   AS GTaxAmt,
		   cast( 0 as smallint)   AS INVType,
		   ''                     AS Warehouse,
		   0                      AS WHLocID,
		    cast( 0 as smallint)  AS InvStatus,
			0 AS AStatus
     

    ------DT
   SELECT 0                       AS Ref,
           0                      AS line,
           0                      AS acct,
           0.00                   AS Quan,
           'DT'                   AS fDesc,
           0.00                   AS price,
           0.00                   AS amount,
           0.00                   AS stax,
           0                      AS Job,
           0                      AS JobItem,
           0                      AS TransID,
           ''                     AS Measure,
           0                      AS Disc,
           0.00                   AS STaxAmt,
           0.00                   AS pricequant,
           ''                     AS billcode,
           0                      AS code,
           0.00                   AS GTaxAmt,
		    cast( 0 as smallint) INVType,
		   '' Warehouse,
		   0 WHLocID,
		    cast( 0 as smallint) InvStatus,
			0 AS AStatus

    ------NT
 SELECT 0                         AS Ref,
           0                      AS line,
           0                      AS acct,
           0.00                   AS Quan,
           'NT' fDesc,
           0.00 AS price,
           0.00                   AS amount,
           0.00                   AS stax,
           0                      AS Job,
           0                      AS JobItem,
           0                      AS TransID,
           ''                     AS Measure,
           0                      AS Disc,
           0.00                   AS STaxAmt,
           0.00                   AS pricequant,
           ''                   AS billcode,
           0                      AS code,
           0.00                      AS GTaxAmt,
		    cast( 0 as smallint) INVType,
		   '' Warehouse,
		   0 WHLocID,
		   cast( 0 as smallint) InvStatus,
		   0 AS AStatus
	 
	------TT
 SELECT 0                      AS Ref,
           0                      AS line,
           0                     AS acct,
           0.00                   AS Quan,
           'TT' fDesc,
           0.00 AS price,
           0.00                   AS amount,
           0.00                   AS stax,
           0                      AS Job,
           0                      AS JobItem,
           0                      AS TransID,
           ''                     AS Measure,
           0                      AS Disc,
           0.00                   AS STaxAmt,
           0.00                   AS pricequant,
           ''                   AS billcode,
           0                      AS code,
           0.00                     AS GTaxAmt,
		    cast( 0 as smallint) INVType,
		   '' Warehouse,
		   0 WHLocID,
		    cast( 0 as smallint) InvStatus,
			0 AS AStatus
	
	END
   ELSE IF( (SELECT len(isnull(CType,'')) FROM   job   WHERE  id = @JobId) = 0)
	BEGIN
	
	------RT
    SELECT 0                      AS Ref,
           0                      AS line,
           id                     AS acct,
           0.00                   AS Quan,
           'RT' fDesc,
           Isnull(i.Price1, 0.00) AS price,
           0.00                   AS amount,
           0.00                   AS stax,
           0                      AS Job,
           0                      AS JobItem,
           0                      AS TransID,
           ''                     AS Measure,
           0                      AS Disc,
           0.00                   AS STaxAmt,
           0.00                   AS pricequant,
           NAME                   AS billcode,
           0                      AS code,
           0.00                   AS GTaxAmt,
		   i.Type as INVType,
		   '' Warehouse,
		   0 WHLocID,
		   i.status as InvStatus,
		   0 AS AStatus
    FROM   Inv i
    WHERE  id = (@INVID)
    ORDER  BY billcode

    ------OT
    SELECT 0                      AS Ref,
           0                      AS line,
           id                     AS acct,
           0.00                   AS Quan,
           'OT' fDesc,
           Isnull(i.Price1, 0.00) AS price,
           0.00                   AS amount,
           0.00                   AS stax,
           0                      AS Job,
           0                      AS JobItem,
           0                      AS TransID,
           ''                     AS Measure,
           0                      AS Disc,
           0.00                   AS STaxAmt,
           0.00                   AS pricequant,
           NAME                   AS billcode,
           0                      AS code,
           0.00                      AS GTaxAmt,
		   i.Type as INVType,
		   '' Warehouse,
		   0 WHLocID,
		   i.status as InvStatus,
		   0 AS AStatus
    FROM   Inv i
    WHERE  id = (@INVID)
    ORDER  BY billcode
     

    ------DT
    SELECT 0                      AS Ref,
           0                      AS line,
           id                     AS acct,
           0.00                   AS Quan,
           'DT' fDesc,
           Isnull(i.Price1, 0.00) AS price,
           0.00                   AS amount,
           0.00                   AS stax,
           0                      AS Job,
           0                      AS JobItem,
           0                      AS TransID,
           ''                     AS Measure,
           0                      AS Disc,
           0.00                   AS STaxAmt,
           0.00                   AS pricequant,
           NAME                   AS billcode,
           0                      AS code,
           0.00                     AS GTaxAmt,
		   i.Type as INVType,
		   '' Warehouse,
		   0 WHLocID,
		   i.status as InvStatus,
		   0 AS AStatus
    FROM   Inv i
    WHERE  id = (@INVID)
    ORDER  BY billcode

    ------NT
    SELECT 0                      AS Ref,
           0                      AS line,
           id                     AS acct,
           0.00                   AS Quan,
           'NT' fDesc,
           Isnull(i.Price1, 0.00) AS price,
           0.00                   AS amount,
           0.00                   AS stax,
           0                      AS Job,
           0                      AS JobItem,
           0                      AS TransID,
           ''                     AS Measure,
           0                      AS Disc,
           0.00                   AS STaxAmt,
           0.00                   AS pricequant,
           NAME                   AS billcode,
           0                      AS code,
           0.00                     AS GTaxAmt,
		   i.Type as INVType,
		   '' Warehouse,
		   0 WHLocID,
		   i.status as InvStatus,
		   0 AS AStatus
    FROM   Inv i
    WHERE  id = (@INVID)
    ORDER  BY billcode
	 
	------TT
    SELECT 0                      AS Ref,
           0                      AS line,
           id                     AS acct,
           0.00                   AS Quan,
           i.fdesc  + ' (TT) '    AS fDesc,
           Isnull(i.Price1, 0.00) AS price,
           0.00                   AS amount,
           0.00                   AS stax,
           0                      AS Job,
           0                      AS JobItem,
           0                      AS TransID,
           ''                     AS Measure,
           0                      AS Disc,
           0.00                   AS STaxAmt,
           0.00                   AS pricequant,
           i.NAME                   AS billcode,
           0                      AS code,
           0.00                    AS GTaxAmt,
		   i.Type as INVType,
		   '' Warehouse,
		   0 WHLocID,
		   i.status as InvStatus,
		   0 AS AStatus
    FROM   Inv i
   WHERE  id = (@INVID)
    ORDER  BY billcode 
	
	END
	 
    ELSE----------------------->>>>>>>>>>>>>>>>>>>
	 
	BEGIN
	   

    ------RT
    SELECT 0                      AS Ref,
           0                      AS line,
           id                     AS acct,
           0.00                   AS Quan,
            i.fdesc  + ' (RT) '   AS fDesc,
           Isnull(i.Price1, 0.00) AS price,
           0.00                   AS amount,
           0.00                   AS stax,
           0                      AS Job,
           0                      AS JobItem,
           0                      AS TransID,
           ''                     AS Measure,
           0                      AS Disc,
           0.00                   AS STaxAmt,
           0.00                   AS pricequant,
           NAME                   AS billcode,
           0                      AS code,
           0.00                     AS GTaxAmt,
		   i.Type as INVType,
		   '' Warehouse,
		   0 WHLocID,
		   i.status as InvStatus,
		   0 AS AStatus
    FROM   Inv i
    WHERE  ID = (case when isnull(@Reg,0) = 0 then @INVID else @Reg end)
    ORDER  BY billcode

    ------OT
    SELECT 0                      AS Ref,
           0                      AS line,
           id                     AS acct,
           0.00                   AS Quan,
           i.fdesc  + ' (OT) ' AS   fDesc,
           Isnull(i.Price1, 0.00) AS price,
           0.00                   AS amount,
           0.00                   AS stax,
           0                      AS Job,
           0                      AS JobItem,
           0                      AS TransID,
           ''                     AS Measure,
           0                      AS Disc,
           0.00                   AS STaxAmt,
           0.00                   AS pricequant,
           NAME                   AS billcode,
           0                      AS code,
          0.00                    AS GTaxAmt,
		   i.Type as INVType,
		   '' Warehouse,
		   0 WHLocID,
		   i.status as InvStatus,
		   0 AS AStatus
    FROM   Inv i
    WHERE  ID = (case when isnull(@OT,0) = 0 then @INVID else @OT end)
    ORDER  BY billcode

    --------DT
    SELECT 0                      AS Ref,
           0                      AS line,
           id                     AS acct,
           0.00                   AS Quan,
           i.fdesc  + ' (DT) ' fDesc,
           Isnull(i.Price1, 0.00) AS price,
           0.00                   AS amount,
           0.00                   AS stax,
           0                      AS Job,
           0                      AS JobItem,
           0                      AS TransID,
           ''                     AS Measure,
           0                      AS Disc,
           0.00                   AS STaxAmt,
           0.00                   AS pricequant,
           NAME                   AS billcode,
           0                      AS code,
           0.00                      AS GTaxAmt,
		   i.Type as INVType,
		   '' Warehouse,
		   0 WHLocID,
		   i.status as InvStatus,
		   0 AS AStatus
    FROM   Inv i
    WHERE  ID = (case when isnull(@DT,0) = 0 then @INVID else @DT end)
    ORDER  BY billcode

    -------NT
    SELECT 0                      AS Ref,
           0                      AS line,
           id                     AS acct,
           0.00                   AS Quan,
           i.fdesc  + ' (NT) '    AS fDesc,
           Isnull(i.Price1, 0.00) AS price,
           0.00                   AS amount,
           0.00                   AS stax,
           0                      AS Job,
           0                      AS JobItem,
           0                      AS TransID,
           ''                     AS Measure,
           0                      AS Disc,
           0.00                   AS STaxAmt,
           0.00                   AS pricequant,
           NAME                   AS billcode,
           0                      AS code,
           0.00                   AS GTaxAmt,
		   i.Type                 AS INVType,
		   ''                     AS Warehouse,
		   0                      AS WHLocID,
		   i.status               AS InvStatus,
		   0                      AS AStatus
    FROM   Inv i
    WHERE  ID = (case when isnull(@NT,0) = 0 then @INVID else @NT end)
    ORDER  BY billcode
	 
	------TT
    SELECT 0                      AS Ref,
           0                      AS line,
           id                     AS acct,
           0.00                   AS Quan,
           i.fdesc  + ' (TT) '    AS fDesc,
           Isnull(i.Price1, 0.00) AS price,
           0.00                   AS amount,
           0.00                   AS stax,
           0                      AS Job,
           0                      AS JobItem,
           0                      AS TransID,
           ''                     AS Measure,
           0                      AS Disc,
           0.00                   AS STaxAmt,
           0.00                   AS pricequant,
           NAME                   AS billcode,
           0                      AS code,
           0.00                   AS GTaxAmt,
		   i.Type                 AS INVType,
		   ''                     AS Warehouse,
		   0                      AS WHLocID,
		   i.status               AS InvStatus,
		   0                      AS AStatus
    FROM   Inv i
    WHERE  ID = (case when isnull(@Reg,0) =0 then @INVID else @Reg end)
    ORDER  BY billcode
	 
	END

     --- Invoicing   For   Ticket
    BEGIN
 
    SELECT isnull(sum(Isnull(Reg, 0)),0) AS RT,
        isnull(sum(Isnull(OT, 0)),0)     AS OT,
        isnull(sum(Isnull(DT, 0)),0)     AS DT,
        isnull(sum(Isnull(NT, 0)),0)     AS NT,
        isnull(sum(Isnull(TT, 0)),0)     AS TT,
		isnull(sum((Isnull(othere, 0)) + (Isnull(Toll, 0)) + (Isnull(Zone, 0))),0)    AS expenses,
		isnull(sum((Isnull(EMile, 0)) - (Isnull(SMile, 0)) ),0)   AS mileage 
    FROM  TicketD
    WHERE  
	(      (@Project =1   and job=@JobId) 
	OR     (@Project =0   and @Workorderonly=1 and WorkOrder=@WorkOrder) 
	OR     (@Project =0   and @Workorderonly=0 and ID=@TicketID) 
	)
	AND   job=@JobId and Charge=1 and ClearCheck=@Reviewonly
	AND   isnull(Invoice,0) = 0 
    AND   isnull(ManualInvoice,0) = 0

	END
 
	  

    -------------------Get Job BillRate-----------------------------------------  
	/*
	 1. IF there are rates at the project level then use that rate if greater than zero. 
	
	 2. IF there are no rates at the project then use the rate at the location level if greater than zero.
	 3. IF there are no rates at the project and location level then use the rate at the customer level if greater than zero.
	*/
	IF( isnull(@JobId,0) = 0)
	begin
	SELECT 
	--RT
	0 AS BillRatePrice

	,
	--OT

	0 AS RateOTPrice

	,--DT 

	0 AS RateDTPrice 

	,--NT	
	
	0 AS RateNTPrice


	,--RateMileage

	0 AS RateMileagePrice

	,--RateTravelPrice 

	0 AS RateTravelPrice 
	END
	ELSE
	BEGIN
    SELECT 
	--RT
	case when isnull(job.BillRate,0) = 0 and isnull(loc.BillRate,0) = 0  and isnull(Owner.BillRate,0) = 0  and len(isnull(job.CType,'')) = 0 
	then @Defaultprice -- fetch default BillRate value from project
	
	when  isnull(job.BillRate,0) = 0 and isnull(loc.BillRate,0) = 0  and isnull(Owner.BillRate,0) = 0 
	then  (SELECT isnull(Price1,0) FROM Inv i  WHERE  ID = ((case when isnull(@Reg,0) = 0 then @INVID else @Reg end))) -- fetch service type BillRate value from job.CType 
	
	when isnull(job.BillRate,0) = 0 and isnull(loc.BillRate,0) = 0
	then Owner.BillRate
	
	when isnull(job.BillRate,0) = 0 
	then loc.BillRate
	
	else job.BillRate 
	end AS BillRatePrice

	,
	--OT

	case when isnull(job.RateOT,0) = 0 and isnull(loc.RateOT,0) = 0  and isnull(Owner.RateOT,0) = 0  and len(isnull(job.CType,'')) = 0 
	then @Defaultprice -- fetch default BillRate value from project
	
	when  isnull(job.RateOT,0) = 0 and isnull(loc.RateOT,0) = 0  and isnull(Owner.RateOT,0) = 0 
	then  (SELECT isnull(Price1,0) FROM Inv i  WHERE  ID = ( (case when isnull(@OT,0) = 0 then @INVID else @OT end))) -- fetch service type BillRate value from job.CType 
	
	when isnull(job.RateOT,0) = 0 and isnull(loc.RateOT,0) = 0
	then Owner.RateOT
	
	when isnull(job.RateOT,0) = 0 
	then loc.RateOT
	
	else job.RateOT 
	end AS RateOTPrice

	,--DT 

	case when isnull(job.RateDT,0) = 0 and isnull(loc.RateDT,0) = 0  and isnull(Owner.RateDT,0) = 0  and len(isnull(job.CType,'')) = 0 
	then @Defaultprice -- fetch default BillRate value from project
	
	when  isnull(job.RateDT,0) = 0 and isnull(loc.RateDT,0) = 0  and isnull(Owner.RateDT,0) = 0 
	then  (SELECT isnull(Price1,0) FROM Inv i  WHERE  ID = ((case when isnull(@DT,0) = 0 then @INVID else @DT end))) -- fetch service type BillRate value from job.CType 
	
	when isnull(job.RateDT,0) = 0 and isnull(loc.RateDT,0) = 0
	then Owner.RateDT
	
	when isnull(job.RateDT,0) = 0 
	then loc.RateDT
	
	else job.RateDT 
	end AS RateDTPrice 

	,--NT	
	
	case when isnull(job.RateNT,0) = 0 and isnull(loc.RateNT,0) = 0  and isnull(Owner.RateNT,0) = 0  and len(isnull(job.CType,'')) = 0 
	then @Defaultprice -- fetch default BillRate value from project
	
	WHEN  isnull(job.RateNT,0) = 0 and isnull(loc.RateNT,0) = 0  and isnull(Owner.RateNT,0) = 0 
	THEN  (SELECT isnull(Price1,0) FROM Inv i  WHERE  ID = ((case when isnull(@NT,0) = 0 then @INVID else @NT end))) -- fetch service type BillRate value from job.CType 
	
	WHEN isnull(job.RateNT,0) = 0 and isnull(loc.RateNT,0) = 0
	THEN Owner.RateNT
	
	WHEN isnull(job.RateNT,0) = 0 
	THEN loc.RateNT
	
	ELSE job.RateNT 
	END AS RateNTPrice


	,--RateMileage

	CASE WHEN isnull(job.RateMileage,0) = 0 and isnull(loc.RateMileage,0) = 0  and isnull(Owner.RateMileage,0) = 0  and len(isnull(job.CType,'')) = 0 
	THEN @Defaultprice -- fetch default BillRate value from project
	
	WHEN  isnull(job.RateMileage,0) = 0 and isnull(loc.RateMileage,0) = 0  and isnull(Owner.RateMileage,0) = 0 
	THEN  @Defaultprice -- fetch service type BillRate value from job.CType 
	
	WHEN isnull(job.RateMileage,0) = 0 and isnull(loc.RateMileage,0) = 0
	THEN Owner.RateMileage
	
	WHEN isnull(job.RateMileage,0) = 0 
	THEN loc.RateMileage
	
	ELSE job.RateMileage 
	END AS RateMileagePrice

	,--RateTravelPrice 

	CASE WHEN isnull(job.RateTravel,0) = 0 and isnull(loc.RateTravel,0) = 0  and isnull(Owner.RateTravel,0) = 0  and len(isnull(job.CType,'')) = 0 
	THEN @Defaultprice -- fetch default BillRate value from project
	
	WHEN  isnull(job.RateTravel,0) = 0 and isnull(loc.RateTravel,0) = 0  and isnull(Owner.RateTravel,0) = 0 
	THEN  @Defaultprice -- fetch service type BillRate value from job.CType 
	
	WHEN isnull(job.RateTravel,0) = 0 and isnull(loc.RateTravel,0) = 0
	THEN Owner.RateTravel
	
	WHEN isnull(job.RateTravel,0) = 0 
	THEN loc.RateTravel
	
	ELSE job.RateTravel 
	END AS RateTravelPrice 

    FROM   job
	INNER JOIN loc on loc.Loc=job.Loc
	INNER JOIN Owner on Owner.ID=loc.Owner
    WHERE  job.ID = @JobId
	END
	-------------------- Invoicing Only For once Ticket --------------
  
  select isnull(t.TicketsID,'0')as TicketsID  from (
    SELECT   (SELECT  STUFF((SELECT ', ' + CAST(ID AS VARCHAR(100))    
	FROM TicketD   
	WHERE  
	(      (@Project =1   and job=@JobId) 
	OR     (@Project =0   and @Workorderonly=1 and WorkOrder=@WorkOrder) 
	OR     (@Project =0   and @Workorderonly=0 and ID=@TicketID) 
	)
	AND   job=@JobId and Charge=1 and  ClearCheck=@Reviewonly
	AND   isnull(Invoice,0) = 0 
	AND   isnull(ManualInvoice,0) = 0
	FOR XML PATH(''), TYPE)
	.value('.','NVARCHAR(MAX)'),1,2,' ') 
	) as TicketsID  ) t

	-----------------Invoice for inventory used billable items on a ticket. 
  
	SELECT    
	0                      AS Ref,
	0                      AS line,
	t.Item                 AS acct,
	t.Quan                 AS Quan,
	t.fDesc                AS  fDesc,
	cast(isnull(t.amount,0)/isnull(t.quan,0) as money) AS price,
	t.amount               AS amount,
	0.00                   AS stax,
	0                      AS Job,
	0                      AS JobItem,
	0                      AS TransID,
	''                     AS Measure,
	0                      AS Disc,
	0.00                   AS STaxAmt,
	0.00                   AS pricequant,
	NAME + ' : Part'       AS billcode,
	0                      AS code,
	0.00                   AS GTaxAmt,
	i.Type                 AS INVType, 
	t.WarehouseID          AS Warehouse,
	t.LocationID           AS WHLocID,
	i.Status               AS InvStatus ,
	0                      AS AStatus
	from TicketI T
	INNER JOIN inv i on i.id=t.Item 
	where 
	t.Charge =1
	AND
	t.ticket    in (SELECT ID FROM TicketD   
	WHERE  
	(      (@Project =1   and job=@JobId) 
	OR     (@Project =0   and @Workorderonly=1 and WorkOrder=@WorkOrder) 
	OR     (@Project =0   and @Workorderonly=0 and ID=@TicketID) 
	)
	AND   job=@JobId and Charge=1 and  ClearCheck=@Reviewonly
	AND   isnull(Invoice,0) = 0 
	AND   isnull(ManualInvoice,0) = 0)