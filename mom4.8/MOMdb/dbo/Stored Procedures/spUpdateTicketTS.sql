CREATE PROCEDURE [dbo].[spUpdateTicketTS] @LocID        INT,
                                       @LocTag       VARCHAR(50),
                                       @LocAdd       VARCHAR(255),
                                       @City         VARCHAR(50),
                                       @State        VARCHAR(2),
                                       @Zip          VARCHAR(100),
                                       @Phone        VARCHAR(28),
                                       @Cell         VARCHAR(50),
                                       @Worker       VARCHAR(50),
                                       @CallDt       DATETIME,
                                       @SchDt        DATETIME,
                                       @Status       SMALLINT,
                                       @EnrouteTime  DATETIME,
                                       @Onsite       DATETIME,
                                       @Complete     DATETIME,
                                       @Category     VARCHAR(25),
                                       @Unit         INT,
                                       @Reason       TEXT,
                                       @CustName     VARCHAR(50),
                                       @custID       INT,
                                       @TicketID     INT,
                                       @EST          NUMERIC(30, 2),
                                       @complDesc    TEXT,
                                       @Reg          NUMERIC(30, 2),
                                       @OT           NUMERIC(30, 2),
                                       @NT           NUMERIC(30, 2),
                                       @TT           NUMERIC(30, 2),
                                       @DT           NUMERIC(30, 2),
                                       @Total        NUMERIC(30, 2),
                                       @Charge       INT,
                                       @Review       INT,
                                       @Who          VARCHAR(30),
                                       @sign         IMAGE,
                                       @remarks      text,
                                       @Type         INT,
                                       @Custom1      VARCHAR(50),
                                       @Custom2      VARCHAR(50),
                                       @Custom3      VARCHAR(50),
                                       @Custom4      VARCHAR(50),
                                       @Custom5      VARCHAR(50),
                                       @Custom6      TINYINT,
                                       @Custom7      TINYINT,
                                       @WorkOrder    VARCHAR(10),
                                       @WorkComplete INT,
                                       @MiscExp      NUMERIC(30, 2),
                                       @TollExp      NUMERIC(30, 2),
                                       @ZoneExp      NUMERIC(30, 2),
                                       @MileStart    INT,
                                       @MileEnd      INT,
                                       @Internet     SMALLINT,
                                       @Invoice      varchar(50),
                                       @TransferTime int,
                                       @CreditHold tinyint,
									   @DispAlert tinyint,
									   @CreditReason varchar(100),
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
									@Customtick5  NUMERIC(30, 2),
									@job int,
									@JobCode varchar(10),
									@ProjectTemplate int,
									@fby varchar(50)
									
									
AS
    DECLARE @Rol INT
    DECLARE @Nature SMALLINT = 0
    DECLARE @Ltype SMALLINT = 0
    DECLARE @ProspectID INT
    declare @DucplicateProspectName int
    declare @prospectcreate int = 0

    --IF( @Invoice = 0 )
    --  BEGIN
    --      SET @Invoice = NULL
    --  END
    
        BEGIN TRANSACTION

/********** Wnen adding prospect ************/
    IF( @custID = 0 )
      BEGIN
          SET @custID = NULL
          SET @Nature = 1
          SET @Ltype = 1

          IF( @LocID = 0 )
            BEGIN
            
            set @prospectcreate = 1
            
             select @DucplicateProspectName = COUNT(1) from Rol r inner join Prospect p on p.Rol=r.ID where Name =@CustName 

				if(@DucplicateProspectName <> 0)
				begin

				 RAISERROR ('Prospect name already exists, please use different Prospect name !', 16, 1)  
				 RETURN

				end
            
                SELECT @ProspectID = isnull(Max(ID) ,0) + 1
                FROM   Prospect

                INSERT INTO Rol
                            (Name,
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
                VALUES      ( @CustName,
                              @LocAdd,
                              @City,
                              @State,
                              @Zip,
                              @Phone,
                              @Contact,
                             'Created on Ticket# '+ CAST( @TicketID as varchar(50)) + CHAR(13) + SPACE(2)+CONVERT(varchar(max), @Remarks),
                              3,
                              0,
                              0,
                              0,
                              Getdate(),
                              Getdate(),
                              1,
                              @Cell,
                              'United States' ,
                              @lat,@lng)

                IF @@ERROR <> 0
                   AND @@TRANCOUNT > 0
                  BEGIN
                      RAISERROR ('Error Occured',16,1)

                      ROLLBACK TRANSACTION

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
							LastUpdatedBy,
							CreatedBy,
							CustomerName
							)
                VALUES      ( @ProspectID,
                              @Rol,
                              '',
                              1,
                              0,
                              Getdate(),
                              Cast(Cast('12/30/1899' AS DATE) AS DATETIME)
                              +cast( Cast(Getdate() AS TIME)as datetime),
                              0,
                              Getdate(),
                              0 ,
                              GETDATE(),
                              GETDATE(),
                              @LastUpdatedBy,
                              @LastUpdatedBy,
                              @CustName
                              )

                IF @@ERROR <> 0
                   AND @@TRANCOUNT > 0
                  BEGIN
                      RAISERROR ('Error Occured',16,1)

                      ROLLBACK TRANSACTION

                      RETURN
                  END

                SET @LocID = @ProspectID
                
                --update PType set [Count] = [Count]+1 where [Type] ='General'
                
                
                if not exists(select 1 from Phone where Rol =@Rol and fDesc = @contact)
                 begin 
                 insert into Phone
				 (
				 Rol,fDesc,Phone,Cell
				 )
				 values
				 (
				 @Rol,@contact,@phone,@cell
				 )
                 end    
                 
                  IF @@ERROR <> 0
                   AND @@TRANCOUNT > 0
                  BEGIN
                      RAISERROR ('Error Occured',16,1)

                      ROLLBACK TRANSACTION

                      RETURN
                  END 
                 
            END
      END

    IF( @WorkOrder = '' )
      BEGIN
          SET @WorkOrder = @TicketID
      END



