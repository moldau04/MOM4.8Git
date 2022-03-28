CREATE PROCEDURE [dbo].[spUpdateTest]		         
	@id int
	,@typeID int
	,@Loc int
	,@Elev int
	,@Last datetime =null
	,@Next datetime =null
	,@Status int
	,@Ticket int =null
	,@Remarks ntext=null
	,@LastDue datetime=null
	,@JobId int=null
	,@fWork int=null
	,@DWork nvarchar=null
	,@Custom1 varchar(50)
	,@Custom2 varchar(50)
	,@Custom3 varchar(50)
	,@Custom4 varchar(50)		  
	,@Amount numeric(32,2)
	,@OverrideAmount numeric(32,2)
	,@ThirdPartyName varchar(50)
	,@ThirdPartyPhone varchar(50)  
	,@TestDueBy int
	,@Charge INT
	,@ThirdParty INT
	,@PriceYear INT 
	,@CreatedBy VARCHAR(100)
AS 
BEGIN 		

	Declare @old_typeID int
	Declare @old_Loc int
	Declare @old_Elev int
	Declare @old_Last datetime 
	Declare @old_Next datetime 
	Declare @old_Status int
	Declare @old_Ticket int
	--Declare @old_Remarks varchar(100)
	Declare @old_LastDue datetime
	Declare @old_JobId int
	Declare @old_fWork int
	Declare @old_DWork nvarchar
	Declare @old_Custom1 varchar(50)
	Declare @old_Custom2 varchar(50)
	Declare @old_Custom3 varchar(50)
	Declare @old_Custom4 varchar(50)		  
	Declare @old_Amount numeric(32,2)
	Declare @old_OverrideAmount numeric(32,2)
	Declare @old_ThirdPartyName varchar(50)
	Declare @old_ThirdPartyPhone varchar(50)  
	Declare @old_TestDueBy int
	Declare @old_Charge INT
	Declare @old_ThirdParty INT

	SELECT  @old_typeID=ID
        ,@old_Loc=Loc
        ,@old_Elev=Elev
        ,@old_Last=Last
        ,@old_Next=Next
        ,@old_Status=Status
        ,@old_Ticket=Ticket
       -- ,@old_Remarks=Remarks
        ,@old_LastDue=LastDue
		,@old_JobId =[JobId]
		,@old_Custom1=Custom1
		,@old_Custom2=Custom2
		,@old_Custom3=Custom3
		,@old_Custom4=Custom4
		,@old_Amount=Amount
		,@old_OverrideAmount=OverrideAmount
		,@old_ThirdPartyName=ThirdPartyName
		,@old_ThirdPartyPhone=ThirdPartyPhone
		,@old_TestDueBy=TestDueBy
		,@old_Charge=[Chargeable]
		,@old_ThirdParty=ThirdParty	
	FROM [LoadTestItem] WHERE LID=@id
	

	UPDATE  [dbo].[LoadTestItem]
	SET   
        [ID]=@typeID
        ,[Loc]=@Loc
        ,[Elev]=@Elev
        ,[Last]=@Last
        ,[Next]=@Next
        ,[Status]=@Status
        ,[Ticket]=@Ticket
        ,[Remarks]=@Remarks
        ,[LastDue]=@LastDue
		,[JobId]=@JobId 
		,[Custom1]=@Custom1
		,[Custom2]=@Custom2
		,[Custom3]=@Custom3
		,[Custom4]=@Custom4
		,[Amount]=@Amount
		,[OverrideAmount]=@OverrideAmount
		,[ThirdPartyName]=@ThirdPartyName
		,[ThirdPartyPhone]=@ThirdPartyPhone
		,[TestDueBy]=@TestDueBy
		,[Chargeable]=@Charge
		,[ThirdParty]=@ThirdParty			
