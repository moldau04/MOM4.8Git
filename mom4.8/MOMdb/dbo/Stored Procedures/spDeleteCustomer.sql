create proc spDeleteCustomer
@Owner int
as
IF NOT EXISTS(SELECT 1 
              FROM   Loc 
              WHERE  Owner = @Owner) 
  BEGIN
   
		declare @rolid int

		SELECT TOP 1 @rolid = rol FROM owner WHERE  id = @Owner 

		DELETE FROM Owner WHERE  ID =  @Owner

		if not exists(
		select 1 from loc where GContractorID = @rolID 
		union
		select 1 from loc where HomeOwnerID = @rolID
		union
		select 1 from job where rol = @rolID  
		)
		BEGIN
		DELETE FROM rol WHERE  id = @rolid
		END
 
		

  END 
ELSE 
  BEGIN 
      RAISERROR ('Locations are assigned to the selected customer, customer cannot be deleted!',16,1) 
      RETURN 
  END
GO