/**********  When staus is other than completed **********/
    IF( @Status <> 4 )
      BEGIN
          UPDATE TicketO
          SET    
          --LDesc1=CONVERT(varchar(50), @LocID),
          LDesc1 = CASE
                            WHEN ( @custID is NULL ) THEN CONVERT(varchar(50), @LocID)
                            ELSE (SELECT ID
                                  FROM   Loc
                                  WHERE  Loc = @locid)
                          END,
                 ----LDesc2=@LocTag,
                 ----LDesc2=(select top 1 ( select top 1  name from Rol where ID=l.Rol) as name  from loc l where Loc=@locid),
                 LDesc2 = CASE
                            WHEN ( @custID is NULL ) THEN (SELECT r.Name
                                                          FROM   Prospect p
                                                                 INNER JOIN Rol r
                                                                         ON r.ID = p.Rol
                                                          WHERE  p.ID = @LocID)
                            ELSE (SELECT Tag
                                  FROM   Loc
                                  WHERE  Loc = @locid)
                          END,
                 LDesc3 = @LocAdd,
                 LDesc4 = @City + ',' + Space(1) + @State + ',' + Space(1) + @Zip,
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
                 fWork = (SELECT id
                          FROM   tblWork
                          WHERE  fDesc = @Worker),
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
                 QBPayrollItem=@QBPayrollItem,
                 QBServiceItem=@QBServiceItem,
                 CustomTick1=@Customtick1,
                 CustomTick2=@Customtick2,
                 CustomTick3=@Customtick3,
                 CustomTick4=@Customtick4,
                 CustomTick5=@Customtick5,
                 Job=@job,
                 fBy=@fby
          WHERE  ID = @TicketID

          IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END
      END

    IF( @Status = 4 )
      BEGIN

 
          IF NOT EXISTS(SELECT 1
                        FROM   TicketD
                        WHERE  ID = @TicketID)
            BEGIN
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
                             CPhone    ,
                             CustomTick1 ,
							CustomTick2 ,
							CustomTick3 ,
							CustomTick4  ,
							CustomTick5 ,
							JobCode        ,
							fBy               
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
                               FROM   tblWork
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
                              @Recommendation,--@remarks,
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
                              --(SELECT TOP 1 Job
                              -- FROM   TicketO
                              -- WHERE  ID = @TicketID),
                              @job,
                              @Invoice,
                              GETDATE(),
                              @TransferTime,
                             @QBServiceItem,
                             @QBPayrollItem,
                             @Cell,
                              @Customtick1 ,
								@Customtick2 ,
								@Customtick3 ,
								@Customtick4 ,
								@Customtick5,
								@JobCode,
								@fby
                               )
            END
          ELSE
            BEGIN
                UPDATE TicketD
                SET    CDate = @CallDt,
                       EDate = @SchDt,
                       TimeRoute = @EnrouteTime,
                       TimeSite = @Onsite,
                       TimeComp = @Complete,
                       Cat = @Category,
                       fDesc = @Reason,
                       Est = @EST,
                       fWork = (SELECT ID
                                FROM   tblWork
                                WHERE  fDesc = @Worker),
                       Loc = @LocID,
                       DescRes = @complDesc,
                       Reg = @Reg,
                       OT = @OT,
                       NT = @NT,
                       TT = @TT,
                       DT = @DT,
                       Total = @Total,
                       Charge = ( CASE
                                    WHEN ( @Invoice = '' ) THEN
                                      CASE
                                        WHEN ( Isnull(Invoice, 0) = 0 ) THEN @Charge
                                        ELSE 0
                                      END
                                    ELSE 0
                                  END ),
                       ClearCheck = @Review,
                       Who = @Who,
                       Elev = @Unit,
                       BRemarks =@Recommendation,-- @remarks,
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
                       ManualInvoice=@Invoice,
                       LastUpdateDate=GETDATE(),
                       TransferTime=@TransferTime,
                     QBServiceItem=@QBServiceItem,
                     QBPayrollItem=@QBPayrollItem,
                     CPhone=@Cell,
                      CustomTick1=@Customtick1,
                 CustomTick2=@Customtick2,
                 CustomTick3=@Customtick3,
                 CustomTick4=@Customtick4,
                 CustomTick5=@Customtick5,
                 Job=@job,
                 JobCode=@JobCode,
                 fBy=@fby
                WHERE  ID = @TicketID
            END
            
            if(RTRIM(LTRIM(@Recommendation ))<>'')
            begin
            if not exists(select 1 from Lead where TicketID=@TicketID)
            begin            
            
				declare @oppname varchar(75)
				set @oppname ='Ticket# '+convert(varchar(67),@TicketID)
            							 
                if ( @custID IS NULL ) begin 
                SELECT @Rol = Rol FROM Prospect WHERE  ID = @LocID
                end
                ELSE begin
                SELECT @Rol =Rol FROM   Loc WHERE  Loc = @LocID
				END
                        
				exec spAddOpportunity 0,@oppname ,@Rol,3,1,@Recommendation,@SchDt,0,0,'','','',0,'',@LastUpdatedBy,0,@TicketID
			IF @@ERROR <> 0
			   AND @@TRANCOUNT > 0
			  BEGIN
				  RAISERROR ('Error Occured',16,1)

				  ROLLBACK TRANSACTION

				  RETURN
			  END
      
			end
            end

          IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END

