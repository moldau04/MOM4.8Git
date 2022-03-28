CREATE PROCEDURE  [dbo].[Getbillcodesforticket2] (@TicketID INT, @JobId    INT)
AS
 
  Declare @DefaultBillcode  varchar(100);
  Declare @Defaultprice  varchar(100);
  Declare @INVID  int;

   --------fetch Default Bill code And Rate
  SELECT @DefaultBillcode=i.NAME,
         @Defaultprice=isnull(i.Price1,0),
         @INVID=i.ID
FROM   Job j
       INNER JOIN Inv i
               ON j.GLRev = i.ID
WHERE  j.id = @JobId



Declare @Ctype varchar(100);
	  Declare @Reg int ;
	  Declare @OT int ;
	  Declare @NT int ;
	  Declare @DT int ;

	  SELECT @Ctype=CType FROM   job  WHERE  id = @JobId
      SELECT @Reg=Reg FROM LType  WHERE  TYPE = (@Ctype);
	  SELECT @OT=OT FROM LType  WHERE  TYPE = (@Ctype);
	  SELECT @DT=DT FROM LType  WHERE  TYPE = (@Ctype);
	  SELECT @NT=NT FROM LType  WHERE  TYPE = (@Ctype); 
 
     if( (SELECT len(isnull(CType,'')) FROM   job   WHERE  id = @JobId) = 0)
	  begin
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
           0                      AS GTaxAmt,
		   i.Type as INVType,		   
		   '' Warehouse,
		   0 WHLocID,
		   i.status as InvStatus
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
           0                      AS GTaxAmt,
		    i.Type as INVType,
		   '' Warehouse,
		   0 WHLocID,
		   i.status as InvStatus
    FROM   Inv i
    WHERE  id = (@INVID)
    ORDER  BY billcode
     

    --------DT
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
           0                      AS GTaxAmt,
		    i.Type as INVType,
		   '' Warehouse,
		   0 WHLocID,
		   i.status as InvStatus
    FROM   Inv i
    WHERE  id = (@INVID)
    ORDER  BY billcode

    -------NT
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
           0                      AS GTaxAmt,
		    i.Type as INVType,
		   '' Warehouse,
		   0 WHLocID,
		   i.status as InvStatus
    FROM   Inv i
    WHERE  id = (@INVID)
    ORDER  BY billcode
	 
	------TT
    SELECT 0                      AS Ref,
           0                      AS line,
           id                     AS acct,
           0.00                   AS Quan,
           'TT' fDesc,
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
           0                      AS GTaxAmt,
		    i.Type as INVType,
		   '' Warehouse,
		   0 WHLocID,
		   i.status as InvStatus
    FROM   Inv i
   WHERE  id = (@INVID)
    ORDER  BY billcode 
	  end
	  else
	  begin
	   

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
           0                      AS GTaxAmt,
		    i.Type as INVType,
		   '' Warehouse,
		   0 WHLocID,
		   i.status as InvStatus
    FROM   Inv i
    WHERE  ID = (case when isnull(@Reg,0) = 0 then @INVID else @Reg end)
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
           0                      AS GTaxAmt,
		    i.Type as INVType,
		   '' Warehouse,
		   0 WHLocID,
		   i.status as InvStatus
    FROM   Inv i
    WHERE  ID = (case when isnull(@OT,0) = 0 then @INVID else @OT end)
    ORDER  BY billcode

    --------DT
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
           0                      AS GTaxAmt,
		    i.Type as INVType,
		   '' Warehouse,
		   0 WHLocID,
		   i.status as InvStatus
    FROM   Inv i
    WHERE  ID = (case when isnull(@DT,0) = 0 then @INVID else @DT end)
    ORDER  BY billcode

    -------NT
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
           0                      AS GTaxAmt,
		    i.Type as INVType,
		   '' Warehouse,
		   0 WHLocID,
		   i.status as InvStatus
    FROM   Inv i
    WHERE  ID = (case when isnull(@NT,0) = 0 then @INVID else @NT end)
    ORDER  BY billcode
	 
	------TT
    SELECT 0                      AS Ref,
           0                      AS line,
           id                     AS acct,
           0.00                   AS Quan,
           'TT' fDesc,
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
           0                      AS GTaxAmt,
		    i.Type as INVType,
		   '' Warehouse,
		   0 WHLocID,
		   i.status as InvStatus
    FROM   Inv i
    WHERE  ID = (case when isnull(@Reg,0) =0 then @INVID else @Reg end)
    ORDER  BY billcode
	 
	end

    SELECT Isnull(Reg, 0)         AS RT,
           Isnull(OT, 0)                      AS OT,
           Isnull(DT, 0)                      AS DT,
           Isnull(NT, 0)                      AS NT,
           Isnull(TT, 0)                      AS TT 
    FROM   TicketD
    WHERE  id = @TicketID
	
     ---------------------Get Job BillRate -----------------------------------------------------------------------------------
	/*
	 1. IF there are rates at the project level then use that rate if greater than zero. 
	 2. IF there are no rates at the project then use the rate at the location level if greater than zero.
	 3. IF there are no rates at the project and location level then use the rate at the customer level if greater than zero.
	*/

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
	
	when  isnull(job.RateNT,0) = 0 and isnull(loc.RateNT,0) = 0  and isnull(Owner.RateNT,0) = 0 
	then  (SELECT isnull(Price1,0) FROM Inv i  WHERE  ID = ((case when isnull(@NT,0) = 0 then @INVID else @NT end))) -- fetch service type BillRate value from job.CType 
	
	when isnull(job.RateNT,0) = 0 and isnull(loc.RateNT,0) = 0
	then Owner.RateNT
	
	when isnull(job.RateNT,0) = 0 
	then loc.RateNT
	
	else job.RateNT 
	end AS RateNTPrice


	,--RateMileage

	case when isnull(job.RateMileage,0) = 0 and isnull(loc.RateMileage,0) = 0  and isnull(Owner.RateMileage,0) = 0  and len(isnull(job.CType,'')) = 0 
	then @Defaultprice -- fetch default BillRate value from project
	
	when  isnull(job.RateMileage,0) = 0 and isnull(loc.RateMileage,0) = 0  and isnull(Owner.RateMileage,0) = 0 
	then  @Defaultprice -- fetch service type BillRate value from job.CType 
	
	when isnull(job.RateMileage,0) = 0 and isnull(loc.RateMileage,0) = 0
	then Owner.RateMileage
	
	when isnull(job.RateMileage,0) = 0 
	then loc.RateMileage
	
	else job.RateMileage 
	end AS RateMileagePrice

	,--RateTravelPrice 

	case when isnull(job.RateTravel,0) = 0 and isnull(loc.RateTravel,0) = 0  and isnull(Owner.RateTravel,0) = 0  and len(isnull(job.CType,'')) = 0 
	then @Defaultprice -- fetch default BillRate value from project
	
	when  isnull(job.RateTravel,0) = 0 and isnull(loc.RateTravel,0) = 0  and isnull(Owner.RateTravel,0) = 0 
	then  @Defaultprice -- fetch service type BillRate value from job.CType 
	
	when isnull(job.RateTravel,0) = 0 and isnull(loc.RateTravel,0) = 0
	then Owner.RateTravel
	
	when isnull(job.RateTravel,0) = 0 
	then loc.RateTravel
	
	else job.RateTravel 
	end AS RateTravelPrice 

    FROM   job
	inner join loc on loc.Loc=job.Loc
	inner join Owner on Owner.ID=loc.Owner
    WHERE  job.ID = @JobId

