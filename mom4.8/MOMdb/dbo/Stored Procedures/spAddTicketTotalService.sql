   
CREATE PROCEDURE [dbo].[spAddTicketTotalService]   
 @LocID        INT,    
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
 @EST          NUMERIC(30, 2),    
 @complDesc    TEXT,    
 @TicketIDOut  INT output,    
 @AID          UNIQUEIDENTIFIER,    
 @Who          VARCHAR(30),    
 @sign         IMAGE,    
 @Reg          NUMERIC(30, 2),    
 @OT           NUMERIC(30, 2),    
 @NT           NUMERIC(30, 2),    
 @TT           NUMERIC(30, 2),    
 @DT           NUMERIC(30, 2),    
 @Total        NUMERIC(30, 2),    
 @Charge       INT,    
 @Review       INT,    
 @Remarks      text,    
 @Level        INT,    
 @Type         INT,    
 @job          INT = NULL,    
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
 @Invoice      VARCHAR(50),    
 @TransferTime int,    
 @CreditHold tinyint,    
    @DispAlert tinyint,    
    @CreditReason text,    
    @IsRecurring tinyint,    
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
    @JobCode varchar(10),    
    @ProjectTemplate int,    
    @wage int,    
    @fby varchar(50),    
    @RecurringDate datetime = null ,             
    @Equipments as tblTypeMultipleEequipments readonly ,    
    @UpdateTasks smallint = 0 ,    
    @TaskCodes as tblTypeTaskCodes readonly ,    
    @BT  NUMERIC(30, 2)=0,    
    @Comments  varchar(1000)=null,    
    @PartsUsed  varchar(100)=NULL,    
    @Zone int = null    
AS    
    DECLARE @TicketID INT    
    DECLARE @Rol INT    
    DECLARE @Nature SMALLINT = 0    
    DECLARE @Ltype SMALLINT = 0    
    DECLARE @ProspectID INT    
    DECLARE @DucplicateProspectName INT    
    declare @prospectcreate int = 0    
 declare @Phase int     
       
    BEGIN TRANSACTION    
          
    SELECT @TicketID = Max([NewID]) + 1    
    FROM   (SELECT Isnull(Max(TicketO.ID), 0) AS [NewID]    
            FROM   TicketO    
            UNION ALL    
            SELECT Isnull(Max(TicketD.ID), 0) AS [NewID]    
            FROM   TicketD) A    
    
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
                
            set @prospectcreate = 1    
                
                SELECT @DucplicateProspectName = Count(1)    
                FROM   Rol r    
                       INNER JOIN Prospect p    
                               ON p.Rol = r.ID    
                WHERE  Name = @CustName    
    
                IF( @DucplicateProspectName <> 0 )    
                  BEGIN    
                      RAISERROR ('Prospect name already exists, please use different Prospect name !',16,1)    
    
                      RETURN    
                  END    
    
                SELECT @ProspectID = isnull(Max(ID) ,0)+ 1    
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
                             Lat,Lng    
                             )    
                VALUES      ( @CustName,    
                              @LocAdd,    
                              @City,    
                              @State,    
                              @Zip,    
                              @Phone,    
                              @Contact,    
                              'Created on Ticket# '+convert(varchar(50), @TicketID) +SPACE(2)+ convert(varchar(max), @Remarks),    
                              3,    
                              0,    
                              0,    
                              0,    
                              Getdate(),    
                              Getdate(),    
                              1,    
                              @Cell,    
                              'United States',    
                              @lat, @lng    
                               )    
    
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
        CreatedBy,    
       LastUpdatedBy,    
       CustomerName    
                       )    
                VALUES      ( @ProspectID,    
                              @Rol,    
                              '',    
                              1,    
                              0,    
                              Getdate(),    
                              Cast(Cast('12/30/1899' AS DATE) AS DATETIME)    
                              + Cast( Cast(Getdate() AS TIME)as datetime),    
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
                    
                --update PType set [Count] = [Count]+1 where [Type] = 'General'    
                    
                    
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
    
 declare @equipcounts int = 0    
 select @equipcounts = count(1) from @Equipments    
 if(@equipcounts = 1)    
 begin    
 select @Unit = elev_id from @Equipments    
 end    
 else    
 if(@equipcounts > 1 )    
 begin    
 select top 1 @Unit = elev_id from @Equipments    
 end    
 else if(@equipcounts = 0 )    
 begin    
 set @Unit = 0    
 end    
    
    
    /* whene ticket status is other than completed */    
    --IF( @Status <> 4 )    
    --  BEGIN    
          IF NOT EXISTS(SELECT 1    
                        FROM   TicketO    
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
                             CustomTick1 ,    
        CustomTick2 ,    
        CustomTick3 ,    
        CustomTick4 ,    
        CustomTick5,    
        Recurring ,  
        Charge    
               
                             )    
                VALUES      ( @TicketID,    
                              --CONVERT(varchar(50), @LocID),    
                              CASE    
                                WHEN ( @custID IS NULL ) THEN CONVERT(VARCHAR(50), @LocID)    
                                ELSE (SELECT ID    
                                      FROM   Loc    
                                      WHERE  Loc = @locid)    
                              END,    
                              ----@LocTag,    
                              ----(select top 1 ( select top 1  name from Rol where ID=l.Rol) as name  from loc l where Loc=@locid),    
                              CASE    
                              WHEN ( @custID IS NULL ) THEN (SELECT r.Name    
                                                               FROM   Prospect p    
                                                                      INNER JOIN Rol r    
                                                                              ON r.ID = p.Rol    
                                                               WHERE  p.ID = @LocID)    
                                ELSE (SELECT Tag    
                                      FROM   Loc    
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
                               FROM   tblWork    
                               WHERE  fDesc = @Worker),    
                              @Ltype,    
                              0,    
                              @Who,    
                              @Type,    
                              newid(),    
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
                              @fby ,    
                              @QBServiceItem,    
                              @QBPayrollItem,    
                              @Customtick1 ,    
       @Customtick2 ,    
       @Customtick3 ,    
       @Customtick4 ,    
       @Customtick5,    
       @RecurringDate,  
       @Charge     
                  
                  )    
            END    
    --  END    
    --ELSE     
 IF( @Status = 4 )    
      BEGIN    
          IF NOT EXISTS(SELECT 1    
                        FROM   TicketDPDA    
                        WHERE  ID = @TicketID)    
            BEGIN    
    
            INSERT INTO TicketDPDA    
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
                            CPhone,           
      Phase,    
      WageC ,    
      fBy       
      --,           
      --RegTrav,    
      --OTTrav,    
      --NTTrav,    
      --DTTrav                     
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
                            @job,          
       @Cell,           
      (select items from dbo.IDSplit(@JobCode,':') where row=1),    
      @wage,    
      @fBy    
      --,            
      --@RegTrav,    
      --@OTTrav,    
      --@NTTrav,    
      --@DTTrav       
       )    
                                  
            IF @@ERROR <> 0    
       AND @@TRANCOUNT > 0    
      BEGIN    
          RAISERROR ('Error Occured',16,1)    
    
          ROLLBACK TRANSACTION    
    
          RETURN    
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
                 declare @defaultuser varchar(50)    
     select top 1 @defaultuser =  fuser from tbluser where DefaultWorker = 1      
    
     declare @oppremarks varchar(500)    
         
     if((select count(1) from @Equipments)=1)    
     begin    
     set @oppremarks = @Recommendation + CHAR(13)+CHAR(10) +'Equipment : ' +(select top 1 isnull((select top 1 isnull(unit,'') +', '+isnull(fDesc,'') from elev where id = elev_id),'') from @Equipments)    
     end    
     else    
     begin    
     set @oppremarks = @Recommendation    
     end        
    
    exec spAddOpportunity 0,@oppname ,@Rol,3,1,@oppremarks,@SchDt,0,0,'','','',0,@defaultuser,@LastUpdatedBy,0,@TicketID    
    IF @@ERROR <> 0    
       AND @@TRANCOUNT > 0    
      BEGIN    
          RAISERROR ('Error Occured',16,1)    
    
          ROLLBACK TRANSACTION    
    
          RETURN    
      END    
          
   end    
            end      
                            
  DECLARE @WorkerID INT    
          SET @WorkerID= (SELECT TOP 1 ID    
                          FROM   tblWork    
                          WHERE  fDesc = @Worker)    
    
  IF( @sign IS NOT NULL )    
     BEGIN    
      EXEC spInsertSignDPDA    
     @WorkerID,    
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
               
    END    
      END    
    
    IF @@ERROR <> 0    
       AND @@TRANCOUNT > 0    
      BEGIN    
          RAISERROR ('Error Occured',16,1)    
    
          ROLLBACK TRANSACTION    
    
          RETURN    
      END    
    
    
   delete from multiple_equipments where ticket_id = @TicketID    
    
   insert into multiple_equipments (ticket_id, elev_id, labor_percentage) select @TicketID, elev_id, labor_percentage from @Equipments    
    
    if(@UpdateTasks = 1)    
