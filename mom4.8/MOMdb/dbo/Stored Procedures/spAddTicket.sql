
Create PROCEDURE [dbo].[spAddTicket]      
 @LocID           INT,        
 @LocTag          VARCHAR(50),        
 @LocAdd          VARCHAR(255),        
 @City            VARCHAR(50),        
 @State           VARCHAR(2),        
 @Zip             VARCHAR(100),        
 @Phone           VARCHAR(28),        
 @Cell            VARCHAR(50),        
 @Worker          VARCHAR(50),        
 @CallDt          DATETIME,        
 @SchDt           DATETIME,        
 @Status          SMALLINT,        
 @EnrouteTime     DATETIME,        
 @Onsite          DATETIME,        
 @Complete        DATETIME,        
 @Category        VARCHAR(25),        
 @Unit            INT,        
 @Reason          TEXT,        
 @CustName        VARCHAR(50),        
 @custID          INT,        
 @EST             NUMERIC(30, 2),        
 @complDesc       TEXT,        
 @TicketIDOut     INT output,        
 @AID             UNIQUEIDENTIFIER,        
 @Who             VARCHAR(30),        
 @sign            IMAGE,        
 @Reg             NUMERIC(30, 2),        
 @OT              NUMERIC(30, 2),        
 @NT              NUMERIC(30, 2),        
 @TT              NUMERIC(30, 2),        
 @DT              NUMERIC(30, 2),        
 @Total           NUMERIC(30, 2),        
 @Charge          INT,        
 @Review          INT,        
 @Remarks         TEXT,        
 @Level           INT,        
 @Type            INT,        
 @job             INT = NULL,        
 @Custom1         VARCHAR(50),        
 @Custom2         VARCHAR(50),        
 @Custom3         VARCHAR(50),        
 @Custom4         VARCHAR(50),        
 @Custom5         VARCHAR(50),        
 @Custom6         TINYINT,        
 @Custom7         TINYINT,        
 @WorkOrder       VARCHAR(10),        
 @WorkComplete    INT,        
 @MiscExp         NUMERIC(30, 2),        
 @TollExp         NUMERIC(30, 2),        
 @ZoneExp         NUMERIC(30, 2),        
 @MileStart       INT,        
 @MileEnd         INT,        
 @Internet        SMALLINT,        
 @Invoice         VARCHAR(50),        
 @TransferTime    INT,        
 @CreditHold      TINYINT,        
 @DispAlert       TINYINT,        
 @CreditReason    TEXT,        
 @IsRecurring     TINYINT,        
 @QBServiceItem   VARCHAR(100),        
 @QBPayrollItem   VARCHAR(100),        
 @LastUpdatedBy   VARCHAR(50),        
 @Contact         VARCHAR(50),        
 @Recommendation  VARCHAR(255),        
 @Customtick1     VARCHAR(50),        
 @Customtick2     VARCHAR(50),        
 @Customtick3     TINYINT,        
 @Customtick4     TINYINT,        
 @lat             VARCHAR(50),        
 @lng             VARCHAR(50),        
 @DefaultRoute    INT,        
 --@Customtick5     NUMERIC(30, 2),
 @Customtick5     VARCHAR(100), 
 @JobCode         VARCHAR(10),        
 @ProjectTemplate INT,        
 @wage            INT,        
 @fby             VARCHAR(50),        
 @RecurringDate   DATETIME = NULL,        
 @Equipments      AS TBLTYPEMULTIPLEEEQUIPMENTS readonly,        
 @UpdateTasks     SMALLINT = 0,        
 @TaskCodes       AS TBLTYPETASKCODES readonly,        
 @BT              NUMERIC(30, 2)=0,        
 @Comments        VARCHAR(1000)=NULL,        
 @PartsUsed       VARCHAR(100)=NULL,        
 @Zone int = null  ,        
  @dtTicketINV as  tblTypeTicketINV  readonly,  
  @screen varchar(50)=null  
                 
