CREATE PROCEDURE [dbo].[spUpdateCustomerLocationRemarks] 
										 @OwnerID      INT,
										 @LocID INT,
                                         @Remarks         VARCHAR(8000),
										 @UpdatedBy varchar(100)
										
AS
	Declare @Val varchar(1000)	
	set @Val=null
	DECLARE @Rol INT
	Declare @CurrentRemarks VARCHAR(8000)
	
	IF(@LocID  > 0)
	BEGIN
	Select @CurrentRemarks = Remarks from Loc Where Loc = @LocID
	select @Rol=Rol from Loc where Loc=@locID

	  UPDATE Rol
          SET  Remarks = @Remarks
		  WHERE  id = @Rol
       
	   Update Loc
	    SET  Remarks = @Remarks
		 where Loc = @locID 

	if(@Remarks is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Location' and ref= @locID and Field='Bill Remarks' order by CreatedStamp desc )
	if(@Val<>CONVERT(VARCHAR(1000), @Remarks))
	begin
	exec log2_insert @UpdatedBy,'Location',@locID,'Bill Remarks',@Val,@Remarks
	end
	Else IF (@CurrentRemarks <> CONVERT(VARCHAR(1000), @Remarks))
	Begin
	exec log2_insert @UpdatedBy,'Location',@locID,'Bill Remarks',@CurrentRemarks,@Remarks
	END
	end     
END	
	ELSE

	BEGIN
	SELECT @Rol = Rol FROM   Owner WHERE  ID = @OwnerID
	Select @CurrentRemarks = Remarks from Rol Where ID = @Rol      
		  UPDATE Rol
          SET  Remarks = @Remarks
		  WHERE  id = @Rol
	
	if(@Remarks is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Customer' and ref= @OwnerID and Field='Remarks' order by CreatedStamp desc )
	if(@Val<>@Remarks)
	begin
	exec log2_insert @UpdatedBy,'Customer',@OwnerID,'Remarks',@Val,@Remarks
	end
	Else IF (@CurrentRemarks <>  @Remarks)
	Begin
	exec log2_insert @UpdatedBy,'Customer',@OwnerID,'Remarks',@CurrentRemarks,@Remarks
	END
	end	
END		    
GO