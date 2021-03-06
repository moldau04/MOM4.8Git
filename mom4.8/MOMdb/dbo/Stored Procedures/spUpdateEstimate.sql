/*--------------------------------------------------------------------
Modified By: Thomas
Modified On: 26 Feb 2019
Desc: For add estimate's equipments

Modified By: Thurstan
Modified On: 30 Nov 2018	
Description: Add Orderno column 

Modified By: Thurstan
Modified On: 13 Dec 2018	
Description: Change length of EstimateCell to 28
--------------------------------------------------------------------*/
CREATE PROCEDURE  [dbo].[spUpdateEstimate]
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
	@EstimateNo int,
	@Jobtype varchar(max),
	@SalesManUerId int,
	@bidDate datetime,
	@billAddress nvarchar(300),
	@phone varchar(28),
	@fax varchar(28),
	@Email varchar(100),
	@EstimateAddress nvarchar (255),
	@EstimateCell nvarchar (28),
	@BidPrice numeric(30,2),
	@Override numeric(30,2),
	@Cont numeric(30,2),
	@OH numeric(30,2),
	@SalesTax VARCHAR(25),
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
	--@GroupName  varchar(255),
	@GroupId int,
	@Opportunity int,
	@GridUserSettings tblTypeGridUserSettings readonly,
	@UpdatedBy varchar(100),
	@CustomItems AS tblTypeCommonCustomItem readonly,
	@EstimateType VARCHAR(50),
	@IsSglBilAmt bit,
	@OpportunityStageID VARCHAR(50),
	@OpportunityName VARCHAR(255)
