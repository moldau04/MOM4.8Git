CREATE PROCEDURE [dbo].[spUpdateTicket]  
 @LocID int,  
 @LocTag varchar(50),  
 @LocAdd varchar(255),  
 @City varchar(50),  
 @State varchar(2),  
 @Zip varchar(100),  
 @Phone varchar(28),  
 @Cell varchar(50),  
 @Worker varchar(50),  
 @CallDt datetime,  
 @SchDt datetime,  
 @Status smallint,  
 @EnrouteTime datetime,  
 @Onsite datetime,  
 @Complete datetime,  
 @Category varchar(25),  
 @Unit int,  
 @Reason text,  
 @CustName varchar(50),  
 @custID int,  
 @TicketID int,  
 @EST numeric(30, 2),  
 @complDesc text,  
 @Reg numeric(30, 2),  
 @OT numeric(30, 2),  
 @NT numeric(30, 2),  
 @TT numeric(30, 2),  
 @DT numeric(30, 2),  
 @Total numeric(30, 2),  
 @Charge int,  
 @Review int,  
 @Who varchar(30),  
 @sign image,  
 @remarks text,  
 @Type int,  
 @Custom1 varchar(50),  
 @Custom2 varchar(50),  
 @Custom3 varchar(50),  
 @Custom4 varchar(50),  
 @Custom5 varchar(50),  
 @Custom6 tinyint,  
 @Custom7 tinyint,  
 @WorkOrder varchar(10),  
 @WorkComplete int,  
 @MiscExp numeric(30, 2),  
 @TollExp numeric(30, 2),  
 @ZoneExp numeric(30, 2),  
 @MileStart int,  
 @MileEnd int,  
 @Internet smallint,  
 @Invoice varchar(50),  
 @TransferTime int,  
 @CreditHold tinyint,  
 @DispAlert tinyint,  
 @CreditReason text,  
 @QBServiceItem varchar(100),  
 @QBPayrollItem varchar(100),  
 @LastUpdatedBy varchar(50),  
 @Contact varchar(50),  
 @Recommendation varchar(255),  
 @Customtick1 varchar(50),  
 @Customtick2 varchar(50),  
 @Customtick3 tinyint,  
 @Customtick4 tinyint,  
 @lat varchar(50),  
 @lng varchar(50),  
 @DefaultRoute int,  
 @Customtick5   VARCHAR(100),
 @job int,  
 @JobCode varchar(255),  
 @ProjectTemplate int,  
 @wage int,  
 @fBy varchar(50),  
 @Equipments AS tblTypeMultipleEequipments READONLY,  
 @UpdateTasks smallint = 0,  
 @TaskCodes AS tblTypeTaskCodes READONLY,  
 @BT numeric(30, 2) = 0,  
 @Comments varchar(1000) = NULL,  
 @PartsUsed varchar(100) = NULL,  
 @IsCreateJob int = 0,  
 @Zone int = NULL,  
 @Level int = 1,  
 @dtTicketINV AS tblTypeTicketINV READONLY ,
 @ClearPR int = 0 
