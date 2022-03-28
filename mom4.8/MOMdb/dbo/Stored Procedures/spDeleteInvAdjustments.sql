CREATE PROCEDURE [dbo].[spDeleteInvAdjustments]			
         
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
		

		INSERT INTO tblInventoryWHTrans(InvID, WarehouseID,LocationID ,Hand,Balance,fOrder,[Committed],[Available],Screen,ScreenID,Mode,Date,TransType,Batch,fDate)
		SELECT Item,WarehouseID,locationID,[Quan]*(-1), [Amount]*(-1),0,0,0,'Item Adjustment',@Id,'Delete',GETDATE(),'Revert',Batch,fdate from IAdj WHERE ID=@ID	
		

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

		
		 exec CalculateInventory
	end
GO