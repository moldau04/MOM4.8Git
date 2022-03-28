CREATE PROCEDURE [dbo].[spUpdateContractLogs]
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


DECLARE @CurrentOwnerName varchar(100)
DECLARE @CurrentLocName varchar(100)
DECLARE @CurrentLocAddress varchar(100)
DECLARE @CurrentPO varchar(25)
DECLARE @CurrentRouteName varchar(75)
DECLARE @CurrentStatus varchar(50)
DECLARE @CurrentCType varchar(30)
DECLARE @CurrentBStart varchar(50)
DECLARE @CurrentBCycle varchar(50)
DECLARE @CurrentDetail varchar(50)
DECLARE @CurrentBAmt numeric(30,2)
DECLARE @CurrentBilling varchar(50)
DECLARE @CurrentSpecifyLocName varchar(150)
DECLARE @CurrentContractBill varchar(50)
DECLARE @CurrentCreditcard tinyint
DECLARE @CurrentSPHandle smallint
DECLARE @CurrentSPRemarks varchar(1000)
DECLARE @CurrentSStart varchar(50)
DECLARE @CurrentCycle varchar(50)
DECLARE @CurrentSWE int
DECLARE @CurrentStime varchar(50)
DECLARE @Currenthours varchar(50)
DECLARE @CurrentIsRenewalNotes varchar(10)
DECLARE @CurrentRenewalNotes varchar(1000)
DECLARE @CurrentBillRate varchar(50)
DECLARE @CurrentRateOT varchar(50)
DECLARE @CurrentRateNT varchar(50)
DECLARE @CurrentRateDT varchar(50)
DECLARE @CurrentRateTravel varchar(50)
DECLARE @CurrentMileage varchar(50)
DECLARE @CurrentEscalationType varchar(50)
DECLARE @CurrentEscalationCycle varchar(50)
DECLARE @CurrentEscalationFactor varchar(50)
DECLARE @CurrentExpiration varchar(50)
DECLARE @CurrentExpirationDate varchar(50)
DECLARE @CurrentExpirationFreq varchar(50)
DECLARE @Currentfdesc varchar(75)
DECLARE @CurrentRemarks varchar(1000)
DECLARE @CurrentEquipment varchar(1000)
DECLARE @CurrentEscalationLast varchar(50)
DECLARE @CurrentOwner int
DECLARE @CurrentLoc int
DECLARE @CurrentRoute varchar(75)
DECLARE @CurrentStatusInt int
DECLARE @CurrentChartInt int
DECLARE @CurrentBStartDt DateTime
DECLARE @CurrentBCycleInt int
DECLARE @CurrentDetailInt smallint
DECLARE @CurrentSStartDt Datetime
DECLARE @CurrentContractLength int 

SELECT
    @CurrentOwner = j.Owner
    , @CurrentLoc = j.Loc
    , @CurrentLocName = ISNULL(l.Tag,'')
	, @CurrentLocAddress = l.Address
	, @CurrentOwnerName = r.Name
FROM JOB j 
INNER JOIN Contract c ON c.Job = j.ID
LEFT JOIN Loc l ON l.Loc = j.Loc
LEFT JOIN Owner o ON j.Owner = o.ID
LEFT JOIN Rol r ON o.Rol = r.ID
WHERE j.ID = @Job

SELECT @CurrentRouteName = Name
FROM Route
WHERE ID = (SELECT Custom20	FROM Job WHERE ID = @job)


	
SELECT @CurrentBilling = CASE WHEN Billing = 0 THEN 'Individual' ELSE 'Combined' END
FROM Owner	WHERE ID = @owner

SELECT
	@CurrentSpecifyLocName = tag
FROM loc
WHERE loc = (SELECT		Central	FROM Owner	WHERE ID = @owner)

SELECT
	@CurrentContractBill =
							CASE
								WHEN Billing = 0 THEN 'Separate per Contract'
								ELSE 'Combined on One Invoice'
							END
FROM Loc
WHERE Loc = @loc