--alter PROCEDURE  [dbo].[Getbillcodesforticket2] (@TicketID INT, @JobId    INT)
--AS
 
--	Declare @DefaultBillcode  varchar(100);
--	Declare @Defaultprice  varchar(100);
--	Declare @INVID  int;

--	  --------fetch Default Bill code And Rate
--	SELECT @DefaultBillcode=i.NAME,
--	  @Defaultprice=isnull(i.Price1,0),
--	  @INVID=i.ID
--	  FROM   Job j
--	  INNER JOIN Inv i
--	  ON j.GLRev = i.ID
--	  WHERE  j.id = @JobId 


--    Declare @Ctype varchar(100);
--	Declare @Reg int ;
--	Declare @OT int ;
--	Declare @NT int ;
--	Declare @DT int ;

--	SELECT @Ctype=CType FROM   job  WHERE  id = @JobId
--    SELECT @Reg=Reg FROM LType  WHERE  TYPE = (@Ctype);
--	SELECT @OT=OT FROM LType  WHERE  TYPE = (@Ctype);
--	SELECT @DT=DT FROM LType  WHERE  TYPE = (@Ctype);
--	SELECT @NT=NT FROM LType  WHERE  TYPE = (@Ctype);  
	 

--   IF( (SELECT len(isnull(CType,'')) FROM   job   WHERE  id = @JobId) = 0)
--	BEGIN
--	------RT
--    SELECT 0                      AS Ref,
--           0                      AS line,
--           id                     AS acct,
--           0.00                   AS Quan,
--           'RT' fDesc,
--           Isnull(i.Price1, 0.00) AS price,
--           0.00                   AS amount,
--           0.00                   AS stax,
--           0                      AS Job,
--           0                      AS JobItem,
--           0                      AS TransID,
--           ''                     AS Measure,
--           0                      AS Disc,
--           0.00                   AS STaxAmt,
--           0.00                   AS pricequant,
--           NAME                   AS billcode,
--           0                      AS code,
--           0                      AS GTaxAmt
--    FROM   Inv i
--    WHERE  id = (@INVID)
--    ORDER  BY billcode

