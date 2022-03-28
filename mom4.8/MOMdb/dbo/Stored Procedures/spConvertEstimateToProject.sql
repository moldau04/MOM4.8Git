CREATE PROCEDURE [dbo].[spConvertEstimateToProject] @estimate int,
		@UpdatedBy varchar(100)
AS

DECLARE @Job int, 
		@loc int, 
		@owner int, 
		@Remarks varchar(max), 
		@fdesc varchar(75),
		@project int,
		@template int,
		@type int,
		@GroupID int,
		@OpportunityID int,
		@IsFinancialDataConvert int =1,
		@IsSglBilAmt bit,
		@EstDesc varchar(150)
		--@contract int = 0

				  
BEGIN TRY
	BEGIN TRANSACTION

	DECLARE @CurrBidDate DateTime 
	DECLARE @CurrStatus smallint
	DECLARE @CurrProject int

	DECLARE @CurrOppStatus smallint
	DECLARE @CurrOppStatusName VARCHAR(100)
	SELECT @CurrOppStatus = l.Status, @CurrOppStatusName = os.Name from OEStatus os INNER JOIN LEAD l ON l.ID = @OpportunityID AND os.ID = l.Status
		  
	SELECT @project=e.Job, @OpportunityID=isnull(e.Opportunity,0)
		--, @loc= e.LocID
		, @loc = (CASE r.Type WHEN 4 THEN (SELECT Top 1 Loc.Loc from loc where loc.Rol = e.RolID) 
			ELSE 0 END
		 )
		, @IsSglBilAmt = ISNULL(IsSglBilAmt,0), @EstDesc = e.fDesc 
		--For logs
		, @CurrBidDate = e.BDate
		, @CurrStatus = e.Status
		, @CurrProject = e.Job
	FROM Estimate e INNER JOIN rol r ON r.ID = e.RolID
	WHERE e.ID= @estimate

	IF @project is not null AND @project != 0
	BEGIN 	
		RAISERROR ('Estimate already converted to project!',16,1)	
	END

	IF @CurrStatus is null OR @CurrStatus != 1
	BEGIN
		RAISERROR ('Only open estimate can be converted to project. Please check again!',16,1)
	END

	IF( @loc <> 0 and @OpportunityID  <> 0 )
	BEGIN
		IF @project is null OR @project = 0
		BEGIN

			DECLARE @JfDesc VARCHAR(255), 
					@Line INT,
					@JCode VARCHAR(10),
					@QtyReq NUMERIC(30,2),
					@BudgetExt NUMERIC(30,2),
					@BudgetUnit NUMERIC(30,2),
					@LabHours NUMERIC(30,2),
					@LabRate NUMERIC(30,2),
					@LabExt NUMERIC(30,2),
					@MatMod NUMERIC(30,2),
					@LabMod NUMERIC(30,2),
					@LabItem INT,
					@MatItem INT,
					@BType INT,
					@SDate DATETIME,
					@Currency NCHAR(10),
					@Vendor VARCHAR(MAX),
					@JobTItemID INT,
					@UM VARCHAR(50)

			DECLARE @MilestoneName VARCHAR(150),
					@BRev NUMERIC(30,2),
					@CreationDt DATETIME,
					@RequiredByDt DATETIME,
					@MType INT,
					@Department VARCHAR(150),
					@MQuantity NUMERIC(30,2),
					@MPrice NUMERIC(30,2),
					@MChangeOrder Tinyint

			DECLARE @BillRate NUMERIC(30,2),
					@RateOT NUMERIC(30,2),
					@RateTravel NUMERIC(30,2),
					@RateNT NUMERIC(30,2),
					@RateDT NUMERIC(30,2),
					@RateMileage NUMERIC(30,2),
					@JobAmount NUMERIC(30,2),
					@PType INT

			SELECT @owner=(select owner from loc where loc=@loc), 
				@Remarks= e.Remarks, @fdesc=e.fDesc, 
				@template = e.Template   , 
				@GroupID=isnull(e.GroupID,0),
				@BillRate=isnull(e.BillRate, 0),
				@RateOT=e.OT,
				@RateTravel=e.RateTravel,
				@RateNT=e.RateNT,
				@RateDT=e.DT,
				@RateMileage=e.RateMileage,
				@JobAmount=isnull(e.Amount, 0),
				@PType=e.PType,
				@OpportunityID=isnull(e.Opportunity,0)
			FROM Estimate e 
			WHERE e.ID= @estimate

			SELECT @type=Type FROM JobT Where ID= @template
		 
			IF NOT EXISTS(SELECT 1 FROM tblEstimateConvertToProject Where OpportunityID=@OpportunityID and isnull(ProjectID,0) > 0 )
			BEGIN
				-----------
				----------- New Feature
				-----------
				DECLARE @ctype varchar(15)
				DECLARE @Wage int
				DECLARE @InvExp int
				DECLARE @InvServ int
				DECLARE @Post smallint
				DECLARE @Charge smallint
				DECLARE @fInt smallint

				SELECT @ctype=j.CType,
					@Wage= j.Wage, 
					@InvExp=j.InvExp,
					@InvServ= j.InvServ ,
					@Post= j.Post,
					@Charge=j.Charge, 
					@fInt= j.fInt
				FROM JobT j LEFT JOIN   
				PRWage p on j.Wage = p.ID LEFT JOIN     
				Inv i on j.InvServ = i.ID LEFT JOIN     
				Chart c on j.InvExp = c.ID              
				WHERE j.id=@template          
       
				INSERT INTO job	(Loc, Template , Owner , fDate
					, Status
					, Remarks, fDesc 
					,Rev ,Mat ,Labor,Cost,Profit,Ratio,Reg ,OT  ,DT  ,TT  ,Hour,BRev,BMat,BLabor,BCost,BProfit,BRatio,BHour,Comm,CreditCard
					,BillRate ,NT  ,Amount    ,Type ,CType ,WageC,GL     ,GLRev   ,Post ,Charge ,fInt ,RateOT ,RateTravel ,RateNT ,RateDT ,RateMileage ,PType
					,FirstLinkedEst)
				values(			@loc, @template, @owner, GETDATE()
					, 0
					, @Remarks,@fdesc
					,0.00,0.00,0.00 ,0.00,0.00  ,0.00 ,0.00,0.00,0.00,0.00,0.00,0.00,0.00,0.00  ,0.00 ,0.00   ,0.00  ,0.00 ,0.00,0
					,@BillRate,0.00,@JobAmount,@type,@ctype,@Wage,@InvExp,@InvServ,@Post,@Charge,@fInt,@RateOT,@RateTravel,@RateNT,@RateDT,@RateMileage,@PType
					,@estimate)

				SET @Job=SCOPE_IDENTITY();

				------------------
				------------------
				------------------
			END
			ELSE
			BEGIN
				IF NOT EXISTS(SELECT 1 FROM tblEstimateConvertToProject Where OpportunityID=@OpportunityID and isnull(ProjectID,0) > 0 and EstimateID = @estimate)
				BEGIN
					SELECT @Job=ProjectID FROM tblEstimateConvertToProject Where OpportunityID=@OpportunityID

					DECLARE @OldBillRate numeric(30,2)
					DECLARE @OldRateTravel numeric(30,2)
					DECLARE @OldRateMileage numeric(30,2)
					DECLARE @OldRateDT numeric(30,2)
					DECLARE @OldRateNT numeric(30,2)
					DECLARE @OldRateOT numeric(30,2)

					SELECT  
						@OldBillRate = BillRate,
						@OldRateTravel = RateTravel,
						@OldRateMileage = RateMileage,
						@OldRateDT = RateDT,
						@OldRateNT = RateNT,
						@OldRateOT = RateOT
					FROM job WHERE id=@Job

					DECLARE @NewBillRate numeric(30,2)
					DECLARE @NewRateTravel numeric(30,2)
					DECLARE @NewRateMileage numeric(30,2)
					DECLARE @NewRateDT numeric(30,2)
					DECLARE @NewRateNT numeric(30,2)
					DECLARE @NewRateOT numeric(30,2)

					UPDATE job 
						SET BillRate=   case BillRate    when 0 then ( case @BillRate    when 0 then BillRate    else @BillRate    end  )  else BillRate    end
						,   RateTravel= case RateTravel  when 0 then ( case @RateTravel  when 0 then RateTravel  else @RateTravel  end  )  else RateTravel  end
						,   RateMileage=case RateMileage when 0 then ( case @RateMileage when 0 then RateMileage else @RateMileage end  )  else RateMileage end
						,   RateDT=     case RateDT      when 0 then ( case @RateDT      when 0 then RateDT      else @RateDT      end  )  else RateDT      end
						,   RateNT=     case RateNT      when 0 then ( case @RateNT      when 0 then RateNT      else @RateNT      end  )  else RateNT      end
						,   RateOT=     case RateOT      when 0 then ( case @RateOT      when 0 then RateOT      else @RateOT      end  )  else RateOT      end		 
					WHERE id=@Job

					SELECT  
						@NewBillRate = BillRate,
						@NewRateTravel = RateTravel,
						@NewRateMileage = RateMileage,
						@NewRateDT = RateDT,
						@NewRateNT = RateNT,
						@NewRateOT = RateOT
					FROM job WHERE id=@Job

					DECLARE @ProjScreen varchar(50)

					IF(@OldBillRate != @NewBillRate)  EXEC log2_insert @UpdatedBy,@ProjScreen,@Job,'Bill Rate',@OldBillRate,@NewBillRate  
					
					IF(@OldRateTravel != @NewRateTravel)  EXEC log2_insert @UpdatedBy,@ProjScreen,@Job,'Travel Rate',@OldRateTravel,@NewRateTravel  
					
					IF(@OldRateMileage != @NewRateMileage)  EXEC log2_insert @UpdatedBy,@ProjScreen,@Job,'Mileage',@OldRateMileage,@NewRateMileage  
					
					IF(@OldRateOT != @NewRateOT)  EXEC log2_insert @UpdatedBy,@ProjScreen,@Job,'OT Rate',@OldRateOT,@NewRateOT  
							    
					IF(@OldRateNT != @NewRateNT)  EXEC log2_insert @UpdatedBy,@ProjScreen,@Job,'NT Rate',@OldRateNT,@NewRateNT  
							   
					IF(@OldRateDT != @NewRateDT)  EXEC log2_insert @UpdatedBy,@ProjScreen,@Job,'DT Rate',@OldRateDT,@NewRateDT  
				END
				ELSE
				BEGIN
					RAISERROR ('Estimate already converted to project!',16,1)
				END
			END

			-- Adding Group from Estimate to project
			IF NOT EXISTS (SELECT 1 FROM tblProjectGroup WHERE ProjectId = @Job AND GroupId = @GroupID)
			BEGIN 
				INSERT INTO tblProjectGroup VALUES (@Job,@GroupID)
			END
	    
			INSERT INTO	tblEstimateConvertToProject([ProjectID] ,[EstimateID],[OpportunityID],[IsFinancialDataConverted])
				VALUES(@Job,@estimate,@OpportunityID,@IsFinancialDataConvert)

			UPDATE Estimate SET Job=@Job, Status=5,LocID = @loc,fFor='ACCOUNT' WHERE ID=@estimate

			UPDATE Lead SET Status=5 WHERE ID =@OpportunityID

			EXEC spConvertEstimateDocToProjectDoc @estimate,@job

			IF(@IsFinancialDataConvert=1)
			---------
			--------- New  Feature "Users can convert Financial Data later"
			---------
			BEGIN
			-------------- BEGIN INSERT PROJECT BOM ITEMS --------------
				DECLARE @EstLine int
				DECLARE db_cursor_mat CURSOR FOR 

				SELECT  Estimate, Line, fDesc, Quan, Cost AS BudgetExt,	Price AS BudgetUnit, Hours AS LabHours, Rate AS LabRate, 
						Labor AS LabExt, Code,  MMod, LMod,	LabItem, MatItem, SDate, EstimateI.Vendor,EstimateI.Currency, BOM.Type 
				FROM	EstimateI 
				LEFT JOIN BOM ON EstimateI.ID = BOM.EstimateIId
				WHERE EstimateI.Estimate = @estimate AND EstimateI.Type = 1

				OPEN db_cursor_mat  
				FETCH NEXT FROM db_cursor_mat INTO
					 @estimate, @EstLine, @JfDesc, @QtyReq,  @BudgetExt, @BudgetUnit, @LabHours, @LabRate,
					 @LabExt, @JCode, @MatMod, @LabMod,  @LabItem, @MatItem, @SDate, @Vendor,
					 @Currency, @BType
					
				WHILE @@FETCH_STATUS = 0
				BEGIN  		

					SELECT @Line=isnull(max(line),0) + 1 from JobTItem where job=@Job and Type=1

					INSERT INTO JobTItem (JobT,Job,Type,fDesc,Code,Actual,Budget,Line,[Percent],Comm
						,Modifier,ETC,ETCMod,Labor,Stored,BHours,OrderNo,GroupID
						, EstConvertId, EstConvertLine)
						values(@template, @Job, 1, @JfDesc, @JCode, 0, @BudgetExt, @Line,0, 0
						, @MatMod, @LabExt, @LabMod, 0, 0,@LabHours,@Line,@GroupID
						, @estimate, @EstLine)
				 
					SET @JobTItemID = SCOPE_IDENTITY()
			
					INSERT INTO [dbo].[BOM] ([JobTItemID],[Type],[QtyRequired],[UM],[BudgetUnit],[BudgetExt],[LabItem],[MatItem],[LabRate],[SDate],[Vendor])
						VALUES (@JobTItemID, @Btype, @QtyReq, @UM, @BudgetUnit, @BudgetExt,@LabItem, @MatItem, @LabRate,@SDate,@Vendor)
					
					--------- Insert into tblInventoryWHTrans-------------
			        -- Inventory
			        IF EXISTS (SELECT 1 FROM Inv Where Type = 0 AND ID =@MatItem)
			        BEGIN
						if (ISNULL(@QtyReq,0) <> 0)
						BEGIN
							INSERT INTO tblInventoryWHTrans(InvID, WarehouseID,LocationID ,Hand,Balance,fOrder,[Committed],[Available],Screen,ScreenID,Mode,Date,TransType,Batch,FDate)
							VALUES (@MatItem,'OFC',0,0,0,0,( @QtyReq * -1 ),0,'Estimate',@estimate,'Convert',GETDATE(),'Revert',0,GETDATE())

							INSERT INTO tblInventoryWHTrans(InvID, WarehouseID,LocationID ,Hand,Balance,fOrder,[Committed],[Available],Screen,ScreenID,Mode,Date,TransType,Batch,FDate)
							VALUES (@MatItem,'OFC',0,0,0,0,@QtyReq,0,'project',@job,'Convert',GETDATE(),'In',0,GETDATE())
						END
					END
			        --------- End Insert into tblInventoryWHTrans----------

					FETCH NEXT FROM db_cursor_mat INTO 
						@estimate, @EstLine, @JfDesc, @QtyReq, @BudgetExt, @BudgetUnit, @LabHours, @LabRate, @LabExt, @JCode, @MatMod, @LabMod,  @LabItem, @MatItem, @SDate, @Vendor, @Currency, @BType

				END  

				CLOSE db_cursor_mat  
				DEALLOCATE db_cursor_mat
				-------------- END INSERT PROJECT BOM ITEMS ---------------------

				---==============================********END*******===============================----- 


				-------------- BEGIN INSERT PROJECT MILESTONE ITEMS --------------
				--Declare @BAmt NUMERIC(30, 2) = 0
				IF(@IsSglBilAmt = 0)
				BEGIN
					DECLARE db_cursor_lb CURSOR FOR 

					SELECT 	EstimateI.Line,	EstimateI.Code AS JCode,EstimateI.fDesc,Milestone.MilestoneName,Milestone.RequiredBy,
							ISNULL(Milestone.Type,0) AS Type, OrgDep.Department, EstimateI.Amount, ISNULL(Milestone.Quantity, 1), 
							ISNULL(Milestone.Price, EstimateI.Amount),
							ChangeOrder
					FROM	EstimateI 
						LEFT JOIN Milestone ON EstimateI.ID = Milestone.EstimateIId
						LEFT JOIN OrgDep ON OrgDep.ID = Milestone.Type 
					WHERE EstimateI.Estimate = @estimate AND EstimateI.Type = 0

					OPEN db_cursor_lb  
					FETCH NEXT FROM db_cursor_lb INTO
						@EstLine, @JCode, @JfDesc, @MilestoneName, @RequiredByDt, @MType, @Department, @BRev, @MQuantity, @MPrice, @MChangeOrder
					
					WHILE @@FETCH_STATUS = 0
					BEGIN  		
						SELECT @Line=isnull(max(line),0) + 1 from JobTItem where job=@Job and Type=0

						INSERT INTO JobTItem (JobT,Job,Type,fDesc,Code,Actual,Budget,Line,OrderNo,GroupID
							, EstConvertId, EstConvertLine)
						VALUES (@template, @Job, 0, @JfDesc, @JCode, 0, @BRev,@Line,@Line,@GroupID
							, @estimate, @EstLine)
					
						SET @JobTItemID = SCOPE_IDENTITY();
					 
						INSERT INTO [dbo].[Milestone]([JobTItemID], [MilestoneName], [RequiredBy], [CreationDate],[Type], [Amount], Quantity, Price, ChangeOrder)
							VALUES(@JobTItemID, @MilestoneName, @RequiredByDt, GETDATE (), @MType, @BRev, @MQuantity, @MPrice, @MChangeOrder)
				  
						--SET @BAmt = @BAmt + @BRev
						FETCH NEXT FROM db_cursor_lb INTO 
							@EstLine, @JCode, @JfDesc, @MilestoneName, @RequiredByDt, @MType, @Department, @BRev, @MQuantity, @MPrice, @MChangeOrder

					END  

					CLOSE db_cursor_lb  
					DEALLOCATE db_cursor_lb
				END
				ELSE
				BEGIN
					DECLARE @TolBilAmt numeric(30,2)
					SELECT @TolBilAmt =	SUM(EstimateI.Amount)
					FROM	EstimateI 
						LEFT JOIN Milestone ON EstimateI.ID = Milestone.EstimateIId
						LEFT JOIN OrgDep ON OrgDep.ID = Milestone.Type 
					WHERE EstimateI.Estimate = @estimate AND EstimateI.Type = 0

					SELECT @Line=isnull(max(line),0) + 1 from JobTItem where job=@Job and Type=0

					SELECT TOP 1
						@JCode = EstimateI.Code
						--, @JfDesc= EstimateI.fDesc
						--, @MilestoneName = Milestone.MilestoneName
						, @RequiredByDt = Milestone.RequiredBy
						, @MType = ISNULL(Milestone.Type,0)
						, @Department = OrgDep.Department
						, @MChangeOrder = Milestone.ChangeOrder
					FROM	EstimateI 
						LEFT JOIN Milestone ON EstimateI.ID = Milestone.EstimateIId
						LEFT JOIN OrgDep ON OrgDep.ID = Milestone.Type 
					WHERE EstimateI.Estimate = @estimate AND EstimateI.Type = 0
					
					INSERT INTO JobTItem (JobT,Job,Type,fDesc,Code,Actual,Budget,Line,OrderNo,GroupID
						, EstConvertId)
					VALUES (@template, @Job, 0, @EstDesc, @JCode, 0, @TolBilAmt,@Line,@Line,@GroupID
						, @estimate)
					
					SET @JobTItemID = SCOPE_IDENTITY();

					INSERT INTO [dbo].[Milestone]([JobTItemID], [MilestoneName], [RequiredBy], [CreationDate],[Type], [Amount], Quantity, Price, ChangeOrder)
							VALUES(@JobTItemID, @EstDesc, @RequiredByDt, GETDATE (), @MType, @TolBilAmt, 1, @TolBilAmt, @MChangeOrder)

					--SET @BAmt = @TolBilAmt
				END

				-- Updating Contract Billing Amount by Billing Value from Estimate
				-- @contract != 0: Contract has just created on converting
				-- @type = 0: Template is "Maintence Recurring"
				--IF (@contract != 0 AND @type = 0)
				--BEGIN
				--	Update Contract Set BAmt = ISNULL(@BAmt, 0) WHERE @contract = Job
				--END
			-------------- END INSERT PROJECT MILESTONE ITEMS --------------
			END


		END 
		ELSE
		BEGIN 	
			RAISERROR ('Estimate already converted to project!',16,1)	
		END 
	END
    ELSE	 
	BEGIN
		IF (@loc=0) RAISERROR ('Project cannot be created for Leads. Please convert the Lead to Customer!',16,1)  
		ELSE if(@OpportunityID=0) RAISERROR ('Opportunity not created for Estimate.',16,1) 	
    END 

    SELECT @job 

	/*-- Logs --*/
	DECLARE  @SoldStatusName  varchar(50)
	SELECT TOP 1 @SoldStatusName=Name FROM OEStatus WHERE ID = 5
	-- Estimate Status
	IF(@CurrStatus != 5)
	BEGIN
		DECLARE @CurrStatusName varchar(50)
		SELECT TOP 1 @CurrStatusName=Name FROM OEStatus WHERE ID = @CurrStatus
		EXEC log2_insert @UpdatedBy,'Estimate',@Estimate,'Estimate Status',@CurrStatusName,@SoldStatusName
	END

	EXEC log2_insert @UpdatedBy,'Estimate',@Estimate,'Project','',@Job

	IF(@CurrOppStatus != 5)
	BEGIN
		EXEC log2_insert @UpdatedBy,'Opportunity',@OpportunityID,'Status',@CurrOppStatusName,@SoldStatusName
	END
	/*-- End Logs --*/

    COMMIT 
END TRY
BEGIN CATCH
	DECLARE @ErrorMessage NVARCHAR(4000);  
	DECLARE @ErrorSeverity INT;  
	DECLARE @ErrorState INT;  
  
	SELECT   
		@ErrorMessage = ERROR_MESSAGE(),  
		@ErrorSeverity = ERROR_SEVERITY(),  
		@ErrorState = ERROR_STATE();  
  
	IF @@TRANCOUNT>0 ROLLBACK	
	RAISERROR (@ErrorMessage, -- Message text.  
				@ErrorSeverity, -- Severity.  
				@ErrorState -- State.  
				);  
    --RETURN
END CATCH