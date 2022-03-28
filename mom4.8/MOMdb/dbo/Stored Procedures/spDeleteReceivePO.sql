CREATE PROCEDURE [dbo].[spDeleteReceivePO]
	@RPOId int,
	@UpdatedBy varchar(100) 
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @RPOStatus int = -1;
	DECLARE @Batchid int;
	DECLARE @POId int;
	DECLARE @Line smallint
	DECLARE @Balance numeric(30,2)
	DECLARE @Selected numeric(30,2) = 0.0
	DECLARE @SelectedQuan numeric(30,2) = 0.0
	DECLARE @BalanceQuan numeric(30,2) = 0.0
	DECLARE @Amount numeric(30,2) = 0.0
	DECLARE @Quan numeric(30,2) = 0.0

	DECLARE @Job int
	DECLARE @Phase smallint
	DECLARE @Inv int
	DECLARE @comm numeric(30, 2) = 0
	DECLARE @TypeID int
	DECLARE @OpSq varchar(150)
	DECLARE @NewPOStatus int = 0;
	DECLARE @IsClosingPO int = 1;
	--Declare @DateCompare varchar(30)
	--Declare @Int int
	DECLARE @text varchar(8000)
	--SELECT @DateCompare = CONVERT(varchar(30), P.fDate, 101) From PO P Where PO = (Select PO From ReceivePO Where ID = @RPOId)
	--SELECT @Int = DATEDIFF(DAY, @DateCompare, '01/01/2019') 
	SET @text = 'SELECT poi.Line
					, poi.Balance + rpoi.Amount as newBalance
					, poi.BalanceQuan + rpoi.Quan as newBalanceQuan
					, poi.Selected - rpoi.Amount as newSelected
					, poi.SelectedQuan - rpoi.Quan as newSelectedQuan
					, poi.Job
					, poi.Phase
					, isnull(b.Type,0) as TypeID
				FROM POItem poi 
					INNER JOIN ReceivePO rpo on poi.PO = rpo.PO
					INNER JOIN RPOItem rpoi on rpoi.ReceivePO = rpo.ID

					LEFT JOIN Job as j ON poi.Job=j.ID 
					LEFT JOIN JobTItem as jt ON jt.Line = poi.Phase and isnull(jt.Job,0) = isnull(j.ID,0)'
			--IF (@Int > 1) 
				SET @text= @text + '    LEFT Join  BOM as b ON b.JobTItemID = jt.ID '
			--ELSE	
			--	SET @text= @text + '    INNER Join  BOM as b ON b.JobTItemID = jt.ID '	
			SET @text= @text + '   	WHERE rpo.ID = '+CONVERT(VARCHAR(100),@RPOId)+' '
			SET @text= @text + '   	and rpoi.POLine = poi.Line'
			
	create table #temp 
		(Line int, newBalance numeric(30,2),newBalanceQuan numeric(30,2), newSelected numeric(30,2),newSelectedQuan numeric(30,2),Job int,Phase int,TypeID int);

	Insert into #temp(Line, newBalance,newBalanceQuan, newSelected ,newSelectedQuan ,Job ,Phase,TypeID) exec (@text)
	-- Check status of Receive PO
	SELECT @RPOStatus=[status], @POId = PO,@Batchid = Batch FROM ReceivePO WHERE @RPOId = id;
	Print @Batchid
	IF @RPOStatus = 0 or @RPOStatus is null
	BEGIN
		-- perform deleting
		BEGIN TRY
		BEGIN TRANSACTION
			-- Update POItem: on Selected, SelectedQuan, Balance, BalanceQuan
			DECLARE db_cursor CURSOR FOR 
				SELECT Line, newBalance,newBalanceQuan, newSelected ,newSelectedQuan ,Job ,Phase,TypeID FROM #temp
				--Drop table #temp
			OPEN db_cursor  
			FETCH NEXT FROM db_cursor INTO 
				@Line
				, @Balance
				, @BalanceQuan
				, @Selected
				, @SelectedQuan
				, @Job
				, @Phase
				, @TypeID
			WHILE @@FETCH_STATUS = 0
			BEGIN
				EXEC [spUpdateJobCommExpDeleteRPO] @Job,@RPOId;
				UPDATE POItem
				SET	Balance = @Balance
					, BalanceQuan = @BalanceQuan
					, Selected = @Selected
					, SelectedQuan = @SelectedQuan
				WHERE PO = @POId and Line = @Line;
				
				IF @Phase IS NOT NULL
				BEGIN				
					SET @comm = ISNULL(
						(SELECT SUM(ISNULL(p.Balance, 0))
						FROM POItem p
							INNER JOIN PO ON p.po = po.po
						WHERE p.Job = @Job
							AND p.Phase = @Phase
							AND po.status IN (0, 3, 4))
						, 0)
						+ ISNULL(
						(SELECT SUM(ISNULL(rp.Amount, 0))
						FROM RPOItem rp
							INNER JOIN ReceivePO r ON r.ID = rp.ReceivePO
							LEFT JOIN POItem p ON r.PO = p.PO AND rp.POLine = p.Line
						WHERE p.Job = @job
							AND p.Phase = @Phase
							AND r.status <> 1)
						, 0);

					IF (@TypeID = 1 OR @TypeID = 2)
					BEGIN
						UPDATE JobTItem
						SET Comm = @comm
						WHERE Type = 1
							AND Job = @Job
							AND Line = @Phase
							AND Code = @OpSq
					END
					ELSE
					BEGIN
						UPDATE JobTItem
						SET Comm = @comm
						WHERE Type = 2
							AND Job = @Job
							AND Line = @Phase
							AND Code = @OpSq
					END
				END
				
				---------------------------------
				SET	@Line = null;
				SET @Balance = null;
				SET @BalanceQuan = null;
				SET @Selected = null;
				SET @SelectedQuan = null;
				SET @Job = null;
				SET	@Phase = null;
				SET	@TypeID = null;
				---------------------------------

				FETCH NEXT FROM db_cursor INTO 
					@Line
					, @Balance
					, @BalanceQuan
					, @Selected
					, @SelectedQuan
					, @Job
					, @Phase
					, @TypeID
			END

			CLOSE db_cursor  
			DEALLOCATE db_cursor

			--Step-1----- Revert Received PO Inventory Items in warehouse ---------------
		--	UPDATE IWH 
		--	 SET IWH.HAND= IWH.HAND    + ( isnull(RPOItem.Quan,0)  * -1  ),
		--	 IWH.BALANCE=  IWH.BALANCE + ( isnull(RPOItem.Amount,0)  * -1),
		--	 IWH.Available= IWH.Available    + ( isnull(RPOItem.Quan,0)  * -1  )
		--	 FROM POItem  
		--	 INNER JOIN   ReceivePO  ON POItem.PO =ReceivePO.PO 
		--	 INNER JOIN RPOItem ON RPOItem.ReceivePO=ReceivePO.ID and RPOItem.POLine=POItem.Line
  --           INNER JOIN INV ON INV.ID=POItem.Inv and INV.Type=0
		--	 INNER JOIN IWAREHOUSELOCADJ IWH ON INV.ID =IWH.INVID
		--	 AND IWH.WAREHOUSEID=POItem.WarehouseID   AND isnull(IWH.LOCATIONID,0)=isnull(POItem.LocationID,0)
  --           WHERE --ReceivePO.PO= @PO  and ReceivePO=@ReceivePo 
		--	 ReceivePO.ID = @RPOId
		--	------- Revert Received PO Inventory Items in warehouse ---------------------
		--	--Step-2----- Revert Received PO Inventory Items ---------------
		--	UPDATE I 
  --      SET i.hand=  (SELECT isnull(sum(isnull(Adj.Hand,0)),0)       FROM IWarehouseLocAdj   adj  WHERE adj.InvID=I.ID),  
  --      I.Balance=   (SELECT isnull(sum(isnull(Adj.Balance,0)),0)    FROM IWarehouseLocAdj   adj  WHERE adj.InvID=I.ID),
		--I.Available= (SELECT isnull(sum(isnull(Adj.Available,0)),0)  FROM IWarehouseLocAdj   adj  WHERE adj.InvID=I.ID),
		--I.Committed= (SELECT isnull(sum(isnull(Adj.Committed,0)),0)  FROM IWarehouseLocAdj   adj  WHERE adj.InvID=I.ID),
		--I.LastUpdateDate=GETDATE()
  --      FROM  INV I WHERE i.Type=0 
			--Step-2----- Revert Received PO Inventory Items ---------------

			-- Revert Enrty for Inv Type RPO --
			DECLARE @LastId int
