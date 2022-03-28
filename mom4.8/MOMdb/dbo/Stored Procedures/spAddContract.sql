CREATE PROCEDURE [dbo].[spAddContract]
    @loc              INT,  
    @owner            INT,  
    @date             DATETIME,  
    @Status           INT,  
    @Creditcard       INT,  
    @Remarks          TEXT,  
    @BStart           DATETIME,  
    @Bcycle           INT,  
    @BAmt             NUMERIC(30, 2),  
    @SStart           DATETIME,  
    @Cycle            INT,  
    @SWE              INT,  
    @Stime            DATETIME,  
    @Sday             INT,  
    @SDate            INT,  
    @ElevJobData      AS [dbo].[TBLTYPEJOINELEVJOB] READONLY,  
    @Route            VARCHAR(75),  
    @hours            NUMERIC(30, 2),  
    @fdesc            VARCHAR(75),  
    @CType            VARCHAR(15),  
    @ExpirationDate   DATETIME,  
    @ExpirationFreq   SMALLINT,  
    @Expiration       SMALLINT,  
    @ContractBill     SMALLINT,  
    @CustomerBill     SMALLINT,  
    @Central          INT,  
    @Chart            INT,  
    @JobT             INT,  
    @EscalationType   SMALLINT,  
    @EscalationCycle  SMALLINT,  
    @EscalationFactor NUMERIC(30, 2),  
    @EscalationLast   DATETIME,  
    @CustomItems      AS TBLTYPECUSTOMTABITEM READONLY,  
    @BillRate         NUMERIC(30, 2),  
    @RateOT           NUMERIC(30, 2),  
    @RateNT           NUMERIC(30, 2),  
    @RateDT           NUMERIC(30, 2),  
    @RateTravel       NUMERIC(30, 2),  
    @Mileage          NUMERIC(30, 2),  
    @PO               VARCHAR(25),  
    @SPHandle         SMALLINT=0,  
    @SPRemarks        TEXT='',  
    @IsRenewalNotes   SMALLINT=0,  
    @RenewalNotes     TEXT='',  
    @Detail           SMALLINT,  
    @taskcategory varchar(15) = '',  
    @UpdatedBy varchar(100),
    @EstimateId int ,
	@ContractLength int=null
AS  

 
IF(isnull(@ContractLength,0) < = 0)
BEGIN 
IF(@Expiration=1)
BEGIN
SELECT @ContractLength=
                CASE isnull(@Bcycle,0)  
			    WHEN 0 THEN   1	          WHEN 1 THEN 1
	            WHEN 2 THEN   3	          WHEN 3 THEN 4
	            WHEN 4 THEN   6           WHEN 5 THEN 12
	            WHEN 6 THEN   999	      WHEN 7 THEN 36
	            WHEN 8 THEN   60	      WHEN 9 THEN 24
				END
 
