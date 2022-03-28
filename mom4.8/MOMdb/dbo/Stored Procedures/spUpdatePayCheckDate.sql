CREATE PROCEDURE [dbo].[spUpdatePayCheckDate]   
 @ID int,    
 @Bank int,  
 @Check int,  
 @CDate datetime,  
 @UpdatedBy varchar(100)  ,
 @UpdtType varchar(100)  

AS  
BEGIN  
 Declare @CurrentDate varchar(50)
 Select @CurrentDate = CONVERT(varchar(50), fDate , 101) from PRReg  WHERE ID = @ID 
 Declare @CurrentRef varchar(50)
 Select @CurrentRef = Ref from PRReg  WHERE ID = @ID
 DECLARE @Val VARCHAR(MAX);
 DECLARE @Batch int;
 Select @Batch =  batch from Trans WHERE ID IN (SELECT TransID FROM PRReg WHERE ID= @ID)
 BEGIN TRY  

 BEGIN TRANSACTION  
  IF @UpdtType = 'CheckNo'
  BEGIN
		UPDATE [dbo].PRReg  
		SET  [Ref] = @Check  
        WHERE ID = @ID 
		
		UPDATE Bank SET NextC = @Check WHERE ID = @ID

		UPDATE Trans SET Ref=@Check WHERE Batch=@Batch
		/********Start Logs************/
		set @Val=null
		if(@Check is not null And @Check != 0)
		begin 	
      		Set @Val =(select Top 1 newVal  from log2 where screen='PayCheck' and ref= @Check and Field='Check #' order by CreatedStamp desc )		
			if(@Val<> Convert(varchar(50), @Check))
			begin
				exec log2_insert @UpdatedBy,'PayCheck',@ID,'Check #',@Val,@Check
			end
			Else IF (@CurrentRef <> Convert(varchar(50), @Check))
			Begin
				exec log2_insert @UpdatedBy,'PayCheck',@ID,'Check #',@CurrentRef,@Check
			END
		end
		/********End Logs************/
  END

  IF @UpdtType = 'CheckDate'
  BEGIN
		UPDATE [dbo].PRReg  
		SET  [fDate] = @CDate  
        WHERE ID = @ID 

		UPDATE Trans SET fDate=@CDate WHERE Batch=@Batch
		/********Start Logs************/
			
	if(@CDate is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='PayCheck' and ref= @Check and Field='Check Date' order by CreatedStamp desc )		
	Declare @CurrentCheckDate varchar(150)
	Select @CurrentCheckDate = CONVERT(varchar(50), @CDate , 101)
	if(@Val<>@CurrentCheckDate)
	begin
	exec log2_insert @UpdatedBy,'PayCheck',@ID,'Check Date',@Val,@CurrentCheckDate
	end
	Else IF (@CurrentDate <> @CurrentCheckDate)
	Begin
	exec log2_insert @UpdatedBy,'PayCheck',@ID,'Check Date',@CurrentDate,@CurrentCheckDate
	END
	end
		/********End Logs************/
  END
  
 
    
  
 COMMIT  
  
 END TRY  
 BEGIN CATCH  
   SELECT ERROR_MESSAGE()    
    IF @@TRANCOUNT>0  
        ROLLBACK   
        RAISERROR ('An error has occurred on this page.',16,1)  
        RETURN    
 END CATCH     
END
GO