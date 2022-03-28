Create Procedure [dbo].[spUpdateStatusProposalForm]
@Status varchar(50)   
,@UpdatedBy varchar(50) 
,@ID int 
AS
Begin
UPDATE [dbo].[ProposalForm]
   SET 
      [UpdatedBy] = @UpdatedBy
      ,[UpdatedOn] =GETDATE()      
      ,[Status] = @Status	  
     Where ID=@ID
UPDATE [dbo].[ProposalFormDetail]
   SET        
      [Status] = @Status	  
     Where ProposalID=@ID
End