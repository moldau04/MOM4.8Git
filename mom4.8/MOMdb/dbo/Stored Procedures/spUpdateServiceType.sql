CREATE PROCEDURE [dbo].[spUpdateServiceType]
	@Type varchar(50),
	@Description varchar(50),
	@Remarks varchar(8000),
	@InvID int,
	@Status smallint,
	@RT int,
	@OT int,
	@NT int,
	@DT int , 
	@LocType      [nvarchar](50) NULL,
	@ExpenseGL    [int] NULL,
	@InterestGL   [int] NULL,
	@LaborWageC   [int] NULL, 
	@route        [varchar](100),
	@Department   [varchar](100),
	@Flage int =0 ,
	@UpdatedBy varchar(100)
	
AS
BEGIN
	DECLARE @CurrStatus smallint;

	SELECT @CurrStatus = ISNULL(Status,0) FROM LType WHERE type= @Type
	
	IF(@Status = 1 AND @CurrStatus = 0)
	BEGIN
		--Check if this service type is using in some open project/equiment/contract/leadequipment or not
		IF EXISTS(  SELECT Top 1 1 from LType lt inner join JobT j on lt.Type = j.CType where lt.Type = @Type and j.Status <> 1 )
		BEGIN
			RAISERROR ('Service type is being used in Project Tempalte!',16,1)
			RETURN
		END  
			IF EXISTS(   SELECT Top 1 1 from LType lt inner join Elev j on lt.Type = j.Cat where lt.Type = @Type and j.Status <> 1)
		BEGIN
			RAISERROR ('Service type is being used in Equiment!',16,1)
			RETURN
		END 
	END

	
UPDATE  LType set fdesc=@Description
		, Remarks=@Remarks
		, InvID=@InvID
		, Reg=@RT 
		, OT=@OT 
		, NT=@NT
		, DT=@DT
		, Status=@Status  
		, LocType=  @LocType       
	    , ExpenseGL= @ExpenseGL   
	    , InterestGL= @InterestGL    
	    , LaborWageC= @LaborWageC   
		, Route =@route  
		, Department=@Department
	WHERE type= @Type
---------- Route ------------
DELETE FROM tblServicetypeRouteMapping WHERE type = @Type
IF ISNULL(@Route,'') != ''
BEGIN
    DECLARE @r TABLE