SELECT
	@CurrentStatus = CASE j.Status
						WHEN 0 THEN 'Active'
						WHEN 1 THEN 'Closed'
						WHEN 2 THEN 'Hold'
						WHEN 3 THEN 'Completed'
					END
	, @CurrentBStart = CONVERT(varchar(50), BStart, 101)
	, @CurrentBCycle = CASE BCycle
						WHEN 0 THEN 'Monthly'
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
	, @CurrentDetail = CASE Detail
						WHEN 0 THEN 'Summary'
						WHEN 1 THEN 'Detailed'
						WHEN 2 THEN 'Detailed w/Price'
					END
	, @CurrentBAmt = BAmt
	, @CurrentPO = PO
	, @CurrentCType = CType
	, @CurrentCreditcard = CreditCard
	, @CurrentSPHandle = SPHandle
	, @CurrentSPRemarks = SRemarks
	, @CurrentSStart = CONVERT(varchar(50), SStart, 101)
	, @CurrentCycle = CASE SCycle
						WHEN -1 THEN 'Never'
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
	, @CurrentSWE = SWE
	, @CurrentStime = FORMAT(STime, 'hh:mm tt')
	, @Currenthours = Convert(varchar(50),ISNULL(Hours,0))
	, @CurrentIsRenewalNotes = IsRenewalNotes
	, @CurrentRenewalNotes = RenewalNotes
	, @CurrentBillRate = BillRate
	, @CurrentRateOT = RateOT
	, @CurrentRateNT = RateNT
	, @CurrentRateDT = RateDT
	, @CurrentRateTravel = RateTravel
	, @CurrentMileage = RateMileage
	, @CurrentEscalationType = CASE BEscType
								WHEN 0 THEN 'Commodity Index'
								WHEN 1 THEN 'Escalation'
								WHEN 2 THEN 'Return'
								WHEN 3 THEN 'Manual'
							END
	, @CurrentEscalationCycle = BEscCycle
	, @CurrentEscalationFactor = BEscFact
	, @CurrentEscalationLast = CONVERT(varchar(50), EscLast, 101)
	, @CurrentExpiration = CASE Expiration
							WHEN 1 THEN 'Contract expiration date'
							WHEN 2 THEN 'Number of frequencies'
							ELSE 'Indefinitely'
						END
	, @CurrentExpirationDate = CONVERT(varchar(50), ExpirationDate, 101)
	, @CurrentExpirationFreq = Frequencies
	, @Currentfdesc = fdesc
	, @CurrentRemarks = CONVERT(varchar(1000), Remarks)
    , @CurrentRoute = j.Custom20
    , @CurrentStatusInt = j.Status
    , @CurrentChartInt = Chart
    , @CurrentBStartDt = BStart
    , @CurrentBCycleInt = BCycle
    , @CurrentDetailInt = Detail
    , @CurrentSStartDt = SStart
	, @CurrentContractLength =BLenght
FROM Contract c
INNER JOIN Job j ON j.ID = c.Job
WHERE Job = @job
	
SELECT @CurrentEquipment = STUFF((SELECT ',  ' + e.Unit FROM Elev e INNER JOIN tblJoinElevJob ej ON e.ID = ej.Elev WHERE ej.Job = @job FOR xml PATH ('')), 1, 1, '')

-- For logs
Declare @Screen varchar(100) = 'Job';
Declare @ScreenProject varchar(100) = 'Project';
Declare @RefId int;
Set @RefId = @Job;
--DECLARE @Val varchar(1000)

IF (Isnull(@ContractLength, 0) != Isnull(@CurrentContractLength, 0))
BEGIN 
    EXEC log2_insert @UpdatedBy,@Screen,@RefId,'ContractLength', @CurrentContractLength, @ContractLength 
END


IF (Isnull(@owner, 0) != Isnull(@CurrentOwner, 0))
BEGIN
    Declare @OwnerName varchar(150)
	Select @OwnerName = r.Name FROM Rol r INNER JOIN Owner o ON o.Rol = r.ID WHERE o.ID = @Owner
    EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Customer Name', @CurrentOwnerName, @OwnerName
    EXEC log2_insert @UpdatedBy,@ScreenProject,@RefId,'Customer Name', @CurrentOwnerName, @OwnerName
END

IF (Isnull(@loc, 0) != Isnull(@CurrentLoc, 0))
BEGIN
    DECLARE @LocName varchar(150)
    DECLARE @LocAddress varchar(150)
    SELECT @LocName = tag, @LocAddress = Address FROM loc WHERE loc = (SELECT Loc FROM Job WHERE ID = @job)
    
    EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Location Name',@CurrentLocName,@LocName
    EXEC log2_insert @UpdatedBy,@ScreenProject,@RefId,'Location Name',@CurrentLocName,@LocName
    EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Location Address',@CurrentLocAddress,@LocAddress
    EXEC log2_insert @UpdatedBy,@ScreenProject,@RefId,'Location Address',@CurrentLocAddress,@LocAddress