AS
BEGIN
	
	SET NOCOUNT ON;

    		Declare @RolType smallint 
			DECLARE @jobtypeid INT

			DECLARE @JfDesc VARCHAR(255)
			DECLARE @JCode VARCHAR(10)
			DECLARE @Line INT
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
			DECLARE @JLine SMALLINT = 0 
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
			DECLARE @OrderNo INT
			DECLARE @Screen Varchar(50) = 'Estimate'
			DECLARE @MPrice NUMERIC(30,2)
			DECLARE @MQuantity NUMERIC(30,2)
			DECLARE @MChangeOrder Tinyint
			

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

			--SELECT TOP 1 @userId = UserId, @pageName = PageName FROM @GridUserSettings
	
			SELECT @RolType = Type FROM   Rol WHERE  ID = @rol
			IF (@RolType = 3 )
			BEGIN
				SET @loc= 0
			END

			SET @jobtypeid=CAST(@Jobtype AS INT)--(SELECT JobT.ID FROM JobT WHERE JobT.Type=CAST(@Jobtype AS INT))
	
			DECLARE @CurrEstOpprt int = 0
			SELECT @CurrEstOpprt = e.Opportunity 
			FROM Estimate e 
			INNER JOIN Lead ld ON ld.ID = e.Opportunity
			WHERE e.ID = @EstimateNo


			UPDATE Estimate
			SET  
							
				Name = @name,fdesc=@fdesc,CompanyName=@CompanyName,
				Remarks=@remarks,RolID=@rol,LocID=@loc,
				fFor=(case @RolType when 3 then 'PROSPECT' else 'ACCOUNT' end),CADExchange=@CADExchange,
				Contact= @Contact ,estimatedate=@estDate  ,Estimate.EstimateBillAddress=@billAddress,Estimate.BDate=@bidDate,
				Estimate.Phone=@phone,Estimate.Fax=@fax,Estimate.EstimateUserId=@SalesManUerId,Estimate.EstimateAddress=@EstimateAddress,Estimate.EstimateEmail=@Email,
				Status=@Status,EstimateCell=@EstimateCell,
				Cont = @Cont, Price = @BidPrice, Quoted = @Override, Overhead = @OH,OHPer=@OHPer,MarkupPer=@MarkupPer,CommissionPer=@CommissionPer,CommissionVal=@CommissionVal,
				STaxName=@SalesTax,STaxRate=@STaxRate,Category=@Category,
				MarkupVal=@MarkupVal,STaxVal=@STaxVal,MatExp=@MatExp,LabExp=@LabExp,OtherExp=@OtherExp,SubToalVal=@SubToalVal,TotalCostVal=@TotalCostVal,PretaxTotalVal=@PretaxTotalVal,
				PType=@PType,Amount=@Amount,BillRate=@BillRate,OT=@OT,RateTravel=@RateTravel,DT=@DT,RateMileage=@RateMileage,RateNT=@RateNT,ContPer=@ContPer
				, Template = @template
				, Discounted = @Discounted
				, DiscountedNotes = @DiscountedNotes
				--, GroupName = @GroupName
				, GroupId = @GroupId
				, Opportunity = @Opportunity
				, EstimateType = @EstimateType
				, IsSglBilAmt = ISNULL(@IsSglBilAmt,0)
			WHERE  ID = @EstimateNo 

			
			DECLARE @CurrEstOpprtStatus varchar(50) = ''
			DECLARE @EstOpprtStatus varchar(50) = ''
			IF @CurrEstOpprt != @Opportunity
			BEGIN
				
				SELECT @CurrEstOpprtStatus = oe.Name
				FROM Lead ld INNER JOIN OEStatus oe ON oe.ID = ld.Status WHERE ld.ID = @CurrEstOpprt
				-- Update the status for the old opportunity
				IF NOT EXISTS (SELECT TOP 1 1
					from lead ld
					inner join Estimate e on e.Opportunity = ld.ID 
					where ld.id = @CurrEstOpprt
					and e.Opportunity is not null)
				BEGIN
					UPDATE LEAD SET Status = 1 WHERE ID = @CurrEstOpprt
				END
				ELSE IF EXISTS (
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
				ELSE IF EXISTS (
					SELECT TOP 1 1
					from lead ld
					inner join Estimate e on e.Opportunity = ld.ID 
					where e.Status = 1 
					and ld.id = @CurrEstOpprt
					and e.Opportunity is not null
				)
				-- There is no Sold estimate link to this opportunity but have an open estimate link to this opportunity. Set Opprt status to 'Quoted'
				BEGIN
					UPDATE LEAD SET Status = 7 WHERE ID = @CurrEstOpprt
				END
				
				SELECT @EstOpprtStatus = oe.Name
				FROM Lead ld INNER JOIN OEStatus oe ON oe.ID = ld.Status WHERE ld.ID = @CurrEstOpprt
				IF(@EstOpprtStatus != @CurrEstOpprtStatus)
					EXEC log2_insert @UpdatedBy,'Opportunity',@CurrEstOpprt,'Status',@CurrEstOpprtStatus,@EstOpprtStatus


				SET @CurrEstOpprtStatus = ''
				SET @EstOpprtStatus = ''

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
				SELECT @EstOpprtStatus = oe.Name
				FROM Lead ld INNER JOIN OEStatus oe ON oe.ID = ld.Status WHERE ld.ID = @Opportunity

				IF(@EstOpprtStatus != @CurrEstOpprtStatus)
					EXEC log2_insert @UpdatedBy,'Opportunity',@Opportunity,'Status',@CurrEstOpprtStatus,@EstOpprtStatus
			END
			ELSE
			BEGIN
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
				SELECT @EstOpprtStatus = oe.Name
				FROM Lead ld INNER JOIN OEStatus oe ON oe.ID = ld.Status WHERE ld.ID = @Opportunity

				IF(@EstOpprtStatus != @CurrEstOpprtStatus)
					EXEC log2_insert @UpdatedBy,'Opportunity',@Opportunity,'Status',@CurrEstOpprtStatus,@EstOpprtStatus
			END

			IF (@RolType = 3)
			BEGIN
				UPDATE Prospect SET STax = @Sales_Tax WHERE Rol = @rol
			END

			

			-- Revert Inventory Item Trans before removing
			If NOT EXISTS (  select 1 from tblEstimateConvertToProject ei where ei.EstimateID=@EstimateNo )
			BEGIN
				DECLARE @INV_WarehouseID varchar(50) = 'OFC';
				INSERT INTO tblInventoryWHTrans(InvID, WarehouseID,LocationID ,Hand,Balance,fOrder,[Committed],[Available],Screen,ScreenID,Mode,Date,TransType,Batch,fDate)
				SELECT b.MatItem, 'OFC',0,0,0,0, ISNULL(ei.Quan,0)*-1,0,@Screen,@EstimateNo,'Edit',GETDATE(),'Revert',0,GETDATE() FROM BOM b
					INNER JOIN EstimateI ei ON ei.ID = b.EstimateIId
					INNER JOIN Inv inv ON inv.ID = b.MatItem
					WHERE ei.Estimate = @EstimateNo AND ISNULL(ei.Quan,0) <> 0--and b.Type= 8
		    END

						
			DELETE BOM FROM BOM INNER JOIN EstimateI ON EstimateI.ID = BOM.EstimateIId
					WHERE EstimateI.Estimate = @EstimateNo 

			DELETE Milestone FROM Milestone INNER JOIN EstimateI ON EstimateI.ID = Milestone.EstimateIId
					WHERE EstimateI.Estimate = @EstimateNo  
			
			DELETE EstimateI WHERE EstimateI.Estimate = @EstimateNo

			--CREATE TABLE #tblEstimateItems
			--(ItemID INT)

			--INSERT INTO #tblEstimateItems
			--SELECT EstimateI.ID FROM EstimateI 
			--	WHERE EstimateI.Estimate = @EstimateNo
			-- to delete only those estimate which are linked with bom and milestone
			--DELETE FROM EstimateI WHERE ID IN (SELECT ItemID FROM #tblEstimateItems)
										
			--DROP TABLE #tblEstimateItems

			------------------------ BEGIN INSERT BOM ITEMS (UPDATE ESTIMATE) ------------------------

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
					SET @EstimateItemID = (SELECT MAX(ISNULL(ID,0))+1 FROM EstimateI)

					--SET IDENTITY_INSERT EstimateI ON          
					
					INSERT INTO EstimateI
						(	Estimate, Line, fDesc, 
							Quan, Cost, Price, Hours, 
							Rate, Labor, Amount, STax, 
							Code, Vendor, Currency, Type, 
							MMU, MMUAmt, LMU, LMUAmt, 
							LStax, LMod, MMod, OrderNO
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

					
					-- Inventory
					IF EXISTS (SELECT 1 FROM Inv Where Type = 0 AND ID =@MatItem)
					BEGIN

						If NOT EXISTS (  select 1 from tblEstimateConvertToProject ei where ei.EstimateID=@EstimateNo )
			
							BEGIN

							INSERT INTO tblInventoryWHTrans(InvID, WarehouseID,LocationID ,Hand,Balance,fOrder,[Committed],[Available],Screen,ScreenID,Mode,Date,TransType,Batch,FDate)
							VALUES (@MatItem,@INV_WarehouseID,0,0,0,0,@QtyReq,0,@Screen,@EstimateNo,'Edit',GETDATE(),'In',0,GETDATE())

						END
					END
			 
					FETCH NEXT FROM db_cursor_BOM INTO

						@JfDesc, @JCode, @JLine, @BType,
						@QtyReq, @UM, @BudgetUnit, @BudgetExt,
						@MatMod, @MatPrice, @MatMarkup, @STax,
						@Currency, @MatItem, @VendorId,
						@LabItem, @LabMod, @LabExt, @LabRate, 
						@LabHours, @SDate, @TotalExt, @LabPrice, 
						@LabMarkup, @LSTax,@OrderNo
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
			------------------------ END INSERT BOM ITEMS (UPDATE ESTIMATE) ------------------------

				
			------------------------ BEGIN INSERT MILESTONE ITEMS (UPDATE ESTIMATE) ------------------------

			SET @JLine = 0
			SET @OrderNo = 0
			DECLARE @CountMilestonItem int;
			DECLARE @CountCursorMileston int = 0;
			SET @CountMilestonItem = ISNULL((SELECT Count(*) FROM @MilestonItem),0);
			IF(@CountMilestonItem > 0)
			BEGIN
				DECLARE db_cursor_Mileston CURSOR FOR 
				SELECT fDesc, JCode, Line, MilesName, RequiredBy, Type, Amount,AmountPer, OrderNo, Quantity, Price, ChangeOrder
				FROM @MilestonItem
				OPEN db_cursor_Mileston  
				FETCH NEXT FROM db_cursor_Mileston INTO @JfDesc, @JCode, @JLine, @MilestoneName, @RequiredBy, 
					@MilestoneType, @BRev,@AmountPer,@OrderNo, @MQuantity, @MPrice, @MChangeOrder
				WHILE @@FETCH_STATUS = 0																																										WHILE @@FETCH_STATUS = 0
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
						(@EstimateNo, @JLine, @JfDesc, @JCode, 0, @BRev,@AmountPer,@OrderNo, @MQuantity, @MPrice)
					SET @EstimateItemID = SCOPE_IDENTITY()

					--SET IDENTITY_INSERT EstimateI OFF    
					-- EstimateI.Type = 0 is revenue type

					INSERT INTO [dbo].[Milestone]
						([EstimateIId], [MilestoneName], [RequiredBy], [CreationDate], [ProjAcquistDate], [Type], [Amount], Quantity, Price, ChangeOrder)
					VALUES
				  		(@EstimateItemID, @MilestoneName, @RequiredBy, GETDATE (), NULL, @MilestoneType, @BRev, @MQuantity, @MPrice, @MChangeOrder)
				

					FETCH NEXT FROM db_cursor_Mileston INTO 
						@JfDesc, @JCode, @JLine, @MilestoneName, @RequiredBy, @MilestoneType, @BRev,@AmountPer, @OrderNo, @MQuantity, @MPrice, @MChangeOrder
		
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
			------------------------ END INSERT MILESTONE ITEMS (UPDATE ESTIMATE) ------------------------


			------------------------ BEGIN INSERT EQUIPMENT ITEMS (INSERT ESTIMATE) ------------------------

			------------ DELETE ALL THE OLD EQUIPMENTS OF ESTIMATE -----------------

			--DELETE EstimateGroupEquipments WHERE EstimateID = @EstimateNo

			--INSERT INTO EstimateGroupEquipments (EstimateID, GroupName, EquipmentID) SELECT @EstimateNo, @GroupName, EquipmentID  FROM @EquipItem

			/*
			* Check and insert Estimate group
			* This code need to be checked again and replaced by another one
			* Just a quick adding for Estimate Group and the related information for NK to continue his task
			* Thomas
			*/
			-- Start
			-- Delete all the old equipment the group
			DELETE tblEstimateGroupEquipment WHERE GroupId = @GroupId
			-- And replace it by the new one
			INSERT INTO tblEstimateGroupEquipment (GroupId, EquipmentID) SELECT @GroupId, EquipmentID  FROM @EquipItem

			-- Clear all equipment on the group that have no linked to any estimate
			--IF @GroupId = 0
			--BEGIN
			--	DELETE tblEstimateGroupEquipment WHERE GroupId 
			--		in (SELECT eg.Id from tblEstimateGroup eg left join  Estimate e   on e.GroupId = eg.Id where ISNULL(e.GroupId,0) = 0)
			--END
			-- End

			--DECLARE db_cursor2 CURSOR FOR SELECT EquipmentID, GroupName FROM @EquipItem
			--OPEN db_cursor2
			--FETCH NEXT FROM db_cursor2 INTO @EquipmentID, @GroupName
			--BEGIN  		
			--	INSERT INTO EstimateGroupEquipments
			--		(EstimateID, GroupName, EquipmentID)	
			--	VALUES
			--		(@EstimateNo, @GroupName, @EquipmentID)

			--	SET @GroupName = null;
			--	SET @EquipmentID = null;

			--FETCH NEXT FROM db_cursor2 INTO @EquipmentID, @GroupName
			--END  

			--CLOSE db_cursor2
			--DEALLOCATE db_cursor2

			------------------------ END INSERT EQUIPMENT ITEMS (INSERT ESTIMATE) ------------------------

					
			--UPDATE PHONE SET EmailRecQuote=1 WHERE PHONE.ID=@Contact
			
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

            --delete from tblCustomJob where JobID = @job            
            -- update custom details for project    
			DECLARE @tblCustomFieldsId int
			DECLARE @Value varchar(255)
			DECLARE @tblTabID int
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

            -- Delete the old value if existed
            DELETE FROM [tblCommonCustomFieldsValue]
            WHERE Ref = @EstimateNo
				AND Screen = @Screen
                AND ([tblCommonCustomFieldsID] NOT IN (SELECT [ID] FROM @CustomItems))

            /********/



			------------------------ Insert/Update Grid user settings (INSERT ESTIMATE) ------------------------
			--SELECT 1 FROM tblUserGridSettings gus INNER JOIN @GridUserSettings dd ON gus.GridId = dd.GridId AND gus.PageName = dd.PageName AND gus.UserId = dd.UserId
			
			--WHERE UserId = @userId AND PageName = @pageName AND GridId = @gridId
			UPDATE tblUserGridSettings SET ColumnsSettings = dd.ColumnSettings 
				FROM tblUserGridSettings gus 
					INNER JOIN @GridUserSettings dd 
					ON gus.GridId = dd.GridId AND gus.PageName = dd.PageName AND gus.UserId = dd.UserId

			INSERT INTO tblUserGridSettings (UserId,PageName,GridId,ColumnsSettings)
				SELECT dd.UserId, dd.PageName, dd.GridId, dd.ColumnSettings 
				FROM tblUserGridSettings gus RIGHT JOIN @GridUserSettings dd 
				ON gus.GridId = dd.GridId AND gus.PageName = dd.PageName AND gus.UserId = dd.UserId 
				WHERE gus.UserId is null AND gus.PageName is null AND gus.GridId is null

   --       	IF EXISTS(SELECT 1 FROM tblUserGridSettings WHERE UserId = @userId AND PageName = @pageName AND GridId = @gridId)
			--BEGIN
			--	UPDATE tblUserGridSettings SET ColumnsSettings = @columSettings WHERE UserId = @userId AND PageName = @pageName AND GridId = @gridId
			--END
			--ELSE 
			--BEGIN
			--	INSERT INTO tblUserGridSettings (UserId,PageName,GridId,ColumnsSettings) VALUES (@userId,@pageName,@gridId, @columSettings)
			--END

			/********Start Logs************/
			/**
			DECLARE @Val VARCHAR(1000)    
			IF(@VendorId is not null And @VendorId != 0)    
			BEGIN      
				Set @Val =(select Top 1 newVal  from log2 where screen='PO' and ref= @PO and Field='Vendor' order by CreatedStamp desc )    
				Declare @VendorName varchar(150)    
				Select @VendorName = r.Name FROM Rol r INNER JOIN Vendor V ON V.Rol = r.ID WHERE V.ID  = @VendorId    
				if(@Val<>@VendorName)    
				begin    
					exec log2_insert @UpdatedBy,'PO',@PO,'Vendor',@Val,@VendorName    
				end    
				Else IF (@CurrentVendor <> @VendorName)    
				Begin    
					exec log2_insert @UpdatedBy,'PO',@PO,'Vendor',@CurrentVendor,@VendorName    
				END    
			end    
			set @Val=null   
			*/
			-- Estimate date
			DECLARE @PrevVal VARCHAR(1000)   
			--IF(@estDate is not null And @estDate != '')
			--BEGIN 	
			--	DECLARE @estDateStr nvarchar(150)
			--	SELECT @estDateStr = convert(varchar, @estDate, 101)
			--	EXEC log2_insert @UpdatedBy,@Screen,@EstimateNo,'Estimate Date','',@estDateStr
			--END
			--SET @Val=null
			-- Bid Close Date
			--IF(@bidDate is not null And @bidDate != '')
			--BEGIN 	
			--	DECLARE @bidDateStr nvarchar(150)
			--	SELECT @bidDateStr = convert(varchar, @bidDate, 101)
			--	EXEC log2_insert @UpdatedBy,@Screen,@EstimateNo,'Bid Close Date','',@bidDateStr
			--END
			--SET @PrevVal=null

			-- Description
			SELECT TOP 1 @PrevVal= newVal  FROM log2 WHERE screen=@Screen AND ref= @EstimateNo and Field='Description' ORDER BY CreatedStamp DESC
			IF(@fdesc is not null AND @PrevVal is not null AND @fdesc != @PrevVal)
			BEGIN 	
				EXEC log2_insert @UpdatedBy,@Screen,@EstimateNo,'Description',@PrevVal,@fdesc
			END
			SET @PrevVal=null
			
			-- Category
			SELECT TOP 1 @PrevVal= newVal  FROM log2 WHERE screen=@Screen AND ref= @EstimateNo and Field='Category' ORDER BY CreatedStamp DESC
			IF(@Category is not null AND @PrevVal is not null AND @Category != @PrevVal)
			BEGIN 	
				EXEC log2_insert @UpdatedBy,@Screen,@EstimateNo,'Category',@PrevVal,@Category
			END
			SET @PrevVal=null

			-- Estimate Status
			SELECT TOP 1 @PrevVal= newVal  FROM log2 WHERE screen=@Screen AND ref= @EstimateNo and Field='Estimate Status' ORDER BY CreatedStamp DESC
			IF(@Status is not null)
			BEGIN
				DECLARE @StatusName varchar(50)
				SELECT TOP 1 @StatusName=Name FROM OEStatus WHERE ID = @Status
				IF(@StatusName is not null And @StatusName != @PrevVal)
				BEGIN
					EXEC log2_insert @UpdatedBy,@Screen,@EstimateNo,'Estimate Status',@PrevVal,@StatusName
				END
			END
			SET @PrevVal = null

			-- Assigned To
			SELECT TOP 1 @PrevVal= newVal  FROM log2 WHERE screen=@Screen AND ref= @EstimateNo and Field='Assigned To' ORDER BY CreatedStamp DESC
			IF(@SalesManUerId is not null)
			BEGIN
				DECLARE @Assigned varchar(50)
				SELECT TOP 1 @Assigned = t.SDesc from terr t where ID = @SalesManUerId
				IF(@Assigned is not null And @Assigned != @PrevVal)
				EXEC log2_insert @UpdatedBy,@Screen,@EstimateNo,'Assigned To',@PrevVal,@Assigned
			END
			SET @PrevVal = null

			-- Customer Name
			SELECT TOP 1 @PrevVal= newVal  FROM log2 WHERE screen=@Screen AND ref= @EstimateNo and Field='Customer Name' ORDER BY CreatedStamp DESC
			IF(@CompanyName is not null And @CompanyName != @PrevVal)
			BEGIN
				EXEC log2_insert @UpdatedBy,@Screen,@EstimateNo,'Customer Name',@PrevVal,@CompanyName
			END
			SET @PrevVal = null

			-- Location Name
			SELECT TOP 1 @PrevVal= newVal  FROM log2 WHERE screen=@Screen AND ref= @EstimateNo and Field='Location Name' ORDER BY CreatedStamp DESC
			IF(@billAddress is not null And @billAddress != @PrevVal)
			BEGIN
				EXEC log2_insert @UpdatedBy,@Screen,@EstimateNo,'Location Name',@PrevVal,@billAddress
			END
			SET @PrevVal = null

			-- Contact Name
			
			--IF(@Contact is not null And @Contact != '')
			--BEGIN
			--	DECLARE @strContactName varchar(255)
			--	IF(@Contact = '0')
			--	BEGIN
			--		SELECT @strContactName = contact FROM Rol WHERE ID = @rol AND Contact is not null AND Contact != ''
			--	END
			--	ELSE
			--	BEGIN
			--		SELECT TOP 1 @strContactName=fDesc FROM PHONE WHERE PHONE.ROL=@rol AND ID=@Contact
			--	END
			
			--	EXEC Log2_Insert_new @UpdatedBy,@Screen,@EstimateNo,'Contact Name',@strContactName
			--END
			EXEC log2_insert_new @UpdatedBy,@Screen,@EstimateNo,'Contact Name',@Contact
			-- Email
			EXEC log2_insert_new @UpdatedBy,@Screen,@EstimateNo,'Email',@Email

			-- Phone
			EXEC log2_insert_new @UpdatedBy,@Screen,@EstimateNo,'Phone',@phone

			-- Cell/Mobile
			EXEC log2_insert_new @UpdatedBy,@Screen,@EstimateNo,'Cell/Mobile',@EstimateCell
			
			-- Fax
			EXEC log2_insert_new @UpdatedBy,@Screen,@EstimateNo,'Fax',@fax
			
			-- Discounted
			SELECT TOP 1 @PrevVal= newVal  FROM log2 WHERE screen=@Screen AND ref= @EstimateNo and Field='Discounted' ORDER BY CreatedStamp DESC
			DECLARE @strDiscounted varchar(1)
			SET @strDiscounted = Convert(Varchar(1),@Discounted)
			IF (@strDiscounted != @PrevVal)
			BEGIN
				EXEC log2_insert @UpdatedBy,@Screen,@EstimateNo,'Discounted',@PrevVal,@strDiscounted
			END
			SET @PrevVal = null

			-- Discounted Notes
			SELECT TOP 1 @PrevVal= newVal  FROM log2 WHERE screen=@Screen AND ref= @EstimateNo and Field='Discounted Notes' ORDER BY CreatedStamp DESC
			IF(@DiscountedNotes is not null And @DiscountedNotes != @PrevVal)
			BEGIN
				EXEC log2_insert @UpdatedBy,@Screen,@EstimateNo,'Discounted Notes',@PrevVal,@DiscountedNotes
			END
			SET @PrevVal = null

			-- Template
			SELECT TOP 1 @PrevVal= newVal  FROM log2 WHERE screen=@Screen AND ref= @EstimateNo and Field='Template' ORDER BY CreatedStamp DESC
			IF(@template is not null)
			BEGIN
				DECLARE @templateName VARCHAR(50) = ''
				SELECT TOP 1 @templateName = t.fDesc from JobT t where ID = @template
				IF(@templateName != @PrevVal)
				EXEC log2_insert @UpdatedBy,@Screen,@EstimateNo,'Template',@PrevVal,@templateName
			END
			SET @PrevVal = null

			-- Department
		
			-- Opportunity #
			SELECT TOP 1 @PrevVal= newVal  FROM log2 WHERE screen=@Screen AND ref= @EstimateNo and Field='Opportunity #' ORDER BY CreatedStamp DESC
			IF(@PrevVal is not null)
			BEGIN
				IF(Convert(int, @PrevVal) != @Opportunity)
				BEGIN
					EXEC log2_insert @UpdatedBy,@Screen,@EstimateNo,'Opportunity #',@PrevVal,@Opportunity
				END
			END
			ELSE
			BEGIN
				EXEC log2_insert @UpdatedBy,@Screen,@EstimateNo,'Opportunity #','',@Opportunity
			END
			SET @PrevVal = null

			-- Opportunity Name
			-- Opportunity Stage
			-- Group Name
			DECLARE @GroupName VARCHAR(1000)
			SELECT TOP 1 @GroupName = GroupName from tblEstimateGroup WHERE Id = @GroupId
			EXEC Log2_Insert_new @UpdatedBy,@Screen,@EstimateNo,'Group Name',@GroupName

			-- Equipment
			-- Bid Price
			DECLARE @strBidPrice varchar(50)
			SELECT TOP 1 @PrevVal= newVal  FROM log2 WHERE screen=@Screen AND ref= @EstimateNo and Field='Bid Price' ORDER BY CreatedStamp DESC
			IF(@PrevVal is not null AND @PrevVal != '')
			BEGIN
				SET @strBidPrice = Convert(varchar(50),@BidPrice)
				IF(@strBidPrice != @PrevVal)
				BEGIN
					EXEC log2_insert @UpdatedBy,@Screen,@EstimateNo,'Bid Price',@PrevVal,@strBidPrice
				END
			END
			ELSE
			BEGIN
				IF(@BidPrice is not null)
				BEGIN
					SET @strBidPrice = Convert(varchar(50),@BidPrice)
					EXEC log2_insert @UpdatedBy,@Screen,@EstimateNo,'Bid Price','',@strBidPrice
				END
			END
			SET @PrevVal = null

			-- Final Bid Price
			DECLARE @strFinalBidPrice varchar(50)
			SELECT TOP 1 @PrevVal= newVal  FROM log2 WHERE screen=@Screen AND ref= @EstimateNo and Field='Final Bid Price' ORDER BY CreatedStamp DESC
			IF(@PrevVal is not null AND @PrevVal != '')
			BEGIN
				SET @strFinalBidPrice = Convert(varchar(50),@Override)
				IF(@strFinalBidPrice != @PrevVal)
				BEGIN
					EXEC log2_insert @UpdatedBy,@Screen,@EstimateNo,'Final Bid Price',@PrevVal,@strFinalBidPrice
				END
			END
			ELSE
			BEGIN
				IF(@Override is not null)
				BEGIN
					SET @strFinalBidPrice = Convert(varchar(50),@Override)
					EXEC log2_insert @UpdatedBy,@Screen,@EstimateNo,'Final Bid Price','',@strFinalBidPrice
				END
			END
			SET @PrevVal = null

			-- Material Exp
			DECLARE @strMatExp varchar(50)
			SELECT TOP 1 @PrevVal= newVal  FROM log2 WHERE screen=@Screen AND ref= @EstimateNo and Field='Material Exp' ORDER BY CreatedStamp DESC
			IF(@PrevVal is not null AND @PrevVal != '')
			BEGIN
				SET @strMatExp = Convert(varchar(50),@MatExp)
				IF(@strMatExp != @PrevVal)
				BEGIN
					EXEC log2_insert @UpdatedBy,@Screen,@EstimateNo,'Material Exp',@PrevVal,@strMatExp
				END
			END
			ELSE
			BEGIN
				IF(@MatExp is not null)
				BEGIN
					SET @strMatExp = Convert(varchar(50),@MatExp)
					EXEC log2_insert @UpdatedBy,@Screen,@EstimateNo,'Material Exp','',@strMatExp
				END
			END
			SET @PrevVal = null

			-- Labor Exp
			DECLARE @strLabExp varchar(50)
			SELECT TOP 1 @PrevVal= newVal  FROM log2 WHERE screen=@Screen AND ref= @EstimateNo and Field='Labor Exp' ORDER BY CreatedStamp DESC
			IF(@PrevVal is not null AND @PrevVal != '')
			BEGIN
				SET @strLabExp = Convert(varchar(50),@LabExp)
				IF(@strLabExp != @PrevVal)
				BEGIN
					
					EXEC log2_insert @UpdatedBy,@Screen,@EstimateNo,'Labor Exp',@PrevVal,@strLabExp
				END
			END
			ELSE
			BEGIN
				IF(@LabExp is not null)
				BEGIN
					SET @strLabExp = Convert(varchar(50),@LabExp)
					EXEC log2_insert @UpdatedBy,@Screen,@EstimateNo,'Labor Exp','',@strLabExp
				END
			END
			SET @PrevVal = null

			-- Other Exp
			DECLARE @strOtherExp varchar(50)
			SELECT TOP 1 @PrevVal= newVal  FROM log2 WHERE screen=@Screen AND ref= @EstimateNo and Field='Other Exp' ORDER BY CreatedStamp DESC
			IF(@PrevVal is not null AND @PrevVal != '')
			BEGIN
				SET @strOtherExp = Convert(varchar(50),@OtherExp)
				IF(@strOtherExp != @PrevVal)
				BEGIN
					EXEC log2_insert @UpdatedBy,@Screen,@EstimateNo,'Other Exp',@PrevVal,@strOtherExp
				END
			END
			ELSE
			BEGIN
				IF(@OtherExp is not null)
				BEGIN
					SET @strOtherExp = Convert(varchar(50),@OtherExp)
					EXEC log2_insert @UpdatedBy,@Screen,@EstimateNo,'Other Exp','',@strOtherExp
				END
			END
			SET @PrevVal = null

			-- Contingencies
			DECLARE @strContingencies varchar(50)
			SELECT TOP 1 @PrevVal= newVal  FROM log2 WHERE screen=@Screen AND ref= @EstimateNo and Field='Contingencies' ORDER BY CreatedStamp DESC
			IF(@PrevVal is not null AND @PrevVal != '')
			BEGIN
				SET @strContingencies = Convert(varchar(50),@Cont)
				IF(@strContingencies != @PrevVal)
				BEGIN
					EXEC log2_insert @UpdatedBy,@Screen,@EstimateNo,'Contingencies',@PrevVal,@strContingencies
				END
			END
			ELSE
			BEGIN
				IF(@Cont is not null)
				BEGIN
					SET @strContingencies = Convert(varchar(50),@Cont)
					EXEC log2_insert @UpdatedBy,@Screen,@EstimateNo,'Contingencies','',@strContingencies
				END
			END
			SET @PrevVal = null

			-- Contingencies Percent
			DECLARE @strContPercent varchar(50)
			SET @strContPercent = Convert(varchar(50),@ContPer)
			EXEC Log2_Insert_new @UpdatedBy,@Screen,@EstimateNo,'Contingencies Percentage',@strContPercent

			-- Subtotal	
			DECLARE @strSubTotal varchar(50)
			SELECT TOP 1 @PrevVal= newVal  FROM log2 WHERE screen=@Screen AND ref= @EstimateNo and Field='Subtotal' ORDER BY CreatedStamp DESC
			IF(@PrevVal is not null AND @PrevVal != '')
			BEGIN
				SET @strSubTotal = Convert(varchar(50),@SubToalVal)
				IF(@strSubTotal != @PrevVal)
				BEGIN
					EXEC log2_insert @UpdatedBy,@Screen,@EstimateNo,'Subtotal',@PrevVal,@strSubTotal
				END
			END
			ELSE
			BEGIN
				IF(@SubToalVal is not null)
				BEGIN
					SET @strSubTotal = Convert(varchar(50),@SubToalVal)
					EXEC log2_insert @UpdatedBy,@Screen,@EstimateNo,'Subtotal','',@strSubTotal
				END
			END
			SET @PrevVal = null

			-- Overhead
			DECLARE @strOH varchar(50)
			SELECT TOP 1 @PrevVal= newVal  FROM log2 WHERE screen=@Screen AND ref= @EstimateNo and Field='Overhead' ORDER BY CreatedStamp DESC
			IF(@PrevVal is not null AND @PrevVal != '')
			BEGIN
				SET @strOH = Convert(varchar(50),@OH)
				IF(@strOH != @PrevVal)
				BEGIN
					EXEC log2_insert @UpdatedBy,@Screen,@EstimateNo,'Overhead',@PrevVal,@strOH
				END
			END
			ELSE
			BEGIN
				IF(@OH is not null)
				BEGIN
					SET @strOH = Convert(varchar(50),@OH)
					EXEC log2_insert @UpdatedBy,@Screen,@EstimateNo,'Overhead','',@strOH
				END
			END
			SET @PrevVal = null

			-- Overhead Percent
			DECLARE @strOHPer varchar(50)
			SET @strOHPer = Convert(varchar(50),@OHPer)
			EXEC Log2_Insert_new @UpdatedBy,@Screen,@EstimateNo,'Overhead Percentage',@strOHPer

			-- Total Cost
			DECLARE @strTotalCost varchar(50)
			SET @strTotalCost = Convert(varchar(50),@TotalCostVal)
			EXEC Log2_Insert_new @UpdatedBy,@Screen,@EstimateNo,'Total Cost',@strTotalCost

			-- Profit
			DECLARE @strProfit varchar(50)
			SET @strProfit = Convert(varchar(50),@MarkupVal)
			EXEC Log2_Insert_new @UpdatedBy,@Screen,@EstimateNo,'Profit',@strProfit

			-- Profit Percent
			DECLARE @strProfitPer varchar(50)
			SET @strProfitPer = Convert(varchar(50),@MarkupPer)
			EXEC Log2_Insert_new @UpdatedBy,@Screen,@EstimateNo,'Profit Percentage',@strProfitPer

			-- Pretax total
			DECLARE @strPretaxTotal varchar(50)
			SET @strPretaxTotal = Convert(varchar(50),@PretaxTotalVal)
			EXEC Log2_Insert_new @UpdatedBy,@Screen,@EstimateNo,'Pretax total',@strPretaxTotal

			-- Sales Tax
			EXEC Log2_Insert_new @UpdatedBy,@Screen,@EstimateNo,'Sales Tax',@Sales_Tax

			-- Sales Tax Rate
			DECLARE @strSTaxRate varchar(50)
			SET @strSTaxRate = Convert(varchar(50),@STaxRate)
			EXEC Log2_Insert_new @UpdatedBy,@Screen,@EstimateNo,'Sales Tax Rate',@strSTaxRate

			-- Sales Tax Value
			DECLARE @strSTaxVal varchar(50)
			SET @strSTaxVal = Convert(varchar(50),@STaxVal)
			EXEC Log2_Insert_new @UpdatedBy,@Screen,@EstimateNo,'Sales Tax Value',@strSTaxVal

			-- Commission
			DECLARE @strCommissionVal varchar(50)
			SET @strCommissionVal = Convert(varchar(50),@CommissionVal)
			EXEC Log2_Insert_new @UpdatedBy,@Screen,@EstimateNo,'Commission',@strCommissionVal

			-- Commission Percent
			DECLARE @strCommissionPer varchar(50)
			SET @strCommissionPer = Convert(varchar(50),@CommissionPer)
			EXEC Log2_Insert_new @UpdatedBy,@Screen,@EstimateNo,'Commission Percentage',@strCommissionPer

			-- Total Hour
			-- Total Price
			-- Remarks
			EXEC Log2_Insert_new @UpdatedBy,@Screen,@EstimateNo,'Remarks',@remarks

			EXEC Log2_Insert_new @UpdatedBy,@Screen,@EstimateNo,'Estimate Type',@EstimateType

			Declare @SglBilAmt varchar(10) = CASE WHEN ISNULL(@IsSglBilAmt,0) = 0 THEN 'No' ELSE 'Yes' END
			EXEC Log2_Insert_new @UpdatedBy,@Screen,@EstimateNo,'Single Billing Amount',@SglBilAmt
		
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
END
