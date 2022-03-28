CREATE PROCEDURE [dbo].[spCreateWarehouse]

			@ID varchar(10),
           @Name varchar(25),
           @Type int=null,
           @Location int =null,
           @Remarks varchar(800) =null,
         --  @Count int =null ,
		   @IsMultiValue bit,
		   @EN INT =  Null ,
		   @Status INT

as
	begin
			DECLARE @success varchar(50)
			set @success=''
	
			if not exists(select 1 from Warehouse where ID=@ID)
				BEGIN
					INSERT INTO [dbo].[Warehouse]
				   ([ID]
				   ,[Name]
				   ,[Type]
				   ,[Location]
				   ,[Remarks]
				   --,[Count]
				   ,[Multi]
				   ,EN,
				   Status)
					 VALUES
				   (@ID,@Name,@Type,@Location,@Remarks,@IsMultiValue,@EN,@Status)

				   set @success=@ID
				   select @success
			   END
			else
				begin
			declare @msg nvarchar(max)
			set @msg= 'Already Exist'-- 'Warehouse '+@ID+' could not be created as it already exists.'
			--print @msg
				--RAISERROR (@msg, 16, 1)
				select @msg
				end
	end
GO