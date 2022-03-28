CREATE PROCEDURE [dbo].[Account_GetSummaryByYear]
(
	@Year VARCHAR(50) 
)
AS
BEGIN
SET NOCOUNT ON
	DECLARE @PeriodStartInt AS INT
	DECLARE @PeriodEndInt AS INT
    DECLARE @PeriodStartString AS VARCHAR(50)
	SET @PeriodStartString = @Year + '01'
	DECLARE @PeriodEndString AS VARCHAR(50)
	SET @PeriodEndString = @Year + '12'
	SET @PeriodStartInt = CAST(@PeriodStartString AS INT)
	SET @PeriodEndInt = CAST(@PeriodEndString AS INT)
	SELECT B.Budget, AD.Acct, AD.Balance, SUM(Credit) AS Credit, SUM(Debit) As Debit, SUM(Amount) As Amount
	from Accounts A INNER JOIN AccountDetails AD ON A.AccountDetailID = AD.AccountDetailID INNER JOIN Budget B ON B.BudgetID = A.BudgetID
	WHERE Period >= @PeriodStartInt AND Period <= @PeriodEndInt
	GROUP BY B.Budget, AD.Acct, AD.Balance
RETURN
END