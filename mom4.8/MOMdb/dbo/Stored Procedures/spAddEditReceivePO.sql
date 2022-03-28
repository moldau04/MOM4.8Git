CREATE PROCEDURE [dbo].[spAddEditReceivePO]
	@ID int,
	@PO int,
	@Ref varchar(100),
	@WB varchar(50),
	@Comments varchar(5000),
	@Amount numeric(30,2),
	@fDate datetime,
	@Batch int,
	@Due datetime,
	@UpdatedBy varchar(100),
	@POReceiveBy int
AS
BEGIN
	SET NOCOUNT ON;
	Declare @CurrentDue varchar(150)
	Declare @CurrPOReceiveBy varchar(150)
	Select @CurrentDue = CONVERT(varchar(150), Due , 101) From PO Where PO =@PO
	Select @CurrPOReceiveBy = CASE WHEN ISNULL(POReceiveBy,9) = 9 THEN '' WHEN POReceiveBy = 0  THEN 'Amount' WHEN POReceiveBy = 1  THEN 'Qty' END From PO Where PO =@PO
BEGIN TRY
BEGIN TRANSACTION
		
	
	SET IDENTITY_INSERT [ReceivePO] ON 

	INSERT INTO [dbo].[ReceivePO]
           (ID, PO, Ref, WB, Comments, Amount, fDate,Batch)
     VALUES
           (@ID, @PO, @Ref, @WB, @Comments, @Amount, @fDate,@Batch)

	SET IDENTITY_INSERT [ReceivePO] OFF
	
	UPDATE PO SET Due=@Due,POReceiveBy= @POReceiveBy WHERE PO=@PO

	EXEC [spAutoUpdatePOStatus] @PO, @UpdatedBy

	/********Start Logs************/
	-- PO #
	if(@PO is not null And @PO != 0)
	Begin 	
		exec log2_insert @UpdatedBy,'ReceivePO',@ID,'PO #','',@PO
	END
	-- Ref
	if(@Ref is not null And @Ref != '')
	Begin
		exec log2_insert @UpdatedBy,'ReceivePO',@ID,'Ref','',@Ref
	END
	-- WB #
	if(@WB is not null And @WB != '')
	Begin
		exec log2_insert @UpdatedBy,'ReceivePO',@ID,'WB #','',@WB
	END
	-- Comments
	if(@Comments is not null And @Comments != '')
	Begin
		exec log2_insert @UpdatedBy,'ReceivePO',@ID,'Comments','',@Comments
	END
	-- Amount
	if(@Amount is not null)
	Begin
		exec log2_insert @UpdatedBy,'ReceivePO',@ID,'Amount','',@Amount
	END
	-- Date
	if(@fDate is not null And @fDate != '')
	Begin 	
		Declare @Calldate nvarchar(150)
		SELECT @Calldate = convert(varchar, @fDate, 101)
		exec log2_insert @UpdatedBy,'ReceivePO',@ID,'Date','',@Calldate
	END

	-- Status
	exec log2_insert @UpdatedBy,'ReceivePO',@ID,'Status','','Open'

	--if(@Batch is not null)
	--	Begin
	--		exec log2_insert @UpdatedBy,'ReceivePO',@ID,'Batch','',@Batch
	--	END

	-- 'Due Date
	Declare @Val varchar(1000)
	if(@Due is not null)
	begin 	
		Set @Val =(select Top 1 newVal  from log2 where screen='PO' and ref= @PO and Field='Due Date' order by CreatedStamp desc )
		Declare @Duedate nvarchar(150)
		SELECT @Duedate = convert(varchar, @Due, 101)
		if(@Val<>@Duedate)
		begin
			exec log2_insert @UpdatedBy,'PO',@PO,'Due Date',@Val,@Duedate
			exec log2_insert @UpdatedBy,'ReceivePO',@ID,'Due Date',@Val,@Duedate
		end
		Else IF (@CurrentDue <> @Duedate)
		Begin
			exec log2_insert @UpdatedBy,'PO',@PO,'Due Date',@CurrentDue,@Duedate
			exec log2_insert @UpdatedBy,'ReceivePO',@ID,'Due Date',@CurrentDue,@Duedate
		END
	end

	if(@POReceiveBy is not null)
	begin 	
		Declare @POReceiveBystr nvarchar(150)
		SET @POReceiveBystr = CASE WHEN ISNULL(@POReceiveBy,9) = 9 THEN '' WHEN @POReceiveBy = 0  THEN 'Amount' WHEN @POReceiveBy = 1  THEN 'Qty' END 
		if(@CurrPOReceiveBy<>@POReceiveBystr)
		begin
			exec log2_insert @UpdatedBy,'PO',@PO,'PO ReceiveBy',@CurrPOReceiveBy,@POReceiveBystr			
		end		
	end

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