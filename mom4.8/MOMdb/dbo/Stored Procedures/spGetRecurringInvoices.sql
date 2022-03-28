CREATE PROCEDURE [dbo].[spGetRecurringInvoices]  
 	@fLoc   int  ,
	@fOwner int,
	@fMonth int,
	@fYear  int ,
	@Handel int,
	@FlagEN int,
	@UserID int,
	@GriDCust nvarchar(100) ='',
	@GriDLoc nvarchar(100) ='',
	@GriDLocAcc nvarchar(100) =''
 
 AS 

     -------------------------###Testing--------------------------> 
 --   Declare     
 --   @fLoc int=0,
	--@fOwner int=557,
	--@fMonth int=12,
	--@fYear int=2020,
	--@Handel int=0,
	--@FlagEN int=0,
	--@UserID int=0
	-------------------------###--------------------------> 

		declare @GSTRate numeric(30,2) = 0
		SET @GSTRate = ISNULL((SELECT CASE WHEN (SELECT Label FROM Custom WHERE Name = 'Country') = 1
									THEN 
										CONVERT(NUMERIC(30,2),(SELECT Label AS GSTRate FROM Custom WHERE Name = 'GSTRate'))
									ELSE 
										0.00
									END
										AS GSTRate),0)



  Create Table #Filterjobs(jobID int )

  INSERT INTO  #Filterjobs

  SELECT tp.Job from Contract tp

  INNER JOIN job j on j.id=tp.job

  INNER JOIN Loc l on l.Loc=tp.Loc

  INNER JOIN Owner o on o.ID=l.Owner

   INNER JOIN rol ro on ro.ID=o.Rol

  where 
  l.loc is 	not null and o.status=0	and l.status=0	and j.status=0	and tp.Status = 0 
  and
  isnull(j.SPHandle,0) = case  when @Handel   <> -1 then @Handel   else isnull(j.SPHandle,0)  end
  and
  j.Loc =      case  when @fLoc     <> 0  then @fLoc     else j.Loc        end
  and 
  j.Owner =    case  when @fOwner   <> 0  then @fOwner   else j.Owner      end

  and    l.Tag      like '%'+@GriDLoc+'%'

  and    l.ID       like '%'+@GriDLocAcc+'%'

  and    ro.Name    like '%'+@GriDCust+'%'
  
  --------------------------------
   Declare @fdate datetime
   Declare @fdesc varchar(max)
   Declare @Ref int
   Declare @amount numeric(30,2)
   Declare @stax numeric(30,4)
   Declare @taxregion varchar(25) 
   Declare @total numeric(30,2) 
   Declare @taxrate numeric(30,4)
   Declare @taxfactor numeric(30,2)
   Declare @taxable numeric(30,2) 
   Declare @type int
   Declare @job int
   Declare @loc int
   Declare @terms varchar(10)
   Declare @PO varchar(25)
   Declare @status int 
   Declare @batch int
   Declare @remarks varchar(50)
   Declare @gtax int
   Declare @worker varchar(75)
   Declare @taxregion2 varchar(50)
   Declare @taxrate2 numeric(30,4)
   Declare @billto varchar(1000)
   Declare @Idate datetime
   Declare @fuser varchar(10)
   Declare @EN int
   Declare @Company varchar(100)
   Declare @acct int
   Declare @Quan numeric(30,2)
   Declare @price numeric(30,2)
   Declare @Jobitem int
   Declare @measure varchar(10)
   Declare @fdescI varchar(100)
   Declare @Frequency varchar(50)
   Declare @Name varchar(25)
   Declare @customername varchar(75)
   Declare @locid varchar(50)
   Declare @locname varchar(75)
   Declare @credit int
   Declare @dworker varchar(50)
   Declare @bcycle int
   Declare @ServiceType varchar(15)
   Declare @EscType varchar(50)
   Declare @Lid varchar(75)
   Declare @ContractBill smallint
   Declare @chart int 
   Declare @custBilling smallint
   Declare @ItemDesc varchar(max)
   Declare @3yrDate datetime
   Declare @5yrDate datetime
   Declare @2yrDate datetime
   Declare @owner int
   Declare @OrgLoc int
   Declare @detailLevel int
   