--SELECT @LastId = ISNULL(MAX(ID),0) FROM Trans
		INSERT INTO Trans (
			--ID,
			[Batch]  
			, [fDate]  
			, [Type] 
		    , [Line] 
			, [Ref] 
			, [fDesc] 
			, [Amount] 
			, [Acct]  
			, [AcctSub]   
			, [Status]   
			, [Sel] 
			, [VInt] 
			, [VDoub]  
			, [EN]   
			, [strRef]
			) 
		SELECT   
			--@LastId+ROW_NUMBER() OVER(ORDER BY ID),
			 @Batchid
			, fDate
			, [Type]  
			, [Line]  
			, [Ref] 
			, 'Deleted Receive PO By '+@UpdatedBy
			, ([Amount] * -1) 
			, [Acct]  
			, [AcctSub]  
			--, cast(  convert(int, (cast( isnull([Status],'0') as numeric(30,2))) * -1) as varchar(10))  
			,[Status]
			, [Sel] 
			, [VInt] 
			, [VDoub]  
			, [EN]   
			, [strRef] 
		FROM Trans 
		WHERE [Batch] =(select Batch from ReceivePO where id=@RPOId AND Batch <>0) 
		
--************************************** REVERT ENTRY FOR INVENTORY BY KHAN **********************************
		SELECT p.PO,p.Line, p.Quan, p.fDesc, p.Price, p.Job, p.Phase,   