--    ------OT
--    SELECT 0                      AS Ref,
--           0                      AS line,
--           id                     AS acct,
--           0.00                   AS Quan,
--           'OT' fDesc,
--           Isnull(i.Price1, 0.00) AS price,
--           0.00                   AS amount,
--           0.00                   AS stax,
--           0                      AS Job,
--           0                      AS JobItem,
--           0                      AS TransID,
--           ''                     AS Measure,
--           0                      AS Disc,
--           0.00                   AS STaxAmt,
--           0.00                   AS pricequant,
--           NAME                   AS billcode,
--           0                      AS code,
--           0                      AS GTaxAmt
--    FROM   Inv i
--    WHERE  id = (@INVID)
--    ORDER  BY billcode
     

--    ------DT
--    SELECT 0                      AS Ref,
--           0                      AS line,
--           id                     AS acct,
--           0.00                   AS Quan,
--           'DT' fDesc,
--           Isnull(i.Price1, 0.00) AS price,
--           0.00                   AS amount,
--           0.00                   AS stax,
--           0                      AS Job,
--           0                      AS JobItem,
--           0                      AS TransID,
--           ''                     AS Measure,
--           0                      AS Disc,
--           0.00                   AS STaxAmt,
--           0.00                   AS pricequant,
--           NAME                   AS billcode,
--           0                      AS code,
--           0                      AS GTaxAmt
--    FROM   Inv i
--    WHERE  id = (@INVID)
--    ORDER  BY billcode

--    ------NT
--    SELECT 0                      AS Ref,
--           0                      AS line,
--           id                     AS acct,
--           0.00                   AS Quan,
--           'NT' fDesc,
--           Isnull(i.Price1, 0.00) AS price,
--           0.00                   AS amount,
--           0.00                   AS stax,
--           0                      AS Job,
--           0                      AS JobItem,
--           0                      AS TransID,
--           ''                     AS Measure,
--           0                      AS Disc,
--           0.00                   AS STaxAmt,
--           0.00                   AS pricequant,
--           NAME                   AS billcode,
--           0                      AS code,
--           0                      AS GTaxAmt
--    FROM   Inv i
--    WHERE  id = (@INVID)
--    ORDER  BY billcode
	 
--	------TT
--    SELECT 0                      AS Ref,
--           0                      AS line,
--           id                     AS acct,
--           0.00                   AS Quan,
--           'TT' fDesc,
--           Isnull(i.Price1, 0.00) AS price,
--           0.00                   AS amount,
--           0.00                   AS stax,
--           0                      AS Job,
--           0                      AS JobItem,
--           0                      AS TransID,
--           ''                     AS Measure,
--           0                      AS Disc,
--           0.00                   AS STaxAmt,
--           0.00                   AS pricequant,
--           NAME                   AS billcode,
--           0                      AS code,
--           0                      AS GTaxAmt
--    FROM   Inv i
--   WHERE  id = (@INVID)
--    ORDER  BY billcode 
--	  END
--	  ELSE-----------------------
--	  BEGIN
	   

