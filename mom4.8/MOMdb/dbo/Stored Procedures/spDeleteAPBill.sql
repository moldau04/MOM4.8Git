CREATE PROCEDURE [dbo].[spDeleteAPBill]
	@id int
AS
BEGIN
	
	SET NOCOUNT ON;

BEGIN TRY
BEGIN TRANSACTION
	DECLARE @Line smallint
    DECLARE @Balance numeric(30,2)
	DECLARE @Selected numeric(30,2) = 0.0
	DECLARE @SelectedQuan numeric(30,2) = 0.0
	DECLARE @BalanceQuan numeric(30,2) = 0.0
	DECLARE @BalAmount numeric(30,2) = 0.0
	DECLARE @Quan numeric(30,2) = 0.0
	DECLARE @NewPOStatus int = 0;
	DECLARE @IsClosingPO int = 1;
	DECLARE @POId int
	declare @RPOID int

	declare @vendor int
	declare @batch int
	declare @amount numeric(30,2)
	declare @tid int
	declare @type smallint
	declare @acct int
	declare @tamount numeric(30,2)
	declare @jobId int
	declare @TRID int
	declare @phase int
	declare @Comm numeric(30,2)
	declare @MatActual numeric(30,2)	
	select @vendor=Vendor, @batch=Batch, @amount=Amount, @TRID=TRID, @POId = ISNULL(PO,0), @RPOID = ISNULL(ReceivePO,0) FROM PJ WHERE ID=@id
	
	IF(@RPOID = 0 and @POId = 0)
	BEGIN  
		INSERT INTO tblInventoryWHTrans(InvID, WarehouseID,LocationID ,Hand,Balance,fOrder,[Committed],[Available],Screen,ScreenID,Mode,Date,TransType,Batch,FDate)
		--SELECT InvID,WarehouseID,LocationID,ISNULL(Hand,0)*-1,isnull(Balance,0)*-1,0,0,0,'APBILL',@id,'Edit',GETDATE(),'Revert' FROM tblInventoryWHTrans WHERE ScreenID = @id AND Screen = 'APBILL' 
		SELECT ItemId, Warehouse , WHLocID ,ISNULL(CAST(Quan AS NUMERIC(30,2)),0)*-1,isnull(CAST(Amount AS NUMERIC(30,2)),0)*-1,0,0,0,'APBILL',@id,'Delete',GETDATE(),'Revert',@batch,GETDATE() FROM APBillItem WHERE Batch =@Batch AND phase= 'Inventory'
	END

	IF(@RPOID <> 0 and @POId <> 0)
	BEGIN  
		INSERT INTO tblInventoryWHTrans(InvID, WarehouseID,LocationID ,Hand,Balance,fOrder,[Committed],[Available],Screen,ScreenID,Mode,Date,TransType,Batch,FDate)
		--SELECT InvID,WarehouseID,LocationID,ISNULL(Hand,0)*-1,isnull(Balance,0)*-1,0,0,0,'APBILL',@id,'Edit',GETDATE(),'Revert' FROM tblInventoryWHTrans WHERE ScreenID = @id AND Screen = 'APBILL' 
		SELECT ItemId, Warehouse , WHLocID ,ISNULL(CAST(Quan AS NUMERIC(30,2)),0)*-1,isnull(CAST(Amount AS NUMERIC(30,2)),0)*-1,0,0,0,'APBILL',@id,'Delete',GETDATE(),'Revert',@Batch,GETDATE() FROM APBillItem WHERE Batch =@Batch AND phase= 'Inventory'
		EXCEPT
		SELECT POItem.Inv, POItem.WarehouseID , POItem.LocationID ,isnull(RPOItem.Quan,0)  * -1,isnull(RPOItem.Amount,0)  * -1,0,0,0,'APBILL',@id,'Delete',GETDATE(),'Revert',@Batch,GETDATE()
			 FROM POItem  
			 INNER JOIN   ReceivePO  ON POItem.PO =ReceivePO.PO 
			 INNER JOIN RPOItem ON RPOItem.ReceivePO=ReceivePO.ID and RPOItem.POLine=POItem.Line
             INNER JOIN INV ON INV.ID=POItem.Inv and INV.Type=0
			 INNER JOIN IWAREHOUSELOCADJ IWH ON INV.ID =IWH.INVID
			 AND IWH.WAREHOUSEID=POItem.WarehouseID   AND isnull(IWH.LOCATIONID,0)=isnull(POItem.LocationID,0)
             WHERE ReceivePO.PO= @POId  and ReceivePO=@RPOID
	END


	DECLARE db_cursor CURSOR FOR 

		SELECT ID, Acct, type, Amount, Vint, CONVERT(INT,VDoub) As VDoub FROM Trans WHERE batch=@batch

	OPEN db_cursor  
	FETCH NEXT FROM db_cursor INTO @tid, @acct, @type, @amount, @jobId, @phase

	WHILE @@FETCH_STATUS = 0
	BEGIN  	
	
		DELETE FROM PJItem WHERE TRID=@tid
		
		IF (@jobId>0)
		BEGIN

			DELETE FROM JobI WHERE TransID=@tid		-- delete JobI details from JobI 

			EXEC spUpdateJobMatExp @jobId

		    EXEC spUpdateJobOtherExp @jobId

			IF (@phase > 0)
			BEGIN

				SET @Comm = ISNULL((SELECT sum(isnull(p.Balance,0)) from POItem p 
										INNER JOIN PO on p.po = po.po
										WHERE p.Job = @jobId and p.Phase = @phase and po.status in (0,3,4)),0) + 
								ISNULL((SELECT sum(isnull(rp.Amount,0)) from RPOItem rp 
										INNER JOIN ReceivePO r on r.ID = rp.ReceivePO
										LEFT JOIN POItem p on r.PO = p.PO AND rp.POLine = p.Line
										WHERE p.Job = @jobId and p.Phase = @phase and r.status = 0),0)
			
				IF (@type =1 or @type =2)
			BEGIN
				SET @MatActual = isnull((select sum(isnull(amount,0)) from jobi 
												where type = 1 
														and job = @jobId 
														and phase = @phase
														and (TransID > 0 or isnull(Labor,0) = 0)),0)

				UPDATE JobTItem 
				SET 
					Actual = @MatActual, 
					Comm = @Comm 
				WHERE		Type = 1 
						AND Job = @jobId 
						AND Line = @phase
						END
						
			ELSE
		   BEGIN
				SET @MatActual = isnull((select sum(isnull(amount,0)) from jobi 
												where type = 2
														and job = @JobId 
														and phase = @phase
														and (TransID > 0 or isnull(Labor,0) = 0)
														),0)
           UPDATE JobTItem 
				SET 
					Actual = @MatActual, 
					Comm = @Comm 
				WHERE		Type = 2
						AND Job = @JobId 
						AND Line = @phase 

           END
		   END

		END
		SET @amount = @amount * -1
	
		DELETE FROM trans where ID=@tid				-- delete transaction from trans
	 

	FETCH NEXT FROM db_cursor INTO  @tid, @acct, @type, @amount, @jobId, @phase

	END	

	CLOSE db_cursor  
	DEALLOCATE db_cursor

	DELETE FROM OpenAP where PJID=@id				-- delete from OpenAP 
	

	DELETE FROM PJ where ID=@id						-- delete from PJ 

		---------------$$$$ Inventory  Adjustment $$$ --------------------

		--UPDATE IWH 
		--SET IWH.HAND= IWH.HAND +  (I.QUAN * -1 )    ,
		--IWH.BALANCE = IWH.BALANCE +  (I.AMOUNT * - 1 )   
		-- FROM IADJ I  
		-- INNER JOIN	IWAREHOUSELOCADJ IWH ON I.ITEM =IWH.INVID
		--  AND I.WAREHOUSEID=IWH.WAREHOUSEID 
		-- WHERE BATCH=@Batch 		AND ISNULL(I.LOCATIONID,0)=0

		-- UPDATE IWH 
		-- SET IWH.HAND= IWH.HAND +  (I.QUAN * -1 )    ,
		-- IWH.BALANCE = IWH.BALANCE +  (I.AMOUNT * - 1 )    
		-- FROM IADJ I  
		-- INNER JOIN		IWAREHOUSELOCADJ IWH ON I.ITEM =IWH.INVID 
		-- AND I.WAREHOUSEID=IWH.WAREHOUSEID  
		-- AND I.LOCATIONID=IWH.LOCATIONID
		-- WHERE BATCH=@Batch 
	 --    AND ISNULL(I.LOCATIONID,0) <> 0  

		-- DELETE FROM IADJ WHERE Batch = @Batch
		
	 --     UPDATE I 
  --        SET i.hand=  (SELECT isnull(sum(isnull(Adj.Hand,0)),0)       FROM IWarehouseLocAdj   adj  WHERE adj.InvID=I.ID),  
  --        I.Balance=   (SELECT isnull(sum(isnull(Adj.Balance,0)),0)    FROM IWarehouseLocAdj   adj  WHERE adj.InvID=I.ID),
		--  I.Available= (SELECT isnull(sum(isnull(Adj.Available,0)),0)  FROM IWarehouseLocAdj   adj  WHERE adj.InvID=I.ID),
		--  I.Committed= (SELECT isnull(sum(isnull(Adj.Committed,0)),0)  FROM IWarehouseLocAdj   adj  WHERE adj.InvID=I.ID),
		--  I.LastUpdateDate=GETDATE()
  --        FROM  INV I WHERE i.Type=0 

	------------------------------END------------------------------------


 
	EXEC [dbo].[spUpdateVendorBalance] @Vendor

	EXEC [dbo].[spCalChartBalance]							-- calculate chart balance

	-- Update status of PO and  Receive PO after deleting PJ ----
			IF EXISTS (SELECT TOP 1 1 FROM ReceivePO WHERE PO = @POId)
			BEGIN
				DECLARE db_cursor1 CURSOR FOR
					SELECT poi.Line
						, poi.Balance
						, poi.BalanceQuan
						, poi.Selected
						, poi.SelectedQuan
						, poi.Amount
						, poi.Quan
					FROM POItem poi 
					WHERE poi.PO = @POId
					
				OPEN db_cursor1
				FETCH NEXT FROM db_cursor1 INTO @Line
					, @Balance
					, @BalanceQuan
					, @Selected
					, @SelectedQuan
					, @BalAmount
					, @Quan
				WHILE @@FETCH_STATUS = 0
				BEGIN
					-- TODO: process to update status of PO
					IF @Balance <> @BalAmount AND @NewPOStatus <> 3
					BEGIN
						SET @NewPOStatus = 4; -- Partial Amount
					END

					IF @BalanceQuan <> @Quan AND @NewPOStatus <> 3
					BEGIN
						SET @NewPOStatus = 3; -- Partial Quantity
					END

					IF @IsClosingPO = 1 AND (@Selected <> @BalAmount OR @SelectedQuan <> @Quan)
					BEGIN
						SET @IsClosingPO = 0;
					END
					---------------------------------
					SET	@Line = null;
					SET @Balance = null;
					SET @BalanceQuan = null;
					SET @Selected = null;
					SET @SelectedQuan = null;
					SET @BalAmount = null;
					SET	@Quan = null;
					---------------------------------

					FETCH NEXT FROM db_cursor1 INTO @Line
						, @Balance
						, @BalanceQuan
						, @Selected
						, @SelectedQuan
						, @BalAmount
						, @Quan
				END

				CLOSE db_cursor1  
				DEALLOCATE db_cursor1

				IF @IsClosingPO = 1
				BEGIN
					SET @NewPOStatus = 5
				END
			END
			ELSE
			BEGIN
				SET @NewPOStatus = 0
			END
			UPDATE ReceivePO SET [Status] = 0 Where ID = @RPOID
			UPDATE PO SET [Status]=@NewPOStatus WHERE PO=@POId
			EXEC CalculateInventory
		-- End Update status of PO and  Receive PO ---
 	COMMIT 
	END TRY
	BEGIN CATCH

	SELECT ERROR_MESSAGE()

    IF @@TRANCOUNT>0
        ROLLBACK	
		RAISERROR ('An error has occurred on this page.',16,1)
        RETURN

	END CATCH
	
END

GO