Create Table     #temp(
    fdate datetime,
    fdesc varchar(max),
	Ref int,
    amount numeric(30,2),
    stax numeric(30,4),
	total numeric(30,2),
    taxregion varchar(25),
    taxrate numeric(30,4),
    taxfactor numeric(30,2),
    taxable numeric(30,2),
    type int,
    job int,
    loc int,
    terms varchar(10),
    PO varchar(25),
    status int ,
    batch int,
    remarks varchar(50),
    gtax int,
    worker varchar(75),
    taxregion2 varchar(50),
    taxrate2 numeric(30,4),
    billto varchar(1000),
    Idate datetime,
    fuser varchar(10),
	EN int,
	Company varchar(100),
    acct int,
	chart int,
    Quan numeric(30,2),
    price numeric(30,2),
    Jobitem int,
    measure varchar(10),
    fdescI varchar(100),
    Frequency varchar(50),
    Name varchar(25),
    customername varchar(75),
    locid varchar(50),
    locname varchar(75),
	credit int,
    dworker varchar(50),
    bcycle int,
    ServiceType varchar(15),
    EscType varchar(50),
	Lid varchar(75),
	ContractBill smallint,
	CustBilling smallint,
	ItemDesc varchar(max),
	Owner int,
	OrgLoc int,
	detailLevel int
)

Create Table     #tempMonthly(
    fdate datetime,
    fdesc varchar(max),
	Ref int,
    amount numeric(30,2),
    stax numeric(30,4),
	total numeric(30,2),
    taxregion varchar(25),
    taxrate numeric(30,4),
    taxfactor numeric(30,2),
    taxable numeric(30,2),
    type int,
    job int,
    loc int,
    terms varchar(10),
    PO varchar(25),
    status int ,
    batch int,
    remarks varchar(50),
    gtax int,
    worker varchar(75),
    taxregion2 varchar(50),
    taxrate2 numeric(30,4),
    billto varchar(1000),
    Idate datetime,
    fuser varchar(10),
		EN int,
	Company varchar(100),
    acct int,
	chart int,
    Quan numeric(30,2),
    price numeric(30,2),
    Jobitem int,
    measure varchar(10),
    fdescI varchar(100),
    Frequency varchar(50),
    Name varchar(25),
    customername varchar(75),
    locid varchar(50),
    locname varchar(75),
	credit int,
    dworker varchar(50),
    bcycle int,
    ServiceType varchar(15),
    EscType varchar(50),
	Lid varchar(75),
	ContractBill smallint,
	CustBilling smallint,
	ItemDesc varchar(max),
	Owner int,
	OrgLoc int,
	detailLevel int
)

Create Table     #tempYearly(
 fdate datetime,
    fdesc varchar(max),
	Ref int,
    amount numeric(30,2),
    stax numeric(30,4),
	total numeric(30,2),
    taxregion varchar(25),
    taxrate numeric(30,4),
    taxfactor numeric(30,2),
    taxable numeric(30,2),
    type int,
    job int,
    loc int,
    terms varchar(10),
    PO varchar(25),
    status int ,
    batch int,
    remarks varchar(50),
    gtax int,
    worker varchar(75),
    taxregion2 varchar(50),
    taxrate2 numeric(30,4),
    billto varchar(1000),
    Idate datetime,
    fuser varchar(10),
		EN int,
	Company varchar(100),
    acct int,
	chart int,
    Quan numeric(30,2),
    price numeric(30,2),
    Jobitem int,
    measure varchar(10),
    fdescI varchar(100),
    Frequency varchar(50),
    Name varchar(25),
    customername varchar(75),
    locid varchar(50),
    locname varchar(75),
	credit int,
    dworker varchar(50),
    bcycle int,
    ServiceType varchar(15),
    EscType varchar(50),
	Lid varchar(75),
	ContractBill smallint,
	CustBilling smallint,
	ItemDesc varchar(max),
	Owner int,
	OrgLoc int,
	detailLevel int
)