--    ------RT
--    SELECT 0                      AS Ref,
--           0                      AS line,
--           id                     AS acct,
--           0.00                   AS Quan,
--           'RT' fDesc,
--           Isnull(i.Price1, 0.00) AS price,
--           0.00                   AS amount,
--           0.00                   AS stax,
--           0                      AS Job,
--           0                      AS JobItem,
--           0                      AS TransID,
--           ''                     AS Measure,
--           0                      AS Disc,
--           0.00                   AS STaxAmt,
--           0.00                   AS pricequant,
--           NAME                   AS billcode,
--           0                      AS code,
--           0                      AS GTaxAmt
--    FROM   Inv i
--    WHERE  ID = (case when isnull(@Reg,0) = 0 then @INVID else @Reg end)
--    ORDER  BY billcode

--    ------OT
--    SELECT 0                      AS Ref,
--           0                      AS line,
--           id                     AS acct,
--           0.00                   AS Quan,
--           'OT' fDesc,
--           Isnull(i.Price1, 0.00) AS price,
--           0.00                   AS amount,
--           0.00                   AS stax,
--           0                      AS Job,
--           0                      AS JobItem,
--           0                      AS TransID,
--           ''                     AS Measure,
--           0                      AS Disc,
--           0.00                   AS STaxAmt,
--           0.00                   AS pricequant,
--           NAME                   AS billcode,
--           0                      AS code,
--           0                      AS GTaxAmt
--    FROM   Inv i
--    WHERE  ID = (case when isnull(@OT,0) = 0 then @INVID else @OT end)
--    ORDER  BY billcode

--    --------DT
--    SELECT 0                      AS Ref,
--           0                      AS line,
--           id                     AS acct,
--           0.00                   AS Quan,
--           'DT' fDesc,
--           Isnull(i.Price1, 0.00) AS price,
--           0.00                   AS amount,
--           0.00                   AS stax,
--           0                      AS Job,
--           0                      AS JobItem,
--           0                      AS TransID,
--           ''                     AS Measure,
--           0                      AS Disc,
--           0.00                   AS STaxAmt,
--           0.00                   AS pricequant,
--           NAME                   AS billcode,
--           0                      AS code,
--           0                      AS GTaxAmt
--    FROM   Inv i
--    WHERE  ID = (case when isnull(@DT,0) = 0 then @INVID else @DT end)
--    ORDER  BY billcode

--    -------NT
--    SELECT 0                      AS Ref,
--           0                      AS line,
--           id                     AS acct,
--           0.00                   AS Quan,
--           'NT' fDesc,
--           Isnull(i.Price1, 0.00) AS price,
--           0.00                   AS amount,
--           0.00                   AS stax,
--           0                      AS Job,
--           0                      AS JobItem,
--           0                      AS TransID,
--           ''                     AS Measure,
--           0                      AS Disc,
--           0.00                   AS STaxAmt,
--           0.00                   AS pricequant,
--           NAME                   AS billcode,
--           0                      AS code,
--           0                      AS GTaxAmt
--    FROM   Inv i
--    WHERE  ID = (case when isnull(@NT,0) = 0 then @INVID else @NT end)
--    ORDER  BY billcode
	 
--	------TT
--    SELECT 0                      AS Ref,
--           0                      AS line,
--           id                     AS acct,
--           0.00                   AS Quan,
--           'TT' fDesc,
--           Isnull(i.Price1, 0.00) AS price,
--           0.00                   AS amount,
--           0.00                   AS stax,
--           0                      AS Job,
--           0                      AS JobItem,
--           0                      AS TransID,
--           ''                     AS Measure,
--           0                      AS Disc,
--           0.00                   AS STaxAmt,
--           0.00                   AS pricequant,
--           NAME                   AS billcode,
--           0                      AS code,
--           0                      AS GTaxAmt
--    FROM   Inv i
--    WHERE  ID = (case when isnull(@Reg,0) =0 then @INVID else @Reg end)
--    ORDER  BY billcode
	 
--	end

