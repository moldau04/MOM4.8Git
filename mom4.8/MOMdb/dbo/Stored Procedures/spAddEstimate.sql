/*--------------------------------------------------------------------
Modified By: Thomas
Modified On: 17 May 2019
Desc: Format code and apply NextEst number from custom table for new Estimate Id

Modified By: Thomas
Modified On: 26 Feb 2019
Desc: For add estimate's equipments

Modified By: Thurstan
Modified On: 30 Nov 2018	
Description: Add Orderno column 
--------------------------------------------------------------------*/
CREATE PROCEDURE [dbo].[spAddEstimate]
	@name varchar(75),
	@fdesc   VARCHAR(255),
	@CompanyName VARCHAR(100),
	@remarks VARCHAR(8000),
	@template  INT,
	@mode    INT,
	@loc int,
	@rol int,
	@CADExchange numeric(30,2),
	@Edited smallint,
	@Status smallint,								 
	@MilestonItem tblTypeEstimateMilestoneItem readonly,
	@BomItem  tblTypeEstimateBomItem readonly,
	@Contact varchar(max),
	@estDate datetime,
	@Jobtype varchar(max),
	@SalesManUerId int,
	@bidDate datetime,
	@billAddress nvarchar(300),
	@phone varchar(28),
	@fax varchar(28),
	@Email varchar(100),
	@EstimateAddress nvarchar (255),
	@EstimateCell nvarchar (22),
	@BidPrice numeric(30,2),
	@Override numeric(30,2),
	@Cont numeric(30,2),
	@OH numeric(30,2),
	@SalesTax VARCHAR(25),
	@Opportunity int,
	@OHPer numeric(30,4),
	@MarkupPer numeric(30,4),
	@CommissionPer numeric(30,4),
	@CommissionVal numeric(30,2),
	@STaxRate numeric(30,4),
	@Category VARCHAR(50),
	@Sales_Tax VARCHAR(25),
	@MarkupVal  numeric(30,2),
	@STaxVal  numeric(30,2),
	@MatExp  numeric(30,2),
	@LabExp  numeric(30,2),
	@OtherExp  numeric(30,2),
	@SubToalVal  numeric(30,2),
	@TotalCostVal  numeric(30,2),
	@PretaxTotalVal  numeric(30,2),
	@PType  smallint,
	@Amount numeric(30,2),
	@BillRate  numeric(30,2),
	@OT  numeric(30,2),
	@RateTravel  numeric(30,2),
	@DT  numeric(30,2),
	@RateMileage  numeric(30,2),
	@RateNT  numeric(30,2),
	@ContPer  numeric(30,4),
	@Discounted  bit,
	@DiscountedNotes  nvarchar(max),
	@EquipItem  tblTypeEquipItem readonly,
	@GroupId INT,
	@GridUserSettings tblTypeGridUserSettings readonly,
	@UpdatedBy varchar(100),
	@CustomItems AS tblTypeCommonCustomItem readonly,
	@EstimateType VARCHAR(50),
	@IsSglBilAmt bit,
	@OpportunityStageID VARCHAR(50),
	@OpportunityName VARCHAR(255)
