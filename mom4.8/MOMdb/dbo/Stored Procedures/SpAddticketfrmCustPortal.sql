CREATE PROC [dbo].[SpAddticketfrmCustPortal] @Equipments AS TBLTYPEMULTIPLEEEQUIPMENTS readonly,
                                            @Unit       INT,
                                            @Worker     VARCHAR(50)='-unassign-',
                                            @CallDt     DATETIME,
                                            @SchDt      DATETIME,
                                            @cat        VARCHAR(50),
                                            @Reason     VARCHAR(max),
                                            @Caller     VARCHAR(30),
                                            @cellerphone VARCHAR(28),
                                            @EST        NUMERIC(30, 2)=1.00,
                                            @LocID      INT,
                                            @fby        VARCHAR(50)='Portal',
                                            @Level      INT=99
AS
    DECLARE @TicketID INT
    DECLARE @_Reason VARCHAR(max);

    SET @_Reason=@Reason;

    DECLARE @LocTag VARCHAR(50),
            @LocAdd VARCHAR(100),
            @City   VARCHAR(50),
            @State  VARCHAR(2),
            @Zip    VARCHAR(100),
            @custID INT

    SELECT @LocTag = l.Tag,
           @LocAdd = l.Address,
           @City = l.City,
           @State = l.State,
           @Zip = l.Zip,
           @custID = l.Owner
    FROM   loc l
    WHERE  l.Loc = @LocID

    SELECT @TicketID = Max([NewID]) + 1
    FROM   (SELECT Isnull(Max(TicketO.ID), 0) AS [NewID]
            FROM   TicketO
            UNION ALL
            SELECT Isnull(Max(TicketD.ID), 0) AS [NewID]
            FROM   TicketD) A

    EXEC Spaddticket
      @LocID,
      NULL,--loctag
      @LocAdd,
      @City,
      @State,
      @Zip,
      null,--phone
      @cellerphone,--cell
      @Worker,
      @CallDt,
      @SchDt,
      0,
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
      @Caller,--@who
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
      @Level,--@Level
      NULL,--@type
      NULL,
      '',--@Custom1
      '',--@Custom2
      '',--@Custom3
      '',--@Custom4
      '',--@Custom5
      0,--@Custom6
      0,--@Custom7
      @TicketID,
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
      @fby,
      NULL,--@RecurringDate     
      @Equipments,
      0--@UpdateTasks
     
    SELECT @TicketID 
