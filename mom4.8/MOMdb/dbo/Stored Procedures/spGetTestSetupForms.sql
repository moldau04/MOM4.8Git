create Procedure spGetTestSetupForms
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
GO


