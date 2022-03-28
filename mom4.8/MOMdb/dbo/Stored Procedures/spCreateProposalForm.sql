CREATE PROCEDURE [dbo].[spCreateProposalForm]
@LocID  int
,@Classification varchar(50)
,@FileName varchar(100)
,@FilePath varchar(500)
,@PdfFilePath varchar(500)
,@FromDate datetime
,@ToDate datetime
,@AddedBy varchar(50)      
,@Type int
,@Status varchar(50) 
,@ListEquipment varchar(max)
,@YearProposal int
,@ID int OUTPUT
,@Chargable BIT	 =0
,@TestTypeID INT	
AS
BEGIN
IF (SELECT COUNT(1) FROM ProposalForm WHERE ListEquipment=@ListEquipment AND YearProposal=@YearProposal AND Classification=@Classification AND LocID=@LocID AND TestTypeID=@TestTypeID) =0
BEGIN
	INSERT INTO [ProposalForm]
			   ([LocID]
			   ,[Classification]
			   ,[FileName]
			   ,[FilePath]
			   ,[PdfFilePath]           
			   ,[FromDate]
			   ,[ToDate]
			   ,[AddedBy]
			   ,[AddedOn]           
			   ,[Type]
			   ,[Status]			  
			   ,[ListEquipment]
			   ,[YearProposal]
			   ,Chargable
			   ,TestTypeID)
		 VALUES
			   (@LocID
			   ,@Classification
			   ,@FileName
			   ,@FilePath
			   ,@PdfFilePath           
			   ,@FromDate
			   ,@ToDate
			   ,@AddedBy
			   ,GETDATE()          
			   ,@Type
			   ,@Status			   
			   ,@ListEquipment
			   ,@YearProposal
			   ,@Chargable
			   ,@TestTypeID)

			   Set @ID= @@IDENTITY
END 
ELSE
BEGIN
	IF(select CHARINDEX(',',@ListEquipment))=0
	BEGIN
		DECLARE @LID INT
        SET @LID=(SELECT TOP 1 LID FROM LoadTestItem WHERE Elev=@ListEquipment AND YEAR(Next)=@YearProposal AND Loc=@LocID)
		IF (SELECT count (1) FROM ProposalFormDetail WHERE TestID=@LID AND YearProposal=@YearProposal)=0
		BEGIN
		INSERT INTO [ProposalForm]
			   ([LocID]
			   ,[Classification]
			   ,[FileName]
			   ,[FilePath]
			   ,[PdfFilePath]           
			   ,[FromDate]
			   ,[ToDate]
			   ,[AddedBy]
			   ,[AddedOn]           
			   ,[Type]
			   ,[Status]			  
			   ,[ListEquipment]
			   ,[YearProposal]
			   ,Chargable
			   ,TestTypeID)
		 VALUES
			   (@LocID
			   ,@Classification
			   ,@FileName
			   ,@FilePath
			   ,@PdfFilePath           
			   ,@FromDate
			   ,@ToDate
			   ,@AddedBy
			   ,GETDATE()          
			   ,@Type
			   ,@Status			   
			   ,@ListEquipment
			   ,@YearProposal
			   ,@Chargable
			   ,@TestTypeID)

			   Set @ID= @@IDENTITY
        END
        ELSE
        BEGIN
			SET @ID=0
        end
	END
	BEGIN
	SET @ID=0
	END

	
END 

		IF @ID <>0
		BEGIN
		INSERT INTO [ProposalFormDetail]
				   ([ProposalID]
				   ,[EquipmentID]
				   ,[TestID]
				   ,[Status]
				   ,[YearProposal]
				   ,[Chargable]
				   )
		select distinct @ID, Elev,LID,@Status,@YearProposal,isnull(Chargeable,1) 
		from LoadTestItem lti
		left join Elev e on e.ID=lti.Elev
		inner join Loc l on l.Loc=e.Loc
		where 
		 l.Status=0 and e.Status=0 
		AND lti.ID in (SELECT TestTypeCoverID FROM TestTypeCover WHERE  TestTypeID=@TestTypeID)		
		AND lti.loc=@LocID
		AND e.Classification=@Classification
		AND e.ID IN (SELECT items   FROM   dbo.Idsplit(@ListEquipment, ','))
			  And (select count(1) from ProposalFormDetail  pd where pd.[ProposalID]= @ID and pd.[EquipmentID]=e.ID and TestID=lti.LID AND YearProposal=@YearProposal)=0

	----INsert for child
	
			INSERT INTO [ProposalFormDetail]
				([ProposalID]
				,[EquipmentID]
				,[TestID]
				,[Status]
				,[YearProposal]
				,[Chargable]
				)
			SELECT DISTINCT @ID, Elev,LID,@Status,@YearProposal,isnull(Chargeable,0)  
			FROM LoadTestItem lti
			 LEFT JOIN Elev e on e.ID=lti.Elev
			 LEFT JOIN Loc l on e.Loc=e.Loc
			WHERE lti.ID =@TestTypeID
			AND l.Status=0 and e.Status=0 		
			AND lti.loc=@LocID
			AND e.Classification=@Classification
			AND e.ID IN (SELECT items        
			FROM   dbo.Idsplit(@ListEquipment, ','))
			AND (select count(1) from ProposalFormDetail  pd where pd.[ProposalID]= @ID and pd.[EquipmentID]=e.ID and TestID=lti.LID AND YearProposal=@YearProposal)=0
        END 

End

