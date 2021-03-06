/*--------------------------------------------------------------------  
Modified By: Thomas  
Modified On: 24 Sep 2019   
Description: Adding Logs  

Modified By: Rustam  
Modified On: 26 Jan 2019   
Description:Update POTypeID column  
--------------------------------------------------------------------*/
CREATE PROCEDURE [dbo].[spAddProject] @job int,
@owner int,
@loc int,
@fdesc varchar(75),
@status smallint = NULL,
@type smallint,
@Remarks varchar(max),
@ctype varchar(15),
@ProjCreationDate datetime,
@PO varchar(25),
@SO varchar(25),
@Certified smallint,
@Custom1 varchar(75),
@Custom2 varchar(75),
@Custom3 varchar(75),
@Custom4 varchar(75),
@Custom5 datetime = NULL,
@template int,
@RolName varchar(75),
@city varchar(50),
@state varchar(2),
@zip varchar(10),
@country varchar(50),
@phone varchar(28),
@cellular varchar(28),
@fax varchar(28),
@contact varchar(50),
@email varchar(50),
@rolRemarks varchar(max),
@rolType smallint,
@InvExp int,
@InvServ int,
@Wage int,
@GLInt int,
@jobtCType varchar(10),
@Post smallint,
@Charge smallint,
@JobClose smallint,
@fInt smallint,
@Items AS tblTypeProjectItem READONLY,
@TeamItems AS tblTypeTeamItem READONLY,
@BomItem AS tblTypeBomItem READONLY,
@MilestonItem AS tblTypeMilestoneItem READONLY,
@CustomItem AS tblTypeCustomTabItem READONLY,
@BillRate numeric(30, 2) = 0,
@RateOT numeric(30, 2) = 0,
@RateNT numeric(30, 2) = 0,
@RateDT numeric(30, 2) = 0,
@RateTravel numeric(30, 2) = 0,
@Mileage numeric(30, 2) = 0,
@rolid int = -1,
@taskcategory varchar(15) = '',
@TaskCodes AS tblTypeTaskCodes READONLY,
@SPHandle smallint = 0,
@SPRemarks text = '',
@IsRenewalNotes smallint = 0,
@RenewalNotes text = '',
@tblGCandHomeOwner AS tblGCandHomeOwner1 READONLY,
@PWIP bit = 0,
@UnrecognizedRevenue int = NULL,
@UnrecognizedExpense int = NULL,
@RetainageReceivable int = NULL,
@ArchitectName nvarchar(50) = NULL,
@ArchitectAdress nvarchar(50) = NULL,
@PType smallint = 0,
@JobAmount numeric(30, 2) = 0,
@ProjectManagerUserID int = 0,
@AssignedProjectUserID int = 0, 
@UpdatedByUserId int =0, 
@TargetHPermission int=0,
@GroupIds varchar(Max) = '',
@SupervisorUserID int = 0,
@ProjectStageID int = 0

