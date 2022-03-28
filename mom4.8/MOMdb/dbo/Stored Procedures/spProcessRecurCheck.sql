CREATE PROCEDURE [dbo].[spProcessRecurCheck]
	@refId int,
	@fUser VARCHAR(50)
AS
BEGIN
	
	SET NOCOUNT ON;

	BEGIN TRY
BEGIN TRANSACTION

	DECLARE @PJID INT
	DECLARE @cRef varchar(max)
	DECLARE @fDate Datetime
	DECLARE @fDesc VARCHAR(250)
	DECLARE @Bank INT
	DECLARE @Vendor INT
	DECLARE @Memo VARCHAR(75)
	DECLARE @NextC BIGINT
	DECLARE @DiscGL INT = NULL
	DECLARE @Type INT
	DECLARE @Frequency INT
	DECLARE @Date Datetime
	DECLARE @CountRecur INT
	DECLARE @GeneratedPJID int
	DECLARE @BillItem AS tblTypeBill

	

	SELECT @PJID = PJID, @Vendor = Vendor,@fDesc = fDesc, @Bank = Bank,@Memo = Memo,@Type = Type,@Frequency = Frequency ,@Date = fDate FROM CDRecurr WHERE ID = @refId

	If @Type = 0
	BEGIN
		SELECT @NextC =  NextC FROM Bank WHERE ID = @Bank
	END
	
	ELSE if(@Type=1)
	begin
	SELECT @NextC =  NextCash FROM Bank WHERE ID = @Bank
	end
	ELSE if(@Type=2)
	begin
	SELECT @NextC =  NextWire FROM Bank WHERE ID = @Bank
	end
	ELSE if(@Type=3)
	begin
	SELECT @NextC =  NextACH FROM Bank WHERE ID = @Bank
	end
	ELSE if(@Type=4)
	begin
	SELECT @NextC =  NextCC FROM Bank WHERE ID = @Bank
	end


	SELECT @cRef = Ref FROM PJRecurr WHERE ID = @PJID

	EXEC spProcessRecurBill @PJID
	SELECT @GeneratedPJID = MAX(ID) FROM PJ 
	

	SELECT @fDate = fDate FROM PJ WHERE Ref = @cRef AND Vendor = @Vendor
	INSERT INTO @BillItem SELECT p.fDate,p.ID,p.Ref,p.TRID,p.fDesc,p.Spec,o.Original,o.Balance,o.Disc,o.Balance 
	FROM PJ p INNER JOIN OpenAp o 
	ON p.ID = o.PJID WHERE p.Ref = @cRef AND p.Vendor = @Vendor AND p.fDate = @Date 
	AND p.ID = @GeneratedPJID

	EXEC spAddCheck @BillItem ,	@Date ,@fDesc,	@Bank ,	@Vendor,@Memo ,	@NextC ,@DiscGL ,@Type ,@fUser 



	
	



	COMMIT 
	
	END TRY

	BEGIN CATCH

	SELECT ERROR_MESSAGE()
	DECLARE @error varchar(1000)=(SELECT ERROR_MESSAGE())
    IF @@TRANCOUNT>0
        ROLLBACK	
		RAISERROR ( @error,16,1)
        RETURN 
	END CATCH 
	
	SELECT @fDate = case @Frequency
							 when 0 then dateadd(mm, 1, @Date)
							 when 1 then dateadd(mm, 2, @Date)
							 when 2 then dateadd(mm, 3, @Date)
							 when 3 then dateadd(mm, 4, @Date)
							 when 4 then dateadd(mm, 6, @Date)
							 when 5 then dateadd(yy, 1, @Date)
							 when 6 then dateadd(dd, 7, @Date)
							 end
UPDATE CDRecurr
	   SET
			fDate = @fDate,
			IsRecon = ISNULL(IsRecon,0)+1
	   WHERE ID = @refId


	SELECT @CountRecur = Count(*) FROM CDRecurr Where fDate <=Getdate()

	return @CountRecur

END