AS  
  
    DECLARE @CurrentOwner varchar(100)  
    DECLARE @CurrentLocName varchar(100)  
    DECLARE @CurrentWorkOrder varchar(10)  
    DECLARE @CurrentInvoice varchar(50)  
    DECLARE @CurrentLocAdd varchar(255)  
    DECLARE @CurrentCity varchar(50)  
    DECLARE @CurrentState varchar(2)  
    DECLARE @CurrentZip varchar(100)  
    DECLARE @CurrentContact varchar(50)  
    DECLARE @CurrentWho varchar(30)  
    DECLARE @CurrentPhone varchar(28)  
    DECLARE @CurrentCell varchar(50)  
    DECLARE @CurrentfBy varchar(50)  
    DECLARE @CurrentCallDt varchar(50)  
    DECLARE @CurrentCallDtTime varchar(50)  
    DECLARE @CurrentDefaultRoute varchar(50)  
    DECLARE @CurrentCategory varchar(25)  
    DECLARE @CurrentReason varchar(1000)  
    DECLARE @CurrentStatus varchar(50)  
    DECLARE @CurrentSchDt varchar(50)  
    DECLARE @CurrentSchDtTime varchar(50)  
    DECLARE @CurrentEST varchar(30)  
    DECLARE @CurrentWorker varchar(50)  
    DECLARE @CurrentDispAlert varchar(10)  
    DECLARE @CurrentCreditHold varchar(10)  
    DECLARE @CurrentCreditReason varchar(1000)  
    DECLARE @CurrentRemarks varchar(1000)  
    DECLARE @CurrentWorkComplete varchar(10)  
    DECLARE @CurrentReview varchar(10)  
	DECLARE @CurrentClearPR varchar(10)  
    DECLARE @CurrentCharge varchar(10)  
    DECLARE @CurrentTransferTime varchar(10)  
    DECLARE @CurrentInternet varchar(10)  
    DECLARE @CurrentcomplDesc varchar(1000)  
    DECLARE @CurrentReg varchar(30)  
    DECLARE @CurrentOT varchar(30)  
    DECLARE @CurrentEnrouteTime varchar(50)  
    DECLARE @CurrentNT varchar(30)  
    DECLARE @CurrentDT varchar(30)  
    DECLARE @CurrentOnsite varchar(50)  
    DECLARE @CurrentTT varchar(30)  
    DECLARE @CurrentBT varchar(30)  
    DECLARE @CurrentComplete varchar(50)  
    DECLARE @Currentjob varchar(30)  
    DECLARE @CurrentJobCode varchar(10)  
    DECLARE @CurrentType varchar(50)  
    DECLARE @Currentwage varchar(50)  
    DECLARE @CurrentQBServiceItem varchar(100)  
    DECLARE @CurrentQBPayrollItem varchar(100)  
    DECLARE @CurrentRecommendation varchar(255)  
    DECLARE @CurrentPartsUsed varchar(100)  
    DECLARE @CurrentComments varchar(1000)  
    DECLARE @CurrentMiscExp varchar(30)  
    DECLARE @CurrentMileStart varchar(30)  
    DECLARE @CurrentZoneExp varchar(30)  
    DECLARE @CurrentMileEnd varchar(30)  
    DECLARE @CurrentTollExp varchar(30)  
    DECLARE @CurrentCustom1 varchar(50)  
    DECLARE @CurrentCustom2 varchar(50)  
    DECLARE @CurrentCustom3 varchar(50)  
    DECLARE @CurrentCustom4 varchar(50)  
    DECLARE @CurrentCustom5 varchar(50)  
    DECLARE @CurrentCustom6 varchar(10)  
    DECLARE @CurrentCustom7 varchar(10)  
    DECLARE @CurrentCustomtick1 varchar(50)  
    DECLARE @CurrentCustomtick2 varchar(50)  
    DECLARE @CurrentCustomtick5 varchar(30)  
    DECLARE @CurrentCustomtick3 varchar(10)  
    DECLARE @CurrentCustomtick4 varchar(10)  
    DECLARE @CurrentEquipment varchar(1000)  
      
    SELECT   @CurrentOwner = r.Name    FROM Rol r WITH (NOLOCK)  
 INNER JOIN Owner o     WITH (NOLOCK) ON o.Rol = r.ID  
 INNER JOIN TicketO tio WITH (NOLOCK) ON tio.Owner = o.ID  
 WHERE tio.ID = @TicketID  
 --WHERE o.ID = (SELECT Owner FROM TicketO WHERE ID = @TicketID)  
  
    SELECT    @CurrentLocName = tag  
    FROM loc l WITH (NOLOCK) INNER JOIN TicketO tio WITH (NOLOCK) ON tio.LID = l.Loc  
    WHERE tio.ID = @TicketID  
    --WHERE loc = (SELECT LID FROM TicketO WHERE ID = @TicketID)  
      
    SELECT    @CurrentContact = r.Contact  
    FROM Rol r WITH (NOLOCK) INNER JOIN loc l WITH (NOLOCK) ON l.Rol = r.ID  
 WHERE l.Loc = @LocID  
 --WHERE ID = (SELECT TOP 1        Rol    FROM Loc    WHERE Loc = @LocID)  
  
    SELECT     @CurrentDefaultRoute = Name  
    FROM Route r WITH (NOLOCK) INNER JOIN loc l WITH (NOLOCK) ON l.Route = r.ID  
 WHERE l.Loc = @LocID  
    --WHERE ID = (SELECT TOP 1        Route    FROM Loc    WHERE Loc = @LocID)  
      
    SELECT  
        @CurrentRemarks = Remarks,  
	  @CurrentCreditReason = CreditReason,  
	  @CurrentCreditHold = Credit,  
	  @CurrentDispAlert = DispAlert  
    FROM Loc  WITH (NOLOCK)     WHERE Loc = @LocID  
  
    SELECT  
  @CurrentInvoice = ManualInvoice,  
        @CurrentWorkComplete = WorkComplete,  
  @CurrentReview = ClearCheck, 
  @CurrentClearPR = ClearPR,
  @CurrentCharge = Charge,  
  @CurrentTransferTime = TransferTime,  
  @CurrentInternet = Internet,  
  @CurrentcomplDesc = DescRes,  
  @CurrentReg = Reg,  
  @CurrentOT = OT,  
  @CurrentNT = NT,  
  @CurrentDT = DT,  
  @CurrentTT = TT,  
  @CurrentBT = break_time,  
  @CurrentJobCode = JobCode,  
  @CurrentPartsUsed = PartsUsed,  
  @CurrentComments = Comments,  
        @CurrentMiscExp = OtherE,  
        @CurrentMileStart = SMile,  
        @CurrentZoneExp = Zone,  
        @CurrentMileEnd = EMile,  
        @CurrentTollExp = Toll  
    FROM TicketD  WITH (NOLOCK)
    WHERE ID = @TicketID  
      
    SELECT  
  @CurrentWorkOrder = WorkOrder,  
        @CurrentLocAdd = LDesc3,  
        @CurrentCity = City,  
        @CurrentState = State,  
        @CurrentZip = Zip,  
        @CurrentWho = Who,  
        @CurrentPhone = Phone,  
        @CurrentCell = CPhone,  
        @CurrentfBy = fBy,  
        @CurrentCallDt = CONVERT(varchar(50), CDate, 101),  
        @CurrentCallDtTime = FORMAT(Cdate, 'hh:mm tt'),  
        @CurrentCategory = Cat,  
        @CurrentReason = fDesc,  
        @CurrentStatus =  
                        CASE Assigned  
                            WHEN 0 THEN 'Un-Assigned'  
                            WHEN 1 THEN 'Assigned'  
                            WHEN 2 THEN 'Enroute'  
                            WHEN 3 THEN 'Onsite'  
                            WHEN 4 THEN 'Completed'  
                            WHEN 5 THEN 'Hold'  
                        END,  
        @CurrentSchDt = CONVERT(varchar(50), EDate, 101),  
        @CurrentSchDtTime = FORMAT(EDate, 'hh:mm tt'),  
        @CurrentEST = Est,  
        @CurrentWorker = DWork,  
        @CurrentEnrouteTime = FORMAT(TimeRoute, 'hh:mm tt'),  
        @CurrentOnsite = FORMAT(TimeSite, 'hh:mm tt'),  
        @CurrentComplete = FORMAT(TimeComp, 'hh:mm tt'),  
        @Currentjob = Job,  
        @CurrentRecommendation = BRemarks,  
        @CurrentCustom1 = Custom1,  
        @CurrentCustom2 = Custom2,  
        @CurrentCustom3 = Custom3,  
        @CurrentCustom4 = Custom4,  
        @CurrentCustom5 = Custom5,  
        @CurrentCustom6 = Custom6,  
        @CurrentCustom7 = Custom7,  
        @CurrentCustomtick1 = CustomTick1,  
        @CurrentCustomtick2 = CustomTick2,  
        @CurrentCustomtick5 = CustomTick5,  
        @CurrentCustomtick3 = CustomTick3,  
        @CurrentCustomtick4 = CustomTick4  
  
    FROM TicketO  WITH (NOLOCK)
    WHERE ID = @TicketID  
     
    SELECT  
        @CurrentType = jt.Type  
    FROM jobtype jt WITH (NOLOCK) INNER JOIN TicketO tio WITH (NOLOCK) on tio.Type = jt.ID   
 WHERE tio.ID = @TicketID  
  
    SELECT  
        @Currentwage = pr.fdesc  
    FROM PRWage pr WITH (NOLOCK) INNER JOIN TicketD tid WITH (NOLOCK) ON tid.WageC = pr.ID  
 WHERE tid.ID = @TicketID  
    --WHERE ID = (SELECT WageC FROM TicketD WHERE ID = @TicketID)  
  
    SELECT  
        @CurrentQBServiceItem = Inv.Name  
    FROM Inv WITH (NOLOCK) INNER JOIN TicketO tio WITH (NOLOCK) ON tio.QBServiceItem = Inv.QBInvID  
 WHERE tio.ID = @TicketID  
    --WHERE QBInvID = (SELECT QBServiceItem FROM TicketO WHERE ID = @TicketID)  
  
    SELECT  
        @CurrentQBPayrollItem = pr.fdesc  
    FROM prwage pr WITH (NOLOCK) INNER JOIN TicketO TiO WITH (NOLOCK) ON TiO.QBPayrollItem = pr.QBwageID  
    WHERE tio.ID = @TicketID  
          
    SELECT  
        @CurrentEquipment = STUFF((SELECT  
            ',  ' + e.Unit  
        FROM Elev e  WITH (NOLOCK)
        INNER JOIN multiple_equipments ej WITH (NOLOCK) 
            ON e.ID = ej.elev_id  
        WHERE ej.ticket_id = @TicketID  
        FOR xml PATH (''))  
        , 1, 1, '')  
  
    DECLARE @Rol int  
    DECLARE @Nature smallint = 0  
    DECLARE @Ltype smallint = 0  
    DECLARE @ProspectID int  
    DECLARE @DucplicateProspectName int  
    DECLARE @prospectcreate int = 0  
    DECLARE @Phase int  
    DECLARE @ProjDate AS datetime = GETDATE();  
  
    --------------        
    IF (@Status <> 4)  
        IF EXISTS (SELECT  
                1  
            FROM TicketDPDA WITH (NOLOCK)  
            WHERE id = @TicketID  
            UNION  
            SELECT  
                1  
            FROM TicketD WITH (NOLOCK) 
            WHERE id = @TicketID)  
        BEGIN  
            RAISERROR ('RAISE_ERROR_1', 16, 1)  
            RETURN  
        END  
  
  
    ------------ // if the location on credit hold it needs to give a warning and prevent the user--------------        
    IF EXISTS (SELECT  
            1  
        FROM loc  WITH (NOLOCK)
        WHERE loc = @LocID  
        AND @custID <> 0  
        AND credit = 1)  
    BEGIN  
        RAISERROR ('Location on credit hold can not update ticket!', 16, 1)  
        RETURN  
    END  
  
    ------------ // if the Job Cost Labor = Burden Rate ---------------------------------------------------------        
 
    IF (@Status = 4)  
    BEGIN  
        IF ([dbo].Checkwagesisrequired(@Worker, 1) = 1)  
        BEGIN  
            RAISERROR ('Please add atleast a single wage item for selected worker!', 16, 1)  
            RETURN  
        END  
    END  
  
    IF (@Status = 4)  
    BEGIN  
        IF EXISTS ((SELECT  
                1  
            FROM CONTROL WITH (NOLOCK) 
            WHERE JobCostLabor = 1  
            AND ISNULL(QBIntegration, 0) = 0))  
        BEGIN  
            IF NOT EXISTS (SELECT  
                    1  
                FROM PRWageItem WITH (NOLOCK)  
                WHERE emp = (SELECT TOP 1  
                    ID  
                FROM EMP  WITH (NOLOCK)
                WHERE CallSign = @Worker)  
                AND PRWageItem.Wage = ISNULL(@wage, 0))  
            BEGIN  
                RAISERROR ('Wage item does not set for selected worker!', 16, 1)  
                RETURN  
            END  
        END  
    END  
  
    --BEGIN TRANSACTION        
     
    /********** Wnen adding prospect ************/  
    IF (@custID = 0)  
    BEGIN  
        SET @custID = NULL  
        SET @Nature = 1  
        SET @Ltype = 1  
  
        IF (@LocID = 0)  
        BEGIN  
  
            SET @prospectcreate = 1  
  
            SELECT  
                @DucplicateProspectName = COUNT(1)  
            FROM Rol r WITH (NOLOCK) 
            INNER JOIN Prospect p WITH (NOLOCK) 
                ON p.Rol = r.ID  
            WHERE Name = @CustName  
  
            IF (@DucplicateProspectName <> 0)  
            BEGIN  
  
 RAISERROR ('Prospect name already exists, please use different Prospect name !', 16, 1)  
                RETURN  
  
            END  
  
            SELECT  
                @ProspectID = ISNULL(MAX(ID), 0) + 1  
            FROM Prospect  WITH (NOLOCK)
  
            INSERT INTO Rol (Name,  
            Address,  
            City,  
            State,  
            Zip,  
            Phone,  
            Contact,  
            Remarks,  
            Type,  
            GeoLock,  
            fLong,  
            Latt,  
            Since,  
            Last,  
            EN,  
            Cellular,  
            Country,  
            Lat, Lng)  
                VALUES (@CustName, @LocAdd, @City, @State, @Zip, @Phone, @Contact, 'Created on Ticket# ' + CAST(@TicketID AS varchar(50)) + CHAR(13) + SPACE(2) + CONVERT(varchar(max), @Remarks), 3, 0, 0, 0, GETDATE(), GETDATE(), 1, @Cell, 'United States',@lat, @lng)  
  
            IF @@ERROR <> 0  
                AND @@TRANCOUNT > 0  
            BEGIN  
                RAISERROR ('Error Occured', 16, 1)  
  
                -- ROLLBACK TRANSACTION        
  
                RETURN  
            END  
  
            SET @Rol = SCOPE_IDENTITY()  
  
            INSERT INTO Prospect (ID,  
            Rol,  
            Type,  
            Level,  
            Status,  
            LDate,  
            LTime,  
            Program,  
            NDate,  
            PriceL,  
            CreateDate,  
            LastUpdateDate,  
            LastUpdatedBy,  
            CreatedBy,  
            CustomerName)  
                VALUES (@ProspectID, @Rol, '', 1, 0, GETDATE(), CAST(CAST('12/30/1899' AS date) AS datetime) + CAST(CAST(GETDATE() AS time) AS datetime), 0, GETDATE(), 0, GETDATE(), GETDATE(), @LastUpdatedBy, @LastUpdatedBy, @CustName)  
  
            IF @@ERROR <> 0  
                AND @@TRANCOUNT > 0  
            BEGIN  
                RAISERROR ('Error Occured', 16, 1)  
  
                --ROLLBACK TRANSACTION        
  
                RETURN  
            END  
  
            SET @LocID = @ProspectID  
  
            --update PType set [Count] = [Count]+1 where [Type] ='General'        
  
  
            IF NOT EXISTS (SELECT  
                    1  
                FROM Phone WITH (NOLOCK)  
                WHERE Rol = @Rol  
                AND fDesc = @contact)  
            BEGIN  
                INSERT INTO Phone (Rol, fDesc, Phone, Cell)  
                    VALUES (@Rol, @contact, @phone, @cell)  
            END  
  
            IF @@ERROR <> 0  
                AND @@TRANCOUNT > 0  
            BEGIN  
                RAISERROR ('Error Occured', 16, 1)  
  
                --ROLLBACK TRANSACTION        
  
                RETURN  
            END  
  
        END  
    END  

    IF (@WorkOrder = '')  
    BEGIN  
        SET @WorkOrder = @TicketID  
    END  
  
  
    DECLARE @equipcounts int = 0  
    SELECT  
        @equipcounts = COUNT(1)  
    FROM @Equipments  
    IF (@equipcounts = 1)  
    BEGIN  
        SELECT  
            @Unit = elev_id  
        FROM @Equipments  
    END  
    ELSE  
    IF (@equipcounts > 1)  
    BEGIN  
        SELECT TOP 1  
            @Unit = elev_id  
        FROM @Equipments  
    END  
    ELSE  
    IF (@equipcounts = 0)  
    BEGIN  
        SET @Unit = 0  
    END  
    	
    /**********  When staus is other than completed **********/  
    IF (@Status <> 4)  
    BEGIN  

        ------IF Department is -1 then  it  will auto populate from project        
        IF (@type = -1  
            AND @job > 0)  
        BEGIN  
            SELECT  
                @type = ISNULL(Type, -1)  
            FROM job  WITH (NOLOCK)
            WHERE id = @job  
        END  
  
        UPDATE TicketO  
        SET  
        --LDesc1=CONVERT(varchar(50), @LocID),        
        LDesc1 =  
                CASE  
                    WHEN (@custID IS NULL) THEN CONVERT(varchar(50), @LocID)  
                    ELSE (SELECT  
                            ID  
                        FROM Loc WITH (NOLOCK) 
                        WHERE Loc = @locid)  
                END,  
        ----LDesc2=@LocTag,        
    ----LDesc2=(select top 1 ( select top 1  name from Rol where ID=l.Rol) as name  from loc l where Loc=@locid),        
        LDesc2 =  
                CASE  
                    WHEN (@custID IS NULL) THEN (SELECT  
                            r.Name  
                        FROM Prospect p WITH (NOLOCK) 
                        INNER JOIN Rol r WITH (NOLOCK)  
                            ON r.ID = p.Rol  
                        WHERE p.ID = @LocID)  
                    ELSE (SELECT  
                            Tag  
                        FROM Loc  WITH (NOLOCK)
                        WHERE Loc = @locid)  
                END,  
        LDesc3 = @LocAdd,  
        LDesc4 = @City + ',' + SPACE(1) + @State + ',' + SPACE(1) + @Zip,  
        City = @City,  
        State = @State,  
        Zip = @Zip,  
        Phone = @Phone,  
        CPhone = @Cell,  
        DWork = @Worker,  
        CDate = @CallDt,  
        EDate = @SchDt,  
        Assigned = @Status,  
        TimeRoute = @EnrouteTime,  
        TimeSite = @Onsite,  
        TimeComp = @Complete,  
        Cat = @Category,  
        LElev = @Unit,  
        fDesc = @Reason,  
        [Owner] = @custID,  
        LID = @LocID,  
        Est = @EST,  
        fWork = (SELECT  
            id  
        FROM tblWork WITH (NOLOCK) 
        WHERE fDesc = @Worker),  
        Who = @Who,  
        BRemarks = @Recommendation,--@remarks,        
        Type = @Type,  
        Custom2 = @Custom2,  
        Custom3 = @Custom3,  
        Custom6 = @Custom6,  
        Custom7 = @Custom7,  
        Custom1 = @Custom1,  
        Custom4 = @Custom4,  
        Custom5 = @Custom5,  
        WorkOrder = @WorkOrder,  
        Nature = @Nature,  
        LType = @Ltype,  
        QBPayrollItem = @QBPayrollItem,  
        QBServiceItem = @QBServiceItem,  
        CustomTick1 = @Customtick1,  
        CustomTick2 = @Customtick2,  
        CustomTick3 = @Customtick3,  
        CustomTick4 = @Customtick4,  
        CustomTick5 = @Customtick5,  
        Job = @job,  
        fBy = @fBy,  
        Level = @Level,  
        Confirmed =  
                   CASE @Status  
                       WHEN 0 THEN 0  
                       ELSE Confirmed  
                   END,  
        Charge = @Charge  
  
        WHERE ID = @TicketID  
  
        IF @@ERROR <> 0  
            AND @@TRANCOUNT > 0  
        BEGIN  
            RAISERROR ('Error Occured', 16, 1)  
  
            --ROLLBACK TRANSACTION        
  
            RETURN  
        END  
    END  
  
    IF (@Status = 4)  
    BEGIN  
  
        IF (@job = 0)  
        BEGIN  
  
            IF (@IsCreateJob = 1)  
  
            BEGIN ----------> Create JOB        
                DECLARE @projremark varchar(75),  
                        @projname varchar(75),  
                        --@templateitems tblTypeProjectItem,        
                        @bomitems tblTypeBomItem,  
                        @MilestonItem tblTypeMilestoneItem,  
                        @servicetype varchar(15),  
                        @InvExp int,  
                        @InvServ int,  
                        @WageS int,  
                        @GLInt int,  
                        @Post tinyint,  
                        @Charges tinyint,  
                        @JobClose tinyint,  
                        @fInt tinyint,  
                        @types int  
  
                SELECT  
                    @projremark = Remarks,  
                    @projname = fDesc,  
                    @servicetype = CType,  
                    @InvExp = InvExp,  
                    @InvServ = InvServ,  
                    @Wages = Wage,  
                    @GLInt = GLInt,  
                    @Post = Post,  
                    @Charges = Charge,  
                    @JobClose = JobClose,  
                    @fInt = fInt,  
                    @types = [Type]  
                FROM JobT  WITH (NOLOCK)
                WHERE ID = @ProjectTemplate  
  
                SET @projremark += '-Added from ticket # ' + CONVERT(varchar(50), @TicketID)  
                SET @projname = ISNULL(@projname, '-') + '-Ticket # ' + CONVERT(varchar(50), @TicketID)  
  
                --insert into @templateitems         
                --select JobT, job, type, fdesc, code, Actual, Budget, line, [Percent] from JobTItem         
                --where JobT = @ProjectTemplate and (Job is null or job = 0)        
  
                INSERT INTO @bomitems (JobT, Job, JobTItemID, fDesc, Code, Line, BType, QtyReq, UM, BudgetUnit, BudgetExt, LabItem, MatItem, MatMod, LabMod, LabExt, LabRate, LabHours, SDate, VendorId, OrderNo, GroupID)  
  
                    SELECT  
                        ji.JobT,  
                        ji.Job,  
                        b.JobTItemID,  
                        --ji.[Type] ,         
                        fdesc,  
                        ji.Code,  
                        --ji.Budget,         
                        Line,  
                        b.[Type],  
                        --b.Item,         
                        b.QtyRequired,  
                        UM,  
                        --b.ScrapFactor ,         
                        b.BudgetUnit,  
                        b.BudgetExt,  
                        b.LabItem,  
                        b.MatItem,  
                        ji.Modifier, --b.MatMod        
                        ji.ETCMod, -- b.LabMod        
                        ji.ETC,  -- b.LabExt        
                        b.LabRate,  
                        ji.BHours, --b.LabHours        
                        b.SDate,  
                        b.Vendor,  
                        Line,  
                        GroupID  
  
                    FROM BOM b WITH (NOLOCK)  
                    INNER JOIN jobtitem ji   WITH (NOLOCK)
                        ON ji.ID = b.JobTItemID  
                    WHERE ji.JobT = @ProjectTemplate  
                    AND (ji.job = 0  
                    OR ji.job IS NULL)  
  
                INSERT INTO @MilestonItem  
                    SELECT  
                        [JobT],  
                        [Job],  
                        m.[JobTItemID],  
                        ji.[Type],  
                        [fdesc],  
                        ji.[Code],  
                        [Line],  
                        m.[MilestoneName],  
                        [RequiredBy],  
                        0,  
                        [ProjAcquistDate],  
                        [ActAcquistDate],  
                        [Comments],  
                        m.[Type],  
                        [Amount],  
                        [Line],  
                        GroupID,
                        m.Quantity,
                        m.Price,
                        m.ChangeOrder
                    FROM Milestone m  WITH (NOLOCK)
                    INNER JOIN jobtitem ji WITH (NOLOCK) 
                        ON ji.ID = m.JobTItemID  
                    WHERE ji.JobT = @ProjectTemplate  
                    AND (ji.job = 0  
                    OR ji.job IS NULL)  
  
            DECLARE @UpdatedByUserId INT =0;   
             SELECT TOP 1 @UpdatedByUserId = ID FROM tblUser WITH (NOLOCK) WHERE fUser = @LastUpdatedBy  
  
                EXEC @job = spAddProject @job = 0,  
                                         @owner = NULL,  
                                         @loc = @LocID,  
                                         @fdesc = @projname,  
                                         @status = 0,  
                                         @type = @types,  
                                         @Remarks = @projremark,  
                                         @ctype = @servicetype,  
                                         @ProjCreationDate = @ProjDate,  
                                         @PO = NULL,  
                                         @SO = NULL,  
                                         @Certified = 0,  
                                         @Custom1 = NULL,  
                                         @Custom2 = NULL,  
                                         @Custom3 = NULL,  
                                         @Custom4 = NULL,  
                                         @Custom5 = NULL,  
                                         @template = @ProjectTemplate,  
                                         @RolName = NULL,  
                                         @city = NULL,  
                                         @state = NULL,  
                                         @zip = NULL,  
                                         @country = NULL,  
                                         @phone = NULL,  
                                         @cellular = NULL,  
                                         @fax = NULL,  
                                         @contact = NULL,  
                                         @email = NULL,  
                                         @rolRemarks = NULL,  
                                         @rolType = NULL,  
                                         @InvExp = @InvExp,  
                                         @InvServ = @InvServ,  
                                         @Wage = @Wages,  
                                         @GLInt = @GLInt,  
                                         @jobtCType = NULL,  
                                         @Post = @Post,  
                                         @Charge = @Charges,  
                                         @JobClose = @JobClose,  
                                         @fInt = @fInt,           
                                         @BomItem = @bomitems,  
                                         @MilestonItem = @MilestonItem,  
                                         @UpdatedByUserId= @UpdatedByUserId,  
                                         @TargetHPermission=0  
                                        --@CustomItem =null        
  
            END-------> END Create JOB        
  
        END  
  
        --if exists(select ID from TicketDPDA where ID =@TicketID)        
        --begin        
  
        DECLARE @RegTrav numeric(30, 2),  
                @OTTrav numeric(30, 2),  
                @NTTrav numeric(30, 2),  
                @DTTrav numeric(30, 2),  
                @CReg numeric(30, 2),  
                @COT numeric(30, 2),  
                @CDT numeric(30, 2),  
                @CNT numeric(30, 2),  
                @CTT numeric(30, 2)  
  
        -- if Multi Travel Feature is ON          
        IF ((SELECT TOP 1  
                Label  
            FROM Custom  WITH (NOLOCK)
            WHERE Name = 'MultiTravel')  
            = '1')  
        BEGIN  
            SELECT  
                @RegTrav = RegTrav,  
                @OTTrav = OTTrav,  
                @NTTrav = NTTrav,  
                @DTTrav = DTTrav  
            FROM (SELECT  
                RegTrav,  
                OTTrav,  
                NTTrav,  
                DTTrav  
            FROM TicketDPDA WITH (NOLOCK)  
            WHERE ID = @TicketID  
            UNION  
            SELECT  
                RegTrav,  
                OTTrav,  
                NTTrav,  
                DTTrav  
            FROM TicketD WITH (NOLOCK) 
            WHERE ID = @TicketID) AS tabtrav  
        END  
        ELSE  
        BEGIN  
            SET @RegTrav = 0;  
            SET @OTTrav = 0;  
            SET @NTTrav = 0;  
            SET @DTTrav = 0;  
        END  
  
  
  
        SELECT  
            @CReg = [CReg],  
            @COT = [COT],  
            @CDT = [CDT],  
            @CNT = [CNT],  
            @CTT = [CTT]  
        FROM PRWageItem pr  WITH (NOLOCK) 
        WHERE pr.Wage = @Wage  
        AND pr.Emp = (SELECT  
            id  
        FROM emp e  WITH (NOLOCK)
        WHERE e.fWork = (SELECT  
            ID  
        FROM tblWork  WITH (NOLOCK)
        WHERE fDesc = @Worker))  
  
        IF (@JobCode IS NULL  
            OR @JobCode = '')  
        BEGIN  
            IF (@job > 0)  
            BEGIN  
                SELECT TOP 1  
                    @JobCode = CONVERT(varchar, j.Line) + ':' + j.Code + ':' + j.fDesc  
                FROM jobtitem j  WITH (NOLOCK) 
                INNER JOIN bom b WITH (NOLOCK)     ON b.JobtItemId = j.ID  
                INNER JOIN BOMT  WITH (NOLOCK)     ON BOMT.ID = b.Type  
                WHERE j.job = @job          AND BOMT.Type = 'labor'  
                IF (@JobCode IS NULL) ------If Labour item not Exists        
                BEGIN  
                    SELECT TOP 1  
                        @JobCode = CONVERT(varchar, j.Line) + ':' + j.Code + ':' + j.fDesc  
                    FROM jobtitem j WITH (NOLOCK)  
                    INNER JOIN bom b  WITH (NOLOCK)      ON b.JobtItemId = j.ID  
                    INNER JOIN BOMT WITH (NOLOCK)        ON BOMT.ID = b.Type  
                    WHERE j.job = @job  
                END  
            END  
        END  
         
  
        IF NOT EXISTS (SELECT  1   FROM TicketD WITH (NOLOCK)  WHERE ID = @TicketID)  
        BEGIN  
            ------IF Department is -1 then  it  will auto populate from project        
            IF (@type = -1  AND @job > 0)  
            BEGIN  
                SELECT  @type = ISNULL(Type, -1)   FROM job WITH (NOLOCK)   WHERE id = @job  
            END  
           ---if(isnull(@Internet,0)=0)  
           --SELECT @Internet = Isnull(TInternet, 0)  FROM   Control   
  
            INSERT INTO TicketD (ID,  
            CDate,  
            EDate,  
            TimeRoute,  
            TimeSite,  
            TimeComp,  
            Cat,  
            fDesc,  
            Est,  
            fWork,  
            Loc,  
            DescRes,  
            Reg,  
            OT,  
            NT,  
            TT,  
            DT,  
            Total,  
            Charge,  
            ClearCheck,  
			ClearPR,
            Who,  
            Type,  
            Status,  
            Elev,  
            BRemarks,  
            Custom2,  
            Custom3,  
            Custom6,  
            Custom7,  
            Custom1,  
            Custom4,  
            Custom5,  
            WorkOrder,  
            WorkComplete,  
            OtherE,  
            Toll,  
            Zone,  
            SMile,  
            EMile,  
            Internet,  
            Job,  
            --Invoice        
            ManualInvoice,  
            lastupdatedate,  
            TransferTime,  
            QBServiceItem,  
            QBPayrollItem,  
            CPhone,  
            CustomTick1,  
            CustomTick2,  
            CustomTick3,  
            CustomTick4,  
            CustomTick5,  
            JobCode,  
            Phase,  
            WageC,  
            fBy,  
            Recurring,  
            JobItemDesc,  
            RegTrav,  
            OTTrav,  
            NTTrav,  
            DTTrav,  
            break_time,  
            Comments,  
            PartsUsed,  
            Level,  
            DDate,  
            TimeCheckOut,  
            TimeCheckOutFlag   
            )  
            VALUES (@TicketID, @CallDt, @SchDt, @EnrouteTime, @Onsite, @Complete, @Category, @Reason, @EST, (SELECT ID FROM tblWork WITH (NOLOCK) WHERE fDesc = @Worker), @LocID, @complDesc, @Reg, @OT, @NT, @TT, @DT, @Total,  
                --CASE        
                --  WHEN ( Isnull(@Invoice, 0) = 0 ) THEN @Charge        
                --  ELSE 0        
                --END,        
                CASE WHEN (@Invoice = '') THEN @Charge ELSE 0 END, @Review, @ClearPR , @Who, @Type, 0, @Unit, @Recommendation,--@remarks,        
                @Custom2, @Custom3, @Custom6, @Custom7, @Custom1, @Custom4, @Custom5, @WorkOrder, @WorkComplete, @MiscExp, @TollExp, @ZoneExp, @MileStart, @MileEnd, @Internet,  
                   
                @job, @Invoice, GETDATE(), @TransferTime, @QBServiceItem, @QBPayrollItem, @Cell, @Customtick1, @Customtick2, @Customtick3, @Customtick4, @Customtick5,   
                (SELECT items FROM dbo.IDSplit(@JobCode, ':') WHERE row = 2),   
                (SELECT items FROM dbo.IDSplit(@JobCode, ':') WHERE row = 1), @wage, @fBy,  
                (SELECT TOP 1 Recurring FROM TicketO WITH (NOLOCK) WHERE ID = @TicketID),   
                (SELECT items FROM dbo.IDSplit(@JobCode, ':') WHERE row = 3), @RegTrav, @OTTrav, @NTTrav, @DTTrav, @BT, @Comments, @PartsUsed, @Level, (SELECT TOP 1 DDate FROM TicketO WITH (NOLOCK) WHERE ID = @TicketID),  
                (SELECT TOP 1 TimeCheckOut FROM TicketDPDA WITH (NOLOCK) WHERE ID = @TicketID),  
                (SELECT TOP 1 TimeCheckOutFlag FROM TicketDPDA WITH (NOLOCK) WHERE ID = @TicketID)  
      
            )  
        --exec spUpdateJobLaborExp @job     
	
        END  
        ELSE  
        BEGIN  
  
            DECLARE @previousjob int  
            DECLARE @previousPhase int  
            SELECT  
                @previousjob = job,  
                @previousPhase = Phase  
            FROM TicketD  WITH (NOLOCK)
            WHERE id = @TicketID  
  
            ------IF Department is -1 then  it  will auto populate from project        
            IF (@type = -1  
                AND @job > 0)  
            BEGIN  
                SELECT  
                    @type = ISNULL(Type, -1)  
                FROM job WITH (NOLOCK) 
                WHERE id = @job  
            END  
  
            UPDATE TicketD  
            SET CDate = @CallDt,  
                EDate = @SchDt,  
                TimeRoute = @EnrouteTime,  
                TimeSite = @Onsite,  
                TimeComp = @Complete,  
                Cat = @Category,  
                fDesc = @Reason,  
                Est = @EST,  
                fWork = (SELECT  
                    ID  
                FROM tblWork  WITH (NOLOCK)
                WHERE fDesc = @Worker),  
                Loc = @LocID,  
                DescRes = @complDesc,  
                Reg = @Reg,  
                OT = @OT,  
                NT = @NT,  
                TT = @TT,  
                DT = @DT,  
                break_time = @BT,  
                Comments = @Comments,  
                PartsUsed = @PartsUsed,  
                Total = @Total,  
                Charge = (CASE  
                    WHEN (@Invoice = '') THEN CASE  
                            WHEN (ISNULL(Invoice, 0) = 0) THEN @Charge  
                            ELSE 0  
                        END  
                    ELSE 0  
                END),  
                ClearCheck = @Review,  
				ClearPR=@ClearPR,
                Who = @Who,  
                Elev = @Unit,  
                BRemarks = @Recommendation,-- @remarks,        
                Type = @Type,  
                Custom2 = @Custom2,  
                Custom3 = @Custom3,  
                Custom6 = @Custom6,  
                Custom7 = @Custom7,  
                Custom1 = @Custom1,  
                Custom4 = @Custom4,  
                Custom5 = @Custom5,  
                WorkOrder = @WorkOrder,  
                WorkComplete = @WorkComplete,  
                OtherE = @MiscExp,  
                Toll = @TollExp,  
                Zone = @ZoneExp,  
                SMile = @MileStart,  
                EMile = @MileEnd,  
                Internet = @Internet,  
                --Invoice = @Invoice        
                ManualInvoice = @Invoice,  
                LastUpdateDate = GETDATE(),  
                TransferTime = @TransferTime,  
                QBServiceItem = @QBServiceItem,  
                QBPayrollItem = @QBPayrollItem,  
                CPhone = @Cell,  
                CustomTick1 = @Customtick1,  
                CustomTick2 = @Customtick2,  
                CustomTick3 = @Customtick3,  
                CustomTick4 = @Customtick4,  
                CustomTick5 = @Customtick5,  
                Job = @job,  
                JobCode = (SELECT  
                    items  
                FROM dbo.IDSplit(@JobCode, ':')  
                WHERE row = 2),  
                phase = (SELECT  
                    items  
                FROM dbo.IDSplit(@JobCode, ':')  
                WHERE row = 1),  
                WageC = @wage,  
                fBy = @fBy,  
                Level = @Level,  
                JobItemDesc = (SELECT  
                    items  
                FROM dbo.IDSplit(@JobCode, ':')  
                WHERE row = 3)  
            WHERE ID = @TicketID  
  
            IF (ISNULL(@previousjob, 0) <> ISNULL(@job, 0))  
            BEGIN  
                DELETE FROM JobI   
                WHERE Ref = @TicketID  
                    AND TransID = -@TicketID  
             EXEC spUpdateJobLaborExp @previousjob,  
                                         @previousPhase  
            END  
        END  
  
  
        DECLARE @hourlyrate numeric(30, 2)  
  
        SELECT  
            @hourlyrate = ISNULL(hourlyrate, 0)  
        FROM tblWork WITH (NOLOCK) 
        WHERE fDesc = @Worker  
  
        DELETE FROM JobI  
        WHERE Ref = @TicketID  
            AND TransID = -@TicketID --and Job =@job         
  
        ------$$$$$ JOb Cast feature $$$$--------        
  
        IF (@job > 0)  
  
        BEGIN  ------>        
 
            INSERT INTO JobI ([Job], [Phase], [fDate], [Ref], [fDesc], [Amount], [TransID], [Type], [UseTax], Labor)  
                VALUES (@job, (SELECT items FROM dbo.IDSplit(@JobCode, ':') WHERE row = 1), @SchDt, @TicketID, 'Labor/Time Spent on Ticket - ' + @Worker, CASE (SELECT ISNULL(JobCostLabor, 0) FROM Control WITH (NOLOCK)) WHEN 0 THEN ((ISNULL(@Reg, 0) + ISNULL(@RegTrav, 0)) * ISNULL(@hourlyrate, 0) + (ISNULL(@OT, 0) + ISNULL(@OTTrav, 0)) * (1.5 * ISNULL(@hourlyrate, 0)) + (ISNULL(@NT, 0) + ISNULL(@NTTrav, 0)) * (1.7 * ISNULL(@hourlyrate, 0)) + (ISNULL(@DT, 0) + ISNULL(@DTTrav, 0)) * (2 * ISNULL(@hourlyrate, 0)) + ISNULL(@TT, 0) * ISNULL(@hourlyrate, 0)) WHEN 1 THEN ((ISNULL(@Reg, 0) + ISNULL(@RegTrav, 0)) * ISNULL(@CReg, 0) + (ISNULL(@OT, 0) + ISNULL(@OTTrav, 0)) * (ISNULL(@COT, 0)) + (ISNULL(@NT, 0) + ISNULL(@NTTrav, 0)) * (ISNULL(@CNT, 0)) + (ISNULL(@DT, 0) + ISNULL(@DTTrav, 0)) * (ISNULL(@CDT, 0)) + ISNULL(@TT, 0) * ISNULL(@CTT, 0)) END, -@TicketID, 1, 0, 1)  
  
            INSERT INTO JobI ([Job], [Phase], [fDate], [Ref], [fDesc], [Amount], [TransID], [Type], [UseTax], Labor)  
               VALUES (@job, (SELECT items FROM dbo.IDSplit(@JobCode, ':') WHERE row = 1), @SchDt, @TicketID, 'Mileage on Ticket', (ISNULL(@MileEnd, 0) - ISNULL(@MileStart, 0)) * ISNULL((SELECT MileageRate FROM Emp WITH (NOLOCK) WHERE CallSign = @Worker), 0), -@TicketID, 1, 0, 0)  
  
            INSERT INTO JobI ([Job], [Phase], [fDate], [Ref], [fDesc], [Amount], [TransID], [Type], [UseTax], Labor)  
                VALUES (@job, (SELECT items FROM dbo.IDSplit(@JobCode, ':') WHERE row = 1), @SchDt, @TicketID, 'Expenses on Ticket', ISNULL(@TollExp, 0) + ISNULL(@ZoneExp, 0) + ISNULL(@MiscExp, 0), -@TicketID, 1, 0, 0)  
  
            SET @Phase = (SELECT  
                items  
            FROM dbo.IDSplit(@JobCode, ':')  
            WHERE row = 1)  
  
            EXEC spUpdateJobLaborExp @job,  
                                     @Phase  
  
        END ----->              
		
        IF (RTRIM(LTRIM(@Recommendation)) <> '')  
        BEGIN  
            IF NOT EXISTS (SELECT  1  FROM Lead  WITH (NOLOCK) WHERE TicketID = @TicketID)  
            BEGIN  
                DECLARE @oppname varchar(75)  
                SET @oppname = 'Ticket# ' + CONVERT(varchar(67), @TicketID)  
  
                IF (@custID IS NULL)  
                BEGIN  
                    SELECT  
                        @Rol = Rol  
                    FROM Prospect WITH (NOLOCK) 
                    WHERE ID = @LocID  
                END  
                ELSE  
                BEGIN  
                    SELECT  
                        @Rol = Rol  
                    FROM Loc  WITH (NOLOCK)
                    WHERE Loc = @LocID  
                END  
                DECLARE @defaultuser varchar(50)  
                SELECT TOP 1  
                    @defaultuser = fuser  
                FROM tbluser  WITH (NOLOCK)
                WHERE DefaultWorker = 1  
  
                DECLARE @oppremarks varchar(500)  
  
                IF ((SELECT  COUNT(1)  FROM @Equipments)  = 1)  
                BEGIN  
                    SET @oppremarks = @Recommendation + CHAR(13) + CHAR(10) 
                                    + 'Equipment : ' + (SELECT TOP 1  ISNULL((SELECT TOP 1  ISNULL(unit, '') + ', ' + ISNULL(fDesc, '') FROM elev WITH (NOLOCK) WHERE id = elev_id) , '')  FROM @Equipments)  
                END  
                ELSE  
                BEGIN  
                    SET @oppremarks = @Recommendation  
                END  
                EXEC Spaddopportunity @ID = 0,  
                                      @fdesc = @oppname,  
                                      @rol = @Rol,  
                                      @Probability = 3,  
                                      @Status = 1,  
                                      @Remarks = @oppremarks,  
                                      @closedate = @SchDt,  
                                      @Mode = 0,  
                                      @owner = 0,  
                                      @NextStep = '',  
                                      @desc = '',  
                                      @Source = '',  
                                      @Amount = 0,  
                                      @Fuser = @defaultuser,  
                                      @AssignedToID = 0,  
                                      @UpdateUser = @LastUpdatedBy,  
                                      @closed = 0,  
                                      @TicketID = @TicketID,  
                                      @BusinessType = @BT,  
                                      @Product = '',  
                                      @OpportunityStageID = 0,  
                                      @CompanyName = @CustName,  
                                      @IsSendMailToSalesPer = 1,  
                                      @Department =null  
                IF @@ERROR <> 0  
                    AND @@TRANCOUNT > 0  
                BEGIN  
                    RAISERROR ('Error Occured', 16, 1)  
  
                    --ROLLBACK TRANSACTION        
  
                    RETURN  
                END  
  
            END  
        END  
  
        IF (@Review = 1)  
        BEGIN  
            UPDATE Elev  
            SET Last = @SchDt  
            WHERE ID = @Unit  
            AND ISNULL(Last, CONVERT(datetime, '01/01/1800')) < @SchDt  
        END  
  
        IF @@ERROR <> 0  
            AND @@TRANCOUNT > 0  
        BEGIN  
            RAISERROR ('Error Occured', 16, 1)  
  
            --ROLLBACK TRANSACTION        
  
            RETURN  
        END  
        /******update loadtestitem if safety test exists for this ticket *******/  
  	print 'Update loadTest'
        IF EXISTS (SELECT TOP 1  
                1  
            FROM LoadTestItemSchedule  WITH (NOLOCK)
            WHERE TicketID = @TicketID)  
        BEGIN  
	print 'Update loadTest 2'
            
			 ----ES-4269-Safety test project is not showing all projects
		   -- update   loadtestitem set JobId=@job  WHERE ticket = @TicketID  and isnull(JobId,0) = 0 
			 --update   LoadTestItemHistory set TicketStatus=1, JobId=@job, Worker=@Worker,  fWork = (SELECT  
    --                ID 
    --            FROM tblWork  WITH (NOLOCK)
    --            WHERE fDesc = @Worker),Schedule=@SchDt, who=  @Who    WHERE TicketID = @TicketID  and isnull(JobId,0) = 0 

			DECLARE @lid int  
            DECLARE @jDate datetime  
            DECLARE @SafetyTestTicketStatus varchar(20);  
            DECLARE @SafetyTestUsername varchar(20);  
            SELECT  
                @SafetyTestUsername = @Worker;  
            SELECT  
                @SafetyTestTicketStatus = (CASE @status  
                    WHEN 4 THEN 'Completed'  
                    ELSE 'Assigned'  
                END)  
            SET @jDate = GETDATE()  

			DECLARE @cur_Year INT

			SET @cur_Year=(SELECT TOP 1 ScheduledYear FROM LoadTestItemSchedule WHERE TicketId= @TicketID)
			IF OBJECT_ID('tempdb..#tempLID') IS NOT NULL DROP TABLE #tempLID
			CREATE Table #tempLID(
			LID         INT 	
			)
			Insert into #tempLID
			select LID from LoadTestItemSchedule WHERE TicketID=@TicketID
			Update LoadTestItemHistory set Last=@jDate, JobId=@job where TestYear=@cur_Year and LID in (select LID from #tempLID)

			UPDATE LoadTestItemSchedule SET TicketStatus=4 WHERE TicketID=@TicketID

			
			Declare @Teststatus INT
			
			DECLARE cur_Loc CURSOR FOR 	
			select LID from LoadTestItemSchedule WHERE TicketID=@TicketID
			OPEN cur_Loc  
			FETCH NEXT FROM cur_Loc INTO @lid
			WHILE @@FETCH_STATUS = 0  
			BEGIN
					EXEC spUpdateTestStatus @lid,@cur_Year
			END	
			CLOSE cur_Loc  
			DEALLOCATE cur_Loc  

            --SELECT  
            --    @lid = lid  
            --FROM loadtestitem  WITH (NOLOCK)
            --WHERE ticket = @TicketID  

			

	--DECLARE cur_Loc CURSOR FOR 	
	--	SELECT  
 --              lid  
 --           FROM loadtestitem  WITH (NOLOCK)
 --           WHERE ticket = @TicketID  
	--OPEN cur_Loc  
	--FETCH NEXT FROM cur_Loc INTO @lid
	--WHILE @@FETCH_STATUS = 0  
	--	BEGIN
	--		EXEC UpdateLoadTestItemDates @lid,  
 --                                        @jDate,  
 --                                        @Who  
 --           EXEC UpdateLoadTestItemStatusAndDates @lid,  
 --                                                 0,  
 --                                                 @jDate  
 --           EXEC spCreateTestHistory @lid,  
 --                                    @SafetyTestUsername,  
 --                                    'Open',  
 --                                    @jDate,  
 --                                    NULL,  
 --                                    @TicketID,  
 --                                    @SafetyTestTicketStatus  
	--	FETCH NEXT FROM cur_Loc INTO @lid
	--	END	
	--CLOSE cur_Loc  
	--DEALLOCATE cur_Loc  

            




        END  
        /************ when ticket is completed the ticket data is transferred to TicketD table and the same ticket is deleted from TicketO and TicketDPDA tables *******************/  
        DELETE FROM TicketDPDA         WHERE ID = @TicketID  
        IF @@ERROR <> 0  
AND @@TRANCOUNT > 0  
        BEGIN  
            RAISERROR ('Error Occured', 16, 1)  
  
            --ROLLBACK TRANSACTION        
  
            RETURN  
        END  
  
        DELETE FROM TicketO   
        WHERE ID = @TicketID  
  
        IF @@ERROR <> 0  
            AND @@TRANCOUNT > 0  
        BEGIN  
            RAISERROR ('Error Occured', 16, 1)  
  
            --ROLLBACK TRANSACTION        
  
            RETURN  
        END  
  
        DECLARE @WorkerID int  
  
        SET @WorkerID = (SELECT TOP 1  
            ID  
        FROM tblWork  WITH (NOLOCK)
        WHERE fDesc = @Worker)  
  
        EXEC Spinsertsign @WorkerID,  
                          @TicketID  
  
  
        ---  ////////// INVENTORTY  $$$$$$$$$$$$$        
  
  
        EXEC [spAddTicketINVInfo] @TicketID = @TicketID,  
                                  @job = @job,  
                                  @dtTicketINV = @dtTicketINV,  
                                  @screen = 'Ticket Inventory Used',  
                                  @mode = 'Edit'  
  
  IF ((SELECT COUNT(1) FROM @dtTicketINV)>0 AND (SELECT COUNT(*) FROM TicketI WITH (NOLOCK) WHERE Ticket=@TicketID AND charge=1)>0)  
  BEGIN  
   UPDATE TicketD  
   SET Charge=1  
   WHERE ID=@TicketID  
  END  
  
  
        ----/////// END INVENTORTY $$$$$$$$$$$$$$        
  
        -----------///////If  Invoice Job Field IS Null For MS Invoice/////----------->        
  
        IF (ISNULL(@Invoice, 0) <> 0)  
        BEGIN  
            IF EXISTS (SELECT  
                    1  
                FROM Invoice  WITH (NOLOCK)
                WHERE ref = @Invoice  
                AND ISNULL(job, 0) = 0)  
            BEGIN  
                UPDATE Invoice  
                SET Job = @job  
                WHERE ref = @Invoice  
                AND ISNULL(job, 0) = 0  
                UPDATE InvoiceI  
                SET Job = @job  
                WHERE ref = @Invoice  
                AND ISNULL(job, 0) = 0  
            END  
        END  
        -------------------------------------------------------------->        
        IF @@ERROR <> 0  
            AND @@TRANCOUNT > 0  
        BEGIN  
            RAISERROR ('Error Occured', 16, 1)  
  
            --ROLLBACK TRANSACTION        
  
            RETURN  
        END  
  
  
    END  
  
    DELETE FROM multiple_equipments     WHERE ticket_id = @TicketID  
  
    INSERT INTO multiple_equipments (ticket_id, elev_id, labor_percentage)  
        SELECT  
            @TicketID,  
            elev_id,  
            labor_percentage  
        FROM @Equipments  
  
    IF (@UpdateTasks = 1)  
    BEGIN  
        IF (@job IS NOT NULL)  
        BEGIN  
            IF (@job <> 0)  
            BEGIN  
                DELETE FROM Ticket_Task_Codes                    WHERE job = @job  
                INSERT INTO Ticket_Task_Codes (task_code, Type, job, Category, username, dateupdated, ticket_id, default_code)  
                    SELECT  
                        task_code,  
                        1,  
                        @job,  
                        Category,  
                        username,  
                        dateupdated,  
                        CASE ticket_id  
                            WHEN 0 THEN @TicketID  
                            ELSE ticket_id  
                        END,  
                        1  
                    FROM @TaskCodes  
            END  
        END  
    END  
  
  
  
    /******* update address in location *********/  
    IF (@custID IS NOT NULL)  
    BEGIN  
        UPDATE Loc  
        SET Address = @LocAdd,  
            City = @City,  
            State = @State,  
            Zip = @Zip,  
            Remarks = @remarks,  
            DispAlert = @DispAlert,  
            Credit = @CreditHold,  
            CreditReason = @CreditReason,  
            Route = @DefaultRoute,  
            Zone = @Zone  
        WHERE Loc = @locID  
        UPDATE Job  
        SET Custom20 = @DefaultRoute  
        WHERE Loc = @locID  
        UPDATE Rol  
        SET Phone = @Phone,  
            Cellular = @Cell,  
            LastUpdateDate = GETDATE(),  
            Contact = @Contact,  
            Lat = @lat,  
            Lng = @lng  
        WHERE ID = (SELECT TOP 1 Rol  
        FROM Loc  WITH (NOLOCK)
        WHERE Loc = @LocID)  
    END  
  
    IF @@ERROR <> 0  
        AND @@TRANCOUNT > 0  
    BEGIN  
        RAISERROR ('Error Occured', 16, 1)  
  
        -- ROLLBACK TRANSACTION        
  
        RETURN  
    END  
  
    IF (@custID IS NULL  
        AND @prospectcreate = 0)  
    BEGIN  
        UPDATE Prospect  
        SET LastUpdateDate = GETDATE(),  
            LastUpdatedBy = @LastUpdatedBy  
        WHERE ID = @LocID  
  
        UPDATE Rol  
        SET Address = @LocAdd,  
            City = @City,  
            State = @State,  
            Zip = @Zip,  
            Remarks = @Remarks,  
            Phone = @Phone,  
            Cellular = @Cell,  
            LastUpdateDate = GETDATE(),  
            Contact = @Contact,  
            Lat = @lat,  
            Lng = @lng  
        WHERE ID = (SELECT TOP 1  
            Rol  
        FROM Prospect WITH (NOLOCK) 
        WHERE ID = @LocID)  
    END  
  
    IF @@ERROR <> 0  
        AND @@TRANCOUNT > 0  
    BEGIN  
        RAISERROR ('Error Occured', 16, 1)  
  
        -- ROLLBACK TRANSACTION        
  
        RETURN  
    END  
  
  
  
    /****** Insert signature ***********/  
    IF (@sign IS NOT NULL)  
    BEGIN  
        EXEC Spinsertticketsign @TicketID,  
                                @sign  
    END  
  
    IF @@ERROR <> 0  
        AND @@TRANCOUNT > 0  
    BEGIN  
        RAISERROR ('Error Occured', 16, 1)  
  
        -- ROLLBACK TRANSACTION        
  
        RETURN  
    END  
  
    /********Start Logs************/  
    DECLARE @Val varchar(1000)  
    IF (@CustName IS NOT NULL)  
    BEGIN  
        SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Customer Name' ORDER BY CreatedStamp DESC),'')  
        IF (@Val <> @CustName)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Customer Name',  
                             @Val,  
                             @CustName  
        END  
        ELSE  
        IF (@CurrentOwner <> @CustName)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Customer Name',  
                             @CurrentOwner,  
                             @CustName  
        END  
    END  
    SET @Val = NULL  
    IF (@LocID IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Location Name' ORDER BY CreatedStamp DESC),'')  
        DECLARE @LocName varchar(150)  
        SELECT  
            @LocName = tag  
        FROM loc WITH (NOLOCK)  
        WHERE loc = (SELECT  
            LID  
        FROM TicketO  WITH (NOLOCK)
        WHERE ID = @TicketID)  
        IF (@Val <> @LocName)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Location Name',  
                             @Val,  
                             @LocName  
        END  
        ELSE  
        IF (@CurrentLocName <> @LocName)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Location Name',  
                             @CurrentLocName,  
                             @LocName  
        END  
    END  
    SET @Val = NULL  
    IF (@WorkOrder IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Work Order' ORDER BY CreatedStamp DESC),'')  
        IF (@Val <> @WorkOrder)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Work Order',  
                             @Val,  
                             @WorkOrder  
        END  
        ELSE  
        IF (@CurrentWorkOrder <> @WorkOrder)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Work Order',  
                             @CurrentWorkOrder,  
                             @WorkOrder  
        END  
    END  
    SET @Val = NULL  
    IF (@Invoice IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Invoice' ORDER BY CreatedStamp DESC),'')  
        IF (@Val <> @Invoice)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Invoice',  
                             @Val,  
                             @Invoice  
        END  
        ELSE  
        IF (@CurrentInvoice <> @Invoice)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Invoice',  
                             @CurrentInvoice,  
                             @Invoice  
        END  
    END  
    SET @Val = NULL  
    IF (@LocAdd IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Location Address' ORDER BY CreatedStamp DESC),'')  
        IF (@Val <> @LocAdd)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Location Address',  
                             @Val,  
                             @LocAdd  
            EXEC log2_insert @LastUpdatedBy,  
                             'Location',  
                             @locID,  
                             'Address',  
                             @Val,  
                             @LocAdd  
        END  
        ELSE  
        IF (@CurrentLocAdd <> @LocAdd)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Location Address',  
                             @CurrentLocAdd,  
                             @LocAdd  
            EXEC log2_insert @LastUpdatedBy,  
                             'Location',  
                             @locID,  
                             'Address',  
                             @CurrentLocAdd,  
                             @LocAdd  
        END  
    END  
    SET @Val = NULL  
    IF (@City IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'City' ORDER BY CreatedStamp DESC),'')  
        IF (@Val <> @City)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'City',  
                             @Val,  
                             @City  
            EXEC log2_insert @LastUpdatedBy,  
                             'Location',  
                             @locID,  
                             'City',  
                             @Val,  
                             @City  
        END  
        ELSE  
        IF (@CurrentCity <> @City)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'City',  
                             @CurrentCity,  
                             @City  
            EXEC log2_insert @LastUpdatedBy,  
                             'Location',  
                             @locID,  
                             'City',  
                             @CurrentCity,  
                             @City  
        END  
    END      SET @Val = NULL  
    IF (@State IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'State' ORDER BY CreatedStamp DESC),'')  
        IF (@Val <> @State)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'State',  
                             @Val,  
                             @State  
            EXEC log2_insert @LastUpdatedBy,  
                             'Location',  
                             @locID,  
                             'State',  
                             @Val,  
                             @State  
        END  
        ELSE  
        IF (@CurrentState <> @State)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'State',  
                             @CurrentState,  
                             @State  
            EXEC log2_insert @LastUpdatedBy,  
                             'Location',  
                             @locID,  
                             'State',  
                             @CurrentState,  
                             @State  
        END  
    END  
    SET @Val = NULL  
    IF (@Zip IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Zip/Postal Code' ORDER BY CreatedStamp DESC),'')  
        IF (@Val <> @Zip)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Zip/Postal Code',  
                             @Val,  
                             @Zip  
            EXEC log2_insert @LastUpdatedBy,  
                             'Location',  
                             @locID,  
                             'Zip',  
                             @Val,  
                             @Zip  
        END  
        ELSE  
        IF (@CurrentZip <> @Zip)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Zip/Postal Code',  
                             @CurrentZip,  
                             @Zip  
            EXEC log2_insert @LastUpdatedBy,  
                             'Location',  
                             @locID,  
                             'Zip',  
                             @CurrentZip,  
                             @Zip  
        END  
    END  
    SET @Val = NULL  
    IF (@Contact IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Main Contact' ORDER BY CreatedStamp DESC),'')  
        IF (@Val <> @Contact)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Main Contact',  
                             @Val,  
                             @Contact  
        END  
        ELSE  
        IF (@CurrentContact <> @Contact)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Main Contact',  
                             @CurrentContact,  
                             @Contact  
        END  
    END  
    SET @Val = NULL  
    IF (@Who IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Caller' ORDER BY CreatedStamp DESC),'')  
        IF (@Val <> @Who)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Caller',  
                             @Val,  
                    @Who  
        END  
        ELSE  
        IF (@CurrentWho <> @Who)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Caller',  
                             @CurrentWho,  
                             @Who  
        END  
    END  
    SET @Val = NULL  
    IF (@Phone IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Contact Phone' ORDER BY CreatedStamp DESC),'')  
        IF (@Val <> @Phone)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Contact Phone',  
                             @Val,  
                             @Phone  
        END  
        ELSE  
        IF (@CurrentPhone <> @Phone)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Contact Phone',  
                             @CurrentPhone,  
                             @Phone  
        END  
    END  
    SET @Val = NULL  
    IF (@Cell IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Caller Phone' ORDER BY CreatedStamp DESC),'')  
        IF (@Val <> @Cell)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Caller Phone',  
                             @Val,  
                             @Cell  
        END  
        ELSE  
        IF (@CurrentCell <> @Cell)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Caller Phone',  
                             @CurrentCell,  
                             @Cell  
        END  
    END  
    SET @Val = NULL  
    IF (@fBy IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Entered By' ORDER BY CreatedStamp DESC),'')  
        IF (@Val <> @fBy)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Entered By',  
                             @Val,  
                             @fBy  
        END  
        ELSE  
        IF (@CurrentfBy <> @fBy)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Entered By',  
                             @CurrentfBy,  
                             @fBy  
        END  
    END  
    SET @Val = NULL  
    IF (@CallDt IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Date Called In' ORDER BY CreatedStamp DESC),'')  
        DECLARE @Calldate nvarchar(150)  
        SELECT  
            @Calldate = CONVERT(varchar, @CallDt, 101)  
        IF (@Val <> @Calldate)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Date Called In',  
                             @Val,  
                             @Calldate  
        END  
        ELSE  
        IF (@CurrentCallDt <> @Calldate)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Date Called In',  
                             @CurrentCallDt,  
                             @Calldate  
        END  
    END  
    SET @Val = NULL  
    IF (@CallDt IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Called In Time' ORDER BY CreatedStamp DESC),'')  
        DECLARE @Calltime nvarchar(150)  
        SELECT  
            @Calltime = FORMAT(@CallDt, 'hh:mm tt')  
        IF (@Val <> @Calltime)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Called In Time',  
                             @Val,  
                             @Calltime  
        END  
        ELSE  
        IF (@CurrentCallDtTime <> @Calltime)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Called In Time',  
                             @CurrentCallDtTime,  
                             @Calltime  
        END  
    END  
    SET @Val = NULL  
    IF (@DefaultRoute IS NOT NULL  
        AND @DefaultRoute != 0)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Default Worker' ORDER BY CreatedStamp DESC),'')  
        DECLARE @DefaultRouteVal varchar(50)  
        SELECT  
            @DefaultRouteVal = Name  
        FROM Route  WITH (NOLOCK)
        WHERE ID = @DefaultRoute  
        IF (@Val <> @DefaultRouteVal)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Default Worker',  
                             @Val,  
                             @DefaultRouteVal  
        END  
        ELSE  
        IF (@CurrentDefaultRoute <> @DefaultRouteVal)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Default Worker',  
                             @CurrentDefaultRoute,  
                             @DefaultRouteVal  
        END  
    END  
    SET @Val = NULL  
    IF (@Category IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Category' ORDER BY CreatedStamp DESC),'')  
        IF (@Val <> @Category)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Category',  
                             @Val,  
                             @Category  
        END  
        ELSE  
        IF (@CurrentCategory <> @Category)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Category',  
                             @CurrentCategory,  
                             @Category  
        END  
    END  
    SET @Val = NULL  
    IF (@Reason IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Reason for service' ORDER BY CreatedStamp DESC),'')  
          
        IF (@Val <> CONVERT(varchar(1000), @Reason))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Reason for service',  
                             @Val,  
                             @Reason  
        END  
        ELSE  
        IF (@CurrentReason <> CONVERT(varchar(1000), @Reason))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Reason for service',  
                             @CurrentReason,  
                             @Reason  
        END  
    END  
    SET @Val = NULL  
    IF (@Status IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Status' ORDER BY CreatedStamp DESC),'')  
          
        DECLARE @StatusVal varchar(50)  
        SELECT  
            @StatusVal =  
                        CASE @Status  
                            WHEN 0 THEN 'Un-Assigned'  
                            WHEN 1 THEN 'Assigned'  
                            WHEN 2 THEN 'Enroute'  
                            WHEN 3 THEN 'Onsite'  
                            WHEN 4 THEN 'Completed'  
                            WHEN 5 THEN 'Hold'  
                        END  
        IF (@Val <> @StatusVal)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Status',  
                             @Val,  
                             @StatusVal  
        END  
        ELSE  
        IF (@CurrentStatus <> @StatusVal)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Status',  
                             @CurrentStatus,  
                             @StatusVal  
        END  
    END  
    SET @Val = NULL  
    IF (@SchDt IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Date Scheduled' ORDER BY CreatedStamp DESC),'')  
          
        DECLARE @SchDtdate nvarchar(150)  
        SELECT  
            @SchDtdate = CONVERT(varchar, @SchDt, 101)  
        IF (@Val <> @SchDtdate)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Date Scheduled',  
                             @Val,  
                             @SchDtdate  
        END  
        ELSE  
        IF (@CurrentSchDt <> @SchDtdate)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Date Scheduled',  
                             @CurrentSchDt,  
                             @SchDtdate  
        END  
    END  
    SET @Val = NULL  
    IF (@SchDt IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Time' ORDER BY CreatedStamp DESC),'')  
          
        DECLARE @SchDttime nvarchar(150)  
        SELECT  
            @SchDttime = FORMAT(@SchDt, 'hh:mm tt')  
        IF (@Val <> @SchDttime)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Time',  
                             @Val,  
                             @SchDttime  
        END  
        ELSE  
        IF (@CurrentSchDtTime <> @SchDttime)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Time',  
                             @CurrentSchDtTime,  
                             @SchDttime  
        END  
    END  
    SET @Val = NULL  
    IF (@EST IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Estimate' ORDER BY CreatedStamp DESC),'')  
          
        IF (@Val <> CONVERT(varchar(30), @EST))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Estimate',  
                             @Val,  
                             @EST  
        END  
        ELSE  
        IF (@CurrentEST <> CONVERT(varchar(30), @EST))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Estimate',  
           @CurrentEST,  
                             @EST  
        END  
    END  
    SET @Val = NULL  
    IF (@Worker IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Assigned Worker' ORDER BY CreatedStamp DESC),'')  
          
        IF (@Val <> @Worker)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Assigned Worker',  
                             @Val,  
                             @Worker  
        END  
        ELSE  
        IF (@CurrentWorker <> @Worker)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Assigned Worker',  
                             @CurrentWorker,  
                             @Worker  
        END  
    END  
    SET @Val = NULL  
    IF (@DispAlert IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Dispatch Alert' ORDER BY CreatedStamp DESC),'')  
          
        IF (@Val <> CONVERT(varchar(10), @DispAlert))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Dispatch Alert',  
                             @Val,  
                             @DispAlert  
            EXEC log2_insert @LastUpdatedBy,  
                             'Location',  
                             @locID,  
                             'Dispatch Alert',  
                             @Val,  
                             @DispAlert  
        END  
        ELSE  
        IF (@CurrentDispAlert <> CONVERT(varchar(10), @DispAlert))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Dispatch Alert',  
                             @CurrentDispAlert,  
                             @DispAlert  
            EXEC log2_insert @LastUpdatedBy,  
                             'Location',  
                             @locID,  
                             'Dispatch Alert',  
                             @CurrentDispAlert,  
                             @DispAlert  
        END  
    END  
    SET @Val = NULL  
    IF (@CreditHold IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Credit Hold' ORDER BY CreatedStamp DESC),'')  
        IF (@Val <> CONVERT(varchar(10), @CreditHold))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Credit Hold',  
                             @Val,  
                             @CreditHold  
            EXEC log2_insert @LastUpdatedBy,  
                             'Location',  
                             @locID,  
                             'Credit Hold',  
                             @Val,  
                             @CreditHold  
        END  
        ELSE  
        IF (@CurrentCreditHold <> CONVERT(varchar(10), @CreditHold))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Credit Hold',  
                             @CurrentCreditHold,  
                             @CreditHold  
            EXEC log2_insert @LastUpdatedBy,  
                             'Location',  
                             @locID,  
                             'Credit Hold',  
                             @CurrentCreditHold,  
                             @CreditHold  
        END  
    END  
    SET @Val = NULL  
    IF (@CreditReason IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Reason' ORDER BY CreatedStamp DESC),'')  
          
        IF (@Val <> CONVERT(varchar(1000), @CreditReason))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Reason',  
                             @Val,  
                             @CreditReason  
            EXEC log2_insert @LastUpdatedBy,  
                             'Location',  
                             @locID,  
                             'Reason',  
                             @Val,  
                             @CreditReason  
        END  
        ELSE  
        IF (@CurrentCreditReason <> CONVERT(varchar(1000), @CreditReason))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Reason',  
                             @CurrentCreditReason,  
                             @CreditReason  
            EXEC log2_insert @LastUpdatedBy,  
                             'Location',  
                             @locID,  
                             'Reason',  
                             @CurrentCreditReason,  
                             @CreditReason  
        END  
    END  
    SET @Val = NULL  
    IF (@remarks IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Remarks' ORDER BY CreatedStamp DESC),'')  
          
        IF (@Val <> CONVERT(varchar(1000), @remarks))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Remarks',  
                             @Val,  
                             @remarks  
            EXEC log2_insert @LastUpdatedBy,  
                             'Location',  
                             @locID,  
                             'Remarks',  
                             @Val,  
                             @Remarks  
        END  
        ELSE  
        IF (@CurrentRemarks <> CONVERT(varchar(1000), @remarks))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Remarks',  
                             @CurrentRemarks,  
                             @remarks  
            EXEC log2_insert @LastUpdatedBy,  
                             'Location',  
                             @locID,  
                             'Remarks',  
                             @CurrentRemarks,  
                             @Remarks  
        END  
    END  
    SET @Val = NULL  
    IF (@WorkComplete IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Work Complete' ORDER BY CreatedStamp DESC),'')  
          
        IF (@Val <> CONVERT(varchar(10), @WorkComplete))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Work Complete',  
                             @Val,  
                             @WorkComplete  
        END  
        ELSE  
        IF (@CurrentWorkComplete <> CONVERT(varchar(10), @WorkComplete))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Work Complete',  
                             @CurrentWorkComplete,  
                             @WorkComplete  
        END  
    END  
    SET @Val = NULL  
    IF (@Review IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Review' ORDER BY CreatedStamp DESC),'')  
          
        IF (@Val <> CONVERT(varchar(10), @Review))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Review',  
                             @Val,  
                             @Review  
        END  
        ELSE  
        IF (@CurrentReview <> CONVERT(varchar(10), @Review))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Review',  
                             @CurrentReview,  
                             @Review  
        END  
    END  

	-----
	SET @Val = NULL  
    IF (@ClearPR IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'ClearPR' ORDER BY CreatedStamp DESC),'')  
          
        IF (@Val <> CONVERT(varchar(10), @ClearPR))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Payroll',  
                             @Val,  
                             @ClearPR  
        END  
        ELSE  
        IF (@CurrentClearPR <> CONVERT(varchar(10), @ClearPR))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Payroll',  
                             @CurrentClearPR,  
                             @ClearPR  
        END  
    END  

    SET @Val = NULL  
    IF (@Charge IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Chargeable' ORDER BY CreatedStamp DESC),'')  
         
        IF (@Val <> CONVERT(varchar(10), @Charge))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Chargeable',  
                             @Val,  
                             @Charge  
        END  
        ELSE  
        IF (@CurrentCharge <> CONVERT(varchar(10), @Charge))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Chargeable',  
                             @CurrentCharge,  
                             @Charge  
        END  
    END  
    SET @Val = NULL  
    IF (@TransferTime IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Timesheet' ORDER BY CreatedStamp DESC),'')  
          
        IF (@Val <> CONVERT(varchar(10), @TransferTime))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Timesheet',  
                             @Val,  
                             @TransferTime  
        END  
        ELSE  
        IF (@CurrentTransferTime <> CONVERT(varchar(10), @TransferTime))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Timesheet',  
                             @CurrentTransferTime,  
                             @TransferTime  
        END  
    END  
    SET @Val = NULL  
    IF (@Internet IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Internet' ORDER BY CreatedStamp DESC),'')  
          
        IF (@Val <> CONVERT(varchar(10), @Internet))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Internet',  
                             @Val,  
                             @Internet  
        END  
        ELSE  
        IF (@CurrentInternet <> CONVERT(varchar(10), @Internet))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Internet',  
                             @CurrentInternet,  
                             @Internet  
        END  
    END  
    SET @Val = NULL  
    IF (@complDesc IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Work Complete Desc' ORDER BY CreatedStamp DESC),'')  
        IF (@Val <> CONVERT(varchar(1000), @complDesc))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Work Complete Desc',  
                             @Val,  
                             @complDesc  
END  
        ELSE  
        IF (@CurrentcomplDesc <> CONVERT(varchar(1000), @complDesc))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Work Complete Desc',  
                             @CurrentcomplDesc,  
                             @complDesc  
        END  
    END  
    SET @Val = NULL  
    IF (@Reg IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'RT' ORDER BY CreatedStamp DESC),'')  
          
        IF (@Val <> CONVERT(varchar(30), @Reg))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'RT',  
                             @Val,  
                             @Reg  
        END  
        ELSE  
        IF (@CurrentReg <> CONVERT(varchar(30), @Reg))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'RT',  
                             @CurrentReg,  
                             @Reg  
        END  
    END  
    SET @Val = NULL  
    IF (@OT IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'OT' ORDER BY CreatedStamp DESC),'')  
          
        IF (@Val <> CONVERT(varchar(30), @OT))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'OT',  
                             @Val,  
                             @OT  
        END  
        ELSE  
        IF (@CurrentOT <> CONVERT(varchar(30), @OT))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'OT',  
                             @CurrentOT,  
                             @OT  
        END  
    END  
    SET @Val = NULL  
    IF (@EnrouteTime IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'En Route' ORDER BY CreatedStamp DESC),'')  
          
        DECLARE @Enroute nvarchar(150)  
        SELECT  
            @Enroute = FORMAT(@EnrouteTime, 'hh:mm tt')  
        IF (@Val <> @Enroute)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'En Route',  
                             @Val,  
                             @Enroute  
        END  
        ELSE  
        IF (@CurrentEnrouteTime <> @Enroute)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'En Route',  
                             @CurrentEnrouteTime,  
                             @Enroute  
        END  
    END  
    SET @Val = NULL  
    IF (@NT IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'NT' ORDER BY CreatedStamp DESC),'')  
          
        IF (@Val <> CONVERT(varchar(30), @NT))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'NT',  
                             @Val,  
                             @NT  
        END  
        ELSE  
        IF (@CurrentNT <> CONVERT(varchar(30), @NT))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'NT',  
                             @CurrentNT,  
                             @NT  
        END  
    END  
    SET @Val = NULL  
    IF (@DT IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'DT' ORDER BY CreatedStamp DESC),'')  
          
        IF (@Val <> CONVERT(varchar(30), @DT))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'DT',  
                             @Val,  
                             @DT  
        END  
        ELSE  
        IF (@CurrentDT <> CONVERT(varchar(30), @DT))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'DT',  
                             @CurrentDT,  
                             @DT  
        END  
    END  
    SET @Val = NULL  
    IF (@Onsite IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'On Site' ORDER BY CreatedStamp DESC),'')  
          
        DECLARE @Onsitetime nvarchar(150)  
        SELECT  
            @Onsitetime = FORMAT(@Onsite, 'hh:mm tt')  
        IF (@Val <> @Onsitetime)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'On Site',  
                             @Val,  
                             @Onsitetime  
        END  
        ELSE  
        IF (@CurrentOnsite <> @Onsitetime)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'On Site',  
                             @CurrentOnsite,  
                             @Onsitetime  
        END  
    END  
    SET @Val = NULL  
    IF (@TT IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'TT' ORDER BY CreatedStamp DESC),'')  
        IF (@Val <> CONVERT(varchar(30), @TT))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'TT',  
                             @Val,  
                             @TT  
        END  
        ELSE  
        IF (@CurrentTT <> CONVERT(varchar(30), @TT))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'TT',  
                             @CurrentTT,  
                             @TT  
        END  
    END  
    SET @Val = NULL  
    IF (@BT IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'BT' ORDER BY CreatedStamp DESC),'0')  
          
        IF (Convert(numeric(30,2),@Val) <> @BT)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'BT',  
                             @Val,  
                             @BT  
        END  
        ELSE  
        IF (Convert(numeric(30,2),@CurrentBT) <> @BT)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'BT',  
                             @CurrentBT,  
                             @BT  
        END  
    END  
    SET @Val = NULL  
    IF (@Complete IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Completed' ORDER BY CreatedStamp DESC),'')  
          
        DECLARE @Completetime nvarchar(150)  
        SELECT  
            @Completetime = FORMAT(@Complete, 'hh:mm tt')  
        IF (@Val <> @Completetime)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Completed',  
                             @Val,  
                             @Completetime  
        END  
        ELSE  
        IF (@CurrentComplete <> @Completetime)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Completed',  
                             @CurrentComplete,  
                             @Completetime  
        END  
    END  
    SET @Val = NULL  
    IF (@job IS NOT NULL  
        AND @job != 0)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Project#' ORDER BY CreatedStamp DESC),'')  
          
        IF (@Val <> @job)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Project#',  
                             @Val,  
                             @job  
        END  
        ELSE  
        IF (@Currentjob <> @job)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Project#',  
                             @Currentjob,  
                             @job  
        END  
    END  
    SET @Val = NULL  
    IF (@JobCode IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Project Type' ORDER BY CreatedStamp DESC),'')  
          
        DECLARE @JobCodeVal varchar(10)  
        SELECT  
            @JobCodeVal = (SELECT  
                items  
            FROM dbo.IDSplit(@JobCode, ':')  
            WHERE row = 2)  
        IF (@Val <> @JobCodeVal)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Project Type',  
                             @Val,  
                             @JobCodeVal  
        END  
        ELSE  
        IF (@CurrentJobCode <> @JobCodeVal)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Project Type',  
                             @CurrentJobCode,  
                             @JobCodeVal  
        END  
    END  
    SET @Val = NULL  
    IF (@Type IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Department' ORDER BY CreatedStamp DESC),'')  
        DECLARE @DeptType varchar(50)  
        SELECT  
            @DeptType = Type  
        FROM jobtype  WITH (NOLOCK)
        WHERE ID = @Type  
        IF (@Val <> @DeptType)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Department',  
                             @Val,  
                             @DeptType  
        END  
        ELSE  
        IF (@CurrentType <> @DeptType)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Department',  
                             @CurrentType,  
                             @DeptType  
        END  
    END  
    SET @Val = NULL  
    IF (@wage IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Wage' ORDER BY CreatedStamp DESC),'')  
        DECLARE @wageVal varchar(50)  
        SELECT  
            @wageVal = fdesc  
        FROM PRWage WITH (NOLOCK) 
        WHERE ID = @wage  
        IF (@Val <> @wageVal)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                      'Ticket',  
                             @TicketID,  
                             'Wage',  
                             @Val,  
                             @wageVal  
        END  
        ELSE  
        IF (@Currentwage <> @wageVal)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Wage',  
                             @Currentwage,  
                             @wageVal  
        END  
    END  
    SET @Val = NULL  
    IF (@QBServiceItem IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Service Item' ORDER BY CreatedStamp DESC),'')  
        DECLARE @QBServiceItemVal varchar(100)  
        SELECT  
            @QBServiceItemVal = Name  
        FROM Inv  WITH (NOLOCK)
        WHERE QBInvID = @QBServiceItem  
        IF (@Val <> @QBServiceItemVal)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Service Item',  
                             @Val,  
                             @QBServiceItemVal  
        END  
        ELSE  
        IF (@CurrentQBServiceItem <> @QBServiceItemVal)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Service Item',  
                             @CurrentQBServiceItem,  
                             @QBServiceItemVal  
        END  
    END  
    SET @Val = NULL  
    IF (@QBPayrollItem IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Payroll Item' ORDER BY CreatedStamp DESC),'')  
        DECLARE @QBPayrollItemVal varchar(100)  
        SELECT  
            @QBPayrollItemVal = fdesc  
        FROM prwage  WITH (NOLOCK)
        WHERE QBwageID = @QBPayrollItem  
        IF (@Val <> @QBPayrollItemVal)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Payroll Item',  
                             @Val,  
                             @QBPayrollItemVal  
        END  
        ELSE  
        IF (@CurrentQBPayrollItem <> @QBPayrollItemVal)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Payroll Item',  
                             @CurrentQBPayrollItem,  
                             @QBPayrollItemVal  
        END  
    END  
    SET @Val = NULL  
    IF (@Recommendation IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Recommendation' ORDER BY CreatedStamp DESC),'')  
        IF (@Val <> @Recommendation)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Recommendation',  
                             @Val,  
                             @Recommendation  
        END  
        ELSE  
        IF (@CurrentRecommendation <> @Recommendation)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Recommendation',  
                             @CurrentRecommendation,  
                             @Recommendation  
        END  
    END  
    SET @Val = NULL  
    IF (@PartsUsed IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Parts Used' ORDER BY CreatedStamp DESC),'')  
        IF (@Val <> @PartsUsed)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Parts Used',  
                             @Val,  
                             @PartsUsed  
        END  
        ELSE  
        IF (@CurrentPartsUsed <> @PartsUsed)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Parts Used',  
                             @CurrentPartsUsed,  
                             @PartsUsed  
        END  
    END  
    SET @Val = NULL  
    IF (@Comments IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Comments' ORDER BY CreatedStamp DESC),'')  
        IF (@Val <> @Comments)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Comments',  
                             @Val,  
                             @Comments  
        END  
        ELSE  
        IF (@CurrentComments <> @Comments)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Comments',  
                             @CurrentComments,  
                             @Comments  
        END  
    END  
    SET @Val = NULL  
    IF (@MiscExp IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Miscellaneous' ORDER BY CreatedStamp DESC),'')  
        IF (@Val <> CONVERT(varchar(30), @MiscExp))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Miscellaneous',  
                             @Val,  
                             @MiscExp  
        END  
        ELSE  
        IF (@CurrentMiscExp <> CONVERT(varchar(30), @MiscExp))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Miscellaneous',  
                             @CurrentMiscExp,  
                             @MiscExp  
        END  
    END  
    SET @Val = NULL  
    IF (@MileStart IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Starting Mileage' ORDER BY CreatedStamp DESC),'')  
        IF (@Val <> CONVERT(varchar(30), @MileStart))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Starting Mileage',  
                             @Val,  
                             @MileStart  
        END  
        ELSE  
        IF (@CurrentMileStart <> CONVERT(varchar(30), @MileStart))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Starting Mileage',  
                             @CurrentMileStart,  
                             @MileStart  
        END  
    END  
    SET @Val = NULL  
    IF (@ZoneExp IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Zone' ORDER BY CreatedStamp DESC),'')  
        IF (@Val <> CONVERT(varchar(30), @ZoneExp))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Zone',  
                             @Val,  
                             @ZoneExp  
        END  
        ELSE  
        IF (@CurrentZoneExp <> CONVERT(varchar(30), @ZoneExp))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                        @TicketID,  
                             'Zone',  
                             @CurrentZoneExp,  
                             @ZoneExp  
        END  
    END  
    SET @Val = NULL  
    IF (@MileEnd IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Ending Mileage' ORDER BY CreatedStamp DESC),'')  
        IF (@Val <> CONVERT(varchar(30), @MileEnd))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Ending Mileage',  
                             @Val,  
                             @MileEnd  
        END  
        ELSE  
        IF (@CurrentMileEnd <> CONVERT(varchar(30), @MileEnd))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Ending Mileage',  
                             @CurrentMileEnd,  
                             @MileEnd  
        END  
    END  
    SET @Val = NULL  
    IF (@TollExp IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Toll' ORDER BY CreatedStamp DESC),'')  
        IF (@Val <> CONVERT(varchar(30), @TollExp))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Toll',  
                             @Val,  
                             @TollExp  
        END  
        ELSE  
        IF (@CurrentTollExp <> CONVERT(varchar(30), @TollExp))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Toll',  
                             @CurrentTollExp,  
                             @TollExp  
        END  
    END  
    SET @Val = NULL  
    IF (@Custom1 IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Custom 1' ORDER BY CreatedStamp DESC),'')  
        IF (@Val <> @Custom1)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Custom 1',  
                             @Val,  
                             @Custom1  
        END  
        ELSE  
        IF (@CurrentCustom1 <> @Custom1)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Custom 1',  
                             @CurrentCustom1,  
                             @Custom1  
        END  
    END  
    SET @Val = NULL  
    IF (@Custom2 IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Custom 2' ORDER BY CreatedStamp DESC),'')  
        IF (@Val <> @Custom2)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Custom 2',  
                             @Val,  
                             @Custom2  
        END  
        ELSE  
        IF (@CurrentCustom2 <> @Custom2)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Custom 2',  
                             @CurrentCustom2,  
                             @Custom2  
        END  
    END  
    SET @Val = NULL  
    IF (@Custom3 IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Custom 3' ORDER BY CreatedStamp DESC),'')  
        IF (@Val <> @Custom3)  
        BEGIN  
        EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Custom 3',  
                             @Val,  
                             @Custom3  
        END  
        ELSE  
        IF (@CurrentCustom3 <> @Custom3)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Custom 3',  
                             @CurrentCustom3,  
                             @Custom3  
        END  
    END  
    SET @Val = NULL  
    IF (@Custom4 IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Custom 4' ORDER BY CreatedStamp DESC),'')  
        IF (@Val <> @Custom4)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Custom 4',  
                             @Val,  
                             @Custom4  
        END  
        ELSE  
        IF (@CurrentCustom4 <> @Custom4)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Custom 4',  
                             @CurrentCustom4,  
                             @Custom4  
        END  
    END  
    SET @Val = NULL  
    IF (@Custom5 IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Custom 5' ORDER BY CreatedStamp DESC),'')  
        IF (@Val <> @Custom5)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Custom 5',  
                             @Val,  
                             @Custom5  
        END  
        ELSE  
        IF (@CurrentCustom5 <> @Custom5)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Custom 5',  
                             @CurrentCustom5,  
                             @Custom5  
        END  
    END  
    SET @Val = NULL  
    IF (@Custom6 IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Custom 6' ORDER BY CreatedStamp DESC),'')  
        IF (@Val <> CONVERT(varchar(10), @Custom6))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Custom 6',  
                             @Val,  
                             @Custom6  
        END  
        ELSE  
        IF (@CurrentCustom6 <> CONVERT(varchar(10), @Custom6))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Custom 6',  
                             @CurrentCustom6,  
                             @Custom6  
        END  
    END  
    SET @Val = NULL  
    IF (@Custom7 IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Custom 7' ORDER BY CreatedStamp DESC),'')  
        IF (@Val <> CONVERT(varchar(10), @Custom7))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Custom 7',  
                             @Val,  
                             @Custom7  
        END  
        ELSE  
        IF (@CurrentCustom7 <> CONVERT(varchar(10), @Custom7))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                            'Custom 7',  
                             @CurrentCustom7,  
                             @Custom7  
        END  
    END  
    SET @Val = NULL  
    IF (@Customtick1 IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Ticket Custom 1' ORDER BY CreatedStamp DESC),'')  
        IF (@Val <> @Customtick1)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Ticket Custom 1',  
                             @Val,  
                             @Customtick1  
        END  
        ELSE  
        IF (@CurrentCustomtick1 <> @Customtick1)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Ticket Custom 1',  
                             @CurrentCustomtick1,  
                             @Customtick1  
        END  
    END  
    SET @Val = NULL  
    IF (@Customtick2 IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Ticket Custom 2' ORDER BY CreatedStamp DESC),'')  
        IF (@Val <> @Customtick2)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Ticket Custom 2',  
                             @Val,  
                             @Customtick2  
        END  
        ELSE  
        IF (@CurrentCustomtick2 <> @Customtick2)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Ticket Custom 2',  
                             @CurrentCustomtick2,  
                             @Customtick2  
        END  
    END  
    SET @Val = NULL  
    IF (@Customtick5 IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Ticket Custom 3' ORDER BY CreatedStamp DESC),'0')  
        IF (@Val <> @Customtick5)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Ticket Custom 3',  
                             @Val,  
                             @Customtick5  
        END  
        ELSE  
        IF (@CurrentCustomtick5 <> @Customtick5)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Ticket Custom 3',  
                             @CurrentCustomtick5,  
                             @Customtick5  
        END  
    END  
    SET @Val = NULL  
    IF (@Customtick3 IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Ticket Checkbox 1' ORDER BY CreatedStamp DESC),'')  
        IF (@Val <> CONVERT(varchar(10), @Customtick3))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Ticket Checkbox 1',  
                             @Val,  
                             @Customtick3  
        END  
        ELSE  
        IF (@CurrentCustomtick3 <> CONVERT(varchar(10), @Customtick3))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Ticket Checkbox 1',  
                             @CurrentCustomtick3,  
                             @Customtick3  
        END  
    END  
    SET @Val = NULL  
    IF (@Customtick4 IS NOT NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Ticket Checkbox 2' ORDER BY CreatedStamp DESC),'')  
        IF (@Val <> CONVERT(varchar(10), @Customtick4))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Ticket Checkbox 2',  
                             @Val,  
                             @Customtick4  
        END  
        ELSE  
        IF (@CurrentCustomtick4 <> CONVERT(varchar(10), @Customtick4))  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Ticket Checkbox 2',  
                             @CurrentCustomtick4,  
                             @Customtick4  
        END  
    END  
    IF (@CurrentEquipment IS NOT NULL  
        OR @CurrentEquipment IS NULL)  
    BEGIN  
  SET @Val = ISNULL((SELECT TOP 1 newVal FROM log2 WITH (NOLOCK) WHERE screen = 'Ticket' AND ref = @TicketID AND Field = 'Equipment' ORDER BY CreatedStamp DESC),'')  
        DECLARE @EquipmentJob varchar(1000)  
        SELECT  
            @EquipmentJob = STUFF((SELECT  
                ',  ' + e.Unit  
            FROM Elev e  
            INNER JOIN multiple_equipments ej  
                ON e.ID = ej.elev_id  
            WHERE ej.ticket_id = @TicketID  
            FOR xml PATH (''))  
            , 1, 1, '')  
        IF (@Val <> @EquipmentJob)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Equipment',  
                             @Val,  
                             @EquipmentJob  
        END  
        ELSE  
        IF (@CurrentEquipment <> @EquipmentJob)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Equipment',  
                             @CurrentEquipment,  
                             @EquipmentJob  
        END  
        ELSE  
        IF (@EquipmentJob IS NOT NULL  
            AND @Val IS NULL)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Equipment',  
                             '',  
                             @EquipmentJob  
        END  
        ELSE  
        IF (@Val != ''  
            AND @EquipmentJob IS NULL)  
        BEGIN  
            EXEC log2_insert @LastUpdatedBy,  
                             'Ticket',  
                             @TicketID,  
                             'Equipment',  
                             @CurrentEquipment,  
                             ''  
        END  
    END  
  
    /********End Logs************/  
    --COMMIT TRANSACTION        
  
    SELECT  @job