END

IF (ISNULL(@PO, '') != ISNULL(@CurrentPO, ''))
BEGIN
    EXEC log2_insert @UpdatedBy,@Screen,@RefId,'PO',@CurrentPO,@PO
    EXEC log2_insert @UpdatedBy,@ScreenProject,@RefId,'PO',@CurrentPO,@PO
END

IF (ISNULL(@Route,'') != ISNULL(@CurrentRoute,'') )
BEGIN
    
    DECLARE @RouteName varchar(75) = NULL
    SELECT @RouteName = Name FROM Route WHERE ID = (SELECT Custom20 FROM Job WHERE ID = @job)
    EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Preferred Worker',@CurrentRouteName,@RouteName
    EXEC log2_insert @UpdatedBy,@ScreenProject,@RefId,'Preferred Worker',@CurrentRouteName,@RouteName
END

IF (@Status != @CurrentStatusInt)
BEGIN
    DECLARE @StatusVal varchar(50)
    SELECT @StatusVal =
                    CASE @Status
                        WHEN 0 THEN 'Active'
                        WHEN 1 THEN 'Closed'
                        WHEN 2 THEN 'Hold'
                        WHEN 3 THEN 'Completed'
                    END
    
    EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Status',@CurrentStatus,@StatusVal
    EXEC log2_insert @UpdatedBy,@ScreenProject,@RefId,'Status',@CurrentStatus,@StatusVal
END

IF (ISNULL(@CType,'') != ISNULL(@CurrentCType,''))
BEGIN
    EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Service Type',@CurrentCType,@CType
    EXEC log2_insert @UpdatedBy,@ScreenProject,@RefId,'Service Type',@CurrentCType,@CType
END

DECLARE @CurrentChart varchar(75)
DECLARE @ChartGLAccount varchar(75)
SET @ChartGLAccount =  ISNULL((SELECT fDesc FROM Chart WHERE ID = @Chart), '')
SET @CurrentChart =  ISNULL((SELECT fDesc FROM Chart WHERE ID = @CurrentChartInt), '')
IF @CurrentChart != @ChartGLAccount
BEGIN
    EXEC log2_insert @UpdatedBy, @Screen,@RefId,'GL Account',@CurrentChart,@ChartGLAccount
END

IF (@BStart IS NOT NULL AND @BStart != @CurrentBStartDt)
BEGIN
    DECLARE @BStartdate nvarchar(150)
    SELECT @BStartdate = CONVERT(varchar, @BStart, 101)
    EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Billing Start Date',@CurrentBStart,@BStartdate
END
IF (ISNULL(@BCycle,0) != IsNULL(@CurrentBCycleInt, 0))
BEGIN
    
    DECLARE @BCycleVal varchar(50)
    SELECT @BCycleVal =
                    CASE @BCycle
                        WHEN 0 THEN 'Monthly'
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
    
    EXEC log2_insert @UpdatedBy,@Screen, @RefId,'Bill Frequency',@CurrentBCycle,@BCycleVal
END

IF (ISNULL(@Detail,0) != ISNULL(@CurrentDetailInt,0))
BEGIN
    
    DECLARE @DetailVal varchar(50)
    SELECT
        @DetailVal =
                    CASE @Detail
                        WHEN 0 THEN 'Summary'
                        WHEN 1 THEN 'Detailed'
                        WHEN 2 THEN 'Detailed w/Price'
                    END
    EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Bill Detail',@CurrentDetail,@DetailVal
END
IF (ISNULL(@BAmt,0) != ISNULL(@CurrentBAmt, 0))
BEGIN
    Declare @CurrentBAmtStr varchar(50) = CONVERT(varchar(50), ISNULL(@CurrentBAmt, 0))
        , @BAmtStr varchar(50) = CONVERT(varchar(50), ISNULL(@BAmt,0))
    EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Billing Amount',@CurrentBAmtStr,@BAmtStr
END


