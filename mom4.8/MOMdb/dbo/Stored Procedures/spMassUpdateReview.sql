CREATE PROCEDURE [dbo].[spMassUpdateReview] @department  INT,
                                             @QBpayroll  VARCHAR(50),
                                             @QBservice  VARCHAR(50),
											 @Payroll    int ,
											 @UpdateBy   VARCHAR(50),
                                             @Data       AS [dbo].[tblTypeMassUpdateTicket] Readonly
AS



    DECLARE @MyCursor CURSOR;

    DECLARE @TicketID INT, @WageCatFlag SMALLINT=0 , @QBIntegration int =0;

	SELECT @QBIntegration=isnull(QBIntegration,0)  from control   WITH (NOLOCK) 

    BEGIN

      SET @MyCursor = CURSOR

      FOR SELECT ticketID   FROM   @Data

      OPEN @MyCursor

      FETCH NEXT FROM @MyCursor INTO @TicketID

      WHILE @@FETCH_STATUS = 0

      BEGIN
-----------------DECLARE ----------------------
            DECLARE @LocID           INT,
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
                    @Reason          VARCHAR(max),
                    @CustName        VARCHAR(50),
                    @custID          INT,
                    --@TicketID     INT,
                    @EST             NUMERIC(30, 2),
                    @complDesc       VARCHAR(max),
                    @Reg             NUMERIC(30, 2),
                    @OT              NUMERIC(30, 2),
                    @NT              NUMERIC(30, 2),
                    @TT              NUMERIC(30, 2),
                    @DT              NUMERIC(30, 2),
                    @Total           NUMERIC(30, 2),
                    @Charge          INT,
                    @Review          INT,
                    @Who             VARCHAR(30),
                    @sign            VARBINARY(max),
                    @remarks         VARCHAR(max),
                    @Type            INT,
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
                    @CreditReason    VARCHAR(max),
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
                    @Customtick5     VARCHAR(100),
                    @job             INT,
                    @JobCode         VARCHAR(255),
                    @ProjectTemplate INT,
                    @wage            INT,
                    @fBy             VARCHAR(50),
                    @Equipments      AS TBLTYPEMULTIPLEEEQUIPMENTS,
                    @prospect        INT,
					@UpdateTasks smallint = 0 ,					
					@BT  NUMERIC(30, 2)=0,
					@Comments  varchar(1000)=null,
					@PartsUsed  varchar(100)=null,
					@TaskCodes as tblTypeTaskCodes,
					@IsCreateJob    INT=0,
					@Level    INT=1

