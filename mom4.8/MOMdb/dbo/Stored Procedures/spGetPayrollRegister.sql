CREATE  PROCEDURE [dbo].[spGetPayrollRegister] 
@sDate Datetime,
@eDate Datetime,
@Emp   int=NULL,
@PageNumber Int = 1,
@PageSize Int = 0,
@SortBy NVARCHAR(50),
@SortType NVARCHAR(50)
AS
  BEGIN

  if @SortBy = ''
  BEGIN
	SET @SortBy = 'Ref '
  END
  if @SortType = ''
  BEGIN
	SET @SortType = 'Asc '
  END
  

  if @PageSize = 0
  BEGIN
  SELECT @PageSize = Count(1) FROM PRReg WHERE fDate >= @sDate AND fDate <= @eDate  
  END

      DECLARE @SQLQuery VARCHAR(max)
	  DECLARE @SQL VARCHAR(max)
	  
	  --Validate pagination parameters
	IF(@PageNumber IS NULL Or @PageNumber <= 0) SET @PageNumber = 1
	IF(@PageSize IS NULL Or @PageSize <= 0) SET @PageSize = 50
	
	--Calculate start and end row to return
	Declare @StartRow Int = ((@PageNumber - 1) * @PageSize) + 1      
	Declare @EndRow Int = @PageNumber * @PageSize

	--DECLARE @PayRegister TABLE (
	CREATE TABLE #PayRegister (

	ID int NULL,
	fDate datetime null, 
	Ref int NULL,
	TInc NUMERIC(30,2) NULL,
	FIT NUMERIC(30,2) NULL,
	FICA NUMERIC(30,2) NULL,
	MEDI NUMERIC(30,2) NULL,
	SIT NUMERIC(30,2) NULL,
	TOther NUMERIC(30,2) NULL,
	TDed NUMERIC(30,2) NULL,
	Net NUMERIC(30,2) NULL,
	Name VARCHAR(MAX) NULL,
    EmpID int NULL,
	Sel Int NULL,
	[Status] VARCHAR(100) NULL
);

      
        
			SET @SQLQuery = 'SELECT PRReg.ID, PRReg.fDate, PRReg.Ref, PRReg.TInc, PRReg.FIT, PRReg.FICA, PRReg.MEDI, PRReg.SIT, PRReg.TOther, PRReg.TDed, PRReg.Net, Emp.Name, Emp.ID AS EmpID,T.Sel,
CASE WHEN T.Sel =0 THEN ''Open'' WHEN T.Sel = 1 THEN ''Cleared'' WHEN T.Sel= 2 THEN ''Void'' END AS [Status] '
				--IF( @EN = 1 )
				--	BEGIN
				--		SET @SQLQuery +=' , UC.IsSel, UC.UserID '
				--	END 
			SET @SQLQuery +=' FROM PRReg INNER JOIN Emp ON PRReg.EmpID=Emp.ID INNER JOIN Rol ON Emp.Rol=Rol.ID INNER JOIN Trans T ON T.ID = PRReg.TransID'
				--IF( @EN = 1 )
				-- BEGIN
				--	 SET @SQLQuery +='  left outer join tblUserCo UC on UC.CompanyID = Rol.EN '
				-- END 
			SET @SQLQuery +=' WHERE  PRReg.fDate >='''+ CONVERT(VARCHAR(50), @sDate + '00:00:00') +''' AND PRReg.fDate <= '''+ CONVERT(VARCHAR(50), @eDate + '00:00:00') +'''  '
				--IF( @EN = 1 )
				--	BEGIN
				--		SET @SQLQuery +=' and UC.IsSel = 1 and UC.UserID =' + CONVERT(NVARCHAR(50), @UserID)
				--	END
				IF( ISNULL(@Emp,0) <> 0 )
					BEGIN
						SET @SQLQuery +=' AND Emp.ID='+ CONVERT(VARCHAR,@Emp)
					END
			SET @SQLQuery +='  ORDER BY PRReg.Ref'

	  Print @SQLQuery;
      INSERT INTO #PayRegister EXECUTE( @SQLQuery)

	  
	  SET @SQL =' SELECT * FROM ( select ROW_NUMBER() OVER(Order By '+@SortBy+'  '+@SortType+') RowNumber, COUNT(1) OVER() TotalRow,* from  (SELECT * FROM #PayRegister  ) as T )  as p 
	  WHERE p.RowNumber BETWEEN '+CONVERT(NVARCHAR(50), @StartRow)+' And '+CONVERT(NVARCHAR(50), @EndRow)+''
	  print @SQL
	  EXECUTE(@SQL)
	  

  END 
