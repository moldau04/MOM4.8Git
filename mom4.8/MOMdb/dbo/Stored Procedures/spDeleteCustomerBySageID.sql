CREATE proc [dbo].[spDeleteCustomerBySageID]
@QBCustomerID varchar(100)
as
declare @CustID int
declare @rolID int
declare @Custname varchar(150)
declare @data as xml
BEGIN TRANSACTION
select @CustID =ID, @rolID = Rol, @Custname = (select name from Rol where ID = o.Rol) from [Owner] o  
WHERE isnull( SageID,'')<>'' and  SageID = @QBCustomerID

set @data = (select * from Owner o inner join Rol r on r.ID= o.Rol where o.ID= @CustID for xml auto)

IF NOT EXISTS(SELECT 1 
              FROM   Loc 
              WHERE  Owner = @CustID) 
  BEGIN 
   if exists(select 1 from Owner where ID=@CustID ) 
      begin
      
      DELETE FROM rol 
      WHERE  id = @rolID
 
		IF @@ERROR <> 0 AND @@TRANCOUNT > 0
					 BEGIN  
						RAISERROR ('Error Occured', 16, 1)  
						ROLLBACK TRANSACTION    
						RETURN
					 END
     
      DELETE FROM Owner WHERE ID=@CustID 
      
		IF @@ERROR <> 0 AND @@TRANCOUNT > 0
					 BEGIN  
						RAISERROR ('Error Occured', 16, 1)  
						ROLLBACK TRANSACTION    
						RETURN
					 END
      
      INSERT INTO tblSyncDeleted 
				(Tbl, 
				 NAME, 
				 RefID, 
				 QBID,
				 DateStamp,
				 Data
				 ) 
	 VALUES      ('OWNER', 
				 @Custname, 
				 @CustID, 
				 @QBCustomerID,
				 GETDATE() ,
				 @data
				 ) 
				 
		IF @@ERROR <> 0 AND @@TRANCOUNT > 0
					 BEGIN  
						RAISERROR ('Error Occured', 16, 1)  
						ROLLBACK TRANSACTION    
						RETURN
					 END
		
	  end
		
  END 
else 
  BEGIN  UPDATE owner SET Status = 1 WHERE ID=@CustID
end

COMMIT TRANSACTION
