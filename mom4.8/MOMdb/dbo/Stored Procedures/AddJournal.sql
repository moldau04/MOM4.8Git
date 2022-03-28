CREATE Procedure [dbo].[AddJournal] (  
            @ID      INT = NULL OUTPUT  
           ,@Batch   INT  --= NULL OUTPUT  
           ,@fDate   DATETIME =  NULL  
           ,@Type    SMALLINT = 50  
           ,@Line    SMALLINT  
           ,@Ref     BIGINT = NULL  
           ,@fDesc   VARCHAR(255)  
           ,@Amount  NUMERIC(30,2)  
           ,@Acct    INT  
           ,@AcctSub INT = NULL  
           ,@Status  VARCHAR(10) = NULL  
           ,@Sel     SMALLINT = 0  
           ,@VInt    INT = NULL  
           ,@VDoub   NUMERIC(30,2) = NULL  
           ,@EN      INT=0  
           ,@strRef  VARCHAR(50) = NULL  
)  
  
AS BEGIN  
  
--IF @ID IS NULL  
--BEGIN  
-- SET @ID=  ISNULL((SELECT MAX(ISNULL(ID,0))+1 FROM Trans),1)  
--END  
  
  
  
--SELECT ID,COUNT(ID) FROM Trans Group BY ID Having Count(ID) >1  
  
--IF @Batch IS NULL SELECT @Batch=ISNULL(MAX(Batch),0)+1 FROM Trans  
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
     VALUES (  
            @Batch  
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
  
END  
--SELECT SCOPE_IDENTITY() AS TransID  
      
return SCOPE_IDENTITY()  