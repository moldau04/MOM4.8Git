CREATE PROCEDURE [dbo].[spAddPO]
	@PO int,
	@fDate datetime,
	@fDesc varchar(max),
	@Amount numeric(30,2), 
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
	@POItem AS tblTypePOItem readonly,
	@UpdatedBy varchar(100),
	@RequestedBy varchar(100),
	@SalesOrderNo varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Line smallint
	DECLARE @Quan numeric(30,2)
	DECLARE @PofDesc varchar(8000)
	DECLARE @PoPrice numeric(30,4)
	DECLARE @PoAmount numeric(30,2)
	DECLARE @Job int
	DECLARE @Phase smallint
	DECLARE @Inv int
	DECLARE @GL int
	DECLARE @TypeID int
	DECLARE @Billed int
	DECLARE @Ticket int
	DECLARE @Balance numeric(30,2)
	DECLARE @Selected numeric(30,2) = 0.0
	DECLARE @PoDue datetime
	DECLARE @SelectedQuan numeric(30,2) = 0.0
	DECLARE @BalanceQuan numeric(30,2) = 0.0
	DECLARE @ItemDesc varchar(30)
	DECLARE @comm numeric(30,2) = 0
	DECLARE @GLRev int = 0
	DECLARE @WarehouseID varchar(5)
	DECLARE @LocationID int = 0
	DECLARE @OpSq varchar(150)