AS        
    DECLARE @TicketID INT        
    DECLARE @Rol INT        
    DECLARE @Nature SMALLINT = 0        
    DECLARE @Ltype SMALLINT = 0        
    DECLARE @ProspectID INT        
    DECLARE @DucplicateProspectName INT        
    DECLARE @prospectcreate INT = 0        
    DECLARE @Phase INT         
 DECLARE @ProjDate as DATETIME = GETDATE();      
    
 IF(@IsRecurring = 1  )        
 BEGIN        
 set @RecurringDate=isnull(@RecurringDate,@SchDt);        
 END        
 if(@fby ='Portal')        
 Begin        
 SET @Nature  = 1         
 End        
        
    --------------- // if the location on credit hold it needs to give a warning and prevent the user---------------        
    IF EXISTS(SELECT 1        
              FROM   loc  WITH (NOLOCK)         
              WHERE  loc = @LocID        
                     AND @custID <> 0        
                     AND credit = 1)        
      BEGIN        
          RAISERROR ('Location on credit hold can not create ticket!',16,1)        
        
          RETURN        
      END        
    
    --------------- if the Job Cost Labor = Burden Rate------------------        
    IF( [dbo].Checkwagesisrequired(@Worker, 1) = 1 )        
      BEGIN        
          IF ( @Status = 4 )        
            BEGIN        
                RAISERROR ('Please add atleast a single wage item for selected worker!',16,1)        
        
                RETURN        
            END        
      END        
        
    --BEGIN TRANSACTION        
        
         
    SELECT @TicketID = Max([NewID]) + 1        
    FROM   (SELECT Isnull(Max(TicketO.ID), 0) AS [NewID]        
            FROM   TicketO  WITH (NOLOCK)        
            UNION ALL        
            SELECT Isnull(Max(TicketD.ID), 0) AS [NewID]        
            FROM   TicketD  WITH (NOLOCK) ) A        
        
    SET @TicketIDOut=@TicketID        
        
    IF( @WorkOrder = '' )        
      BEGIN        
          SET @WorkOrder = @TicketID        
      END        
        
    /* When adding prospects */        
    IF( @custID = 0 )        
      BEGIN        
          SET @custID = NULL        
          SET @Nature = 1        
          SET @Ltype = 1        
        
          IF( @LocID = 0 )        
            BEGIN        
                SET @prospectcreate = 1        
        
                SELECT @DucplicateProspectName = Count(1)        
                FROM   Rol r   WITH (NOLOCK)        
                       INNER JOIN Prospect p        
                               ON p.Rol = r.ID        
                WHERE  NAME = @CustName        
        
                IF( @DucplicateProspectName <> 0 )        
                  BEGIN        
                      RAISERROR ('Prospect name already exists, please use different Prospect name !',16,1)        
        
                      RETURN        
                  END        
        
                SELECT @ProspectID = Isnull(Max(ID), 0) + 1        
                FROM   Prospect   WITH (NOLOCK)        
        
                INSERT INTO Rol        
                            (NAME,        
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
                             Lat,        
                             Lng)        
                VALUES      ( @CustName,        
                              @LocAdd,        
                              @City,        
                              @State,        
                              @Zip,        
                              @Phone,        
                              @Contact,        
                              'Created on Ticket# '        
                              + CONVERT(VARCHAR(50), @TicketID) + Space(2)        
                              + CONVERT(VARCHAR(max), @Remarks),        
                              3,        
                              0,        
                              0,        
                              0,        
                              Getdate(),        
                              Getdate(),        
                              1,        
                              @Cell,        
                              'United States',        
                              @lat,        
                              @lng )        
        
               IF @@ERROR <> 0        
                   AND @@TRANCOUNT > 0        
                  BEGIN        
                      RAISERROR ('Error Occured',16,1)        
        
                      --ROLLBACK TRANSACTION        
        
                      RETURN        
                  END        
        
                SET @Rol=Scope_identity()        
        
                INSERT INTO Prospect        
                            (ID,        
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
                             CreatedBy,        
                             LastUpdatedBy,        
                             CustomerName)        
                VALUES      ( @ProspectID,        
                              @Rol,        
                              '',        
                              1,        
                              0,        
                              Getdate(),        
                              Cast(Cast('12/30/1899' AS DATE) AS DATETIME)        
                              + Cast( Cast(Getdate() AS TIME)AS DATETIME),        
                              0,        
                              Getdate(),        
                              0,        
                              Getdate(),        
                              Getdate(),        
                              @LastUpdatedBy,        
                              @LastUpdatedBy,        
                              @CustName )        
        
                IF @@ERROR <> 0        
                   AND @@TRANCOUNT > 0        
                  BEGIN        
                      RAISERROR ('Error Occured',16,1)        
        
                      --ROLLBACK TRANSACTION        
        
                      RETURN        
                  END        
        
                SET @LocID = @ProspectID        
        
                --update PType set [Count] = [Count]+1 where [Type] = 'General'        
                IF NOT EXISTS(SELECT 1        
                              FROM   Phone  WITH (NOLOCK)        
                              WHERE  Rol = @Rol        
                                     AND fDesc = @contact)        
                  BEGIN        
                      INSERT INTO Phone        
                                  (Rol,        
                                   fDesc,        
                                   Phone,        
                                   Cell)        
         VALUES      ( @Rol,        
                                    @contact,        
                                    @phone,        
                                    @cell )        
                  END        
        
                IF @@ERROR <> 0        
                   AND @@TRANCOUNT > 0        
                  BEGIN        
                      RAISERROR ('Error Occured',16,1)        
        
                      --ROLLBACK TRANSACTION        
        
                      RETURN        
                  END        
            END        
      END        
        
    DECLARE @equipcounts INT = 0        
        
    SELECT @equipcounts = Count(1)        
    FROM   @Equipments        
        
    IF( @equipcounts = 1 )        
      BEGIN        
          SELECT @Unit = elev_id        
          FROM   @Equipments        
      END        
    ELSE IF( @equipcounts > 1 )        
      BEGIN        
          SELECT TOP 1 @Unit = elev_id        
          FROM   @Equipments        
      END        
    ELSE IF( @equipcounts = 0 )        
      BEGIN        
          SET @Unit = 0        
      END        
        
    /* whene ticket status is other than completed */        
    IF( @Status <> 4 )        
      BEGIN        
          IF NOT EXISTS(SELECT 1        
                        FROM   TicketO   WITH (NOLOCK)       
                        WHERE  ID = @TicketID)        
            BEGIN        
                INSERT INTO TicketO        
                            (ID,        
                             LDesc1,        
                             LDesc2,        
                             LDesc3,        
                             LDesc4,        
                             City,        
                             State,        
                             Zip,        
                             Phone,        
                             CPhone,        
                             DWork,        
                             CDate,        
                             EDate,        
                             Assigned,        
                             TimeRoute,        
                             TimeSite,        
                             TimeComp,        
                             Cat,        
                             LElev,        
                             fDesc,        
                             Est,        
                             [Owner],        
                             LID,        
                             fWork,        
                             LType,        
                             Confirmed,        
                             Who,        
         Type,        
                             AID,        
                             BRemarks,        
                             Level,        
                             Job,        
                             Custom1,        
                             Custom2,        
                             Custom3,        
                             Custom4,        
                             Custom5,        
                             Custom6,        
                             Custom7,        
                             WorkOrder,        
                             Nature,        
                             fBy,        
                             QBServiceItem,        
                             QBPayrollItem,        
                             CustomTick1,        
                             CustomTick2,        
                             CustomTick3,        
                             CustomTick4,        
                             CustomTick5,        
                             Recurring  ,   
        Charge      
         )        
                VALUES      ( @TicketID,        
                              --CONVERT(varchar(50), @LocID),        
                              CASE        
                                WHEN ( @custID IS NULL ) THEN CONVERT(VARCHAR(50), @LocID)        
                                ELSE (SELECT ID        
                                      FROM   Loc  WITH (NOLOCK)         
                                      WHERE  Loc = @locid)        
                              END,        
                              ----@LocTag,        
                              ----(select top 1 ( select top 1  name from Rol where ID=l.Rol) as name  from loc l where Loc=@locid),        
                              CASE        
                                WHEN ( @custID IS NULL ) THEN (SELECT r.NAME        
                                                               FROM   Prospect p    WITH (NOLOCK)      
                                                                      INNER JOIN Rol r   WITH (NOLOCK)        
                                                                            ON r.ID = p.Rol        
                                                               WHERE  p.ID = @LocID)        
                                ELSE (SELECT Tag        
                                      FROM   Loc  WITH (NOLOCK)        
                                      WHERE  Loc = @locid)        
                              END,        
                              @LocAdd,        
                              @City + ',' + Space(1) + @State + ',' + Space(1) + @Zip,        
                              @City,        
                              @State,        
                              @Zip,        
                              @Phone,        
                              @Cell,        
                              @Worker,        
                              @CallDt,        
                              @SchDt,        
                              @Status,        
                              @EnrouteTime,        
                              @Onsite,        
                              @Complete,        
                              @Category,        
                              @Unit,        
                              @Reason,        
                              @EST,        
                              @custID,        
                              @LocID,        
                              (SELECT id        
                               FROM   tblWork   WITH (NOLOCK)       
                               WHERE  fDesc = @Worker),        
                              @Ltype,        
                              0,        
                              @Who,        
                              @Type,        
                              Newid(),        
                              @Recommendation,--@Remarks,        
                              @Level,        
                              @job,        
                              @Custom1,        
                              @Custom2,        
                              @Custom3,        
                              @custom4,        
                              @custom5,        
                              @Custom6,        
                              @Custom7,        
                              @WorkOrder,        
                              @Nature,        
                              @fby,        
                              @QBServiceItem,        
                              @QBPayrollItem,        
                              @Customtick1,        
                              @Customtick2,        
                              @Customtick3,        
                              @Customtick4,        
                              @Customtick5,        
                              @RecurringDate,   
         @Charge      
         )        
            END        
      END        
    ELSE IF( @Status = 4 )        
      BEGIN        
          IF NOT EXISTS(SELECT 1        
                        FROM   TicketD  WITH (NOLOCK)        
                        WHERE  ID = @TicketID)        
            BEGIN        
             
                IF( @job = 0 )        
                  BEGIN  ---- IF( @job = 0 )        
                      DECLARE @projremark   VARCHAR(75),        
                              @projname     VARCHAR(75),        
                              --@templateitems tblTypeProjectItem,        
                              @bomitems     TBLTYPEBOMITEM,        
                              @MilestonItem TBLTYPEMILESTONEITEM,        
                              @servicetype  VARCHAR(15),        
                              @InvExp       INT,        
                              @InvServ      INT,        
                              @WageS        INT,        
                              @GLInt        INT,        
                              @Post         TINYINT,        
                              @Charges      TINYINT,        
                              @JobClose     TINYINT,        
                              @fInt         TINYINT,        
                              @types        INT        
        
                      SELECT @projremark = Remarks,        
                             @projname = fDesc,        
                             @servicetype = CType,        
                             @InvExp = InvExp,        
                             @InvServ = InvServ,        
                             @Wages = Wage,        
                             @GLInt = GLInt,   
                             @Post = Post,        
                             @Charges =  case @Charges when 1 then @Charges else  Charge end ,         
                             @JobClose = JobClose,        
                             @fInt = fInt,        
                             @types = [Type]        
                      FROM   JobT   WITH (NOLOCK)       
                      WHERE  ID = @ProjectTemplate        
        
                      SET @projremark += '-Added from ticket # '        
                                         + CONVERT(VARCHAR(50), @TicketID)        
                           
          SET @projname += '-Ticket # '   + CONVERT(VARCHAR(50), @TicketID)        
        
                              
                      INSERT INTO @bomitems        
                                  (JobT,        
                                   Job,        
                                   JobTItemID,        
                                   fDesc,        
                                   Code,        
                                   Line,        
  BType,        
                                   QtyReq,        
                                   UM,        
                                   BudgetUnit,        
                                   BudgetExt,        
                                   LabItem,        
                                   MatItem,        
                                   MatMod,        
                                   LabMod,        
                                   LabExt,        
                                   LabRate,        
                                   LabHours,        
                                   SDate,        
                                   VendorId,OrderNo,GroupID)        
                      SELECT ji.JobT,        
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
                             ji.Modifier,--b.MatMod        
                             ji.ETCMod,-- b.LabMod        
                             ji.ETC,-- b.LabExt        
                             b.LabRate,        
                             ji.BHours,--b.LabHours        
                             b.SDate,        
                             b.Vendor,        
        Line,        
        GroupID        
                      --ji.Actual ,         
                      --ji.[Percent]        
                      FROM   BOM b  WITH (NOLOCK)        
                             INNER JOIN jobtitem ji  WITH (NOLOCK)        
                                     ON ji.ID = b.JobTItemID        
                      WHERE  ji.JobT = @ProjectTemplate        
                             AND ( ji.job = 0        
                                    OR ji.job IS NULL )        
        
                      INSERT INTO @MilestonItem        
                      SELECT [JobT],        
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
                            Line,GroupID,    
                            m.Quantity,
                            m.Price,
                            m.ChangeOrder
                      FROM   Milestone m  WITH (NOLOCK)        
                             INNER JOIN jobtitem ji  WITH (NOLOCK)        
                                     ON ji.ID = m.JobTItemID        
                      WHERE  ji.JobT = @ProjectTemplate        
                             AND ( ji.job = 0        
                                    OR ji.job IS NULL )        
        
                        DECLARE @UpdatedByUserId INT =0;   
                     SELECT TOP 1 @UpdatedByUserId = ID FROM tblUser  WITH (NOLOCK)  WHERE fUser = @LastUpdatedBy  
  
                      EXEC @job = Spaddproject        
                        @job =0,        
                        @owner=NULL,        
                        @loc=@LocID,        
                        @fdesc=@projname,        
                        @status=0,        
                        @type=@types,        
                        @Remarks=@projremark,        
                        @ctype =@servicetype,        
                        @ProjCreationDate=@ProjDate,        
                        @PO =NULL,        
                        @SO =NULL,        
                        @Certified = 0,        
                        @Custom1 =NULL,        
                        @Custom2 =NULL,        
                        @Custom3 =NULL,        
                        @Custom4 =NULL,        
                        @Custom5 =NULL,        
                        @template =@ProjectTemplate,        
                        @RolName=NULL,        
                        @city =NULL,        
                        @state =NULL,        
                        @zip =NULL,        
                        @country =NULL,        
                        @phone =NULL,        
                        @cellular =NULL,        
                        @fax =NULL,        
                        @contact =NULL,        
                        @email =NULL,        
                        @rolRemarks =NULL,        
                        @rolType =NULL,        
                        @InvExp =@InvExp,        
                        @InvServ =@InvServ,        
                        @Wage =@Wages,        
                        @GLInt =@GLInt,        
                        @jobtCType =NULL,        
                        @Post =@Post,        
                        @Charge =@Charges,        
                        @JobClose =@JobClose,        
                        @fInt =@fInt,            
                        @BomItem = @bomitems,        
                        @MilestonItem = @MilestonItem,  
                        @UpdatedByUserId = @UpdatedByUserId,  
                        @TargetHPermission=0  
                          
                  END  ---- IF( @job = 0 )        
               
                DECLARE @RegTrav NUMERIC(30, 2),        
                        @OTTrav  NUMERIC(30, 2),        
                        @NTTrav  NUMERIC(30, 2),        
                        @DTTrav  NUMERIC(30, 2),        
                        @CReg    NUMERIC(30, 2),        
                        @COT     NUMERIC(30, 2),        
                        @CDT     NUMERIC(30, 2),        
                        @CNT     NUMERIC(30, 2),        
                        @CTT     NUMERIC(30, 2)        
        
                SELECT @CReg = [CReg],        
                       @COT = [COT],        
                       @CDT = [CDT],        
                       @CNT = [CNT],        
                       @CTT = [CTT]        
                FROM   PRWageItem pr     WITH (NOLOCK)     
                WHERE  pr.Wage = @Wage        
                       AND pr.Emp = (SELECT id        
                                     FROM   emp e   WITH (NOLOCK)       
                                     WHERE  e.fWork = (SELECT ID        
                                                       FROM   tblWork  WITH (NOLOCK)        
                                  WHERE  fDesc = @Worker))        
        
                IF( @JobCode IS NULL  OR @JobCode = '' )        
                  BEGIN        
                      IF( @job > 0 )        
                        BEGIN        
                            SELECT TOP 1 @JobCode = CONVERT(VARCHAR, j.Line) + ':' + j.Code + ':'        
                                                    + j.fDesc        
                            FROM   jobtitem j  WITH (NOLOCK)        
                                   INNER JOIN bom b        
                                           ON b.JobtItemId = j.ID        
                                   INNER JOIN BOMT        
                                           ON BOMT.ID = b.Type        
                            WHERE  j.job = @job        
                                   AND BOMT.Type = 'labor'        
        
                            IF( @JobCode IS NULL ) ------If Labour item not Exists        
                              BEGIN        
                                  SELECT TOP 1 @JobCode = CONVERT(VARCHAR, j.Line) + ':' + j.Code + ':'        
                                                          + j.fDesc        
                                  FROM   jobtitem j  WITH (NOLOCK)        
                            INNER JOIN bom b        
                                                 ON b.JobtItemId = j.ID        
                                         INNER JOIN BOMT        
                                                 ON BOMT.ID = b.Type        
                                  WHERE  j.job = @job        
                              END        
                        END        
                  END        
        
                --if(@JobCode IS NULL and @Status = 4)         
                --BEGIN               
                --  RAISERROR ('No labor type item exist for the project. Please add a labor type item to the project to proceed. ',16,1)         
                --  ROLLBACK TRANSACTION        
                --  return         
                --END         
        
                INSERT INTO TicketD        
                            (ID,        
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
      Who,        
                             Type,        
                             Status,        
                             Elev,        
                             BRemarks,        
                             Level,        
                             Custom1,        
                             Custom2,        
                             Custom3,        
                             Custom4,        
                             Custom5,        
                             Custom6,        
                             Custom7,        
                             WorkOrder,        
                             WorkComplete,        
                             OtherE,        
                             Toll,        
                             Zone,        
                             SMile,        
                             EMile,        
                             Internet,        
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
                             Job,        
                             JobCode,        
                             Phase,        
                             WageC,        
                             fBy,        
                             JobItemDesc,        
                             break_time,        
                             Comments,        
                             PartsUsed,         
        Recurring         
         )        
                VALUES      ( @TicketID,        
                              @CallDt,        
                              @SchDt,        
                              @EnrouteTime,        
                              @Onsite,        
                              @Complete,        
                              @Category,        
                              @Reason,        
                              @EST,        
                              (SELECT ID        
                               FROM   tblWork   WITH (NOLOCK)       
                               WHERE  fDesc = @Worker),        
                              @LocID,        
                              @complDesc,        
                              @Reg,        
                              @OT,        
                @NT,        
                              @TT,        
                              @DT,        
                              @Total,        
                              --CASE        
                              --  WHEN ( Isnull(@Invoice, 0) = 0 ) THEN @Charge        
                              --  ELSE 0        
                              --END,        
                              CASE        
                                WHEN ( @Invoice = '' ) THEN @Charge        
                                ELSE 0        
                              END,        
                              @Review,        
                              @Who,        
                              @Type,        
                              0,        
                              @Unit,        
                              @Recommendation,--@Remarks,        
                              @Level,        
                              @Custom1,        
                              @Custom2,        
                              @Custom3,        
                              @Custom4,        
                              @Custom5,        
                              @Custom6,        
                              @Custom7,        
                              @WorkOrder,        
                              @WorkComplete,        
                              @MiscExp,        
                              @TollExp,        
                              @ZoneExp,        
                              @MileStart,        
                              @MileEnd,        
                              @Internet,        
    @Invoice,        
                              Getdate(),        
                              @TransferTime,        
                              @QBServiceItem,        
                              @QBPayrollItem,        
                              @Cell,        
                              @Customtick1,        
                              @Customtick2,        
                              @Customtick3,        
                              @Customtick4,        
                              @Customtick5,        
                              @job,        
                              (SELECT items        
                               FROM   dbo.Idsplit(@JobCode, ':')        
              WHERE  row = 2),        
                              (SELECT items        
                               FROM   dbo.Idsplit(@JobCode, ':')        
                               WHERE  row = 1),        
                              @wage,        
                              @fby,        
                              (SELECT items        
                               FROM   dbo.Idsplit(@JobCode, ':')        
                               WHERE  row = 3),        
                              @BT,        
                              @Comments,        
                              @PartsUsed,        
         @RecurringDate         
          )        
        
                IF @@ERROR <> 0        
                   AND @@TRANCOUNT > 0        
                  BEGIN        
                      RAISERROR ('Error Occured',16,1)        
        
                      --ROLLBACK TRANSACTION        
        
                      RETURN        
                  END        
        
    if(@job > 0)        
        
    BEGIN------------        
                DECLARE @hourlyrate NUMERIC(30, 2)        
        
                SELECT @hourlyrate = Isnull(hourlyrate, 0)        
                FROM   tblWork   WITH (NOLOCK)       
                WHERE  fDesc = @Worker        
        
                DELETE FROM JobI         
                WHERE  Ref = @TicketID        
                       AND TransID = -@TicketID --and Job =@job         
        
                INSERT INTO JobI        
                            ([Job],        
                             [Phase],        
                             [fDate],        
                             [Ref],        
                             [fDesc],        
                             [Amount],        
                             [TransID],        
                             [Type],        
                      [UseTax],        
                             Labor)        
                VALUES      (@job,        
                             (SELECT items        
                              FROM   dbo.Idsplit(@JobCode, ':')        
                              WHERE  row = 1),        
                             @SchDt,        
                             @TicketID,        
                             'Labor/Time Spent on Ticket - ' + @Worker,        
                             CASE (SELECT Isnull(JobCostLabor, 0)                                     FROM   Control  WITH (NOLOCK) )        
                               WHEN 0 THEN ( ( Isnull(@Reg, 0) + Isnull(@RegTrav, 0) ) * Isnull(@hourlyrate, 0) + ( Isnull(@OT, 0) + Isnull(@OTTrav, 0) ) * ( 1.5 * Isnull(@hourlyrate, 0) )       
          + ( Isnull(@NT, 0) + Isnull(@NTTrav, 0) ) * ( 1.7 * Isnull(@hourlyrate, 0) )      
          + ( Isnull(@DT, 0) + Isnull(@DTTrav, 0) ) * ( 2 * Isnull(@hourlyrate, 0) )       
          + Isnull(@TT, 0) * Isnull(@hourlyrate, 0) )         
          WHEN 1 THEN ( ( Isnull(@Reg, 0) + Isnull(@RegTrav, 0) ) * Isnull(@CReg, 0)       
          + ( Isnull(@OT, 0) + Isnull(@OTTrav, 0) ) * ( Isnull(@COT, 0) ) + ( Isnull(@NT, 0) + Isnull(@NTTrav, 0) ) * ( Isnull(@CNT, 0) )       
          + ( Isnull(@DT, 0) + Isnull(@DTTrav, 0) ) * ( Isnull(@CDT, 0) ) + Isnull(@TT, 0) * Isnull(@CTT, 0) )        
                             END,        
                             -@TicketID,        
                             1,        
                             0,        
                             1 )        
        
                INSERT INTO JobI        
                            ([Job],        
                             [Phase],        
                             [fDate],        
             [Ref],        
                             [fDesc],        
                             [Amount],        
                             [TransID],        
                             [Type],        
                             [UseTax],        
                             Labor)        
                VALUES      (@job,        
                             (SELECT items        
                              FROM   dbo.Idsplit(@JobCode, ':')        
                              WHERE  row = 1),        
                             @SchDt,        
                             @TicketID,        
                             'Mileage on Ticket',        
                             ( Isnull(@MileEnd, 0) - Isnull(@MileStart, 0) ) * Isnull((SELECT MileageRate        
                                                                                       FROM   Emp  WITH (NOLOCK)        
                                                                                       WHERE  CallSign = @Worker), 0),        
                             -@TicketID,        
                             1,        
                             0,        
                             0 )        
        
                INSERT INTO JobI        
                            ([Job],        
                             [Phase],        
                             [fDate],        
                             [Ref],        
                             [fDesc],        
                             [Amount],        
                             [TransID],        
                             [Type],        
                             [UseTax],        
                             Labor)        
                VALUES      (@job,        
                             (SELECT items        
                              FROM   dbo.Idsplit(@JobCode, ':')        
                              WHERE  row = 1),        
                             @SchDt,        
                             @TicketID,        
                             'Expenses on Ticket',        
                             Isnull(@TollExp, 0) + Isnull(@ZoneExp, 0)        
                             + Isnull(@MiscExp, 0),        
                             -@TicketID,        
                             1,        
                             0,        
                             0 )        
        
                SET @Phase = (SELECT items        
                              FROM   dbo.Idsplit(@JobCode, ':')        
                              WHERE  row = 1)        
        
                EXEC Spupdatejoblaborexp        
                  @job,        
                  @Phase        
        
        
      END----------        
        
        
                IF( Rtrim(Ltrim(@Recommendation)) <> '' )        
                  BEGIN        
                      IF NOT EXISTS(SELECT 1        
                                    FROM   Lead   WITH (NOLOCK)       
                                    WHERE  TicketID = @TicketID)        
                        BEGIN        
                            DECLARE @oppname VARCHAR(75)        
        
                            SET @oppname ='Ticket# ' + CONVERT(VARCHAR(67), @TicketID)        
        
                            IF ( @custID IS NULL )        
                              BEGIN        
           SELECT @Rol = Rol        
                                  FROM   Prospect  WITH (NOLOCK)        
                                  WHERE  ID = @LocID        
                              END        
                            ELSE        
                              BEGIN        
                                  SELECT @Rol = Rol        
                                  FROM   Loc  WITH (NOLOCK)         
                                  WHERE  Loc = @LocID        
                              END        
        
                            DECLARE @defaultuser VARCHAR(50)        
        
                            SELECT TOP 1 @defaultuser = fuser        
                            FROM   tbluser  WITH (NOLOCK)        
                            WHERE  DefaultWorker = 1        
        
                            DECLARE @oppremarks VARCHAR(500)        
        
                            IF( (SELECT Count(1)        
                                 FROM   @Equipments) = 1 )        
                              BEGIN        
                                  SET @oppremarks = @Recommendation + Char(13) + Char(10)      
                                                    + 'Equipment : '        
                                                    + (SELECT TOP 1 Isnull((SELECT TOP 1 Isnull(unit, '') + ', ' + Isnull(fDesc, '')        
                                                                            FROM   elev   WITH (NOLOCK)       
                                                                            WHERE  id = elev_id), '')        
                                                       FROM   @Equipments)        
                              END        
                            ELSE        
                              BEGIN        
                                  SET @oppremarks = @Recommendation        
                              END        
        
                            EXEC Spaddopportunity        
                              @ID = 0,        
                              @fdesc= @oppname,        
                              @rol=@Rol,        
                              @Probability= 3,        
                              @Status= 1,        
                              @Remarks= @oppremarks,        
                              @closedate= @SchDt,        
                              @Mode = 0,        
                              @owner= 0,        
                              @NextStep= '',        
                              @desc= '',        
                              @Source= '',        
                              @Amount= 0,        
                              @Fuser= @defaultuser,        
                              @AssignedToID= 0,        
                              @UpdateUser=@LastUpdatedBy,        
                              @closed=0,        
                              @TicketID= @TicketID,        
                              @BusinessType= @BT,        
                              @Product = '',        
                              @OpportunityStageID= 0,        
                              @CompanyName =@CustName,        
                              @IsSendMailToSalesPer=1,  
         @Department=null  
        
                            IF @@ERROR <> 0        
                               AND @@TRANCOUNT > 0        
                BEGIN        
                                  RAISERROR ('Error Occured',16,1)        
        
                                  --ROLLBACK TRANSACTION        
        
                                  RETURN        
                              END        
                        END        
                  END        
        
                IF( @Review = 1 )        
                  BEGIN        
                      UPDATE Elev        
                      SET    Last = @SchDt        
                      WHERE  ID = @Unit        
                             AND Isnull(Last, CONVERT(DATETIME, '01/01/1800')) < @SchDt        
                  END        
            END        
      END        
        
    IF @@ERROR <> 0        
       AND @@TRANCOUNT > 0        
      BEGIN        
          RAISERROR ('Error Occured',16,1)        
        
         -- ROLLBACK TRANSACTION        
        
          RETURN        
      END        
        
    DELETE FROM multiple_equipments        
    WHERE  ticket_id = @TicketID        
        
    INSERT INTO multiple_equipments        
                (ticket_id,        
                 elev_id,        
                 labor_percentage)        
    SELECT @TicketID,        
           elev_id,        
           labor_percentage        
    FROM   @Equipments        
        
    IF( @UpdateTasks = 1 )        
     BEGIN        
          IF( @job IS NOT NULL )        
            BEGIN        
                IF( @job <> 0 )        
                  BEGIN        
                      DELETE FROM Ticket_Task_Codes        
                      WHERE  job = @job        
        
                      INSERT INTO Ticket_Task_Codes        
                                  (task_code,        
                                   Type,        
                        job,        
                                   Category,        
                                   username,        
                                   dateupdated,        
                                   ticket_id,        
                                   default_code)        
                      SELECT task_code,        
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
                      FROM   @TaskCodes        
                  END        
            END        
      END        
        
    /* update location address */        
    IF( @custID IS NOT NULL        
        AND @IsRecurring = 0 )        
      BEGIN        
          UPDATE Loc        
          SET    Address = @LocAdd,        
                 City = @City,        
                 State = @State,        
                 Zip = @Zip,        
                 Remarks = @Remarks,        
                 DispAlert = @DispAlert,        
                 Credit = @CreditHold,        
                 CreditReason = @CreditReason,        
                 Route = @DefaultRoute,        
     Zone=@Zone        
          WHERE  Loc = @locID        
        
          UPDATE Job        
          SET    Custom20 = @DefaultRoute        
          WHERE  Loc = @locID        
        
          UPDATE Rol        
          SET    Phone = @Phone,        
                 Cellular = @Cell,        
                 LastUpdateDate = Getdate(),        
                 Contact = @Contact,        
                 Lat = @lat,        
                 Lng = @lng        
          WHERE  ID = (SELECT TOP 1 Rol        
                       FROM   Loc        
                       WHERE  Loc = @LocID)        
      END        
        
    IF @@ERROR <> 0        
       AND @@TRANCOUNT > 0        
      BEGIN        
          RAISERROR ('Error Occured',16,1)        
        
          --ROLLBACK TRANSACTION        
        
          RETURN        
      END        
        
    IF( @custID IS NULL        
        AND @IsRecurring = 0        
        AND @prospectcreate = 0 )        
      BEGIN        
          UPDATE Prospect        
          SET    LastUpdateDate = Getdate(),        
                 LastUpdatedBy = @LastUpdatedBy        
          WHERE  ID = @LocID        
        
          UPDATE Rol        
          SET    Address = @LocAdd,        
                 City = @City,        
                 State = @State,        
                 Zip = @Zip,        
                 Remarks = @Remarks,        
                 Phone = @Phone,        
                 Cellular = @Cell,        
                 LastUpdateDate = Getdate(),        
                 Contact = @Contact,        
                 Lat = @lat,        
                 Lng = @lng        
          WHERE  ID = (SELECT TOP 1 Rol        
                       FROM   Prospect   WITH (NOLOCK)       
                       WHERE  ID = @LocID)        
      END        
        
    IF @@ERROR <> 0        
       AND @@TRANCOUNT > 0        
      BEGIN        
          RAISERROR ('Error Occured',16,1)        
        
          --ROLLBACK TRANSACTION        
        
          RETURN        
      END        
        
    /* insert signature image */        
    IF( @sign IS NOT NULL )        
      BEGIN        
          EXEC Spinsertticketsign        
            @TicketID,        
            @sign        
      END        
        
        
