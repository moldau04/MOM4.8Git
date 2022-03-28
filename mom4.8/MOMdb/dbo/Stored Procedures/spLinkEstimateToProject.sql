CREATE Procedure spLinkEstimateToProject
	@EstimateId int,
	@Job int,
	@UpdatedBy VARCHAR(200)
AS
BEGIN
	DECLARE @Val VARCHAR(1000)  

	DECLARE @BDate VARCHAR(200)
	DECLARE @newBDate VARCHAR(200)
	DECLARE @Status INT
	DECLARE @oldProject VARCHAR(200)
	DECLARE @BidCloseDate datetime = GETDATE();
	DECLARE @CurrEstOpprt int

	SELECT @Status=status
		, @BDate=convert(varchar, BDate, 101)
		, @oldProject=ISNULL(CONVERT(VARCHAR,job),'') 
		, @CurrEstOpprt = Opportunity
	FROM Estimate 
	WHERE ID=@EstimateId

	DECLARE @CurrOppStatus smallint
	DECLARE @CurrOppStatusName VARCHAR(100) = ''
	SELECT @CurrOppStatus = l.Status, @CurrOppStatusName = os.Name from OEStatus os INNER JOIN LEAD l ON l.ID = @CurrEstOpprt AND os.ID = l.Status

	IF @oldProject is null OR @oldProject = '' OR @oldProject = '0'
	BEGIN
		
		UPDATE JOb 
		SET FirstLinkedEst=@EstimateId
		WHERE ISNULL(FirstLinkedEst,0)=0 AND ID=@Job

		UPDATE Estimate
		SET Job=@Job
			,Status=5
			,BDate=@BidCloseDate
		WHERE ID=@EstimateId

		-- Update Opportunity Status
		UPDATE Lead SET Status=5 WHERE ID = @CurrEstOpprt

		/* --- Logs --- */
		-- Estimate Status
		DECLARE @StatusVal varchar(50)
		DECLARE @OldStatusVal varchar(50)
		SELECT TOP 1 @StatusVal=Name FROM OEStatus WHERE ID = 5
		SELECT TOP 1 @OldStatusVal=Name FROM OEStatus WHERE ID = @Status
	
		IF (@OldStatusVal <> @StatusVal)
		BEGIN
			EXEC log2_insert @UpdatedBy,'Estimate',@EstimateId,'Estimate Status',@OldStatusVal,@StatusVal
		END
		SET @newBDate =convert(varchar, @BidCloseDate, 101)
		IF (@BDate <> @newBDate)
		BEGIN
			EXEC log2_insert @UpdatedBy,'Estimate',@EstimateId,'Estimate Bid Close date',@BDate,@newBDate
		END

		DECLARE @newJob varchar(200)
		SET @newJob=CONVERT(VARCHAR,@Job)
		IF (@oldProject <> @newJob)
		BEGIN
			EXEC log2_insert @UpdatedBy,'Estimate',@EstimateId,'Estimate Project',@oldProject,@newJob
		END

		-- Need to check because for old estimate have no opportunity
		IF EXISTS (SELECT 1 FROM Lead WHERE ID = @CurrEstOpprt)
		BEGIN
			IF (@CurrOppStatus is not null AND @CurrOppStatus != 5)
			BEGIN
				EXEC log2_insert @UpdatedBy,'Opportunity',@CurrEstOpprt,'Status',@CurrOppStatusName,@StatusVal
			END
		END
		/* --- End Logs ---*/
		SELECT e.Job, e.BDate, e.Status FROM Estimate e WHERE e.id = @EstimateId
	END		
	ELSE
	BEGIN
		RAISERROR ('Estimate was linked to a project already',16,1)	
	END
END