Create Table     #tempSelect(
 fdate datetime,
    fdesc varchar(max),
	Ref int,
    amount numeric(30,2),
    stax numeric(30,4),
	total numeric(30,2),
    taxregion varchar(25),
    taxrate numeric(30,4),
    taxfactor numeric(30,2),
    taxable numeric(30,2),
    type int,
    job int,
    loc int,
    terms varchar(10),
    PO varchar(25),
    status int ,
    batch int,
    remarks varchar(50),
    gtax int,
    worker varchar(75),
    taxregion2 varchar(50),
    taxrate2 numeric(30,4),
    billto varchar(1000),
    Idate datetime,
    fuser varchar(10),
		EN int,
	Company varchar(100),
    acct int,
	chart int,
    Quan numeric(30,2),
    price numeric(30,2),
    Jobitem int,
    measure varchar(10),
    fdescI varchar(100),
    Frequency varchar(50),
    Name varchar(25),
    customername varchar(75),
    locid varchar(50),
    locname varchar(75),
	credit int,
    dworker varchar(50),
    bcycle int,
    ServiceType varchar(15),
    EscType varchar(50),
	Lid varchar(75),
	ContractBill smallint,
	CustBilling smallint,
	ItemDesc varchar(max),
	Owner int,
	OrgLoc int,
	detailLevel int
)

Create Table     #tempFinal(
 fdate datetime,
    fdesc varchar(max),
	Ref int,
    amount numeric(30,2),
    stax numeric(30,4),
	total numeric(30,2),
    taxregion varchar(25),
    taxrate numeric(30,4),
    taxfactor numeric(30,2),
    taxable numeric(30,2),
    type int,
    job int,
    loc int,
    terms varchar(10),
    PO varchar(25),
    status int ,
    batch int,
    remarks varchar(50),
    gtax int,
    worker varchar(75),
    taxregion2 varchar(50),
    taxrate2 numeric(30,4),
    billto varchar(1000),
    Idate datetime,
    fuser varchar(10),
		EN int,
	Company varchar(100),
    acct int,
	chart int,
    Quan numeric(30,2),
    price numeric(30,2),
    Jobitem int,
    measure varchar(10),
    fdescI varchar(100),
    Frequency varchar(50),
    Name varchar(25),
    customername varchar(75),
    locid varchar(50),
    locname varchar(75),
	credit int,
    dworker varchar(50),
    bcycle int,
    ServiceType varchar(15),
    EscType varchar(50),
	Lid varchar(75),
	ContractBill smallint,
	CustBilling smallint,
	ItemDesc varchar(max),
	Owner int,
	OrgLoc int,
	detailLevel int
)

