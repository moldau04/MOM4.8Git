CREATE PROCEDURE [dbo].[spUpdateLeadRevenueByEstimate] 
	  @ID INT,
	  @Amount NUMERIC (30, 2),
	  @UpdateUser varchar(50),
	  @EstimateID INT,
	  @OpportunityStageID INT
AS
BEGIN
	DECLARE @currOpprId int
	-- Get current opportunity of an estimate
	SELECT @currOpprId = Opportunity FROM Estimate Where ID = @EstimateID

	DECLARE @esQuoted NUMERIC (30, 2);
	-- Get old Quoted value of Estimate by ID
	SELECT @esQuoted = CASE When ISNULL(Quoted, 0) = 0 Then ISNULL(Price, 0)
							ELSE Quoted END
	FROM Estimate WHERE ID = @EstimateID

	IF(@currOpprId is not null AND @currOpprId != @ID)
	BEGIN
		-- Remove amount from previous estimate
		UPDATE Lead
		SET 
			Revenue = Case WHEN Revenue - ISNULL(@esQuoted, 0) >= 0 THEN Revenue - ISNULL(@esQuoted, 0)
						ELSE 0 END--Replace the old value by a new value
		WHERE ID=@currOpprId

		UPDATE Lead
		SET 
			Revenue = Revenue + @Amount,--Replace the old value by a new value
			OpportunityStageID = @OpportunityStageID,
			CreatedBy=@UpdateUser,
			LastUpdatedBy=@UpdateUser
		WHERE ID=@ID
	END
	ELSE
	BEGIN
		UPDATE Lead
		SET 
			Revenue = Case WHEN Revenue + @Amount - ISNULL(@esQuoted, 0) >= 0 THEN Revenue + @Amount - ISNULL(@esQuoted, 0)
						ELSE 0 END,--Replace the old value by a new value
			OpportunityStageID = @OpportunityStageID,
			CreatedBy=@UpdateUser,
			LastUpdatedBy=@UpdateUser
		WHERE ID=@ID
	END

	--DECLARE @esCount int;
	---- Get count of estimate point to opportunity
	--SELECT @esCount = Count(*) from Estimate where Opportunity = @ID

	--IF(@esCount = 1)
	--BEGIN
	--	UPDATE Lead
	--	SET 
	--		Revenue= @Amount,--Replace the old value by a new value
	--		OpportunityStageID = @OpportunityStageID,
	--		CreatedBy=@UpdateUser,
	--		LastUpdatedBy=@UpdateUser
	--	WHERE ID=@ID
	--END
	--ELSE
	--BEGIN
	--	UPDATE Lead
	--	SET 
	--		Revenue = Case WHEN Revenue + @Amount - ISNULL(@esQuoted, 0) >= 0 THEN Revenue + @Amount - ISNULL(@esQuoted, 0)
	--					ELSE 0 END,--Replace the old value by a new value
	--		OpportunityStageID = @OpportunityStageID,
	--		CreatedBy=@UpdateUser,
	--		LastUpdatedBy=@UpdateUser
	--	WHERE ID=@ID
	--END
END
