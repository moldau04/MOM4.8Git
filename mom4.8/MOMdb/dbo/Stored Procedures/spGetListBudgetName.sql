CREATE PROCEDURE spGetListBudgetName
AS
BEGIN
    Declare @yearEnd INT
	Declare @currentMonth INT 
	Declare @financialYear INT 
	
	IF EXISTS  (select [dbo].[Control].[YE] FROM [dbo].[control] )
	 BEGIN
	   SET @yearEnd = CONVERT(int, (SELECT [dbo].[Control].[YE] FROM [dbo].[control] )) 	   
	   print  @yearEnd
     END
	ELSE
     BEGIN
	   SET @yearEnd = (Select MONTH(Getdate()))
	   SET @yearEnd =  @yearEnd  -1
	   print  @yearEnd
     END 

	 SET @currentMonth = (Select MONTH(Getdate()))

	 IF(@currentMonth > @yearEnd)
	   BEGIN
	     SET @financialYear = (Select YEAR(Getdate()) + 1 )
       END
      ELSE
	     SET @financialYear = (Select YEAR(Getdate()))

	   Select [dbo].[Budget].[BudgetID], [dbo].[Budget].Budget from [dbo].[Budget] where Year = @financialYear
END