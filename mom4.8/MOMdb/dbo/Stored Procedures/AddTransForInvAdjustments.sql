CREATE Procedure [dbo].[AddTransForInvAdjustments] (
        
            @Batch   INT = NULL 
           ,@fDate   DATETIME =  NULL
           ,@Type    SMALLINT
           ,@Line    SMALLINT
           ,@Ref     INT = NULL
           ,@fDesc   VARCHAR(255)
           ,@Amount  NUMERIC(30,2)
           ,@Acct    INT
           ,@AcctSub INT
           ,@Status  VARCHAR(10)
           ,@Sel     SMALLINT = NULL
           ,@VInt    INT = 0
           ,@VDoub   NUMERIC(30,2) =0
           ,@EN      INT=0
           ,@strRef  VARCHAR(50) = NULL
)

AS BEGIN

DECLARE @ID INT

--SET @ID= (SELECT ISNULL(MAX(ID),0)+1 FROM Trans)
IF @Batch IS NULL SELECT @Batch=ISNULL(MAX(Batch),0)+1 FROM Trans
IF @fDate IS NULL SELECT @fDate= CAST(
                          FLOOR( CAST( GETDATE() AS FLOAT ) )
                          AS DATETIME
                         )
INSERT INTO Trans
           ([Batch]
           ,[fDate]
           ,[Type]
           ,[Line]
           ,[Ref]
           ,[fDesc]
           ,[Amount]
           ,[Acct]
           ,[AcctSub]
           ,[Status]
           ,[Sel]
           ,[VInt]
           ,[VDoub]
           ,[EN]
           ,[strRef])
     VALUES (@Batch
           ,@fDate
           ,@Type
           ,@Line
           ,@Ref
           ,@fDesc
           ,@Amount
           ,@Acct
           ,@AcctSub
           ,@Status
           ,@Sel
           ,@VInt
           ,@VDoub
           ,@EN
           ,@strRef
)
SET @ID = SCOPE_IDENTITY()
SELECT @ID

END