( 
  route VARCHAR(8000)
)
INSERT @r select  @route
INSERT INTO tblServicetypeRouteMapping (route,type)
SELECT   LTRIM(RTRIM(m.n.value('.[1]','int'))) AS route,@Type FROM
(
SELECT  CAST('<XMLRoot><RowData>' + REPLACE(route,',','</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x
FROM   @r )t CROSS APPLY x.nodes('/XMLRoot/RowData')m(n)
END
---------- Department ------------
DELETE FROM tblServicetypeDepartmentMapping WHERE type = @Type
IF ISNULL(@Department,'') != ''
BEGIN
    DECLARE @d TABLE
( 
  Depart VARCHAR(8000)
)
INSERT @d select  @Department
INSERT INTO tblServicetypeDepartmentMapping (Department,type)
SELECT  LTRIM(RTRIM(m.n.value('.[1]','int'))) AS Depart,@Type FROM
(
SELECT  CAST('<XMLRoot><RowData>' + REPLACE(Depart,',','</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x
FROM   @d )t CROSS APPLY x.nodes('/XMLRoot/RowData')m(n)
END

  IF (@Flage=1)

  BEGIN 


	INSERT @d select  @Department

	INSERT @r select  @route

	-- Adding logs for project
	-- Finance - General
	DECLARE @currInvExp int
	DECLARE @currInvServ int
	DECLARE @currWage int
	DECLARE @currGLInt int
	DECLARE @currCType varchar(10)
	DECLARE @InvExp int
	DECLARE @InvServ int
	DECLARE @Wage int
	DECLARE @GLInt int
	DECLARE @CType varchar(10)
	DECLARE @RefId int
	Declare @Screen varchar(50) = 'Project'
	Declare @ScreenContract varchar(50) = ''

	SELECT @Wage =       isnull(t1.LaborWageC,j1.WageC),
		@InvServ=       isnull(t1.InvID,j1.GLRev),
		@InvExp=          isnull(t1.ExpenseGL,j1.GL)  , 
		@GLInt=  isnull(t1.InterestGL,j1.InterestGL) ,
		@currInvExp = j1.GL
		, @currInvServ = j1.GLRev
		, @currWage = j1.WageC
		, @currGLInt = j1.InterestGL
		, @currCType = j1.CType
		, @RefId = j1.ID
	FROM LType  t1 
	INNER JOIN Job j1  on t1.Type=j1.CType   
	INNER JOIN loc on loc.loc=j1.Loc and loc.Type=@LocType
	WHERE t1.Type=@Type   
		and j1.Type in (SELECT Department FROM tblServicetypeDepartmentMapping WHERE Type = @Type)
		and Loc.Route in (SELECT Route FROM tblServicetypeRouteMapping WHERE Type = @Type)
		and isnull(t1.Route,'') <> ''  and isnull(t1.Department,'') <> ''

	IF EXISTS (SELECT c.Job from job j 
				inner join Contract c on c.Job=j.ID 
				where c.Job=@RefId)
	BEGIN
		SET @ScreenContract = 'Job'
	END
	-- Finance - ExpenseGL - @InvExp
	IF(@InvExp != @currInvExp)
	BEGIN
		DECLARE @currExpenseGL varchar(255), @logExpenseGL varchar(255)
		SET @currExpenseGL = ISNULL((SELECT c.fDesc FROM chart c WHERE c.ID = @currInvExp),'')
		SET @logExpenseGL = ISNULL((SELECT c.fDesc FROM chart c WHERE c.ID = @InvExp),'')
		EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Finance - ExpenseGL',@currExpenseGL,@logExpenseGL
		IF @ScreenContract != ''
		BEGIN
			EXEC log2_insert @UpdatedBy,@ScreenContract,@RefId,'Finance - ExpenseGL',@currExpenseGL,@ExpenseGL
		END
	END
	-- Finance - Inerest GL - @GLInt
	IF(@GLInt != @currGLInt)
	BEGIN
		DECLARE @currInerestGL varchar(255), @InerestGL varchar(255)
		SET @currInerestGL = ISNULL((SELECT c.fDesc FROM chart c WHERE c.ID = @currGLInt),'')
		SET @InerestGL = ISNULL((SELECT c.fDesc FROM chart c WHERE c.ID = @GLInt),'')
		EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Finance - Inerest GL',@currInerestGL,@InerestGL
		IF @ScreenContract != ''
		BEGIN
			EXEC log2_insert @UpdatedBy,@ScreenContract,@RefId,'Finance - Inerest GL',@currInerestGL,@InerestGL
		END
	END
	-- Finance - Billing Code - @InvServ
	IF(@InvServ != @currInvServ)
	BEGIN
		DECLARE @currBillCode varchar(255), @BillCode varchar(255)
		SET @currBillCode = ISNULL((SELECT INV.Name FROM Inv WHERE Inv.ID = @currInvServ),'')
		SET @BillCode = ISNULL((SELECT INV.Name FROM Inv WHERE Inv.ID = @InvServ),'')

		EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Finance - Billing Code',@currBillCode,@BillCode
		IF @ScreenContract != ''
		BEGIN
			EXEC log2_insert @UpdatedBy,@ScreenContract,@RefId,'Finance - Billing Code',@currBillCode,@BillCode
		END
	END
	-- Finance - Labor Wage - @Wage
	IF(@Wage != @currWage)
	BEGIN
		DECLARE @currLaborWage varchar(255), @LaborWage varchar(255)
		SET @currLaborWage = ISNULL((SELECT fDesc FROM PRWage WHERE ID = @currWage),'')
		SET @LaborWage = ISNULL((SELECT fDesc FROM PRWage WHERE ID = @Wage),'')
		EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Finance - Labor Wage',@currLaborWage,@LaborWage
		IF @ScreenContract != ''
		BEGIN
			EXEC log2_insert @UpdatedBy,@ScreenContract,@RefId,'Finance - Labor Wage',@currLaborWage,@LaborWage
		END
	END
	-- Finance - Service Type - @ctype
	IF(@ctype != @currCtype)
	BEGIN
		EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Finance - Service Type',@currCtype,@ctype
		IF @ScreenContract != ''
		BEGIN
			EXEC log2_insert @UpdatedBy,@ScreenContract,@RefId,'Finance - Service Type',@currCtype,@ctype
		END
	END
	-- End logs


	update j1 set 
		j1.WageC=       isnull(t1.LaborWageC,j1.WageC),
		j1.GLRev=       isnull(t1.InvID,j1.GLRev),
		j1.GL=          isnull(t1.ExpenseGL,j1.GL)  , 
		J1.InterestGL=  isnull(t1.InterestGL,j1.InterestGL) 
	FROM LType  t1 
	INNER JOIN Job j1  on t1.Type=j1.CType   
	inner join loc on loc.loc=j1.Loc and loc.Type=@LocType
	WHERE t1.Type=@Type   
		and j1.Type in (SELECT Department FROM tblServicetypeDepartmentMapping WHERE Type = @Type)
		and Loc.Route in (SELECT Route FROM tblServicetypeRouteMapping WHERE Type = @Type)
		and isnull(t1.Route,'') <> ''  and isnull(t1.Department,'') <> ''
  END

END





