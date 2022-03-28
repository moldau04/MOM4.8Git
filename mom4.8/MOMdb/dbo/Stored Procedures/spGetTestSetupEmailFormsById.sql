CREATE PROCEDURE [dbo].[spGetTestSetupEmailFormsById]
@ID int
AS
SELECT [ID]
      ,[Name]
	  ,[Body]
      ,[AddedBy]
      ,[AddedOn]
	  ,[UpdatedBy]
      ,[UpdatedOn]	  
	  ,[IsActive]
  FROM [dbo].[TestSetupEmailForms]
  Where ID=@ID
