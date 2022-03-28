CREATE PROCEDURE [dbo].[spUpdateTestTicket]	
	@id int		  
    ,@Status int
    ,@Ticket int =NULL
    ,@CreateTickeForAll BIT=0
	,@UpdatedBy varchar(200)
AS 
BEGIN 		


	Declare @old_Status int
	Declare @old_Ticket int


	SELECT 
        @old_Status=Status
       , @old_Ticket=Ticket
        
	FROM [LoadTestItem] WHERE LID=@id
	


	UPDATE [dbo].[LoadTestItem]
	SET
	[Status]=@Status
	,[Ticket]=@Ticket   
	WHERE [LID]=@id	
	
	--Update Log
	Declare @Val varchar(1000)
    if(@Status is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='SafetyTest' and ref= @id and Field='Test Status' order by CreatedStamp desc )		
	if(@Val<>@Status)
	begin
	exec log2_insert @UpdatedBy,'SafetyTest',@id,'Test Status',@Val,@Status
	end
	Else IF (@old_Status <> @Status)
	Begin
	exec log2_insert @UpdatedBy,'SafetyTest',@id,'Test Status',@old_Status,@Status
	END
	end
 set @Val=null 

     if(@Ticket is not null)
	begin 	
      	Set @Val =(select Top 1 newVal  from log2 where screen='SafetyTest' and ref= @id and Field='Ticket' order by CreatedStamp desc )		
	if(@Val<>@Ticket)
	begin
	exec log2_insert @UpdatedBy,'SafetyTest',@id,'Ticket',@Val,@Ticket
	end
	Else IF (@old_Ticket <> @Ticket)
	Begin
	exec log2_insert @UpdatedBy,'SafetyTest',@id,'Ticket',@old_Ticket,@Ticket
	END
	end
 set @Val=null 




	--Update for Test Cover
	DECLARE @TestType INT 
	DECLARE @Loc Int 
	SELECT @TestType=ID, @Loc=Loc FROM LoadTestItem WHERE [LID]=@id
	IF OBJECT_ID('tempdb..#tempLID') IS NOT NULL DROP TABLE #tempLID
	CREATE Table #tempLID(
	LID         INT 	
	)
	INSERT INTO #tempLID
	SELECT LID FROM [LoadTestItem]
	WHERE ISNULL(Ticket,0)=0 AND Loc =@Loc 
		AND LID IN (SELECT lt.LID FROM LoadTestItem lt WHERE Loc=@loc AND ID in(select TestTypeCoverID from TestTypeCover where TestTypeID=@TestType))
		AND LID !=@id

		IF (@CreateTickeForAll=1)
	BEGIN	
		INSERT INTO #tempLID
		SELECT LID FROM [LoadTestItem]		
		WHERE ISNULL(Ticket,0)=0 AND Loc =@Loc 
		AND LID IN (SELECT lt.LID FROM LoadTestItem lt WHERE Loc=@loc AND ID in(select TestTypeID from TestTypeCover where TestTypeCoverID=@TestType))
		AND LID !=@id
    END 
	
	DECLARE @c_LID Int 
	DECLARE cur_LID CURSOR FOR 	
		SELECT  LID from #tempLID
	OPEN cur_LID  
	FETCH NEXT FROM cur_LID INTO @c_LID
	WHILE @@FETCH_STATUS = 0  
		BEGIN

		SELECT 
        @old_Status=Status
       , @old_Ticket=Ticket
        
	FROM [LoadTestItem] WHERE LID=@c_LID
	
			UPDATE [dbo].[LoadTestItem]
			SET
			[Status]=@Status
			,[Ticket]=@Ticket   
			WHERE ISNULL(Ticket,0)=0 AND Loc =@Loc 		
			AND LID !=@c_LID

			  if(@Status is not null)
				begin 	
      				Set @Val =(select Top 1 newVal  from log2 where screen='SafetyTest' and ref= @c_LID and Field='Test Status' order by CreatedStamp desc )		
				if(@Val<>@Status)
				begin
				exec log2_insert @UpdatedBy,'SafetyTest',@c_LID,'Test Status',@Val,@Status
				end
				Else IF (@old_Status <> @Status)
				Begin
				exec log2_insert @UpdatedBy,'SafetyTest',@c_LID,'Test Status',@old_Status,@Status
				END
				end
			 set @Val=null 

				 if(@Ticket is not null)
				begin 	
      				Set @Val =(select Top 1 newVal  from log2 where screen='SafetyTest' and ref= @c_LID and Field='Ticket' order by CreatedStamp desc )		
				if(@Val<>@Ticket)
				begin
				exec log2_insert @UpdatedBy,'SafetyTest',@c_LID,'Ticket',@Val,@Ticket
				end
				Else IF (@old_Ticket <> @Ticket)
				Begin
				exec log2_insert @UpdatedBy,'SafetyTest',@c_LID,'Ticket',@old_Ticket,@Ticket
				END
				end
			 set @Val=null 


		FETCH NEXT FROM cur_LID INTO @c_LID
		END	
	CLOSE cur_LID  
	DEALLOCATE cur_LID  

END