p.Rquan, p.Billed, p.Ticket, p.Due, p.GL, p.Freight, p.Inv,  
p.Amount as Ordered,  
p.Selected as PrvIn, 
p.Balance as Outstanding,  
rp.Amount as Received,
p.Quan as OrderedQuan, 
p.SelectedQuan as PrvInQuan,  
p.BalanceQuan as OutstandQuan,  
isnull(rp.Quan,0) as ReceivedQuan,p.WarehouseID,p.LocationID,  
rp.POLine,                                   
rp.ReceivePO,rp.IsReceiveIssued ,            
isNULL((SELECT top 1  1 FROM INV with (nolock) WHERE ID = (p.Inv)and type = 0),0) IsItemsExistsInInventory  , 
( SELECT TOP 1   Wh.Name  FROM InvWarehouse As INW with (nolock) inner join Warehouse AS Wh with (nolock) on Wh.ID = INW.WarehouseID   where  INW.InvID=p.Inv  and  
INW.WareHouseID=p.WarehouseID) As WarehouseName  ,           
(Select top 1 Name from WHLoc WH with (nolock) where WH.WareHouseID = p.WarehouseID and id = p.LocationID) As WarehouseLoc   
    INTO #Temp2
FROM ReceivePO AS r   with (nolock)           
RIGHT JOIN RPOItem AS rp with (nolock) on rp.ReceivePO = r.ID     
INNER JOIN POItem AS p with (nolock) ON p.Line = rp.POLine 
INNER JOIN BOMT ON BOMT.ID = p.TypeID
WHERE r.ID = @RPOID and p.PO = r.PO    AND BOMT.Type = 'Inventory'

----------------------------- REVRRT PO ------------
INSERT INTO tblInventoryWHTrans (InvID,WarehouseID,LocationID,Hand,Balance,fOrder,Committed,Available,Screen,ScreenID,Mode,Date,TransType,Batch,fDate)
		SELECT Inv,WarehouseID,LocationID,0,0,ISNULL(ReceivedQuan,0),0,0,'RPO',@RPOID,'Edit',GETDATE(),'Revert',@Batchid,GETDATE() FROM #Temp2 
		WHERE ReceivePO = @RPOID 
--------------------------------------------

INSERT INTO tblInventoryWHTrans (InvID,WarehouseID,LocationID,Hand,Balance,fOrder,Committed,Available,Screen,ScreenID,Mode,Date,TransType,Batch,fDate)
		SELECT Inv,WarehouseID,LocationID,ISNULL(ReceivedQuan,0)*-1,ISNULL(Received,0)*-1,0,0,0,'RPO',@RPOID,'Delete',GETDATE(),'Revert',@Batchid,GETDATE() FROM #Temp2 
		WHERE ReceivePO = @RPOID 

		DROP TABLE #Temp2

