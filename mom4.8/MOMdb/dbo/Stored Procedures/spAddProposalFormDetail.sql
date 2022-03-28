CREATE PROCEDURE [dbo].[spAddProposalFormDetail]
 @ProposalID  int      
,@EquipmentID int
,@Status varchar(50) 
,@TestID INT
,@YearProposal INT 
,@ID int output
,@Chargable BIT =0
AS
Begin
	if not exists(select 1 from ProposalFormDetail where [ProposalID]=@ProposalID and [EquipmentID]=@EquipmentID and [TestID]=@TestID)
	begin
	
		INSERT INTO [ProposalFormDetail]
			   ([ProposalID]
			   ,[EquipmentID]
			   ,[TestID]
			   ,[Status]
			   ,[YearProposal]
			   ,[Chargable]
			   )
		 VALUES
			   (@ProposalID
			   ,@EquipmentID
			   ,@TestID
			   ,@Status
			   ,@YearProposal
			   ,@Chargable)
			   Set @ID= @@IDENTITY
	END
   
	

	Declare @ElveId int
	Declare @TestTypeID int
	Declare @Loc int
	Declare @Classification varchar(500)
	Declare @lsElveId VARCHAR(500)
	
	
	select @TestTypeID=lti.ID,@Loc=e.Loc, @Classification=e.Classification
	from LoadTestItem  lti
	left join Elev e on e.ID=lti.Elev
	where lti.LID=@TestID

	SELECT ListEquipment FROM ProposalForm WHERE ID=@ProposalID

	SET @lsElveId=ISNULL((SELECT ListEquipment FROM ProposalForm WHERE ID=@ProposalID),'')
	

	INSERT INTO [ProposalFormDetail]
				   ([ProposalID]
				   ,[EquipmentID]
				   ,[TestID]
				   ,[Status]
				   ,[YearProposal]
				   ,[Chargable]
				   )
		select distinct @ProposalID, Elev,LID,@Status,@YearProposal,Chargeable 
		from LoadTestItem lti
		left join Elev e on e.ID=lti.Elev
		inner join Loc l on l.Loc=e.Loc
		where 
		 l.Status=0 and e.Status=0 
		AND lti.ID in (SELECT TestTypeCoverID FROM TestTypeCover WHERE  TestTypeID=@TestTypeID)
		and isnull(Year(Next), Year(GETDATE()))=@YearProposal and lti.loc=@Loc
		and e.Classification=@Classification
		AND e.ID IN (SELECT items   FROM   dbo.Idsplit(@lsElveId, ','))
			  And (select count(1) from ProposalFormDetail  pd where pd.[ProposalID]= @ProposalID and pd.[EquipmentID]=e.ID and TestID=lti.LID)=0

	----INsert for child
	
					INSERT INTO [ProposalFormDetail]
				   ([ProposalID]
				   ,[EquipmentID]
				   ,[TestID]
				   ,[Status]
				   ,[YearProposal]
				   ,[Chargable]
				   )
				   select distinct @ProposalID, Elev,LID,@Status,@YearProposal,Chargeable 
		from LoadTestItem lti
		left join Elev e on e.ID=lti.Elev
		left join Loc l on e.Loc=e.Loc
		where lti.ID =@TestTypeID
		 AND l.Status=0 and e.Status=0 
		and isnull(Year(Next), Year(GETDATE()))=@YearProposal and lti.loc=@Loc
		and e.Classification=@Classification
		AND e.ID IN (SELECT items        
                               FROM   dbo.Idsplit(@lsElveId, ','))
	  And (select count(1) from ProposalFormDetail  pd where pd.[ProposalID]= @ProposalID and pd.[EquipmentID]=e.ID and TestID=lti.LID)=0


	  

	
	--DECLARE cur_item CURSOR FOR 	

	--	select distinct @ProposalID, Elev,LID,@Status,@YearProposal,Chargeable 
	--	from LoadTestItem lti
	--	left join Elev e on e.ID=lti.Elev
	--	inner join Loc l on l.Loc=e.Loc
	--	where 
	--	 l.Status=0 and e.Status=0 
	--	AND lti.ID in (SELECT TestTypeCoverID FROM TestTypeCover WHERE  TestTypeID=@TestTypeID)
	--	and isnull(Year(Next), Year(GETDATE()))=@YearProposal and lti.loc=@Loc
	--	and e.Classification=@Classification
	--	AND e.ID IN (SELECT items   FROM   dbo.Idsplit(@lsElveId, ','))
	--		  And (select count(1) from ProposalFormDetail  pd where pd.[ProposalID]= @ProposalID and pd.[EquipmentID]=e.ID and TestID=lti.LID)=0
	--OPEN cur_item  
	--FETCH NEXT FROM cur_item INTO @ProposalID ,@EquipmentID,@TestID,@Status,@YearProposal,@Chargable
	--WHILE @@FETCH_STATUS = 0  
	--	BEGIN			
	--		IF (select count(1) from ProposalFormDetail where[ProposalID]= @ProposalID and [EquipmentID]=@EquipmentID and TestID=@TestID)=0
	--		BEGIN
	--				INSERT INTO [ProposalFormDetail]
	--			   ([ProposalID]
	--			   ,[EquipmentID]
	--			   ,[TestID]
	--			   ,[Status]
	--			   ,[YearProposal]
	--			   ,[Chargable]
	--			   )
	--			 VALUES
	--			   (@ProposalID
	--			   ,@EquipmentID
	--			   ,@TestID
	--			   ,@Status
	--			   ,@YearProposal
	--			   ,@Chargable)
	--		END	
			  
	--	FETCH NEXT FROM cur_item INTO @ProposalID ,@EquipmentID,@TestID,@Status,@YearProposal,@Chargable
	--	END	
	--CLOSE cur_item  
	--DEALLOCATE cur_item  


	--DECLARE cur_item CURSOR FOR 	
	--	select distinct @ProposalID, Elev,LID,@Status,@YearProposal,Chargeable 
	--	from LoadTestItem lti
	--	left join Elev e on e.ID=lti.Elev
	--	left join Loc l on e.Loc=e.Loc
	--	where lti.ID =@TestTypeID
	--	 AND l.Status=0 and e.Status=0 
	--	and isnull(Year(Next), Year(GETDATE()))=@YearProposal and lti.loc=@Loc
	--	and e.Classification=@Classification
	--	AND e.ID IN (SELECT items        
 --                              FROM   dbo.Idsplit(@lsElveId, ','))
	--  And (select count(1) from ProposalFormDetail  pd where pd.[ProposalID]= @ProposalID and pd.[EquipmentID]=e.ID and TestID=lti.LID)=0
	--OPEN cur_item  
	--FETCH NEXT FROM cur_item INTO @ProposalID ,@EquipmentID,@TestID,@Status,@YearProposal,@Chargable
	--WHILE @@FETCH_STATUS = 0  
	--	BEGIN			
	--		IF (select count(1) from ProposalFormDetail where[ProposalID]= @ProposalID and [EquipmentID]=@EquipmentID and TestID=@TestID)=0
	--		BEGIN		

	--				INSERT INTO [ProposalFormDetail]
	--			   ([ProposalID]
	--			   ,[EquipmentID]
	--			   ,[TestID]
	--			   ,[Status]
	--			   ,[YearProposal]
	--			   ,[Chargable]
	--			   )
	--			 VALUES
	--			   (@ProposalID
	--			   ,@EquipmentID
	--			   ,@TestID
	--			   ,@Status
	--			   ,@YearProposal
	--			   ,@Chargable)
	--		END	
			  
	--	FETCH NEXT FROM cur_item INTO @ProposalID ,@EquipmentID,@TestID,@Status,@YearProposal,@Chargable
	--	END	
	--CLOSE cur_item  
	--DEALLOCATE cur_item  


END