WHERE  [LID]=@id

	IF (@Ticket is not null)
		BEGIN 
			update TicketO set fWork=@fWork,DWork=(select Top 1 fDesc from tblWork where id=@fWork) where TicketO.ID=@Ticket
		END    
			
		IF (SELECT COUNT(1) FROM LoadTestItemHistoryPrice WHERE LID=@id AND PriceYear=@PriceYear)!=1
		BEGIN		
			INSERT INTO LoadTestItemHistoryPrice (LID,PriceYear,CreatedBy,CreatedDate,[DefaultAmount],[OverrideAmount],DueDate,Chargeable,ThirdPartyRequired,ThirdPartyName,ThirdPartyPhone)
		VALUES (@Id,@PriceYear,@CreatedBy,GETDATE(),@Amount,@OverrideAmount,@Next,@Charge,@ThirdParty,@ThirdPartyName,@ThirdPartyPhone)	
		END 
		ELSE
        BEGIN
			UPDATE LoadTestItemHistoryPrice 
			SET DefaultAmount=@Amount,[OverrideAmount]	=@OverrideAmount, DueDate=@Next, Chargeable=@Charge,ThirdPartyRequired=@ThirdParty,[ThirdPartyName]=@ThirdPartyName,[ThirdPartyPhone]=@ThirdPartyPhone
			WHERE LID=@id AND PriceYear=@PriceYear
		END 

--Update Log
	Declare @Val varchar(1000)
	if(@loc is not null And @loc != 0)
	BEGIN 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='SafetyTest' and ref= @id and Field='Location Name' order by CreatedStamp desc )		
	Declare @CurrentLocation varchar(100)
	Declare @CurrentLocName varchar(100)
	Select @CurrentLocation = tag from loc where loc = @loc
	Select @CurrentLocName = tag from loc where loc = @old_Loc
	if(@Val<>@CurrentLocation)
	BEGIN
	exec log2_insert @CreatedBy,'SafetyTest',@id,'Location Name',@Val,@CurrentLocation
	end
	Else IF (@CurrentLocName <> @CurrentLocation)
	BEGIN
	exec log2_insert @CreatedBy,'SafetyTest',@id,'Location Name',@CurrentLocName,@CurrentLocation
	END
	END 
 set @Val=null

	
  if(@typeID is not null)
	BEGIN 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='SafetyTest' and ref= @id and Field='Test Type' order by CreatedStamp desc )		
	if(@Val<>@typeID)
	BEGIN
	exec log2_insert @CreatedBy,'SafetyTest',@id,'Test Type',@Val,@typeID
	end
	Else IF (@old_typeID <> @typeID)
	BEGIN
	exec log2_insert @CreatedBy,'SafetyTest',@id,'Test Type',@old_typeID,@typeID
	END
	end
 set @Val=null 
 
   if(@Last is not null)
	BEGIN 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='SafetyTest' and ref= @id and Field='Last Due Date' order by CreatedStamp desc )		
	if(@Val<>@Last)
	BEGIN
	exec log2_insert @CreatedBy,'SafetyTest',@id,'Last Due Date',@Val,@Last
	end
	Else IF (@old_Last <> @Last)
	BEGIN
	exec log2_insert @CreatedBy,'SafetyTest',@id,'Last Due Date',@old_Last,@Last
	END
	end
 set @Val=null 

  

   if(@Next is not null)
	BEGIN 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='SafetyTest' and ref= @id and Field='Next Due Date' order by CreatedStamp desc )		
	if(@Val<>@Next)
	BEGIN
	exec log2_insert @CreatedBy,'SafetyTest',@id,'Next Due Date',@Val,@Next
	end
	Else IF (@old_Next <> @Next)
	BEGIN
	exec log2_insert @CreatedBy,'SafetyTest',@id,'Next Due Date',@old_Next,@Next
	END
	end
 set @Val=null 

    if(@Status is not null)
	BEGIN 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='SafetyTest' and ref= @id and Field='Test Status' order by CreatedStamp desc )		
	if(@Val<>@Status)
	BEGIN
	exec log2_insert @CreatedBy,'SafetyTest',@id,'Test Status',@Val,@Status
	end
	Else IF (@old_Status <> @Status)
	BEGIN
	exec log2_insert @CreatedBy,'SafetyTest',@id,'Test Status',@old_Status,@Status
	END
	end
 set @Val=null 

     if(@Ticket is not null)
	BEGIN 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='SafetyTest' and ref= @id and Field='Ticket' order by CreatedStamp desc )		
	if(@Val<>@Ticket)
	BEGIN
	exec log2_insert @CreatedBy,'SafetyTest',@id,'Ticket',@Val,@Ticket
	END 
	Else IF (@old_Ticket <> @Ticket)
	BEGIN
	exec log2_insert @CreatedBy,'SafetyTest',@id,'Ticket',@old_Ticket,@Ticket
	END
	END 
 set @Val=null 
 
