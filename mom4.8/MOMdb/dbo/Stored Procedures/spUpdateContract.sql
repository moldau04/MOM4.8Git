CREATE PROCEDURE [dbo].[spUpdateContract]@loc              INT,
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
                                         @Job              INT,
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
                                         @SPRemarks        TEXT ='',
                                         @IsRenewalNotes   SMALLINT=0,
                                         @RenewalNotes     TEXT='',
										 @Detail           SMALLINT,
										 @UpdatedBy varchar(100) ,
										 @ContractLength int
AS
    BEGIN TRANSACTION

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

    EXEC spUpdateContractLogs 
          @loc             
        , @owner           
        , @date            
        , @Status          
        , @Creditcard      
        , @Remarks         
        , @BStart          
        , @Bcycle          
        , @BAmt            
        , @SStart          
        , @Cycle           
        , @SWE             
        , @Stime           
        , @Sday            
        , @SDate           
        , @ElevJobData     
        , @Route           
        , @Job             
        , @hours           
        , @fdesc           
        , @CType           
        , @ExpirationDate  
        , @ExpirationFreq  
        , @Expiration      
        , @ContractBill    
        , @CustomerBill    
        , @Central         
        , @Chart           
        , @JobT            
        , @EscalationType  
        , @EscalationCycle 
        , @EscalationFactor
        , @EscalationLast  
        , @CustomItems     
        , @BillRate        
        , @RateOT          
        , @RateNT          
        , @RateDT          
        , @RateTravel      
        , @Mileage         
        , @PO              
        , @SPHandle        
        , @SPRemarks       
        , @IsRenewalNotes  
        , @RenewalNotes    
        , @Detail          
        , @UpdatedBy
		, @ContractLength

    DECLARE @tblCustomFieldsId INT
    DECLARE @tblTabID INT
    DECLARE @Label VARCHAR(50)
    DECLARE @TabLine SMALLINT
    DECLARE @Value VARCHAR(50)
    DECLARE @Format SMALLINT

    UPDATE Job
    SET    Loc = @loc,
           Owner = @owner,
           fDate = @date,
           Status = @Status,
           CreditCard = @Creditcard,
           Remarks = @Remarks,
           Custom20 = @route,
           fDesc = @fdesc,
           CType = @CType,
           LastUpdateDate = Getdate( ),
           BillRate = @BillRate,
           RateOT = @RateOT,
           RateNT = @RateNT,
           RateDT = @RateDT,
           RateTravel = @RateTravel,
           RateMileage = @Mileage,
           PO = @PO,
           SPHandle = @SPHandle,
           SRemarks = @SPRemarks,
		   IsRenewalNotes = @IsRenewalNotes,
           RenewalNotes = @RenewalNotes ,
		   GLRev=(select isnull(l.InvID,GLRev) from ltype l  where l.type=@CType)
    WHERE  ID = @job

    IF @@ERROR <> 0      AND @@TRANCOUNT > 0 
	   
	   BEGIN

          RAISERROR ('Error Occured',16,1)

          ROLLBACK TRANSACTION

          RETURN
      END

    UPDATE Contract
    SET    BStart = @BStart,
           BCycle = @Bcycle,
           BAmt = @BAmt,
           SStart = @SStart,
           SCycle = @Cycle,
           SWE = @SWE,
           STime = @Stime,
           SDay = @Sday,
           SDate = @SDate,
           Loc = @loc,
           Owner = @owner,
           Hours = @hours,
           Status = @Status,
           ExpirationDate =   CASE @Expiration   WHEN 1    THEN    @ExpirationDate ELSE   NULL      END,
           Frequencies =      CASE @Expiration   WHEN 2    THEN     @ExpirationFreq
                              ELSE   NULL              END,
           Expiration = @Expiration,
           Chart = @Chart,
           BEscType = @EscalationType,
           BEscCycle = @EscalationCycle,
           BEscFact = @EscalationFactor,
           EscLast = @EscalationLast,
		   Detail=@Detail,
		   LastUpdateDate=GETDATE() ,
		   BLenght = @ContractLength
    WHERE  Job = @Job

 
	
	EXECUTE SpUpdatingProjectBudgetAmount @job=@job,@UpdatedBy=@UpdatedBy

    IF @@ERROR <> 0
       AND @@TRANCOUNT > 0 BEGIN
          RAISERROR ('Error Occured',16,1) 
          ROLLBACK TRANSACTION 
          RETURN
      END

    DELETE FROM tbljoinElevJob
    WHERE  Job = @Job

    IF @@ERROR <> 0
       AND @@TRANCOUNT > 0 BEGIN
          RAISERROR ('Error Occured',16,1) 
          ROLLBACK TRANSACTION 
          RETURN
      END

    INSERT INTO tbljoinElevJob
                (Job,       elev,              price,
                 Hours)
    SELECT @Job,
           Elevunit,
           price,
           hours
    FROM   @ElevJobData

    ----- Update the price in Elev  table when user change it at Contract Screen
    UPDATE e SET e.Price=t.Price
	FROM tblJoinElevJob t
	INNER JOIN elev e ON e.id=t.Elev
	WHERE t.Job=@Job 
	AND t.Price <> e.Price
	AND t.Price <> 0 
	---------------------


    IF @@ERROR <> 0      AND @@TRANCOUNT > 0
	
	 BEGIN

          RAISERROR ('Error Occured',16,1)

          ROLLBACK TRANSACTION

          RETURN
      END

    UPDATE Loc
    SET    Maint = 1,
           Route = @Route,
           Billing = @ContractBill
    WHERE  Loc = @loc -- change by Mayuri 25th dec,15 for billing details in Loc, owner table
    UPDATE [Owner]
    SET    Billing = @CustomerBill,
           Central = @Central
    WHERE  ID = @owner

    --------------------------------------------- update custom data of recurring contract -----------------------------------------
    DECLARE @countValue INT

    SELECT @countValue = Count( * )
    FROM   tblCustomJobT
    WHERE  JobID = @job

    IF ( @countValue = 0 ) BEGIN
          DECLARE DB_CURSOR CURSOR FOR
            SELECT [ID],
                   [tblTabID],
                   [Label],
                   [Line],
                   [Value],
                   [Format]
            FROM   @CustomItems

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
      END
    ELSE BEGIN
        DECLARE DB_CURSOR CURSOR FOR
          SELECT [ID],
                 [tblTabID],
                 [Label],
                 [Line],
                 [Value],
                 [Format]
          FROM   @CustomItems

        OPEN DB_CURSOR

        FETCH NEXT FROM DB_CURSOR INTO @tblCustomFieldsId,
                                       @tblTabID,
                                       @Label,
                                       @TabLine,
                                       @Value,
                                       @Format

        WHILE @@FETCH_STATUS = 0 BEGIN
              UPDATE [dbo].[tblCustomJobT]
              SET    [Value] = @Value
              WHERE  JobID = @job
                     AND tblCustomFieldsID = @tblCustomFieldsId

              FETCH NEXT FROM DB_CURSOR INTO @tblCustomFieldsId,
                                             @tblTabID,
                                             @Label,
                                             @TabLine,
                                             @Value,
                                             @Format
          END

        CLOSE DB_CURSOR

        DEALLOCATE DB_CURSOR
    END
		
    COMMIT TRANSACTION 
GO