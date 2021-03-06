CREATE proc [dbo].[spDeleteCustomerByListID]
@QBCustomerID varchar(100)
as
declare @CustID int
declare @rolID int
declare @Custname varchar(150)

select @CustID =ID, @rolID = Rol, @Custname = (select name from Rol where ID = o.Rol) from [Owner] o  
WHERE isnull( QBCustomerID,'')<>'' and  QBCustomerID = @QBCustomerID

IF NOT EXISTS(SELECT 1 
              FROM   Loc 
              WHERE  Owner = @CustID) 
  BEGIN 
   if exists(select 1 from Owner where ID=@CustID ) 
      begin

      if not exists(
	  select 1 from loc where GContractorID = @rolID 
	  union
	  select 1 from loc where HomeOwnerID = @rolID
	  union
	   select 1 from job where rol = @rolID  
	  )
	  BEGIN
      DELETE FROM rol WHERE  id = @rolID
	  END

      DELETE FROM Owner WHERE ID=@CustID 
           
      INSERT INTO tblSyncDeleted 
				(Tbl, 
				 NAME, 
				 RefID, 
				 QBID,
				 DateStamp) 
	 VALUES      ('OWNER', 
				 @Custname, 
				 @CustID, 
				 @QBCustomerID,
				 GETDATE() ) 
		end
		
  END 
else 
  BEGIN  UPDATE owner SET Status = 1 WHERE ID=@CustID
end
GO

