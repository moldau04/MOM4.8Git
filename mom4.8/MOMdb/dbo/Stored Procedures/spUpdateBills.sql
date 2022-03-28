CREATE PROCEDURE [dbo].[spUpdateBills] @APBillslineItems tblTypeAPBillslineItem READONLY,
@PJID int,
@Vendor int,
@Date datetime,
@PostingDate datetime,
@Due datetime,
@Ref varchar(50),
@Memo varchar(max),
@DueIn smallint,
@PO int = NULL,
@ReceivePo int = NULL,
@Status smallint,
@Disc numeric(30, 4),
@Custom1 varchar(50),
@Custom2 varchar(50),
@Batch int,
@TransId int,
@UpdatedBy varchar(100),
@IfPaid int = NULL,
@PJSTax Decimal(10,4),
@PJSTaxName Varchar(50),
@PJSTaxGL int,
@PJSTaxRate Decimal(10,4),
@PJUTax Decimal(10,4),
@PJUTaxName Varchar(50),
@PJUTaxGL int,
@PJUTaxRate Decimal(10,4),
@PJGST Varchar(50),
@PJGSTGL int,
@PJGSTRate Decimal(10,4),
@IsPOClose BIT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CurrentVendor varchar(100)
    SELECT
        @CurrentVendor = r.Name
    FROM Rol r
    INNER JOIN Vendor V
        ON V.Rol = r.ID
    WHERE V.ID = (SELECT
        Vendor
    FROM PJ
    WHERE ID = @PJID)
    DECLARE @CurrentDue varchar(50)
    SELECT
        @CurrentDue = CONVERT(varchar(50), Due, 101)
    FROM OpenAP
    WHERE PJID = @PJID

    DECLARE @CurrentPO varchar(50)
    DECLARE @CurrentReceivePo varchar(50)
    DECLARE @CurrentRef varchar(50)
    DECLARE @CurrentDate varchar(50)
    DECLARE @CurrentPostingDate varchar(50)
    DECLARE @CurrentDueIn varchar(50)
    DECLARE @CurrentDisc numeric(30, 4)
    DECLARE @CurrentSpec varchar(50)
    DECLARE @CurrentMemo varchar(1000)
    DECLARE @CurrentCustom1 varchar(50)
    DECLARE @CurrentCustom2 varchar(50)
    DECLARE @CurrentAmount numeric(30, 2)
    DECLARE @CurrentUseTax numeric(30, 4)
    DECLARE @CurrentStatus smallint
    DECLARE @CurrentIfPaid varchar(50)
    SELECT
        @CurrentPO = PO,
        @CurrentReceivePo = ReceivePO,
        @CurrentRef = Ref,
        @CurrentDate = CONVERT(varchar(50), IDate, 101),
        @CurrentPostingDate = CONVERT(varchar(50), fDate, 101),
        @CurrentDueIn = Terms,
        @CurrentDisc = Disc,
        @CurrentSpec =
                      CASE Spec
                          WHEN 0 THEN 'Input Only'
                          WHEN 1 THEN 'Hold - No Invoices'
                          WHEN 2 THEN 'Hold - No Materials'
                          WHEN 3 THEN 'Hold - Other'
                          WHEN 4 THEN 'Verified'
                          WHEN 5 THEN 'Selected'
                      END,
        @CurrentMemo = fDesc,
        @CurrentCustom1 = Custom1,
        @CurrentCustom2 = Custom2,
        @CurrentAmount = Amount,
        @CurrentUseTax = UseTax,
        @CurrentIfPaid = IfPaid,
        @CurrentStatus = [Status]
    FROM PJ
    WHERE ID = @PJID

    DECLARE @ID int
    DECLARE @acct int
    DECLARE @job int
    DECLARE @phase smallint = 0
    DECLARE @LineTransId int = NULL
    DECLARE @line int = 0
    DECLARE @IsUseTax bit
    DECLARE @fDesc varchar(max)
    DECLARE @amount numeric(30, 2)
    DECLARE @utax numeric(30, 4)
    DECLARE @UTaxGL int
    DECLARE @totalUtax numeric(30, 2) = 0
    DECLARE @PreTotal numeric(30, 2) = 0
    DECLARE @PrvTotalAmt numeric(30, 2) = 0
    DECLARE @UtaxName varchar(25)
    DECLARE @TypeId int
    DECLARE @ItemId int
    DECLARE @ItemDesc varchar(30)
    DECLARE @MatActual numeric(30, 2) = 0
    DECLARE @Comm numeric(30, 2) = 0
    DECLARE @GLRev int = 0
    DECLARE @Ticket int = 0
    DECLARE @OpSq varchar(150) = NULL
    DECLARE @tQuan numeric(30, 2) = NULL
    DECLARE @Warehouse varchar(50) = NULL
    DECLARE @WHLocID int = NULL
    DECLARE @PhaseName varchar(100) = NULL

	DECLARE @STax bit
	DECLARE @STaxAmt numeric(30,4)  
	DECLARE @STaxGL int 
	DECLARE @GSTTaxAmt numeric(30,4)  
	DECLARE @GSTTaxGL int 
	DECLARE @STaxName varchar(50)
	DECLARE @STaxRate numeric(30,4)  
	DECLARE @GSTRate numeric(30,4)
	DECLARE @GTax bit
    DECLARE @Price numeric(30,4)

	DECLARE @IsPO int 	
	DECLARE @TypeDesc VARCHAR(150) 
	

	DECLARE @APBillslineItemsAK tblTypeAPBillslineItem 
	DECLARE @RowNo int

    BEGIN TRY

        BEGIN TRANSACTION

            DELETE FROM JobI
            WHERE TransID IN (SELECT
                    ID
                FROM Trans
                WHERE Type = 41
                AND Batch = @Batch)
                AND Type IN (1, 2)
            DELETE FROM PJItem
            WHERE TRID IN (SELECT
                    ID
                FROM Trans
                WHERE Type = 41
                AND Batch = @Batch)
            DELETE FROM Trans
            WHERE Batch = @Batch
                AND Type = 41
			UPDATE Trans SET strRef = @Ref WHERE Batch = @Batch AND Type = 40
            -- insert debit transaction of ap bill  
			DECLARE @sPJID int
			SELECT @sPJID = ID FROM PJ WHERE Batch = @Batch
			INSERT INTO tblInventoryWHTrans(InvID, WarehouseID,LocationID ,Hand,Balance,fOrder,[Committed],[Available],Screen,ScreenID,Mode,Date,TransType,Batch,fDate)
			--SELECT InvID,WarehouseID,LocationID,ISNULL(Hand,0)*-1,isnull(Balance,0)*-1,0,0,0,'APBILL',@PO,'Edit',GETDATE(),'Revert' FROM tblInventoryWHTrans WHERE ScreenID = @sPJID AND Screen = 'APBILL' 
			SELECT ItemId, Warehouse , WHLocID ,ISNULL(CAST(Quan AS NUMERIC(30,2)),0)*-1,isnull(CAST(Amount AS NUMERIC(30,2)),0)*-1,0,0,0,'APBILL',@sPJID,'Edit',GETDATE(),'Revert',@Batch,GETDATE() FROM APBillItem WHERE Batch =@Batch AND phase= 'Inventory'

            DELETE FROM tblInventoryWHTrans WHERE Batch = @Batch AND Screen= 'Project' AND [Committed] <>0
            ------------------------------- BEGIN --- ADD BOM AND NON-INVENTORY ITEMS  ------------------------------------------  
            CREATE TABLE #temp (
				RowNo int identity(1,1),
                ID int NULL,
                AcctID int NULL,
                fDesc varchar(max) NULL,
                Amount numeric(30, 2) NULL,
                UseTax numeric(30, 4) NULL,
                UtaxName varchar(25) NULL,
                UTaxGL int NULL,
                JobID int NULL,
                PhaseID int NULL,
                ItemID int NULL,
                ItemDesc varchar(150) NULL,
                TypeID int NULL,
                TypeDesc varchar(150) NULL,
                Ticket int NULL,
                OpSq varchar(150) NULL,
                Quan numeric(30, 2) NULL,
                Warehouse varchar(50),
                WHLocID int,
                PhaseName varchar(100),

				[STax] [bit] NULL,
	[STaxName] [varchar](50) NULL,
	[STaxRate] [numeric](30, 4) NULL,
	[STaxAmt] [numeric](30, 4) NULL,
	[STaxGL] [int] NULL,
	[GSTRate] [numeric](30, 4) NULL,
	[GSTTaxAmt] [numeric](30, 4) NULL,
	[GSTTaxGL] [int] NULL,
	[IsPO] [int] NULL,
	[GTax] [bit] NULL,
    [Price] [numeric](30, 4) NULL
            )

            DECLARE db_cursor1 CURSOR FOR

            SELECT
                ID,
                AcctID,
                fDesc,
                Amount,
                UseTax,
                JobID,
                PhaseID,
                ItemID,
                UtaxName,
                UTaxGL,
                TypeID,
                ItemDesc,
                @Ticket,
                OpSq,
                Quan,
                Warehouse,
                WHLocID,
                Phase,
				STax,STaxName,STaxRate,STaxAmt,STaxGL,GSTRate,GSTTaxAmt,GSTTaxGL,GTax,Price ,TypeDesc,IsPO
            FROM @APBillslineItems

            OPEN db_cursor1
            FETCH NEXT FROM db_cursor1 INTO
            @ID, @acct, @fDesc, @amount, @utax, @job, @phase, @ItemId, @UtaxName, @UTaxGL, @TypeId, @ItemDesc, @Ticket, @OpSq, @tQuan, @Warehouse, @WHLocID, @PhaseName,@STax,@STaxName,@STaxRate,@STaxAmt,@STaxGL,@GSTRate,@GSTTaxAmt,@GSTTaxGL,@GTax,@Price,@TypeDesc,@IsPO

            WHILE @@FETCH_STATUS = 0
            BEGIN

                -- if(@job is not null  and (@TypeId = 1 or @TypeId = 2) and (@phase is null))  
                --IF (@job IS NOT NULL)-- and @phase is not null) -- and (@TypeID =1 or @TypeID =2 )  
				IF (@job IS NOT NULL OR ISNULL(@job,0)=0)
                BEGIN
					if @job is NULL
					BEGIN
						SET @job = 0
					END
                    IF (@ItemID IS NOT NULL
                        AND @ItemId != ''
                        AND @ItemId != 0)
                    BEGIN
                        -- add into inv table  
                        --IF (@ID = 0
                        --    OR @ID IS NULL)
                            EXEC @phase = spAddBOMItem @job,
                                                       @TypeId,
                                                       @ItemId,
                                                       @fDesc,
                                                       @phase,
                                                       @OpSq
                    END


                    if exists ( select 1 from inv where id=@ItemId and Type=0)
                    BEGIN
				        --------------------$$$$  Commmited $$$$$$
                        

                        if (@tQuan <> 0)
                        BEGIN
                            Declare @Committed numeric(32,2)=0;
                        
	                        select  @Committed = SUM(isnull(Committed,0)) from tblInventoryWHTrans where Screen='Project' and InvID=@ItemID and ScreenID=@job and WarehouseID='OFC'
	  
	                        if( @tQuan <= @Committed) set @Committed= (@tQuan * -1 );
	 
	                        else if( @Committed <= 0) set @Committed=0;	
    
	                        else   set @Committed= @Committed * -1  ;

	                        if(@Committed <> 0  )
		                    BEGIN
	 	                        INSERT INTO tblInventoryWHTrans(InvID, WarehouseID,LocationID ,Hand,Balance,fOrder,[Committed],[Available],Screen,ScreenID,Mode,Date,TransType,Batch,FDate)
                                VALUES( @ItemId,'OFC',0,0,0,0,@Committed,0,'Project',@job,'Edit',GETDATE(),'out',@Batch,GETDATE())
		                    END
                        END
		            --------------------$$$$  Commmited $$$$$$
                    END



                    ELSE
                    --IF (@ItemID IS NULL
                    --    OR @ItemId = '')
                    BEGIN
                        -- add into inv table (as non inventory type) and add as bom item  
                        --if exists (select top 1 1 from inv where Name = @ItemDesc and fDesc = @fDesc) -- check if item name and description is already exists!  
                        IF EXISTS (SELECT TOP 1 1 FROM inv WHERE Name = @ItemDesc) -- check if item name and description is already exists!  
                        BEGIN
                            --set @ItemId = (select top 1 ID from inv where Name = @ItemDesc and fDesc = @fDesc and type = 2)
                            SET @ItemId = (SELECT TOP 1
                                ID
                            FROM inv
                            WHERE Name = @ItemDesc
                            AND type = 2)
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
									-- IF(@ID =0 or @ID IS NULL)      
									EXEC @phase = spAddBOMItem @job,
															   @TypeId,
															   @ItemId,
															   @fDesc,
															   @OPhase,
															   @OpSq
								END
								ELSE
								BEGIN

									-- IF(@ID =0 or @ID IS NULL)      
									EXEC @phase = spAddBOMItem @job,
															   @TypeId,
															   @ItemId,
															   @fDesc,
															   @phase,
															   @OpSq
								END
							END
							ELSE
							BEGIN
								EXEC @phase = spAddBOMItem @job,
															   @TypeId,
															   @ItemId,
															   @fDesc,
															   @phase,
															   @OpSq
							END
                        END
                        ELSE
                        BEGIN
                            IF @ItemDesc IS NOT NULL
                                AND @ItemDesc != ''
                            BEGIN
                                SET @GLRev = ISNULL((SELECT
                                    SAcct
                                FROM Job job
                                INNER JOIN Inv inv
                                    ON job.GLRev = inv.ID
                                WHERE job.ID = @job)
                                , 0)
                                INSERT INTO Inv (Name, fdesc, Cat, Balance, Measure, Tax, AllowZero, InUse, Type, Sacct, Status, Price1)
                                    VALUES (@ItemDesc, @fDesc, 0, 0, 'Each', 0, 0, 0, 2, @GLRev, 0, 0)
                                SET @ItemId = SCOPE_IDENTITY()

                                --	IF(@ID =0 or @ID IS NULL)      
                                EXEC @phase = spAddBOMItem @job,
                                                           @TypeId,
                                                           @ItemId,
                                                           @fDesc,
                                                           @phase,
                                                           @OpSq
                            END
                            ELSE
                            BEGIN
                                --	IF(@ID =0 or @ID IS NULL)      
                                EXEC @phase = spAddBOMItem @job,
                                                           @TypeId,
                                                           @ItemId,
                                                           @fDesc,
                                                           @phase,
                                                           @OpSq
                            END
                        END
                    END
                END
				/*
                ELSE
                IF (@job = 0
                    AND @phase = 0)  --and (@TypeID =8 )  
                BEGIN
                    IF (@ItemID IS NOT NULL)
                    BEGIN
                        -- add into inv table  
                        --	IF(@ID =0 or @ID IS NULL)      
                        EXEC @phase = spAddBOMItem @Job,
                                                   @TypeId,
                                                   @ItemId,
                                                   @fDesc,
                                                   @phase,
                                                   @OpSq

                    END
                    ELSE
                    IF (@ItemID IS NULL)
                    BEGIN
                        -- add into inv table (as non inventory type) and add as bom item  

                        --if exists (select top 1 1 from inv where Name = @ItemDesc and fDesc = @fDesc) -- check if item name and description is already exists!  
                        IF EXISTS (SELECT TOP 1
                                1
                            FROM inv
                            WHERE Name = @ItemDesc) -- check if item name and description is already exists!  
                        BEGIN
                            --set @ItemId = (select top 1 ID from inv where Name = @ItemDesc and fDesc = @fDesc and type = 2)  
                            SET @ItemId = (SELECT TOP 1
                                ID
                            FROM inv
                            WHERE Name = @ItemDesc
                            AND type = 2)
                            -- IF(@ID =0 or @ID IS NULL)      
                            EXEC @phase = spAddBOMItem @Job,
                                                       @TypeId,
                                                       @ItemId,
                                                       @fDesc,
                                                       @phase,
                                                       @OpSq

                        END
                        ELSE
                        BEGIN
                            IF @ItemDesc IS NOT NULL
                                AND @ItemDesc != ''
                            BEGIN
                                SET @GLRev = ISNULL((SELECT
                                    SAcct
                                FROM Job job
                                INNER JOIN Inv inv
                                    ON job.GLRev = inv.ID
                                WHERE job.ID = @Job)
                                , 0)
                                INSERT INTO Inv (Name, fdesc, Cat, Balance, Measure, Tax, AllowZero, InUse, Type, Sacct, Status, Price1)
                                    VALUES (@ItemDesc, @fDesc, 0, 0, 'Each', 0, 0, 0, 2, @GLRev, 0, 0)
                                SET @ItemId = SCOPE_IDENTITY()

                                -- IF(@ID =0 or @ID IS NULL)      
                                EXEC @phase = spAddBOMItem @Job,
                                                           @TypeId,
                                                           @ItemId,
                                                           @fDesc,
                                                           @phase,
                                                           @OpSq
                            END
                        END

                    END

                END
				*/
                INSERT INTO #temp (ID, AcctID, fDesc, Amount, UseTax, JobID, PhaseID, ItemID, UtaxName, UTaxGL, TypeID, ItemDesc, Ticket, OpSq, Quan, Warehouse, WHLocID, PhaseName,STax,STaxName,STaxRate,STaxAmt,STaxGL,GSTRate,GSTTaxAmt,GSTTaxGL,GTax,Price,TypeDesc,IsPO)
                    VALUES (NULL, @acct, @fDesc, @amount, @utax, @job, @phase, @ItemId, @UtaxName, @UTaxGL, @TypeId, @ItemDesc, @Ticket, @OpSq, @tQuan, @Warehouse, @WHLocID, @PhaseName,@STax,@STaxName,@STaxRate,@STaxAmt,@STaxGL,@GSTRate,@GSTTaxAmt,@GSTTaxGL,@GTax,@Price,@TypeDesc,@IsPO)

                --------------->

                SET @ID = NULL;
                SET @acct = NULL;
                SET @fDesc = NULL;
                SET @amount = NULL;
                SET @utax = NULL;
                SET @job = NULL;
                SET @phase = NULL;
                SET @ItemId = NULL;
                SET @UtaxName = NULL;
                SET @UTaxGL = NULL;
                SET @TypeId = NULL;
                SET @ItemDesc = NULL;
                SET @Ticket = NULL;
                SET @OpSq = NULL;
                SET @tQuan = NULL;
                SET @Warehouse = NULL;
                SET @WHLocID = NULL;
                SET @PhaseName = NULL;
				SET @STax =null;
		         SET @STaxName=null;
		         SET @STaxRate =null;
		         SET @STaxAmt=null;
		         SET @STaxGL =null;
		         SET @GSTRate =null;
		         SET @GSTTaxAmt =null;
		         SET @GSTTaxGL=null;
		         SET @GTax=null;
                 SET @Price=null;
                --------------->


                FETCH NEXT FROM db_cursor1 INTO
                @ID, @acct, @fDesc, @amount, @utax, @job, @phase, @ItemId, @UtaxName, @UTaxGL, @TypeId, @ItemDesc, @Ticket, @OpSq, @tQuan, @Warehouse, @WHLocID, @PhaseName,@STax,@STaxName,@STaxRate,@STaxAmt,@STaxGL,@GSTRate,@GSTTaxAmt,@GSTTaxGL,@GTax,@Price,@TypeDesc,@IsPO
            END

            CLOSE db_cursor1
            DEALLOCATE db_cursor1

            -------------------------------END --- ADD BOM AND NON-INVENTORY ITEMS------------------------------------------  


            SELECT
                --@PreTotal = SUM(Amount)
				@PreTotal = SUM(Amount)+SUM(ISNULL(STaxAmt,0))+SUM(ISNULL(GSTTaxAmt,0))
            FROM @APBillslineItems
            SET @amount = @PreTotal * -1
            UPDATE Trans
            SET fDate = @PostingDate,
                fDesc = @Memo,
                Amount = @amount,
                AcctSub = @Vendor
            WHERE ID = @TransId


            DECLARE db_cursor2 CURSOR FOR

            SELECT
			RowNo,
                AcctID,
                fDesc,
                Amount,
                UseTax,
                JobID,
                PhaseID,
                ItemID,
                UtaxName,
                UTaxGL,
                Ticket,
                OpSq,
                TypeID,
                Quan,
				STax,STaxName,STaxRate,STaxAmt,STaxGL,GSTRate,GSTTaxAmt,GSTTaxGL,Warehouse,WHLocID,PhaseName,GTax,Price
            FROM #temp

            OPEN db_cursor2


            FETCH NEXT FROM db_cursor2 INTO @RowNo,@acct, @fDesc, @amount, @utax, @job, @phase, @ItemId, @UtaxName, @UTaxGL, @Ticket, @OpSq, @TypeID, @tQuan,@STax,@STaxName,@STaxRate,@STaxAmt,@STaxGL,@GSTRate,@GSTTaxAmt,@GSTTaxGL,@Warehouse,@WHLocID,@PhaseName,@GTax,@Price

            WHILE @@FETCH_STATUS = 0
            BEGIN
                IF (@job = 0)
                BEGIN
                    SET @job = NULL
                END
                IF (@phase = 0)
                BEGIN
                    SET @phase = NULL
                END

                SET @IsUseTax = 0
                DECLARE @tUtaxAmt numeric(30, 2)

                IF (@utax > 0)
                BEGIN
                    SET @IsUseTax = 1
                    SET @tUtaxAmt = (@amount * @utax) / 100
                    SET @fDesc = @fDesc + ' (Amount Before Use Tax - $' + CONVERT(varchar, CAST(CAST(ISNULL(@amount, 0) AS decimal) AS money), 1) + ')'

                    SET @amount = @amount + @tUtaxAmt
                    SET @totalUtax = @totalUtax + @tUtaxAmt
                END
		--		IF(@STax = 1 AND ISNULL(@STaxAmt,0) <> 0)
		--BEGIN
		--	SET @amount = @amount + @STaxAmt
		--END
		--IF(@STax = 1 AND ISNULL(@GSTTaxAmt,0) <> 0)
		--BEGIN
		--	SET @amount = @amount + @GSTTaxAmt
		--END

		

                EXEC @LineTransId = [dbo].[AddJournal] NULL,
                                                       @Batch,
                                                       @PostingDate,
                                                       41,
                                                       @line,
                                                       0,
                                                       @fDesc,
                                                       @amount,
                                                       @acct,
                                                       @ItemId,
                                                       @tQuan,
                                                       0,
                                                       @job,
                                                       @phase,
                                                       0,
                                                       @Ref

				UPDATE #temp SET ID =  @LineTransId WHERE RowNo = @RowNo
				IF @PhaseName = 'Inventory'
					BEGIN				
						INSERT INTO [dbo].[PJItem]
						([TRID]
						,WarehouseID
						,LocationID )
						VALUES
						(@LineTransId
						,@Warehouse,@WHLocID)
					END




		--				IF(@STax = 1 AND ISNULL(@STaxAmt,0) <> 0)
		--BEGIN
		--	SET @amount = @amount + @STaxAmt
		--END
		----IF(@STax = 1 AND ISNULL(@GSTTaxAmt,0) <> 0)
		--IF(@GTax = 1 AND ISNULL(@GSTTaxAmt,0) <> 0)
		--BEGIN
		--	SET @amount = @amount + @GSTTaxAmt
		--END

                IF @job IS NOT NULL
                BEGIN

                    INSERT INTO [dbo].[JobI] ([Job], [Phase], [fDate], [Ref], [fDesc], [Amount], [TransID], [Type], [UseTax], [APTicket])
                        VALUES (@job, @phase, @PostingDate, @Ref, @fDesc, @amount, @LineTransId, 1, @IsUseTax, @Ticket)



                    IF @phase IS NOT NULL
                    BEGIN
                        SET @Comm = ISNULL((SELECT
                            SUM(ISNULL(p.Balance, 0))
                        FROM POItem p
                        INNER JOIN PO
                            ON p.po = po.po
                        WHERE p.Job = @Job
                        AND p.Phase = @phase
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
                        AND p.Phase = @phase
                        AND r.status = 0)
                        , 0)




                        SET @MatActual = ISNULL((SELECT
                            SUM(ISNULL(amount, 0))
                        FROM jobi
                        WHERE type = 1
                        AND job = @job
                        AND phase = @phase
                        AND (TransID > 0
                        OR ISNULL(Labor, 0) = 0))
                        , 0)

                        UPDATE JobTItem
                        SET Actual = @MatActual,
                            Comm = @Comm
                        WHERE Type = 1
                        AND Job = @job
                        AND Line = @phase
                        AND Code = @OpSq


                    END

                END


                SET @line = @line + 1
                IF (@IsUseTax = 1)
                BEGIN
				
                    --SET @fDesc = 'Use Tax Payable'
					SET @fDesc = @UtaxName+' Payable'
                    SET @amount = @tUtaxAmt * -1

                    EXEC [dbo].[AddJournal] NULL,
                                            @Batch,
                                            @PostingDate,
                                            41,
                                            @line,
                                            0,
                                            @fDesc,
                                            @amount,
                                            @UTaxGL,
                                            NULL,
                                            NULL,
                                            0,
                                            @job,
                                            NULL,
                                            0,
                                            @Ref

                    --INSERT INTO [dbo].[PJItem] ([TRID], [Stax], [Amount], [UseTax])
                        --VALUES (@LineTransId, @UtaxName, @tUtaxAmt, @utax)
					INSERT INTO [dbo].[PJItem]
				   ([TRID] ,[Stax] ,[Amount]  ,[UseTax]   ,TaxType)
			 VALUES  (@LineTransId  ,@UtaxName  ,@tUtaxAmt  ,@utax  ,1)
                    SET @line = @line + 1

                END

				SET @line = @line + 1
		IF(@STax = 1 AND ISNULL(@STaxAmt,0) <> 0)
		BEGIN
		
			--SET @fDesc = 'Sales Tax Payable'
			SET @fDesc = @STaxName+' Payable'
			--SET @amount = @STaxAmt * -1
			SET @amount = @STaxAmt
			EXEC [dbo].[AddJournal] null,@Batch,@PostingDate,41,@line,0,@fDesc,@amount,@STaxGL,null,null,0,@job,null,0,@Ref
			
			INSERT INTO [dbo].[PJItem]
				   ([TRID]
				   ,[Stax]
				   ,[Amount]
				   ,[UseTax]
				   ,TaxType)
			 VALUES
				   (@LineTransId
				   ,@STaxName
				   ,@STaxAmt
				   ,@STaxRate
				   ,0)
			SET @line = @line + 1
		
		END

		print ISNULL(CONVERT(int,@GTax),0)
		print @GSTTaxAmt
		SET @line = @line + 1
		--IF(@STax = 1 AND ISNULL(@GSTTaxAmt,0) <> 0)
		IF(@GTax = 1 AND ISNULL(@GSTTaxAmt,0) <> 0)		
		BEGIN
		
			SET @fDesc = 'GST Payable'
			--SET @amount = @GSTTaxAmt * -1
			SET @amount = @GSTTaxAmt
			EXEC [dbo].[AddJournal] null,@Batch,@PostingDate,41,@line,0,@fDesc,@amount,@GSTTaxGL,null,null,0,@job,null,0,@Ref
			
			INSERT INTO [dbo].[PJItem]
				   ([TRID]
				   ,[Stax]
				   ,[Amount]
				   ,[UseTax]
				   ,TaxType)
			 VALUES
				   (@LineTransId
				   ,'GST'
				   ,@GSTTaxAmt
				   ,@GSTRate
				   ,2)
			SET @line = @line + 1
		
		END



                ----RESET----->
				SET @RowNo = NULL;
                SET @acct = NULL;
                SET @fDesc = NULL;
                SET @amount = NULL;
                SET @utax = NULL;
                SET @job = NULL;
                SET @phase = NULL;
                SET @ItemId = NULL;
                SET @UtaxName = NULL;
                SET @UTaxGL = NULL;
                SET @Ticket = NULL;
                SET @OpSq = NULL;
                SET @TypeID = NULL;
                SET @tQuan = NULL;
				SET @STax= NULL;
				SET @STaxName= NULL;
				SET @STaxRate= NULL;
				SET @STaxAmt= NULL;
				SET @STaxGL= NULL;
				SET @GSTRate= NULL;
				SET @GSTTaxAmt= NULL;
				SET @GSTTaxGL= NULL;
				SET @Warehouse= NULL;
				SET @WHLocID = NULL; 
				SET @PhaseName= NULL;
				SET @GTax= NULL;
                SET @Price= NULL;
                --------->

                FETCH NEXT FROM db_cursor2 INTO @RowNo,@acct, @fDesc, @amount, @utax, @job, @phase, @ItemId, @UtaxName, @UTaxGL, @Ticket, @OpSq, @TypeID, @tQuan,@STax,@STaxName,@STaxRate,@STaxAmt,@STaxGL,@GSTRate,@GSTTaxAmt,@GSTTaxGL,@Warehouse,@WHLocID,@PhaseName,@GTax,@Price
            END

            CLOSE db_cursor2
            DEALLOCATE db_cursor2

            -- CREDIT TRANSACTION ------------------------------------------------------------------  

            SELECT
                @PrvTotalAmt = Amount
            FROM PJ
            WHERE ID = @PJID


            UPDATE [dbo].[PJ]
            SET [ID] = @PJID,
                [fDate] = @PostingDate,
                [Ref] = @Ref,
                [fDesc] = @Memo,
                [Amount] = @PreTotal,
                [Vendor] = @Vendor,
                --[Status] = @Status,
                [Terms] = @DueIn,
                [PO] = @PO,
                [Spec] = @Status,				
                [IDate] = @Date,
                [UseTax] = @totalUtax,
                [Disc] = @Disc,
                [Custom1] = @Custom1,
                [Custom2] = @Custom2,
                [ReceivePO] = @ReceivePo,
                [IfPaid] = @IfPaid,
                STax = @PJSTax ,
                STaxName= @PJSTaxName,
                STaxGL =@PJSTaxGL,
                STaxRate =@PJSTaxRate,
                UTax= @PJUTax,
                UTaxName =@PJUTaxName,
                UTaxGL = @PJUTaxGL,
                UTaxRate =@PJUTaxRate,
                GST =@PJGST,
                GSTGL =@PJGSTGL,
                GSTRate =@PJGSTRate


            WHERE ID = @PJID


            UPDATE [dbo].[OpenAP]
            SET [Vendor] = @Vendor,
                [fDate] = @Date,
                [Due] = @Due,
                [fDesc] = CONVERT(varchar(255), @Memo),
                [Original] = @PreTotal,
                [Balance] = @PreTotal,
                [Disc] = 0,
                [Ref] = @Ref
            WHERE PJID = @PJID


            --UPDATE v  
            --SET Balance = ((SELECT isnull(Balance,0) as Balance FROM Vendor WHERE ID = @Vendor) + @PrvTotalAmt)  
            --FROM Vendor v   
            -- WHERE v.ID = @Vendor  


            --UPDATE v  
            --SET Balance = ((SELECT isnull(Balance,0) as Balance FROM Vendor WHERE ID = @Vendor) - @PreTotal)  
            --FROM Vendor v   
            -- WHERE v.ID = @Vendor  

            DECLARE db_cursor3 CURSOR FOR   --------- BEGIN UPDATE JOB COST OF JOB ----------------  

            SELECT
                JobID
            FROM #temp
            GROUP BY JobID

            OPEN db_cursor3
            FETCH NEXT FROM db_cursor3 INTO @job

            WHILE @@FETCH_STATUS = 0
            BEGIN
                IF ISNULL(@job,0) <> 0
                BEGIN
                    EXEC spUpdateJobMatExp @job
    
                    EXEC spUpdateJobOtherExp @job

                    EXEC spUpdateJobcostByJob @job
                END
                SET @job = NULL;

                FETCH NEXT FROM db_cursor3 INTO @job
            END

            CLOSE db_cursor3
            DEALLOCATE db_cursor3     --------- END UPDATE JOB COST OF JOB ------------------  



            ---------------$$$$ ---  Inventory  Adjustment For AP Bills $$$ ---------------------- 


            ---------------$$$$ Step 1 REVERT Inventory Adjustment  $$$ --------------------

            --UPDATE IWH
            --SET IWH.HAND = IWH.HAND + (I.QUAN * -1),
            --    IWH.BALANCE = IWH.BALANCE + (I.AMOUNT * -1)
            --FROM IADJ I
            --INNER JOIN IWAREHOUSELOCADJ IWH
            --    ON I.ITEM = IWH.INVID
            --    AND I.WAREHOUSEID = IWH.WAREHOUSEID
            --WHERE BATCH = @Batch
            --AND ISNULL(I.LOCATIONID, 0) = 0

            --UPDATE IWH
            --SET IWH.HAND = IWH.HAND + (I.QUAN * -1),
            --    IWH.BALANCE = IWH.BALANCE + (I.AMOUNT * -1)
            --FROM IADJ I
            --INNER JOIN IWAREHOUSELOCADJ IWH
            --    ON I.ITEM = IWH.INVID
            --    AND I.WAREHOUSEID = IWH.WAREHOUSEID
            --    AND I.LOCATIONID = IWH.LOCATIONID
            --WHERE BATCH = @Batch
            --AND ISNULL(I.LOCATIONID, 0) <> 0

            --DELETE FROM IADJ
            --WHERE Batch = @Batch

            --UPDATE I
            --SET i.hand = (SELECT
            --        ISNULL(SUM(ISNULL(Adj.Hand, 0)), 0)
            --    FROM IWarehouseLocAdj adj
            --    WHERE adj.InvID = I.ID),
            --    I.Balance = (SELECT
            --        ISNULL(SUM(ISNULL(Adj.Balance, 0)), 0)
            --    FROM IWarehouseLocAdj adj
            --    WHERE adj.InvID = I.ID),
            --    I.Available = (SELECT
            --        ISNULL(SUM(ISNULL(Adj.Available, 0)), 0)
            --    FROM IWarehouseLocAdj adj
            --    WHERE adj.InvID = I.ID),
            --    I.Committed = (SELECT
            --        ISNULL(SUM(ISNULL(Adj.Committed, 0)), 0)
            --    FROM IWarehouseLocAdj adj
            --    WHERE adj.InvID = I.ID),
            --    I.LastUpdateDate = GETDATE()
            --FROM INV I
            --WHERE i.Type = 0
            ------------------------------END------------------------------------


            ---$$$ Step 2  Add new line items


            ----RESET----->

            SET @acct = NULL;
            SET @fDesc = NULL;
            SET @amount = NULL;
            SET @utax = NULL;
            SET @job = NULL;
            SET @phase = NULL;
            SET @ItemId = NULL;
            SET @UtaxName = NULL;
            SET @UTaxGL = NULL;
            SET @Ticket = NULL;
            SET @OpSq = NULL;
            SET @TypeID = NULL;
            SET @tQuan = NULL;
            SET @Warehouse = NULL;
            SET @WHLocID = NULL;
            SET @PhaseName = NULL;
			SET @STax= NULL;
				SET @STaxName= NULL;
				SET @STaxRate= NULL;
				SET @STaxAmt= NULL;
				SET @STaxGL= NULL;
				SET @GSTRate= NULL;
				SET @GSTTaxAmt= NULL;
				SET @GSTTaxGL= NULL;
				SET @GTax = NULL;
                SET @Price = NULL;
            --------------->

            DECLARE db_cursorINV CURSOR FOR
            SELECT
                AcctID,
                fDesc,
                Amount,
                UseTax,
                JobID,
                PhaseID,
                ItemID,
                UtaxName,
                UTaxGL,
                Ticket,
                OpSq,
                TypeID,
                Quan,
                Warehouse,
                WHLocID,
                PhaseName,
				STax,STaxName,STaxRate,STaxAmt,STaxGL,GSTRate,GSTTaxAmt,GSTTaxGL,GTax,Price
            FROM #temp
            OPEN db_cursorINV

            FETCH NEXT FROM db_cursorINV
            INTO @acct, @fDesc, @amount, @utax, @job, @phase, @ItemId, @UtaxName, @UTaxGL, @Ticket, @OpSq, @TypeID, @tQuan, @Warehouse, @WHLocID, @PhaseName,@STax,@STaxName,@STaxRate,@STaxAmt,@STaxGL,@GSTRate,@GSTTaxAmt,@GSTTaxGL,@GTax,@Price

            WHILE @@FETCH_STATUS = 0
            BEGIN  --- While 	


                IF (EXISTS (SELECT
                        1
                    FROM Inv
                    WHERE ID = @ItemId
                    AND type = 0)
                    )
                    AND (ISNULL(@PhaseName, '') = 'Inventory')
                BEGIN

                    ---UPDATE LCOST PRICE -- INV TABLE
                    DECLARE @lcost numeric(30, 2) = 0;
                    SELECT
                        @lcost = (ISNULL(@amount, 0) / ISNULL(@tQuan, 0));
                    IF (@lcost > 0)
                        UPDATE Inv
                        SET LCost = (@lcost),
                            LVendor = @Vendor
                        WHERE ID = @ItemId
                        AND type = 0


                    -----IF WAREHOUSE AND LOCATION BOTH  SELECTED
                    IF ((ISNULL(@Warehouse, '') <> '')
                        AND (ISNULL(@WHLocID, 0) <> 0))
                    BEGIN

                        IF NOT EXISTS (SELECT
                                1
                            FROM InvWarehouse i
                            WHERE i.InvID = @ItemId
                            AND i.WarehouseID = @Warehouse)

                        BEGIN
                            INSERT INTO InvWarehouse (InvID, WarehouseID)
                                VALUES (@ItemId, @Warehouse)
                        END

                        IF NOT EXISTS (SELECT
                                1
                            FROM IWarehouseLocAdj i
                            WHERE i.InvID = @ItemId
                            AND i.WarehouseID = @Warehouse
                            AND i.LocationID = @WHLocID)

                        BEGIN
                            INSERT INTO IWarehouseLocAdj (InvID, WarehouseID, LocationID, Hand, Balance, fOrder, [Committed], Available)
                                VALUES (@ItemId, @Warehouse, @WHLocID, 0, 0, 0, 0, 0)
                        END
                        --UPDATE i
                        --SET i.Hand = ISNULL(i.Hand, 0) + ISNULL(@tQuan, 0),
                        --    i.Balance = ISNULL(i.Balance, 0) + ISNULL(@amount, 0)
                        --FROM IWarehouseLocAdj i
                        --WHERE i.InvID = @ItemId
                        --AND i.WarehouseID = @Warehouse
                        --AND i.LocationID = @WHLocID

                        ----- Calculate Available 
                        --UPDATE i
                        --SET i.Available = i.Hand + i.fOrder - i.Committed
                        --FROM IWarehouseLocAdj i
                        --WHERE i.InvID = @ItemId
                        --AND i.WarehouseID = @Warehouse
                        --AND i.LocationID = @WHLocID

						INSERT INTO tblInventoryWHTrans(InvID, WarehouseID,LocationID ,Hand,Balance,fOrder,[Committed],[Available],Screen,ScreenID,Mode,Date,TransType,Batch,fdate)
						VALUES (@ItemId, @Warehouse , @WHLocID ,ISNULL(@tQuan,0),isnull(@amount,0),0,0,0,'APBILL',@PJID,'Edit',GETDATE(),'In',@Batch,GETDATE())

                    END
                    -------IF WAREHOUSE SELECT AND LOCATION DOES NOT SELECTED
                    ELSE
                    BEGIN

                        IF NOT EXISTS (SELECT
                                1
                            FROM InvWarehouse i
                            WHERE i.InvID = @ItemId
                            AND i.WarehouseID = @Warehouse)

                        BEGIN
                            INSERT INTO InvWarehouse (InvID, WarehouseID)
                                VALUES (@ItemId, @Warehouse)
                        END

                        IF NOT EXISTS (SELECT
                                1
                            FROM IWarehouseLocAdj i
                            WHERE i.InvID = @ItemId
                            AND i.WarehouseID = @Warehouse
                            AND ISNULL(i.LocationID, 0) = 0)

                        BEGIN
                            INSERT INTO IWarehouseLocAdj (InvID, WarehouseID, LocationID, Hand, Balance, fOrder, [Committed], Available)
                                VALUES (@ItemId, @Warehouse, NULL, 0, 0, 0, 0, 0)
                        END

                        --UPDATE i
                        --SET i.Hand = ISNULL(i.Hand, 0) + ISNULL(@tQuan, 0),
                        --    i.Balance = ISNULL(i.Balance, 0) + ISNULL(@amount, 0)
                        --FROM IWarehouseLocAdj i
                        --WHERE i.InvID = @ItemId
                        --AND i.WarehouseID = @Warehouse
                        --AND ISNULL(i.LocationID, 0) = 0

                        ----- Calculate Available 
                        --UPDATE i
                        --SET i.Available = i.Hand + i.fOrder - i.Committed
                        --FROM IWarehouseLocAdj i
                        --WHERE i.InvID = @ItemId
                        --AND i.WarehouseID = @Warehouse
                        --AND ISNULL(i.LocationID, 0) = 0

						INSERT INTO tblInventoryWHTrans(InvID, WarehouseID,LocationID ,Hand,Balance,fOrder,[Committed],[Available],Screen,ScreenID,Mode,Date,TransType,Batch,fdate)
						VALUES (@ItemId, @Warehouse , @WHLocID ,ISNULL(@tQuan,0),isnull(@amount,0),0,0,0,'APBILL',@PJID,'Edit',GETDATE(),'In',@Batch,GETDATE())
                    END

                    --------------- INV Item Adjustment ------------------>

                    --EXECUTE [dbo].[spCreateInvAdjustments] @fdate = @Date,
                    --                                       @fDesc = 'AP Bills',
                    --                                       @Quan = @tQuan,
                    --                                       @Amount = @amount,
                    --                                       @Item = @ItemId,
                    --                                       @Batch = @Batch,
                    --                                       @TransID = @TransId,
                    --                                       @Acct = @acct,
                    --                                       @WarehouseID = @Warehouse,
                    --                                       @locationID = @WHLocID,
                    --                                       @type = 1



                    ----RESET-----> 
                    SET @acct = NULL;
                    SET @fDesc = NULL;
                    SET @amount = NULL;
                    SET @utax = NULL;
                    SET @job = NULL;
                    SET @phase = NULL;
                    SET @ItemId = NULL;
                    SET @UtaxName = NULL;
                    SET @UTaxGL = NULL;
                    SET @Ticket = NULL;
                    SET @OpSq = NULL;
                    SET @TypeID = NULL;
                    SET @tQuan = NULL;
                    SET @Warehouse = NULL;
                    SET @WHLocID = NULL;
                    SET @PhaseName = NULL;
					SET @STax= NULL;
				SET @STaxName= NULL;
				SET @STaxRate= NULL;
				SET @STaxAmt= NULL;
				SET @STaxGL= NULL;
				SET @GSTRate= NULL;
				SET @GSTTaxAmt= NULL;
				SET @GSTTaxGL= NULL;
				SET @GTax= NULL;
                SET @Price= NULL;
                --------------->
                END

                FETCH NEXT FROM db_cursorINV
                INTO @acct, @fDesc, @amount, @utax, @job, @phase, @ItemId, @UtaxName, @UTaxGL, @Ticket, @OpSq, @TypeID, @tQuan, @Warehouse, @WHLocID, @PhaseName,@STax,@STaxName,@STaxRate,@STaxAmt,@STaxGL,@GSTRate,@GSTTaxAmt,@GSTTaxGL,@GTax,@Price


            END --- While 

            CLOSE db_cursorINV

            DEALLOCATE db_cursorINV

            --UPDATE I
            --SET i.hand = (SELECT
            --        ISNULL(SUM(ISNULL(Adj.Hand, 0)), 0)
            --    FROM IWarehouseLocAdj adj
            --    WHERE adj.InvID = I.ID),
            --    I.Balance = (SELECT
            --        ISNULL(SUM(ISNULL(Adj.Balance, 0)), 0)
            --    FROM IWarehouseLocAdj adj
            --    WHERE adj.InvID = I.ID),
            --    I.Available = (SELECT
            --        ISNULL(SUM(ISNULL(Adj.Available, 0)), 0)
            --    FROM IWarehouseLocAdj adj
            --    WHERE adj.InvID = I.ID),
            --    I.Committed = (SELECT
            --        ISNULL(SUM(ISNULL(Adj.Committed, 0)), 0)
            --    FROM IWarehouseLocAdj adj
            --    WHERE adj.InvID = I.ID),
            --    I.LastUpdateDate = GETDATE()
            --FROM INV I
            --WHERE i.Type = 0

            --------------- END INV  Adjustment ------------------>
            IF(@ReceivePo > 0 and @PO > 0)
		    BEGIN  
			    IF (@IsPOClose = 1)		------------------------ PO CLOSE --------------------------
			    BEGIN
				    EXEC spClosePO @PO, @UpdatedBy
				END
		    END
            EXEC [dbo].[spUpdateVendorBalance] @Vendor

            EXEC [dbo].[spCalChartBalance]

			insert into @APBillslineItemsAK SELECT ID ,AcctID ,fDesc,Amount,UseTax,UtaxName,JobID,	PhaseID,ItemID,PhaseName,UTaxGL,ItemDesc,TypeID,TypeDesc,Quan,Ticket,OpSq,Warehouse,WHLocID,
	STax,STaxName,STaxRate,STaxAmt,STaxGL,GSTRate,GSTTaxAmt,GSTTaxGL,IsPO,GTax,Price FROM #temp

			EXEC [dbo].[spAddApBillItems] @Batch,@APBillslineItemsAK
			EXEC CalculateInventory
            DROP TABLE #temp
            /********Start Logs************/
            DECLARE @Val varchar(1000)
            IF (@Vendor IS NOT NULL
                AND @Vendor != 0)
            BEGIN
                SET @Val = (SELECT TOP 1
                    newVal
                FROM log2
                WHERE screen = 'Bills'
                AND ref = @PJID
                AND Field = 'Vendor'
                ORDER BY CreatedStamp DESC)
                DECLARE @CurrentVendorName varchar(150)
                SELECT
                    @CurrentVendorName = r.Name
                FROM Rol r
                INNER JOIN Vendor V
                    ON V.Rol = r.ID
                WHERE V.ID = @Vendor
                IF (@Val <> @CurrentVendorName)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'Bills',
                                     @PJID,
                                     'Vendor',
                                     @Val,
                                     @CurrentVendorName
                END
                ELSE
                IF (@CurrentVendor <> @CurrentVendorName)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'Bills',
                                     @PJID,
                                     'Vendor',
                                     @CurrentVendor,
                                     @CurrentVendorName
                END
            END
            SET @Val = NULL
            IF (@PO IS NOT NULL
                AND @PO != 0)
            BEGIN
                SET @Val = (SELECT TOP 1
                    newVal
                FROM log2
                WHERE screen = 'Bills'
                AND ref = @PJID
                AND Field = 'PO #'
                ORDER BY CreatedStamp DESC)
                IF (@Val <> CONVERT(varchar(50), @PO))
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'Bills',
                                     @PJID,
                                     'PO #',
                                     @Val,
                                     @PO
                END
                ELSE
                IF (@CurrentPO <> CONVERT(varchar(50), @PO))
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'Bills',
                                     @PJID,
                                     'PO #',
                                     @CurrentPO,
                                     @PO
                END
            END
            SET @Val = NULL
            IF (@ReceivePo IS NOT NULL
                AND @ReceivePo != 0)
            BEGIN
                SET @Val = (SELECT TOP 1
                    newVal
                FROM log2
                WHERE screen = 'Bills'
                AND ref = @PJID
                AND Field = 'Reception No#'
                ORDER BY CreatedStamp DESC)
                IF (@Val <> CONVERT(varchar(50), @ReceivePo))
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'Bills',
                                     @PJID,
                                     'Reception No#',
                                     @Val,
                                     @ReceivePo
                END
                ELSE
                IF (@CurrentReceivePo <> CONVERT(varchar(50), @ReceivePo))
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'Bills',
                                     @PJID,
                                     'Reception No#',
                                     @CurrentReceivePo,
                                     @ReceivePo
                END
            END
            SET @Val = NULL
            IF (@Ref IS NOT NULL)
            BEGIN
                SET @Val = (SELECT TOP 1
                    newVal
                FROM log2
                WHERE screen = 'Bills'
                AND ref = @PJID
                AND Field = 'Ref No.'
                ORDER BY CreatedStamp DESC)
                IF (@Val <> @Ref)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'Bills',
                                     @PJID,
                                     'Ref No.',
                                     @Val,
                                     @Ref
                END
                ELSE
                IF (@CurrentRef <> @Ref)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'Bills',
                                     @PJID,
                                     'Ref No.',
                                     @CurrentRef,
                                     @Ref
                END
            END
            SET @Val = NULL
            IF (@Date IS NOT NULL)
            BEGIN
                SET @Val = (SELECT TOP 1
                    newVal
                FROM log2
                WHERE screen = 'Bills'
                AND ref = @PJID
                AND Field = 'Date'
                ORDER BY CreatedStamp DESC)
                DECLARE @IDate nvarchar(150)
                SELECT
                    @IDate = CONVERT(varchar, @Date, 101)
                IF (@Val <> @IDate)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'Bills',
                                     @PJID,
                                     'Date',
                                     @Val,
                                     @IDate
                END
                ELSE
                IF (@CurrentDate <> @IDate)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'Bills',
                                     @PJID,
                                     'Date',
                                     @CurrentDate,
                                     @IDate
                END
            END
            SET @Val = NULL
            IF (@PostingDate IS NOT NULL)
            BEGIN
                SET @Val = (SELECT TOP 1
                    newVal
                FROM log2
                WHERE screen = 'Bills'
                AND ref = @PJID
                AND Field = 'Posting Date'
                ORDER BY CreatedStamp DESC)
                DECLARE @PostingDateDate nvarchar(150)
                SELECT
                    @PostingDateDate = CONVERT(varchar, @PostingDate, 101)
                IF (@Val <> @PostingDateDate)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'Bills',
                                     @PJID,
                                     'Posting Date',
                                     @Val,
                                     @PostingDateDate
                END
                ELSE
                IF (@CurrentPostingDate <> @PostingDateDate)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'Bills',
                                     @PJID,
                                     'Posting Date',
                                     @CurrentPostingDate,
                                     @PostingDateDate
                END
            END
            SET @Val = NULL
            IF (@Due IS NOT NULL)
            BEGIN
                SET @Val = (SELECT TOP 1
                    newVal
                FROM log2
                WHERE screen = 'Bills'
                AND ref = @PJID
                AND Field = 'Due Date'
                ORDER BY CreatedStamp DESC)
                DECLARE @DueDate nvarchar(150)
                SELECT
                    @DueDate = CONVERT(varchar, @Due, 101)
                IF (@Val <> @DueDate)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'Bills',
                                     @PJID,
                                     'Due Date',
                                     @Val,
                                     @DueDate
                END
                ELSE
                IF (@CurrentDue <> @DueDate)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'Bills',
                                     @PJID,
                                     'Due Date',
                                     @CurrentDue,
                                     @DueDate
                END
            END
            SET @Val = NULL
            IF (@DueIn IS NOT NULL
                AND @DueIn != 0)
            BEGIN
                SET @Val = (SELECT TOP 1
                    newVal
                FROM log2
                WHERE screen = 'Bills'
                AND ref = @PJID
                AND Field = 'Due In'
                ORDER BY CreatedStamp DESC)
                IF (@Val <> CONVERT(varchar(50), @DueIn))
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'Bills',
                                     @PJID,
                                     'Due In',
                                     @Val,
                                     @DueIn
                END
                ELSE
                IF (@CurrentDueIn <> CONVERT(varchar(50), @DueIn))
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'Bills',
                                     @PJID,
                                     'Due In',
                                     @CurrentDueIn,
                                     @DueIn
                END
            END
            SET @Val = NULL
            IF (@Disc IS NOT NULL)
            BEGIN
                SET @Val = (SELECT TOP 1
                    newVal
                FROM log2
                WHERE screen = 'Bills'
                AND ref = @PJID
                AND Field = '% Disc'
                ORDER BY CreatedStamp DESC)
                IF (CONVERT(numeric(30, 4), @Val) <> @Disc)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'Bills',
                                     @PJID,
                                     '% Disc',
                                     @Val,
                                     @Disc
                END
                ELSE
                IF (@CurrentDisc <> @Disc)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'Bills',
                                     @PJID,
                                     '% Disc',
                                     @CurrentDisc,
                                     @Disc
                END
            END
            SET @Val = NULL
            IF (@Status IS NOT NULL)
            BEGIN
			 
                SET @Val = (SELECT TOP 1
                    newVal
                FROM log2
                WHERE screen = 'Bills'
                AND ref = @PJID
                AND Field = 'Spec'
                ORDER BY CreatedStamp DESC)
                DECLARE @SpecVal varchar(50)
                SELECT
                    @SpecVal =
                                  CASE @Status
                                      WHEN 0 THEN 'Input Only'
                                      WHEN 1 THEN 'Hold - No Invoices'
                                      WHEN 2 THEN 'Hold - No Materials'
                                      WHEN 3 THEN 'Hold - Other'
                                      WHEN 4 THEN 'Verified'
                                      WHEN 5 THEN 'Selected'
                                  END
                IF (@Val <> @SpecVal)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'Bills',
                                     @PJID,
                                     'Spec',
                                     @Val,
                                     @SpecVal
                END
                ELSE
                IF (@CurrentSpec <> @SpecVal)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'Bills',
                                     @PJID,
                                     'Spec',
                                     @CurrentSpec,
                                     @SpecVal
                END
            END
            SET @Val = NULL
            IF (@Memo IS NOT NULL)
            BEGIN
                SET @Val = (SELECT TOP 1
                    newVal
                FROM log2
                WHERE screen = 'Bills'
                AND ref = @PJID
                AND Field = 'Memo'
                ORDER BY CreatedStamp DESC)
                IF (@Val <> @Memo)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'Bills',
                                     @PJID,
                                     'Memo',
                                     @Val,
                                     @Memo
                END
                ELSE
                IF (@CurrentMemo <> @Memo)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'Bills',
                                     @PJID,
                                     'Memo',
                                     @CurrentMemo,
                                     @Memo
                END
            END
            SET @Val = NULL

            IF (@Custom1 IS NOT NULL)
            BEGIN
                SET @Val = (SELECT TOP 1
                    newVal
                FROM log2
                WHERE screen = 'Bills'
                AND ref = @PJID
                AND Field = 'Custom1'
                ORDER BY CreatedStamp DESC)
                IF (@Val <> @Custom1)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'Bills',
                                     @PJID,
                                     'Custom1',
                                     @Val,
                                     @Custom1
                END
                ELSE
                IF (@CurrentCustom1 <> @Custom1)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'Bills',
                                     @PJID,
                                     'Custom1',
                                     @CurrentCustom1,
                                     @Custom1
                END
                ELSE
                IF (@Val IS NULL
                    AND @Custom1 IS NOT NULL)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'Bills',
                                     @PJID,
                                     'Custom1',
                                     @CurrentCustom1,
                                     @Custom1
                END
            END
            SET @Val = NULL
            IF (@Custom2 IS NOT NULL)
            BEGIN
                SET @Val = (SELECT TOP 1
                    newVal
                FROM log2
                WHERE screen = 'Bills'
                AND ref = @PJID
                AND Field = 'Custom2'
                ORDER BY CreatedStamp DESC)
                IF (@Val <> @Custom2)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'Bills',
                                     @PJID,
                                     'Custom2',
                                     @Val,
                                     @Custom2
                END
                ELSE
                IF (@CurrentCustom2 <> @Custom2)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'Bills',
                                     @PJID,
                                     'Custom2',
                                     @CurrentCustom2,
                                     @Custom2
                END
                ELSE
                IF (@Val IS NULL
                    AND @Custom2 IS NOT NULL)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'Bills',
                                     @PJID,
                                     'Custom2',
                                     @CurrentCustom2,
                                     @Custom2
                END
            END

            SET @Val = NULL
            IF (@PreTotal IS NOT NULL)
            BEGIN
                SET @Val = (SELECT TOP 1
                    newVal
                FROM log2
                WHERE screen = 'Bills'
                AND ref = @PJID
                AND Field = 'Amount'
                ORDER BY CreatedStamp DESC)
                IF (CONVERT(numeric(30, 2), @Val) <> @PreTotal)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'Bills',
                                     @PJID,
                                     'Amount',
                                     @Val,
                                     @PreTotal
                END
                ELSE
                IF (@CurrentAmount <> @PreTotal)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'Bills',
                                     @PJID,
                                     'Amount',
                                     @CurrentAmount,
                                     @PreTotal
                END
            END

            SET @Val = NULL
            IF (@totalUtax IS NOT NULL)
            BEGIN
                SET @Val = (SELECT TOP 1
                    newVal
                FROM log2
                WHERE screen = 'Bills'
                AND ref = @PJID
                AND Field = 'Use Tax'
                ORDER BY CreatedStamp DESC)
                IF (CONVERT(numeric(30, 4), @Val) <> @totalUtax)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'Bills',
                                     @PJID,
                                     'Use Tax',
                                     @Val,
                                     @totalUtax
                END
                ELSE
                IF (@CurrentUseTax <> @totalUtax)
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'Bills',
                                     @PJID,
                                     'Use Tax',
                                     @CurrentUseTax,
                                     @totalUtax
                END
            END
            SET @Val = NULL
            IF (@IfPaid IS NOT NULL
                AND @IfPaid != 0)
            BEGIN
                SET @Val = (SELECT TOP 1
                    newVal
                FROM log2
                WHERE screen = 'Bills'
                AND ref = @PJID
                AND Field = 'IfPaid'
                ORDER BY CreatedStamp DESC)
                IF (@Val <> CONVERT(varchar(50), @IfPaid))
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'Bills',
                                     @PJID,
                                     'IfPaid',
                                     @Val,
                                     @IfPaid
                END
                ELSE
                IF (@CurrentIfPaid <> CONVERT(varchar(50), @IfPaid))
                BEGIN
                    EXEC log2_insert @UpdatedBy,
                                     'Bills',
                                     @PJID,
                                     'IfPaid',
                                     @CurrentIfPaid,
                                     @IfPaid
                END
            END

        --set @Val=null
        --if(@Status is not null)
        --begin 	
        --    Set @Val =(select Top 1 newVal  from log2 where screen='Bills' and ref= @PJID and Field='Status' order by CreatedStamp desc )	
        --	Declare @CurrentStatusVal varchar(50)
        --	Select @CurrentStatusVal = Case @Status WHEN 0 THEN 'Open' WHEN 1 THEN 'Closed' END 
        --	if(@Val <> @CurrentStatusVal)
        --	begin
        --		exec log2_insert @UpdatedBy,'Bills',@PJID,'Status',@Val,@CurrentStatusVal
        --	end
        --	Else IF (@CurrentStatus <> @CurrentStatusVal)
        --	Begin
        --		exec log2_insert @UpdatedBy,'Bills',@PJID,'Status',@CurrentStatus,@CurrentStatusVal
        --	END
        --end

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
