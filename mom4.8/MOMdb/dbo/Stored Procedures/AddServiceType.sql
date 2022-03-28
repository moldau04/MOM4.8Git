CREATE PROC AddServiceType
(   @Type varchar(15),
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
	@route        [varchar](1000),
	@Department   [varchar](100)
	)
AS
BEGIN

IF NOT EXISTS(SELECT 1 FROM LTYPE WHERE  TYPE =@Type)  
BEGIN 

INSERT INTO LTYPE (TYPE, FDESC, REMARKS, MATCHARGE,FREE, REG,OT,NT,DT, STATUS,     LocType , ExpenseGL, InterestGL, LaborWageC,InvID , EN , Route, Department) 

VALUES         (  @TYPE, @Description, @REMARKS,0, 0,   @RT, @OT, @NT, @DT, @STATUS , @LocType, @ExpenseGL, @InterestGL, @LaborWageC, @InvID , 0, @route, @Department)  

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

END
ELSE 
BEGIN 
RAISERROR ('SERVICE TYPE ALREADY EXISTS, PLEASE USE DIFFERENT SERVICE  !',16,1)
RETURN
END 
END