if(@LastDue is not null)
	BEGIN 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='SafetyTest' and ref= @id and Field='LastDue' order by CreatedStamp desc )		
	if(@Val<>@LastDue)
	BEGIN
	exec log2_insert @CreatedBy,'SafetyTest',@id,'LastDue',@Val,@LastDue
	end
	Else IF (@old_LastDue <> @LastDue)
	BEGIN
	exec log2_insert @CreatedBy,'SafetyTest',@id,'LastDue',@old_LastDue,@LastDue
	END
	end
 set @Val=null 
    if(@JobId is not null)
	BEGIN 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='SafetyTest' and ref= @id and Field='Project' order by CreatedStamp desc )		
	if(@Val<>@JobId)
	BEGIN
	exec log2_insert @CreatedBy,'SafetyTest',@id,'Project',@Val,@JobId
	end
	Else IF (@old_JobId <> @JobId)
	BEGIN
	exec log2_insert @CreatedBy,'SafetyTest',@id,'Project',@old_JobId,@JobId
	END
	end
 set @Val=null 
    if(@fWork is not null)
	BEGIN 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='SafetyTest' and ref= @id and Field='fWork' order by CreatedStamp desc )		
	if(@Val<>@fWork)
	BEGIN
	exec log2_insert @CreatedBy,'SafetyTest',@id,'fWork',@Val,@fWork
	end
	Else IF (@old_fWork <> @fWork)
	BEGIN
	exec log2_insert @CreatedBy,'SafetyTest',@id,'fWork',@old_fWork,@fWork
	END
	end
 set @Val=null 


    if(@DWork is not null)
	BEGIN 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='SafetyTest' and ref= @id and Field='DWork' order by CreatedStamp desc )		
	if(@Val<>@DWork)
	BEGIN
	exec log2_insert @CreatedBy,'SafetyTest',@id,'DWork',@Val,@DWork
	end
	Else IF (@old_DWork <> @DWork)
	BEGIN
	exec log2_insert @CreatedBy,'SafetyTest',@id,'DWork',@old_fWork,@DWork
	END
	end
 set @Val=null 
 if(@Custom1 is not null)
	BEGIN 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='SafetyTest' and ref= @id and Field='Custom1' order by CreatedStamp desc )		
	if(@Val<>@Custom1)
	BEGIN
	exec log2_insert @CreatedBy,'SafetyTest',@id,'Custom1',@Val,@Custom1
	end
	Else IF (@old_Custom1 <> @Custom1)
	BEGIN
	exec log2_insert @CreatedBy,'SafetyTest',@id,'Custom1',@old_Custom1,@Custom1
	END
	end
 set @Val=null 
 if(@Custom2 is not null)
	BEGIN 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='SafetyTest' and ref= @id and Field='Custom2' order by CreatedStamp desc )		
	if(@Val<>@Custom2)
	BEGIN
	exec log2_insert @CreatedBy,'SafetyTest',@id,'Custom2',@Val,@Custom2
	end
	Else IF (@old_Custom2 <> @Custom2)
	BEGIN
	exec log2_insert @CreatedBy,'SafetyTest',@id,'Custom2',@old_Custom2,@Custom2
	END
	end
 set @Val=null 
	
 if(@Custom3 is not null)
	BEGIN 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='SafetyTest' and ref= @id and Field='Custom3' order by CreatedStamp desc )		
	if(@Val<>@Custom3)
	BEGIN
	exec log2_insert @CreatedBy,'SafetyTest',@id,'Custom3',@Val,@Custom3
	end
	Else IF (@old_Custom3 <> @Custom3)
	BEGIN
	exec log2_insert @CreatedBy,'SafetyTest',@id,'Custom3',@old_Custom3,@Custom3
	END
	end
 set @Val=null 

 if(@Custom4 is not null)
	BEGIN 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='SafetyTest' and ref= @id and Field='Custom4' order by CreatedStamp desc )		
	if(@Val<>@Custom4)
	BEGIN
	exec log2_insert @CreatedBy,'SafetyTest',@id,'Custom4',@Val,@Custom4
	end
	Else IF (@old_Custom4<> @Custom4)
	BEGIN
	exec log2_insert @CreatedBy,'SafetyTest',@id,'Custom4',@old_Custom4,@Custom4
	END
	end
 set @Val=null 
	
	 if(@Amount is not null)
	BEGIN 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='SafetyTest' and ref= @id and Field='Default Amount' order by CreatedStamp desc )		
	if(@Val<>@Amount)
	BEGIN
	exec log2_insert @CreatedBy,'SafetyTest',@id,'Default Amount',@Val,@Amount
	end
	Else IF (@old_Amount<> @Amount)
	BEGIN
	exec log2_insert @CreatedBy,'SafetyTest',@id,'Default Amount',@old_Amount,@Amount
	END
	end
 set @Val=null
	  
	  	 if(@OverrideAmount is not null)
	BEGIN 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='SafetyTest' and ref= @id and Field='Override Amount' order by CreatedStamp desc )		
	if(@Val<>@OverrideAmount)
	BEGIN
	exec log2_insert @CreatedBy,'SafetyTest',@id,'Override Amount',@Val,@OverrideAmount
	end
	Else IF (@old_OverrideAmount<> @OverrideAmount)
	BEGIN
	exec log2_insert @CreatedBy,'SafetyTest',@id,'Override Amount',@old_OverrideAmount,@OverrideAmount
	END
	end
 set @Val=NULL
 
 	  	 if(@ThirdPartyName is not null)
	BEGIN 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='SafetyTest' and ref= @id and Field='Third Party Name' order by CreatedStamp desc )		
	if(@Val<>@ThirdPartyName)
	BEGIN
	exec log2_insert @CreatedBy,'SafetyTest',@id,'Third Party Name',@Val,@ThirdPartyName
	end
	Else IF (@old_ThirdPartyName<> @ThirdPartyName)
	BEGIN
	exec log2_insert @CreatedBy,'SafetyTest',@id,'Third Party Name',@old_ThirdPartyName,@ThirdPartyName
	END
	end
 set @Val=NULL
 
 if(@ThirdPartyPhone is not null)
	BEGIN 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='SafetyTest' and ref= @id and Field='Third Party Phone' order by CreatedStamp desc )		
	if(@Val<>@ThirdPartyPhone)
	BEGIN
	exec log2_insert @CreatedBy,'SafetyTest',@id,'Third Party Phone',@Val,@ThirdPartyPhone
	end
	Else IF (@old_ThirdPartyPhone<> @ThirdPartyPhone)
	BEGIN
	exec log2_insert @CreatedBy,'SafetyTest',@id,'Third Party Phone',@old_ThirdPartyPhone,@ThirdPartyPhone
	END
	end
 set @Val=NULL
 
  if(@TestDueBy is not null)
	BEGIN 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='SafetyTest' and ref= @id and Field='Test Due By' order by CreatedStamp desc )		
	if(@Val<>@TestDueBy)
	BEGIN
	exec log2_insert @CreatedBy,'SafetyTest',@id,'Test Due By',@Val,@TestDueBy
	end
	Else IF (@old_TestDueBy<> @TestDueBy)
	BEGIN
	exec log2_insert @CreatedBy,'SafetyTest',@id,'Test Due By',@old_ThirdPartyPhone,@TestDueBy
	END
	end
 set @Val=NULL
 
   if(@Charge is not null)
	BEGIN 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='SafetyTest' and ref= @id and Field='Chargeable' order by CreatedStamp desc )		
	if(@Val<>@Charge)
	BEGIN
	exec log2_insert @CreatedBy,'SafetyTest',@id,'Chargeable',@Val,@Charge
	end
	Else IF (@old_Charge<> @Charge)
	BEGIN
	exec log2_insert @CreatedBy,'SafetyTest',@id,'Chargeable',@old_Charge,@Charge
	END
	end
 set @Val=NULL
 
   if(@ThirdParty is not null)
	BEGIN 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='SafetyTest' and ref= @id and Field='Third Party' order by CreatedStamp desc )		
	if(@Val<>@ThirdParty)
	BEGIN
	exec log2_insert @CreatedBy,'SafetyTest',@id,'Third Party',@Val,@ThirdParty
	end
	Else IF (@old_ThirdParty<> @ThirdParty)
	BEGIN
	exec log2_insert @CreatedBy,'SafetyTest',@id,'Third Party',@old_ThirdParty,@ThirdParty
	END
	end
 set @Val=NULL
 --=========================================
	-- Update for test cover
	