DECLARE @BillingVal varchar(50)
SELECT @BillingVal =
                    CASE
                        WHEN @CustomerBill = 0 THEN 'Individual'
                        ELSE 'Combined'
                    END
IF (@CurrentBilling <> @BillingVal)
BEGIN
    EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Billing',@CurrentBilling,@BillingVal
    EXEC log2_insert @UpdatedBy,@ScreenProject,@RefId,'Billing',@CurrentBilling,@BillingVal
END


DECLARE @SpecifyLocName varchar(150)
SELECT
    @SpecifyLocName = tag
FROM loc
WHERE loc = (SELECT
    Central
FROM Owner
WHERE ID = @owner)
IF (@CurrentSpecifyLocName <> @SpecifyLocName)
BEGIN
    EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Specify Location',@CurrentSpecifyLocName,@SpecifyLocName
    EXEC log2_insert @UpdatedBy,@ScreenProject,@RefId,'Specify Location',@CurrentSpecifyLocName,@SpecifyLocName
END



DECLARE @ContractBillVal varchar(50)
SELECT
    @ContractBillVal =
                        CASE
                            WHEN @ContractBill = 0 THEN 'Separate per Contract'
                            ELSE 'Combined on One Invoice'
                        END
IF (@CurrentContractBill <> @ContractBillVal)
BEGIN
    EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Contract Billing',@CurrentContractBill,@ContractBillVal
    EXEC log2_insert @UpdatedBy,@ScreenProject,@RefId,'Contract Billing',@CurrentContractBill,@ContractBillVal
END
    
IF (ISNULL(@CurrentCreditcard,0) <> ISNULL(@Creditcard,0))
BEGIN
    EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Credit Card',@CurrentCreditcard,@Creditcard
    EXEC log2_insert @UpdatedBy,@ScreenProject,@RefId,'Credit Card',@CurrentCreditcard,@Creditcard
END
    
    
IF (ISNULL(@CurrentSPHandle,0) <> ISNULL(@SPHandle, 0))
BEGIN
    EXEC log2_insert @UpdatedBy,@Screen,@RefId,'IsSpecial Notes',@CurrentSPHandle,@SPHandle
    EXEC log2_insert @UpdatedBy,@ScreenProject,@RefId,'Special Notes',@CurrentSPHandle,@SPHandle
END
    

IF (@CurrentSPRemarks <> CONVERT(varchar(1000), @SPRemarks))
BEGIN
    EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Special Notes',@CurrentSPRemarks,@SPRemarks
    EXEC log2_insert @UpdatedBy,@ScreenProject,@RefId,'Special Notes',@CurrentSPRemarks,@SPRemarks
END
    
IF (@SStart != @CurrentSStartDt)
BEGIN
    
    DECLARE @SStartdate nvarchar(150)
    SELECT
        @SStartdate = CONVERT(varchar, @SStart, 101)
    IF (@CurrentSStart <> @SStartdate)
    BEGIN
        EXEC log2_insert @UpdatedBy,
                         @Screen,
                         @RefId,
                         'Schedule Start Date',
                         @CurrentSStart,
                         @SStartdate
    END
    
END

DECLARE @CycleFreqVal varchar(50)
SELECT
    @CycleFreqVal =
                    CASE @Cycle
                        WHEN -1 THEN 'Never'
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
IF (ISNULL(@CurrentCycle,'') <> ISNULL(@CycleFreqVal,''))
BEGIN
    EXEC log2_insert @UpdatedBy,
                        @Screen,
                        @RefId,
                        'Schedule Frequency',
                        @CurrentCycle,
                        @CycleFreqVal
END
    

IF (ISNULL(@CurrentSWE,0) <> ISNULL(@SWE,0))
BEGIN
    EXEC log2_insert @UpdatedBy,
                        @Screen,
                        @RefId,
                        'Weekends',
                        @CurrentSWE,
                        @SWE
END
    
DECLARE @Stimedate nvarchar(150)
SELECT
    @Stimedate = FORMAT(@Stime, 'hh:mm tt')
IF (@CurrentStime <> @Stimedate)
BEGIN
    EXEC log2_insert @UpdatedBy,
                        @Screen,
                        @RefId,
                        'Scheduled Time',
                        @CurrentStime,
                        @Stimedate
END
    

