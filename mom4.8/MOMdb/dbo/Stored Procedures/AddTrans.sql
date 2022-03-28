CREATE PROCEDURE [dbo].[AddTrans] (
            @ID      INT = NULL OUTPUT
           ,@Batch   INT = NULL OUTPUT
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
--IF @ID IS NULL SELECT @ID=ISNULL(MAX(ID),0)+1 FROM Trans
IF @Batch IS NULL SELECT @Batch=ISNULL(MAX(Batch),0)+1 FROM Trans
IF @fDate IS NULL SELECT @fDate= CAST(FLOOR( CAST( GETDATE() AS FLOAT ) )  AS DATETIME)
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

END
