CREATE PROCEDURE [dbo].[spPostToProject]
	@CallDt DateTime,
	@Caller VARCHAR(30),
	@cat    VARCHAR(50),
	@Reason VARCHAR(max),
	@InventoryItems tblTypePostToProject ReadOnly
AS
DECLARE @TickectIDs varchar(255) = ''
BEGIN TRY
    BEGIN TRANSACTION
		DECLARE @DefaultCat    VARCHAR(50) = 'Material Allocation'
		DECLARE @DefaultReason    VARCHAR(50) = 'Material Allocation'
		DECLARE @Equipments TBLTYPEMULTIPLEEEQUIPMENTS
		DECLARE @taskCodes tblTypeTaskCodes
		SELECT JobID, Worker, SchDate INTO #TempTable
		FROM @InventoryItems 
		GROUP BY JobID, Worker, SchDate

		-- Check and insert default category "Material Allocation" if it hasn't existed in database
		IF NOT EXISTS (SELECT 1 FROM Category WHERE Type = @DefaultCat)
		BEGIN
			INSERT INTO Category (Type) Values (@DefaultCat)
		END

		DECLARE @job INT, @worker VARCHAR(255), @SchDate DATETIME

		DECLARE db_cursor CURSOR FOR 
		SELECT * FROM #TempTable
		OPEN db_cursor  
		FETCH NEXT FROM db_cursor INTO @job,@worker,@SchDate
		WHILE @@FETCH_STATUS = 0
		BEGIN
			DECLARE @LocID      INT,
					@LocTag     VARCHAR(50),
					@LocAdd     VARCHAR(100),
					@City       VARCHAR(50),
					@State      VARCHAR(2),
					@Zip        VARCHAR(100),
					@custID     INT,
					@Department INT,
					@Phone      VARCHAR(28),
					@Cell       VARCHAR(50),
					@remarks    VARCHAR(max),
					@lat             VARCHAR(50),
					@lng             VARCHAR(50),
					@DefaultRoute    INT,
					@CreditHold      TINYINT,
					@DispAlert       TINYINT,
					@CreditReason    VARCHAR(max),
					@Contact         VARCHAR(50),
					@JobCode VARCHAR(10)
			SELECT @LocID = j.Loc,
					@LocTag = l.Tag,
					@LocAdd = l.Address,
					@City = l.City,
					@State = l.State,
					@Zip = l.Zip,
					@custID = l.Owner,
					@Department = j.Type,
					@remarks = l.Remarks,
					@DefaultRoute = l.Route,
					@CreditHold = l.Credit,
					@DispAlert = l.DispAlert,
					@CreditReason = l.CreditReason
			FROM   job j
			INNER JOIN loc l
					ON l.loc = j.Loc
			WHERE  j.ID = @job

			SELECT top 1 @Phone=Phone,
				@Cell=Cellular,
				@lat = Lat,
				@lng = Lng,
				@Contact = Contact
			from rol where ID=(select top 1 rol from loc where loc=@LocID)

			--Select @JobCode = (SELECT TOP 1 CONVERT(VARCHAR, j.Line) + ':' + j.Code + ':' + j.fDesc
			--					FROM   jobtitem j
   --                                INNER JOIN bom b ON b.JobtItemId = j.ID
   --                                INNER JOIN BOMT ON BOMT.ID = b.Type
   --                         WHERE  j.job = @job
   --                                )

			DECLARE @TicketID INT
			SELECT @TicketID = Max([NewID]) + 1
			FROM   (SELECT Isnull(Max(TicketO.ID), 0) AS [NewID]
					FROM   TicketO
					UNION ALL
					SELECT Isnull(Max(TicketD.ID), 0) AS [NewID]
					FROM   TicketD) A

			DECLARE @dtTicketINV as tblTypeTicketINV
			DELETE FROM @dtTicketINV

			DECLARE @DistinctInvItems as tblTypePostToProject
			DELETE FROM @DistinctInvItems
			INSERT INTO @DistinctInvItems (
				[PhaseID]		,
				[Phase]			,
				[Inv]			,
				[ItemDesc]		,
				[fDesc]			,
				[Quan]			,
				[WHLocID]		,
				[WarehouseID]	,
				[TypeID]		,
				[JobID]			,
				[AcctID]		,
				[AcctNo]		,
				[SchDate]		,
				[Worker]		,
				[Billed]
			)
			SELECT [PhaseID]	,
				[Phase]			,
				[Inv]			,
				[ItemDesc]		,
				[fDesc]			,
				SUM([Quan])		,
				[WHLocID]		,
				[WarehouseID]	,
				[TypeID]		,
				[JobID]			,
				[AcctID]		,
				[AcctNo]		,
				[SchDate]		,
				[Worker]		,
				[Billed]
			FROM @InventoryItems 
			WHERE JobID=@job and  Worker=@worker and SchDate = @SchDate
			GROUP BY [PhaseID]	,
				[Phase]			,
				[Inv]			,
				[ItemDesc]		,
				[fDesc]			,
				[WHLocID]		,
				[WarehouseID]	,
				[TypeID]		,
				[JobID]			,
				[AcctID]		,
				[AcctNo]		,
				[SchDate]		,
				[Worker]		,
				[Billed]

			INSERT INTO @dtTicketINV (Ticket, AID, fDesc, Item, Line, LocationID, Phase, PhaseName, Quan, TypeID, WarehouseID, Charge)
			SELECT @TicketID, AcctNo, fDesc, Inv, ROW_NUMBER() OVER(ORDER BY [Inv] DESC), WHLocID, PhaseID, Phase, Quan, TypeID, WarehouseID, Billed from @DistinctInvItems 
			where JobID=@job and  Worker=@worker and SchDate = @SchDate
			
			SELECT * FROM @dtTicketINV
			-- Put code for create ticket here
			EXEC spAddTicket
						  @LocID,
						  @LocTag,
						  @LocAdd,
						  @City,
						  @State,
						  @Zip,
						  @Phone,--phone
						  @Cell,--cell
						  @Worker,
						  @CallDt,
						  @SchDate,
						  4,--@Status,
						  NULL,--@EnrouteTime
						  NULL,--@Onsite
						  NULL,--@Complete
						  @DefaultCat,
						  null,--@Unit,
						  @DefaultReason,
						  NULL,--@CustName
						  @custID,
						  0,--@EST,
						  @DefaultReason,--@complDesc
						  0,--@TicketIDOut
						  NULL,--@AID
						  @Caller,
						  NULL,--@sign
						  0.00,--reg
						  0.00,--ot
						  0.00,--nt
						  0.00,--tt
						  0.00,--dt
						  0.00,--total
						  0,--@Charge
						  1,--@Review
						  @remarks,--@Remarks
						  1,--@Level
						  @Department,
						  @job,
						  '',--@Custom1
						  '',--@Custom2
						  '',--@Custom3
						  '',--@Custom4
						  '',--@Custom5
						  0,--@Custom6
						  0,--@Custom7
						  @TicketID,
						  1,-- @WorkComplete
						  0,-- @MiscExp
						  0,-- @TollExp
						  0,-- @ZoneExp
						  0,-- @MileStart
						  0,-- @MileEnd
						  0,-- @Internet
						  '',-- @Invoice
						  0,--@ @TransferTime
						  @CreditHold,-- @CreditHold
						  @DispAlert,--  @DispAlert
						  @CreditReason,-- @CreditReason
						  0,-- @IsRecurring
						  NULL,-- @QBServiceItem
						  NULL,-- @QBPayrollItem
						  '',--@ @LastUpdatedBy
						  @Contact,--@ @Contact
						  '',--@ @Recommendation
						  '',--@ @Customtick1
						  '',--@ @Customtick2
						  '',--@ @Customtick3
						  '',--@ @Customtick4
						  @lat,-- @lat
						  @lng,-- @lng
						  @DefaultRoute,-- @DefaultRoute
						  NULL,-- @Customtick5
						  NULL,-- @JobCode
						  NULL,-- @ProjectTemplate
						  NULL,-- @wage
						  @Caller,
						  NULL,--@RecurringDate     
						  @Equipments,
						  0,--@UpdateTasks
						  @taskCodes,--tblTypeTaskCodes ,
						  0,--@BT
						  null ,--@Comments
						  null, --@PartsUsed 
						  0,-- @Zone
						  @dtTicketINV,
						  'Post To Project'
			-- End create ticket
			SET @TickectIDs = @TickectIDs + Convert(varchar(10),@TicketID) + ','
			-- Need to move this source code to spAddTicket sp
			--IF ((SELECT COUNT(1) FROM @dtTicketINV)>0 AND  (SELECT COUNT(*) FROM TicketI WHERE Ticket=@TicketID AND charge=1)>0)
			--BEGIN
			--	UPDATE TicketD
			--	SET Charge=1
			--	WHERE ID=@TicketID
   --         End
			
			-- Reset values
			SET @job=null
			SET @worker=null 
			SET @SchDate=null
			--
			FETCH NEXT FROM db_cursor INTO @job,@worker,@SchDate
		END  
		CLOSE db_cursor  
		DEALLOCATE db_cursor
	COMMIT
	SELECT @TickectIDs
END TRY
BEGIN CATCH
    DECLARE @err varchar(max)
    SELECT @err = ERROR_MESSAGE()
    IF @@TRANCOUNT > 0 ROLLBACK
    RAISERROR (@err, 16, 1)
    RETURN
END CATCH