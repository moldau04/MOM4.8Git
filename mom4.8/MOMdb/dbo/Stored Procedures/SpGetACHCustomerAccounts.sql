CREATE PROCEDURE [dbo].[SpGetACHCustomerAccounts]
(
	@OwnerID INT  
) 
AS
 BEGIN
 SELECT * FROM tblCustomerAccounts 
 WHERE OwnerID=@OwnerID  
 END