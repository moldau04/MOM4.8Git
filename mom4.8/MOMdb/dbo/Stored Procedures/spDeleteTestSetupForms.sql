Create Procedure spDeleteTestSetupForms
@ID int 
AS
BEGIN
Delete from.[TestSetupForms] WHERE ID=@ID
END