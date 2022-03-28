CREATE PROCEDURE [dbo].[spUpdateJobRev] 
	@job int
AS

  /******************************************************NK*****************************************************************
----- 1  CALCULATE LABOR EXPENSE   

-- SELECT (SELECT  SUM(AMOUNT) FROM JOBI WHERE bomt.Type='LABOR') LABOR

-- SELECT (SELECT  SUM(AMOUNT) FROM JOBI WHERE JOB= @JOB  AND TRANSID < 0 AND LABOR =1 ) LABOR

---- 2  CALCULATE MATRIAL  EXPENSE 

--SELECT (SELECT  SUM(AMOUNT) FROM JOBI WHERE bomt.Type='Materials'  or bomt.Type='Inventory') MATRIAL
 

------ 3 CALCULATE OTHER EXPENSE = 

 --SELECT (SELECT  SUM(AMOUNT) FROM JOBI WHERE (bomt.Type<>'Materials'  and bomt.Type <> 'Labor' and bomt.Type<>'Inventory')
 
------ 4 TOTALEXPENSE =  

--- ==  Total Expense = Labor Expense + Material Expense + Other Expense 
***********************************************************************************************************************/
   
    EXEC  [dbo].[spUpdateJobcostByJob] @job