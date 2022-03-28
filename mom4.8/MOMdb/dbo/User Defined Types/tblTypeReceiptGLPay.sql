CREATE TYPE [dbo].[tblTypeReceiptGLPay] AS TABLE (  
    [ID]     INT             NULL,   
    [Amount] NUMERIC (30, 2) NULL,
   [Description] varchar(500) 
   )