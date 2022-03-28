CREATE PROCEDURE [dbo].[spUpdateAPCheckDate]   
 @ID int,    
 @fDate datetime,  
 @Ref bigint,  
 @UpdatedBy varchar(100)  
AS  
BEGIN  
 Declare @CurrentDate varchar(50)
 Select @CurrentDate = CONVERT(varchar(50), fDate , 101) from CD  WHERE ID = @ID 
 Declare @CurrentRef varchar(50)
 Select @CurrentRef = Ref from CD  WHERE ID = @ID

 BEGIN TRY  

 BEGIN TRANSACTION  
  
  
 UPDATE [dbo].[CD]  
 SET  [fDate] = @fDate  
      ,[Ref] = @Ref  
       
 WHERE ID = @ID  
    
 /********Start Logs************/
 Declare @Val varchar(1000)
	if(@fDate is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='APCheck' and ref= @Ref and Field='Check Date' order by CreatedStamp desc )		
	Declare @CurrentCheckDate varchar(150)
	Select @CurrentCheckDate = CONVERT(varchar(50), @fDate , 101)
	if(@Val<>@CurrentCheckDate)
	begin
	exec log2_insert @UpdatedBy,'APCheck',@ID,'Check Date',@Val,@CurrentCheckDate
	end
	Else IF (@CurrentDate <> @CurrentCheckDate)
	Begin
	exec log2_insert @UpdatedBy,'APCheck',@ID,'Check Date',@CurrentDate,@CurrentCheckDate
	END
	end
 set @Val=null
 if(@Ref is not null And @Ref != 0)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='APCheck' and ref= @Ref and Field='Check #' order by CreatedStamp desc )		
	if(@Val<> Convert(varchar(50), @Ref))
	begin
	exec log2_insert @UpdatedBy,'APCheck',@ID,'Check #',@Val,@Ref
	end
	Else IF (@CurrentRef <> Convert(varchar(50), @Ref))
	Begin
	exec log2_insert @UpdatedBy,'APCheck',@ID,'Check #',@CurrentRef,@Ref
	END
	end

 /********End Logs************/ 
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