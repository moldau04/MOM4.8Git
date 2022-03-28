﻿
Create PROC [dbo].[spAddProjectTicket] @Workers      VARCHAR(max),
                                      @days         INT,
                                      @ProjectID    INT,
                                      @CallDt       DATETIME,
                                      @SchDt        DATETIME,
                                      @cat          VARCHAR(50),
                                      @Reason       VARCHAR(max),
                                      @Caller       VARCHAR(30),
                                      @Status       INT,
                                      @EST          NUMERIC(30, 2),
                                      @DispAlert    SMALLINT,
                                      @CreditReason TEXT,
                                      @Unit         INT,
                                      @Equipments   AS TBLTYPEMULTIPLEEEQUIPMENTS readonly
									   
AS
    DECLARE @_Reason VARCHAR(max); 
    SET @_Reason=@Reason;

    DECLARE @LocID      INT,
            @LocTag     VARCHAR(50),
            @LocAdd     VARCHAR(100),
            @City       VARCHAR(50),
            @State      VARCHAR(2),
            @Zip        VARCHAR(100),
            @Worker     VARCHAR(50),
            @custID     INT,
            @Department INT,
			@count int = 0 ,
			@WorkOrder VARCHAR(10)  --Added var

    SELECT @LocID = j.Loc,
           @LocTag = l.Tag,
           @LocAdd = l.Address,
           @City = l.City,
           @State = l.State,
           @Zip = l.Zip,
           @custID = l.Owner,
           @Department = j.Type
    FROM   job j
           INNER JOIN loc l
                   ON l.loc = j.Loc
    WHERE  j.ID = @ProjectID

    UPDATE loc
    SET    DispAlert = @DispAlert,
           CreditReason = @CreditReason
    WHERE  Loc = @LocID

    IF( Isnull(@DispAlert, 0) = 1 )
      BEGIN
          UPDATE loc
          SET    CreditReason = @CreditReason
          WHERE  Loc = @LocID
      END

    IF( @Workers = '' )
      BEGIN
          SET @Workers= '-unassign-'
      END

    DECLARE db_cursor CURSOR FOR
      SELECT items
      FROM   dbo.Split(@Workers, ',')

    OPEN db_cursor

    FETCH NEXT FROM db_cursor INTO @Worker

    WHILE @@FETCH_STATUS = 0
      BEGIN
	      SET @count = @count + 1
          DECLARE @day INT = 0
          DECLARE @TicketID INT
          DECLARE @fwork VARCHAR(50)

          SELECT @TicketID = Max([NewID]) + 1
          FROM   (SELECT Isnull(Max(TicketO.ID), 0) AS [NewID]
                  FROM   TicketO
                  UNION ALL
                  SELECT Isnull(Max(TicketD.ID), 0) AS [NewID]
                  FROM   TicketD) A

	    --For Multiple Workorder
		IF(@count > 1)
	     Begin
			IF( @WorkOrder = '' )        
			  BEGIN        
				  SET @WorkOrder = (Select MAX(ID) from TicketO)        
			  END
	     End
		Else
		 Begin
			SET @WorkOrder = @TicketID
		 End

          WHILE ( @day < @days )
            BEGIN
                DECLARE @Schdays DATETIME

                SET @Schdays = Dateadd(day, @day, @SchDt)

                IF( @Worker = '-unassign-' )
                  BEGIN
                      SET @Worker=''
                  END

                IF( @day > 0 )
                  BEGIN
                      SET @Reason=@cat
                  END
                ELSE
                  BEGIN
                      SET @Reason=@_Reason
                  END

                EXEC Spaddticket
                  @LocID,
                  NULL,--loctag
                  @LocAdd,
                  @City,
                  @State,
                  @Zip,
                  NULL,--phone
                  NULL,--cell
                  @Worker,
                  @CallDt,
                  @Schdays,
                  @Status,
                  NULL,--@EnrouteTime
                  NULL,--@Onsite
                  NULL,--@Complete
                  @cat,
                  @Unit,
                  @Reason,
                  NULL,--@CustName
                  @custID,
                  @EST,
                  NULL,--@complDesc
                  0,--@TicketIDOut
                  NULL,--@AID
                  @Caller,
                  NULL,--@sign
                  0.00,--reg
                  0.00,--ot
                  0.00,--nt
                  0.00,--tt
                  0.00,--dt
                  0.00,--total
                  0,--@Charge
                  0,--@Review
                  '',--@Remarks
                  10,--@Level
                  @Department,
                  @ProjectID,
                  '',--@Custom1
                  '',--@Custom2
                  '',--@Custom3
                  '',--@Custom4
                  '',--@Custom5
                  0,--@Custom6
                  0,--@Custom7
                  @WorkOrder,  --@TicketID
                  1,-- @WorkComplete
                  0,-- @MiscExp
                  0,-- @TollExp
                  0,-- @ZoneExp
                  0,-- @MileStart
                  0,-- @MileEnd
                  1,-- @Internet
                  '',-- @Invoice
                  0,--@ @TransferTime
                  0,-- @CreditHold
                  0,--  @DispAlert
                  '',-- @CreditReason
                  1,-- @IsRecurring
                  NULL,-- @QBServiceItem
                  NULL,-- @QBPayrollItem
                  '',--@ @LastUpdatedBy
                  '',--@ @Contact
                  '',--@ @Recommendation
                  '',--@ @Customtick1
                  '',--@ @Customtick2
                  '',--@ @Customtick3
                  '',--@ @Customtick4
                  '',-- @lat
                  '',-- @lng
                  0,-- @DefaultRoute
                  NULL,-- @Customtick5
                  NULL,-- @JobCode
                  NULL,-- @ProjectTemplate
                  NULL,-- @wage
                  @Caller,
                  NULL,--@RecurringDate     
                  @Equipments,
                  0--@UpdateTasks

                --tblTypeTaskCodes ,
                --0,--@BT
                --null ,--@Comments
                --null --@PartsUsed  

                IF @@ERROR <> 0
                   AND @@TRANCOUNT > 0
                  BEGIN
                      RAISERROR ('Error Occured',16,1)

                      CLOSE db_cursor

                      DEALLOCATE db_cursor

                      RETURN
                  END

                SET @day = @day + 1
            END

          FETCH NEXT FROM db_cursor INTO @Worker 
		 
      END

    CLOSE db_cursor

    DEALLOCATE db_cursor 

	select @TicketID
GO