BEGIN TRY
BEGIN TRANSACTION
		
	
	SET IDENTITY_INSERT [PO] ON 

	INSERT INTO [dbo].[PO]
           ([PO],[fDate],[fDesc],[Amount],[Vendor],[Status],[Due],[ShipVia],[Terms],[FOB],[ShipTo],[Approved],[Custom1],[Custom2],[ApprovedBy],[ReqBy],[fBy],[PORevision],[SalesOrderNo],[POReasonCode],[CourrierAcct],[RequestedBy])
     VALUES
           (@PO,@fDate,@fDesc,@Amount,@VendorId,@Status,@Due,@ShipVia,@Terms,@FOB,@ShipTo,@Approved,@Custom1,@Custom2,@ApprovedBy,@ReqBy,@fBy,@PORevision,@SalesOrderNo,@POReasonCode,@CourrierAcct,@RequestedBy)

	SET IDENTITY_INSERT [PO] OFF
	
	CREATE table #temp
	(
	ID int null,
	Line smallint null,
	AcctID int null,
	fDesc varchar(8000) null,
	Quan numeric(30,2) null,
	Price numeric(30,2) null,
	Amount numeric(30,2) null,
	JobID numeric(30,2) null,
	PhaseID int null,
	Inv int null, 
	Billed int null,
	Ticket int null,
	Due datetime null,
	TypeID int null,
	ItemDesc varchar(30) null,
	WarehouseID varchar(5) null,
	LocationID int null,
	OpSq varchar(150) null
	)
	
	DECLARE db_cursor CURSOR FOR 
	
	SELECT Line, Quan, fDesc, Price, Amount, JobID, PhaseID, Inv, AcctID, Billed, Ticket, Due, TypeID, ItemDesc,WarehouseID,LocationID,OpSq FROM @POItem

	OPEN db_cursor  
	FETCH NEXT FROM db_cursor INTO 
		@Line, @Quan, @PofDesc, @PoPrice, @PoAmount, @Job, @Phase, @Inv, @GL, @Billed, @Ticket, @PoDue, @TypeID, @ItemDesc,@WarehouseID, @LocationID,@OpSq
		
	WHILE @@FETCH_STATUS = 0
	BEGIN  		
		/*
		if (@PofDesc != '' and @job is not null  and  @Phase is not null)  --and (@TypeID =1 or @TypeID =2)
		begin

			if(@Inv is not null)
			begin
				-- insert bom job item
				exec @Phase = spAddBOMItem @job, @TypeID, @Inv, @PofDesc,@Phase,@OpSq

			end
			else if (@Inv is null)
			begin
				-- add into inv table (as non inventory type) and add as bom item\]
				--if exists (select top 1 1 from inv where Name = @ItemDesc and fDesc = @PofDesc) -- check if item name and description is already exists!
				if exists (select top 1 1 from inv where Name = @ItemDesc) -- check if item name and description is already exists!
				begin
					--set @Inv = (select top 1 ID from inv where Name = @ItemDesc and fDesc = @PofDesc and type = 2)
					set @Inv = (select top 1 ID from inv where Name = @ItemDesc and type = 2)

					--CHECK IF ITEM ALREADY EXIST IN BOM
					if exists (select top 1 line from jobtitem where job=@job and fDesc=@ItemDesc)
					BEGIN
						DECLARE @OPhase smallint
						SET @OPhase=(select top 1 line from jobtitem where job=@job and fDesc=@ItemDesc)
						-- insert bom job item
						exec @Phase = spAddBOMItem @job, @TypeID, @Inv, @PofDesc,@OPhase,@OpSq
					END
					ELSE
					BEGIN
						-- insert bom job item
					exec @Phase = spAddBOMItem @job, @TypeID, @Inv, @PofDesc,@Phase,@OpSq
					END
				end
				else
				begin
					if @ItemDesc is not null and @ItemDesc!=''
						begin
							SET @GLRev = ISNULL((SELECT SAcct FROM Job job inner join Inv inv on  job.GLRev=inv.ID WHERE job.ID = @Job),0)
							INSERT INTO Inv (Name, fdesc, Cat, Balance, Measure, Tax, AllowZero, InUse, Type, Sacct, Status, Price1) 
							VALUES (@ItemDesc,@PofDesc,0,0,'Each',0,0,0,2,@GLRev,0,0)
				
							SET @Inv = SCOPE_IDENTITY()


							-- insert bom job item
							exec @Phase = spAddBOMItem @job, @TypeID, @Inv, @PofDesc,@Phase,@OpSq
						end
					else
						begin
							exec @Phase = spAddBOMItem @job, @TypeID, @Inv, @PofDesc,@Phase,@OpSq
						end
				end

			
	
			end
			
		end

		if (@PofDesc != '' and @job =0 and  @Phase = 0)  --and  @TypeID =8
		begin
			if(@Inv is not null)
			begin
				-- insert bom job item
				exec @Phase = spAddBOMItem @job, @TypeID, @Inv, @PofDesc,@Phase,@OpSq

			end
			else if (@Inv is null)
			begin
				-- add into inv table (as non inventory type) and add as bom item
				--if exists (select top 1 1 from inv where Name = @ItemDesc and fDesc = @PofDesc) -- check if item name and description is already exists!
				if exists (select top 1 1 from inv where Name = @ItemDesc) -- check if item name and description is already exists!
				begin
					--set @Inv = (select top 1 ID from inv where Name = @ItemDesc and fDesc = @PofDesc and type = 2)
					set @Inv = (select top 1 ID from inv where Name = @ItemDesc and type = 2)

					-- insert bom job item
					exec @Phase = spAddBOMItem @job, @TypeID, @Inv, @PofDesc,@Phase,@OpSq
				end
				else
				begin
					if @ItemDesc is not null and @ItemDesc!=''
						begin
							SET @GLRev = ISNULL((SELECT SAcct FROM Job job inner join Inv inv on  job.GLRev=inv.ID WHERE job.ID = @job),0)
							INSERT INTO Inv (Name, fdesc, Cat, Balance, Measure, Tax, AllowZero, InUse, Type, Sacct, Status, Price1) 
							VALUES (@ItemDesc,@PofDesc,0,0,'Each',0,0,0,2,@GLRev,0,0)
							SET @Inv = SCOPE_IDENTITY()

							-- insert bom job item
							exec @Phase = spAddBOMItem @job, @TypeID, @Inv, @PofDesc,@Phase,@OpSq
						end
				end
			end
		end
		*/

		if (@PofDesc != '' and @job is not null)--  and  @Phase is not null)  --and (@TypeID =1 or @TypeID =2)
		begin
			if(@Inv is not null and @Inv != '')
			begin
				-- insert bom job item
				exec @Phase = spAddBOMItem @job, @TypeID, @Inv, @PofDesc,@Phase,@OpSq

			end
			else-- if (@Inv is null)
			begin
				-- add into inv table (as non inventory type) and add as bom item\]
				--if exists (select top 1 1 from inv where Name = @ItemDesc and fDesc = @PofDesc) -- check if item name and description is already exists!
				if exists (select top 1 1 from inv where Name = @ItemDesc) -- check if item name and description is already exists!
				begin
					--set @Inv = (select top 1 ID from inv where Name = @ItemDesc and fDesc = @PofDesc and type = 2)
					set @Inv = (select top 1 ID from inv where Name = @ItemDesc and type = 2)

					if (@job != 0)
					begin
						--CHECK IF ITEM ALREADY EXIST IN BOM
						if exists (select top 1 line from jobtitem where job=@job and fDesc=@ItemDesc)
						BEGIN
							DECLARE @OPhase smallint
							SET @OPhase=(select top 1 line from jobtitem where job=@job and fDesc=@ItemDesc)
							-- insert bom job item
							exec @Phase = spAddBOMItem @job, @TypeID, @Inv, @PofDesc,@OPhase,@OpSq
						END
						ELSE
						BEGIN
							-- insert bom job item
							exec @Phase = spAddBOMItem @job, @TypeID, @Inv, @PofDesc,@Phase,@OpSq
						END
					end
					else
					begin
						exec @Phase = spAddBOMItem @job, @TypeID, @Inv, @PofDesc,@Phase,@OpSq
					END
				end
				else
				begin
					if @ItemDesc is not null and @ItemDesc!=''
					begin
						SET @GLRev = ISNULL((SELECT SAcct FROM Job job inner join Inv inv on  job.GLRev=inv.ID WHERE job.ID = @Job),0)
						INSERT INTO Inv (Name, fdesc, Cat, Balance, Measure, Tax, AllowZero, InUse, Type, Sacct, Status, Price1) 
						VALUES (@ItemDesc,@PofDesc,0,0,'Each',0,0,0,2,@GLRev,0,0)
			
						SET @Inv = SCOPE_IDENTITY()

						-- insert bom job item
						exec @Phase = spAddBOMItem @job, @TypeID, @Inv, @PofDesc,@Phase,@OpSq
					end
					else
					begin
						exec @Phase = spAddBOMItem @job, @TypeID, @Inv, @PofDesc,@Phase,@OpSq
					end
				end
			end
		end

		insert into #temp (Line, Quan, fDesc, Price, Amount, JobID, PhaseID, Inv, AcctID, Billed, Ticket, Due, TypeID, ItemDesc,WarehouseID,LocationID,OpSq )
		values (@Line, @Quan, @PofDesc, @PoPrice, @PoAmount, @Job, @Phase, @Inv, @GL, @Billed, @Ticket, @PoDue, @TypeID, @ItemDesc,@WarehouseID, @LocationID,@OpSq)


		-------------->
			SET @Line=Null; SET @Quan=Null; SET @PofDesc=Null; SET @PoPrice=Null; SET @PoAmount=Null; SET @Job=Null; SET @Phase=Null; SET @Inv=Null; SET @GL=Null; SET @Billed=Null; SET @Ticket=Null; SET @PoDue=Null; SET @TypeID=Null; SET @ItemDesc=Null;SET @WarehouseID=Null; SET @LocationID=Null;SET @OpSq=Null;
		--------------->

	FETCH NEXT FROM db_cursor INTO 
		 @Line, @Quan, @PofDesc, @PoPrice, @PoAmount, @Job, @Phase, @Inv, @GL, @Billed, @Ticket, @PoDue, @TypeID, @ItemDesc,@WarehouseID, @LocationID,@OpSq
	END  



	CLOSE db_cursor  
	DEALLOCATE db_cursor

	----------------------------$$$$$$$ INSERT PO ITEMS $$$$$$$$--------------------------

	DECLARE db_cursor1 CURSOR FOR 

	SELECT Line, Quan, fDesc, Price, Amount, JobID, PhaseID, Inv, AcctID,  Billed, Ticket, Due,WarehouseID,LocationID,OpSq,TypeID FROM #temp 

	OPEN db_cursor1  
	FETCH NEXT FROM db_cursor1 INTO 
		@Line, @Quan, @PofDesc, @PoPrice, @PoAmount, @Job, @Phase, @Inv, @GL, @Billed, @Ticket, @PoDue, @WarehouseID, @LocationID,@OpSq,@TypeID
	WHILE @@FETCH_STATUS = 0
	BEGIN  		
		
		SET @Balance  = @PoAmount
		SET @BalanceQuan = @Quan

		INSERT INTO [dbo].[POItem]
           ([PO],[Line],[Quan],[fDesc],[Price],[Amount],[Job],[Phase],[Inv],[GL],[Freight],[Rquan],[Billed],[Ticket],[Selected],[Balance],[Due],[SelectedQuan],[BalanceQuan],WarehouseID,LocationID,TypeID)
		VALUES
           (@PO,@Line,@Quan,@PofDesc,@PoPrice,@PoAmount,@Job,@Phase,@Inv,@GL,0,null,@Billed,@Ticket,@Selected,@Balance,@PoDue,@SelectedQuan,@BalanceQuan, @WarehouseID, @LocationID,@TypeID)

		   ----Update Lcost-------->

		   --UPDATE Inv SET LCost=@PoPrice WHERE TYPE=0 and id=@Inv

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
		
		IF ISNULL(@Job,0) <> 0
		BEGIN
			EXEC spUpdateJobCommExp @Job
		END

		IF @Phase IS NOT NULL
			BEGIN
				
				SET @comm = ISNULL((SELECT sum(isnull(p.Balance,0)) from POItem p 
									INNER JOIN PO on p.po = po.po
									WHERE p.Job = @Job and p.Phase = @Phase and po.status in (0,3,4)),0) + 
							ISNULL((SELECT sum(isnull(rp.Amount,0)) from RPOItem rp 
									INNER JOIN ReceivePO r on r.ID = rp.ReceivePO
									LEFT JOIN POItem p on r.PO = p.PO AND rp.POLine = p.Line
									WHERE p.Job = @job and p.Phase = @Phase and r.status = 0),0)
					IF (@TypeID =1 or @TypeID =2)
			BEGIN
				UPDATE	JobTItem 

						SET Comm = @comm 

				WHERE		Type = 1 
						AND Job = @Job 
						AND Line = @Phase
						AND Code=@OpSq  
			END
			ELSE
			BEGIN
				UPDATE	JobTItem 

						SET Comm = @comm 

				WHERE		Type = 2
						AND Job = @Job 
						AND Line = @Phase 
						AND Code=@OpSq 
			END


			END
	-------------->
   SET @Line= NULL ; SET @Quan= NULL ; SET @PofDesc= NULL ; SET @PoPrice= NULL ; SET @PoAmount= NULL ; SET @Job= NULL ; SET @Phase= NULL ; SET @Inv= NULL ; SET @GL= NULL ; SET @Billed= NULL ; SET @Ticket= NULL ; SET @PoDue= NULL ; SET @WarehouseID= NULL ; SET @LocationID= NULL ;SET @OpSq= NULL ; SET @TypeID = NULL;

    -------------->

	FETCH NEXT FROM db_cursor1 INTO 
		 @Line, @Quan, @PofDesc, @PoPrice, @PoAmount, @Job, @Phase, @Inv, @GL, @Billed, @Ticket, @PoDue, @WarehouseID, @LocationID,@OpSq,@TypeID 
	END  

	CLOSE db_cursor1  
	DEALLOCATE db_cursor1


	DECLARE @ChartId int

	--SELECT TOP 1 @ChartId=ID FROM Chart WHERE DefaultNo='D9991' AND Status=0 ORDER BY ID 
	 

	DROP TABLE #temp

	exec spCalChartBalance
	--UPDATE Chart 
	--   SET Balance = ISNULL (p.Balance , 0)
	--  FROM Chart c LEFT JOIN
	--	(SELECT Sum(Amount) AS Balance
	--		FROM PO) p
	--		ON c.DefaultNo = 'D9991' AND Status = 0

	      -----------Inventory  Adjustment-------------->

		  --UPDATE i  
    --      SET i.hand=(SELECT isnull(sum(isnull(Adj.Hand,0)),0)     FROM IWarehouseLocAdj   adj  WHERE adj.InvID=I.ID) , 
    --      i.Balance= (SELECT isnull(sum(isnull(Adj.Balance,0)),0)  FROM IWarehouseLocAdj   adj  WHERE adj.InvID=I.ID) 
    --      FROM  INV I WHERE i.Type=0 

		insert into tblInventoryWHTrans(InvID, WarehouseID,LocationID ,Hand,Balance,fOrder,[Committed],[Available],Screen,ScreenID,Mode,Date,TransType,Batch,fDate)
         select POItem.Inv, POItem.WarehouseID , POItem.LocationID ,0,0,POItem.Quan,0,0,Screen= 'PO',@PO,'Add',GETDATE(),'None',0,GETDATE() from POItem 
			inner join BOMT on BOMT.ID=POItem.TypeID --and  BOMT.Type='Inventory'
			where PO =@PO and BOMT.Type='Inventory' 
			--select * from tblInventoryWHTrans
			exec CalculateInventory
		  ----------------------------------------------->
