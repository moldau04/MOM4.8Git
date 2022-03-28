Create proc [dbo].[spAddRoute]
@name varchar(50),
@mech int,
@remarks varchar(8000),
@id int,
@Color varchar(100),
@UpdatedBy varchar(100),
@Status BIT
As

DECLARE @IsValid SMALLINT=1
IF @id>0
BEGIN
	IF (@Status=0 AND (SELECT COUNT(1) FROM Loc WHERE Route=@id)>0)
	BEGIN
		SET @IsValid= 0
	END
END
IF @IsValid = 1
BEGIN
Declare @Currentname varchar(50)
Select @Currentname =  Name from Route where ID =@id
Declare @Currentmech varchar(150)
Select @Currentmech =  fDesc From tblWork Where ID =(Select Mech from Route where ID =@id)
Declare @Currentremarks varchar(8000)
Select @Currentremarks =  Remarks from Route where ID =@id

Declare  @Currentstatus Varchar (50)
Select @Currentstatus = Case When Status = 1 Then 'Active' Else 'Inactive' END from Route where ID = @id

Declare @CurrentColor varchar(8000)
Select @CurrentColor =  Color from Route where ID =@id


if (@id = 0)
begin
if not exists(select 1 from Route where Name = @name)
begin 
insert into Route 
(
Name,
Mech,
Remarks,
Color,
Status
)
values
(
@name,
@mech,
@remarks,
@Color,
@Status
)
SET @id = Scope_identity()
end
else
BEGIN
    RAISERROR ('Name already exists, please use different name !',16,1)
    RETURN
END
end
else
begin
if not exists(select 1 from Route where Name = @name and ID<>@id)
begin 
update Route set
Name=@name,
Mech=@mech,
Remarks=@remarks,
Color=@Color,
Status=@Status
where id= @id
end
else
BEGIN
    RAISERROR ('Name already exists, please use different name !',16,1)
    RETURN
END
end
 /********Start Logs************/
 Declare @Val varchar(1000)
 if(@name is not null And @name != '')
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Route' and ref= @id and Field='Name' order by CreatedStamp desc )		
	if(@Val<>@name)
	begin
	exec log2_insert @UpdatedBy,'Route',@id,'Name',@Val,@name
	end
	Else IF (@Currentname <>  @name)
	Begin
	exec log2_insert @UpdatedBy,'Route',@id,'Name',@Currentname,@name
	END
	Else IF (@Val is null And @name != '')
	Begin
	exec log2_insert @UpdatedBy,'Route',@id,'Name','',@name
	END
	end
 set @Val=null
 if(@mech is not null And @mech != 0)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Route' and ref= @id and Field='Worker' order by CreatedStamp desc )		
	Declare @mechVal varchar(150)
	Select @mechVal =  fDesc From tblWork Where ID = @mech
	if(@Val<>@mechVal)
	begin
	exec log2_insert @UpdatedBy,'Route',@id,'Worker',@Val,@mechVal
	end
	Else IF (@Currentmech <>  @mechVal)
	Begin
	exec log2_insert @UpdatedBy,'Route',@id,'Worker',@Currentmech,@mechVal
	END
	Else IF (@Val is null And @mech != 0)
	Begin
	exec log2_insert @UpdatedBy,'Route',@id,'Worker','',@mechVal
	END
	end
	set @Val=null
	if(@remarks is not null And @remarks != '')
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Route' and ref= @id and Field='Remarks' order by CreatedStamp desc )		
	if(@Val<>@remarks)
	begin
	exec log2_insert @UpdatedBy,'Route',@id,'Remarks',@Val,@remarks
	end
	Else IF (@Currentremarks <>  @remarks)
	Begin
	exec log2_insert @UpdatedBy,'Route',@id,'Remarks',@Currentremarks,@remarks
	END
	Else IF (@Val is null And @remarks != '')
	Begin
	exec log2_insert @UpdatedBy,'Route',@id,'Remarks','',@remarks
	END
	END
    	
	
   	set @Val=null
	if(@status is not null)
	begin 		
      	Set @Val =(select Top 1 newVal  from log2 where screen='Route' and ref= @id and Field='Status' order by CreatedStamp desc )
		Declare @StatusVal varchar(50)
		Select @StatusVal = Case When @status = 1 Then 'Active' Else 'Inactive' END
		--select @Currentstatus, @StatusVal,		 @Val
		if(@Val<>@StatusVal)
			begin			
				exec log2_insert @UpdatedBy,'Route',@id,'Status',@Val,@StatusVal
			end
		Else IF (@Currentstatus <> @StatusVal)
			Begin			
				exec log2_insert @UpdatedBy,'Route',@id,'Status',@Currentstatus,@StatusVal
			END
	end

	set @Val=null
  if(@Color is not null And @Color != '')
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='Route' and ref= @id and Field='Color' order by CreatedStamp desc )		
	if(@Val<>@Color)
	begin
	exec log2_insert @UpdatedBy,'Route',@id,'Color',@Val,@Color
	end
	Else IF (@CurrentColor <>  @Color)
	Begin
	exec log2_insert @UpdatedBy,'Route',@id,'Color',@CurrentColor,@Color
	END
	Else IF (@Val is null And @Color != '')
	Begin
	exec log2_insert @UpdatedBy,'Route',@id,'Color','',@Color
	END
	END
	
	/********End Logs************/
END
SELECT @IsValid