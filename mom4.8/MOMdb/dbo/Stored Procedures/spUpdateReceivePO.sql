CREATE PROCEDURE [dbo].[spUpdateReceivePO]
	@RID int,
	@Amount numeric(30,2),
	@POID int,
	@Batch int,
	@Due datetime,
	@UpdatedBy varchar(100)
AS
BEGIN
	DECLARE @CurrentDueDate varchar(50)
	SELECT @CurrentDueDate = CONVERT(varchar(50), Due , 101) FROM PO WHERE PO=@POID
	DECLARE @CurrentAmount numeric(30,2)
	SELECT @CurrentAmount = Amount FROM [ReceivePO] WHERE ID = @RID AND PO = @POID

	BEGIN TRY
	BEGIN TRANSACTION
		
		UPDATE [ReceivePO] SET Amount = @Amount, Batch = @Batch WHERE ID = @RID AND PO = @POID

		UPDATE PO SET Due=@Due WHERE PO=@POID

		/********Start Logs************/
		DECLARE @Val varchar(1000)
		IF(@Amount is not null)
		BEGIN 	
			SET @Val =(select Top 1 newVal  from log2 where screen='ReceivePO' and ref= @RID and Field='Amount' order by CreatedStamp desc )		
			IF(Convert(numeric(30,2), @Val) <> @Amount)
			BEGIN
				EXEC log2_insert @UpdatedBy,'ReceivePO',@RID,'Amount',@Val,@Amount
			END
			ELSE IF (@CurrentAmount <> @Amount)
			BEGIN
				EXEC log2_insert @UpdatedBy,'ReceivePO',@RID,'Amount',@CurrentAmount,@Amount
			END
		END

		IF(@Due is not null)
		BEGIN 	
			SET @Val =(select Top 1 newVal  from log2 where screen='PO' and ref= @RID and Field='Due Date' order by CreatedStamp desc )
			DECLARE @Duedate nvarchar(150)
			SELECT @Duedate = convert(varchar, @Due, 101)
			IF(@Val<>@Duedate)
			BEGIN
				EXEC log2_insert @UpdatedBy,'PO',@POID,'Due Date',@Val,@Duedate
				EXEC log2_insert @UpdatedBy,'ReceivePO',@RID,'Due Date',@Val,@Duedate
			END
			ELSE IF (@CurrentDueDate <> @Duedate)
			BEGIN
				EXEC log2_insert @UpdatedBy,'PO',@POID,'Due Date',@CurrentDueDate,@Duedate
				EXEC log2_insert @UpdatedBy,'ReceivePO',@RID,'Due Date',@CurrentDueDate,@Duedate
			END
		END

		EXEC [spAutoUpdatePOStatus] @POID, @UpdatedBy

	 /********End Logs************/

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