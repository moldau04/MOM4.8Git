CREATE PROCEDURE [dbo].[GetAccountID]
(	
	@fDesc Varchar(50),
	@Type  Varchar(50),
	@Acct  Varchar(50)
)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT AccountID FROM Account WHERE fDesc = @fDesc AND Type = @Type AND Acct = @Acct
END