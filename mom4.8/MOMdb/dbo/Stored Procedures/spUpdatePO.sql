CREATE PROCEDURE [dbo].[spUpdatePO] @PO int,
@fDate datetime,
@fDesc varchar(max),
@Amount numeric(30, 2),
@VendorId int,
@Status smallint,
@Due datetime,
@ShipVia varchar(50),
@Terms smallint,
@FOB varchar(50),
@ShipTo varchar(8000),
@Approved smallint,
@Custom1 varchar(50),
@Custom2 varchar(50),
@ApprovedBy varchar(25),
@ReqBy int,
@fBy varchar(50),
@POReasonCode varchar(50),
@CourrierAcct varchar(50),
@PORevision varchar(3),
@POItem AS tblTypePOItem READONLY,
@ApprovalStatus AS int,
@UpdatedBy varchar(100),
@RequestedBy varchar(100),
@IsPOClose BIT,
@SalesOrderNo varchar(50)
AS
BEGIN



insert into tblInventoryWHTrans(InvID, WarehouseID,LocationID ,Hand,Balance,fOrder,[Committed],[Available],Screen,ScreenID,Mode,Date,TransType,Batch,fDate)
select POItem.Inv, POItem.WarehouseID , POItem.LocationID ,0,0,(ABS(POItem.Quan)-ISNULL(POItem.SelectedQuan,0)) * -1,0,0  ,Screen= 'PO',@PO,'Edit',GETDATE(),'Revert',0,GETDATE() from POItem 
			inner join BOMT on BOMT.ID=POItem.TypeID --and  BOMT.Type='Inventory'
			where PO =@PO and  BOMT.Type='Inventory'

    SET NOCOUNT ON;
    DECLARE @CurrentVendor varchar(100)
    SELECT
        @CurrentVendor = r.Name
    FROM Rol r
    INNER JOIN Vendor V
        ON V.Rol = r.ID
    WHERE V.ID = (SELECT
        Vendor
    FROM PO
    WHERE PO = @PO)
    DECLARE @CurrentShipTo varchar(1000)
    SELECT
        @CurrentShipTo = [ShipTo]
    FROM PO
    WHERE PO = @PO
    DECLARE @CurrentfDesc varchar(1000)
    SELECT
        @CurrentfDesc = [fDesc]
    FROM PO
    WHERE PO = @PO
    DECLARE @CurrentShipVia varchar(50)
    SELECT
        @CurrentShipVia = [ShipVia]
    FROM PO
    WHERE PO = @PO
    DECLARE @CurrentCourrierAcct varchar(50)
    SELECT
        @CurrentCourrierAcct = [CourrierAcct]
    FROM PO
    WHERE PO = @PO
    DECLARE @CurrentfDate varchar(50)
    SELECT
        @CurrentfDate = CONVERT(varchar(50), fDate, 101)
    FROM PO
    WHERE PO = @PO
    DECLARE @CurrentDue varchar(50)
    SELECT
        @CurrentDue = CONVERT(varchar(50), Due, 101)
    FROM PO
    WHERE PO = @PO
    DECLARE @CurrentTerms varchar(150)
    SELECT
        @CurrentTerms = Name
    FROM tblterms
    WHERE ID = (SELECT
        Terms
    FROM PO
    WHERE PO = @PO)
	DECLARE @CurrentSalesOrderNo varchar(50)
    SELECT
        @CurrentSalesOrderNo = [SalesOrderNo]
    FROM PO
    WHERE PO = @PO
    DECLARE @CurrentPOReasonCode varchar(50)
    SELECT
        @CurrentPOReasonCode = [POReasonCode]
    FROM PO
    WHERE PO = @PO
    DECLARE @CurrentPORevision varchar(50)
    SELECT
        @CurrentPORevision = [PORevision]
    FROM PO
    WHERE PO = @PO
    DECLARE @CurrentFOB varchar(50)
    SELECT
        @CurrentFOB = [FOB]
    FROM PO
    WHERE PO = @PO
    DECLARE @CurrentfBy varchar(50)
    SELECT
        @CurrentfBy = [fBy]
    FROM PO
    WHERE PO = @PO
    DECLARE @CurrentApproved varchar(10)
    SELECT
        @CurrentApproved = [Approved]
    FROM PO
    WHERE PO = @PO
    DECLARE @CurrentStatus varchar(50)
    SELECT
        @CurrentStatus =
                        CASE [Status]
                            WHEN 0 THEN 'Open'
                            WHEN 1 THEN 'Closed'
                            WHEN 2 THEN 'Void'
                            WHEN 3 THEN 'Partial-Quantity'
                            WHEN 4 THEN 'Partial-Amount'
                            WHEN 5 THEN 'Closed At Received PO'
                        END
    FROM PO
    WHERE PO = @PO
    DECLARE @CurrentAmount numeric(30, 2)
    SELECT
        @CurrentAmount = Amount
    FROM PO
    WHERE PO = @PO
    DECLARE @CurrentRequested varchar(10)
    SELECT
        @CurrentRequested = RequestedBy
    FROM PO
    WHERE PO = @PO

    DECLARE @ID int
    DECLARE @Line smallint
    DECLARE @Quan numeric(30, 2)
    DECLARE @PofDesc varchar(8000)
    DECLARE @PoPrice numeric(30, 4)
    DECLARE @PoAmount numeric(30, 2)
    DECLARE @Job int
    DECLARE @Phase smallint
    DECLARE @Inv int
    DECLARE @GL int
    DECLARE @Billed int
    DECLARE @Ticket int
    DECLARE @Balance numeric(30, 2)
    DECLARE @Selected numeric(30, 2) = 0.0
    DECLARE @PoDue datetime
    DECLARE @SelectedQuan numeric(30, 2) = 0.0
    DECLARE @BalanceQuan numeric(30, 2) = 0.0
    DECLARE @TypeID int
    DECLARE @ItemDesc varchar(30)
    DECLARE @comm numeric(30, 2) = 0
    DECLARE @GLRev int = 0
    DECLARE @WarehouseID varchar(5)
    DECLARE @LocationID int = 0
    DECLARE @OpSq varchar(150)

    BEGIN TRY
        BEGIN TRANSACTION
            UPDATE [dbo].[PO]
            SET [fDate] = @fDate,
                [fDesc] = @fDesc,
                [Amount] = @Amount,
                [Vendor] = @VendorId,
                [Status] = @Status,
                [Due] = @Due,
                [ShipVia] = @ShipVia,
                [Terms] = @Terms,
                [FOB] = @FOB,
                [ShipTo] = @ShipTo,
                [Approved] = @Approved,
                [Custom1] = @Custom1,
                [Custom2] = @Custom2,
                [ApprovedBy] = @ApprovedBy,
                [ReqBy] = @ReqBy,
                [fBy] = @fBy,
                [PORevision] = @PORevision,
                [SalesOrderNo] = @SalesOrderNo,
                [POReasonCode] = @POReasonCode,
                [CourrierAcct] = @CourrierAcct,
                [RequestedBy] = @RequestedBy
            WHERE PO = @PO

            ---ES-1757 Mitsu - Approve PO add Status Reapprove   
            ----Update ApprovalStatus table when ApprovalStatus = 3  
            IF (@ApprovalStatus = 3)
            BEGIN
                UPDATE [dbo].ApprovalStatus
                SET Status = 3
                WHERE PO = @PO
            END

            --- if the PO.fdesc starts with Reapprove then to automatically change the status of that PO   
            IF (CHARINDEX('reapprove', LOWER(@fDesc)) > 0)
            BEGIN
                UPDATE ApprovalStatus
                SET Status = 3
                WHERE PO = @PO
            END

            DELETE FROM POItem
            WHERE PO = @PO

            CREATE TABLE #temp (
                ID int NULL,
                Line smallint NULL,
                AcctID int NULL,
                fDesc varchar(8000) NULL,
                Quan numeric(30, 2) NULL,
                Price numeric(30, 2) NULL,
                Amount numeric(30, 2) NULL,
                JobID numeric(30, 2) NULL,
                PhaseID int NULL,
                Inv int NULL,
                Billed int NULL,
                Ticket int NULL,
                Due datetime NULL,
                TypeID int NULL,
                ItemDesc varchar(30) NULL,
                WarehouseID varchar(5) NULL,
                LocationID int NULL,
                OpSq varchar(150) NULL,
                Selected numeric(30, 2) NULL,
                SelectedQuan numeric(30, 2) NULL,
            )

            DECLARE db_cursor1 CURSOR FOR

            SELECT ID,
                Line,
                Quan,
                fDesc,
                Price,
                Amount,
                JobID,
                PhaseID,
                Inv,
                AcctID,
                Billed,
                Ticket,
                Due,
                TypeID,
                ItemDesc,
                WarehouseID,
                LocationID,
                OpSq,
                Selected,
                SelectedQuan
            FROM @POItem

            OPEN db_cursor1
            FETCH NEXT FROM db_cursor1 INTO
            @ID, @Line, @Quan, @PofDesc, @PoPrice, @PoAmount, @Job, @Phase, @Inv, @GL, @Billed, @Ticket, @PoDue, @TypeID, @ItemDesc, @WarehouseID, @LocationID, @OpSq, @Selected, @SelectedQuan

            WHILE @@FETCH_STATUS = 0
            BEGIN

                --if(@Job is not null and (@Phase = 0 or @Phase is null) and (@TypeID = 1 or @TypeID = 2))      
                IF (@PofDesc != ''
                    AND @job IS NOT NULL)
                    --AND @Phase IS NOT NULL)  --and (@TypeID =1 or @TypeID =2)      
                BEGIN
                    IF (@Inv IS NOT NULL and @Inv != '' and @Inv != 0)
                    BEGIN
                        -- add into inv table      
                        IF (@ID = 0 OR @ID IS NULL)
                            EXEC @Phase = spAddBOMItem @Job,
                                                       @TypeID,
                                                       @Inv,
                                                       @PofDesc,
                                                       @Phase,
                                                       @OpSq
                    END
                    ELSE
                    --IF (@Inv IS NULL OR @Inv = 0)
                    BEGIN
                        IF EXISTS (SELECT TOP 1 1 FROM inv WHERE Name = @ItemDesc) -- check if item name and description is already exists!    
                        BEGIN
							SET @Inv = (SELECT TOP 1 ID FROM inv WHERE Name = @ItemDesc and type = 2)
							
							IF (@job != 0)
							BEGIN
								--CHECK IF ITEM ALREADY EXIST IN BOM    
								IF EXISTS (SELECT TOP 1
										line
									FROM jobtitem
									WHERE job = @job
									AND fDesc = @ItemDesc)
								BEGIN
									DECLARE @OPhase smallint
									SET @OPhase = (SELECT TOP 1
										line
									FROM jobtitem
									WHERE job = @job
									AND fDesc = @ItemDesc)
									IF (@ID = 0 OR @ID IS NULL)
										EXEC @Phase = spAddBOMItem @Job,
																   @TypeID,
																   @Inv,
																   @PofDesc,
																   @OPhase,
																   @OpSq
								END
								ELSE
								BEGIN
									IF (@ID = 0 OR @ID IS NULL)
										EXEC @Phase = spAddBOMItem @Job,
																   @TypeID,
																   @Inv,
																   @PofDesc,
																   @Phase,
																   @OpSq
								END
							END
							ELSE
							BEGIN
								IF (@ID = 0 OR @ID IS NULL)
										EXEC @Phase = spAddBOMItem @Job,
																   @TypeID,
																   @Inv,
																   @PofDesc,
																   @Phase,
																   @OpSq
							END
                        END
                        ELSE
                        BEGIN
                            IF @ItemDesc IS NOT NULL
                                AND @ItemDesc != ''
                            BEGIN
                                -- add into inv table (as non inventory type) and add as bom item      
                                SET @GLRev = ISNULL((SELECT
                                    SAcct
                                FROM Job job
                                INNER JOIN Inv inv
                                    ON job.GLRev = inv.ID
                                WHERE job.ID = @job)
                                , 0)
                                INSERT INTO Inv (Name, fdesc, Cat, Balance, Measure, Tax, AllowZero, InUse, Type, Sacct, Status, Price1)
                                    VALUES (@ItemDesc, @PofDesc, 0, 0, 'Each', 0, 0, 0, 2, @GLRev, 0, 0)

                                SET @Inv = SCOPE_IDENTITY()
                                --IF (@ID = 0 OR @ID IS NULL)
                                EXEC @Phase = spAddBOMItem @Job,
                                                            @TypeID,
                                                            @Inv,
                                                            @PofDesc,
                                                            @Phase,
                                                            @OpSq
                            END
                            ELSE
                            BEGIN
                                IF (@ID = 0 OR @ID IS NULL)
                                    EXEC @Phase = spAddBOMItem @Job,
                                                               @TypeID,
                                                               @Inv,
                                                               @PofDesc,
                                                               @Phase,
                                                               @OpSq
                            END
                        END
                    END
                END

                --IF (@PofDesc != ''
                --    AND @job = 0
                --    AND @Phase = 0)  --and  @TypeID =8       
                --BEGIN
                --    IF (@Inv IS NOT NULL)
                --    BEGIN
                --        -- insert bom job item      
                --        IF (@ID = 0
                --            OR @ID IS NULL)
                --            EXEC @Phase = spAddBOMItem @job,
                --                                       @TypeID,
                --                                       @Inv,
                --                                       @PofDesc,
                --                                       @Phase,
                --                                       @OpSq

                --    END
                --    ELSE
                --    IF (@Inv IS NULL)
                --    BEGIN
                --        -- add into inv table (as non inventory type) and add as bom item      
                --        --if exists (select top 1 1 from inv where Name = @ItemDesc and fDesc = @PofDesc) -- check if item name and description is already exists!    
                --        IF EXISTS (SELECT TOP 1
                --                1
                --            FROM inv
                --            WHERE Name = @ItemDesc) -- check if item name and description is already exists!      
                --        BEGIN
                --            --set @Inv = (select top 1 ID from inv where Name = @ItemDesc and fDesc = @PofDesc and type = 2)      
                --            SET @Inv = (SELECT TOP 1
                --                ID
                --            FROM inv
                --            WHERE Name = @ItemDesc
                --            AND type = 2)

                --            -- insert bom job item      
                --            IF (@ID = 0
                --                OR @ID IS NULL)
                --                EXEC @Phase = spAddBOMItem @job,
                --                                           @TypeID,
                --                                           @Inv,
                --                                           @PofDesc,
                --                                           @Phase,
                --                                           @OpSq
                --        END
                --        ELSE
                --        BEGIN
                --            IF @ItemDesc IS NOT NULL
                --                AND @ItemDesc != ''
                --            BEGIN
                --                SET @GLRev = ISNULL((SELECT
                --                    SAcct
                --                FROM Job job
                --                INNER JOIN Inv inv
                --                    ON job.GLRev = inv.ID
                --                WHERE job.ID = @job)
                --                , 0)
                --                INSERT INTO Inv (Name, fdesc, Cat, Balance, Measure, Tax, AllowZero, InUse, Type, Sacct, Status, Price1)
                --                    VALUES (@ItemDesc, @PofDesc, 0, 0, 'Each', 0, 0, 0, 2, @GLRev, 0, 0)

                --                SET @Inv = SCOPE_IDENTITY()

                --                -- insert bom job item      
                --                IF (@ID = 0
                --                    OR @ID IS NULL)
                --                    EXEC @Phase = spAddBOMItem @job,
                --                                               @TypeID,
                --                                               @Inv,
                --                                               @PofDesc,
                --                                               @Phase,
                --                                               @OpSq
                --            END
                --        END
                --    END
                --END

                INSERT INTO #temp (Line, Quan, fDesc, Price, Amount, JobID, PhaseID, Inv, AcctID, Billed, Ticket, Due, TypeID, ItemDesc, WarehouseID, LocationID, OpSq, Selected, SelectedQuan)
                    VALUES (@Line, @Quan, @PofDesc, @PoPrice, @PoAmount, @Job, @Phase, @Inv, @GL, @Billed, @Ticket, @PoDue, @TypeID, @ItemDesc, @WarehouseID, @LocationID, @OpSq, @Selected, @SelectedQuan)

                ------------>    
                SET @ID = NULL;
                SET @Line = NULL;
                SET @Quan = NULL;
                SET @PofDesc = NULL;
                SET @PoPrice = NULL;
                SET @PoAmount = NULL;
                SET @Job = NULL;
                SET @Phase = NULL;
                SET @Inv = NULL;
                SET @GL = NULL;
                SET @Billed = NULL;
                SET @Ticket = NULL;
                SET @PoDue = NULL;
                SET @TypeID = NULL;
                SET @ItemDesc = NULL;
                SET @WarehouseID = NULL;
                SET @LocationID = NULL;
                SET @OpSq = NULL;
                SET @Selected = NULL;
                SET @SelectedQuan = NULL;
                ------------>    


                FETCH NEXT FROM db_cursor1 INTO
                @ID, @Line, @Quan, @PofDesc, @PoPrice, @PoAmount, @Job, @Phase, @Inv, @GL, @Billed, @Ticket, @PoDue, @TypeID, @ItemDesc, @WarehouseID, @LocationID, @OpSq, @Selected, @SelectedQuan
            END

            CLOSE db_cursor1
            DEALLOCATE db_cursor1

            -----------------------------$$$$$$$$$$$$$$$$$$$$$$$----------------------------    

            DECLARE db_cursor CURSOR FOR

            SELECT
                Line,
                Quan,
                fDesc,
                Price,
                Amount,
                JobID,
                PhaseID,
                Inv,
                AcctID,
                Billed,
                Ticket,
                Due,
                WarehouseID,
                LocationID,
                OpSq,
                Selected,
                SelectedQuan,
                TypeID
            FROM #temp

            OPEN db_cursor
            FETCH NEXT FROM db_cursor INTO
            @Line, @Quan, @PofDesc, @PoPrice, @PoAmount, @Job, @Phase, @Inv, @GL, @Billed, @Ticket, @PoDue, @WarehouseID, @LocationID, @OpSq, @Selected, @SelectedQuan, @TypeID
            WHILE @@FETCH_STATUS = 0
            BEGIN
                SET @Balance = @PoAmount - @Selected
                SET @BalanceQuan = @Quan - @SelectedQuan

                INSERT INTO [dbo].[POItem] ([PO], [Line], [Quan], [fDesc], [Price], [Amount], [Job], [Phase], [Inv], [GL], [Billed], [Ticket], [Selected], [Balance], [Due], [SelectedQuan], [BalanceQuan], WarehouseID, LocationID, TypeID)

                    VALUES (@PO, @Line, @Quan, @PofDesc, @PoPrice, @PoAmount, @Job, @Phase, @Inv, @GL, @Billed, @Ticket, @Selected, @Balance, @PoDue, @SelectedQuan, @BalanceQuan, @WarehouseID, @LocationID, @TypeID)

                EXEC spUpdateJobCommExp @Job

				   ----Update Lcost-------->
		   UPDATE Inv SET LCost=@PoPrice WHERE TYPE=0 and id=@Inv

		   if exists ( select 1 from Inv WHERE TYPE=0 and id=@Inv )

		   begin

		     if not exists (select * from InvWarehouse where InvID=@Inv and WareHouseID=@WarehouseID)
			 begin 
			 insert into InvWarehouse (InvID , WareHouseID )
			 select @Inv , @WarehouseID
			 end

			 if not exists (select * from IWarehouseLocAdj where InvID=@Inv and WareHouseID=@WarehouseID)
			 begin 
			INSERT INTO [dbo].[IWarehouseLocAdj]
			([InvID]           ,[WarehouseID]           ,[LocationID]
           ,[Hand]           ,[Balance]           ,[fOrder]           ,[Committed]           ,[Available])
			 select @Inv , @WarehouseID , 0  ,0,0,0,0,0
			 end

		   end


		   
		   
			

		  -------------------------> 

                IF @Phase IS NOT NULL
                BEGIN
                    SET @comm = ISNULL((SELECT
                        SUM(ISNULL(p.Balance, 0))
                    FROM POItem p
                    INNER JOIN PO
                        ON p.po = po.po
                    WHERE p.Job = @Job
                    AND p.Phase = @Phase
                    AND po.status IN (0, 3, 4))
                    , 0) +
                    ISNULL((SELECT
                        SUM(ISNULL(rp.Amount, 0))
                    FROM RPOItem rp
                    INNER JOIN ReceivePO r
                        ON r.ID = rp.ReceivePO
                        LEFT JOIN POItem p
                            ON r.PO = p.PO
                            AND rp.POLine = p.Line
                    WHERE p.Job = @job
                    AND p.Phase = @Phase
                    AND r.status <> 1)
                    , 0)

                    IF (@TypeID = 1
                        OR @TypeID = 2)
                    BEGIN
                        UPDATE JobTItem

                        SET Comm = @comm

                        WHERE Type = 1
                        AND Job = @Job
                        AND Line = @Phase
                        AND Code = @OpSq
                    END
                    ELSE
                    BEGIN
                        UPDATE JobTItem

                        SET Comm = @comm

                        WHERE Type = 2
                        AND Job = @Job
                        AND Line = @Phase
                        AND Code = @OpSq
                    END

                END

                ----------------------->    

                SET @Line = NULL;
                SET @Quan = NULL;
                SET @PofDesc = NULL;
                SET @PoPrice = NULL;
                SET @PoAmount = NULL;
                SET @Job = NULL;
                SET @Phase = NULL;
                SET @Inv = NULL;
                SET @GL = NULL;
                SET @Billed = NULL;
                SET @Ticket = NULL;
                SET @PoDue = NULL;
                SET @WarehouseID = NULL;
                SET @LocationID = NULL;
                SET @OpSq = NULL;
                SET @Selected = NULL;
                SET @SelectedQuan = NULL;
                SET @TypeID = NULL;
                ----------------------->    


                FETCH NEXT FROM db_cursor INTO
                @Line, @Quan, @PofDesc, @PoPrice, @PoAmount, @Job, @Phase, @Inv, @GL, @Billed, @Ticket, @PoDue, @WarehouseID, @LocationID, @OpSq, @Selected, @SelectedQuan, @TypeID
            END

            CLOSE db_cursor
            DEALLOCATE db_cursor

            DROP TABLE #temp

			insert into tblInventoryWHTrans(InvID, WarehouseID,LocationID ,Hand,Balance,fOrder,[Committed],[Available],Screen,ScreenID,Mode,Date,TransType,Batch,fDate)
         select POItem.Inv, POItem.WarehouseID , POItem.LocationID ,0,0,ISNULL(POItem.Quan,0)-ISNULL(POItem.SelectedQuan,0),0,0,Screen= 'PO',@PO,'Edit',GETDATE(),'None',0,GETDATE() from POItem 
			inner join BOMT on BOMT.ID=POItem.TypeID --and  BOMT.Type='Inventory'
			where PO =@PO and  BOMT.Type='Inventory'

            EXEC spCalChartBalance
            --UPDATE Chart       
            --   SET Balance = ISNULL (p.Balance , 0)      
            --  FROM Chart c LEFT JOIN      
            -- (SELECT Sum(Amount) AS Balance      
            --  FROM PO) p      
            --  ON c.DefaultNo = 'D9991' AND Status = 0  
			

			EXEC [spAutoUpdatePOStatus] @PO, @UpdatedBy
			EXEC CalculateInventory

              
			    IF (@IsPOClose = 1)		------------------------ PO CLOSE --------------------------
			    BEGIN
					if (@CurrentStatus = 'Partial-Quantity' OR @CurrentStatus='Partial-Amount' OR @CurrentStatus='Closed At Received PO')
					BEGIN
						EXEC spClosePO @PO, @UpdatedBy
					END
				END
		    
            /********Start Logs************/
            DECLARE @Val varchar(1000)
            IF (@VendorId IS NOT NULL
                AND @VendorId != 0)
            BEGIN
                SET @Val = (SELECT TOP 1
                    newVal
                FROM log2
                WHERE screen = 'PO'
                AND ref = @PO
                AND Field = 'Vendor'
                ORDER BY CreatedStamp DESC)
                DECLARE @VendorName varchar(150)
                SELECT
                    @VendorName = r.Name
                FROM Rol r
                INNER JOIN Vendor V
                    ON V.Rol = r.ID
                WHERE V.ID = @VendorId
                IF (@Val <> @VendorName)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'PO',
                                     @PO,
                                     'Vendor',
                                     @Val,
                                     @VendorName
                END
                ELSE
                IF (@CurrentVendor <> @VendorName)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'PO',
                                     @PO,
                                     'Vendor',
                                     @CurrentVendor,
                                     @VendorName
                END
            END
            SET @Val = NULL
            IF (@ShipTo IS NOT NULL)
            BEGIN
                SET @Val = (SELECT TOP 1
                    newVal
                FROM log2
                WHERE screen = 'PO'
                AND ref = @PO
                AND Field = 'Ship To'
                ORDER BY CreatedStamp DESC)
                IF (@Val <> @ShipTo)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'PO',
                                     @PO,
                                     'Ship To',
                                     @Val,
                                     @ShipTo
                END
                ELSE
                IF (@CurrentShipTo <> @ShipTo)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'PO',
                                     @PO,
                                     'Ship To',
                                     @CurrentShipTo,
                                     @ShipTo
                END
            END
            SET @Val = NULL
            IF (@fDesc IS NOT NULL)
            BEGIN
                SET @Val = (SELECT TOP 1
                    newVal
                FROM log2
                WHERE screen = 'PO'
                AND ref = @PO
                AND Field = 'Comments'
                ORDER BY CreatedStamp DESC)
                IF (@Val <> CONVERT(varchar(1000), @fDesc))
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'PO',
                                     @PO,
                                     'Comments',
                                     @Val,
                                     @fDesc
                END
                ELSE
                IF (@CurrentfDesc <> CONVERT(varchar(1000), @fDesc))
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'PO',
                                     @PO,
                                     'Comments',
                                     @CurrentfDesc,
                                     @fDesc
                END
            END
            SET @Val = NULL
            IF (@ShipVia IS NOT NULL)
            BEGIN
                SET @Val = (SELECT TOP 1
                    newVal
                FROM log2
                WHERE screen = 'PO'
                AND ref = @PO
                AND Field = 'Courier'
                ORDER BY CreatedStamp DESC)
                IF (@Val <> @ShipVia)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'PO',
                                     @PO,
                                     'Courier',
                                     @Val,
                                     @ShipVia
                END
                ELSE
                IF (@CurrentShipVia <> @ShipVia)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'PO',
                                     @PO,
                                     'Courier',
                                     @CurrentShipVia,
                                     @ShipVia
                END
            END
            SET @Val = NULL
            IF (@CourrierAcct IS NOT NULL)
            BEGIN
                SET @Val = (SELECT TOP 1
                    newVal
                FROM log2
                WHERE screen = 'PO'
                AND ref = @PO
                AND Field = 'Courier Account #'
                ORDER BY CreatedStamp DESC)
                IF (@Val <> @CourrierAcct)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'PO',
                                     @PO,
                                     'Courier Account #',
                                     @Val,
                                     @CourrierAcct
                END
                ELSE
                IF (@CurrentCourrierAcct <> @CourrierAcct)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'PO',
                                     @PO,
                                     'Courier Account #',
                                     @CurrentCourrierAcct,
                                     @CourrierAcct
                END
            END
            SET @Val = NULL
            IF (@fDate IS NOT NULL)
            BEGIN
                SET @Val = (SELECT TOP 1
                    newVal
                FROM log2
                WHERE screen = 'PO'
                AND ref = @PO
                AND Field = 'Date'
                ORDER BY CreatedStamp DESC)
                DECLARE @Calldate nvarchar(150)
                SELECT
                    @Calldate = CONVERT(varchar, @fDate, 101)
                IF (@Val <> @Calldate)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'PO',
                                     @PO,
                                     'Date',
                                     @Val,
                                     @Calldate
                END
                ELSE
                IF (@CurrentfDate <> @Calldate)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'PO',
                                     @PO,
                                     'Date',
                                     @CurrentfDate,
                                     @Calldate
                END
            END
            SET @Val = NULL
            SET @Val = NULL
            IF (@Due IS NOT NULL)
            BEGIN
                SET @Val = (SELECT TOP 1
                    newVal
                FROM log2
                WHERE screen = 'PO'
                AND ref = @PO
                AND Field = 'Due Date'
                ORDER BY CreatedStamp DESC)
                DECLARE @Duedate nvarchar(150)
                SELECT
                    @Duedate = CONVERT(varchar, @Due, 101)
                IF (@Val <> @Duedate)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'PO',
                                     @PO,
                                     'Due Date',
                                     @Val,
                                     @Duedate
                END
                ELSE
                IF (@CurrentDue <> @Duedate)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'PO',
                                     @PO,
                                     'Due Date',
                                     @CurrentDue,
                                     @Duedate
                END
            END
            SET @Val = NULL
            IF (@Terms IS NOT NULL)
            BEGIN
                SET @Val = (SELECT TOP 1
                    newVal
                FROM log2
                WHERE screen = 'PO'
                AND ref = @PO
                AND Field = 'Payment Terms'
                ORDER BY CreatedStamp DESC)
                DECLARE @PaymentTerms varchar(150)
                SELECT
                    @PaymentTerms = Name
                FROM tblterms
                WHERE ID = @Terms
                IF (@Val <> @PaymentTerms)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'PO',
                                     @PO,
                                     'Payment Terms',
                                     @Val,
                                     @PaymentTerms
                END
                ELSE
                IF (@CurrentTerms <> @PaymentTerms)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'PO',
                                     @PO,
                                     'Payment Terms',
                                     @CurrentTerms,
                                     @PaymentTerms
                END
            END
			SET @Val = NULL
            IF (@SalesOrderNo IS NOT NULL)
            BEGIN
                SET @Val = (SELECT TOP 1
                    newVal
                FROM log2
                WHERE screen = 'PO'
                AND ref = @PO
                AND Field = 'Sales Order #'
                ORDER BY CreatedStamp DESC)
                IF (@Val <> @SalesOrderNo)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'PO',
                                     @PO,
                                     'Sales Order #',
                                     @Val,
                                     @SalesOrderNo
                END
                ELSE
                IF (@CurrentSalesOrderNo <> @SalesOrderNo)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'PO',
                                     @PO,
                                     'Sales Order #',
                                     @CurrentSalesOrderNo,
                                     @SalesOrderNo
                END
            END
            SET @Val = NULL
            IF (@POReasonCode IS NOT NULL)
            BEGIN
                SET @Val = (SELECT TOP 1
                    newVal
                FROM log2
                WHERE screen = 'PO'
                AND ref = @PO
                AND Field = 'PO Reason Code'
                ORDER BY CreatedStamp DESC)
                IF (@Val <> @POReasonCode)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'PO',
                                     @PO,
                                     'PO Reason Code',
                                     @Val,
                                     @POReasonCode
                END
                ELSE
                IF (@CurrentPOReasonCode <> @POReasonCode)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'PO',
                                     @PO,
                                     'PO Reason Code',
                                     @CurrentPOReasonCode,
                                     @POReasonCode
                END
            END
            SET @Val = NULL
            IF (@PORevision IS NOT NULL)
            BEGIN
                SET @Val = (SELECT TOP 1
                    newVal
                FROM log2
                WHERE screen = 'PO'
                AND ref = @PO
                AND Field = 'PO Revision'
                ORDER BY CreatedStamp DESC)
                IF (@Val <> @PORevision)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'PO',
                                     @PO,
                                     'PO Revision',
                                     @Val,
                                     @PORevision
                END
                ELSE
                IF (@CurrentPORevision <> @PORevision)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'PO',
                                     @PO,
                                     'PO Revision',
                                     @CurrentPORevision,
                                     @PORevision
                END
            END
            SET @Val = NULL
            IF (@FOB IS NOT NULL)
            BEGIN
                SET @Val = (SELECT TOP 1
                    newVal
                FROM log2
                WHERE screen = 'PO'
                AND ref = @PO
                AND Field = 'Incoterms'
                ORDER BY CreatedStamp DESC)
                IF (@Val <> @FOB)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'PO',
                                     @PO,
                                     'Incoterms',
                                     @Val,
                                     @FOB
                END
                ELSE
                IF (@CurrentFOB <> @FOB)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'PO',
                                     @PO,
                                     'Incoterms',
                                     @CurrentFOB,
                                     @FOB
                END
            END
            SET @Val = NULL
            IF (@fBy IS NOT NULL)
            BEGIN
                SET @Val = (SELECT TOP 1
                    newVal
                FROM log2
                WHERE screen = 'PO'
                AND ref = @PO
                AND Field = 'Created By'
                ORDER BY CreatedStamp DESC)
                IF (@Val <> @fBy)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'PO',
                                     @PO,
                                     'Created By',
                                     @Val,
                                     @fBy
                END
                ELSE
                IF (@CurrentfBy <> @fBy)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'PO',
                                     @PO,
                                     'Created By',
                                     @CurrentfBy,
                                     @fBy
                END
            END
            SET @Val = NULL
            IF (@Approved IS NOT NULL)
            BEGIN
                SET @Val = (SELECT TOP 1
                    newVal
                FROM log2
                WHERE screen = 'PO'
                AND ref = @PO
                AND Field = 'Approved'
                ORDER BY CreatedStamp DESC)
                IF (@Val <> CONVERT(varchar(10), @Approved))
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'PO',
                                     @PO,
                                     'Approved',
                                     @Val,
                                     @Approved
                END
                ELSE
                IF (@CurrentApproved <> CONVERT(varchar(10), @Approved))
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'PO',
                                     @PO,
                                     'Approved',
                                     @CurrentApproved,
                                     @Approved
                END
            END
            SET @Val = NULL
            IF (@Status IS NOT NULL)
            BEGIN
                SET @Val = (SELECT TOP 1
                    newVal
                FROM log2
                WHERE screen = 'PO'
                AND ref = @PO
                AND Field = 'Status'
                ORDER BY CreatedStamp DESC)
                DECLARE @CurrentStatusVal varchar(50)
                SELECT
                    @CurrentStatusVal =
                                           CASE @Status
                                               WHEN 0 THEN 'Open'
                                               WHEN 1 THEN 'Closed'
                                               WHEN 2 THEN 'Void'
                                               WHEN 3 THEN 'Partial-Quantity'
                                               WHEN 4 THEN 'Partial-Amount'
                                               WHEN 5 THEN 'Closed At Received PO'
                                           END
                IF (@Val <> @CurrentStatusVal)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'PO',
                                     @PO,
                                     'Status',
                                     @Val,
                                     @CurrentStatusVal
                END
                ELSE
                IF (@CurrentStatus <> @CurrentStatusVal)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'PO',
                                     @PO,
                                     'Status',
                                     @CurrentStatus,
                                     @CurrentStatusVal
                END
            END
            SET @Val = NULL
            IF (@Amount IS NOT NULL)
            BEGIN
                SET @Val = (SELECT TOP 1
                    newVal
                FROM log2
                WHERE screen = 'PO'
                AND ref = @PO
                AND Field = 'Amount'
                ORDER BY CreatedStamp DESC)
                IF (@Val <> @Amount)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'PO',
                                     @PO,
                                     'Amount',
                                     @Val,
                                     @Amount
                END
                ELSE
                IF (@CurrentAmount <> @Amount)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'PO',
                                     @PO,
                                     'Amount',
                                     @CurrentAmount,
                                     @Amount
                END
                SET @Val = NULL
                IF (@RequestedBy IS NOT NULL)
                BEGIN
                    SET @Val = (SELECT TOP 1
                        newVal
                    FROM log2
                    WHERE screen = 'PO'
                    AND ref = @PO
                    AND Field = 'RequestedBy'
                    ORDER BY CreatedStamp DESC)
                    IF (@Val <> @RequestedBy)
                    BEGIN
                        EXEC log2_insert @UpdatedBy,
                                         'PO',
                                         @PO,
                                         'RequestedBy',
                                         @Val,
                                         @RequestedBy
                    END
                    ELSE
                    IF (@CurrentRequested <> @RequestedBy)
                    BEGIN
                        EXEC log2_insert @UpdatedBy,
                                         'PO',
                                         @PO,
                                         'RequestedBy',
                                         @CurrentRequested,
                                         @RequestedBy
                    END
                END
            END
        /********End Logs************/

        COMMIT
    END TRY
    BEGIN CATCH

        SELECT
            ERROR_MESSAGE()

        IF @@TRANCOUNT > 0
            ROLLBACK
        RAISERROR ('An error has occurred on this page.', 16, 1)
        RETURN

    END CATCH

END
GO
