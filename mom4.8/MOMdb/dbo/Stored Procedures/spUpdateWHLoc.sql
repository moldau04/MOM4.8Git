CREATE PROCEDURE [dbo].[spUpdateWHLoc]
	

	@WHLocID int,
	@WareHouseLocation Varchar(75),
	@WareHouseID varchar(5)
	
AS
BEGIN
 
				DECLARE @success varchar(50)
			 if not exists(select 1 from WHLoc where Name=@WareHouseLocation and WareHouseID=@WareHouseID)
			 begin
				UPDATE WHLoc SET Name=@WareHouseLocation WHERE ID=@WHLocID
				set @success='success'
				end 
			
				else
				 begin 
				 set @success ='WareHouseLocation '+@WareHouseLocation+' is already exists.'
				 end

					
				 	select @success
				  
				  
				  
				
					
			
			
			

		
END