--   SELECT  sum(Isnull(Reg, 0))              AS RT,
--           sum(Isnull(OT, 0))               AS OT,
--           sum(Isnull(DT, 0))               AS DT,
--           sum(Isnull(NT, 0))               AS NT,
--           sum(Isnull(TT, 0))               AS TT,
--		  sum( (Isnull(othere, 0)) +  (Isnull(Toll, 0)) +  (Isnull(Zone, 0)) )  AS expenses,
--		  sum(  (Isnull(EMile, 0)) -  (Isnull(SMile, 0)) ) AS mileage 
--    FROM  TicketD
--    WHERE JOB = @JobId
--	AND   Charge=1
--    AND   isnull(Invoice,0) = 0 
--    AND   isnull(ManualInvoice,0) = 0 

--    -------------------Get Job BillRate-----------------------------------------  
--	/*
--	 1. IF there are rates at the project level then use that rate if greater than zero. 
--	 2. IF there are no rates at the project then use the rate at the location level if greater than zero.
--	 3. IF there are no rates at the project and location level then use the rate at the customer level if greater than zero.
--	*/

--   SELECT 
--	--RT
--	case when isnull(job.BillRate,0) = 0 and isnull(loc.BillRate,0) = 0  and isnull(Owner.BillRate,0) = 0  and len(isnull(job.CType,'')) = 0 
--	then @Defaultprice -- fetch default BillRate value from project
	
--	when  isnull(job.BillRate,0) = 0 and isnull(loc.BillRate,0) = 0  and isnull(Owner.BillRate,0) = 0 
--	then  (SELECT isnull(Price1,0) FROM Inv i  WHERE  ID = ((case when isnull(@Reg,0) = 0 then @INVID else @Reg end))) -- fetch service type BillRate value from job.CType 
	
--	when isnull(job.BillRate,0) = 0 and isnull(loc.BillRate,0) = 0
--	then Owner.BillRate
	
--	when isnull(job.BillRate,0) = 0 
--	then loc.BillRate
	
--	else job.BillRate 
--	end AS BillRatePrice

--	,
--	--OT

--	case when isnull(job.RateOT,0) = 0 and isnull(loc.RateOT,0) = 0  and isnull(Owner.RateOT,0) = 0  and len(isnull(job.CType,'')) = 0 
--	then @Defaultprice -- fetch default BillRate value from project
	
--	when  isnull(job.RateOT,0) = 0 and isnull(loc.RateOT,0) = 0  and isnull(Owner.RateOT,0) = 0 
--	then  (SELECT isnull(Price1,0) FROM Inv i  WHERE  ID = ( (case when isnull(@OT,0) = 0 then @INVID else @OT end))) -- fetch service type BillRate value from job.CType 
	
--	when isnull(job.RateOT,0) = 0 and isnull(loc.RateOT,0) = 0
--	then Owner.RateOT
	
--	when isnull(job.RateOT,0) = 0 
--	then loc.RateOT
	
--	else job.RateOT 
--	end AS RateOTPrice

--	,--DT 

--	case when isnull(job.RateDT,0) = 0 and isnull(loc.RateDT,0) = 0  and isnull(Owner.RateDT,0) = 0  and len(isnull(job.CType,'')) = 0 
--	then @Defaultprice -- fetch default BillRate value from project
	
--	when  isnull(job.RateDT,0) = 0 and isnull(loc.RateDT,0) = 0  and isnull(Owner.RateDT,0) = 0 
--	then  (SELECT isnull(Price1,0) FROM Inv i  WHERE  ID = ((case when isnull(@DT,0) = 0 then @INVID else @DT end))) -- fetch service type BillRate value from job.CType 
	
--	when isnull(job.RateDT,0) = 0 and isnull(loc.RateDT,0) = 0
--	then Owner.RateDT
	
--	when isnull(job.RateDT,0) = 0 
--	then loc.RateDT
	
--	else job.RateDT 
--	end AS RateDTPrice 

--	,--NT	
	
--	case when isnull(job.RateNT,0) = 0 and isnull(loc.RateNT,0) = 0  and isnull(Owner.RateNT,0) = 0  and len(isnull(job.CType,'')) = 0 
--	then @Defaultprice -- fetch default BillRate value from project
	
--	WHEN  isnull(job.RateNT,0) = 0 and isnull(loc.RateNT,0) = 0  and isnull(Owner.RateNT,0) = 0 
--	THEN  (SELECT isnull(Price1,0) FROM Inv i  WHERE  ID = ((case when isnull(@NT,0) = 0 then @INVID else @NT end))) -- fetch service type BillRate value from job.CType 
	