AS

        if exists ( select 1 from job where id=@job)
		begin
		   INSERT INTO tblInventoryWHTrans(InvID, WarehouseID,LocationID ,Hand,Balance,fOrder,[Committed],[Available],Screen,ScreenID,Mode,Date,TransType,Batch,FDate)
		   select     b.[MatItem],'OFC',0,0,0,0,( b.[QtyRequired] * -1 ),0,'Project',@job,'Edit',GETDATE(),'Revert',0,GETDATE() 
		   from [BOM] b
           INNER JOIN Inv inv on inv.ID = b.MatItem           
           where  [JobTItemID] in (select id from JobTItem where job =@job) AND ISNULL(b.[QtyRequired],0) <> 0 --and Type= ( select top 1 id from BOMT where Type='Inventory' )
						 
		end


    DECLARE @rolAddress varchar(255)
    DECLARE @website varchar(50)
    SELECT @owner = Owner FROM Loc WHERE Loc = @loc
    --declare @rolid int            

    DECLARE @jobTItemId int
    DECLARE @jfDesc varchar(255)
    DECLARE @jCode varchar(10)
    DECLARE @jBudget numeric(30, 2)
    DECLARE @Line smallint
    DECLARE @Btype smallint
    DECLARE @Bitem int
    DECLARE @QtyReq numeric(30, 2)
    DECLARE @UM varchar(50)
    DECLARE @ScrapFact numeric(30, 2)
    DECLARE @BudgetUnit numeric(30, 2)
    DECLARE @BudgetExt numeric(30, 2)
    DECLARE @ProjAcquDate datetime
    DECLARE @jtype smallint
    DECLARE @MileName varchar(150)
    DECLARE @RequiredBy datetime
    DECLARE @LeadTime numeric(30, 2)
    DECLARE @OrgDep int
    DECLARE @Amount numeric(30, 2)
    DECLARE @MQuantity numeric(30, 2)
    DECLARE @MPrice numeric(30, 2)
    DECLARE @MChangeOrder tinyint
    DECLARE @tblCustomFieldsId int
    DECLARE @Value varchar(255)
    DECLARE @tblTabID int
    DECLARE @Label varchar(255)
    DECLARE @TabLine smallint
    DECLARE @Format smallint
    DECLARE @UpdatedDate datetime
    DECLARE @Username varchar(50)
    DECLARE @LabItem int
    DECLARE @MatItem int
    DECLARE @LabMod numeric(30, 2)
    DECLARE @MatMod numeric(30, 2)
    DECLARE @LabExt numeric(30, 2)
    DECLARE @LabRate numeric(30, 2)
    DECLARE @LabHours numeric(30, 2)
    DECLARE @SDate datetime
    DECLARE @Vendor int
    DECLARE @bRev numeric(30, 2)
    DECLARE @bMat numeric(30, 2)
    DECLARE @bLabor numeric(30, 2)
    DECLARE @bCost numeric(30, 2)
    DECLARE @bRatio numeric(30, 2)
    DECLARE @bProfit numeric(30, 2)
    DECLARE @bHour numeric(30, 2)
    DECLARE @GContractorID int;
    DECLARE @HomeOwnerID int;
    DECLARE @MatDesc varchar(50);
    DECLARE @GLRev int = 0;
    DECLARE @OrderNo int;
    DECLARE @GroupID int;
    DECLARE @IsAlert bit;
    DECLARE @IsTask bit;
    DECLARE @TeamMember varchar(max);
    DECLARE @TeamMemberDisplay varchar(max);
    DECLARE @UserRole varchar(max);
    DECLARE @UserRoleDisplay varchar(max);
    DECLARE @bother numeric(30, 2);
    DECLARE @otherExp numeric(30, 2);
    DECLARE @GanttTaskID int;
    DECLARE @EstConvertId int;
    DECLARE @EstConvertLine smallint;
	DECLARE @Period INT = YEAR(GETDATE()) * 100 + MONTH(GETDATE());
	DECLARE @OldMilestonAmount numeric(30, 2) = 0;

	DECLARE @UpdatedBy varchar(100);
	SELECT @UpdatedBy = ISNULL((SELECT TOP 1 fUser FROM tblUser WHERE ID = @UpdatedByUserId),'Maintenance')--fUser FROM tblUser WHERE ID = @UpdatedByUserId
	-- For logs
	Declare @Screen varchar(100) = 'Project';
    Declare @ScreenContract varchar(100) = 'Job';
	Declare @RefId int = @Job;
	DECLARE @customerName varchar(255)
	DECLARE @locationName varchar(255)
	DECLARE @templateName varchar(255)
	DECLARE @statusName varchar(255)
	DECLARE @ProjManager varchar(255)
	DECLARE @Supervisor varchar(255)
	DECLARE @vcSRemarks varchar(max) = ISNULL(Convert(varchar(max),@SPRemarks),'')
	DECLARE @vcRenewNotes varchar(max) = ISNULL(Convert(varchar(max),@RenewalNotes),'')
    DECLARE @tblTH table(code varchar(255) null , GroupID int null, Job  int , TargetHours numeric(30, 2) null)--, GanttTaskID int null) 
    Declare @IsContractExist bit
    IF EXISTS (SELECT 1 FROM Contract WHERE Job = @job) SET @IsContractExist = 1
    ELSE SET @IsContractExist = 0
    --select             
    --@project=e.Job,             
    --@loc= e.LocID,             
    --@owner=(select owner from loc where loc=e.locid)            
    --from Estimate e             
    --where e.ID= @estimate            

    --if(@loc <> 0)            
    --begin            
    --create table #tempRol            
    --( RolID int )            

    DECLARE @ProjectStageName nvarchar(255) = ''
    SELECT @ProjectStageName = Description FROM tblProjectStage WHERE ID = @ProjectStageID


    BEGIN TRY
        BEGIN TRANSACTION

            -------------------------------------Add/Update GC/Ho info ---------------------------------------------            
            EXECUTE spAddGCandHomeOwner @tblGCandHomeOwner,
                                        @GContractorID OUTPUT,
                                        @HomeOwnerID OUTPUT
            --------------------------------------------------------------------------------------------------------            

            SET @rolid = @GContractorID
            UPDATE Loc
            SET GContractorID = ISNULL(@GContractorID, 0),
                HomeOwnerID = ISNULL(@HomeOwnerID, 0)
            WHERE Loc = @loc


           
            IF OBJECT_ID('tempdb.dbo.#MilestonItem') IS NOT NULL
                DROP TABLE #MilestonItem
            SELECT
                * INTO #MilestonItem
            FROM @MilestonItem
            IF (@PType = 1)
            BEGIN
                IF ((SELECT
                        SUM(Amount)
                    FROM #MilestonItem)
                    <> @JobAmount)
                BEGIN
                    UPDATE #MilestonItem
                    SET Amount = @JobAmount
                        , Quantity = 1
                        , Price = @JobAmount
                    WHERE Line = 1
                    UPDATE #MilestonItem
                    SET Amount = 0
                        , Quantity = 0
                        , Price = 0
                    WHERE Line > 1
                END
            END
            ELSE
            IF (@PType = 2)
            BEGIN
                IF ((SELECT
                        SUM(Amount)
                    FROM @MilestonItem)
                    > @JobAmount)
                BEGIN
                    UPDATE #MilestonItem
                    SET Amount = @JobAmount
                        , Quantity = 1
                        , Price = @JobAmount
                    WHERE Line = 1
                    UPDATE #MilestonItem
                    SET Amount = 0
                        , Quantity = 0
                        , Price = 0
                    WHERE Line > 1
                END
            END


            /****Add update Job****/
            DECLARE @INV_WarehouseID varchar(50) = 'OFC';
            IF (@job = 0)
            BEGIN
                --if(@RolName<>'')            
                --begin            
                -- --INSERT INTO #tempRol            
                -- exec @rolid = spAddRolDetails @RolName, @city, @state, @zip, @phone, @fax, @contact, @rolAddress, @email, @website, @country, @cellular, @rolRemarks, @rolType            
                -- --SELECT TOP 1 @rolid=RolID FROM #tempRol            
                --end            

                INSERT INTO job (Loc,
					Owner,
					fDate,
					Status,
					Remarks,
					fDesc,
					Type,
					PO,
					SO,
					Certified,
					Rev, Mat, Labor, Cost, Profit, Ratio, Reg, OT, DT, TT, Hour, BRev, BMat, BLabor, BCost, BProfit, BRatio, BHour, Comm, NT,
					Amount,
					Template,
					Custom21,
					Custom22,
					Custom23,
					Custom24,
					Custom25,
					ProjCreationDate,
					Rol,
					LastUpdateDate,
					BillRate,
					RateOT,
					RateNT,
					RateDT,
					RateTravel,
					RateMileage,
					TaskCategory,
					CType,
					WageC,
					GL,
					GLRev,
					Post,
					Charge,
					fInt,
					SPHandle,
					SRemarks,
					IsRenewalNotes,
					RenewalNotes,
					PWIP,
					UnrecognizedRevenue,
					UnrecognizedExpense,
					RetainageReceivable,
					ArchitectName,
					ArchitectAdress,
					PType,
					ProjectManagerUserID,
					AssignedProjectUserID,
				   TargetHPermission  ,
				   InterestGL,
                   SupervisorUserID,
                    Stage
                )
                VALUES (@loc
					, @owner, GETDATE()
					, @status
					, @Remarks
					, @fdesc
					, @type
					, @PO
					, @SO
					, @Certified
					, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00
					, @JobAmount
					, @template, @Custom1, @Custom2, @Custom3, @Custom4, @Custom5, @ProjCreationDate, @rolid, GETDATE(), @BillRate, @RateOT, @RateNT, @RateDT, @RateTravel, @Mileage, @taskcategory, 

					  
					@Ctype,   --- Service type  ====>   CType      
					
                    @Wage,    ---  Labor GL  ====> WageC
					
                    @InvExp,  --- Expense GL  === > GL

					           ---     Interest GL  === > 
					
                    @InvServ,  --- billing Code   === >  GLRev
					
                    @Post,    --- Posting Method 
					
                    @Charge,  --- Chargable 
					
                    @fInt,    --- Charge Interest 
					
                    @SPHandle, 
					@SPRemarks, 
					@IsRenewalNotes, 
					@RenewalNotes, @PWIP, 
					@UnrecognizedRevenue,
					@UnrecognizedExpense, 
					@RetainageReceivable, 
					@ArchitectName, 
					@ArchitectAdress, 
					@PType, 
					@ProjectManagerUserID, 
					@AssignedProjectUserID,
					@TargetHPermission ,
					@GLInt,
                    @SupervisorUserID,
                    @ProjectStageID
                    )

                SET @job = @@IDENTITY

                IF (@template <> 0)
                BEGIN
                    UPDATE [dbo].[JobT]
                    SET
                    -- [InvExp] = @InvExp            
                    -- ,[InvServ] = @InvServ            
                    -- ,[Wage] = @Wage            
                    -- ,[CType] = @jobtCType            
                    -- ,[Post] = @Post            
                    -- ,[Charge] = @Charge            
                    -- ,[fInt] = @fInt            
                    --[GLInt] = @GLInt,
                    [JobClose] = @JobClose

                    WHERE ID = @template
                END

                INSERT INTO Team (Line, JobID, Title, MomUserID, FirstName, LastName, Email, Mobile)
                    SELECT
                        Line,
                        @job,
                        Title,
                        MomUserID,
                        FirstName,
                        LastName,
                        Email,
                        Mobile
                    FROM @TeamItems
                -- Insert Project Mananger into Team
                DECLARE @fUser varchar(100)
                DECLARE @fFirst varchar(100)
                DECLARE @Last varchar(100)
                DECLARE @Title varchar(100)

                SELECT
                    @Title = ISNULL(e.Title, ''),
                    @fUser = ISNULL(t.fUser, ''),
                    @fFirst = ISNULL(e.fFirst, ''),
                    @Last = ISNULL(e.Last, '')
                FROM [dbo].[tblUser] t
                LEFT JOIN [dbo].[Emp] e
                    ON e.CallSign = t.fUser
                WHERE e.ID = @ProjectManagerUserID

                IF @fUser <> ''
                    AND (SELECT
                        COUNT(*)
                    FROM Team
                    WHERE JobID = @job
                    AND MomUserID = @fUser)
                    = 0
                BEGIN
                    DECLARE @lineTeam int
                    SET @lineTeam = (SELECT
                        MAX(ISNULL(line, 0)) + 1
                    FROM Team
                    WHERE JobID = @job)
                    INSERT INTO Team (Line, JobID, Title, MomUserID, FirstName, LastName)
                        VALUES (@lineTeam, @job, 'Project Manager', @fUser, @fFirst, @Last)

                END

                INSERT INTO JobTItem (JobT,
				Job,
				Type,
				fDesc,
				Code,
				Actual,
				Budget,
				Line,
				[Percent],
				Comm,
				Modifier,
				ETC,
				ETCMod,
				Labor,
				OrderNo)
                    SELECT
                        JobT,
                        @Job,
                        Type,
                        fDesc,
                        Code,
                        Actual,
                        Budget,
                        Line,
                        [Percent],
                        0,
                        0,
                        0,
                        0,
                        0,
                        OrderNo
                    FROM @Items

                IF @@ERROR <> 0
                    AND @@TRANCOUNT > 0
                BEGIN
                    RAISERROR ('Error Occured', 16, 1)
                    ROLLBACK TRANSACTION
                    RETURN
                END

                DECLARE db_cursor CURSOR FOR

                SELECT
                    fDesc,
                    Code,
                    Line,
                    Btype,
                    QtyReq,
                    UM,
                    BudgetUnit,
                    BudgetExt,
                    LabItem,
                    MatItem,
                    LabMod,
                    MatMod,
                    LabExt,
                    LabRate,
                    LabHours,
                    SDate,
                    VendorId,
                    MatDesc,
                    OrderNo,
                    GroupID
                FROM @BomItem

                OPEN db_cursor
                FETCH NEXT FROM db_cursor INTO @jfdesc, @jcode, @Line, @Btype, @QtyReq, @UM, @BudgetUnit, @BudgetExt, @LabItem, @MatItem,
                @LabMod, @MatMod, @LabExt, @LabRate, @LabHours, @SDate, @Vendor, @MatDesc, @OrderNo, @GroupID

                WHILE @@FETCH_STATUS = 0
                BEGIN
                    IF (@MatItem IS NULL
                        OR @MatItem = ''
                        OR @MatItem = 0)
                    BEGIN
                        -- add into inv table (as non inventory type) and add as bom item      
                        IF EXISTS (SELECT TOP 1 1 FROM inv WHERE Name = @MatDesc AND fDesc = @jfDesc) -- check if item name and description is already exists!      
                        BEGIN
                            SET @MatItem = (SELECT TOP 1
                                ID
                            FROM inv
                            WHERE Name = @MatDesc
                            AND fDesc = @jfdesc
                            AND type = 2)
                        END
                        ELSE
                        BEGIN
                            IF (@MatDesc IS NOT NULL
                                AND @MatDesc <> '')
                            BEGIN
                                SET @GLRev = ISNULL((SELECT
                                    SAcct
                                FROM Job job
                                INNER JOIN Inv inv
                                    ON job.GLRev = inv.ID
                                WHERE job.ID = @Job)
                                , 0)
                                INSERT INTO Inv (Name, fdesc, Cat, Balance, Measure, Tax, AllowZero, InUse, Type, Sacct, Status, Price1)
                                    VALUES (@MatDesc, @jfdesc, 0, 0, 'Each', 0, 0, 0, 2, @GLRev, 0, 0)
                                SET @MatItem = SCOPE_IDENTITY()
                            END
                        END
                    END
                    INSERT INTO JobTItem (JobT, Job, Type, fDesc, Code, Actual, Budget,
                    Line, [Percent], Comm, Modifier, ETC, ETCMod, Labor, Stored, BHours, OrderNo, GroupID)
                        VALUES (@template, @Job, 1, @jfDesc, @jCode, 0, @BudgetExt, @Line, 0, 0, @MatMod, @LabExt, @LabMod, 0, 0, @LabHours, @OrderNo, @GroupID)
                    SET @jobTItemId = SCOPE_IDENTITY()

                    -- JobTItem.Type = 0 is revenue type            
                    -- JobTItem.Type = 1 is expense type            

                    IF @@ERROR <> 0
                        AND @@TRANCOUNT > 0
                    BEGIN
                        RAISERROR ('Error Occured', 16, 1)
                        ROLLBACK TRANSACTION
                        RETURN

                    END
                    --DECLARE @bitemVal int = 0            
                    -- If(@Bitem != '')            
                    -- BEGIN            
                    --  SET @bitemVal = CAST(@Bitem AS INT)            
                    -- END            

                    INSERT INTO [dbo].[BOM] ([JobTItemID], [Type], [QtyRequired], [UM], [BudgetUnit], [BudgetExt], [LabItem], [MatItem], [LabRate], [SDate], [Vendor])
                        VALUES (@jobTItemId, @Btype, @QtyReq, @UM, @BudgetUnit, @BudgetExt, @LabItem, @MatItem, @LabRate, @SDate, @Vendor)

                    --------- Insert into tblInventoryWHTrans-------------
			        -- Inventory
			        IF EXISTS (SELECT 1 FROM Inv Where Type = 0 AND ID =@MatItem)
			        BEGIN
				        INSERT INTO tblInventoryWHTrans(InvID, WarehouseID,LocationID ,Hand,Balance,fOrder,[Committed],[Available],Screen,ScreenID,Mode,Date,TransType,Batch,FDate)
				        VALUES (@MatItem,'OFC',0,0,0,0,@QtyReq,0,'Project',@job,'Add',GETDATE(),'In',0,GETDATE())
			        END
			        --------- End Insert into tblInventoryWHTrans----------

                    FETCH NEXT FROM db_cursor INTO
                    @jfdesc, @jcode, @Line, @Btype, @QtyReq, @UM, @BudgetUnit, @BudgetExt, @LabItem, @MatItem,
                    @LabMod, @MatMod, @LabExt, @LabRate, @LabHours, @SDate, @Vendor, @MatDesc, @OrderNo, @GroupID

                END

                CLOSE db_cursor
                DEALLOCATE db_cursor


                -- add milestones for project            
                DECLARE db_cursor1 CURSOR FOR

                SELECT
                    jtype,
                    fdesc,
                    jCode,
                    Line,
                    MilesName,
                    RequiredBy,
                    LeadTime,
                    Type,
                    Amount,
                    OrderNo,
                    GroupID,
                    Quantity,
                    Price,
                    ChangeOrder
                FROM #MilestonItem

                OPEN db_cursor1
                FETCH NEXT FROM db_cursor1 INTO @jType, @jfdesc, @jcode, @Line, @MileName, @RequiredBy, @LeadTime, @OrgDep, @Amount, @OrderNo, @GroupID, @MQuantity, @MPrice, @MChangeOrder
                WHILE @@FETCH_STATUS = 0
                BEGIN
                    INSERT INTO JobTItem (JobT,
                        Job,
                        Type,
                        fDesc,
                        Code,
                        Actual,
                        Budget,
                        Line,
                        [Percent],
                        Comm,
                        Modifier,
                        ETC,
                        ETCMod,
                        Labor,
                        Stored,
                        OrderNo)
                    VALUES (@template, @Job, @jType, @jfDesc, @jCode, 0, @Amount, @Line, 0, 0, 0, 0, 0, 0, 0, @OrderNo)
                    SET @jobTItemId = SCOPE_IDENTITY()

                    IF @@ERROR <> 0
                        AND @@TRANCOUNT > 0
                    BEGIN
                        RAISERROR ('Error Occured', 16, 1)
                        ROLLBACK TRANSACTION
                        RETURN
                    END

                    INSERT INTO [dbo].[Milestone] ([JobTItemID]
                        , [MilestoneName]
                        , [RequiredBy]
                        , [CreationDate]
                        , [ProjAcquistDate]
                        , [Type]
                        , [Amount]
                        , [Quantity]
                        , [Price]
                        , [ChangeOrder])
                    VALUES (@jobTItemId, @MileName, @RequiredBy, GETDATE(), @ProjAcquDate, @OrgDep, @Amount, @MQuantity, @MPrice,@MChangeOrder)

                    FETCH NEXT FROM db_cursor1 INTO @jType, @jfdesc, @jcode, @Line, @MileName, @RequiredBy, @LeadTime, @OrgDep, @Amount, @OrderNo, @GroupID, @MQuantity, @MPrice,@MChangeOrder
                END

                CLOSE db_cursor1
                DEALLOCATE db_cursor1

				-- Add group to project: addnew project only
				INSERT INTO tblProjectGroup (ProjectId,GroupId) 
				SELECT @job, id from tblEstimateGroup g left join tblProjectGroup pg on g.Id = pg.GroupId 
					WHERE g.Id in (Select items from Split(@GroupIds,',')) and (pg.ProjectId is null OR pg.ProjectId!=@job)
				

				-- Adding logs in case create new Job
				/********Start Logs************/
				-- Update job id after create
				Set @RefId = @job;
				-- Project Name
				IF(@fdesc is not null And @fdesc != '')
				BEGIN 	
					EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Project Name','',@fdesc
				END
				-- Customer
				IF(@owner is not null And @owner != 0)
				BEGIN 	
					--DECLARE @customerName varchar(255)
					Select @customerName = isnull(r.Name,'') from Owner o inner join Rol r on o.Rol = r.ID where o.Id = @owner
					EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Customer','',@customerName
				END
				-- Location
				IF(@loc is not null And @loc != 0)
				BEGIN 	
					Select @locationName = Tag from loc where loc = @loc
					EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Location','',@locationName
                    Declare @LocAddress varchar(150)  
	                Select @LocAddress = Address from loc where loc = (Select Loc from Job where ID = @job)  
	                exec log2_insert @UpdatedBy,@Screen,@RefId,'Location Address','',@LocAddress  
				END
				if(@PO is not null And @PO !='')  
                Begin  
	                exec log2_insert @UpdatedBy,@Screen,@RefId,'PO','',@PO  
                END  

                -- Template Type
				IF(@template is not null And @template != 0)
				BEGIN 	
					select  @templateName = j.fdesc from JobT j where j.ID = @template
					EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Template Type','',@templateName
				END
				-- Task Category
				IF(@taskcategory is not null And @taskcategory != '')
				BEGIN 	
					EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Task Category','',@taskcategory
				END
				-- Department
				IF(@type is not null And @type != 0)
				BEGIN 	
					DECLARE @typeName varchar(255)
					SELECT @typeName = Type FROM JobType WHERE Id= @type
					EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Department','',@typeName
				END
				-- Status
				IF(@status is not null And @status != 0)
				BEGIN 	
					SELECT IDENTITY (INT, 0, 1) AS ID, Status INTO #tempJStatus FROM [JStatus]
					SELECT @statusName = Status FROM #tempJStatus where ID = @status
					Drop table #tempJStatus
					EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Status','',@statusName
				END
				-- Default Salesperson
				-- Salesperson 2
				-- Project Manager
				IF(@ProjectManagerUserID is not null And @ProjectManagerUserID != 0)
				BEGIN 	
					SET @ProjManager = Isnull((select Top 1 CallSign from Emp where id=@ProjectManagerUserID ),'')
					EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Project Manager','',@ProjManager
				END

                IF(@SupervisorUserID is not null And @SupervisorUserID != 0)
				BEGIN 	
					SET @Supervisor = Isnull((select Top 1 CallSign from Emp where id=@SupervisorUserID ),'')
					EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Supervisor','',@Supervisor
				END
				
				-- Remarks
				IF(@Remarks is not null And @Remarks != '')
				BEGIN 	
					EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Remarks','',@Remarks
				END
				-- Notes
				-- Project Creation Date
				IF(@ProjCreationDate is not null And @ProjCreationDate != '')
				BEGIN 	
					DECLARE @bidDateStr varchar(150)
					SELECT @bidDateStr = convert(varchar, @ProjCreationDate, 101)
					EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Project Creation Date','',@bidDateStr
				END
				-- @Certified
				-- Certified Job
				IF(@Certified is not null And @Certified != 0)
				BEGIN 	
					EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Certified Job','','True'
				END
				
				-- Custom 1
				IF(@Custom1 is not null And @Custom1 != '')
				BEGIN 	
					EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Custom 1','',@Custom1
				END
				-- Custom 2
				IF(@Custom2 is not null And @Custom2 != '')
				BEGIN 	
					EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Custom 2','',@Custom2
				END
				-- Custom 3
				IF(@Custom3 is not null And @Custom3 != '')
				BEGIN 	
					EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Custom 3','',@Custom3
				END
				-- Custom 4
				IF(@Custom4 is not null And @Custom4 != '')
				BEGIN 	
					EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Custom 4','',@Custom4
				END
				-- Custom 5
				IF(@Custom5 is not null And @Custom5 != '')
				BEGIN 	
					DECLARE @custom5Str varchar(150)
					SELECT @custom5Str = convert(varchar, @Custom5, 101)
					EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Custom 5','',@custom5Str
				END

				-- Special Notes
				IF(ISNULL(@SPHandle,0) != 0)
				BEGIN 	
					EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Special Notes','','True'
				END
				-- Special Note Content
				IF(@vcSRemarks != '')
				BEGIN 	
					Declare @logSPRemarks varchar(1000) = Convert(Varchar(1000), @vcSRemarks)
					EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Special Note Content','',@logSPRemarks
				END

				-- Renew Notes
				IF(ISNULL(@IsRenewalNotes, 0) != 0)
				BEGIN 	
					EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Renew Notes','','True'
				END
				-- Renew Note Content
				IF(@vcRenewNotes != '')
				BEGIN 	
					Declare @logRenewalNotes varchar(1000) = Convert(Varchar(1000), @vcRenewNotes)
					EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Renew Note Content','',@logRenewalNotes
				END

                if(@BillRate is not null)  
                Begin  
	                exec log2_insert @UpdatedBy,@Screen,@RefId,'Bill Rate','',@BillRate  
                END  
                if(@RateOT is not null)  
                Begin  
	                exec log2_insert @UpdatedBy,@Screen,@RefId,'OT Rate','',@RateOT  
                END  
                if(@RateNT is not null)  
                Begin  
	                exec log2_insert @UpdatedBy,@Screen,@RefId,'NT Rate','',@RateNT  
                END  
                if(@RateDT is not null)  
                Begin  
	                exec log2_insert @UpdatedBy,@Screen,@RefId,'DT Rate','',@RateDT  
                END  
                if(@RateTravel is not null)  
                Begin  
	                exec log2_insert @UpdatedBy,@Screen,@RefId,'Travel Rate','',@RateTravel  
                END  
                if(@Mileage is not null)  
                Begin  
	                exec log2_insert @UpdatedBy,@Screen,@RefId,'Mileage','',@Mileage  
                END 
                Declare @EquipmentJob VARCHAR(1000)  
                Select @EquipmentJob  =  STUFF((SELECT ',  ' + e.Unit From Elev e inner Join tblJoinElevJob ej on e.ID = ej.Elev Where ej.Job = @job FOR XML PATH('')), 1, 1, '')  
                IF (@EquipmentJob is not NUll)  
                Begin  
	                exec log2_insert @UpdatedBy,@Screen,@RefId,'Equipment','',@EquipmentJob  
                END  
                if(@CType is not null And @CType != '')  
                Begin  
	                exec log2_insert @UpdatedBy,@Screen,@RefId,'Service Type','',@CType  
                END 

                IF(@ProjectStageID is not null AND @ProjectStageID != 0)  
                BEGIN  
	                EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Stage','',@ProjectStageName  
                END 
				/********End Logs************/
                      
            END
			ELSE
			/******Update Job******/
			BEGIN
                DECLARE @JobStatus smallint
                -- Checking project closed before updating
                SELECT @JobStatus = IsNull(Status, 0) FROM Job WHERE ID = @Job
                IF @JobStatus = 1 OR @JobStatus = 3
                BEGIN
                    RAISERROR ('Cannot change the completed/closed project.', 16, 1)
                END

                DELETE FROM Team
                WHERE JobID = @Job

                INSERT INTO Team (Line, JobID, Title, MomUserID, FirstName, LastName, Email, Mobile)
                    SELECT
                        Line,
                        JobID,
                        Title,
                        MomUserID,
                        FirstName,
                        LastName,
                        Email,
                        Mobile
                    FROM @TeamItems

				/********Start Logs************/
				-- Adding logs in case update Job
				Declare @currProjName varchar(75)
				Declare @currCusName varchar(255)
				Declare @currlocName varchar(255)
				Declare @currTemplateName varchar(255)
				Declare @currTaskCate varchar(255)
				Declare @currDepartment varchar(255)
				Declare @currStatus varchar(75)
				Declare @currProjManager varchar(255)
				Declare @currSupervisor varchar(255)
				Declare @currRemarks varchar(Max)
				Declare @currProjCreationDate varchar(255)
				Declare @currCertified smallint
				DECLARE @currCustom1 varchar(75)
				DECLARE @currCustom2 varchar(75)
				DECLARE @currCustom3 varchar(75)
				DECLARE @currCustom4 varchar(75)
				DECLARE @currCustom5 varchar(75)
				DECLARE @currSPHandle smallint
				DECLARE @currIsRenewalNotes smallint
				DECLARE @currSRemarks varchar(max)
				DECLARE @currRenewalNote varchar(max)
                -- Finance - ExpenseGL - @InvExp        GL ,  
                -- Finance - Inerest GL - @GLInt        InterestGL,
                -- Finance - Billing Code - @InvServ    GLRev,
                -- Finance - Labor Wage - @Wage          WageC,
                -- Finance - Service Type - @ctype      CType, 
                DECLARE @currInvExp int
                DECLARE @currInvServ int
                DECLARE @currWage int
                DECLARE @currGLInt int
                DECLARE @currCType varchar(10)
                DECLARE @currStage int

				SELECT IDENTITY (INT, 0, 1) AS ID, Status INTO #tempJStatus1 FROM [JStatus]

				Select @currProjName = j.fDesc
					, @currCusName= r.Name
					, @currlocName = l.Tag
					, @currTemplateName = jt.fDesc
					, @currTaskCate = j.TaskCategory
					, @currDepartment = Isnull(jty.Type,'')
					, @currStatus = st.Status
					, @currProjManager = ISNULL(u.CallSign,'')
					, @currSupervisor = ISNULL(us.CallSign,'')
					, @currRemarks = j.Remarks
					, @currProjCreationDate = ISNULL(Convert(varchar,j.ProjCreationDate,101),'')
					, @currCertified = ISNULL(j.Certified,0)
					, @currCustom1 = ISNULL(Custom21,'')
					, @currCustom2 = ISNULL(Custom22,'')
					, @currCustom3 = ISNULL(Custom23,'')
					, @currCustom4 = ISNULL(Custom24,'')
					, @currCustom5 = ISNULL(Convert(varchar,j.Custom25,101),'')
					, @currSPHandle = ISNULL(j.SPHandle, 0)
					, @currSRemarks = ISNULL(Convert(varchar(max),j.SRemarks),'')
					--, @currRenewalNote = Convert(varchar(1000),ISNULL(j.RenewalNotes,''))
					--, @currSRemarks = Convert(varchar(1000),ISNULL(j.SRemarks,''))
					, @currIsRenewalNotes = ISNULL(j.IsRenewalNotes, 0)
					, @currRenewalNote = ISNULL(Convert(varchar(max),j.RenewalNotes),'')
                    , @currInvExp = j.GL
                    , @currInvServ = j.GLRev
                    , @currWage = j.WageC
                    , @currGLInt = j.InterestGL
                    , @currCType = j.CType
                    , @currStage = ISNULL(j.Stage,0)
				from job j 
				inner join Owner o on j.Owner = o.ID
				inner join Rol r on o.Rol = r.ID
				inner join Loc l on j.Loc = l.Loc
				left join JobT jt on j.Template = jt.ID 
				left join JobType jty on jty.ID = j.Type
				inner join #tempJStatus1 st on st.ID = j.Status
				left join Emp u on u.ID = j.ProjectManagerUserID
                left join Emp us ON us.ID = j.SupervisorUserID
				where j.ID = @job
				if(@status is not null)
					Set @statusName = isnull((SELECT Status FROM #tempJStatus1 where ID = @status),'')
				Drop table #tempJStatus1

				-- Project Name
				IF(@fdesc != @currProjName)
				BEGIN 	
					EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Project Name',@currProjName,@fdesc
                    IF @IsContractExist = 1
                        EXEC log2_insert @UpdatedBy,@ScreenContract,@RefId,'Contract Description',@currProjName,@fdesc
				END
				-- Customer
				Set @customerName = Isnull((Select Isnull(r.Name,'') from Owner o inner join Rol r on o.Rol = r.ID where o.Id = @owner),'')
				IF(@customerName != @currCusName)
				BEGIN 	
					EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Customer',@currCusName,@customerName
                    IF @IsContractExist = 1
					    EXEC log2_insert @UpdatedBy,@ScreenContract,@RefId,'Customer Name',@currCusName,@customerName
				END
				-- Location
				Set @locationName = Isnull((Select Tag from loc where loc = @loc),'')
				IF(@loc is not null And @loc != 0)
				BEGIN 	
					if(@locationName != @currlocName)
                    BEGIN
						EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Location',@currlocName,@locationName
                        IF @IsContractExist = 1
						    EXEC log2_insert @UpdatedBy,@ScreenContract,@RefId,'Location Name',@currlocName,@locationName
                    END
				END
				-- Address
				-- Template Type
				IF(@template is not null And @template != 0)
				BEGIN 	
					select  @templateName = j.fdesc from JobT j where j.ID = @template
					if(@templateName != @currTemplateName)
						EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Template Type',@currTemplateName,@templateName
				END
				-- Task Category
				IF(@taskcategory != @currTaskCate)
				BEGIN 	
					EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Task Category',@currTaskCate,@taskcategory
				END
				-- Department
				IF(@type is not null)
				BEGIN 	
					SET @typeName = Isnull((SELECT Type FROM JobType WHERE Id= @type),'')
					if(@typeName != @currDepartment)
						EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Department',@currDepartment,@typeName
				END
				-- Status
				IF(@statusName != @currStatus)
				BEGIN 	
					EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Status',@currStatus,@statusName
                    IF @IsContractExist = 1
					    EXEC log2_insert @UpdatedBy,@ScreenContract,@RefId,'Status',@currStatus,@statusName
				END
				-- Default Salesperson
				-- Salesperson 2
				-- Project Manager
				IF(@ProjectManagerUserID is not null)
				BEGIN 	
					SET @ProjManager = Isnull((select Top 1 CallSign from Emp where id=@ProjectManagerUserID ),'')
					if(@ProjManager != @currProjManager)
						EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Project Manager',@currProjManager,@ProjManager
				END
                IF(@SupervisorUserID is not null)
				BEGIN 	
					SET @Supervisor = Isnull((select Top 1 CallSign from Emp where id=@SupervisorUserID ),'')
					if(@Supervisor != @currSupervisor)
						EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Supervisor',@currSupervisor,@Supervisor
				END
				-- Remarks
				IF(@Remarks != @currRemarks)
				BEGIN 	
					EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Remarks',@currRemarks,@Remarks
                    IF @IsContractExist = 1
					    EXEC log2_insert @UpdatedBy,@ScreenContract,@RefId,'Remarks',@currRemarks,@Remarks
				END
				-- Notes
				-- Project Creation Date
				Declare @strProjCreationDate varchar(255) = ISNULL(convert(varchar, @ProjCreationDate, 101),'')
				IF(@strProjCreationDate != @currProjCreationDate)
				BEGIN 	
					EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Project Creation Date',@currProjCreationDate,@strProjCreationDate
				END

				-- Certified Job
				IF(ISNULL(@Certified, 0) != @currCertified)
				BEGIN
					Declare @strCertified varchar(10) = CASE ISNULL(@Certified, 0) WHEN 0 THEN 'False' ELSE 'True' END
					Declare @strCurrCertified varchar(10) = CASE @currCertified WHEN 0 THEN 'False' ELSE 'True' END
					EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Certified Job',@strCurrCertified,@strCertified
				END

				-- Custom 1
				IF(@Custom1 is not null And @Custom1 != @currCustom1)
				BEGIN 	
					EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Custom 1',@currCustom1,@Custom1
				END
				-- Custom 2
				IF(@Custom2 is not null And @Custom2 != @currCustom2)
				BEGIN 	
					EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Custom 2',@currCustom2,@Custom2
				END
				-- Custom 3
				IF(@Custom3 is not null And @Custom3 != @currCustom3)
				BEGIN 	
					EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Custom 3',@currCustom3,@Custom3
				END
				-- Custom 4
				IF(@Custom4 is not null And @Custom4 != @currCustom4)
				BEGIN 	
					EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Custom 4',@currCustom4,@Custom4
				END
				-- Custom 5
				Declare @strCustom5 varchar(255) = ISNULL(convert(varchar, @Custom5, 101),'')
				IF(@strCustom5 != @currCustom5)
				BEGIN 	
					EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Custom 5',@currCustom5,@strCustom5
				END

				-- Special Notes
				IF(@currSPHandle != ISNULL(@SPHandle, 0))
				BEGIN 	
					Declare @strSPHandle varchar(10) = CASE ISNULL(@SPHandle, 0) WHEN 0 THEN 'False' ELSE 'True' END
					Declare @strCurrSPHandle varchar(10) = CASE @currSPHandle WHEN 0 THEN 'False' ELSE 'True' END
					EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Special Notes',@strCurrSPHandle,@strSPHandle
                    IF @IsContractExist = 1
					    EXEC log2_insert @UpdatedBy,@ScreenContract,@RefId,'IsSpecial Notes',@strCurrSPHandle,@strSPHandle
				END
				-- Special Note Content
				IF(@vcSRemarks != @currSRemarks)
				BEGIN 	
					Declare @logSRemarksU varchar(1000) = Convert(Varchar(1000), @vcSRemarks)
					Declare @logCurrSRemarksU varchar(1000) = Convert(Varchar(1000), @currSRemarks)
					EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Special Note Content',@logCurrSRemarksU,@logSRemarksU
                    IF @IsContractExist = 1
					    EXEC log2_insert @UpdatedBy,@ScreenContract,@RefId,'Special Notes',@logCurrSRemarksU,@logSRemarksU
				END

				-- Renew Notes
				IF(@currIsRenewalNotes != ISNULL(@IsRenewalNotes, 0))
				BEGIN 	
					Declare @strIsRenewalNotes varchar(10) = CASE ISNULL(@IsRenewalNotes, 0) WHEN 0 THEN 'False' ELSE 'True' END
					Declare @strCurrIsRenewalNotes varchar(10) = CASE @currIsRenewalNotes WHEN 0 THEN 'False' ELSE 'True' END
					EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Renew Notes',@strCurrIsRenewalNotes,@strIsRenewalNotes
				END
				-- Renew Note Content
				IF(@vcRenewNotes != @currRenewalNote)
				BEGIN 	
					Declare @logRenewalNotesU varchar(1000) = Convert(Varchar(1000), @vcRenewNotes)
					Declare @logCurrRenewalNotesU varchar(1000) = Convert(Varchar(1000), @currRenewalNote)
					EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Renew Note Content',@logCurrRenewalNotesU,@logRenewalNotesU
				END

                -- Finance - General
                
                -- Finance - ExpenseGL - @InvExp
                IF(@InvExp != @currInvExp)
                BEGIN
                    DECLARE @currExpenseGL varchar(255), @ExpenseGL varchar(255)
                    SET @currExpenseGL = ISNULL((SELECT c.fDesc FROM chart c WHERE c.ID = @currInvExp),'')
                    SET @ExpenseGL = ISNULL((SELECT c.fDesc FROM chart c WHERE c.ID = @InvExp),'')
                    EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Finance - ExpenseGL',@currExpenseGL,@ExpenseGL
                END
                -- Finance - Inerest GL - @GLInt
                IF(@GLInt != @currGLInt)
                BEGIN
                    DECLARE @currInerestGL varchar(255), @InerestGL varchar(255)
                    SET @currInerestGL = ISNULL((SELECT c.fDesc FROM chart c WHERE c.ID = @currGLInt),'')
                    SET @InerestGL = ISNULL((SELECT c.fDesc FROM chart c WHERE c.ID = @GLInt),'')
                    EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Finance - Inerest GL',@currInerestGL,@InerestGL
                END
                -- Finance - Billing Code - @InvServ
                IF(@InvServ != @currInvServ)
                BEGIN
                    DECLARE @currBillCode varchar(255), @BillCode varchar(255)
                    SET @currBillCode = ISNULL((SELECT INV.Name FROM Inv WHERE Inv.ID = @currInvServ),'')
                    SET @BillCode = ISNULL((SELECT INV.Name FROM Inv WHERE Inv.ID = @InvServ),'')

                    EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Finance - Billing Code',@currBillCode,@BillCode
                END
                -- Finance - Labor Wage - @Wage
                IF(@Wage != @currWage)
                BEGIN
                    DECLARE @currLaborWage varchar(255), @LaborWage varchar(255)
                    SET @currLaborWage = ISNULL((SELECT fDesc FROM PRWage WHERE ID = @currWage),'')
                    SET @LaborWage = ISNULL((SELECT fDesc FROM PRWage WHERE ID = @Wage),'')
                    EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Finance - Labor Wage',@currLaborWage,@LaborWage
                END
                -- Finance - Service Type - @ctype
                IF(@ctype != @currCtype)
                BEGIN
                    EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Finance - Service Type',@currCtype,@ctype
                    IF @IsContractExist = 1
                        EXEC log2_insert @UpdatedBy,@ScreenContract,@RefId,'Service Type',@currCtype,@ctype
                END

                IF(@currStage != @ProjectStageID)
                BEGIN
                    DECLARE @currStageName nvarchar(255) = ''
                    SELECT @currStageName = Description FROM tblProjectStage WHERE ID = @currStage
                    EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Stage',@currStageName,@ProjectStageName
                END
                
                /********End Logs************/

                UPDATE job
                SET CloseDate = GETDATE()
                WHERE id = @Job
                AND status <> 1
                AND @status = 1


                UPDATE Job
                SET Loc = @loc,
                    Owner = @owner,
                    Remarks = @Remarks,
                    fDesc = @fdesc,
                    Template = @template,
                    type = @type,
                    status = @status,
                    PO = @PO,
                    SO = @SO,
                    Certified = @certified,
                    ProjCreationDate = @ProjCreationDate,
                    Custom21 = @Custom1,
                    Custom22 = @Custom2,
                    Custom23 = @Custom3,
                    Custom24 = @Custom4,
                    Custom25 = @Custom5,
                    Rol = @rolid,
                    LastUpdateDate = GETDATE(),
                    BillRate = @BillRate,
                    RateOT = @RateOT,
                    RateNT = @RateNT,
                    RateDT = @RateDT,
                    RateTravel = @RateTravel,
                    RateMileage = @Mileage,
                    TaskCategory = @taskcategory,
                    CType = @ctype,
                    WageC = @Wage,
                    GL = @InvExp,
                    GLRev = @InvServ,
                    Post = @Post,
                    Charge = @Charge,
                    fInt = @fInt,
					InterestGL=@GLInt,
                    SPHandle = @SPHandle,
                    SRemarks = @SPRemarks,
                    IsRenewalNotes = @IsRenewalNotes,
                    RenewalNotes = @RenewalNotes,
                    PWIP = @PWIP,
                    UnrecognizedRevenue = @UnrecognizedRevenue,
                    UnrecognizedExpense = @UnrecognizedExpense,
                    RetainageReceivable = @RetainageReceivable,
                    ArchitectName = @ArchitectName,
                    ArchitectAdress = @ArchitectAdress,
                    PType = @PType,
                    Amount = @JobAmount,
                    ProjectManagerUserID = @ProjectManagerUserID,
                    AssignedProjectUserID = @AssignedProjectUserID,
					TargetHPermission = @TargetHPermission,
					SupervisorUserID = @SupervisorUserID,
                    Stage = @ProjectStageID
                WHERE ID = @Job

                --------Ref SECO-389 If user Change project status then it should be updated contract status as well.    
                UPDATE Contract
                SET status = @status
                WHERE Job = @Job

                INSERT INTO JobTItem (JobT,
                        Job,
                        Type,
                        fDesc,
                        Code,
                        Actual,
                        Budget,
                        Line,
                        [Percent],
                        Comm,
                        Modifier,
                        ETC,
                        ETCMod,
                        Labor,
                        OrderNo)
                    SELECT
                        JobT,
                        @Job,
                        Type,
                        fDesc,
                        Code,
                        Actual,
                        Budget,
                        Line,
                        [Percent],
                        0,
                        0,
                        0,
                        0,
                        0,
                        OrderNo
                    FROM @Items

                IF (@template <> 0)
                BEGIN
                    UPDATE [dbo].[JobT]
                    SET       
                    --[GLInt] = @GLInt,
                    [JobClose] = @JobClose

                    WHERE ID = @template
                END

                CREATE TABLE #tbljobitem (
                    ItemID int,
                    GanttTaskID int,
                    EstConvertId int,
                    EstConvertLine smallint
                )

                INSERT INTO #tbljobitem
                    SELECT
                        jobitem.ID, jobitem.GanttTaskID, jobitem.EstConvertId, jobitem.EstConvertLine
                    FROM JobtItem jobitem
                    INNER JOIN BOM b
                        ON b.JobTItemID = jobitem.ID
                    WHERE job = @job --and jobitem.Type!=2            

                INSERT INTO #tbljobitem
                    SELECT
                        jobitem.ID, jobitem.GanttTaskID, jobitem.EstConvertId, jobitem.EstConvertLine
                    FROM JobtItem jobitem
                    INNER JOIN Milestone m
                        ON m.JobTItemID = jobitem.ID
                    WHERE job = @job

                -- Revert Inventory Item Trans before removing
			    --DECLARE @INV_WarehouseID varchar(50) = 'OFC';
			    INSERT INTO tblInventoryWHTrans(InvID, WarehouseID,LocationID ,Hand,Balance,fOrder,[Committed],[Available],Screen,ScreenID,Mode,Date,TransType,Batch,fDate)
			    SELECT b.MatItem, 'OFC',0,0,0,0, ISNULL(b.QtyRequired,0)*-1,0,'Project',jt.Job,'Edit',GETDATE(),'Revert',0,GETDATE() 
                FROM BOM b INNER JOIN JobTItem jt ON jt.ID = b.JobTItemID
				    WHERE jt.Job = @job and b.Type= 8

                DELETE b
                    FROM BOM b
                    INNER JOIN JobTItem j
                        ON b.JobTItemID = j.ID
                WHERE j.Job = @job
                    AND j.Type != 2

				-------------  $$$$ -- UPDATE WIP CONTRACT PRICE ----- $$$$$$$
				SELECT @OldMilestonAmount = SUM(ISNULL(Amount, 0)) FROM Milestone m
                    INNER JOIN JobTItem j
                        ON m.JobTItemID = j.ID
                WHERE j.Job = @Job

				UPDATE t1
				SET t1.ContractPrice = t1.ContractPrice - @OldMilestonAmount
				FROM ProjectWIPDetail t1
					INNER JOIN ProjectWIP t2 ON t1.WIPID = t2.ID
				WHERE t2.Period = @Period AND t2.IsPost = 0 AND t1.Job = @Job


                DELETE m
                    FROM Milestone m
                    INNER JOIN JobTItem j
                        ON m.JobTItemID = j.ID
                WHERE j.Job = @job


				INSERT INTO @tblTH
				SELECT Code , GroupID ,Job ,TargetHours--, GanttTaskID 
                FROM JobTItem  WHERE ID IN (SELECT   ItemID FROM #tbljobitem)

                DELETE FROM JobTItem
                WHERE ID IN (SELECT
                        ItemID
                    FROM #tbljobitem)  -- to delete only those jobitem which are linked with bom and milestone            

                DECLARE db_cursor CURSOR FOR

                SELECT
                    fDesc,
                    Code,
                    Line,
                    Btype,
                    QtyReq,
                    UM,
                    BudgetUnit,
                    BudgetExt,
                    LabItem,
                    MatItem,
                    LabMod,
                    MatMod,
                    LabExt,
                    LabRate,
                    LabHours,
                    SDate,
                    VendorId,
                    MatDesc,
                    OrderNo,
                    GroupID,
                    t.GanttTaskID,
                    t.EstConvertId,
                    t.EstConvertLine
                FROM @BomItem b left join #tbljobitem t ON t.ItemID = JobTItemID

                OPEN db_cursor
                FETCH NEXT FROM db_cursor INTO @jfdesc, @jcode, @Line, @Btype, @QtyReq, @UM, @BudgetUnit, @BudgetExt, @LabItem, @MatItem,
                    @LabMod, @MatMod, @LabExt, @LabRate, @LabHours, @SDate, @Vendor, @MatDesc, @OrderNo, @GroupID,@GanttTaskID,
                    @EstConvertId,@EstConvertLine

                WHILE @@FETCH_STATUS = 0
                BEGIN
                    IF (@MatItem IS NULL
                        OR @MatItem = ''
                        OR @MatItem = 0)
                    BEGIN
                        -- add into inv table (as non inventory type) and add as bom item      
                        IF EXISTS (SELECT TOP 1
                                1
                            FROM inv
                            WHERE Name = @MatDesc
                            AND fDesc = @jfDesc) -- check if item name and description is already exists!      
                        BEGIN
                            SET @MatItem = (SELECT TOP 1
                                ID
                            FROM inv
                            WHERE Name = @MatDesc
                            AND fDesc = @jfdesc
                            AND type = 2)
                        END
                        ELSE
                        BEGIN
                            IF (@MatDesc IS NOT NULL
                                AND @MatDesc <> '')
                            BEGIN
                                SET @GLRev = ISNULL((SELECT
                                    SAcct
                                FROM Job job
                                INNER JOIN Inv inv
                                    ON job.GLRev = inv.ID
                                WHERE job.ID = @Job)
                                , 0)
                                INSERT INTO Inv (Name, fdesc, Cat, Balance, Measure, Tax, AllowZero, InUse, Type, Sacct, Status, Price1)
                                    VALUES (@MatDesc, @jfdesc, 0, 0, 'Each', 0, 0, 0, 2, @GLRev, 0, 0)
                                SET @MatItem = SCOPE_IDENTITY()
                            END
                        END
                    END
                    IF (@jCode = '999')
                    BEGIN
                        UPDATE POItem
                        SET TypeID = @Btype
                        WHERE Job = @Job
                        AND Phase = @Line
                        AND fDesc = @jfDesc
                    END
                    INSERT INTO JobTItem (JobT, Job, Type, fDesc, Code, Actual, Budget,
                        Line, [Percent], Comm, Modifier, ETC, ETCMod, Labor, Stored, BHours, OrderNo, GroupID, GanttTaskID,
                        EstConvertId, EstConvertLine)
                    VALUES (@template, @Job, 1, @jfDesc, @jCode, 0, @BudgetExt, 
                        @Line, 0, 0, @MatMod, @LabExt, @LabMod, 0, 0, @LabHours, @OrderNo, @GroupID, @GanttTaskID,
                        @EstConvertId, @EstConvertLine)
                    SET @jobTItemId = SCOPE_IDENTITY()


                    -- JobTItem.Type = 0 is revenue type            
                    -- JobTItem.Type = 1 is expense type            

                    INSERT INTO [dbo].[BOM] ([JobTItemID], [Type], [QtyRequired], [UM], [BudgetUnit], [BudgetExt], [LabItem], [MatItem], [LabRate], [SDate], [Vendor])
                        VALUES (@jobTItemId, @Btype, @QtyReq, @UM, @BudgetUnit, @BudgetExt, @LabItem, @MatItem, @LabRate, @SDate, @Vendor)

                    -- Update Gantt Task Vendor
                    IF (@Vendor is not null AND @Vendor!=0)
                    BEGIN
                        UPDATE g SET g.VendorID=@Vendor 
                            , g.Vendor = (SELECT TOP 1 r.Name FROM Vendor v LEFT JOIN Rol r ON r.ID = v.Rol WHERE v.ID = @Vendor)
                        FROM GanttTasks g 
                        WHERE g.ID = @GanttTaskID
                    END
                    ELSE
                    BEGIN
                        UPDATE g SET g.VendorID=0 
                            , g.Vendor = null
                        FROM GanttTasks g 
                        WHERE g.ID = @GanttTaskID AND ISNULL(g.VendorID,0) != 0
                    END
                    --------- Insert into tblInventoryWHTrans-------------
			        -- Inventory
			        IF EXISTS (SELECT 1 FROM Inv Where Type = 0 AND ID =@MatItem)
			        BEGIN
				        INSERT INTO tblInventoryWHTrans(InvID, WarehouseID,LocationID ,Hand,Balance,fOrder,[Committed],[Available],Screen,ScreenID,Mode,Date,TransType,Batch,FDate)
				        VALUES (@MatItem,'OFC',0,0,0,0,@QtyReq,0,'Project',@job,'Edit',GETDATE(),'In',0,GETDATE())
			        END			        
			        --------- End Insert into tblInventoryWHTrans----------

                    FETCH NEXT FROM db_cursor INTO @jfdesc, @jcode, @Line, @Btype, @QtyReq, @UM, @BudgetUnit, @BudgetExt, @LabItem, @MatItem,
                        @LabMod, @MatMod, @LabExt, @LabRate, @LabHours, @SDate, @Vendor, @MatDesc, @OrderNo, @GroupID,@GanttTaskID,
                        @EstConvertId,@EstConvertLine
                END

                CLOSE db_cursor
                DEALLOCATE db_cursor

                DECLARE db_cursor1 CURSOR FOR

                SELECT
                    jtype,
                    fdesc,
                    jCode,
                    Line,
                    MilesName,
                    RequiredBy,
                    LeadTime,
                    Type,
                    Amount,
                    OrderNo,
                    GroupID,
                    Quantity,
                    Price,
                    t.EstConvertId,
                    t.EstConvertLine,
                    ChangeOrder
                FROM #MilestonItem left join #tbljobitem t ON t.ItemID = jobTItem

                OPEN db_cursor1
                FETCH NEXT FROM db_cursor1 INTO @jType, @jfdesc, @jcode, @Line, @MileName, @RequiredBy, @LeadTime, @OrgDep, @Amount, @OrderNo
                    , @GroupID, @MQuantity, @MPrice
                    , @EstConvertId, @EstConvertline,@MChangeOrder
                WHILE @@FETCH_STATUS = 0
                BEGIN
                    INSERT INTO JobTItem (JobT,
                        Job,
                        Type,
                        fDesc,
                        Code,
                        Actual,
                        Budget,
                        Line,
                        [Percent],
                        Comm,
                        Modifier,
                        ETC,
                        ETCMod,
                        Labor,
                        Stored,
                        OrderNo,
                        GroupID,
                        EstConvertId,
                        EstConvertLine)
                    VALUES (@template, @Job, @jType, @jfDesc, @jCode, 0, @Amount, @Line, 0, 0, 0, 0, 0, 0, 0, @OrderNo, @GroupID
                        , @EstConvertId, @EstConvertline)
                    SET @jobTItemId = SCOPE_IDENTITY()


                    INSERT INTO [dbo].[Milestone] ([JobTItemID]
                        , [MilestoneName]
                        , [RequiredBy]
                        , [CreationDate]
                        , [ProjAcquistDate]
                        , [Type]
                        , [Amount]
                        , [Quantity]
                        , [Price]
                        , [ChangeOrder])
                    VALUES (@jobTItemId, @MileName, @RequiredBy, GETDATE(), @ProjAcquDate, @OrgDep, @Amount, @MQuantity, @MPrice,@MChangeOrder)

					-------------  $$$$ -- UPDATE WIP CONTRACT PRICE ----- $$$$$$$
					UPDATE t1
					SET t1.ContractPrice = t1.ContractPrice + @Amount
					FROM ProjectWIPDetail t1
						INNER JOIN ProjectWIP t2 ON t1.WIPID = t2.ID
					WHERE t2.Period = @Period AND t2.IsPost = 0 AND t1.Job = @Job

                    FETCH NEXT FROM db_cursor1 INTO @jType, @jfdesc, @jcode, @Line, @MileName, @RequiredBy, @LeadTime, @OrgDep, @Amount, @OrderNo
                        , @GroupID, @MQuantity, @MPrice
                        , @EstConvertId, @EstConvertline,@MChangeOrder
                END

                CLOSE db_cursor1
                DEALLOCATE db_cursor1


				UPDATE t2 set t2.TargetHours=t1.TargetHours from   @tblTH t1 inner join JobTItem t2 on t1.Job=t2.Job  and t1.code=t2.Code and t1.GroupID=t2.GroupID and t2.Type=1

                -- ReUpdate Gantt chart ID for JobTItem
                --UPDATE t2 set t2.GanttTaskID=t1.GanttTaskID FROM   @tblTH t1 inner join JobTItem t2 on t1.Job=t2.Job  and t1.code=t2.Code and t1.GroupID=t2.GroupID and (t2.Type = 0 OR t2.Type = 1) 

                ----------------------------------- tblCustomJob --------------------------------------  
                --------------------------------------- start -----------------------------------------  

                --SELECT * FROM @CustomItem ctItem INNER JOIN tblCustomJob ON   

                --delete from tblCustomJob where JobID = @job            
                -- update custom details for project    

                DECLARE db_cursor2 CURSOR FOR

                SELECT
                    [ID],
                    [tblTabID],
                    [Label],
                    [Line],
                    [Value],
                    [Format],
                    [UpdatedDate],
                    [Username],
                    IsAlert,
                    IsTask,
                    TeamMember,
                    TeamMemberDisplay,
                    UserRole,
                    UserRoleDisplay
                FROM @CustomItem

                OPEN db_cursor2
                FETCH NEXT FROM db_cursor2 INTO @tblCustomFieldsId, @tblTabID, @Label, @TabLine, @Value, @Format, @UpdatedDate, @Username, @IsAlert, @IsTask, @TeamMember, @TeamMemberDisplay, @UserRole, @UserRoleDisplay

                WHILE @@FETCH_STATUS = 0
                BEGIN
                    IF EXISTS (SELECT
                            1
                        FROM [tblCustomJob]
                        WHERE tblCustomFieldsID = @tblCustomFieldsId
                        AND JobID = @Job)
                    BEGIN
                  
                        IF EXISTS (SELECT
                                1
                            FROM [tblCustomJob]
                            WHERE tblCustomFieldsID = @tblCustomFieldsId
                            AND JobID = @Job
                            AND ((@Format != 5
                            AND ISNULL([Value], '') != ISNULL(@Value, ''))
                            OR (@Format = 5
                            AND ISNULL([Value], 'False') != ISNULL(@Value, 'False'))))
                        BEGIN
                            UPDATE [dbo].[tblCustomJob]
                            SET [Value] = @Value,
                                [IsAlert] = @IsAlert,
                                [IsTask] = @IsTask,
                                [TeamMember] = @TeamMember,
                                [TeamMemberDisplay] = @TeamMemberDisplay,
                                [UserRole] = @UserRole,
                                [UserRoleDisplay] = @UserRoleDisplay,
                                [UpdatedDate] = @UpdatedDate,
                                [Username] = @Username
                            WHERE tblCustomFieldsID = @tblCustomFieldsId
                            AND JobID = @Job
                            AND ((@Format != 5
                            AND ISNULL([Value], '') != ISNULL(@Value, ''))
                            OR (@Format = 5
                            AND ISNULL([Value], 'False') != ISNULL(@Value, 'False')))
                        END
                        ELSE
                        BEGIN
                            UPDATE [dbo].[tblCustomJob]
                            SET [Value] = @Value,
                                [IsAlert] = @IsAlert,
                                [IsTask] = @IsTask,
                                [TeamMember] = @TeamMember,
                                [TeamMemberDisplay] = @TeamMemberDisplay,
                                [UserRole] = @UserRole,
                                [UserRoleDisplay] = @UserRoleDisplay
                            --[UpdatedDate] = @UpdatedDate,
                            --[Username] = @Username
                            WHERE tblCustomFieldsID = @tblCustomFieldsId
                            AND JobID = @Job
                        END
                    END
                    ELSE
                    BEGIN
                        IF ((@Format != 5
                            AND '' != ISNULL(@Value, ''))
                            OR (@Format = 5
                            AND 'False' != ISNULL(@Value, 'False')))
                        --OR 0 != @IsAlert OR ''!=ISNULL(@TeamMember,'') OR '' != ISNULL(@TeamMemberDisplay,''))
                        BEGIN
                            INSERT INTO [dbo].[tblCustomJob] ([JobID]
                            , [tblCustomFieldsID]
                            , [Value]
                            , [UpdatedDate]
                            , [Username]
                            , [IsAlert]
                            , [IsTask]
                            , [TeamMember]
                            , [TeamMemberDisplay]
                            , [UserRole]
                            , [UserRoleDisplay]
                            )
                            VALUES (@Job, @tblCustomFieldsId, @Value, @UpdatedDate, @Username, @IsAlert, @IsTask, @TeamMember, @TeamMemberDisplay, @UserRole, @UserRoleDisplay)
                        END
                        ELSE
                        BEGIN
                            INSERT INTO [dbo].[tblCustomJob] ([JobID]
                            , [tblCustomFieldsID]
                            , [Value]
                            , [IsAlert]
                            , [IsTask]
                            , [TeamMember]
                            , [TeamMemberDisplay]
                            , [UserRole]
                            , [UserRoleDisplay]
                            )
                            VALUES (@Job, @tblCustomFieldsId, @Value, @IsAlert,@IsTask, @TeamMember, @TeamMemberDisplay, @UserRole, @UserRoleDisplay)
                        END
                    END
                    FETCH NEXT FROM db_cursor2 INTO @tblCustomFieldsId, @tblTabID, @Label, @TabLine, @Value, @Format, @UpdatedDate, @Username, @IsAlert,@IsTask, @TeamMember, @TeamMemberDisplay, @UserRole, @UserRoleDisplay
                END

                CLOSE db_cursor2
                DEALLOCATE db_cursor2
				--------------------------------------- end -----------------------------------------  
            END

            -- Delete the old value if existed
            DELETE FROM tblCustomJob
            WHERE JobID = @job
                AND ([tblCustomFieldsID] NOT IN (SELECT
                    [ID]
                FROM @CustomItem)
                )

            /********/

            SET @bHour = ISNULL((SELECT SUM(ISNULL(BHours, 0)) FROM jobtitem WHERE type = 1 AND job = @job), 0)

            SET @brev = ISNULL((SELECT SUM(ISNULL(Budget, 0)) FROM JobTItem WHERE Type = 0 AND Job = @job), 0)

            SET @bcost = ISNULL((SELECT (SUM(ISNULL(Budget, 0)) + SUM(ISNULL(Modifier, 0)) + SUM(ISNULL(ETC, 0)) + SUM(ISNULL(ETCMod, 0)))
								FROM JobTItem WHERE Type = 1 AND Job = @job), 0)

            SET @bmat = ISNULL((SELECT (SUM(ISNULL(Budget, 0)) + SUM(ISNULL(Modifier, 0)))
								FROM JobTItem
								INNER JOIN bom
									ON bom.JobTItemID = JobTItem.ID
									INNER JOIN BOMT
										ON bomt.ID = bom.Type
								WHERE (bomt.Type = 'Materials'
									OR bomt.Type = 'Inventory')
									AND Job = @job), 0)

            SET @bOther = ISNULL((SELECT (SUM(ISNULL(Budget, 0)) + SUM(ISNULL(Modifier, 0)))
								FROM JobTItem
								INNER JOIN bom
									ON bom.JobTItemID = JobTItem.ID
									INNER JOIN BOMT
										ON bomt.ID = bom.Type
								WHERE bomt.Type <> 'Materials'
									--AND bomt.Type <> 'Labor'
									AND bomt.Type <> 'Inventory'
									AND Job = @job), 0)

            SET @blabor = ISNULL((SELECT (SUM(ISNULL(j.ETC, 0)) + SUM(ISNULL(j.ETCMod, 0)))
            FROM JobTItem j
            WHERE Type = 1 AND Job = @job), 0)

            SET @bprofit = @brev - @bcost

            IF @brev <> 0
            BEGIN
                SET @bratio = CONVERT(numeric(30, 2), ((@bprofit / @brev) * 100))
            END
            ELSE
            BEGIN
                SET @bratio = 0
            END

			--Delete WipDetail
			Delete from WIPDetails where ID in (
			select wd.ID from WIPDetails wd
			inner join WIPHeader wh on wh.id=wd.WIPId and wh.JobId=@job
			and line not in (select line from JobTItem jsub where jsub.Job =@job  and jsub.Type = 0))

			/*** Logs for updating buget values ***/
			
            Declare @CurrBHour    numeric(30, 2)
            Declare @CurrBRev     numeric(30, 2)
            Declare @CurrBMat     numeric(30, 2)
            Declare @CurrBLabor   numeric(30, 2)
            Declare @CurrBCost    numeric(30, 2)
            Declare @CurrBProfit  numeric(30, 2)
            Declare @CurrBRatio   numeric(30, 2)
            Declare @CurrBOther   numeric(30, 2)
            

			Select 
				@CurrBHour   = BHour   ,
                @CurrBRev    = BRev    ,
                @CurrBMat    = BMat    ,
                @CurrBLabor  = BLabor  ,
                @CurrBCost   = BCost   ,
                @CurrBProfit = BProfit ,
                @CurrBRatio  = BRatio  ,
                @CurrBOther  = IsNULL(BOther,0)  
			FROM Job
            WHERE ID = @job
			 
			-- Budget - Hours
			IF @CurrBHour != @bHour
			BEGIN
				DECLARE @strCurrBHour varchar(50)
				SET @strCurrBHour = Convert(varchar(50),@CurrBHour)
				DECLARE @strbHour varchar(50)
				SET @strbHour = Convert(varchar(50),@bHour)
				EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Budget - Hours',@strCurrBHour,@strbHour
                --IF @IsContractExist = 1
                --    EXEC log2_insert @UpdatedBy,@ScreenContract,@RefId,'Budget - Hours',@strCurrBHour,@strbHour
			END
			-- Budget - Revenue
			IF @CurrBRev != @bRev
			BEGIN
				DECLARE @strCurrBRev varchar(50)
				SET @strCurrBRev = Convert(varchar(50),@CurrBRev)
				DECLARE @strbRev varchar(50)
				SET @strbRev = Convert(varchar(50),@bRev)
				EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Budget - Revenue',@strCurrBRev,@strbRev
        --        IF @IsContractExist = 1
				    --EXEC log2_insert @UpdatedBy,@ScreenContract,@RefId,'Budget - Revenue',@strCurrBRev,@strbRev
			END

			-- Budget - Material Expense
			IF @CurrBMat != @bMat
			BEGIN
				DECLARE @strCurrBMat varchar(50)
				SET @strCurrBMat = Convert(varchar(50),@CurrBMat)
				DECLARE @strbMat varchar(50)
				SET @strbMat = Convert(varchar(50),@bMat)
				EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Budget - Material Expense',@strCurrBMat,@strbMat
        --        IF @IsContractExist = 1
				    --EXEC log2_insert @UpdatedBy,@ScreenContract,@RefId,'Budget - Material Expense',@strCurrBMat,@strbMat
				    
			END

			-- Budget - Labor Expense
			IF @CurrBLabor != @bLabor
			BEGIN
				DECLARE @strCurrBLabor varchar(50)
				SET @strCurrBLabor = Convert(varchar(50),@CurrBLabor)
				DECLARE @strbLabor varchar(50)
				SET @strbLabor = Convert(varchar(50),@bLabor)
				EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Budget - Labor Expense',@strCurrBLabor,@strbLabor
        --        IF @IsContractExist = 1
				    --EXEC log2_insert @UpdatedBy,@ScreenContract,@RefId,'Budget - Labor Expense',@strCurrBLabor,@strbLabor
			END

			-- Budget - Total Expense
			IF @CurrBCost != @bCost
			BEGIN
				DECLARE @strCurrBCost varchar(50)
				SET @strCurrBCost = Convert(varchar(50),@CurrBCost)
				DECLARE @strbCost varchar(50)
				SET @strbCost = Convert(varchar(50),@bCost)
				EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Budget - Total Expense',@strCurrBCost,@strbCost
        --        IF @IsContractExist = 1
				    --EXEC log2_insert @UpdatedBy,@ScreenContract,@RefId,'Budget - Total Expense',@strCurrBCost,@strbCost
			END

			-- Budget - Profit
			IF @CurrBProfit != @bProfit
			BEGIN
				DECLARE @strCurrBProfit varchar(50)
				SET @strCurrBProfit = Convert(varchar(50),@CurrBProfit)
				DECLARE @strbProfit varchar(50)
				SET @strbProfit = Convert(varchar(50),@bProfit)
				EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Budget - Profit',@strCurrBProfit,@strbProfit
        --        IF @IsContractExist = 1
				    --EXEC log2_insert @UpdatedBy,@ScreenContract,@RefId,'Budget - Profit',@strCurrBProfit,@strbProfit
			END

			-- Budget - % in Profit
			IF @CurrBRatio != @bRatio
			BEGIN
				DECLARE @strCurrBRatio varchar(50)
				SET @strCurrBRatio = Convert(varchar(50),@CurrBRatio)
				DECLARE @strbRatio varchar(50)
				SET @strbRatio = Convert(varchar(50),@bRatio)
				EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Budget - % in Profit',@strCurrBRatio,@strbRatio
        --        IF @IsContractExist = 1
				    --EXEC log2_insert @UpdatedBy,@ScreenContract,@RefId,'Budget - % in Profit',@strCurrBRatio,@strbRatio
			END

			-- Budget - Other Expense
			IF @CurrBOther != @BOther
			BEGIN
				DECLARE @strCurrBOther varchar(50)
				SET @strCurrBOther = Convert(varchar(50),@CurrBOther)
				DECLARE @strBOther varchar(50)
				SET @strBOther = Convert(varchar(50),@BOther)
				EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Budget - Other Expense',@strCurrBOther,@strBOther
        --        IF @IsContractExist = 1
				    --EXEC log2_insert @UpdatedBy,@ScreenContract,@RefId,'Budget - Other Expense',@strCurrBOther,@strBOther
			END

			/*** End logs ***/

            UPDATE Job
            SET BHour = @bHour,
                BRev = @bRev,
                BMat = @bMat,
                BLabor = @bLabor,
                BCost = @bCost,
                BProfit = @bProfit,
                BRatio = @bRatio,
                BOther = @bOther
            WHERE ID = @job

            IF (@job IS NOT NULL)
            BEGIN
                IF (@job <> 0)
                BEGIN
                    DELETE FROM Ticket_Task_Codes
                    WHERE job = @job
                    INSERT INTO Ticket_Task_Codes (task_code, Type, job, Category, username, dateupdated, ticket_id, default_code)
                        SELECT
                            task_code,
                            1,
                            @job,
                            Category,
                            username,
                            dateupdated,
                            ticket_id,
                            1
                        FROM @TaskCodes

                    --Ref SECO-285 if for the project  Tickets Chargeable box is checked then need to make all tickets chargeable. 
                    --Excluding invoiced Ticket.     
                    IF (@Charge = 1)
                    BEGIN
                        UPDATE TicketO SET Charge = 1 WHERE job = @job
                        UPDATE TicketD SET Charge = 1 WHERE job = @job AND (ISNULL(Invoice, 0) = 0 AND ISNULL(ManualInvoice, '0') = '0')
                        UPDATE TicketDPDA SET Charge = 1 WHERE job = @job AND (ISNULL(Invoice, 0) = 0)
                    END
                END
            END
        COMMIT
    END TRY
    BEGIN CATCH
        --SELECT ERROR_MESSAGE()
        --IF @@TRANCOUNT > 0
        --    ROLLBACK
        --RAISERROR ('An error has occurred on this page.', 16, 1)

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

    EXEC [dbo].[spUpdateJobcostByJob] @job
    EXEC CalculateInventory

	update t  set t.Count = isnull( (select Count(1) from Job where Template=t.id and Status=0) ,0) from JobT t

    RETURN @job