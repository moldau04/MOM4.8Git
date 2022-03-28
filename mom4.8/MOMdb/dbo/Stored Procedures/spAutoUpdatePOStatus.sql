CREATE PROCEDURE [dbo].[spAutoUpdatePOStatus]
	@POId int,
	@UpdatedBy varchar(100) 
	
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @RPOStatus int = -1;

	DECLARE @Line smallint
	DECLARE @Balance numeric(30,2)
	DECLARE @Selected numeric(30,2) = 0.0
	DECLARE @SelectedQuan numeric(30,2) = 0.0
	DECLARE @BalanceQuan numeric(30,2) = 0.0
	DECLARE @Amount numeric(30,2) = 0.0
	DECLARE @Quan numeric(30,2) = 0.0

	
	DECLARE @NewPOStatus int = 0;
	DECLARE @IsClosingPO int = 1;
	
	BEGIN TRY
	--BEGIN TRANSACTION
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
					if @BalanceQuan < 0
					BEGIN
						SET @NewPOStatus = 1; -- Closed -- As per Anita mam suggest on 04-May-2020 on Skype MOMDevTeam Group
					END
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
				SET @NewPOStatus = 5
				--UPDATE PO SET [Status]=5 WHERE PO=@POId AND [Status] <> 1
			END
			--ELSE
			--BEGIN
			--	UPDATE PO SET [Status]=@NewPOStatus WHERE PO=@POId AND [Status] <> 1
			--END
		END
		ELSE
		BEGIN
			SET @NewPOStatus = 0
			--UPDATE PO SET [Status]=0 WHERE PO=@POId AND [Status] <> 1
		END

		UPDATE PO SET [Status]=@NewPOStatus WHERE PO=@POId AND [Status] <> 1

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

		/** End log*/

		-- Commit transaction
		--COMMIT;
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