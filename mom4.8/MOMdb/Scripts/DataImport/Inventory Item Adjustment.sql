-- =============================================
-- Author		:	NK
-- Create date	:	10 DEC, 2019
-- Description	:	To insert Item Adjustment
-- =============================================
 

    DECLARE @Acctid INT,
		
		@Item_No INT,
		@OnHand INT,
		@Price DECIMAL(18,2),
		@Total DECIMAL(18,2)

    DECLARE	@InvID			INT,
		@WarehouseID	INT,
		@locationID		INT,
		@fDate			DATETIME = '2018-08-31',
		@fDesc			VARCHAR(100) = 'Inventory Adjustment',
		@Batch			INT,
		@Acct			INT = (SELECT ID FROM Chart WHERE Acct = '3120000'),

		@Ref			INT,

		@TransID		INT
 
	SET @Acctid = (SELECT Label from Custom where Name='DefaultInvGLAcct')
	
	DECLARE DB_CURSOR_New CURSOR FOR 
	SELECT i.ID,Item_no,OnHand,Price,Total FROM Inv AS i
	INNER JOIN Import_ItemAdjustment31August AS it ON it.item_no = i.Name
	WHERE OnHand > 0 
	Order by i.id 
  
    OPEN DB_CURSOR_New FETCH NEXT FROM DB_CURSOR_New INTO @InvID, @Item_No, @OnHand, @Price, @Total
  
    WHILE @@FETCH_STATUS = 0 
	BEGIN  
		
		SET @Batch = (SELECT ISNULL(MAX(Batch),0)+1 AS MAXBatch FROM Trans)

		SELECT TOP 1 @WarehouseID = INW.WarehouseID, @locationID = Wl.ID
		FROM InvWarehouse		AS INW 
		INNER JOIN Warehouse	AS Wh ON Wh.ID = INW.WarehouseID 
		LEFT JOIN WHLoc			AS Wl ON INW.WarehouseID = wl.WarehouseID
		where INW.InvID = @InvID

		EXEC spUpdateInvWarehouseForAjustments @InvID, @WarehouseID, @locationID, @OnHand, @Total

		EXEC @Ref = spCreateInvAdjustments @fDate, @fDesc, @OnHand, @Total, @InvID, @Batch, 0, @Acct, @WarehouseID, @locationID

		EXEC @TransID = AddTransForInvAdjustments @Batch, @fDate, 60, 0, @Ref, @fDesc, @Total, @Acctid, @InvID, @OnHand, 0, 0, 0, 0, NULL

		EXEC spUpdateInvAdjustments @Ref, @fDate, @fDesc, @OnHand, @Total, @InvID, @Batch, @TransID, @Acct

		EXEC spUpdateChartBalanceForInvAdjustments @Acctid, @Total

		Declare @Total_Minus NUMERIC(18, 2) = @Total * -1

		EXEC AddTrans NULL, @Batch, @fDate, 61, 0, @Ref, @fDesc, @Total_Minus, @Acct, NULL, '', 0, 0, 0, 0, NULL
		
		EXEC spUpdateChartBalanceForInvAdjustments @Acct, @Total  ---  @Acct is diff from @Acctid
		 

		FETCH NEXT FROM DB_CURSOR_New INTO @InvID, @Item_No, @OnHand, @Price, @Total	
    END  
  
    CLOSE DB_CURSOR_New  
  
    DEALLOCATE DB_CURSOR_New