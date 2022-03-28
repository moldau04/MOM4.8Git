create Procedure spGetTestSetupFormsById
@ID int
AS
SELECT [ID]
      ,[Name]
      ,[FileName]
      ,[FilePath]      
      ,[MIME]
      ,[AddedBy]
      ,[AddedOn]
	   ,[UpdatedBy]
      ,[UpdatedOn]
	  ,[Type]
	  ,[IsActive]
  FROM [dbo].[TestSetupForms]
  Where ID=@ID
GO


