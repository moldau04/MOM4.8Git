CREATE Procedure [dbo].[DeleteTransForInvAdjustments] (
        
            @Batch   INT 
          
)

AS BEGIN

	DELETE FROM Trans WHERE Trans.Batch=@Batch

END
GO