/********Start Logs************/
 if(@VendorId is not null And @VendorId != 0)
	Begin 	
		Declare @VendorName varchar(150)
		Select @VendorName = r.Name FROM Rol r INNER JOIN Vendor V ON V.Rol = r.ID WHERE V.ID  = @VendorId
	exec log2_insert @UpdatedBy,'PO',@PO,'Vendor','',@VendorName
	END
if(@ShipTo is not null And @ShipTo != '')
	Begin
	exec log2_insert @UpdatedBy,'PO',@PO,'Ship To','',@ShipTo
	END
if(@fDesc is not null And @fDesc != '')
	Begin
	exec log2_insert @UpdatedBy,'PO',@PO,'Comments','',@fDesc
	END
if(@ShipVia is not null And @ShipVia != '')
	Begin
	exec log2_insert @UpdatedBy,'PO',@PO,'Courier','',@ShipVia
	END
 if(@CourrierAcct is not null And @CourrierAcct != '')
	Begin
	exec log2_insert @UpdatedBy,'PO',@PO,'Courier Account #','',@CourrierAcct
	END
if(@fDate is not null And @fDate != '')
	Begin 	
	 Declare @Calldate nvarchar(150)
	 SELECT @Calldate = convert(varchar, @fDate, 101)
	exec log2_insert @UpdatedBy,'PO',@PO,'Date','',@Calldate
	END
	if(@Due is not null And @Due != '')
	Begin 	
	Declare @Duedate nvarchar(150)
	SELECT @Duedate = convert(varchar, @Due, 101)
	exec log2_insert @UpdatedBy,'PO',@PO,'Due Date','',@Duedate
	END
  if(@Terms is not null)
	Begin 	
	Declare @PaymentTerms varchar(150)
	Select @PaymentTerms = Name from tblterms WHERE ID  = @Terms
	exec log2_insert @UpdatedBy,'PO',@PO,'Payment Terms','',@PaymentTerms
	END
