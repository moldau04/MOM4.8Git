CREATE PROCEDURE [dbo].[SpAddACHCustomerAccounts]
(
	@OwnerID INT , 
	@RoutingNo VARCHAR (10) ,
	@AccountNo VARCHAR  (20) ,
	@Name VARCHAR(20)  
) 
AS
 IF NOT Exists (SELECT 1 FROM tblCustomerAccounts 
 WHERE OwnerID=@OwnerID 
 AND RoutingNo=@RoutingNo 
 AND AccountNo=@AccountNo
 AND Name=@Name
 )
   BEGIN 
      INSERT INTO tblCustomerAccounts(OwnerID,RoutingNo,AccountNo,Name)
	  VALUES(@OwnerID,@RoutingNo,@AccountNo,@Name)
   END