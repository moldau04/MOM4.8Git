CREATE PROCEDURE [dbo].[spUpdateInvAdjustments]			
         
			@ID [int],
			@fDate [datetime],
			@fDesc [varchar](255),
			@Quan [numeric](30, 2),
			@Amount [numeric](30, 2) =NULL,
			@Item [int] =NULL,
			@Batch [int] =NULL,
			@TransID [int]= NULL,
			@Acct [int]= NULL
as
	begin
		

		 UPDATE [dbo].[IAdj]
	   SET 
		  [fDate] = @fDate
		  ,[fDesc] = @fDesc
		  ,[Quan] = @Quan
		  ,[Amount] = @Amount
		  ,[Item] = @Item
		  ,[Batch] = @Batch
		  ,[TransID] = @TransID
		  ,[Acct] = @Acct
		WHERE ID=@ID
				
		

	end
GO