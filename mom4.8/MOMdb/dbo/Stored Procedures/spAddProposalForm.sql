CREATE PROCEDURE [dbo].[spAddProposalForm]
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

		
End