begin    
if(@job is not null)    
begin    
 if(@job <> 0)    
 begin    
  delete from Ticket_Task_Codes where job = @job     
  insert into Ticket_Task_Codes     
  (task_code,Type, job,Category,username,dateupdated, ticket_id, default_code)    
  select task_code, 1, @job,Category,username,dateupdated,case ticket_id when 0 then @TicketID else ticket_id end,1 from @TaskCodes      
 end    
end    
end    
    
    
    /* update location address */    
        
    IF( @custID IS NOT NULL and @IsRecurring = 0 )    
      BEGIN    
          UPDATE Loc    
          SET    Address = @LocAdd,    
                 City = @City,    
                 State = @State,    
                 Zip = @Zip,    
                 Remarks = @Remarks,    
                 DispAlert=@DispAlert,    
     Credit=@CreditHold,    
     CreditReason=@CreditReason ,    
     Route=@DefaultRoute,    
     Zone=@Zone    
          WHERE  Loc = @locID            
          update Job set Custom20=@DefaultRoute where Loc=@locID    
        update Rol set Phone= @Phone, Cellular=@Cell, LastUpdateDate=GETDATE(), Contact=@Contact, Lat =@lat , Lng=@lng     
        where ID=(select top 1 Rol from Loc where Loc = @LocID)    
      
      END    
    
    IF @@ERROR <> 0    
       AND @@TRANCOUNT > 0    
      BEGIN    
          RAISERROR ('Error Occured',16,1)    
    
          ROLLBACK TRANSACTION    
    
          RETURN    
      END    
          
      IF( @custID IS NULL and @IsRecurring = 0 and @prospectcreate=0)    
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
   Contact=@Contact ,     
   Lat =@lat ,     
   Lng=@lng     
   where ID=(select top 1 Rol from Prospect where ID = @LocID)    
      END    
    
    IF @@ERROR <> 0    
       AND @@TRANCOUNT > 0    
      BEGIN    
          RAISERROR ('Error Occured',16,1)    
    
          ROLLBACK TRANSACTION    
    
          RETURN    
      END    
    
    /* insert signature image */    
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
    
    SELECT @TicketID    
    
    COMMIT TRANSACTION 