
-- =============================================
-- Author:		<Harsh dwivedi>
-- Create date: <16th dec 2018>
-- Description:	<Save timesheet data>
-- =============================================
CREATE PROCEDURE [dbo].[spSaveTimeCardJob]
	@Super NVARCHAR(255),
	@Worker NVARCHAR(100),
	@Category NVARCHAR(100),
	@MarkedReview INT=0,
	@Timesheet INT =0,
	@UserName NVARCHAR(100),
	@DataTable AS tblTypeTimeCardinput READONLY,
	@Result INT OUTPUT

AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @LocID           INT,
            @LocTag          VARCHAR(50),
            @LocAdd          VARCHAR(255),
            @City            VARCHAR(50),
            @State           VARCHAR(2),
            @Zip             VARCHAR(100),
            @Phone           VARCHAR(28),
            @Cell            VARCHAR(50),
            --@Worker          VARCHAR(50),
            @CallDt          DATETIME,
            @SchDt           DATETIME,
            @Status          SMALLINT,
            @EnrouteTime     DATETIME,
            @Onsite          DATETIME,
            @Complete        DATETIME,
            --@Category        VARCHAR(25),
            @Unit            INT,
            @Reason          VARCHAR(max),
            @CustName        VARCHAR(50),
            @custID          INT, 
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
            @Customtick5     NUMERIC(30, 2),
            @job             INT,
            @JobCode         VARCHAR(10),
            @ProjectTemplate INT,
            @wage            INT,
            @fBy             VARCHAR(50),
            @Equipments      AS tblTypeMultipleEequipments,
            @prospect        INT,
			@UpdateTasks smallint = 0 ,					
			@BT  NUMERIC(30, 2)=0,
			@Comments  varchar(1000)=null,
			@PartsUsed  varchar(100)=null,
			@TaskCodes AS tblTypeTaskCodes,
			@AID   UNIQUEIDENTIFIER, 
            @Level    INT,
			@IsRecurring     TINYINT,
			@TicketIDOut INT=0,
			@RecurringDate DATETIME=NULL,
			@Zone int = null,
			@Cate NVARCHAR(100)

	  DECLARE @Counter INT
	  SET @Counter = 1

	  WHILE (SELECT COUNT(1) FROM @DataTable) >= @Counter  
			BEGIN

			IF (SELECT COUNT(1) FROM @DataTable) < @Counter  
			     BEGIN   
					BREAK; 
				 END
			   SELECT @LocID=loc FROM Job WHERE ID =(SELECT project FROM @DataTable WHERE ID=@Counter)
			   SELECT @prospect = 0,
                   @LocID = @LocID,
                   @LocTag = (SELECT Tag
                              FROM   loc
                              WHERE  loc = @LocID),
                   @LocAdd = (SELECT Address
                              FROM   loc
                              WHERE  loc = @LocID),
                   @City = (SELECT City
                            FROM   loc
                            WHERE  loc = @LocID),
                   @State = (SELECT State
                             FROM   loc
                             WHERE  loc = @LocID),
                   @Zip = (SELECT Zip
                           FROM   loc
                           WHERE  loc = @LocID),
                   @Phone = (select top 1 Phone from rol where ID=(select top 1 rol from loc where loc=@LocID)),
                   @Cell =  (select top 1 Cellular from rol where ID=(select top 1 rol from loc where loc=@LocID)),
                   @Worker = @Worker,
                   @CallDt =d.[time],
				   @SchDt = d.[time],
                   @Status =  4,
                   @EnrouteTime = d.[time],
                   @Onsite = d.[time],
                   @Complete = DATEADD(HOUR, (d.reg+d.ot+d.dt+d.nt+d.travel), d.[time]),--@TotalTime,-- need to confirm from anita
                   --@Category = @category,
				   @Cate =d.cate,
                   @Unit = null,
                   @Reason = d.[desc],
                   @CustName = NULL,
                   @custID = (SELECT Owner
                              FROM   loc
                              WHERE  loc = @LocID), 
                   @EST = null,
                   @complDesc = d.[desc],-- need confirm with Anita				 
				   @TicketIDOut=0,
                   @Reg = d.reg,
                   @OT = d.ot,
                   @NT = d.nt,
                   @TT =d.travel,
                   @DT = d.dt,
                   @Total = (reg+ot+dt+nt+travel),--to do calculation
                   @Charge = (SELECT charge FROM Job Where ID=project),--need confirm with Anita
                   @Review =@MarkedReview,
                   @Who ='Timesheet',-- d.who_called, --need confirm with Anita
                   @remarks = (SELECT Remarks
                               FROM   loc
                               WHERE  loc = @LocID),
                   @Type = (SELECT type FROM Job WHERE ID=project),--need confirm with NK
                   @Custom1 = NULL,
                   @Custom2 = NULL,
                   @Custom3 = NULL,
                   @Custom4 = NULL,
                   @Custom5 = NULL,
                   @Custom6 = NULL,
                   @Custom7 = NULL,
                   @WorkOrder = d.wo,
                   @WorkComplete = (CASE WHEN @MarkedReview =1 THEN 1 ELSE NULL END),
                   @MiscExp = 0,
                   @TollExp = 0,
                   @ZoneExp = 0,
                   @MileStart = 0,
                   @MileEnd = 0,
                   @Internet = 0,
                   @Invoice = '',
                   @TransferTime = @Timesheet,
                   @CreditHold = (SELECT Credit
                                  FROM   loc
                                  WHERE  loc = @LocID),
                   @DispAlert = (SELECT DispAlert
                                 FROM   loc
                                 WHERE  loc = @LocID),
                   @CreditReason = (SELECT CreditReason
                                    FROM   loc
                                    WHERE  loc = @LocID),
                   @QBServiceItem = null,
                   @QBPayrollItem = null,
                   @LastUpdatedBy = NULL,
                   @Contact = (SELECT Contact
                               FROM   rol
                               WHERE  id = (SELECT TOP 1 Rol
                                            FROM   Loc
                                            WHERE  Loc =@LocID)),
                   @Recommendation = null,
                   @Customtick1 = null,
                   @Customtick2 = null,
                   @Customtick3 = null,
                   @Customtick4 = null,
                   @lat = (SELECT Lat
                           FROM   rol
                           WHERE  id = (SELECT TOP 1 Rol
                                        FROM   Loc
                                        WHERE  Loc = @LocID)),
                   @lng = (SELECT Lng
                           FROM   rol
                           WHERE  id = (SELECT TOP 1 Rol
                                        FROM   Loc
                                        WHERE  Loc = @LocID)),
                   @DefaultRoute = (SELECT Route
                                    FROM   loc
                                    WHERE  loc = @LocID),
                   @Customtick5 = null,

                   @job = d.project,
                   @JobCode = (SELECT TOP 1 CONVERT(VARCHAR, j.Line) + ':' + j.Code + ':' + j.fDesc
								FROM   jobtitem j
                                   INNER JOIN bom b ON b.JobtItemId = j.ID
                                   INNER JOIN BOMT ON BOMT.ID = b.Type
                            WHERE  j.job = d.project
                                   AND j.Line=d.type),--job code is type--CONVERT(VARCHAR, j.Line) + ':' + j.Code + ':'+ j.fDesc
                   @ProjectTemplate = NULL,
                   @wage = wage,
                   @fBy =  @UserName,--need confirm with Anita
				   @BT=0, 
				   @UpdateTasks  =null ,	 
				   @Comments  =null,
				   @PartsUsed   =null--,
				   
				   --@Equipments = @equipment
				   FROM @DataTable d WHERE ID=@Counter

				   SET @sign=NULL  

		IF(SELECT equipment FROM @DataTable WHERE ID=@Counter)<> ''
		BEGIN
			DECLARE @Equipment NVARCHAR(250)
			SELECT @Equipment=equipment FROM @DataTable WHERE ID=@Counter
			INSERT INTO @Equipments
			SELECT 0, value,0 FROM STRING_SPLIT(@Equipment, ',');
		END
         
      EXEC spAddTicket
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
                               -- @Category = @Category,
							    @Category = @Cate,
                                @Unit = @Unit,
                                @Reason = @Reason,
                                @CustName = @CustName,
                                @custID = @custID, 
                                @EST = @EST,
                                @complDesc = @complDesc,
								@TicketIDOut =@TicketIDOut out,
								@AID = @AID,
                                @Who = @Who,
                                @sign= @sign,
                                @Reg = @Reg,
                                @OT = @OT,
                                @NT = @NT,
                                @TT = @TT,
                                @DT = @DT,
                                @Total = @Total,
                                @Charge = @Charge,
                                @Review = @Review, 
                                @remarks = @remarks,
								@Level =  @Level,
                                @Type = @Type,
								@job = @job,
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
								@IsRecurring  = @IsRecurring , 
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
                                @JobCode = @JobCode,
                                @ProjectTemplate = @ProjectTemplate,
                                @wage = @wage,
                                @fBy = @fBy,
								@RecurringDate  = @RecurringDate,
                                @Equipments = @Equipments,
								@UpdateTasks  = @UpdateTasks ,	
								@TaskCodes =@TaskCodes,				
					            @BT =@BT,
					            @Comments =@Comments,
					            @PartsUsed =@PartsUsed,
								@Zone=@Zone
			    
				--DELETE FROM @DataTable WHERE ID=@Counter
				DELETE FROM @Equipments

				SET @Counter=@Counter+1;
				  
			END
	SEt @Result=1
END
