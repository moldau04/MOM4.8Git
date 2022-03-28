CREATE PROCEDURE [dbo].[spCreateInvAdjustments]			
         
			
			@fDate [datetime],
			@fDesc [varchar](255),
			@Quan [numeric](30, 2),
			@Amount [numeric](30, 2) =NULL,
			@Item [int]= NULL,
			@Batch [int] =NULL,
			@TransID [int] =NULL,
			@Acct [int]= NULL,
			@WarehouseID varchar(5)=null,
			@locationID int=null,

			@Type int=0
AS
	BEGIN

		DECLARE @Id int
		SET @Id=(SELECT ISNULL(MAX(ID),0)+1 AS ID FROM IAdj)
		INSERT INTO [dbo].[IAdj]
           (
		   [ID]
			,[fDate]
           ,[fDesc]
           ,[Quan]
           ,[Amount]
           ,[Item]
           ,[Batch]
           ,[TransID]
           ,[Acct]
		   ,[WarehouseID]
		   ,[LocationID],
		   [Type]
		   )
     VALUES
           (
			@Id
			,@fDate
           ,@fDesc
           ,@Quan
           ,@Amount
           ,@Item
           ,@Batch
           ,@TransID
           ,@Acct
		   ,@WarehouseID
		   ,@locationID
		    ,@Type
		   )	

		 INSERT INTO tblInventoryWHTrans(InvID, WarehouseID,LocationID ,Hand,Balance,fOrder,[Committed],[Available],Screen,ScreenID,Mode,Date,TransType,Batch,fDate)
		 VALUES (@Item,@WarehouseID,@locationID,@Quan,@Amount,0,0,0,'Item Adjustment',@Id,'Add',GETDATE(),'In',@Batch,@fDate)
		 
		 exec CalculateInventory

		SELECT @Id

	END
GO

