CREATE Procedure [dbo].[spDeleteTestSetupEmailForms]
@ID int 
AS
BEGIN
Delete FROM [TestSetupEmailForms] WHERE ID=@ID
END