Create Table     #tempPooja(
     
    fdate datetime,
    fdesc varchar(max),
	Ref int,
    amount numeric(30,2),
    stax numeric(30,4),
	total numeric(30,2),
    taxregion varchar(25),
    taxrate numeric(30,4),
    taxfactor numeric(30,2),
    taxable numeric(30,2),
    type int,
    job int,
    loc int,
    terms varchar(10),
    PO varchar(25),
    status int ,
    batch int,
    remarks varchar(50),
    gtax int,
    worker varchar(75),
    taxregion2 varchar(50),
    taxrate2 numeric(30,4),
    billto varchar(1000),
    Idate datetime,
    fuser varchar(10),
		EN int,
	Company varchar(100),
    acct int,
	chart int,
    Quan numeric(30,2),
    price numeric(30,2),
    Jobitem int,
    measure varchar(10),
    fdescI varchar(100),
    Frequency varchar(50),
    Name varchar(25),
    customername varchar(75),
    locid varchar(50),
    locname varchar(75),
	credit int,
    dworker varchar(50),
    bcycle int,
    ServiceType varchar(15),
    EscType varchar(50),
	Lid varchar(75),
	ContractBill smallint,
	CustBilling smallint,
	ItemDesc varchar(max),
	Owner int,
	OrgLoc int,
	detailLevel int
)

     INSERT INTO   #temp
      SELECT   c.BStart  AS fdate, 
	   '' as fdesc,
	   (Select Top 1  I.Ref from  Invoice I Where  I.Job = (j.ID)) as Ref,
       c.BAmt as amount, 
       (((c.BAmt * 1)* st.Rate)/100) AS stax, 
       (isnull(st.Rate,0)+isnull(c.BAmt,0))   AS total, 
	   isnull(l.STax,'''') AS Taxregion,        
       (isnull(st.Rate,0))  AS taxrate, 
       100.00    AS taxfactor, 
       0.00  AS taxable,
       0  AS type, 
       j.ID AS job, 
       a.Loc,
       '' AS terms, 
       j.PO, 
       l.status, 
       '0'  AS batch, 
       'Recurring' AS remarks, 
       0  AS gtax, 
       j.Custom20  AS worker, 
       '' AS Taxregion2, 
       0.00  AS taxrate2, 
	   a.BillTo,
      c.BStart AS Idate, 
       '' AS fuser,
	   0 EN,
	   '' As Company, 
	   isnull(inv.ID,(select Top 1 ID from Inv where Name='recurring')) as acct,
	   isnull(c.chart,(select Top 1 SAcct from Inv where Name='recurring')) as chart,
       1.00                                AS Quan, 
       c.BAmt                              AS price, 
	   isnull((select top 1 line from jobtitem where type = 0 and job = c.Job order by line),0) AS jobitem, 
       isnull(inv.Measure,(SELECT Top 1 measure 
        FROM   Inv I 
        WHERE  I.Name = 'Recurring'))       AS measure, 
       CASE c.BCycle 
         WHEN 0 THEN 'Monthly recurring billing'
         WHEN 1 THEN 'Bi-Monthly recurring billing'
         WHEN 2 THEN 'Quarterly recurring billing'
		 WHEN 3 THEN '3 Times/Year recurring billing'
         WHEN 4 THEN 'Semi-Annually recurring billing'
         WHEN 5 THEN 'Annually recurring billing'
		 WHEN 7 THEN '3 Years recurring billing'
		 WHEN 8 THEN '5 Years recurring billing'
		 WHEN 9 THEN '2 Years recurring billing'
       END                                 AS fdescI, 
       CASE c.bcycle 
         WHEN 0 THEN 'Monthly'
         WHEN 1 THEN 'Bi-Monthly' 
         WHEN 2 THEN 'Quarterly'
         WHEN 3 THEN '3 Times/Year'
         WHEN 4 THEN 'Semi-Annually'
         WHEN 5 THEN 'Annually'
         WHEN 6 THEN 'Never'
		 WHEN 7 THEN '3 Years'
		 WHEN 8 THEN '5 Years'
		 WHEN 9 THEN '2 Years'
       END                                 Frequency, 
       st.Name, 
       (SELECT TOP 1 name 
        FROM   rol 
        WHERE  id = (SELECT TOP 1 rol 
                     FROM   owner 
                     WHERE  id = l.Owner)) AS customername, 
       a.ID as locid,
	   l.Tag as locname,
	   isnull(l.credit,0) as credit,
       (SELECT Name 
        FROM   Route ro 
        WHERE  ro.ID = j.Custom20)         AS dworker ,
        c.bcycle,
		lt.type as serviceType, 
		CASE c.BEscType 
         WHEN 0 THEN 'Commodity Index'
         WHEN 1 THEN 'Escalation' 
         WHEN 2 THEN 'Return'
         WHEN 3 THEN 'Manual'
       END AS EscType, 
		a.lid,
		a.ContractBill,
		isnull(o.Billing,0) as CustBilling, 
	 
		l.ID as ItemDesc,
		l.Owner,
		l.loc As OrgLoc,
		c.Detail  
FROM   Loc l 
       LEFT OUTER JOIN STax st   ON l.STax = st.Name 
       INNER JOIN Job j      ON l.Loc = j.Loc  and j.ID IN (select * from #Filterjobs)
       LEFT JOIN ltype lt  on lt.type=j.ctype
	   LEFT JOIN Inv inv on lt.InvID=inv.ID 
	   INNER JOIN Owner o on l.Owner = o.ID
	   --LEFT JOIN Rol r on r.ID = l.Rol
	   --LEFT JOIN Branch B on B.ID = r.EN  
       INNER JOIN Contract c   ON j.ID = c.Job       
		left join
		(select li.Loc,t.Job,t.lid,t.ContractBill,li.ID,li.Tag, 
			REPLACE(REPLACE(r.Address,CHAR(10),''),Char(13),'')+ ',' + Char(13)+CHAR(10) + r.City + ', ' + r.State + ' ' + r.Zip AS BillTo
			from loc as li right join 
		(select   c.Job, case when o.Billing=1 then o.Central else l.loc end as lid, isnull(l.Billing,0) As ContractBill
		from loc as l, Owner as o, Contract as c where l.loc =c.Loc and l.Owner=o.ID) t 
			ON li.Loc = t.lid
		Left join Rol r ON li.Rol = r.ID
		) a
		ON c.Job = a.Job    
		  
		 
 
DECLARE db_cursor CURSOR FOR 

select * from     #temp 

OPEN db_cursor  
FETCH NEXT FROM db_cursor INTO @fdate,@fdesc,@Ref,@amount,@stax,@total,@taxregion,@taxrate,@taxfactor,@taxable,@type,@job,@loc,@terms,@PO,@status,@batch,@remarks,
@gtax,@worker,@taxregion2,@taxrate2,@billto,@Idate,@fuser,@EN,@Company,@acct,@chart,@Quan,@price,@Jobitem,@measure,@fdescI,@Frequency,@Name,@customername,@locid,@locname,@credit, @dworker,@bcycle,@ServiceType, @EscType, @Lid, @ContractBill, @custBilling, @ItemDesc, @owner , @OrgLoc , @detailLevel

WHILE @@FETCH_STATUS = 0
BEGIN  				

	DECLARE @FlagConst INT = 12
		
		DECLARE @Flag INT = 1			
		DECLARE @intFlag INT
		DECLARE @intDayFlag INT	
		SET @intFlag =  DATEPART ( m , @fdate)
		
		WHILE (@Flag <=@FlagConst )
		BEGIN
		
		declare @sdate datetime
	
		
		set @sdate=DATEADD(m, @intFlag - MONTH( @fdate),  @fdate)
	
	
	 
		if @fdate=@sdate
		begin
		set @sdate=DATEADD(m, @intFlag - MONTH( @fdate),  @fdate)
		end
		else
		begin

		insert into     #tempMonthly	
		(
		 fdate ,    fdesc , Ref,   amount ,    stax , total,   taxregion ,        taxrate ,    taxfactor ,    taxable ,    type ,    job ,    loc ,    terms ,    PO ,
		     status  ,    batch ,    remarks ,    gtax ,    worker ,    taxregion2 ,    taxrate2 ,    billto ,    Idate ,    fuser , EN,  Company,  acct , chart,   Quan ,
		         price ,    Jobitem ,    measure ,    fdescI ,    Frequency ,    Name ,    customername,    locid,    locname ,  credit,   dworker ,    bcycle ,ServiceType, EscType, Lid,ContractBill, CustBilling, ItemDesc, Owner ,OrgLoc,detailLevel
		)	
		values
		(@sdate,@fdesc,@Ref,@amount,@stax,@total,@taxregion,@taxrate,@taxfactor,@taxable,@type,@job,@loc,@terms,@PO,@status,@batch,@remarks,
		@gtax,@worker,@taxregion2,@taxrate2,@billto,@sdate,@fuser,@EN,@Company,@acct,@chart,@Quan,@price,@Jobitem,@measure,@fdescI,@Frequency,@Name,@customername,@locid,@locname,@credit,@dworker,@bcycle ,@ServiceType, @EscType, @Lid, @ContractBill, @custBilling, @ItemDesc, @owner ,@OrgLoc,@detailLevel
		)
		end
		-- Monthly
		if(@bcycle=0)
		begin
		SET @intFlag = @intFlag + 1
		end		
		else
		-- Bi-Monthly
		if(@bcycle=1)
		begin
		SET @intFlag = @intFlag + 2
		end
		else
		-- Quarterly
		if(@bcycle=2)
		begin
		SET @intFlag = @intFlag + 3
		end
		else
	    -- 3 times a year
	    if(@bcycle=3)
		begin
		SET @intFlag = @intFlag + 4
		end

		-- Semiannually
		if(@bcycle=4)
		begin
		SET @intFlag = @intFlag + 6
		end
		else
		-- Annually
		if(@bcycle=5)
		begin
		SET @intFlag = @intFlag + 12
		end
		-- 3 years
		if(@bcycle=7)
		begin
		SET @intFlag = @intFlag + 36
		end
		-- 5 years
		if(@bcycle=8)
		begin
		SET @intFlag = @intFlag + 60
		end
		-- 2 years
		if(@bcycle=9)
		begin
		SET @intFlag = @intFlag + 24
		end
		SET @Flag = @Flag + 1
END			

		
FETCH NEXT FROM db_cursor INTO  @fdate,@fdesc,@Ref,@amount,@stax,@total,@taxregion,@taxrate,@taxfactor,@taxable,@type,@job,@loc,@terms,@PO,@status,@batch,@remarks,
@gtax,@worker,@taxregion2,@taxrate2,@billto,@Idate,@fuser,@EN,@Company,@acct,@chart,@Quan,@price,@Jobitem,@measure,@fdescI,@Frequency,@Name,@customername,@locid,@locname,@credit,@dworker,@bcycle ,@ServiceType, @EscType,@Lid, @ContractBill , @custBilling, @ItemDesc, @owner ,@OrgLoc,@detailLevel

END  

CLOSE db_cursor  
DEALLOCATE db_cursor
 
insert into     #tempFinal 
select * from     #temp 
  
union 
select * from     #tempMonthly
  
DECLARE db_cursor CURSOR FOR 

select * from     #tempFinal WHERE bcycle not in (7,8,9)

OPEN db_cursor  
FETCH NEXT FROM db_cursor INTO @fdate,@fdesc,@Ref,@amount,@stax,@total,@taxregion,@taxrate,@taxfactor,@taxable,@type,@job,@loc,@terms,@PO,@status,@batch,@remarks,
@gtax,@worker,@taxregion2,@taxrate2,@billto,@Idate,@fuser,@EN,@Company,@acct,@chart,@Quan,@price,@Jobitem,@measure,@fdescI,@Frequency,@Name,@customername,@locid,@locname,@credit,@dworker,@bcycle,@ServiceType, @EscType, @Lid, @ContractBill, @custBilling, @ItemDesc, @owner,@Orgloc,@detailLevel

WHILE @@FETCH_STATUS = 0
BEGIN  	
declare @IntYear int =DATEPART ( YEAR , @fdate) 
WHILE ( @IntYear <= @fYear )
		BEGIN
		set @IntYear = @IntYear+1

		set @sdate=DATEADD(YEAR,@IntYear-YEAR(@fdate),@fdate)
		
		insert into     #tempYearly 
		(
		 fdate ,    fdesc ,    Ref, amount ,    stax , total,   taxregion ,        taxrate ,    taxfactor ,    taxable ,    type ,    job ,    loc ,    terms ,    PO , status  ,    batch ,    remarks ,
		 gtax ,    worker ,    taxregion2 ,    taxrate2 ,    billto ,    Idate ,    fuser , EN,  Company,  acct ,  chart,   Quan , price ,    Jobitem ,    measure ,    fdescI ,    Frequency ,    Name ,  
		 customername,    locid,    locname , credit,   dworker ,    bcycle ,ServiceType, EscType, Lid, ContractBill, CustBilling, ItemDesc, Owner ,OrgLoc,detailLevel
		)	
		values
		(@sdate,@fdesc,@Ref,@amount,@stax,@total,@taxregion,@taxrate,@taxfactor,@taxable,@type,@job,@loc,@terms,@PO,@status,@batch,@remarks,
		@gtax,@worker,@taxregion2,@taxrate2,@billto,@sdate,@fuser,@EN,@Company,@acct,@chart,@Quan,@price,@Jobitem,@measure,@fdescI,@Frequency,@Name,
		@customername,@locid,@locname,@credit,@dworker,@bcycle ,@ServiceType, @EscType, @Lid,@ContractBill, @custBilling, @ItemDesc, @owner ,@OrgLoc,@detailLevel
		)
		End
		
						
		
FETCH NEXT FROM db_cursor INTO  @fdate,@fdesc,@Ref,@amount,@stax,@total,@taxregion,@taxrate,@taxfactor,@taxable,@type,@job,@loc,@terms,@PO,@status,@batch,@remarks,
@gtax,@worker,@taxregion2,@taxrate2,@billto,@Idate,@fuser,@EN,@Company,@acct,@chart,@Quan,@price,@Jobitem,@measure,@fdescI,@Frequency,@Name,@customername,@locid,@locname,@credit,@dworker,@bcycle ,@ServiceType, @EscType, @Lid, @ContractBill, @custBilling, @ItemDesc, @owner ,@OrgLoc,@detailLevel

END	

CLOSE db_cursor  
DEALLOCATE db_cursor

DECLARE db1_cursor CURSOR FOR 

select * from     #tempFinal WHERE bcycle in (7,8,9)

OPEN db1_cursor  
FETCH NEXT FROM db1_cursor INTO @fdate,@fdesc,@Ref,@amount,@stax,@total,@taxregion,@taxrate,@taxfactor,@taxable,@type,@job,@loc,@terms,@PO,@status,@batch,@remarks,
@gtax,@worker,@taxregion2,@taxrate2,@billto,@Idate,@fuser,@EN,@Company,@acct,@chart,@Quan,@price,@Jobitem,@measure,@fdescI,@Frequency,@Name,@customername,@locid,@locname,@credit,
@dworker,@bcycle,@ServiceType, @EscType, @Lid, @ContractBill, @custBilling, @ItemDesc, @owner , @OrgLoc, @detailLevel

WHILE @@FETCH_STATUS = 0
BEGIN  	

	if (@bcycle = 7)
	begin
		
		set @3yrDate = @fDate
		
		while DATEPART(yyyy, @3yrDate) <= @fYear
		begin
			set @3yrDate = dateadd(yy, 3, @3yrDate)
			insert into     #tempYearly 
				(
					fdate ,    fdesc ,    Ref, amount ,    stax , total,   taxregion ,        taxrate ,    taxfactor ,    taxable ,    type ,    job ,    loc ,    terms ,    PO ,
					status  ,    batch ,    remarks ,    gtax ,    worker ,    taxregion2 ,    taxrate2 ,    billto ,    Idate ,    fuser , EN , Company,  acct ,  chart,   Quan ,
					price ,    Jobitem ,    measure ,    fdescI ,    Frequency ,    Name ,    customername,    locid,    locname , credit,   dworker ,    bcycle ,
					ServiceType, EscType, Lid, ContractBill, CustBilling, ItemDesc, Owner ,OrgLoc,detailLevel
				)	
				values
				(@3yrDate,@fdesc,@Ref,@amount,@stax,@total,@taxregion,@taxrate,@taxfactor,@taxable,@type,@job,@loc,@terms,@PO,
				 @status,@batch,@remarks,@gtax,@worker,@taxregion2,@taxrate2,@billto,@3yrDate,@fuser,@EN,@Company,@acct,@chart,@Quan,
				 @price,@Jobitem,@measure,@fdescI,@Frequency,@Name,@customername,@locid,@locname,@credit,@dworker,@bcycle,
				 @ServiceType, @EscType, @Lid,@ContractBill, @custBilling, @ItemDesc, @owner , @OrgLoc,@detailLevel
				)
		end
	end
	else if (@bcycle = 8)
	begin
		set @5yrDate = @fDate
		while DATEPART(yyyy, @5yrDate) <= @fYear
		begin
			set @5yrDate = dateadd(yy, 5, @5yrDate)
			insert into     #tempYearly 
				(
					fdate ,    fdesc ,  Ref,  amount ,    stax , total,   taxregion ,        taxrate ,    taxfactor ,    taxable ,    type ,    job ,    loc ,    terms ,    PO ,
					status  ,    batch ,    remarks ,    gtax ,    worker ,    taxregion2 ,    taxrate2 ,    billto ,    Idate ,    fuser , EN, Company,  acct ,  chart,   Quan ,
					price ,    Jobitem ,    measure ,    fdescI ,    Frequency ,    Name ,    customername,    locid,    locname ,  credit ,  dworker ,    bcycle ,ServiceType, EscType,
					Lid, ContractBill, CustBilling, ItemDesc, Owner ,OrgLoc,detailLevel
				)	
				values
				(@5yrDate,@fdesc,@Ref,@amount,@stax,@total,@taxregion,@taxrate,@taxfactor,@taxable,@type,@job,@loc,@terms,@PO,
				 @status,@batch,@remarks,@gtax,@worker,@taxregion2,@taxrate2,@billto,@5yrDate,@fuser,@EN,@Company,@acct,@chart,@Quan,
				 @price,@Jobitem,@measure,@fdescI,@Frequency,@Name,@customername,@locid,@locname,@credit,@dworker,@bcycle ,@ServiceType, @EscType,
				 @Lid,@ContractBill, @custBilling, @ItemDesc, @owner ,@OrgLoc,@detailLevel
				)
		end
	end
	else if (@bcycle = 9)
	begin
		set @2yrDate = @fDate
		while DATEPART(yyyy, @2yrDate) <= @fYear
		begin
			set @2yrDate = dateadd(yy, 2, @2yrDate)
			insert into     #tempYearly 
				(
					fdate ,    fdesc ,   Ref, amount ,    stax , total,   taxregion ,        taxrate ,    taxfactor ,    taxable ,    type ,    job ,    loc ,    terms ,    PO ,
					status  ,    batch ,    remarks ,    gtax ,    worker ,    taxregion2 ,    taxrate2 ,    billto ,    Idate ,    fuser , EN, Company,  acct ,  chart,   Quan ,
					price ,    Jobitem ,    measure ,    fdescI ,    Frequency ,    Name ,    customername,    locid,    locname , credit ,   dworker ,    bcycle ,ServiceType, EscType,
					Lid, ContractBill, CustBilling, ItemDesc, Owner ,OrgLoc,detailLevel
				)	
				values
				(@2yrDate,@fdesc,@Ref,@amount,@stax,@total,@taxregion,@taxrate,@taxfactor,@taxable,@type,@job,@loc,@terms,@PO,
				 @status,@batch,@remarks,@gtax,@worker,@taxregion2,@taxrate2,@billto,@2yrDate,@fuser,@EN,@Company,@acct,@chart,@Quan,
				 @price,@Jobitem,@measure,@fdescI,@Frequency,@Name,@customername,@locid,@locname,@credit,@dworker,@bcycle ,@ServiceType, @EscType,
				 @Lid,@ContractBill, @custBilling, @ItemDesc, @owner ,@OrgLoc,@detailLevel
				)
		end
	end


FETCH NEXT FROM db1_cursor INTO  @fdate,@fdesc,@Ref,@amount,@stax,@total,@taxregion,@taxrate,@taxfactor,@taxable,@type,@job,@loc,@terms,@PO,@status,@batch,@remarks,
@gtax,@worker,@taxregion2,@taxrate2,@billto,@Idate,@fuser,@EN,@Company,@acct,@chart,@Quan,@price,@Jobitem,@measure,@fdescI,@Frequency,@Name,@customername,@locid,@locname,@credit,@dworker,
@bcycle ,@ServiceType, @EscType, @Lid, @ContractBill, @custBilling, @ItemDesc, @owner , @OrgLoc ,@detailLevel

END	

CLOSE db1_cursor  
DEALLOCATE db1_cursor


insert into       #tempSelect
select * from     #tempFinal
union
select * from     #tempYearly

  insert into #tempPooja
 select * from     #tempSelect where 
MONTH(fdate) =   @fMonth  
AND YEAR(fdate) = @fYear  


 select 
  (select top 1 * from  (
 (select top 1 Mihu.ref  from invoice Mihu 
 inner join Contract c on c.Job=Mihu.Job
 inner join Loc pl on pl.Loc=Mihu.Loc 
 inner join Owner po on po.ID=pl.Owner
where  po.ID=o.ID and year(Mihu.fDate) = (@fYear) and month(Mihu.fDate) = (@fMonth) 
and    mihu.Job= case o.Billing   when 1 then mihu.Job  else tp.job end   
and    isnull(Mihu.IsRecurring,0) = 1
 order by Mihu.IsRecurring desc 
) 
 union
  (select top 1 Mihu.ref  from invoice Mihu 
  inner join Contract c on c.Job=Mihu.Job
  inner join Loc pl on pl.Loc=Mihu.Loc 
  inner join Owner po on po.ID=pl.Owner
where  po.ID=o.ID and year(Mihu.fDate) = (@fYear) and month(Mihu.fDate) = (@fMonth) 
and    mihu.Loc= case l.Billing   when 1 then tp.loc  else mihu.Loc   end   
and tp.ContractBill=1 and tp.CustBilling=0
and    isnull(Mihu.IsRecurring,0) = 1
 order by Mihu.IsRecurring desc 
)   )as xxx
 )
as InvoiceID  , 
j.SRemarks ,
tp.* 
, (select top 1  st.Type from  STax st   where tp.Name = st.Name  ) TaxType
,CONVERT(NUMERIC(30,2), 0) as GST
,CONVERT(NUMERIC(30,2), 0) as PST
,  @GSTRate as GSTRate ,  isnull(c.ExpirationDate,01/01/1900) ExpirationDate

   from  #tempPooja  tp 
inner join job j on j.id=tp.job and j.ID in (select jj.jobID from #Filterjobs jj)
 inner join Contract c on c.Job=j.ID
inner join Loc l on l.Loc=tp.Loc
inner join Owner o on o.ID=l.Owner

  

drop table     #temp
drop table     #tempMonthly
drop table     #tempFinal
drop table     #tempYearly
drop table     #tempSelect
drop table     #tempPooja
drop table     #Filterjobs
GO 