IF (@Currenthours <> CONVERT(varchar(50), ISNULL(@hours,0)))
BEGIN
    EXEC log2_insert @UpdatedBy,
                        @Screen,
                        @RefId,
                        'Total Hours',
                        @Currenthours,
                        @hours
END
    
IF (@CurrentIsRenewalNotes <> ISNULL(@IsRenewalNotes,0))
BEGIN
    EXEC log2_insert @UpdatedBy,
                        @Screen,
                        @RefId,
                        'IsRenewal Notes',
                        @CurrentIsRenewalNotes,
                        @IsRenewalNotes
    EXEC log2_insert @UpdatedBy,
                        @ScreenProject,
                        @RefId,
                        'IsRenewal Notes',
                        @CurrentIsRenewalNotes,
                        @IsRenewalNotes
END
    
IF (@CurrentRenewalNotes <> CONVERT(varchar(1000), ISNULL(@RenewalNotes,'')))
BEGIN
    EXEC log2_insert @UpdatedBy,
                        @Screen,
                        @RefId,
                        'Renewal Notes',
                        @CurrentRenewalNotes,
                        @RenewalNotes
    EXEC log2_insert @UpdatedBy,
                        @ScreenProject,
                        @RefId,
                        'Renewal Notes',
                        @CurrentRenewalNotes,
                        @RenewalNotes
END
    
IF (@CurrentBillRate <> CONVERT(varchar(50), @BillRate))
BEGIN
    EXEC log2_insert @UpdatedBy,
                        @Screen,
                        @RefId,
                        'Bill Rate',
                        @CurrentBillRate,
                        @BillRate
    EXEC log2_insert @UpdatedBy,
                        @ScreenProject,
                        @RefId,
                        'Bill Rate',
                        @CurrentBillRate,
                        @BillRate
END
    
IF (@CurrentRateOT <> CONVERT(varchar(50), @RateOT))
BEGIN
    EXEC log2_insert @UpdatedBy,
                        @Screen,
                        @RefId,
                        'OT Rate',
                        @CurrentRateOT,
                        @RateOT
    EXEC log2_insert @UpdatedBy,
                        @ScreenProject,
                        @RefId,
                        'OT Rate',
                        @CurrentRateOT,
                        @RateOT
END
    

    
IF (@CurrentRateNT <> CONVERT(varchar(50), @RateNT))
BEGIN
    EXEC log2_insert @UpdatedBy,
                        @Screen,
                        @RefId,
                        'NT Rate',
                        @CurrentRateNT,
                        @RateNT
    EXEC log2_insert @UpdatedBy,
                        @ScreenProject,
                        @RefId,
                        'NT Rate',
                        @CurrentRateNT,
                        @RateNT
END
    
IF (@CurrentRateDT <> CONVERT(varchar(50), @RateDT))
BEGIN
    EXEC log2_insert @UpdatedBy,
                        @Screen,
                        @RefId,
                        'DT Rate',
                        @CurrentRateDT,
                        @RateDT
    EXEC log2_insert @UpdatedBy,
                        @ScreenProject,
                        @RefId,
                        'DT Rate',
                        @CurrentRateDT,
                        @RateDT
END
    

IF (@CurrentRateTravel <> CONVERT(varchar(50), @RateTravel))
BEGIN
    EXEC log2_insert @UpdatedBy,
                        @Screen,
                        @RefId,
                        'Travel Rate',
                        @CurrentRateTravel,
                        @RateTravel

    EXEC log2_insert @UpdatedBy,
                        @ScreenProject,
                        @RefId,
                        'Travel Rate',
                        @CurrentRateTravel,
                        @RateTravel
END
    

IF (@CurrentMileage <> CONVERT(varchar(50), @Mileage))
BEGIN
    EXEC log2_insert @UpdatedBy,
                        @Screen,
                        @RefId,
                        'Mileage',
                        @CurrentMileage,
                        @Mileage

    EXEC log2_insert @UpdatedBy,
                        @ScreenProject,
                        @RefId,
                        'Mileage',
                        @CurrentMileage,
                        @Mileage
END
    

DECLARE @EscalationTypeVal varchar(50)
SELECT
    @EscalationTypeVal =
                        CASE @EscalationType
                            WHEN 0 THEN 'Commodity Index'
                            WHEN 1 THEN 'Escalation'
                            WHEN 2 THEN 'Return'
                            WHEN 3 THEN 'Manual'
                        END
