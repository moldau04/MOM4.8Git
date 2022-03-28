 CREATE PROCEDURE [dbo].[spAddProposalFormForEquipment]
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
,@ID int output
AS
Begin
if not exists(select 1 from ProposalForm where [LocID]=@LocID and [Classification]=@Classification and [YearProposal]=@YearProposal and [Type]=@Type and [ListEquipment]=@ListEquipment)
	begin
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
			   ,[YearProposal])
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
			   ,@YearProposal)

			   Set @ID= @@IDENTITY
	End
ELSE
	BEGIN
		UPDATE [ProposalForm]
		SET    [FileName]=@FileName
			   ,[FilePath]=@FilePath
			   ,[PdfFilePath]=@PdfFilePath
			   ,[ListEquipment]=@ListEquipment
			   ,[Status]=@Status
			   ,[UpdatedBy]=@AddedBy
			   ,[UpdatedOn]=GETDATE()			  
		where [LocID]=@LocID and [Classification]=@Classification and [YearProposal]=@YearProposal and [Type]=@Type and [ListEquipment]=@ListEquipment
	END

end