---  ////////// INVENTORTY  $$$$$$$$$$$$$        
        
         
 exec [spAddTicketINVInfo]         
    @TicketID =@TicketID ,        
    @job =@job,        
    @dtTicketINV=@dtTicketINV,  
    @screen=@screen,  
    @mode = 'Add'  
  
  
IF ((SELECT COUNT(1) FROM @dtTicketINV)>0 AND (SELECT COUNT(*) FROM TicketI  WITH (NOLOCK)  WHERE Ticket=@TicketID AND charge=1)>0)  
BEGIN  
 UPDATE TicketD  
 SET Charge=1  
 WHERE ID=@TicketID  
END  
                 
        
----/////// END INVENTORTY $$$$$$$$$$$$$$        
        
    IF @@ERROR <> 0        
       AND @@TRANCOUNT > 0        
      BEGIN        
          RAISERROR ('Error Occured',16,1)        
        
          --ROLLBACK TRANSACTION        
        
          RETURN        
      END        
        
  /********Start Logs************/        
 if(@CustName is not null And @CustName != '')        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Customer Name','',@CustName        
 END         
 if(@LocID is not null)        
 Begin          
  Declare @LocName varchar(150)        
  Select @LocName = tag from loc  WITH (NOLOCK)  where loc = (Select LID from TicketO  WITH (NOLOCK)  where ID =@TicketID)        
  exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Location Name','',@LocName        
 END        
 if(@WorkOrder is not null And @WorkOrder !='')        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Work Order','',@WorkOrder        
 END        
 if(@Invoice is not null And @Invoice !='')       
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Invoice','',@Invoice        
 END        
 if(@LocAdd is not null And @LocAdd !='')        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Location Address','',@LocAdd        
 END        
 if(@City is not null And @City != '')        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'City','',@City        
 END        
 if(@State is not null And @State != '')        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'State','',@State        
 END        
 if(@Zip is not null And @Zip != '')        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Zip/Postal Code', '', @Zip        
 END        
 if(@Contact is not null And @Contact != '')        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Main Contact','',@Contact        
 END        
 if(@Who is not null And @Who != '')        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Caller','',@Who        
 END        
 if(@Phone is not null And @Phone != '')        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Contact Phone','',@Phone        
 END        
 if(@Cell is not null And @Cell != '')        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Caller Phone','',@Cell        
 END        
 if(@fBy is not null And @fBy != '')        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Entered By','',@fBy        
 END         
 if(@CallDt is not null And @CallDt != '')        
 Begin          
  Declare @Calldate nvarchar(150)        
  SELECT @Calldate = convert(varchar, @CallDt, 101)        
  exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Date Called In','',@Calldate        
 END        
 if(@CallDt is not null And @CallDt != '')        
 Begin          
  Declare @Calltime nvarchar(150)        
  SELECT @Calltime = FORMAT(@CallDt, 'hh:mm tt')        
  exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Called In Time','',@Calltime        
 END        
 if(@DefaultRoute is not null And @DefaultRoute != 0)        
 Begin          
 Declare @DefaultRouteVal varchar(50)        
 Select @DefaultRouteVal =  Name from Route  WITH (NOLOCK)  where ID= @DefaultRoute        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Default Worker','',@DefaultRouteVal        
 END        
 if(@Category is not null And @Category != '')        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Category','',@Category        
 END        
 if(@Reason is not null)        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Reason for service','',@Reason        
 END        
 if(@Status is not null)        
 Begin          
 Declare @StatusVal varchar(50)        
 Select @StatusVal = Case @Status WHEN 0 THEN 'Un-Assigned' WHEN 1 THEN 'Assigned' WHEN 2 THEN 'Enroute' WHEN 3 THEN 'Onsite' WHEN 4 THEN 'Completed' WHEN 5 THEN 'Hold' END         
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Status','',@StatusVal        
 END        
 if(@SchDt is not null And @SchDt != '')        
 Begin          
  Declare @SchDtdate nvarchar(150)        
  SELECT @SchDtdate = convert(varchar, @SchDt, 101)        
  exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Date Scheduled','',@SchDtdate        
 END        
 if(@SchDt is not null And @SchDt != '')        
 Begin          
  Declare @SchDttime nvarchar(150)        
  SELECT @SchDttime = FORMAT(@SchDt, 'hh:mm tt')        
  exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Time','',@SchDttime        
 END        
 if(@EST is not null)        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Estimate','',@EST        
 END        
 if(@Worker is not null And @Worker != '')        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Assigned Worker','',@Worker        
 END        
 if(@DispAlert is not null)        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Dispatch Alert','',@DispAlert        
 END        
 if(@CreditHold is not null)        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Credit Hold','',@CreditHold        
 END        
 if(@CreditReason is not null)        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Reason','',@CreditReason        
 END        
 if(@remarks is not null)        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Remarks','',@remarks        
 END        
 if(@WorkComplete is not null)        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Work Complete','',@WorkComplete        
 END        
 if(@Review is not null)        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Review','',@Review        
 END        
 if(@Charge is not null)        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Chargeable','',@Charge        
 END        
 if(@TransferTime is not null)        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Timesheet','',@TransferTime        
 END        
 if(@Internet is not null)        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Internet','',@Internet        
 END        
 if(@complDesc is not null)        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Work Complete Desc','',@complDesc        
 END        
 if(@Reg is not null)        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'RT','', @Reg        
 END        
 if(@OT is not null)        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'OT','',@OT        
 END        
 if(@EnrouteTime is not null And @EnrouteTime != '')        
 Begin          
  Declare @Enroute nvarchar(150)        
  SELECT @Enroute = FORMAT(@EnrouteTime, 'hh:mm tt')        
  exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'En Route','',@Enroute        
 END        
 if(@NT is not null)        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'NT','',@NT        
 END        
 if(@DT is not null)        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'DT','',@DT        
 END        
 if(@Onsite is not null And @Onsite != '')        
 Begin          
  Declare @Onsitetime nvarchar(150)        
  SELECT @Onsitetime = FORMAT(@Onsite, 'hh:mm tt')        
  exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'On Site','',@Onsitetime        
 END        
 if(@TT is not null)        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'TT','',@TT        
 END        
 if(@BT is not null)        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'BT','',@BT        
 END        
 if(@Complete is not null And @Complete != '')        
 Begin          
  Declare @Completetime nvarchar(150)        
  SELECT @Completetime = FORMAT(@Complete, 'hh:mm tt')        
  exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Completed','',@Completetime        
 END        
 if(@job is not null And @job != 0)        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Project#','',@job        
 END        
 if(@JobCode is not null And @JobCode != '')        
 Begin          
 Declare @JobCodeVal varchar(10)        
 Select @JobCodeVal =  (select items from dbo.IDSplit(@JobCode,':') where row=2)        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Project Type','',@JobCodeVal        
 END        
 if(@Type is not null)        
 Begin          
 Declare @DeptType varchar(50)        
 Select @DeptType = Type from jobtype Where ID = @Type        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Department','',@DeptType        
 END        
 if(@wage is not null And @wage != 0)        
 Begin          
 Declare @wageVal varchar(50)        
 Select @wageVal = fdesc From PRWage  WITH (NOLOCK)  where ID = @wage        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Wage','',@wageVal        
 END        
 if(@QBServiceItem is not null And @QBServiceItem != '')        
 Begin          
 Declare @QBServiceItemVal varchar(100)        
 Select @QBServiceItemVal = Name From Inv  WITH (NOLOCK)  Where QBInvID = @QBServiceItem        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Service Item','',@QBServiceItemVal        
 END        
 if(@QBPayrollItem is not null And @QBPayrollItem != '')        
 Begin          
 Declare @QBPayrollItemVal varchar(100)        
 Select @QBPayrollItemVal = fdesc From prwage  WITH (NOLOCK)  Where QBwageID = @QBPayrollItem        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Payroll Item','',@QBPayrollItemVal        
 END        
 if(@Recommendation is not null And @Recommendation != '')        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Recommendation','',@Recommendation        
 END        
 if(@PartsUsed is not null And @PartsUsed != '')        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Parts Used','',@PartsUsed        
 END        
 if(@Comments is not null And @Comments != '')        
 Begin        
exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Comments','',@Comments        
 END        
 if(@MiscExp is not null)        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Miscellaneous','',@MiscExp        
 END        
 if(@MileStart is not null)        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Starting Mileage','',@MileStart        
 END        
 if(@ZoneExp is not null)        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Zone','',@ZoneExp        
 END        
 if(@MileEnd is not null)        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Ending Mileage','',@MileEnd        
 END        
 if(@TollExp is not null)        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Toll','',@TollExp        
 END        
 if(@Custom1 is not null And @Custom1 != '')        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Custom 1','',@Custom1        
 END        
 if(@Custom2 is not null And @Custom2 != '')        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Custom 2','',@Custom2        
 END        
 if(@Custom3 is not null And @Custom3 != '')        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Custom 3','',@Custom3        
 END        
 if(@Custom4 is not null And @Custom4 != '')        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Custom 4','',@Custom4        
 END        
 if(@Custom5 is not null And @Custom5 != '')        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Custom 5','',@Custom5        
 END        
 if(@Custom6 is not null)        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Custom 6','',@Custom6        
 END        
 if(@Custom7 is not null)        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Custom 7','',@Custom7        
 END        
 if(@Customtick1 is not null And @Customtick1 != '')        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Ticket Custom 1','',@Customtick1        
 END        
 if(@Customtick2 is not null And @Customtick2 != '')        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Ticket Custom 2','',@Customtick2        
 END        
 if(@Customtick5 is not null)        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Ticket Custom 3','',@Customtick5        
 END         
 if(@Customtick3 is not null)        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Ticket Checkbox 1','',@Customtick3        
 END        
 if(@Customtick4 is not null)        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Ticket Checkbox 2','',@Customtick4        
 END        
 Declare @EquipmentJob VARCHAR(1000)        
 Select @EquipmentJob  =  STUFF((SELECT ',  ' + e.Unit From Elev e  WITH (NOLOCK)  inner Join multiple_equipments ej on e.ID = ej.elev_id Where ej.ticket_id = @TicketID FOR XML PATH('')), 1, 1, '')        
 IF (@EquipmentJob is not NUll)        
 Begin        
 exec log2_insert @LastUpdatedBy,'Ticket',@TicketID,'Equipment','',@EquipmentJob        
 END        
  /********End Logs************/        
        
    SELECT @TicketID        
        
    --COMMIT TRANSACTION   
	