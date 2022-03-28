CREATE PROCEDURE [dbo].[spVoidPayCheck]   
 @ID int,    
 @UpdatedBy varchar(100)  
AS  
BEGIN  
 Declare @CurrentDate varchar(50)
 Declare @Check int
 Select @CurrentDate = CONVERT(varchar(50), fDate , 101),@Check = Ref from PRReg  WHERE ID = @ID 
 DECLARE @Val VARCHAR(MAX);
 DECLARE @Batch int;
 DECLARE @CurrentStatus int;
 DECLARE @TransID int;
 Select @Batch = Batch,@CurrentStatus = Sel,@TransID = ID from Trans WHERE ID IN (SELECT TransID FROM PRReg WHERE ID= @ID)
 BEGIN TRY  
 
 BEGIN TRANSACTION  
  
		UPDATE [dbo].Trans  
		SET  Sel = 1,Amount=0,[Status]='VD'+CONVERT(varchar(50), GETDATE() , 1),fDesc = 'Voided on '+CONVERT(varchar(50), GETDATE() , 101)+' by '+ @UpdatedBy + fDesc 
		WHERE ID = @TransID 
		
		UPDATE PRReg SET 
		Yreg=0,HReg=0,HYReg=0,OT=0,YOT=0,HOT=0,HYOT=0,DT=0,YDT=0,HDT=0,HYDT=0,NT=0,YNT=0,HNT=0,HYNT=0,TT=0,YTT=0,HTT=0,HYTT=0,
		Hol=0,YHol=0,HHol=0,HYHol=0,Vac=0,YVac=0,HVac=0,HYVac=0,Zone=0,YZone=0,Reimb=0,YReimb=0,Mile=0,YMile=0,Bonus=0,YBonus=0,
		WFIT=0,WFica=0,WMedi=0,WFuta=0,WSit=0,WVac=0,WWComp=0,WUnion=0,FIT=0,YFIT=0,FICA=0,YFICA=0,MEDI=0,YMEDI=0,FUTA=0,YFUTA=0,SIT=0,YSIT=0,Local=0,YLocal=0,
		TOTher=0,YTOTher=0,TInc=0,TDed=0,Net=0,VThis=0,CompMedi=0,WMediOverTH=0,Sick=0,YSick=0,WSick=0,HSick=0,HYSick=0,HSickAccrued=0,HYSickAccrued=0,HVacAccrued=0
		WHERE ID = @ID


		DELETE FROM PRRegWItem WHERE CheckID = @ID

		DELETE FROM Trans WHERE Batch = @Batch AND Type = 91
		
		/********Start Logs************/
		set @Val=null
		if(@Check is not null And @Check != 0)
		begin 	
      		
				exec log2_insert @UpdatedBy,'PayCheck',@ID,'Status','Open','Void'
			
		end
		/********End Logs************/
  
 
    
  
 COMMIT  
  
 END TRY  
 BEGIN CATCH  
   --SELECT ERROR_MESSAGE()    
    IF @@TRANCOUNT>0  
        ROLLBACK   
        RAISERROR ('An error has occurred on this page.',16,1)  
        RETURN    
 END CATCH     
END
GO