AS
BEGIN
	
	Declare @RolType smallint 
	DECLARE @jobtypeid INT
	DECLARE @EstimateNo INT
	DECLARE @JfDesc VARCHAR(255)
	DECLARE @JCode VARCHAR(10)
	DECLARE @BType SMALLINT
	DECLARE @QtyReq NUMERIC(30,2)
	DECLARE @UM VARCHAR(50)
	DECLARE @BudgetUnit NUMERIC(30,2)
	DECLARE @BudgetExt NUMERIC(30,2)
	DECLARE @MatItem INT
	DECLARE @MatMod NUMERIC(30,2)
	DECLARE @MatPrice NUMERIC(30,2)
	DECLARE @MatMarkup NUMERIC(30,2)
	DECLARE @STax TINYINT
	DECLARE @Currency VARCHAR(10)
	DECLARE @LabItem INT
	DECLARE @LabMod NUMERIC(30,2)
	DECLARE @LabExt NUMERIC(30,2)
	DECLARE @LabRate NUMERIC(30,2)
	DECLARE @LabHours NUMERIC(30,2)
	DECLARE @LabPrice NUMERIC(30,2)
	DECLARE @LabMarkup NUMERIC(30,2)
	DECLARE @LSTax SMALLINT
	DECLARE @SDate DATETIME
	DECLARE @VendorID INT
	--DECLARE @Vendor VARCHAR(100)
	DECLARE @JLine INT = 0 
	DECLARE @OrderNo INT
	DECLARE @JobT INT
	DECLARE @TotalExt NUMERIC(30,2)
	DECLARE @EstimateItemID INT
	DECLARE @JType SMALLINT = 0
	DECLARE @MilestoneName VARCHAR(150)
	DECLARE @RequiredBy DATETIME
	DECLARE @LeadTime NUMERIC(30,2)
	DECLARE @MilestoneType INT
	DECLARE @BRev NUMERIC(30,2)
	DECLARE @AmountPer VARCHAR(150)
	DECLARE @NextEstimateNo INT = 0
	DECLARE @MPrice NUMERIC(30,2)
	DECLARE @MQuantity NUMERIC(30,2)
	DECLARE @MChangeOrder tinyint

	BEGIN TRY
	BEGIN TRANSACTION
		DECLARE @OpprtAmount numeric(30,2)
		DECLARE @OpprtStageId int 
		SET @OpprtStageId = Convert(int, @OpportunityStageID);
		IF (@Override is not null And @Override != 0) SET @OpprtAmount = @Override
		ELSE SET @OpprtAmount = @BidPrice
		IF @Opportunity is not null AND @Opportunity != 0
		BEGIN
			-- Update amount for opportunity
			
			EXEC spUpdateLeadRevenueByEstimate @Opportunity,@OpprtAmount,@UpdatedBy,0,@OpprtStageId
		END
		ELSE
		BEGIN
			SET @Opportunity = 0
			EXEC @Opportunity = spAddOpportunity @Opportunity,-- @ID          INT out,
				@OpportunityName,			   --@fdesc       VARCHAR(75),
				@rol,						   --@rol         INT,
				null,						   --@Probability INT,
				7, -- Quoted				   --@Status      SMALLINT,
				@fdesc,						   --@Remarks     VARCHAR(max),
				@bidDate,					   --@closedate   DATETIME,
				0, -- AddNew				   --@Mode        SMALLINT,
				null,						   --@owner int,
				null,						   --@NextStep varchar(50),
				null,							--@desc varchar(max),
				null,							--@Source varchar(70),
				@OpprtAmount,				   	--@Amount numeric(30,2),
				@UpdatedBy,					   	--@Fuser varchar(50),
				@SalesManUerId,				   	--@AssignedToID int,
				@UpdatedBy,					   	--@UpdateUser varchar(50),
				null,						   	--@closed smallint,
				null,						   	--@TicketID int,
				null,						   	--@BusinessType varchar(50),
				null,						   	--@Product varchar(50),
				@OpprtStageId,				   	--@OpportunityStageID int,
				@CompanyName,				   	--@CompanyName VARCHAR(75),
				0,							   	--@IsSendMailToSalesPer bit=0,
				null							   	-- @Department Int
		END			
		
		IF @Opportunity = 0
		BEGIN
			RAISERROR ('Error on create Opportunity',16,1)
		END

		SELECT @RolType = Type FROM   Rol WHERE  ID = @rol
		IF(@RolType = 3 )
		BEGIN
			SET @loc= 0
		END

		SET @jobtypeid=CAST(@Jobtype AS INT)--(SELECT JobT.ID FROM JobT WHERE JobT.Type=CAST(@Jobtype AS INT))
			
		SET @EstimateNo = (SELECT (MAX(ISNULL(ID,0))+1) FROM Estimate)

		IF @EstimateNo IS NULL
		BEGIN
			SET @EstimateNo=1;
		END

		-- Get next estimateno from custom table
		SELECT @NextEstimateNo=Label FROM Custom WHERE Name = 'NextEst'
		-- Check if nextEstNo > maxEstNo then take it for new EstNo
		IF @NextEstimateNo > @EstimateNo
		BEGIN
			SET @EstimateNo = @NextEstimateNo
		END

		SET IDENTITY_INSERT Estimate ON  

		INSERT INTO Estimate
        (
			ID, Name,fDate,fdesc,CompanyName,Remarks,EstTemplate, RolID,
			LocID, fFor,CADExchange, Status,Template,Type,
			EstimateBillAddress,BDate,
			Phone,Fax,EstimateUserId,EstimateAddress,EstimateEmail,EstimateCell,
			Cont, Price, Quoted, Overhead, Opportunity, STaxName,STaxRate,OHPer,MarkupPer,CommissionPer,CommissionVal,Category,
			MarkupVal,STaxVal,MatExp,LabExp,OtherExp,SubToalVal,TotalCostVal,PretaxTotalVal,PType,Amount,BillRate,OT,RateTravel,DT,
			RateMileage,RateNT,ContPer
			, Discounted
			, DiscountedNotes
			, GroupId
			, Contact
			, EstimateType
			, IsSglBilAmt
		)
        VALUES     
		(
			@EstimateNo,@name,@estDate,@fdesc,@CompanyName,@remarks,0,@rol,
			@loc,case @RolType when 3 then 'PROSPECT' else 'ACCOUNT' end,@CADExchange,@Status,@template,@jobtypeid,
			@billAddress,@bidDate,
			@phone,@fax,@SalesManUerId,@EstimateAddress,@Email,@EstimateCell,
			@Cont, @BidPrice, @Override, @OH, @Opportunity,@SalesTax,@STaxRate,@OHPer,@MarkupPer,@CommissionPer,@CommissionVal,@Category,
			@MarkupVal,@STaxVal,@MatExp,@LabExp,@OtherExp,@SubToalVal,@TotalCostVal,@PretaxTotalVal,@PType,@Amount,@BillRate,@OT,@RateTravel,@DT,
			@RateMileage,@RateNT,@ContPer
			, @Discounted
			, @DiscountedNotes
			, @GroupId
			, @Contact
			, @EstimateType
			, @IsSglBilAmt
		)

		IF @Opportunity <> 0
		BEGIN
			--UPDATE Lead SET [Status]=7  WHERE ID=@Opportunity

			DECLARE @CurrEstOpprtStatus varchar(50) = ''
			SELECT @CurrEstOpprtStatus = oe.Name
			FROM Lead ld INNER JOIN OEStatus oe ON oe.ID = ld.Status WHERE ld.ID = @Opportunity

			-- Check and update Opportunity after updating Estimate Status
			IF EXISTS (
				SELECT TOP 1 1
				from lead ld
				inner join Estimate e on e.Opportunity = ld.ID 
				where e.Status = 5 
				and ld.id = @Opportunity
				and e.Opportunity is not null
			) -- There is a Sold estimate link to this opportunity: Set Opprt status to 'Sold'
			BEGIN
				UPDATE LEAD SET Status = 5 WHERE ID = @Opportunity
			END
			ELSE -- There is no Sold estimate link to this opportunity but have an open estimate link to this opportunity. Set Opprt status to 'Quoted'
			BEGIN
				UPDATE LEAD SET Status = 7 WHERE ID = @Opportunity
			END

			DECLARE @EstOpprtStatus  varchar(50) = ''
			SELECT @EstOpprtStatus = oe.Name
			FROM Lead ld INNER JOIN OEStatus oe ON oe.ID = ld.Status WHERE ld.ID = @Opportunity

			IF(@EstOpprtStatus != @CurrEstOpprtStatus)
				EXEC log2_insert @UpdatedBy,'Opportunity',@Opportunity,'Status',@CurrEstOpprtStatus,@EstOpprtStatus

			-- TODO: this for 
			--DECLARE @countEst int;
			--SET @countEst = ISNULL((SELECT COUNT(*) FROM Estimate WHERE Opportunity = @Opportunity),0);
			--IF @countEst = 1
			--BEGIN
			--	UPDATE Lead SET CloseDate = @bidDate WHERE ID=@Opportunity
			--END
			--ELSE IF @countEst = 0
			--BEGIN
			--	UPDATE Lead SET CloseDate = null WHERE ID=@Opportunity
			--END
		END

		SET IDENTITY_INSERT Estimate ON  
		
		IF (@RolType = 3)
		BEGIN
			UPDATE Prospect SET STax = @Sales_Tax WHERE Rol = @rol
		END
			
		------------------------ BEGIN INSERT BOM ITEMS (INSERT ESTIMATE) ------------------------
		DECLARE @CountBOMItem int;
		DECLARE @CountBOMCursor int = 0;
		SET @CountBOMItem = ISNULL((SELECT Count(*) FROM @BomItem),0);
		IF(@CountBOMItem > 0)	
		BEGIN			
			DECLARE db_cursor_BOM CURSOR FOR 
			SELECT fDesc, Code, Line, BType,
					QtyReq, UM, BudgetUnit, BudgetExt,
					MatMod, MatPrice, MatMarkup, STax,
					Currency, MatItem, VendorId,
					LabItem, LabMod, LabExt, LabRate, 
					LabHours, SDate, TotalExt, LabPrice, 
					LabMarkup, LStax, OrderNo
			FROM @BomItem
			OPEN db_cursor_BOM  
			FETCH NEXT FROM db_cursor_BOM INTO 
					@JfDesc, @JCode, @JLine, @BType,
					@QtyReq, @UM, @BudgetUnit, @BudgetExt,
					@MatMod, @MatPrice, @MatMarkup, @STax,
					@Currency, @MatItem, @VendorId,
					@LabItem, @LabMod, @LabExt, @LabRate, 
					@LabHours, @SDate, @TotalExt, @LabPrice, 
					@LabMarkup, @LSTax, @OrderNo
			WHILE @@FETCH_STATUS = 0
			BEGIN  	
				SET @CountBOMCursor += 1; 
				--SET @JLine = @JLine + 1
				SET @JLine = @JLine
				SET @OrderNo = @OrderNo
				--SET @EstimateItemID = (SELECT MAX(ISNULL(ID,0))+1 FROM EstimateI)

				--SET IDENTITY_INSERT EstimateI ON          
					
				INSERT INTO EstimateI
					(	Estimate, Line, fDesc, 
						Quan, Cost, Price, Hours, 
						Rate, Labor, Amount, STax, 
						Code, Vendor, Currency, Type, 
						MMU, MMUAmt, LMU, LMUAmt, 
						LStax, LMod, MMod, OrderNo
					)	
				VALUES
					(	@EstimateNo, @JLine, LEFT(@JfDesc,150), 
						@QtyReq, @BudgetExt, @BudgetUnit, @LabHours, 
						@LabRate, @LabExt, @TotalExt, @STax,
						@JCode,	@VendorID, @Currency, 1, 
						@MatMarkup, @MatPrice, @LabMarkup, @LabPrice, 
						@LSTax, @LabMod, @MatMod, @OrderNo
					)
					
				------ JobTItem.Type = 1 is expense type
					
				--SET IDENTITY_INSERT EstimateI OFF
				SET @EstimateItemID = SCOPE_IDENTITY()
				INSERT INTO [dbo].[BOM]
						([EstimateIId],[Type],[QtyRequired],[UM],[BudgetUnit],[BudgetExt],
							[LabItem],[MatItem],[LabRate],[SDate],[Vendor])
				VALUES (@EstimateItemID, @Btype, @QtyReq, @UM, @BudgetUnit, @BudgetExt, 
							@LabItem, @MatItem, @LabRate,@SDate,@VendorID)

				--------- Insert into tblInventoryWHTrans-------------
				DECLARE @INV_WarehouseID varchar(50) = 'OFC';
				-- Inventory
				IF EXISTS (SELECT 1 FROM Inv Where Type = 0 AND ID =@MatItem)
				BEGIN
					INSERT INTO tblInventoryWHTrans(InvID, WarehouseID,LocationID ,Hand,Balance,fOrder,[Committed],[Available],Screen,ScreenID,Mode,Date,TransType,Batch,FDate)
					VALUES (@MatItem,'OFC',0,0,0,0,@QtyReq,0,'Estimate',@EstimateNo,'Add',GETDATE(),'In',0,GETDATE())
				END
				--------- End Insert into tblInventoryWHTrans----------
				FETCH NEXT FROM db_cursor_BOM INTO
					@JfDesc, @JCode, @JLine, @BType,
					@QtyReq, @UM, @BudgetUnit, @BudgetExt,
					@MatMod, @MatPrice, @MatMarkup, @STax,
					@Currency, @MatItem, @VendorId,
					@LabItem, @LabMod, @LabExt, @LabRate, 
					@LabHours, @SDate, @TotalExt, @LabPrice, 
					@LabMarkup, @LSTax, @OrderNo
		
				END  
			CLOSE db_cursor_BOM  
			DEALLOCATE db_cursor_BOM
		END

		IF @CountBOMItem > 0 AND @CountBOMCursor <> @CountBOMItem
		BEGIN
			IF @@TRANCOUNT>0  ROLLBACK	
			RAISERROR ('An error on updating BOM list',16,1)
			RETURN
		END
		------------------------ END INSERT BOM ITEMS (INSERT ESTIMATE) ------------------------

		------------------------ BEGIN INSERT MILESTONE ITEMS (INSERT ESTIMATE) ------------------------

		SET @JLine = 0
		SET @OrderNo = 0
		DECLARE @CountMilestonItem int;
		DECLARE @CountCursorMileston int = 0;
		SET @CountMilestonItem = ISNULL((SELECT Count(*) FROM @MilestonItem),0);
		IF(@CountMilestonItem > 0)
		BEGIN
			DECLARE db_cursor_Mileston CURSOR FOR 
			SELECT fDesc, JCode, Line, MilesName, RequiredBy, Type, Amount,AmountPer, OrderNo, Quantity, Price,ChangeOrder FROM @MilestonItem
			OPEN db_cursor_Mileston  
			FETCH NEXT FROM db_cursor_Mileston INTO 
				@JfDesc, @JCode, @JLine, @MilestoneName, @RequiredBy, @MilestoneType, @BRev,@AmountPer, @OrderNo, @MQuantity, @MPrice,@MChangeOrder
			WHILE @@FETCH_STATUS = 0
			BEGIN  		
				SET @CountCursorMileston +=1;
				--SET @EstimateItemID = (SELECT MAX(ISNULL(ID,0)+1) FROM EstimateI)
						
				--SET IDENTITY_INSERT EstimateI ON    	
				IF (@EstimateType = 'bid')
				BEGIN
					SET @MQuantity= 1
					SET @MPrice = @BRev
				END
				ELSE IF (@EstimateType = 'quote')
				BEGIN
					SET @AmountPer = 0
				END
						
				INSERT INTO EstimateI
					(Estimate, Line, fDesc, Code, Type, Amount,AmountPer, OrderNo, Quan, Price)	
				VALUES
					(@EstimateNo, @JLine, @JfDesc, @JCode, 0, @BRev,@AmountPer, @OrderNo, @MQuantity, @MPrice)
				SET @EstimateItemID = SCOPE_IDENTITY()

				-- SET IDENTITY_INSERT EstimateI OFF    
				-- EstimateI.Type = 0 is revenue type
					
				INSERT INTO [dbo].[Milestone]
					([EstimateIId], [MilestoneName], [RequiredBy], [CreationDate], [ProjAcquistDate], [Type], [Amount], Quantity, Price, ChangeOrder)
				VALUES
					(@EstimateItemID, @MilestoneName, @RequiredBy, GETDATE (), NULL, @MilestoneType, @BRev, @MQuantity, @MPrice,@MChangeOrder)
				

				FETCH NEXT FROM db_cursor_Mileston INTO @JfDesc, @JCode, @JLine, @MilestoneName, @RequiredBy, @MilestoneType, @BRev,@AmountPer,@OrderNo, @MQuantity, @MPrice,@MChangeOrder
			END  

			CLOSE db_cursor_Mileston  
			DEALLOCATE db_cursor_Mileston
		END

		IF @CountMilestonItem > 0 AND @CountCursorMileston <> @CountMilestonItem
		BEGIN
			IF @@TRANCOUNT>0  ROLLBACK	
			RAISERROR ('An error on updating Billing list',16,1)
			RETURN
		END
		------------------------ END INSERT MILESTONE ITEMS (INSERT ESTIMATE) ------------------------

		------------------------ BEGIN INSERT EQUIPMENT ITEMS (INSERT ESTIMATE) ----------------------

		-- Delete all the old equipment the group
		DELETE tblEstimateGroupEquipment WHERE GroupId = @GroupId
		-- And replace it by the new one
		INSERT INTO tblEstimateGroupEquipment (GroupId, EquipmentID) SELECT @GroupId, EquipmentID  FROM @EquipItem

		------------------------ END INSERT EQUIPMENT ITEMS (INSERT ESTIMATE) ------------------------

		-- Insert the task contact to Phone table
		IF(ISNULL(@Contact,'') != '')
		BEGIN
			IF NOT EXISTS(SELECT 1 FROM Phone WHERE Rol =@Rol and fDesc = @contact)
			BEGIN 
				INSERT INTO Phone
				(
					Rol,fDesc,Phone,Fax,Cell,Email,EmailRecQuote
				)
				VALUES
				(
					@Rol,@contact,@phone,@fax,@EstimateCell,@Email,1
				)
			END
			ELSE
			BEGIN
				UPDATE Phone 
				SET Phone = @Phone
					, Email = @Email
					, Fax = @fax
					, Cell = @EstimateCell
					, EmailRecQuote = 1
				WHERE Rol =@Rol and fDesc = @contact
			END

		END	 

		----------------------------------- tblCustomJob --------------------------------------  
        --------------------------------------- start -----------------------------------------  

        --SELECT * FROM @CustomItem ctItem INNER JOIN tblCustomJob ON   
		DECLARE @Screen varchar(50) = 'Estimate'
        --delete from tblCustomJob where JobID = @job            
        -- update custom details for project    
		DECLARE @tblCustomFieldsId int
		DECLARE @Value varchar(255)
		DECLARE @Label varchar(255)
		DECLARE @TabLine smallint
		DECLARE @Format smallint
		DECLARE @UpdatedDate datetime
		DECLARE @Username varchar(50)
		DECLARE @IsAlert bit;
		DECLARE @TeamMember varchar(max);
		DECLARE @TeamMemberDisplay varchar(max);

        DECLARE db_cursor_EstTags CURSOR FOR

        SELECT
            [ID],
            [Label],
            [Line],
            [Value],
            [Format],
            [UpdatedDate],
            [Username],
            IsAlert,
            TeamMember,
            TeamMemberDisplay
        FROM @CustomItems

        OPEN db_cursor_EstTags
        FETCH NEXT FROM db_cursor_EstTags INTO @tblCustomFieldsId, @Label, @TabLine, @Value, @Format, @UpdatedDate, @Username, @IsAlert, @TeamMember, @TeamMemberDisplay

        WHILE @@FETCH_STATUS = 0
        BEGIN
			IF EXISTS (SELECT 1 FROM [tblCommonCustomFieldsValue]
				WHERE tblCommonCustomFieldsID = @tblCustomFieldsId
				AND Ref = @EstimateNo)
            BEGIN
                  
                IF EXISTS (SELECT
                        1
                    FROM tblCommonCustomFieldsValue
                    WHERE tblCommonCustomFieldsID = @tblCustomFieldsId
                    AND Ref = @EstimateNo
					AND Screen = @Screen
                    AND ((@Format != 5
                    AND ISNULL([Value], '') != ISNULL(@Value, ''))
                    OR (@Format = 5
                    AND ISNULL([Value], 'False') != ISNULL(@Value, 'False'))))
                BEGIN
                    UPDATE [dbo].tblCommonCustomFieldsValue
                    SET [Value] = @Value,
                        [IsAlert] = @IsAlert,
                        [TeamMember] = @TeamMember,
                        [TeamMemberDisplay] = @TeamMemberDisplay,
                        [UpdatedDate] = @UpdatedDate,
                        [Username] = @Username
                    WHERE tblCommonCustomFieldsID = @tblCustomFieldsId
						AND Ref = @EstimateNo
						AND Screen = @Screen
                        AND ((@Format != 5
                        AND ISNULL([Value], '') != ISNULL(@Value, ''))
                        OR (@Format = 5
                        AND ISNULL([Value], 'False') != ISNULL(@Value, 'False')))
                END
                ELSE
                BEGIN
                    UPDATE [dbo].tblCommonCustomFieldsValue
                    SET [Value] = @Value,
                        [IsAlert] = @IsAlert,
                        [TeamMember] = @TeamMember,
                        [TeamMemberDisplay] = @TeamMemberDisplay
                    WHERE tblCommonCustomFieldsID = @tblCustomFieldsId
						AND Ref = @EstimateNo
						AND Screen = @Screen
                END
            END
            ELSE
            BEGIN
                IF ((@Format != 5 AND '' != ISNULL(@Value, ''))
                    OR (@Format = 5 AND 'False' != ISNULL(@Value, 'False')))
                    --OR 0 != @IsAlert OR ''!=ISNULL(@TeamMember,'') OR '' != ISNULL(@TeamMemberDisplay,''))
                BEGIN
                    INSERT INTO [dbo].tblCommonCustomFieldsValue (Ref
                        , [tblCommonCustomFieldsID]
                        , [Value]
                        , [UpdatedDate]
                        , [Username]
                        , [IsAlert]
                        , [TeamMember]
                        , [TeamMemberDisplay]
						, [Screen]
                        )
                    VALUES (@EstimateNo, @tblCustomFieldsId, @Value, @UpdatedDate, @Username, @IsAlert, @TeamMember, @TeamMemberDisplay, @Screen)
                END
                ELSE
                BEGIN
                    INSERT INTO [dbo].tblCommonCustomFieldsValue (Ref
                    , [tblCommonCustomFieldsID]
                    , [Value]
                    , [IsAlert]
                    , [TeamMember]
                    , [TeamMemberDisplay]
					, [Screen]
                    )
                    VALUES (@EstimateNo, @tblCustomFieldsId, @Value, @IsAlert, @TeamMember, @TeamMemberDisplay,@Screen)
                END
            END
            FETCH NEXT FROM db_cursor_EstTags INTO @tblCustomFieldsId, @Label, @TabLine, @Value, @Format, @UpdatedDate, @Username, @IsAlert, @TeamMember, @TeamMemberDisplay
        END

        CLOSE db_cursor_EstTags
        DEALLOCATE db_cursor_EstTags
		--------------------------------------- end -----------------------------------------  


          	
		UPDATE tblUserGridSettings SET ColumnsSettings = dd.ColumnSettings 
			FROM tblUserGridSettings gus 
				INNER JOIN @GridUserSettings dd 
				ON gus.GridId = dd.GridId AND gus.PageName = dd.PageName AND gus.UserId = dd.UserId

		INSERT INTO tblUserGridSettings (UserId,PageName,GridId,ColumnsSettings)
			SELECT dd.UserId, dd.PageName, dd.GridId, dd.ColumnSettings 
			FROM tblUserGridSettings gus RIGHT JOIN @GridUserSettings dd 
			ON gus.GridId = dd.GridId AND gus.PageName = dd.PageName AND gus.UserId = dd.UserId 
			WHERE gus.UserId is null AND gus.PageName is null AND gus.GridId is null		
			

		/********Start Logs************/

		-- Estimate date
		IF(@estDate is not null And @estDate != '')
		BEGIN 	
			DECLARE @estDateStr nvarchar(150)
			SELECT @estDateStr = convert(varchar, @estDate, 101)
			EXEC log2_insert @UpdatedBy,'Estimate',@EstimateNo,'Estimate Date','',@estDateStr
		END

		-- Bid Close Date
		IF(@bidDate is not null And @bidDate != '')
		BEGIN 	
			DECLARE @bidDateStr nvarchar(150)
			SELECT @bidDateStr = convert(varchar, @bidDate, 101)
			EXEC log2_insert @UpdatedBy,'Estimate',@EstimateNo,'Bid Close Date','',@bidDateStr
		END

		-- Description
		IF(@fdesc is not null And @fdesc != '')
		BEGIN 	
			EXEC log2_insert @UpdatedBy,'Estimate',@EstimateNo,'Description','',@fdesc
		END

		-- Category
		IF(@Category is not null And @Category != '' And @Category != 'Select')
		BEGIN 	
			EXEC log2_insert @UpdatedBy,'Estimate',@EstimateNo,'Category','',@Category
		END

		-- Estimate Status
		IF(@Status is not null And @Status != '')
		BEGIN
			DECLARE @StatusName varchar(50)
			SELECT TOP 1 @StatusName=Name FROM OEStatus WHERE ID = @Status
			IF(@StatusName is not null And @StatusName != '')
			EXEC log2_insert @UpdatedBy,'Estimate',@EstimateNo,'Estimate Status','',@StatusName
		END
		
		-- Assigned To
		IF(@SalesManUerId is not null And @SalesManUerId != 0)
		BEGIN
			DECLARE @Assigned varchar(50)
			SELECT TOP 1 @Assigned = t.SDesc from terr t where ID = @SalesManUerId
			IF(@Assigned is not null And @Assigned != '')
			EXEC log2_insert @UpdatedBy,'Estimate',@EstimateNo,'Assigned To','',@Assigned
		END

		-- Customer Name
		IF(@CompanyName is not null And @CompanyName != '')
		BEGIN
			EXEC log2_insert @UpdatedBy,'Estimate',@EstimateNo,'Customer Name','',@CompanyName
		END
		-- Location Name
		IF(@billAddress is not null And @billAddress != '')
		BEGIN
			EXEC log2_insert @UpdatedBy,'Estimate',@EstimateNo,'Location Name','',@billAddress
		END
		-- Contact Name
		IF(@Contact is not null And @Contact != '')
		BEGIN
			--DECLARE @strContactName varchar(255)
			--IF(@Contact = '0')
			--BEGIN
			--	SELECT @strContactName = contact FROM Rol WHERE ID = @rol AND Contact is not null AND Contact != ''
			--END
			--ELSE
			--BEGIN
			--	SELECT TOP 1 @strContactName=fDesc FROM PHONE WHERE PHONE.ROL=@rol AND ID=@Contact
			--END
			
			--EXEC Log2_Insert_new @UpdatedBy,'Estimate',@EstimateNo,'Contact Name',@strContactName

			EXEC Log2_Insert @UpdatedBy,'Estimate',@EstimateNo,'Contact Name','',@Contact
		END
		-- Email
		IF(@Email is not null And @Email != '')
		BEGIN
			EXEC log2_insert @UpdatedBy,'Estimate',@EstimateNo,'Email','',@Email
		END
		-- Phone
		IF(@phone is not null And @phone != '')
		BEGIN
			EXEC log2_insert @UpdatedBy,'Estimate',@EstimateNo,'Phone','',@phone
		END
		-- Cell/Mobile
		IF(@EstimateCell is not null And @EstimateCell != '')
		BEGIN
			EXEC log2_insert @UpdatedBy,'Estimate',@EstimateNo,'Cell/Mobile','',@EstimateCell
		END
		-- Fax
		IF(@fax is not null And @fax != '')
		BEGIN
			EXEC log2_insert @UpdatedBy,'Estimate',@EstimateNo,'Fax','',@fax
		END
		-- Discounted
		EXEC log2_insert @UpdatedBy,'Estimate',@EstimateNo,'Discounted','',@Discounted
		-- Discounted Notes
		IF(@DiscountedNotes is not null And @DiscountedNotes != '')
		BEGIN
			EXEC log2_insert @UpdatedBy,'Estimate',@EstimateNo,'Discounted Notes','',@DiscountedNotes
		END
		-- Template
		IF(@template is not null And @template != 0)
		BEGIN
			DECLARE @templateName VARCHAR(50) = ''
			SELECT TOP 1 @templateName = t.fDesc from JobT t where ID = @template
			EXEC log2_insert @UpdatedBy,'Estimate',@EstimateNo,'Template','',@templateName
		END
		-- Department
		
		-- Opportunity #
		IF(@Opportunity is not null And @Opportunity != 0)
		BEGIN
			EXEC log2_insert @UpdatedBy,'Estimate',@EstimateNo,'Opportunity #','',@Opportunity
		END
		-- Opportunity Name
		-- Opportunity Stage
		-- Group Name
		IF(@GroupId is not null And @GroupId != 0)
		BEGIN
			DECLARE @GroupName VARCHAR(255) = ''
			SELECT TOP 1 @GroupName = GroupName from tblEstimateGroup WHERE Id = @GroupId
			EXEC log2_insert @UpdatedBy,'Estimate',@EstimateNo,'Group Name','',@GroupName
		END
		-- Equipment
		-- Bid Price
		IF(@BidPrice is not null And @BidPrice != 0)
		BEGIN
			DECLARE @strBidPrice varchar(50)
			SET @strBidPrice = Convert(varchar(50),@BidPrice)
			EXEC log2_insert @UpdatedBy,'Estimate',@EstimateNo,'Bid Price','',@strBidPrice
		END
		-- Final Bid Price
		IF(@Override is not null And @Override != 0)
		BEGIN
			DECLARE @strFinalBidPrice varchar(50)
			SET @strFinalBidPrice = Convert(varchar(50),@Override)
			EXEC log2_insert @UpdatedBy,'Estimate',@EstimateNo,'Final Bid Price','',@strFinalBidPrice
		END

		-- Material Exp
		IF(@MatExp is not null And @MatExp != 0)
		BEGIN
			DECLARE @strMatExp varchar(50)
			SET @strMatExp = Convert(varchar(50),@MatExp)
			EXEC log2_insert @UpdatedBy,'Estimate',@EstimateNo,'Material Exp','',@strMatExp
		END
		-- Labor Exp
		IF(@LabExp is not null And @LabExp != 0)
		BEGIN
			DECLARE @strLabExp varchar(50)
			SET @strLabExp = Convert(varchar(50),@LabExp)
			EXEC log2_insert @UpdatedBy,'Estimate',@EstimateNo,'Labor Exp','',@strLabExp
		END
		-- Other Exp
		IF(@OtherExp is not null And @OtherExp != 0)
		BEGIN
			DECLARE @strOtherExp varchar(50)
			SET @strOtherExp = Convert(varchar(50),@OtherExp)
			EXEC log2_insert @UpdatedBy,'Estimate',@EstimateNo,'Other Exp','',@strOtherExp
		END
		-- Contingencies
		IF(@Cont is not null And @Cont != 0)
		BEGIN
			DECLARE @strContingencies varchar(50)
			SET @strContingencies = Convert(varchar(50),@Cont)
			EXEC log2_insert @UpdatedBy,'Estimate',@EstimateNo,'Contingencies','',@strContingencies
		END
		-- Contingencies Percent
		IF(@ContPer is not null And @ContPer != 0)
		BEGIN
			DECLARE @strContPercent varchar(50)
			SET @strContPercent = Convert(varchar(50),@ContPer)
			EXEC log2_insert @UpdatedBy,'Estimate',@EstimateNo,'Contingencies Percentage','',@strContPercent
		END
		-- Subtotal	
		IF(@SubToalVal is not null And @SubToalVal != 0)
		BEGIN
			DECLARE @strSubTotal varchar(50)
			SET @strSubTotal = Convert(varchar(50),@SubToalVal)
			EXEC log2_insert @UpdatedBy,'Estimate',@EstimateNo,'Subtotal','',@strSubTotal
		END
		-- Overhead
		IF(@OH is not null And @OH != 0)
		BEGIN
			DECLARE @strOH varchar(50)
			SET @strOH = Convert(varchar(50),@OH)
			EXEC log2_insert @UpdatedBy,'Estimate',@EstimateNo,'Overhead','',@strOH
		END
		-- Overhead Percent
		IF(@OHPer is not null And @OHPer != 0)
		BEGIN
			DECLARE @strOHPer varchar(50)
			SET @strOHPer = Convert(varchar(50),@OHPer)
			EXEC log2_insert @UpdatedBy,'Estimate',@EstimateNo,'Overhead Percentage','',@strOHPer
		END
		-- Total Cost
		IF(@TotalCostVal is not null And @TotalCostVal != 0)
		BEGIN
			DECLARE @strTotalCost varchar(50)
			SET @strTotalCost = Convert(varchar(50),@TotalCostVal)
			EXEC log2_insert @UpdatedBy,'Estimate',@EstimateNo,'Total Cost','',@strTotalCost
		END
		-- Profit
		IF(@MarkupVal is not null And @MarkupVal != 0)
		BEGIN
			DECLARE @strProfit varchar(50)
			SET @strProfit = Convert(varchar(50),@MarkupVal)
			EXEC log2_insert @UpdatedBy,'Estimate',@EstimateNo,'Profit','',@strProfit
		END
		-- Profit Percent
		IF(@MarkupPer is not null And @MarkupPer != 0)
		BEGIN
			DECLARE @strProfitPer varchar(50)
			SET @strProfitPer = Convert(varchar(50),@MarkupPer)
			EXEC log2_insert @UpdatedBy,'Estimate',@EstimateNo,'Profit Percentage','',@strProfitPer
		END
		-- Pretax total
		IF(@PretaxTotalVal is not null And @PretaxTotalVal != 0)
		BEGIN
			DECLARE @strPretaxTotal varchar(50)
			SET @strPretaxTotal = Convert(varchar(50),@PretaxTotalVal)
			EXEC log2_insert @UpdatedBy,'Estimate',@EstimateNo,'Pretax total','',@strPretaxTotal
		END
		-- Sales Tax
		IF(@Sales_Tax is not null AND @Sales_Tax!='')
		BEGIN
			--DECLARE @strSTaxName varchar(50)
			--SET @strSTaxRate = Convert(varchar(50),@Sales_Tax)
			EXEC log2_insert @UpdatedBy,'Estimate',@EstimateNo,'Sales Tax','',@Sales_Tax
		END
		-- Sales Tax Rate
		IF(@STaxRate is not null And @STaxRate != 0)
		BEGIN
			DECLARE @strSTaxRate varchar(50)
			SET @strSTaxRate = Convert(varchar(50),@STaxRate)
			EXEC log2_insert @UpdatedBy,'Estimate',@EstimateNo,'Sales Tax Rate','',@strSTaxRate
		END
		-- Sales Tax Value
		IF(@STaxVal is not null And @STaxVal != 0)
		BEGIN
			DECLARE @strSTaxVal varchar(50)
			SET @strSTaxVal = Convert(varchar(50),@STaxVal)
			EXEC log2_insert @UpdatedBy,'Estimate',@EstimateNo,'Sales Tax Value','',@strSTaxVal
		END
		-- Commission
		IF(@CommissionVal is not null And @CommissionVal != 0)
		BEGIN
			DECLARE @strCommissionVal varchar(50)
			SET @strCommissionVal = Convert(varchar(50),@CommissionVal)
			EXEC log2_insert @UpdatedBy,'Estimate',@EstimateNo,'Commission','',@strCommissionVal
		END
		-- Commission Percent
		IF(@CommissionPer is not null And @CommissionPer != 0)
		BEGIN
			DECLARE @strCommissionPer varchar(50)
			SET @strCommissionPer = Convert(varchar(50),@CommissionPer)
			EXEC log2_insert @UpdatedBy,'Estimate',@EstimateNo,'Commission Percentage','',@strCommissionPer
		END
		-- Total Hour
		-- Total Price
		-- Remarks
		IF(@remarks is not null And @remarks != '')
		BEGIN
			EXEC log2_insert @UpdatedBy,'Estimate',@EstimateNo,'Remarks','',@remarks
		END

		IF(@EstimateType is not null And @EstimateType != '')
		BEGIN
			EXEC log2_insert @UpdatedBy,'Estimate',@EstimateNo,'Estimate Type','',@EstimateType
		END
		
		Declare @SglBilAmt varchar(10) = CASE WHEN ISNULL(@IsSglBilAmt,0) = 0 THEN 'No' ELSE 'Yes' END
		EXEC log2_insert @UpdatedBy,'Estimate',@EstimateNo,'Single Billing Amount','',@SglBilAmt
		 /********End Logs************/


	
		COMMIT 
	END TRY
	BEGIN CATCH
		--SELECT ERROR_MESSAGE()
		--IF @@TRANCOUNT>0 ROLLBACK	
		--RAISERROR ('An error has occurred on this page.',16,1)
		--RETURN
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
		RETURN
	END CATCH
	EXEC CalculateInventory
	SELECT @EstimateNo	

END
