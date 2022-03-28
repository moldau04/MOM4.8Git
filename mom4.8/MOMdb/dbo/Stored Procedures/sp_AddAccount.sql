CREATE PROCEDURE [dbo].[Account_Update]
(
	@AccountID INT = NULL OUTPUT,
	@Acct	 VARCHAR(50),
	@fDesc	 VARCHAR(50),
	@Balance	 VARCHAR(50),
	@Type		 VARCHAR(50),
	@Sub			 VARCHAR(50),
	@Remarks		VARCHAR(50),
	@Control	 VARCHAR(50),
	@InUse		 VARCHAR(50),
	@Detail	 VARCHAR(50),
	@CAlias		 VARCHAR(50),
	@Status			 VARCHAR(50),
	@Sub2	 VARCHAR(50),
	@DAT	 VARCHAR(50),
	@Branch	 VARCHAR(50),
	@CostCenter	 VARCHAR(50),
	@AccRoot	    VARCHAR(50)
)
AS
BEGIN
	SET NOCOUNT ON;
	IF @AccountID is NULL 
	IF NOT EXISTS(SELECT * FROM Account WHERE Acct = @Acct AND fDesc = @fDesc AND Type = @Type)
	BEGIN
			INSERT INTO Account(Acct,fDesc,Balance,Type,Sub,Remarks,Control,InUse,Detail,CAlias,Status,Sub2,DAT,Branch,CostCenter,AccRoot) 
			VALUES (@Acct,@fDesc,@Balance,@Type,@Sub,@Remarks,@Control,@InUse,@Detail,@CAlias,@Status,@Sub2,@DAT,@Branch,@CostCenter,@AccRoot)
			SET @AccountID = scope_identity()
			END
	ELSE
	BEGIN
	SET @AccountID = (SELECT AccountID FROM Account WHERE Acct = @Acct AND fDesc = @fDesc AND Type = @Type)
	END
END