-------------------SELECT--------------

            SELECT @prospect = LType,
                   @LocID = LID,
                   @LocTag = (SELECT Tag  FROM   loc  WITH (NOLOCK)  WHERE  loc = LID),
                   @LocAdd = (SELECT Address FROM   loc  WITH (NOLOCK)  WHERE  loc = LID),
                   @City = (SELECT City  FROM   loc  WITH (NOLOCK)  WHERE  loc = LID),
                   @State = (SELECT State  FROM   loc  WITH (NOLOCK)  WHERE  loc = LID),
                   @Zip = (SELECT Zip  FROM   loc  WITH (NOLOCK) WHERE  loc = LID),
                   @Phone = (select top 1 Phone from rol  WITH (NOLOCK) where ID=(select top 1 rol from loc  WITH (NOLOCK) where loc=o.LID)),
                   @Cell =  (select top 1 Cellular from rol  WITH (NOLOCK) where ID=(select top 1 rol from loc  WITH (NOLOCK) where loc=o.LID)),
                   @Worker = DWork,
                   @CallDt = o.CDate,
                   @SchDt = o.EDate,
                   @Status = 4,
                   @EnrouteTime = d.TimeRoute,
                   @Onsite = d.TimeSite,
                   @Complete = d.TimeComp,
                   @Category = o.Cat,
                   @Unit = o.LElev,
                   @Reason = o.fDesc,
                   @CustName = NULL,
                   @custID = o.Owner,
                   --@TicketID = o.ID,
                   @EST = o.Est,
                   @complDesc = d.DescRes,
                   @Reg = d.Reg,
                   @OT = d.OT,
                   @NT = d.NT,
                   @TT = d.TT,
                   @DT = d.DT,
                   @Total = d.Total,
                   @Charge = d.Charge,
                   @Review = d.ClearCheck,
                   @Who = o.Who,
                   --@sign = ,
                   @remarks = (SELECT Remarks FROM   loc   WITH (NOLOCK)  WHERE  loc = LID),
                   @Type = o.Type,
                   @Custom1 = o.Custom1,
                   @Custom2 = o.Custom2,
                   @Custom3 = o.Custom3,
                   @Custom4 = o.Custom4,
                   @Custom5 = o.Custom5,
                   @Custom6 = o.Custom6,
                   @Custom7 = o.Custom7,
                   @WorkOrder = o.WorkOrder,
                   @WorkComplete = d.WorkComplete,
                   @MiscExp = d.OtherE,
                   @TollExp = d.Toll,
                   @ZoneExp = d.Zone,
                   @MileStart = d.SMile,
                   @MileEnd = d.EMile,
                   @Internet = d.Internet,
                   @Invoice = NULL,
                   @TransferTime = NULL,
                   @CreditHold = (SELECT Credit  FROM   loc   WITH (NOLOCK) WHERE  loc = lid),
                   @DispAlert = (SELECT DispAlert  FROM   loc   WITH (NOLOCK) WHERE  loc = lid),
                   @CreditReason = (SELECT CreditReason  FROM   loc  WITH (NOLOCK) WHERE  loc = lid),
                   @QBServiceItem = @QBservice,
                   @QBPayrollItem = @QBpayroll,
                   @LastUpdatedBy = NULL,
                   @Contact = (SELECT Contact  FROM   rol    WHERE  id = (SELECT TOP 1 Rol FROM   Loc   WITH (NOLOCK) WHERE  Loc = lid)),
                   @Recommendation = o.BRemarks,
                   @Customtick1 = o.CustomTick1,
                   @Customtick2 = o.CustomTick2,
                   @Customtick3 = o.CustomTick3,
                   @Customtick4 = o.CustomTick4,
                   @lat = (SELECT Lat  FROM   rol   WHERE  id = (SELECT TOP 1 Rol  FROM   Loc  WITH (NOLOCK)   WHERE  Loc = lid)),
                   @lng = (SELECT Lng  FROM   rol   WHERE  id = (SELECT TOP 1 Rol  FROM   Loc  WITH (NOLOCK) WHERE  Loc = lid)),
                   @DefaultRoute = (SELECT Route  FROM   loc  WHERE  loc = lid),
                   @Customtick5 = o.CustomTick5,
                   @job = o.Job,
                   @JobCode = ( SELECT TOP 1 CONVERT(varchar, j.Line) + ':' + j.Code + ':' + j.fDesc
                FROM jobtitem j  WITH (NOLOCK)
                INNER JOIN bom b  WITH (NOLOCK)
                    ON b.JobtItemId = j.ID
                INNER JOIN BOMT  WITH (NOLOCK)
                    ON BOMT.ID = b.Type
                WHERE j.job = o.job and j.Code=o.JobCode and j.Line=d.Phase),
                   @ProjectTemplate = NULL,
                   @wage = d.WageC,
                   @fBy = o.fBy,
				   @BT=d.break_time, 
				   @UpdateTasks  = @UpdateTasks ,	 
				   @Comments  =d.Comments,
				   @PartsUsed   =d.PartsUsed,
				   @Level=d.Level ----New Field

            --,@Equipments = NULL

            FROM   TicketO o  WITH (NOLOCK)    INNER JOIN TicketDPDA d  WITH (NOLOCK)   ON o.ID = d.ID  WHERE  o.ID = @ticketID

			IF EXISTS(select 1 FROM   TicketO o  WITH (NOLOCK)    INNER JOIN TicketDPDA d  WITH (NOLOCK)    ON o.ID = d.ID  WHERE  o.ID = @ticketID)

			BEGIN -------------If Exists>

            DECLARE @query NVARCHAR(350)

			----------------------------
			 
			DECLARE @signaturetbl VARCHAR(100);

			SET @signaturetbl='PDA_'+ CONVERT(VARCHAR(50), (SELECT ID FROM tblWork  WITH (NOLOCK) WHERE fDesc=@worker))

			IF EXISTS(SELECT TABLE_NAME FROM information_schema.tables where TABLE_NAME = @signaturetbl)
            BEGIN
			 
            SET @query = 'SELECT TOP 1 @signs = signature FROM ' + @signaturetbl + ' WHERE  pdaticketid = @tic' 

            EXECUTE Sp_executesql     @query, N'@tic int, @signs varbinary(max) OUTPUT',   @tic=@ticketID,  @signs=@sign OUTPUT 

			END 

             -----------------------------

            INSERT INTO @Equipments
            SELECT ticket_id,   elev_id, labor_percentage  FROM   multiple_equipments  WITH (NOLOCK) WHERE  ticket_id = @TicketID

            --BEGIN TRANSACTION
			
            IF( @prospect = 0 )
              BEGIN
                  IF( @Status = 4 )
                    BEGIN
						-- added check for wages, if wage is null then get wage by project and user otherwise don't update
						IF(ISNULL(@wage,0)=0)
						BEGIN
							 DECLARE @USERWageCount INT =0, @Jobwage INT
							 SELECT @Jobwage=WageC FROM job  WITH (NOLOCK) WHERE id=@job 
							 SELECT @USERWageCount=count(Wage) 
							 FROM PRWageItem pr  WITH (NOLOCK)
							 WHERE pr.Emp =  (SELECT id FROM emp e  WITH (NOLOCK) where e.fWork =
							 ((SELECT ID FROM tblWork  WITH (NOLOCK)  WHERE  fDesc = @Worker)))						
							 IF (@USERWageCount = 1)
							 BEGIN
						     (SELECT  @wage=(Wage) FROM PRWageItem pr  WITH (NOLOCK) 
							 WHERE pr.Emp = (SELECT id FROM emp e  WITH (NOLOCK) where e.fWork = 	
							 (SELECT ID FROM tblWork  WITH (NOLOCK) WHERE fDesc = @Worker)))
							 END
							 ELSE IF EXISTS (SELECT 1 FROM PRWageItem pr  WITH (NOLOCK)
							 WHERE pr.Wage = @Jobwage and pr.Emp = (SELECT id FROM emp e  WITH (NOLOCK) where e.fWork = 
							(SELECT ID FROM tblWork  WITH (NOLOCK) WHERE fDesc = @Worker)))
							 BEGIN 
							 SELECT @wage=(Wage) FROM PRWageItem pr  WITH (NOLOCK) 
							 WHERE  pr.Wage = @Jobwage and pr.Emp = (SELECT id FROM emp e where e.fWork = 
							(SELECT ID FROM tblWork  WITH (NOLOCK) WHERE  fDesc = @Worker))
							 END ELSE BEGIN SET @WageCatFlag=1 END
							 SET @USERWageCount =0;
							 SET @Jobwage =0;
						END

                        IF (ISNULL(@wage,0) <> 0 or (@QBIntegration=1 ))
                          BEGIN
                              EXEC spupdateticket
                                @LocID = @LocID,
                                @LocTag = @LocTag,
                                @LocAdd = @LocAdd,
                                @City = @City,
                                @State = @State,
                                @Zip = @Zip,
                                @Phone = @Phone,
                                @Cell = @Cell,
                                @Worker = @Worker,
                                @CallDt = @CallDt,
                                @SchDt = @SchDt,
                                @Status = @Status,
                                @EnrouteTime = @EnrouteTime,
                                @Onsite = @Onsite,
                                @Complete = @Complete,
                                @Category = @Category,
                                @Unit = @Unit,
                                @Reason = @Reason,
                                @CustName = @CustName,
                                @custID = @custID,
                                @TicketID = @TicketID,
                                @EST = @EST,
                                @complDesc = @complDesc,
                                @Reg = @Reg,
                                @OT = @OT,
                                @NT = @NT,
                                @TT = @TT,
                                @DT = @DT,
                                @Total = @Total,
                                @Charge = @Charge,
                                @Review = @Review,
                                @Who = @Who,
                                @sign = @sign,
                                @remarks = @remarks,
                                @Type = @Type,
                                @Custom1 = @Custom1,
                                @Custom2 = @Custom2,
                                @Custom3 = @Custom3,
                                @Custom4 = @Custom4,
                                @Custom5 = @Custom5,
                                @Custom6 = @Custom6,
                                @Custom7 = @Custom7,
                                @WorkOrder = @WorkOrder,
                                @WorkComplete = @WorkComplete,
                                @MiscExp = @MiscExp,
                                @TollExp = @TollExp,
                                @ZoneExp = @ZoneExp,
                                @MileStart = @MileStart,
                                @MileEnd = @MileEnd,
                                @Internet = @Internet,
                                @Invoice = @Invoice,
                                @TransferTime = @TransferTime,
                                @CreditHold = @CreditHold,
                                @DispAlert = @DispAlert,
                                @CreditReason = @CreditReason,
                                @QBServiceItem = @QBServiceItem,
                                @QBPayrollItem = @QBPayrollItem,
                                @LastUpdatedBy = @LastUpdatedBy,
                                @Contact = @Contact,
                                @Recommendation = @Recommendation,
                                @Customtick1 = @Customtick1,
                                @Customtick2 = @Customtick2,
                                @Customtick3 = @Customtick3,
                                @Customtick4 = @Customtick4,
                                @lat = @lat,
                                @lng = @lng,
                                @DefaultRoute = @DefaultRoute,
                                @Customtick5 = @Customtick5,
                                @job = @job,
                                @JobCode = @JobCode,
                                @ProjectTemplate = @ProjectTemplate,
                                @wage = @wage,
                                @fBy = @fBy,
                                @Equipments = @Equipments,
								@UpdateTasks  = @UpdateTasks ,	
								@TaskCodes =@TaskCodes,				
					            @BT =@BT,
					            @Comments =@Comments,
					            @PartsUsed =@PartsUsed,
								@IsCreateJob=@IsCreateJob 	,
								@Level=@Level,
								@clearPR=0
                          END
                    END
              END

			
             DELETE FROM @Equipments 
			 DELETE FROM @TaskCodes
			 SET @SIGN =NULL;
			 --- $$$SET NULL ALL Local variable $$$  ---
			        SET  @LocID           =NULL ;
                    SET  @LocTag          =NULL ;
                    SET  @LocAdd          =NULL ;
                    SET  @City            =NULL ;
                    SET  @State           =NULL ;
                    SET  @Zip             =NULL ;
                    SET  @Phone           =NULL ;
                    SET  @Cell            =NULL ;
                    SET  @Worker          =NULL ;
                    SET  @CallDt          =NULL ;
                    SET  @SchDt           =NULL ;
                    SET  @Status          =NULL ;
                    SET  @EnrouteTime     =NULL ;
                    SET  @Onsite          =NULL ;
                    SET  @Complete        =NULL ;
                    SET  @Category        =NULL ;
                    SET  @Unit            =NULL ;
                    SET  @Reason          =NULL ;
                    SET  @CustName        =NULL ;
                    SET  @custID          =NULL ; 
                    SET  @EST             =NULL ;
                    SET  @complDesc       =NULL ;
                    SET  @Reg             =NULL ;
                    SET  @OT              =NULL ;
                    SET  @NT              =NULL ;
                    SET  @TT              =NULL ;
                    SET  @DT              =NULL ;
                    SET  @Total           =NULL ;
                    SET  @Charge          =NULL ;
                    SET  @Review          =NULL ;
                    SET  @Who             =NULL ;
                    SET  @sign            =NULL ;
                    SET  @remarks         =NULL ;
                    SET  @Type            =NULL ;
                    SET  @Custom1         =NULL ;
                    SET  @Custom2         =NULL ;
                    SET  @Custom3         =NULL ;
                    SET  @Custom4         =NULL ;
                    SET  @Custom5         =NULL ;
                    SET  @Custom6         =NULL ;
                    SET  @Custom7         =NULL ;
                    SET  @WorkOrder       =NULL ;
                    SET  @WorkComplete    =NULL ;
                    SET  @MiscExp         =NULL ;
                    SET  @TollExp         =NULL ;
                    SET  @ZoneExp         =NULL ;
                    SET  @MileStart       =NULL ;
                    SET  @MileEnd         =NULL ;
                    SET  @internet        =NULL ;
                    SET  @Invoice         =NULL ;
                    SET  @TransferTime    =NULL ;
                    SET  @CreditHold      =NULL ;
                    SET  @DispAlert       =NULL ;
                    SET  @CreditReason     =NULL ;
                    SET  @QBServiceItem   =NULL ;
                    SET  @QBPayrollItem   =NULL ;
                    SET  @LastUpdatedBy   =NULL ;
                    SET  @Contact         =NULL ;
                    SET  @Recommendation   =NULL ;
                    SET  @Customtick1     =NULL ;
                    SET  @Customtick2     =NULL ;
                    SET  @Customtick3     =NULL ;
                    SET  @Customtick4     =NULL ;
                    SET  @lat             =NULL ;
                    SET  @lng             =NULL ;
                    SET  @DefaultRoute    =NULL ;
                    SET  @Customtick5     =NULL ;
                    SET  @job             =NULL ;
                    SET  @JobCode        =NULL ;
                    SET  @ProjectTemplate =NULL ;
                    SET  @wage            =NULL ;
                    SET  @fBy             =NULL ; 
                    SET  @prospect        =NULL ;
					SET  @UpdateTasks     =NULL  ;					
					SET  @BT  = NULL  ;
					SET  @Comments = NULL ;
					SET  @PartsUsed   =null;

			 --------

            --COMMIT TRANSACTION

			END----------------If Exists>

            FETCH NEXT FROM @MyCursor INTO @TicketID
        END;

      CLOSE @MyCursor;

      DEALLOCATE @MyCursor;
  END;
   

	   IF (@QBIntegration =0) 
	   
	  BEGIN ---3
	   
	  IF EXISTS(select 1 FROM   TicketD  WITH (NOLOCK)  INNER JOIN @Data d ON TicketD.ID = d.TicketID   WHERE (Isnull(WageC,0) =0))
	  BEGIN---2

	  Declare  @temptable TABLE ( ROWNO int IDENTITY (1, 1) NOT NULL ,  TicketID int NOT NULL ) 
	  INSERT INTO @temptable (TicketID)

	  SELECT Ticketd.id TicketID   FROM   TicketD   WITH (NOLOCK)   
	  INNER JOIN @Data d ON TicketD.ID = d.TicketID  WHERE (Isnull(WageC,0) =0)

	  DECLARE @ROW_Count int ;       DECLARE @ROW_NO int =1;
      SELECT  @ROW_Count=max(ROWNO) from @temptable where TicketID is not null 


		 WHILE(@ROW_NO <=@ROW_Count)
		 BEGIN---1

		 
		 DECLARE @nkID int;  

		 SELECT @nkID=TicketID FROM @temptable WHERE TicketID is not null and ROWNO=@ROW_NO
    
		 EXEC spReCalCulateLaborexpense    @nkTicketID =@nkID 

		 SET @ROW_NO+=1; 

		 END-----1 

	     END---2

	     END---3
 	 
      UPDATE TicketD     SET    TicketD.ClearCheck = d.clearcheck,   TicketD.transfertime = d.transfertime, 
	  TicketD.charge = CASE  WHEN ( Isnull(TicketD.Charge, 0) = 0   AND Isnull(TicketD.ClearCheck, 0) = 0 )  THEN Isnull((SELECT TOP 1 Isnull(chargeable, 0)  FROM   Category   WHERE  Type = TicketD.Cat), 0)   ELSE TicketD.charge  END
      FROM   TicketD     INNER JOIN @Data d ON TicketD.ID = d.TicketID   WHERE (Isnull(WageC,0) <> 0 OR @QBIntegration=1)
	  
      DECLARE @tinternet INT = 0;      SELECT @tinternet = Isnull(TInternet, 0)  FROM   Control

      IF( @tinternet = 1 )

      BEGIN
          UPDATE TicketD   SET    TicketD.Internet = 1
		  FROM   TicketD INNER JOIN @Data d ON TicketD.ID = d.TicketID  
		  WHERE (Isnull(WageC,0) <> 0 OR @QBIntegration=1)
      END


	  IF( @Payroll = 1 )
      BEGIN


          UPDATE TicketD   SET    TicketD.ClearPR = 1
		  FROM   TicketD INNER JOIN @Data d ON TicketD.ID = d.TicketID  
		  WHERE (Isnull(d.ClearPR,0) = 1)

		  INSERT INTO [Log2] (   [fUser]         , [Screen]   ,[Ref] ,[Field] , [NewVal]  )
		  SELECT                  @UpdateBy , 'Ticket',   d.TicketID,'Mass-Payroll',  'Y'  
		  FROM   TicketD INNER JOIN @Data d ON TicketD.ID = d.TicketID  
		  WHERE (Isnull(d.ClearPR,0) = 1) 
  

      END
	  
	       --------------------------------------
		   --------------------------------------> Log
		   --------------------------------------

	      INSERT INTO [Log2] (   [fUser]  , [Screen]  ,[Ref] ,[Field] , [NewVal]  )
		  SELECT   @UpdateBy , 'Ticket',   d.TicketID,'Mass-Review',  'Y'  
		  FROM   TicketD INNER JOIN @Data d ON TicketD.ID = d.TicketID  
		  WHERE (Isnull(d.Clearcheck,0) = 1)


		  INSERT INTO [Log2] (   [fUser]         , [Screen]   ,[Ref] ,[Field] , [NewVal]  )
		  SELECT                  @UpdateBy , 'Ticket',   d.TicketID,'Mass-Timesheet',  'Y'  
		  FROM   TicketD INNER JOIN @Data d ON TicketD.ID = d.TicketID  
		  WHERE (Isnull(d.transfertime,0) = 1)


	  IF EXISTS(select 1 FROM   TicketD  WITH (NOLOCK)   INNER JOIN @Data d ON TicketD.ID = d.TicketID WHERE (Isnull(WageC,0) =0))
	  BEGIN SET @WageCatFlag=1; END

	  IF (@QBIntegration =1)  BEGIN SET @WageCatFlag=0;  END

	  SELECT @WageCatFlag