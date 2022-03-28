create procedure [dbo].[spUpdateSenderInfoProposalForm]
@SendFrom varchar(50)   
,@SendTo varchar(500) 
,@ID int 
AS
Begin
UPDATE [dbo].[ProposalForm]
   SET      
      [SendOn] =GETDATE()      
      ,[SendTo] = @SendTo  
	  ,[SendFrom]=@SendFrom
	,[SendMailStatus]=1
     Where ID=@ID
End