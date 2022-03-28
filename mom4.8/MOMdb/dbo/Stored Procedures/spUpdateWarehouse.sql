
CREATE PROCEDURE [dbo].[spUpdateWarehouse]

			@ID varchar(10),
           @Name varchar(25),
           @Type int=null,
           @Location int =null,
           @Remarks varchar(800) =null,
         --  @Count int =null ,
		   @IsMultiValue bit ,
		    @EN INT =  Null ,
		   @Status INT

as
	begin
			DECLARE @success int
			set @success=0
			
		
				BEGIN
				UPDATE [Warehouse] SET 

					
				   [Name]=@Name
				   ,[Type]=@Type
				   ,[Location]=@Location
				   ,[Remarks]=@Remarks
				--   ,[Count]=@Count
				   ,[Multi]=@IsMultiValue,
				   EN=@EN,
				   [Status]=@Status
				   WHERE Warehouse.ID=@ID
					
					--set @success=@ID
			   END
			
		
			

		

		select @success

	end
GO