IF (@CurrentEscalationType <> ISNULL(@EscalationTypeVal, ''))
BEGIN
    EXEC log2_insert @UpdatedBy,
                        @Screen,
                        @RefId,
                        'Escalation Type',
                        @CurrentEscalationType,
                        @EscalationTypeVal
END
    

    
IF (@CurrentEscalationCycle <> Convert(Varchar(50),@EscalationCycle))
BEGIN
    EXEC log2_insert @UpdatedBy,
                        @Screen,
                        @RefId,
                        'Escalation Cycle',
                        @CurrentEscalationCycle,
                        @EscalationCycle
END
    

IF (@CurrentEscalationFactor <> CONVERT(varchar(50), @EscalationFactor))
BEGIN
    EXEC log2_insert @UpdatedBy,
                        @Screen,
                        @RefId,
                        'Escalation Factor',
                        @CurrentEscalationFactor,
                        @EscalationFactor
END
   

DECLARE @EscalationLastdate nvarchar(150)
SELECT
    @EscalationLastdate = CONVERT(varchar, @EscalationLast, 101)
IF (@CurrentEscalationLast <> @EscalationLastdate)
BEGIN
    EXEC log2_insert @UpdatedBy,
                        @Screen,
                        @RefId,
                        'Escalated Last',
                        @CurrentEscalationLast,
                        @EscalationLastdate
END
   

DECLARE @ExpirationVal varchar(50)
SELECT
    @ExpirationVal =
                    CASE @Expiration
                        WHEN 1 THEN 'Contract expiration date'
                        WHEN 2 THEN 'Number of frequencies'
                        ELSE 'Indefinitely'
                    END
IF (@CurrentExpiration <> @ExpirationVal)
BEGIN
    EXEC log2_insert @UpdatedBy,
                        @Screen,
                        @RefId,
                        'Expiration',
                        @CurrentExpiration,
                        @ExpirationVal
END
    

DECLARE @ExpirationDateVal nvarchar(150)
SELECT
    @ExpirationDateVal = CONVERT(varchar, @ExpirationDate, 101)
IF (@CurrentExpirationDate <> @ExpirationDateVal)
BEGIN
    EXEC log2_insert @UpdatedBy,
                        @Screen,
                        @RefId,
                        'Expiration Date',
                        @CurrentExpirationDate,
                        @ExpirationDateVal
END
    

IF (@CurrentExpirationFreq <> @ExpirationFreq)
BEGIN
    EXEC log2_insert @UpdatedBy,
                        @Screen,
                        @RefId,
                        'Exp Frequencies',
                        @CurrentExpirationFreq,
                        @ExpirationFreq
END
    

IF (@Currentfdesc <> @fdesc)
BEGIN
    EXEC log2_insert @UpdatedBy,
                        @Screen,
                        @RefId,
                        'Contract Description',
                        @Currentfdesc,
                        @fdesc

    EXEC log2_insert @UpdatedBy,
                        @ScreenProject,
                        @RefId,
                        'Project Name',
                        @Currentfdesc,
                        @fdesc
END
    

IF (@CurrentRemarks <> CONVERT(varchar(1000), @Remarks))
BEGIN
    EXEC log2_insert @UpdatedBy,
                        @Screen,
                        @RefId,
                        'Remarks',
                        @CurrentRemarks,
                        @Remarks
    EXEC log2_insert @UpdatedBy,
                        @ScreenProject,
                        @RefId,
                        'Remarks',
                        @CurrentRemarks,
                        @Remarks
END
    
    
DECLARE @EquipmentJob varchar(1000)
SELECT
    @EquipmentJob = STUFF((SELECT
        ',  ' + e.Unit
    FROM Elev e
    INNER JOIN tblJoinElevJob ej
        ON e.ID = ej.Elev
    WHERE ej.Job = @job
    FOR xml PATH (''))
    , 1, 1, '')
IF (@CurrentEquipment <> @EquipmentJob)
BEGIN
    EXEC log2_insert @UpdatedBy,
                        @Screen,
                        @RefId,
                        'Equipment',
                        @CurrentEquipment,
                        @EquipmentJob
    EXEC log2_insert @UpdatedBy,
                        @ScreenProject,
                        @RefId,
                        'Equipment',
                        @CurrentEquipment,
                        @EquipmentJob
END
    