--************************************** REVERT ENTRY FOR INVENTORY BY KHAN ********************************************
			   		 	  	  	   	
			-- Delete RPOItem
			DELETE FROM RPOItem WHERE ReceivePO = @RPOId;
			
			-- Delete ReceivePO
			DELETE FROM ReceivePO WHERE id = @RPOId;

			-- Update status of PO after deleting Receive PO
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
					, @Amount
					, @Quan
				WHILE @@FETCH_STATUS = 0
				BEGIN
					-- TODO: process to update status of PO
					IF @Balance <> @Amount AND @NewPOStatus <> 3
					BEGIN
						SET @NewPOStatus = 4; -- Partial Amount
					END

					IF @BalanceQuan <> @Quan AND @NewPOStatus <> 3
					BEGIN
						SET @NewPOStatus = 3; -- Partial Quantity
					END

					IF @IsClosingPO = 1 AND (@Selected <> @Amount OR @SelectedQuan <> @Quan)
					BEGIN
						SET @IsClosingPO = 0;
					END
					---------------------------------
					SET	@Line = null;
					SET @Balance = null;
					SET @BalanceQuan = null;
					SET @Selected = null;
					SET @SelectedQuan = null;
					SET @Amount = null;
					SET	@Quan = null;
					---------------------------------

					FETCH NEXT FROM db_cursor1 INTO @Line
						, @Balance
						, @BalanceQuan
						, @Selected
						, @SelectedQuan
						, @Amount
						, @Quan
				END

				CLOSE db_cursor1  
				DEALLOCATE db_cursor1

				IF @IsClosingPO = 1
				BEGIN
					--UPDATE PO SET [Status]=5 WHERE PO=@POId AND [Status] <> 1
					SET @NewPOStatus = 5
				END
				--ELSE
				--BEGIN
				--	UPDATE PO SET [Status]=@NewPOStatus WHERE PO=@POId AND [Status] <> 1
				--END
			END
			ELSE
			BEGIN
				--UPDATE PO SET [Status]=0 WHERE PO=@POId AND [Status] <> 1
				SET @NewPOStatus = 0
			END

			UPDATE PO SET [Status]=@NewPOStatus WHERE PO=@POId AND [Status] <> 1
			IF @NewPOStatus = 0
			BEGIN
				UPDATE PO SET [POReceiveBy]=NULL WHERE PO=@POId AND [Status] <> 1
			END

			/** Add log for changing PO Status*/
			DECLARE @CurrentStatus varchar(50)
			SELECT @CurrentStatus = Case [Status] WHEN 0 THEN 'Open' WHEN 1 THEN 'Closed' WHEN 2 THEN 'Void' WHEN 3 THEN 'Partial-Quantity' WHEN 4 THEN 'Partial-Amount' WHEN 5 THEN 'Closed At Received PO' END From PO Where PO =@POId
			DECLARE @CurrentStatusVal varchar(50)
			SELECT @CurrentStatusVal = Case @NewPOStatus WHEN 0 THEN 'Open' WHEN 1 THEN 'Closed' WHEN 2 THEN 'Void' WHEN 3 THEN 'Partial-Quantity' WHEN 4 THEN 'Partial-Amount' WHEN 5 THEN 'Closed At Received PO' END 
			DECLARE @Val varchar(50)
			SET @Val =(select Top 1 newVal  from log2 where screen='PO' and ref= @POId and Field='Status' order by CreatedStamp desc )
			IF (@Val<>@CurrentStatusVal)
			BEGIN
				EXEC log2_insert @UpdatedBy,'PO',@POId,'Status',@Val,@CurrentStatusVal
			END
			ELSE IF (@CurrentStatus <> @CurrentStatusVal)
			BEGIN
				EXEC log2_insert @UpdatedBy,'PO',@POId,'Status',@CurrentStatus,@CurrentStatusVal
			END
			EXEC CalculateInventory
			/** End log*/

			-- Commit transaction
			COMMIT;
		END TRY
		BEGIN CATCH

			SELECT ERROR_MESSAGE()

			IF @@TRANCOUNT>0
				ROLLBACK	
				RAISERROR ('An error has occurred on this page.',16,1)
				RETURN

		END CATCH
	END
	ELSE IF @RPOStatus = 1
	BEGIN
		-- Showing warning Receive PO closed
		RAISERROR ('This receive PO was closed!',16,1)
		RETURN
	END
	ELSE
	BEGIN
		-- Error Receive PO doesn't exits
		RAISERROR ('Error: Receive PO hasn''t existed in database yet!',16,1)
		RETURN
	END
END
GO