
CREATE PROCEDURE [dbo].[spUpdateBankDetails](
     @ID      INT = NULL OUTPUT
    ,@fDesc   VARCHAR(75)
    --,@Rol INT 
	,@NBranch VARCHAR(20) =  NULL
	,@NAcct VARCHAR(20) =  NULL
	,@NRoute VARCHAR(20) =  NULL
	,@NextC INT =  NULL
	,@NextD INT =  NULL
	,@NextE INT =  NULL
	,@Rate NUMERIC(30,2) =  NULL
	,@CLimit NUMERIC(30,2) =  NULL
	,@Warn SMALLINT =  NULL
	--,@Recon NUMERIC(30,2) =  0.00
	--,@Balance NUMERIC(30,2) =  0.00
	,@Status SMALLINT =  NULL
	--,@InUse SMALLINT =  0
	--,@Chart INT
	,@Rol INT 
)
AS
BEGIN
	
	SET NOCOUNT ON;
		UPDATE Bank
		SET fDesc = @fDesc
		--,Rol = @Rol
		,NBranch = @NBranch
		,NAcct = @NAcct
		,NRoute = @NRoute
		,NextC = @NextC
		,NextD = @NextD
		,NextE = @NextE
		,Rate = @Rate
		,CLimit = @CLimit
		,Warn = @Warn
		--,Recon = @Recon
		--,Balance = @Balance
		,Status = @Status
		--,InUse = @InUse
		WHERE Rol = @Rol
    
END
