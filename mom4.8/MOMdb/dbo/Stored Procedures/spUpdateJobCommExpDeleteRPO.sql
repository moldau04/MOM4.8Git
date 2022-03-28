CREATE PROCEDURE [dbo].[spUpdateJobCommExpDeleteRPO](
	@job int,
	@RPOId int
	)
AS


/******************************************************NK*****************************************************************
-----1  CALCULATE LABOR EXPENSE   

--SELECT (SELECT  SUM(AMOUNT) FROM JOBI WHERE bomt.Type='LABOR') LABOR

--SELECT (SELECT  SUM(AMOUNT) FROM JOBI WHERE JOB= @JOB  AND TRANSID < 0 AND LABOR =1 ) LABOR



----2  CALCULATE MATRIAL  EXPENSE 

--SELECT (SELECT  SUM(AMOUNT) FROM JOBI WHERE bomt.Type='Materials'  or bomt.Type='Inventory') MATRIAL
 

------3 CALCULATE OTHER EXPENSE = 

 --SELECT (SELECT  SUM(AMOUNT) FROM JOBI WHERE (bomt.Type<>'Materials'  and bomt.Type <> 'Labor' and bomt.Type<>'Inventory')
 
------4 TOTALEXPENSE =  

--- ==  Total Expense = Labor Expense + Material Expense + Other Expense 
***********************************************************************************************************************/
BEGIN
	
	SET NOCOUNT ON;

BEGIN TRY
--BEGIN TRANSACTION

    DECLARE @cost NUMERIC(30,2)
	DECLARE @profit NUMERIC(30,2)
	DECLARE @ratio NUMERIC(30,2)
	DECLARE @comm NUMERIC(30,2)
	DECLARE @rev NUMERIC(30,2) = ISNULL((SELECT isnull(rev,0) as Rev FROM Job WHERE ID = @job),0)

	SET @comm = ISNULL((SELECT sum(isnull(p.Balance,0)) from POItem p 
									INNER JOIN PO on p.po = po.po
									WHERE p.Job = @job and po.status in (0,3,4)),0) + 
				ISNULL((SELECT sum(isnull(rp.Amount,0)) from RPOItem rp 
									INNER JOIN ReceivePO r on r.ID = rp.ReceivePO
									LEFT JOIN POItem p on r.PO = p.PO AND rp.POLine = p.Line
									WHERE p.Job = @job and r.ID = @RPOId And ISNULL(r.status,0) = 0),0)
 
	SET @cost =   ISNULL((SELECT SUM(ISNULL(Amount,0))  FROM jobi WHERE Job = @job AND Type in (1)), 0) 

	
	SET @profit = @rev - @cost

	IF @rev <> 0
	BEGIN

		SET @ratio = CONVERT(numeric(30,2),((@profit / @rev) * 100))

	END
	ELSE
	BEGIN
		SET @ratio = 0
	END

    UPDATE Job
	SET
		Cost = @cost,
		Profit = @profit,
		Ratio = @ratio, 
		Comm = @comm
	WHERE ID = @job

	

--COMMIT 
	END TRY
	BEGIN CATCH

	SELECT ERROR_MESSAGE()

    IF @@TRANCOUNT>0
        --ROLLBACK	
		RAISERROR ('An error has occurred on this page.',16,1)
        RETURN

END CATCH

END
GO