/************ when ticket is completed the ticket data is transferred to TicketD table and the same ticket is deleted from TicketO and TicketDPDA tables *******************/
          DELETE FROM TicketDPDA
          WHERE  ID = @TicketID

          IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END

          DELETE FROM TicketO
          WHERE  ID = @TicketID

          IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END

          DECLARE @WorkerID INT

          SET @WorkerID= (SELECT TOP 1 ID
                          FROM   tblWork
                          WHERE  fDesc = @Worker)

          EXEC Spinsertsign
            @WorkerID,
            @TicketID

          IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END
      END

/******* update address in location *********/
    IF( @custID IS NOT NULL )
      BEGIN
          UPDATE Loc
          SET    Address = @LocAdd,
                 City = @City,
                 State = @State,
                 Zip = @Zip,
                 Remarks=@remarks,
                 DispAlert=@DispAlert,
				 Credit=@CreditHold,
				 CreditReason=@CreditReason,
				 Route=@DefaultRoute
          WHERE  Loc = @locID
          
          update Rol set Phone= @Phone, Cellular=@Cell, LastUpdateDate=GETDATE(), Contact=@Contact , Lat =@lat , Lng=@lng where ID=(select top 1 Rol from Loc where Loc = @LocID)
      END

    IF @@ERROR <> 0
       AND @@TRANCOUNT > 0
      BEGIN
          RAISERROR ('Error Occured',16,1)

          ROLLBACK TRANSACTION

          RETURN
      END
      
      IF( @custID IS NULL and @prospectcreate=0 )
      BEGIN
			UPDATE Prospect
			SET    LastUpdateDate=GETDATE(), LastUpdatedBy=@LastUpdatedBy 
			WHERE  ID = @LocID
          
			update Rol set 
			Address = @LocAdd,
			City = @City,
			State = @State,
			Zip = @Zip,
			Remarks = @Remarks,
			Phone= @Phone, 
			Cellular=@Cell, 
			LastUpdateDate=GETDATE(), 
			Contact=@Contact 
			, Lat =@lat , Lng=@lng
			where ID=(select top 1 Rol from Prospect where ID = @LocID)
      END

    IF @@ERROR <> 0
       AND @@TRANCOUNT > 0
      BEGIN
          RAISERROR ('Error Occured',16,1)

          ROLLBACK TRANSACTION

          RETURN
      END

    --else
    --	begin
    --		update TicketD set
    --		CDate=@CallDt,
    --		EDate=@SchDt,
    --		TimeRoute=@EnrouteTime,
    --		TimeSite=@Onsite,
    --		TimeComp=@Complete,
    --		Cat=@Category,
    --		fDesc=@Reason,
    --		Est=@EST,
    --		fWork=(select ID from tblWork where fDesc=@Worker),
    --		Loc=@LocID,
    --		DescRes=@complDesc,
    --		Reg=@Reg,
    --		OT=@OT,
    --		NT=@NT, 
    --		TT=@TT,
    --		DT=@DT,
    --		Total=@Total,
    --		Charge=@Charge
    --		where ID=@TicketID
    --	end
    --end
    
    /****** Insert signature ***********/
    IF( @sign IS NOT NULL )
      BEGIN
          EXEC Spinsertticketsign
            @TicketID,
            @sign
      END

    IF @@ERROR <> 0
       AND @@TRANCOUNT > 0
      BEGIN
          RAISERROR ('Error Occured',16,1)

          ROLLBACK TRANSACTION

          RETURN
      END

    COMMIT TRANSACTION

	select @job