--	WHEN isnull(job.RateNT,0) = 0 and isnull(loc.RateNT,0) = 0
--	THEN Owner.RateNT
	
--	WHEN isnull(job.RateNT,0) = 0 
--	THEN loc.RateNT
	
--	ELSE job.RateNT 
--	END AS RateNTPrice


--	,--RateMileage

--	CASE WHEN isnull(job.RateMileage,0) = 0 and isnull(loc.RateMileage,0) = 0  and isnull(Owner.RateMileage,0) = 0  and len(isnull(job.CType,'')) = 0 
--	THEN @Defaultprice -- fetch default BillRate value from project
	
--	WHEN  isnull(job.RateMileage,0) = 0 and isnull(loc.RateMileage,0) = 0  and isnull(Owner.RateMileage,0) = 0 
--	THEN  @Defaultprice -- fetch service type BillRate value from job.CType 
	
--	WHEN isnull(job.RateMileage,0) = 0 and isnull(loc.RateMileage,0) = 0
--	THEN Owner.RateMileage
	
--	WHEN isnull(job.RateMileage,0) = 0 
--	THEN loc.RateMileage
	
--	ELSE job.RateMileage 
--	END AS RateMileagePrice

--	,--RateTravelPrice 

--	CASE WHEN isnull(job.RateTravel,0) = 0 and isnull(loc.RateTravel,0) = 0  and isnull(Owner.RateTravel,0) = 0  and len(isnull(job.CType,'')) = 0 
--	THEN @Defaultprice -- fetch default BillRate value from project
	
--	WHEN  isnull(job.RateTravel,0) = 0 and isnull(loc.RateTravel,0) = 0  and isnull(Owner.RateTravel,0) = 0 
--	THEN  @Defaultprice -- fetch service type BillRate value from job.CType 
	
--	WHEN isnull(job.RateTravel,0) = 0 and isnull(loc.RateTravel,0) = 0
--	THEN Owner.RateTravel
	
--	WHEN isnull(job.RateTravel,0) = 0 
--	THEN loc.RateTravel
	
--	ELSE job.RateTravel 
--	END AS RateTravelPrice 

--    FROM   job
--	INNER JOIN loc on loc.Loc=job.Loc
--	INNER JOIN Owner on Owner.ID=loc.Owner
--    WHERE  job.ID = @JobId

--	--if the project is set to Quoted then to pull the quoted amount from the project 
--	  --and mark all tickets invoiced with
--      --the invoice number on all the tickets. 
--	  --<option value="0">None</option>
--      --<option value="1">Quoted</option>
--	  --<option value="2">Maximum</option>

--	SELECT  (SELECT  STUFF((SELECT ', ' + CAST(ID AS VARCHAR(100))  
--          FROM TicketD  WHERE job=j.ID  and Charge=1
--          and (isnull(Invoice,0) = 0 AND  isnull(ManualInvoice,0) = 0 )  
--          FOR XML PATH(''), TYPE)
--         .value('.','NVARCHAR(MAX)'),1,2,' ') 
--          ) as TicketsID ,  
--		  j.ID as Job, isnull(j.PType,0) as BillType 
--		  FROM   job j  WHERE  j.id = @JobId 
 
--	SELECT 0     AS Ref,
--           0                       AS line,
--           ( SELECT ID FROM   Inv i WHERE  id = (@INVID))   AS acct,
--           cast (1.0  as numeric)    AS Quan,
--           JobTItem.fDesc         AS fDesc,
--           Isnull(JobTItem.Budget, 0.00) AS price,
--           0.00                      AS amount,
--           0.00                   AS stax,
--           0                      AS Job,
--           0                      AS JobItem,
--           0                      AS TransID,
--           ''                     AS Measure,
--           0                      AS Disc,
--           0.00                   AS STaxAmt,
--           0.00                   AS pricequant,
--           ( SELECT NAME FROM   Inv i WHERE  id = (@INVID))   AS billcode,
--           cast (JobTItem.Line  as int)                     AS code,
--           0                      AS GTaxAmt 
--	      FROM JobTItem   WHERE job=@JobId and TYPE=0
