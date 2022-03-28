CREATE PROCEDURE [dbo].[spVoidedTicket]

    @LocID int,
	@UpdatedBy  VARCHAR(50),
	@Tickets  int = 0

    AS

	       DECLARE  @ticketID int;

		   DECLARE  @tmpTickettbl table ( PK int IDENTITY(1,1) , TicketID int )		   

		   IF (@Tickets=0 and @LocID !=0)
		   BEGIN
		   INSERT INTO @tmpTickettbl (TicketID)
		   SELECT id FROM TicketO where LID=@LocID and Assigned <> 4     
		   END
		   ELSE
		   BEGIN
		   INSERT INTO @tmpTickettbl (TicketID)
		   SELECT id FROM TicketO where Assigned <> 4 and ID =  @Tickets 
		   END

		   DECLARE @ROW_Count int ;
           DECLARE @ROW_NO int =1;
           SELECT  @ROW_Count =max(PK) FROM  @tmpTickettbl WHERE PK is not null 


		   WHILE(@ROW_NO <= @ROW_Count)

           BEGIN  ----3

           PRINT(@ROW_NO)
  
   
        IF  EXISTS (SELECT 1 FROM @tmpTickettbl i WHERE i.pk=@ROW_NO )

	   BEGIN  ----2
	     
		 SELECT @ticketID=TicketID FROM @tmpTickettbl i WHERE i.pk=@ROW_NO

     -----------------DECLARE ----------------------

        DECLARE   
                    @LocTag          VARCHAR(50),
                    @LocAdd          VARCHAR(255),
                    @City            VARCHAR(50),
                    @State           VARCHAR(2),
                    @Zip             VARCHAR(100),
                    @Phone           VARCHAR(28),
                    @Cell            VARCHAR(50),
                    @Worker          VARCHAR(50),
                    @CallDt          DATETIME,
                    @SchDt           DATETIME ,
                    @Status          SMALLINT ,
                    @EnrouteTime     DATETIME ,
                    @Onsite          DATETIME ,
                    @Complete        DATETIME ,
                    @Category        VARCHAR(25),
                    @Unit            INT=0,
                    @Reason          VARCHAR(max),
                    @CustName        VARCHAR(50),
                    @custID          INT=0, 
                    @EST             NUMERIC(30, 2),
                    @complDesc       VARCHAR(max)='Voided',
                    @Reg             NUMERIC(30, 2)=0,
                    @OT              NUMERIC(30, 2)=0,
                    @NT              NUMERIC(30, 2)=0,
                    @TT              NUMERIC(30, 2)=0,
                    @DT              NUMERIC(30, 2)=0,
                    @Total           NUMERIC(30, 2)=0,
                    @Charge          INT=0,
                    @Review          INT=0,
                    @Who             VARCHAR(30),
                    @sign            VARBINARY(max),
                    @remarks         VARCHAR(max)='Voided',
                    @Type            INT,
                    @Custom1         VARCHAR(50),
                    @Custom2         VARCHAR(50),
                    @Custom3         VARCHAR(50),
                    @Custom4         VARCHAR(50),
                    @Custom5         VARCHAR(50),
                    @Custom6         TINYINT,
                    @Custom7         TINYINT,
                    @WorkOrder       VARCHAR(10),
                    @WorkComplete    INT=1,
                    @MiscExp         NUMERIC(30, 2),
                    @TollExp         NUMERIC(30, 2),
                    @ZoneExp         NUMERIC(30, 2),
                    @MileStart       INT=0,
                    @MileEnd         INT=0,
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
                    @job             INT=0,
                    @JobCode         VARCHAR(255)='',
                    @ProjectTemplate INT,
                    @wage            INT=0,
                    @fBy             VARCHAR(50),
                    @Equipments      AS TBLTYPEMULTIPLEEEQUIPMENTS,
                    @prospect        INT,
					@UpdateTasks smallint = 0 ,					
					@BT  NUMERIC(30, 2)=0,
					@Comments  varchar(1000)=null,
					@PartsUsed  varchar(100)=null,
					@TaskCodes as tblTypeTaskCodes,
					@IsCreateJob    INT=0,
					@Level    INT=1,
					@RegTrav NUMERIC(30, 2)=0, 
				    @OTTrav NUMERIC(30, 2)=0, 
				    @NTTrav NUMERIC(30, 2)=0, 
				    @DTTrav NUMERIC(30, 2)=0
            
			 
     -------------------SELECT--------------
 
            SELECT @prospect = LType,
                   @LocID = LID,
                   @LocTag = (SELECT Tag  FROM   loc  WHERE  loc = LID),
                   @LocAdd = (SELECT Address FROM   loc  WHERE  loc = LID),
                   @City = (SELECT City  FROM   loc  WHERE  loc = LID),
                   @State = (SELECT State  FROM   loc  WHERE  loc = LID),
                   @Zip = (SELECT Zip  FROM   loc WHERE  loc = LID),
                   @Phone = (select top 1 Phone from rol where ID=(select top 1 rol from loc where loc=o.LID)),
                   @Cell =  (select top 1 Cellular from rol where ID=(select top 1 rol from loc where loc=o.LID)),
                   @Worker = DWork,
                   @CallDt = o.CDate,
                   @SchDt = o.EDate,
                   @Status = 4,
                   @EnrouteTime   =getdate(),
                   @Onsite   =getdate(),
                   @Complete =getdate(),
                   @Category = o.Cat,
                   @Unit = o.LElev,
                   @Reason = o.fDesc,
                   @CustName = NULL,
                   @custID = o.Owner, 
                   @EST = o.Est,
                   @complDesc = 'Voided',
                   @Reg = 0,
                   @OT = 0,
                   @NT = 0,
                   @TT = 0,
                   @DT = 0,
                   @Total = 0,
                   @Charge = o.Charge,
                   @Review = 1,
                   @Who = o.Who, 
                   @remarks = (SELECT Remarks FROM   loc   WHERE  loc = LID),
                   @Type = o.Type,
                   @Custom1 = o.Custom1,
                   @Custom2 = o.Custom2,
                   @Custom3 = o.Custom3,
                   @Custom4 = o.Custom4,
                   @Custom5 = o.Custom5,
                   @Custom6 = o.Custom6,
                   @Custom7 = o.Custom7,
                   @WorkOrder = o.WorkOrder,
                   @WorkComplete = 0,
                   @MiscExp = 0,
                   @TollExp = 0,
                   @ZoneExp = 0,
                   @MileStart = 0,
                   @MileEnd = o.EMile,
                   @Internet = 0,
                   @Invoice = NULL,
                   @TransferTime = NULL,
                   @CreditHold = (SELECT Credit  FROM   loc   WHERE  loc = lid),
                   @DispAlert = (SELECT DispAlert  FROM   loc  WHERE  loc = lid),
                   @CreditReason = (SELECT CreditReason  FROM   loc WHERE  loc = lid),                   
                   @LastUpdatedBy = NULL,
                   @Contact = (SELECT Contact  FROM   rol    WHERE  id = (SELECT TOP 1 Rol FROM   Loc  WHERE  Loc = lid)),
                   @Recommendation = o.BRemarks,
                   @Customtick1 = o.CustomTick1,
                   @Customtick2 = o.CustomTick2,
                   @Customtick3 = o.CustomTick3,
                   @Customtick4 = o.CustomTick4,
                   @lat = (SELECT Lat  FROM   rol   WHERE  id = (SELECT TOP 1 Rol  FROM   Loc  WHERE  Loc = lid)),
                   @lng = (SELECT Lng  FROM   rol   WHERE  id = (SELECT TOP 1 Rol  FROM   Loc  WHERE  Loc = lid)),
                   @DefaultRoute = (SELECT Route  FROM   loc  WHERE  loc = lid),
                  @Customtick5 =(Case ISNUMERIC( o.CustomTick5) when 1 then CONVERT(numeric(30,2),  o.CustomTick5 ) else 0 end ) ,
                   @job = o.Job,
                   @JobCode = o.JobCode ,
                   @ProjectTemplate = NULL, 
                   @fBy = o.fBy, 
				   @Level=o.Level  
				   FROM   TicketO o    
				   WHERE  o.ID = @ticketID
              

        IF   EXISTS (SELECT  1  FROM TicketO  WHERE ID = @TicketID and Assigned <> 4)

        BEGIN ----1

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
			TimeCheckOutFlag ,
			Assigned
			)
                VALUES (@TicketID, 
				@CallDt, @SchDt, 
				@EnrouteTime, 
				@Onsite, 
				@Complete,
				@Category, 
				@Reason, 
				@EST, 
				(SELECT ID FROM tblWork WHERE fDesc = @Worker),
				@LocID, 
				@complDesc, 
				@Reg, 
				@OT, 
				@NT, 
				@TT, 
				@DT, 
				@Total,                   
                CASE WHEN (@Invoice = '') THEN @Charge ELSE 0 END, 
				@Review, 
				@Who, 
				@Type, 
				0, 
				@Unit, 
				@Recommendation,     
                @Custom2, 
				@Custom3, 
				@Custom6, 
				@Custom7, 
				@Custom1, 
				@Custom4, 
				@Custom5, 
				@WorkOrder, 
				@WorkComplete, 
				@MiscExp, 
				@TollExp,
				@ZoneExp, 
				@MileStart, 
				@MileEnd, 
				@Internet, 
                @job, 
				@Invoice, GETDATE(), 
				@TransferTime, 
				@QBServiceItem, 
				@QBPayrollItem, 
				@Cell, 
				@Customtick1, 
				@Customtick2, 
				@Customtick3, 
				@Customtick4, 
				@Customtick5, 
				(SELECT items FROM dbo.IDSplit(@JobCode, ':') WHERE row = 2), 
				(SELECT items FROM dbo.IDSplit(@JobCode, ':') WHERE row = 1), @wage, @fBy,
				(SELECT TOP 1 Recurring FROM TicketO WHERE ID = @TicketID), 
				(SELECT items FROM dbo.IDSplit(@JobCode, ':') WHERE row = 3), 
				@RegTrav, 
				@OTTrav, 
				@NTTrav, 
				@DTTrav, 
				@BT, @Comments, @PartsUsed, @Level, (SELECT TOP 1 DDate FROM TicketO WHERE ID = @TicketID),
				(SELECT TOP 1 TimeCheckOut FROM TicketDPDA WHERE ID = @TicketID),
				(SELECT TOP 1 TimeCheckOutFlag FROM TicketDPDA WHERE ID = @TicketID),
				6
				)          
        
		    DELETE FROM TicketO  WHERE ID = @TicketID and Assigned <> 4
		    	-- Update Logs for Contract
			INSERT INTO log2 (fUser,   Screen,  Ref,         Field,    OldVal,  NewVal)
			 
			SELECT       @UpdatedBy,  'Ticket' , @TicketID, 'Status' ,   'Open',   'Voided' 
		END-----1


		 END   ---2
     

  	SET @ROW_NO+=1; 

  END ----3
 
       