if(@SalesOrderNo is not null And @SalesOrderNo != '')
	Begin
	exec log2_insert @UpdatedBy,'PO',@PO,'Sales Order #','',@SalesOrderNo
	END
if(@POReasonCode is not null And @POReasonCode != '')
	Begin
	exec log2_insert @UpdatedBy,'PO',@PO,'PO Reason Code','',@POReasonCode
	END
 if(@PORevision is not null And @PORevision != '')
	Begin
	exec log2_insert @UpdatedBy,'PO',@PO,'PO Revision','',@PORevision
	END
 if(@FOB is not null And @FOB != '')
	Begin
	exec log2_insert @UpdatedBy,'PO',@PO,'Incoterms','',@FOB
	END
 if(@fBy is not null And @fBy != '')
	Begin
	exec log2_insert @UpdatedBy,'PO',@PO,'Created By','',@fBy
	END
 if(@Approved is not null)
	Begin
	exec log2_insert @UpdatedBy,'PO',@PO,'Approved','',@Approved
	END
if(@Status is not null)
	Begin 	
	Declare @CurrentStatusVal varchar(50)
	Select @CurrentStatusVal = Case @Status WHEN 0 THEN 'Open' WHEN 1 THEN 'Closed' WHEN 2 THEN 'Void' WHEN 3 THEN 'Partial-Quantity' WHEN 4 THEN 'Partial-Amount' WHEN 5 THEN 'Closed At Received PO' END 
	exec log2_insert @UpdatedBy,'PO',@PO,'Status','',@CurrentStatusVal
	END
if(@Amount is not null)
	Begin
		exec log2_insert @UpdatedBy,'PO',@PO,'Amount','',@Amount
	END
 if(@RequestedBy is not null)
	Begin
	exec log2_insert @UpdatedBy,'PO',@PO,'RequestedBy','',@RequestedBy
	END	
 /********End Logs************/

COMMIT 
END TRY
BEGIN CATCH

	SELECT ERROR_MESSAGE()

	IF @@TRANCOUNT>0
		ROLLBACK	
		RAISERROR ('An error has occurred on this page.',16,1)
		RETURN

END CATCH

END
GO