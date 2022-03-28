CREATE Procedure [dbo].[spGetTestSetupFormsByType]
@Type int
AS
SELECT [ID]
      ,[Name]
      ,[FileName]
      ,[FilePath]      
      ,[MIME]
	  ,[Type]
      ,[AddedBy]
      ,[AddedOn]
	  ,[UpdatedBy]
      ,[UpdatedOn]
	  ,[IsActive]
  FROM [dbo].[TestSetupForms]
  Where Type=@Type and [IsActive]=1