DECLARE @TestTypeCover INT
DECLARE @LID INT
		
SET @TestTypeCover= ISNULL((SELECT TestTypeCoverID FROM TestTypeCover WHERE TestTypeID=@typeID),0)
IF @TestTypeCover!=0
BEGIN
	SET @LID=(SELECT LID FROM LoadTestItem WHERE Elev=@Elev AND ID= @TestTypeCover AND YEAR(Next)=YEAR(@Next))
				
	SELECT 
		@old_Amount=Amount
		,@old_OverrideAmount=OverrideAmount		
		,@old_Charge=[Chargeable]		
	FROM [LoadTestItem] WHERE LID=@LID

			UPDATE LoadTestItem
			SET Chargeable=0 , Amount=0,OverrideAmount=0
			WHERE Elev=@Elev AND ID= @TestTypeCover AND YEAR(Next)=YEAR(@Next)
			

			UPDATE LoadTestItemHistoryPrice
			SET Chargeable=0 , DefaultAmount=0,OverrideAmount=0
			WHERE PriceYear=YEAR(@Next) AND LID= (SELECT LID FROM LoadTestItem WHERE Elev=@Elev AND ID= @TestTypeCover AND YEAR(Next)=YEAR(@Next) )



		set @Val=null 
		set @Amount=0
		set @OverrideAmount=0
		set @Charge=0
		 if(@Amount is not null)
		BEGIN 	
      		Set @Val =(select Top 1 newVal  from log2 where screen='SafetyTest' and ref= @LID and Field='Default Amount' order by CreatedStamp desc )		
		if(@Val<>@Amount)
		BEGIN
		exec log2_insert @CreatedBy,'SafetyTest',@LID,'Default Amount',@Val,@Amount
		end
		Else IF (@old_Amount<> @Amount)
		BEGIN
		exec log2_insert @CreatedBy,'SafetyTest',@LID,'Default Amount',@old_Amount,@Amount
		END
		end
	 set @Val=null
	  
	  		 if(@OverrideAmount is not null)
		BEGIN 	
      		Set @Val =(select Top 1 newVal  from log2 where screen='SafetyTest' and ref= @LID and Field='Override Amount' order by CreatedStamp desc )		
		if(@Val<>@OverrideAmount)
		BEGIN
		exec log2_insert @CreatedBy,'SafetyTest',@LID,'Override Amount',@Val,@OverrideAmount
		end
		Else IF (@old_OverrideAmount<> @OverrideAmount)
		BEGIN
		exec log2_insert @CreatedBy,'SafetyTest',@LID,'Override Amount',@old_OverrideAmount,@OverrideAmount
		END
		end
	 set @Val=NULL
 
	   if(@Charge is not null)
		BEGIN 	
      		Set @Val =(select Top 1 newVal  from log2 where screen='SafetyTest' and ref= @LID and Field='Chargeable' order by CreatedStamp desc )		
		if(@Val<>@Charge)
		BEGIN
		exec log2_insert @CreatedBy,'SafetyTest',@LID,'Chargeable',@Val,@Charge
		end
		Else IF (@old_Charge<> @Charge)
		BEGIN
		exec log2_insert @CreatedBy,'SafetyTest',@LID,'Chargeable',@old_Charge,@Charge
		END
		end
	 set @Val=NULL 
END 
END 