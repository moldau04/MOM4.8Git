CREATE PROCEDURE [dbo].[spClosePO] 
	 @PO INT,
     @User NVARCHAR(50)		
AS
BEGIN

DECLARE @POID int
DECLARE @fDesc NVARCHAR(MAX)
DECLARE @Job INT
DECLARE @Phase INT
DECLARE @Inv INT
DECLARE @GL INT
DECLARE @Freight NUMERIC(30,2)
DECLARE @Rquan NUMERIC(30,2)
DECLARE @Billed int
DECLARE @Ticket int
DECLARE @Selected NUMERIC(30,2)
DECLARE @Balance NUMERIC(30,2)
DECLARE @Due datetime
DECLARE @WarehouseID VARCHAR(50)
DECLARE @LocationID int
DECLARE @TypeID int
DECLARE @Line int
DECLARE @POStatus int
DECLARE @BalanceQuan NUMERIC(30,2)
DECLARE @BalanceQuantity NUMERIC(30,2) 

SELECT PO,fDesc,Job,Phase,Inv,GL,Freight,Rquan,Billed,Ticket,Selected,Balance,Due,WarehouseID,LocationID,TypeID,BalanceQuan INTO #TempPO FROM POItem WHERE PO = @PO AND Balance <> 0 

DECLARE db_cursor1 CURSOR FOR 
	
	SELECT PO,fDesc,Job,Phase,Inv,GL,Freight,Rquan,Billed,Ticket,Selected,Balance,Due,WarehouseID,LocationID,TypeID,BalanceQuan  FROM #TempPO 

	OPEN db_cursor1  
	FETCH NEXT FROM db_cursor1 INTO 
		 @POID,@fDesc,@Job,@Phase,@Inv,@GL,@Freight,@Rquan,@Billed,@Ticket,@Selected,@Balance,@Due,@WarehouseID,@LocationID,@TypeID,@BalanceQuan
		
	WHILE @@FETCH_STATUS = 0
	BEGIN  		

		SELECT @Line = MAX(ISNULL(Line,0))+1 FROM POItem WHERE PO = @PO
		print @Line
		INSERT INTO POItem(PO,Line,Quan,fDesc,Price,Amount,Job,Phase,Inv,GL,Freight,Rquan,Billed,Ticket,Selected,Balance,Due,SelectedQuan,BalanceQuan,WarehouseID,LocationID,TypeID,ForceClose)
	    VALUES ( @POID,@Line,0,@fDesc+' Close by '+@User+'  on '+Convert(VARCHAR,GETDATE())+' [Amt# '+CONVERT(VARCHAR,(@Balance*(-1)))+' ]' ,0,0,@Job,@Phase,@Inv,@GL,@Freight,@Rquan,@Billed,@Ticket,@Selected,@Balance*(-1),@Due,1,0,@WarehouseID,@LocationID,@TypeID,1 )
		-------START $$$$ REVERT ONORDER QTY FOR INVENTORY ITEM-------->
		IF ((SELECT Type FROM BOMT WHERE ID = @TypeID) = 'Inventory')
		BEGIN
		SET @BalanceQuantity = (@BalanceQuan*(-1)) 
		EXEC spInsertInInvWHTrans  	@Inv,@WarehouseID,@LocationID,0,0,@BalanceQuantity,0,0,'APBILL',@POID,'Add','None',0
	    
		END
		-------END $$$$ REVERT ONORDER QTY FOR INVENTORY ITEM-------->

	SET  @POID= NULL ;  SET  @fDesc= NULL ;  SET  @Job= NULL ;  SET  @Phase= NULL ;  SET  @Inv= NULL ;  SET  @GL= NULL ;  SET  @Freight= NULL ;  SET  @Rquan= NULL ;  
	SET  @Billed= NULL ;  SET  @Ticket= NULL ;  SET  @Selected= NULL ;  SET  @Balance= NULL; SET @Due= NULL; SET @WarehouseID= NULL; SET @LocationID= NULL;set @TypeID=null; SET @Line = NULL;SET @BalanceQuan = NULL;
	SET @BalanceQuantity = NULL;
	--------------->

	FETCH NEXT FROM db_cursor1 INTO 
		 @POID,@fDesc,@Job,@Phase,@Inv,@GL,@Freight,@Rquan,@Billed,@Ticket,@Selected,@Balance,@Due,@WarehouseID,@LocationID,@TypeID,@BalanceQuan
	END  


CLOSE db_cursor1  
DEALLOCATE db_cursor1

Declare @CurrentStatusVal varchar(50)
SELECT @POStatus = Status FROM PO WHERE PO = @PO
Select @CurrentStatusVal = Case @POStatus WHEN 0 THEN 'Open' WHEN 1 THEN 'Closed' WHEN 2 THEN 'Void' WHEN 3 THEN 'Partial-Quantity' WHEN 4 THEN 'Partial-Amount' WHEN 5 THEN 'Closed At Received PO' END 
IF Exists(SELECT * FROM ReceivePO WHERE PO = @PO AND ISNULL(Status,0) =0)
BEGIN
	UPDATE PO SET Status = 5 WHERE PO = @PO
	exec log2_insert @User,'PO',@PO,'Status',@CurrentStatusVal,'Closed At Received PO'
END
ELSE 
BEGIN
	UPDATE PO SET Status = 1 WHERE PO = @PO
	exec log2_insert @User,'PO',@PO,'Status',@CurrentStatusVal,'Closed'
	exec log2_insert @User,'PO',@PO,'Force-Close','','Yes'
END

DROP TABLE #TempPO

END