END
ELSE SET @ContractLength=999
END
  
 

    DECLARE @Job INT  
    DECLARE @tblCustomFieldsId INT  
    DECLARE @tblTabID INT  
    DECLARE @Label VARCHAR(50)  
    DECLARE @TabLine SMALLINT  
    DECLARE @Value VARCHAR(50)  
    DECLARE @Format VARCHAR(50)  
    DECLARE @ProjDate as DATETIME = GETDATE();  

 	IF NOT EXISTS (SELECT TOP 1  ID   FROM   JobT    WHERE  Type = 0 ) 
	BEGIN	
	      RAISERROR ('Please set up default project template .',16,1)  
	END
  
    BEGIN TRANSACTION  
  
    DECLARE @ProjectTemplate INT,  
            @projremark      VARCHAR(75),  
            @projname        VARCHAR(75),  
            --@templateitems tblTypeProjectItem,  
            @bomitems        TBLTYPEBOMITEM,  
            @MilestonItem    TBLTYPEMILESTONEITEM,  
            @servicetype     VARCHAR(15),  
            @InvExp          INT,  
            @InvServ         INT,  
            @WageS           INT,  
            @GLInt           INT,  
            @Post            TINYINT,  
            @Charges         TINYINT,  
            @JobClose        TINYINT,  
            @fInt            TINYINT,  
            @types           INT  
  
    SELECT TOP 1 @ProjectTemplate = ID   FROM   JobT   WHERE  Type = 0  
  
    SELECT @projremark = Remarks + CONVERT(VARCHAR(MAX), @Remarks),  
           @projname = fDesc,  
           @servicetype = CASE  
                            WHEN @CType = ''  
                            THEN  
                              CType  
                            ELSE  
                              @CType  
                          END,  
           @InvExp = InvExp,  
           @InvServ = InvServ,  
           @Wages = Wage,  
           @GLInt = GLInt,  
           @Post = Post,  
           @Charges = Charge,  
           @JobClose = JobClose,  
           @fInt = fInt,  
           @types = [Type]  
    FROM   JobT  
    WHERE  ID = @ProjectTemplate  

	IF (isnull(@Wages,0)=0) 
	BEGIN

	      Declare @vPT varchar(1000)='Please set up the labor wage at the project template level.';

		  select @vPT=@vPT+ ' (' + fDesc + ' )' from JobT where ID=@ProjectTemplate
	
	      RAISERROR (@vPT,16,1)  
  
          ROLLBACK TRANSACTION  
  
          RETURN  
	END
  
    IF (@EstimateId is null OR @EstimateId = 0)
    BEGIN
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
                     VendorId,GroupID)  
        SELECT JI.JobT,  
               JI.Job,  
               B.JobTItemID,  
               --ji.[Type] ,   
               fdesc,  
               JI.Code,  
               --ji.Budget,   
               Line,  
               B.[Type],  
               --b.Item,   
               B.QtyRequired,  
               UM,  
               --b.ScrapFactor ,   
               B.BudgetUnit,  
               B.BudgetExt,  
               B.LabItem,  
               B.MatItem,  
               JI.Modifier,--b.MatMod  
               JI.ETCMod,-- b.LabMod  
               JI.ETC,-- b.LabExt  
               B.LabRate,  
               JI.BHours,--b.LabHours  
               B.SDate,  
               B.Vendor,GroupID  
        --ji.Actual ,   
        --ji.[Percent]  
        FROM   BOM B  
               INNER JOIN jobtitem JI  
                       ON JI.ID = B.JobTItemID  
        WHERE  JI.JobT = @ProjectTemplate  
               AND ( JI.job = 0  
                      OR JI.job IS NULL )  
  
        INSERT INTO @MilestonItem  
        SELECT [JobT],  
               [Job],  
               M.[JobTItemID],  
               JI.[Type],  
               [fdesc],  
               JI.[Code],  
               [Line],  
               M.[MilestoneName],  
               [RequiredBy],  
               0,  
               [ProjAcquistDate],  
               [ActAcquistDate],  
               [Comments],  
               M.[Type],  
               [Amount],  
                NULL,  
                GroupId,
                m.Quantity,
                m.Price,
                m.ChangeOrder
        FROM   Milestone M  
               INNER JOIN jobtitem JI  
                       ON JI.ID = M.JobTItemID  
        WHERE  JI.JobT = @ProjectTemplate         AND ( JI.job = 0    OR JI.job IS NULL )  
    END
    ELSE
    BEGIN
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
                     VendorId,GroupID)  
        SELECT @ProjectTemplate,  
               0,  
               B.JobTItemID,  
               --ji.[Type] ,   
               EI.fDesc,  
               EI.Code,  
               --ji.Budget,   
               EI.Line,  
               B.[Type],  
               --b.Item,   
               ISNULL(EI.Quan, 0),  
               B.UM,  
               --b.ScrapFactor ,   
               ISNULL(EI.Price, 0),  
               ISNULL(EI.Cost, 0),  
               B.LabItem,  
               B.MatItem,  
               ISNULL(EI.MMod, 0),--b.MatMod  
               EI.LMod,-- b.LabMod  
               EI.Labor,-- b.LabExt  
               EI.Rate,  
               EI.Hours,--b.LabHours  
               B.SDate,  
               B.Vendor,
               e.GroupId
        --ji.Actual ,   
        --ji.[Percent]  
        FROM   BOM B  
               INNER JOIN EstimateI EI  ON B.EstimateIId = EI.ID AND EI.Type = 1
               INNER JOIN Estimate e ON e.Id = EI.Estimate
        WHERE  EI.Estimate = @EstimateId AND EI.Type = 1

        INSERT INTO @MilestonItem  
        SELECT @ProjectTemplate,  
               0,  
               M.[JobTItemID],  
               EstimateI.[Type],  
               EstimateI.[fdesc],  
               EstimateI.[Code],  
               EstimateI.[Line],  
               M.[MilestoneName],  
               M.[RequiredBy],  
               0,  
               [ProjAcquistDate],  
               [ActAcquistDate],  
               [Comments],  
               M.[Type],  
               ISNULL(EstimateI.Amount, 0),  
                NULL,  
                e.GroupId,
                m.Quantity,
                m.Price,
                m.ChangeOrder
        FROM EstimateI Left JOIN Milestone M ON EstimateI.ID = M.EstimateIId
        LEFT JOIN OrgDep ON M.Type = OrgDep.ID
        INNER JOIN Estimate e ON e.Id = EstimateI.Estimate
        WHERE EstimateI.Estimate = @EstimateId AND EstimateI.Type = 0   

        --
        UPDATE @MilestonItem
        SET Amount = @BAmt,
            Quantity = 1,
            Price = @BAmt
        WHERE Line = 1
        UPDATE @MilestonItem
        SET Amount = 0
            , Quantity = 0
            , Price = 0
        WHERE Line > 1
    END
    

    DECLARE @UpdatedByUserId INT =0; 

    SELECT TOP 1 @UpdatedByUserId = ID FROM tblUser WHERE fUser = @UpdatedBy


    EXEC @Job = spAddProject  
        @job =0,  
        @owner=NULL,  
        @loc=@loc,  
        @fdesc=@fdesc,  
        @status=@Status,  
        @type=0,  
        @Remarks= @projremark,  
        @ctype =@servicetype,  
        @ProjCreationDate= @ProjDate,  
        @PO =@PO,  
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
        @BillRate=@BillRate,  
        @RateOT=@RateOT,  
        @RateNT=@RateNT,  
        @RateDT=@RateDT,  
        @RateTravel=@RateTravel,  
        @Mileage=@Mileage,  
        @SPHandle =@SPHandle,  
        @SPRemarks =@SPRemarks,  
        @IsRenewalNotes =@IsRenewalNotes,  
        @RenewalNotes =@RenewalNotes,  
	    @taskcategory =@taskcategory,
	    @UpdatedByUserId=@UpdatedByUserId,
	    @TargetHPermission=0
     
  
    UPDATE job   SET   Custom20 = @Route,      
                    CreditCard =     @Creditcard,    SPHandle = @SPHandle,   
                    SRemarks =       @SPRemarks,   
                    IsRenewalNotes = @IsRenewalNotes,     
                    RenewalNotes =   @RenewalNotes  
    WHERE  ID =      @Job  

    -- Case convert estimate
    IF (@EstimateId is not null AND @EstimateId <> 0)
    BEGIN
        DECLARE @GroupID int;
        DECLARE @OpportunityID int;
        DECLARE @IsFinancialDataConvert int = 1;
        DECLARE @CurrStatus smallint
        SELECT @GroupID = IsNull(GroupId,0)
            , @OpportunityID = ISNULL(Opportunity,0) 
            , @CurrStatus = Status
        FROM Estimate Where ID = @EstimateId

		IF NOT EXISTS(SELECT 1 FROM tblEstimateConvertToProject Where OpportunityID=@OpportunityID and isnull(ProjectID,0) > 0 and EstimateID = @EstimateId)
        BEGIN
	        

	        DECLARE @CurrOppStatus smallint
	        DECLARE @CurrOppStatusName VARCHAR(100)
	        SELECT @CurrOppStatus = l.Status, @CurrOppStatusName = os.Name from OEStatus os INNER JOIN LEAD l ON l.ID = @OpportunityID AND os.ID = l.Status
            
            -- Adding Group from Estimate to project
		    IF NOT EXISTS (SELECT 1 FROM tblProjectGroup WHERE ProjectId = @Job AND GroupId = @GroupID)
		    BEGIN 
			    INSERT INTO tblProjectGroup VALUES (@Job,@GroupID)
		    END
	    
		    INSERT INTO	tblEstimateConvertToProject([ProjectID] ,[EstimateID],[OpportunityID],[IsFinancialDataConverted])
			    VALUES(@Job,@EstimateId,@OpportunityID,@IsFinancialDataConvert)

		    UPDATE Estimate SET Job=@Job, Status=5 WHERE ID=@EstimateId

            UPDATE Job SET FirstLinkedEst=@EstimateId WHERE ID=@Job

		    UPDATE Lead SET Status=5 WHERE ID =@OpportunityID

            UPDATE jt SET
                jt.EstConvertId = @estimateid,
                jt.EstConvertLine = CASE ei.Type WHEN 1 THEN ei.Line ELSE null END -- Update estimate line in case BOM items only
            FROM JobTItem jt 
			INNER JOIN EstimateI ei on ei.Line = jt.Line and jt.Type = ei.Type
			WHERE jt.Job = @Job and ei.Estimate = @estimateid


            /*-- Logs --*/
	        DECLARE  @SoldStatusName  varchar(50)
	        SELECT TOP 1 @SoldStatusName=Name FROM OEStatus WHERE ID = 5
	        -- Estimate Status
	        IF(@CurrStatus != 5)
	        BEGIN
		        DECLARE @CurrStatusName varchar(50)
		        SELECT TOP 1 @CurrStatusName=Name FROM OEStatus WHERE ID = @CurrStatus
		        EXEC log2_insert @UpdatedBy,'Estimate',@EstimateId,'Estimate Status',@CurrStatusName,@SoldStatusName
	        END

	        EXEC log2_insert @UpdatedBy,'Estimate',@EstimateId,'Project','',@Job

	        IF(@CurrOppStatus != 5)
	        BEGIN
		        EXEC log2_insert @UpdatedBy,'Opportunity',@OpportunityID,'Status',@CurrOppStatusName,@SoldStatusName
	        END
            /*-- End Logs --*/

            EXEC spConvertEstimateDocToProjectDoc @EstimateId, @job
        END
        ELSE
        BEGIN
            RAISERROR ('Estimate already converted to project!',16,1)  
  
            IF @@TRANCOUNT > 0  
                ROLLBACK TRANSACTION  
  
            RETURN  
        END

    END
    IF @@ERROR <> 0 
	BEGIN  
        RAISERROR ('Error Occured',16,1)  
  
        IF @@TRANCOUNT > 0  ROLLBACK TRANSACTION  
  
        RETURN  
    END  
  
    INSERT INTO Contract  
                (Job,  
                 BStart,  
                 BCycle,  
                 BAmt,  
                 SStart,  
                 SCycle,  
                 SWE,  
                 STime,  
                 SDay,  
                 SDate,  
                 Loc,  
                 Owner,  
                 Hours,  
                 Status,  
                 ExpirationDate,  
                 Frequencies,  
                 Expiration,  
                 Chart,  
                 BEscType,  
                 BEscCycle,  
                 BEscFact,  
                 EscLast,  
                Detail,BLenght)  
    VALUES      ( @Job,  
                  @BStart,  
                  @Bcycle,  
                  @BAmt,  
                  @SStart,  
                  @Cycle,  
                  @SWE,  
                  @Stime,  
                  @Sday,  
                  @SDate,  
                  @loc,  
                  @owner,  
                  @hours,  
                  @Status,  
                  CASE @Expiration  
                    WHEN 1  
                    THEN  
                      @ExpirationDate  
                    ELSE  
                      NULL  
                  END,  
                  CASE @Expiration  
                    WHEN 2  
                    THEN  
                      @ExpirationFreq  
                    ELSE  
                      NULL  
                  END,  
                  @Expiration,  
                  @Chart,  
                  @EscalationType,  
                  @EscalationCycle,  
                  @EscalationFactor,  
                  @EscalationLast,  
                @Detail  , @ContractLength)  
  
    INSERT INTO Contract_RecurringBilling VALUES(@Job,@BStart,1,NULL,NULL)  
   
    --------------------------->  
  
   UPDATE jc  set jc.BHours =   
        (  
        CASE c.SCycle   
            WHEN 0 THEN c.Hours --Monthly \n");  
            WHEN 1 THEN c.Hours / 2 --Bi-Monthly \n");  
            WHEN 2 THEN c.Hours / 3 --Quarterly \n");  
            WHEN 3 THEN c.Hours / 6 --Semi-Anually \n");  
            WHEN 4 THEN c.Hours / 12 --Anually \n");  
            WHEN 5 THEN (c.Hours * 4.3) --Weekly \n");  
            WHEN 6 THEN (c.Hours * (2.15))  --Bi-Weekly \n");  
            WHEN 7 THEN ( c.Hours / ( 2.9898 ) ) --Every 13 Weeks  \n");  
            WHEN 10 THEN c.Hours / 12*2 --Every 2 Years \n");  
            WHEN 8 THEN c.Hours / 12*3 --Every 3 Years \n");  
            WHEN 9 THEN c.Hours / 12*5 --Every 5 Years \n");  
            WHEN 11 THEN c.Hours / 12*7 --Every 7 Years \n");  
            WHEN 13 THEN (c.Hours * ( CASE c.SWE WHEN 1 THEN 30 ELSE   21.66 END) ) --Daily \n");  
            WHEN 14 THEN (c.Hours * (2) ) --Twice a Month \n");  
            WHEN 15 THEN (c.Hours / (4) ) --3 Times/Year \n"); 
            else jc.BHours     
        END  
        )    
    from  jobtitem jc   
    inner join CONTRACT c on c.job=jc.Job  
    WHERE  jc.Line= ( select  min(jcc.Line) from jobtitem jcc where  jcc.type=1 and jcc.fDesc='Labor' and jcc.Job=@Job )  and jc.type=1 and jc.fDesc='Labor' and jc.Job=@Job  
  
    UPDATE b set   b.LabRate =   
        ( CASE (SELECT Isnull(JobCostLabor, 0)  FROM   Control)  
            WHEN 1 THEN Isnull(PR.CReg, 0)  
            ELSE (CASE Isnull(PR.Reg, 0) WHEN 0 THEN Isnull(w.hourlyrate, 0) ELSE Isnull(PR.Reg, 0) END)  
        END )  
    FROM CONTRACT c   
    INNER JOIN JobTItem jt on c.job=jt.Job   
    INNER JOIN bom b on b.JobTItemID=jt.ID   
    INNER JOIN job j on c.job=j.id    
    INNER JOIN loc l on l.loc=c.Loc   
    INNER JOIN Route r on r.ID=l.Route   
    INNER JOIN tblWork w on w.ID =r.Mech  
    INNER JOIN emp e on e.fWork=w.ID INNER JOIN PRWageItem PR on PR.Wage=j.WageC and pr.Emp=e.ID  
        AND jt.Line= (select  min(jcc.Line) from jobtitem jcc where  jcc.type=1 and jcc.fDesc='Labor' and jcc.Job=@Job )
        and jt.type=1 and jt.fDesc='Labor' and c.job=@Job  
    
    UPDATE jc  set   jc.ETC= (m.LabRate * jc.BHours) 
    from bom  m   
    INNER JOIN jobtitem jc on m.JobTItemID=jc.ID 
    WHERE   jc.Line= ( select  min(jcc.Line) from jobtitem jcc where  jcc.type=1 and jcc.fDesc='Labor' and jcc.Job=@Job )   
        and jc.type=1 and jc.fDesc='Labor'   
        AND  isnull(m.LabRate,0) <> 0   
        AND  isnull(jc.BHours,0) <> 0 and jc.Job=@Job  
    
    UPDATE m set m.amount=     
        case c.BCycle  
        WHEN 0 THEN c.BAmt        --Monthly  \n");  
        WHEN 1 THEN c.BAmt / 2    --Bi-Monthly  \n");  
        WHEN 2 THEN c.BAmt / 3    --Quarterly  \n");  
        WHEN 3 THEN c.BAmt / 4    --3 Times/Year  \n");  
        WHEN 4 THEN c.BAmt / 6    --Semi-Annually   \n");  
        WHEN 5 THEN c.BAmt / 12   --Annually \n");   
        WHEN 7 THEN c.BAmt / (12*3)    --'3 Years'  \n");  
        WHEN 8 THEN c.BAmt / (12*5)    --'5 Years'  \n");  
        WHEN 9 THEN c.BAmt / (12*2)    --'2 Years'  \n");  
        else m.Amount end   
    FROM Milestone  m   
    INNER JOIN jobtitem jc ON m.JobTItemID=jc.ID   
    INNER JOIN contract c on c.job =jc.Job  
    WHERE jc.Line= 1 AND jc.type=0 and c.Job=@Job  
  
    UPDATE jc set jc.Budget= m.amount
    FROM Milestone  m   
    INNER JOIN jobtitem jc ON m.JobTItemID=jc.ID   
    INNER JOIN contract c on c.job =jc.Job  
    WHERE jc.Line= 1 AND jc.type=0  and c.Job=@Job 

    EXEC [dbo].[spUpdateJobcostByJob]  @job =@job  
  
    ------------------------------>  
  
    IF @@ERROR <> 0    
    BEGIN  
        RAISERROR ('Error Occured',16,1)    
        IF  @@TRANCOUNT > 0   ROLLBACK TRANSACTION    
        RETURN  
    END  
  
    INSERT INTO tbljoinElevJob  (Job,  elev,   price,  Hours)  
    SELECT @Job,  Elevunit,  price,   hours   FROM   @ElevJobData  
  
    ----- Update the price in Elev  table when user change it at Contract Screen  
    UPDATE e SET e.Price=t.Price  
    FROM tblJoinElevJob t  INNER JOIN elev e ON e.id=t.Elev  
    WHERE t.Job=@Job  AND t.Price <> e.Price AND t.Price <> 0   
 ---------------------  
  
  
    IF @@ERROR <> 0 
    BEGIN  
        RAISERROR ('Error Occured',16,1)    
        IF  @@TRANCOUNT > 0   ROLLBACK TRANSACTION    
        RETURN  
    END  
  
    UPDATE Loc        SET    Maint = 1,           Billing = @ContractBill    WHERE  Loc = @loc -- change by Mayuri 25th dec,15 for billing details in Loc, owner table  
  
    UPDATE [Owner]    SET    Billing = @CustomerBill,  Central = @Central    WHERE  ID = @owner  
  
    --------------------------------------------- update custom data of recurring contract -----------------------------------------  
    DECLARE DB_CURSOR CURSOR FOR  
  
    SELECT [ID], [tblTabID], [Label],  [Line],  [Value], [Format]  FROM   @CustomItems  
  
    OPEN DB_CURSOR  
  
    FETCH NEXT FROM DB_CURSOR INTO @tblCustomFieldsId,  
                                   @tblTabID,  
                                   @Label,  
                                   @TabLine,  
                                   @Value,  
                                   @Format  
  
    WHILE @@FETCH_STATUS = 0 BEGIN  
          INSERT INTO [dbo].[tblCustomJobT]  
                      ([JobTID],  
                       [JobID],  
                       [tblCustomFieldsID],  
                       [Value])  
          VALUES      (@JobT,  
                       @Job,  
                       @tblCustomFieldsId,  
                       @Value)  
  
          FETCH NEXT FROM DB_CURSOR INTO @tblCustomFieldsId,  
                                         @tblTabID,  
                                         @Label,  
                                         @TabLine,  
                                         @Value,  
                                         @Format  
      END  
  
    CLOSE DB_CURSOR  
  
    DEALLOCATE DB_CURSOR  

    ------------- Logs for recurring contract  -----------------------

    Declare @Screen varchar(100) = 'Job';

    Declare @ScreenProject varchar(100) = 'Project'; 
	 
    EXEC log2_insert @UpdatedBy,@Screen,@job,'ContractLength','',@ContractLength  

    if(@owner is not null)  
    Begin    
	    Declare @OwnerName varchar(150)  
	    Select @OwnerName = r.Name FROM Rol r INNER JOIN Owner o ON o.Rol = r.ID WHERE o.ID = @Owner  
	    exec log2_insert @UpdatedBy,@Screen,@job,'Customer Name','',@OwnerName  
    END  
    if(@loc is not null)  
    Begin    
	    Declare @LocName varchar(150)  
	    Select @LocName = tag from loc where loc = (Select Loc from Job where ID = @job)  
	    exec log2_insert @UpdatedBy,@Screen,@job,'Location Name','',@LocName  
    END  
    if(@loc is not null)  
    Begin    
	    Declare @LocAddress varchar(150)  
	    Select @LocAddress = Address from loc where loc = (Select Loc from Job where ID = @job)  
	    exec log2_insert @UpdatedBy,@Screen,@job,'Location Address','',@LocAddress  
    END  
    if(@PO is not null And @PO !='')  
    Begin  
	    exec log2_insert @UpdatedBy,@Screen,@job,'PO','',@PO  
    END  
    if(@Route is not null And @Route != '')  
    Begin    
	    Declare @RouteName   VARCHAR(75)  
	    Select @RouteName = Name from Route where ID = (Select Custom20 from Job where ID = @job)  
	    If(@RouteName IS Null)  
	    BEGIN  
		    exec log2_insert @UpdatedBy,@Screen,@job,'Preferred Worker','','Unassigned'  
	    END  
	    Else  
	    BEGIN  
		    exec log2_insert @UpdatedBy,@Screen,@job,'Preferred Worker','',@RouteName  
	    END  
    END  
    if(@Status is not null)  
    Begin    
	    Declare @StatusVal varchar(50)  
	    Select @StatusVal = Case @Status WHEN 0 THEN 'Active' WHEN 1 THEN 'Closed' WHEN 2 THEN 'Hold' WHEN 3 THEN 'Completed' END   
	    exec log2_insert @UpdatedBy,@Screen,@job,'Status','',@StatusVal  
    END  
    if(@CType is not null And @CType != '')  
    Begin  
	    exec log2_insert @UpdatedBy,@Screen,@job,'Service Type','',@CType  
    END  
    if(@Chart is not null)  
    Begin    
	    Declare @ChartGLAccount   VARCHAR(75)  
	    Select @ChartGLAccount = fDesc from Chart where ID = (Select Chart from Contract where Job = @Job)  
	    exec log2_insert @UpdatedBy,@Screen,@job,'GL Account','',@ChartGLAccount  
    END  
    if(@BStart is not null And @BStart != '')  
    Begin    
	    Declare @BStartdate nvarchar(150)  
	    SELECT @BStartdate = convert(varchar, @BStart, 101)  
	    exec log2_insert @UpdatedBy,@Screen,@job,'Billing Start Date','',@BStartdate  
    END  
    if(@BCycle is not null)  
    Begin    
	    Declare @BCycleVal varchar(50)  
	    Select @BCycleVal = Case @BCycle WHEN 0 THEN 'Monthly' 
									    WHEN 1 THEN 'Bi-Monthly' 
									    WHEN 2 THEN 'Quarterly' 
									    WHEN 3 THEN '3 Times/Year' 
									    WHEN 4 THEN 'Semi-Annually' 
									    WHEN 5 THEN 'Annually'
									    WHEN 6 THEN 'Never' 
									    WHEN 7 THEN '3 Years' 
									    WHEN 8 THEN '5 Years' 
									    WHEN 9 THEN '2 Years' 
						    END  
	    exec log2_insert @UpdatedBy,@Screen,@job,'Bill Frequency','',@BCycleVal  
    END  
    if(@Detail is not null)  
    Begin    
	    Declare @DetailVal varchar(50)  
	    Select @DetailVal = Case @Detail WHEN 0 THEN 'Summary' 
									    WHEN 1 THEN 'Detailed' 
									    WHEN 2 THEN 'Detailed w/Price' 
						    END  
	    exec log2_insert @UpdatedBy,@Screen,@job,'Bill Detail','',@DetailVal  
    END  
    if(@BAmt is not null)  
    Begin  
	    exec log2_insert @UpdatedBy,@Screen,@job,'Billing Amount','',@BAmt  
    END  
    if(@CustomerBill is not null)  
    Begin    
	    Declare @BillingVal varchar(50)  
	    Select @BillingVal = Case When @CustomerBill = 0 Then 'Individual' Else 'Combined' END  
	    exec log2_insert @UpdatedBy,@Screen,@job,'Billing','',@BillingVal  
    END  
    if(@Central is not null And @Central != 0)  
    Begin    
	    Declare @SpecifyLocName varchar(150)  
	    Select @SpecifyLocName = tag from loc where loc = (Select Central from Owner WHERE  ID = @owner)  
	    exec log2_insert @UpdatedBy,@Screen,@job,'Specify Location','',@SpecifyLocName  
    END  
    if(@ContractBill is not null)  
    Begin    
	    Declare @ContractBillVal varchar(50)  
	    Select @ContractBillVal = Case When @ContractBill = 0 Then 'Separate per Contract' Else 'Combined on One Invoice' END  
	    exec log2_insert @UpdatedBy,@Screen,@job,'Contract Billing','',@ContractBillVal  
    END  
    if( @Creditcard is not null)  
    Begin    
	    exec log2_insert @UpdatedBy,@Screen,@job,'Credit Card','', @Creditcard  
    END  
    if(@SPHandle is not null)  
    Begin  
	    exec log2_insert @UpdatedBy,@Screen,@job,'IsSpecial Notes','',@SPHandle  
    END  
    if(@SPRemarks is not null)  
    Begin  
	    exec log2_insert @UpdatedBy,@Screen,@job,'Special Notes','',@SPRemarks  
    END  
    if(@SStart is not null And @SStart != '')  
    Begin    
	    Declare @SStartdate nvarchar(150)  
	    SELECT @SStartdate = convert(varchar, @SStart, 101)  
	    exec log2_insert @UpdatedBy,@Screen,@job,'Schedule Start Date','',@SStartdate  
    END  
    if(@Cycle is not null)  
    Begin    
	    Declare @CycleFreqVal varchar(50)  
	    Select @CycleFreqVal = Case  @Cycle WHEN -1 THEN 'Never' 
									    WHEN 0 THEN 'Monthly' 
									    WHEN 1 THEN 'Bi-Monthly' 
									    WHEN 2 THEN 'Quarterly'
									    WHEN 3 THEN 'Semi-Annually'
									    WHEN 4 THEN 'Annually' 
									    WHEN 5 THEN 'Weekly' 
									    WHEN 6 THEN 'Bi-Weekly' 
									    WHEN 7 THEN 'Every 13 Weeks'
									    WHEN 8 THEN 'Every 3 Years' 
									    WHEN 9 THEN 'Every 5 Years'
									    WHEN 10 THEN 'Every 2 Years'
									    WHEN 11 THEN 'Every 7 Years'
									    WHEN 12 THEN 'On-Demand' 
									    WHEN 13 THEN 'Daily'
									    WHEN 14 THEN 'Twice a Month' 
							    END  
	    exec log2_insert @UpdatedBy,@Screen,@job,'Schedule Frequency','',@CycleFreqVal  
    END  
    if(@SWE is not null)  
    Begin    
        exec log2_insert @UpdatedBy,@Screen,@job,'Weekends','',@SWE  
    END  
    if(@Stime is not null And @Stime != '')  
    Begin    
	    Declare @Stimedate nvarchar(150)  
	    SELECT @Stimedate = FORMAT(@Stime, 'hh:mm tt')  
	    exec log2_insert @UpdatedBy,@Screen,@job,'Scheduled Time','',@Stimedate  
    END  
    if(@hours is not null)  
    Begin  
	    exec log2_insert @UpdatedBy,@Screen,@job,'Total Hours','',@hours  
    END  
    if(@IsRenewalNotes is not null)  
    Begin  
	    exec log2_insert @UpdatedBy,@Screen,@job,'IsRenewal Notes','',@IsRenewalNotes  
    END  
    if(@RenewalNotes is not null)  
    Begin  
	    exec log2_insert @UpdatedBy,@Screen,@job,'Renewal Notes','',@RenewalNotes  
    END  
    if(@BillRate is not null)  
    Begin  
	    exec log2_insert @UpdatedBy,@Screen,@job,'Bill Rate','',@BillRate  
    END  
    if(@RateOT is not null)  
    Begin  
	    exec log2_insert @UpdatedBy,@Screen,@job,'OT Rate','',@RateOT  
    END  
    if(@RateNT is not null)  
    Begin  
	    exec log2_insert @UpdatedBy,@Screen,@job,'NT Rate','',@RateNT  
    END  
    if(@RateDT is not null)  
    Begin  
	    exec log2_insert @UpdatedBy,@Screen,@job,'DT Rate','',@RateDT  
    END  
    if(@RateTravel is not null)  
    Begin  
	    exec log2_insert @UpdatedBy,@Screen,@job,'Travel Rate','',@RateTravel  
    END  
    if(@Mileage is not null)  
    Begin  
	    exec log2_insert @UpdatedBy,@Screen,@job,'Mileage','',@Mileage  
    END  
    if(@EscalationType is not null)  
    Begin    
	    Declare @EscalationTypeVal varchar(50)  
	    Select  @EscalationTypeVal = Case @EscalationType WHEN 0 THEN 'Commodity Index' WHEN 1 THEN 'Escalation' WHEN 2 THEN 'Return' WHEN 3 THEN 'Manual' END  
	    exec log2_insert @UpdatedBy,@Screen,@job,'Escalation Type','',@EscalationTypeVal  
    END  
    if(@EscalationCycle is not null)  
    Begin  
	    exec log2_insert @UpdatedBy,@Screen,@job,'Escalation Cycle','',@EscalationCycle  
    END  
    if(@EscalationFactor is not null)  
    Begin  
	    exec log2_insert @UpdatedBy,@Screen,@job,'Escalation Factor','',@EscalationFactor  
    END  
    if(@EscalationLast is not null And @EscalationLast != '')  
    Begin    
	    Declare @EscalationLastdate nvarchar(150)  
	    SELECT @EscalationLastdate = convert(varchar, @EscalationLast, 101)  
	    exec log2_insert @UpdatedBy,@Screen,@job,'Escalated Last','',@EscalationLastdate  
    END  
    if(@Expiration is not null)  
    Begin    
	    Declare @ExpirationVal Varchar(50)  
	    Select @ExpirationVal = Case @Expiration When  1 Then 'Contract expiration date' When 2 Then 'Number of frequencies' Else 'Indefinitely' END  
	    exec log2_insert @UpdatedBy,@Screen,@job,'Expiration','',@ExpirationVal  
    END  
    if(@ExpirationDate is not null And @ExpirationDate != '')  
    Begin    
	    Declare @ExpirationDateVal nvarchar(150)  
	    SELECT @ExpirationDateVal = convert(varchar, @ExpirationDate, 101)  
	    exec log2_insert @UpdatedBy,@Screen,@job,'Expiration Date','',@ExpirationDateVal  
    END  
    if(@ExpirationFreq is not null)  
    Begin    
	    exec log2_insert @UpdatedBy,@Screen,@job,'Exp Frequencies','',@ExpirationFreq  
    END  
    if(@fdesc is not null And @fdesc != '')  
    Begin  
	    exec log2_insert @UpdatedBy,@Screen,@job,'Contract Description','',@fdesc  
    END  
    if(@Remarks is not null)  
    Begin  
	    exec log2_insert @UpdatedBy,@Screen,@job,'Remarks','',@Remarks  
    END   
    Declare @EquipmentJob VARCHAR(1000)  
    Select @EquipmentJob  =  STUFF((SELECT ',  ' + e.Unit From Elev e inner Join tblJoinElevJob ej on e.ID = ej.Elev Where ej.Job = @job FOR XML PATH('')), 1, 1, '')  
    IF (@EquipmentJob is not NUll)  
    Begin  
	    exec log2_insert @UpdatedBy,@Screen,@job,'Equipment','',@EquipmentJob  
    END  

    ------------ End Logs for recurring contract  -----------------------
    COMMIT TRANSACTION