CREATE PROCEDURE [dbo].[spUpdateReceivePODue]
	@ID int,
	@PO int,
	@Due datetime,
	@UpdatedBy varchar(100)
AS
BEGIN
	SET NOCOUNT ON;
	Declare @CurrentDue varchar(150)
	Select @CurrentDue = CONVERT(varchar(150), Due , 101) From PO Where PO =@PO
BEGIN TRY
BEGIN TRANSACTION
	
	UPDATE PO SET Due=@Due WHERE PO=@PO

/********Start Logs************/
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