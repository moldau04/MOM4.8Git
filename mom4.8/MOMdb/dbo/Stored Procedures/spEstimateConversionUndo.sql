CREATE PROCEDURE [dbo].[spEstimateConversionUndo]
	@estimateId int,
	@projectId int,
	@UpdatedBy varchar(100)
AS

DECLARE @IsOpenTran bit = 0
BEGIN TRY
	IF @@TRANCOUNT=0 
	BEGIN
		SET @IsOpenTran = 1
		BEGIN TRAN
	END
		--DECLARE @CurrBidDate DateTime 
		DECLARE @CurrStatus smallint
		DECLARE @CurrProject int
		DECLARE @CurrEstOpprt int

		SELECT @CurrProject = e.Job, @CurrStatus = e.Status, @CurrEstOpprt = e.Opportunity 
		FROM Estimate e WHERE e.ID = @estimateId

		DECLARE @isConverted bit = 0
		DECLARE @isFin bit = 0
		DECLARE @isConvertTracking bit = 0
		SELECT @isConverted = 1, @isFin = t.IsFinancialDataConverted from tblEstimateConvertToProject t where projectid = @projectId and EstimateID = @estimateId
		
		IF @isConverted = 1
		BEGIN
			IF @isFin = 1
			BEGIN
				--------- Insert into tblInventoryWHTrans-------------
				-- Inventory
				--INSERT INTO tblInventoryWHTrans(InvID
				--	, WarehouseID,LocationID ,Hand,Balance,fOrder
				--	, [Committed]
				--	, [Available],Screen
				--	, ScreenID
				--	, Mode,Date,TransType,Batch,FDate)
				--SELECT 
				--	b.MatItem,
				--	'OFC',0,0,0,0,
				--	(b.QtyRequired * (-1)),
				--	0,'Project',
				--	@projectId,
				--	'UndoConvert',GETDATE(),'Revert',0,GETDATE()
				--FROM JobTItem jt 
				--LEFT JOIN bom b on b.JobTItemID = jt.ID
				--INNER JOIN inv on inv.id = b.MatItem and inv.Type = 0
				--WHERE jt.Job = @projectId and jt.EstConvertId = @estimateId and jt.Type = 1

				-- Remove on update project
				INSERT INTO tblInventoryWHTrans(InvID
					, WarehouseID,LocationID ,Hand,Balance,fOrder
					, [Committed]
					, [Available],Screen
					, ScreenID
					, Mode,Date,TransType,Batch,FDate)
				SELECT 
					bj.MatItem,
					'OFC',0,0,0,0,
					--(b.QtyRequired * (-1)),
					CASE WHEN bj.QtyRequired >= be.QtyRequired THEN be.QtyRequired * (-1)
					ELSE bj.QtyRequired  * (-1) END,
					0,'Project',
					@projectId,
					'UndoConvert',GETDATE(),'Revert',0,GETDATE()
				FROM JobTItem jt 
				INNER JOIN bom bj on bj.JobTItemID = jt.ID and jt.Type = 1 and jt.Job = @projectId
				INNER JOIN EstimateI ei on ei.Estimate = jt.EstConvertId and jt.EstConvertId is not null
					AND	jt.EstConvertLine is not null and jt.EstConvertLine = ei.Line and ei.Estimate = @estimateid
				INNER JOIN bom be on be.EstimateIId = ei.id and ei.type = 1 and ei.Estimate = @estimateid
				INNER JOIN inv on inv.id = bj.MatItem and inv.Type = 0
				WHERE
					(bj.MatItem = be.MatItem and jt.fDesc = ei.fDesc) or
					(be.MatItem is null and bj.MatItem is null and jt.fDesc = ei.fDesc)

				
				--INSERT INTO tblInventoryWHTrans(InvID
				--	, WarehouseID,LocationID ,Hand,Balance,fOrder
				--	,[Committed]
				--	,[Available],Screen
				--	,ScreenID
				--	,Mode,Date,TransType,Batch,FDate)
				--SELECT 
				--	b.MatItem,
				--	'OFC',0,0,0,0,
				--	b.QtyRequired,
				--	0, 'Estimate',
				--	@estimateId,
				--	'UndoConvert', GETDATE(),'In',0,GETDATE()
				--FROM JobTItem jt 
				--LEFT JOIN bom b on b.JobTItemID = jt.ID
				--INNER JOIN inv on inv.id = b.MatItem and inv.Type = 0
				--WHERE jt.Job = @projectId and jt.EstConvertId = @estimateId and jt.Type = 1

				-- Adding new on reopen estimate
				INSERT INTO tblInventoryWHTrans(InvID
					, WarehouseID,LocationID ,Hand,Balance,fOrder
					,[Committed]
					,[Available],Screen
					,ScreenID
					,Mode,Date,TransType,Batch,FDate)
				SELECT 
					b.MatItem,
					'OFC',0,0,0,0,
					b.QtyRequired,
					0, 'Estimate',
					@estimateId,
					'UndoConvert', GETDATE(),'In',0,GETDATE()
				FROM EstimateI ei
				INNER JOIN bom b on b.EstimateIId = ei.ID and ei.Type = 1
				INNER JOIN inv on inv.id = b.MatItem and inv.Type = 0
				WHERE ei.Estimate = @estimateId and ei.Type = 1
				--------- End Insert into tblInventoryWHTrans----------

				SELECT Top 1 @isConvertTracking = 1 from JobTItem jt inner join Estimate e on e.ID = jt.EstConvertId 
				WHERE jt.JobT = e.Template and jt.Job = e.Job and e.ID = @estimateId

				IF @isConvertTracking = 1
				BEGIN
					--select 
					--	jt.ID ,
					--	jt.Job,
					--	jt.Budget,
					--	jt.Modifier,
					--	jt.ETC,
					--	jt.ETCMod,
					--	jt.BHours, --Hour
					--	jt.Line,
					--	bj.QtyRequired,
					--	bj.[BudgetUnit],
					--	bj.[BudgetExt],
					--	bj.[LabRate],
					--	bj.MatItem
					--FROM JobTItem jt 
					--INNER JOIN bom bj on bj.JobTItemID = jt.ID and jt.Type = 1 and jt.Job = @projectId
					--INNER JOIN EstimateI ei on ei.Estimate = jt.EstConvertId and jt.EstConvertId is not null
					--	AND	jt.EstConvertLine is not null and jt.EstConvertLine = ei.Line and ei.Estimate = @estimateid
					--INNER JOIN bom be on be.EstimateIId = ei.id and ei.type = 1 and ei.Estimate = @estimateid
					--WHERE
					--	(bj.MatItem = be.MatItem and jt.fDesc = ei.fDesc) or
					--	(be.MatItem is null and bj.MatItem is null and jt.fDesc = ei.fDesc)

					UPDATE bj set 
						bj.QtyRequired =	CASE WHEN bj.QtyRequired > be.QtyRequired THEN bj.QtyRequired - be.QtyRequired ELSE 0 END,
						bj.[BudgetUnit] =	CASE WHEN bj.[BudgetUnit] > be.[BudgetUnit] THEN bj.[BudgetUnit] - be.[BudgetUnit] ELSE 0 END,
						bj.[BudgetExt] =	CASE WHEN bj.[BudgetExt] > be.[BudgetExt] THEN bj.[BudgetExt] - be.[BudgetExt] ELSE 0 END,
						bj.[LabRate] =		CASE WHEN bj.[LabRate] > be.[LabRate] THEN bj.[LabRate] - be.[LabRate] ELSE 0 END--,
					FROM JobTItem jt 
					INNER JOIN bom bj on bj.JobTItemID = jt.ID and jt.Type = 1 and jt.Job = @projectId
					INNER JOIN EstimateI ei on ei.Estimate = jt.EstConvertId and jt.EstConvertId is not null
						AND	jt.EstConvertLine is not null and jt.EstConvertLine = ei.Line and ei.Estimate = @estimateid
					INNER JOIN bom be on be.EstimateIId = ei.id and ei.type = 1 and ei.Estimate = @estimateid
					WHERE
						(bj.MatItem = be.MatItem and jt.fDesc = ei.fDesc) or
						(be.MatItem is null and bj.MatItem is null and jt.fDesc = ei.fDesc)

					UPDATE jt set 
						jt.Budget =		CASE WHEN jt.Budget > ei.Cost THEN  jt.Budget - ei.Cost ELSE 0 END,
						jt.Modifier =	CASE WHEN jt.Modifier > ei.Labor THEN  jt.Modifier - ei.Labor ELSE 0 END,
						jt.ETC =		CASE WHEN jt.ETC > ei.MMod THEN  jt.ETC - ei.MMod ELSE 0 END,
						jt.ETCMod =		CASE WHEN jt.ETCMod > ei.LMod THEN  jt.ETCMod - ei.LMod ELSE 0 END,
						jt.BHours =		CASE WHEN jt.BHours > ei.Hours THEN  jt.BHours - ei.Hours  ELSE 0 END,
						jt.EstConvertId = null,
						jt.EstConvertLine = null
					FROM JobTItem jt 
					INNER JOIN bom bj on bj.JobTItemID = jt.ID and jt.Type = 1 and jt.Job = @projectId
					INNER JOIN EstimateI ei on ei.Estimate = jt.EstConvertId and jt.EstConvertId is not null
						AND	jt.EstConvertLine is not null and jt.EstConvertLine = ei.Line and ei.Estimate = @estimateid
					INNER JOIN bom be on be.EstimateIId = ei.id and ei.type = 1 and ei.Estimate = @estimateid
					WHERE
						(bj.MatItem = be.MatItem and jt.fDesc = ei.fDesc) or
						(be.MatItem is null and bj.MatItem is null and jt.fDesc = ei.fDesc)

					/* Old
					--select 
					--	jt.Budget,
					--	m.Amount,
					--	m.Quantity,
					--	m.Price
					--from JobTItem jt 
					--left join Milestone m on m.JobTItemID = jt.ID
					--where jt.Job = @projectId and jt.EstConvertId = @estimateId and jt.Type = 0

					--UPDATE jt set
					--	jt.Budget = 0
					--FROM JobTItem jt 
					--LEFT JOIN Milestone m on m.JobTItemID = jt.ID
					--WHERE jt.Job = @projectId and jt.EstConvertId = @estimateId and jt.Type = 0

					--UPDATE m set
					--	m.Amount = 0,
					--	m.Quantity = 0,
					--	m.Price = 0
					--FROM JobTItem jt 
					--LEFT JOIN Milestone m on m.JobTItemID = jt.ID
					--WHERE jt.Job = @projectId and jt.EstConvertId = @estimateId and jt.Type = 0

					*/

					IF EXISTS (SELECT Top 1 1 FROM JobTItem jt 
								WHERE 
									jt.Job = @projectId
									AND jt.Type = 0
									AND jt.EstConvertId is not null
									AND jt.EstConvertId = @estimateId
									AND jt.EstConvertLine is not null)
					BEGIN
						--select 
						--	jt.Budget,
						--	mj.Amount,
						--	mj.Quantity,
						--	mj.Price,

						--	ei.Amount,
						--	me.Amount,
						--	me.Quantity,
						--	me.Price

						--CASE WHEN jt.Budget > ei.Amount then jt.Budget - ei.Amount ELSE 0 END
						--case when mj.Amount > me.Amount then mj.Amount - me.Amount else 0 end,
						--1,
						--case when mj.Amount > me.Amount then mj.Amount - me.Amount else 0 end
						--from JobTItem jt 
						--inner join Milestone mj on mj.JobTItemID = jt.ID and jt.Type = 0 and jt.Job = @projectId
						--inner join EstimateI ei on ei.Estimate = jt.EstConvertId  and ei.Type = 0
						--	and jt.EstConvertId is not null and jt.EstConvertLine is not null 
						--	and jt.EstConvertLine = ei.Line and ei.Estimate = @estimateid
						--inner join Milestone me on me.EstimateIId = ei.ID

						UPDATE mj SET 
							mj.Amount = case when mj.Amount > me.Amount then mj.Amount - me.Amount else 0 end,
							mj.Quantity = 1,
							mj.Price = case when mj.Amount > me.Amount then mj.Amount - me.Amount else 0 end
						FROM JobTItem jt 
						INNER JOIN Milestone mj on mj.JobTItemID = jt.ID and jt.Type = 0 and jt.Job = @projectId
						INNER JOIN EstimateI ei on ei.Estimate = jt.EstConvertId  and ei.Type = 0
							AND	jt.EstConvertId is not null and jt.EstConvertLine is not null 
							AND	jt.EstConvertLine = ei.Line and ei.Estimate = @estimateid
						INNER JOIN Milestone me on me.EstimateIId = ei.ID

						UPDATE jt SET 
							jt.Budget = CASE WHEN jt.Budget > ei.Amount then jt.Budget - ei.Amount ELSE 0 END,
							jt.EstConvertId = null,
							jt.EstConvertLine = null
						FROM JobTItem jt 
						INNER JOIN Milestone mj on mj.JobTItemID = jt.ID and jt.Type = 0 and jt.Job = @projectId
						INNER JOIN EstimateI ei on ei.Estimate = jt.EstConvertId  and ei.Type = 0
							AND jt.EstConvertId is not null and jt.EstConvertLine is not null 
							AND	jt.EstConvertLine = ei.Line and ei.Estimate = @estimateid
						INNER JOIN Milestone me on me.EstimateIId = ei.ID
					END
					ELSE
					BEGIN
						DECLARE @Count int = 0
						SELECT	@Count = Count(*) from JobTItem jt 
						WHERE	jt.Job = @projectId
							AND jt.Type = 0
							AND jt.EstConvertId is not null
							AND jt.EstConvertId = @estimateId


						IF @Count = 1
						BEGIN
							--SELECT 
							--	jt.Budget,
							--	mj.Amount,
							--	mj.Quantity,
							--	mj.Price,

							--	esum.Budget,
							--	esum.Amount,

							--	case when jt.Budget > esum.Budget then jt.Budget - esum.Budget else 0 end,
							--	case when mj.Amount > esum.Amount then mj.Amount - esum.Amount else 0 end,
							--	1,
							--	case when mj.Amount > esum.Amount then mj.Amount - esum.Amount else 0 end
							--from		JobTItem jt 
							--inner join	Milestone mj on mj.JobTItemID = jt.ID and jt.Type = 0 and jt.Job = @projectId
							--inner join (select 
							--				ei.Estimate,
							--				sum(ei.Amount) Budget,
							--				sum(me.Amount) Amount
							--			from EstimateI ei 
							--			inner join Milestone me on me.EstimateIId = ei.ID 
							--				and ei.Estimate = @estimateId 
							--				and ei.Type = 0
							--			GROUP by ei.Estimate
							--			) esum ON esum.Estimate = jt.EstConvertId

							UPDATE mj SET 
								mj.Amount =	case when mj.Amount > esum.Amount then mj.Amount - esum.Amount else 0 end,
								mj.Quantity = 1,
								mj.Price = case when mj.Amount > esum.Amount then mj.Amount - esum.Amount else 0 end
							FROM		JobTItem jt 
							INNER JOIN	Milestone mj on mj.JobTItemID = jt.ID and jt.Type = 0 and jt.Job = @projectId
							INNER JOIN (select 
											ei.Estimate,
											sum(ei.Amount) Budget,
											sum(me.Amount) Amount
										from EstimateI ei 
										inner join Milestone me on me.EstimateIId = ei.ID 
											and ei.Estimate = @estimateId 
											and ei.Type = 0
										GROUP by ei.Estimate
										) esum ON esum.Estimate = jt.EstConvertId

							UPDATE jt SET 
								jt.Budget = case when jt.Budget > esum.Budget then jt.Budget - esum.Budget else 0 end,
								jt.EstConvertId = null,
								jt.EstConvertLine = null
							FROM		JobTItem jt 
							INNER JOIN	Milestone mj on mj.JobTItemID = jt.ID and jt.Type = 0 and jt.Job = @projectId
							INNER JOIN (select 
											ei.Estimate,
											sum(ei.Amount) Budget,
											sum(me.Amount) Amount
										from EstimateI ei 
										inner join Milestone me on me.EstimateIId = ei.ID 
											and ei.Estimate = @estimateId 
											and ei.Type = 0
										GROUP by ei.Estimate
										) esum ON esum.Estimate = jt.EstConvertId

							
						END
						ELSE IF @Count > 1
						BEGIN
							RAISERROR ('There is an error on undo revenue amount',16,1)	
						END
					END
				END
			END

			-- TODO: remove project, estimate out of the convert table
			--select * from tblEstimateConvertToProject where projectid = @projectId and EstimateID = @estimateId
			DELETE tblEstimateConvertToProject where projectid = @projectId and EstimateID = @estimateId

		END

		-- case 1: linked only
		--select * from job where id = @projectId and FirstLinkedEst = @estimateId
		--select * from estimate where ID =@estimateId and Job = @projectid
		-- check if default estimate of project then reset it
		UPDATE job set FirstLinkedEst = (SELECT TOP 1 ID FROM Estimate WHERE Job = @projectId and ID != @estimateId Order By ID)
		WHERE id = @projectId and FirstLinkedEst = @estimateId
		-- remove project no from estimate and update estimate status to open
		UPDATE estimate set job = null, Status = 1 where ID =@estimateId and Job = @projectid

		--DECLARE @CurrEstOpprtStatus smallint
		DECLARE @CurrEstOpprtStatus varchar(50) = ''
		
		SELECT @CurrEstOpprtStatus = oe.Name
		FROM Lead ld INNER JOIN OEStatus oe ON oe.ID = ld.Status WHERE ld.ID = @CurrEstOpprt

		-- Check and update Opportunity after updating Estimate Status
		IF EXISTS (
			SELECT TOP 1 1
			from lead ld
			inner join Estimate e on e.Opportunity = ld.ID 
			where e.Status = 5 
			and ld.id = @CurrEstOpprt
			and e.Opportunity is not null
		) -- There is a Sold estimate link to this opportunity: Set Opprt status to 'Sold'
		BEGIN
			UPDATE LEAD SET Status = 5 WHERE ID = @CurrEstOpprt
		END
		ELSE -- There is no Sold estimate link to this opportunity but have an open estimate link to this opportunity. Set Opprt status to 'Quoted'
		BEGIN
			UPDATE LEAD SET Status = 7 WHERE ID = @CurrEstOpprt
		END

		DECLARE @EstOpprtStatus  varchar(50) = ''
		SELECT @EstOpprtStatus = oe.Name
		FROM Lead ld INNER JOIN OEStatus oe ON oe.ID = ld.Status WHERE ld.ID = @CurrEstOpprt

		/*-- Logs --*/
		-- Estimate Status
		IF(@CurrStatus != 1)
		BEGIN
			DECLARE @CurrStatusName varchar(50), @OpenStatusName varchar(50)
			SELECT TOP 1 @CurrStatusName=Name FROM OEStatus WHERE ID = @CurrStatus
			SELECT TOP 1 @OpenStatusName=Name FROM OEStatus WHERE ID = 1
			EXEC log2_insert @UpdatedBy,'Estimate',@estimateId,'Estimate Status',@CurrStatusName,@OpenStatusName
		END

		EXEC log2_insert @UpdatedBy,'Estimate',@estimateId,'Project',@CurrProject,''

		IF(@EstOpprtStatus != @CurrEstOpprtStatus)
			EXEC log2_insert @UpdatedBy,'Opportunity',@CurrEstOpprt,'Status',@CurrEstOpprtStatus,@EstOpprtStatus
		/*-- End Logs --*/
	IF @@TRANCOUNT>0 AND @IsOpenTran = 1
	BEGIN
		COMMIT
	END
END TRY
BEGIN CATCH
	DECLARE @ErrorMessage NVARCHAR(4000);  
	DECLARE @ErrorSeverity INT;  
	DECLARE @ErrorState INT;  
  
	SELECT   
		@ErrorMessage = ERROR_MESSAGE(),  
		@ErrorSeverity = ERROR_SEVERITY(),  
		@ErrorState = ERROR_STATE();  
  
	IF @@TRANCOUNT>0 AND @IsOpenTran = 1 ROLLBACK	
	RAISERROR (@ErrorMessage, -- Message text.  
				@ErrorSeverity, -- Severity.  
				@ErrorState -- State.  
				);  

END CATCH