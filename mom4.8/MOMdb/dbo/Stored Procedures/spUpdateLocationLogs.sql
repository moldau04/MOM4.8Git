CREATE PROCEDURE [dbo].[spUpdateLocationLogs] 
@Consult int,
@Account varchar(50),
@LocName varchar(100),
@Address varchar(255),
@status smallint,
@City varchar(50),
@State varchar(2),
@Zip varchar(10),
@Route int,
@Terr int,--Default salesperson
@Remarks text,
@ContactName varchar(50),
@Phone varchar(50),
@fax varchar(28),
@Cellular varchar(28),
@Email varchar(50),
@Website varchar(50),
@RolAddress varchar(255),
@RolCity varchar(50),
@RolState varchar(2),
@RolZip varchar(10),
@Type varchar(50),
@locID int,
@Owner int,
@Stax varchar(25),
@Lat varchar(50),
@Lng varchar(50),
@Custom1 varchar(50),
@Custom2 varchar(50),
@To varchar(250),
@CC varchar(250),
@ToInv varchar(250),
@CCInv varchar(250),
@CreditHold tinyint,
@CreditFlag tinyint,
@DispAlert tinyint,
@CreditReason text,
@ContractBill tinyint,
@terms int,
@BillRate numeric(30, 2),
@OT numeric(30, 2),
@NT numeric(30, 2),
@DT numeric(30, 2),
@Travel numeric(30, 2),
@Mileage numeric(30, 2),
@EmailInvoice AS bit,
@PrintInvoice AS bit,
@CopyToLocAndJob bit = 0,
@Terr2 int = NULL, ----Second salesperson
@STax2 varchar(25),
@UTax varchar(25),
@Zone int = NULL,
@UpdatedBy varchar(100),
@Country varchar(50),
@RolCountry varchar(50)

AS
    --DECLARE @ID int
    DECLARE @CurrentOwner varchar(150)
    DECLARE @CurrentLocName varchar(100)
	DECLARE @CurrentAccount varchar(50)
	DECLARE @Currentstatus varchar(50)
	DECLARE @CurrentType varchar(50)
	DECLARE @CurrentRoute varchar(50)
	DECLARE @CurrentTerr varchar(50)--Default salesperson
    DECLARE @CurrentTerr2 varchar(50)--Default salesperson
	DECLARE @CurrentAddress varchar(255)
    DECLARE @CurrentCity varchar(50)
    DECLARE @CurrentState varchar(10)
    DECLARE @CurrentCountry varchar(50)
    DECLARE @CurrentZip varchar(10)
    DECLARE @CurrentLat varchar(50)
	DECLARE @CurrentLng varchar(50)
	DECLARE @CurrentContactName varchar(50)
    DECLARE @CurrentPhone varchar(50)
    DECLARE @CurrentCellular varchar(28)
    DECLARE @Currentfax varchar(28)
    DECLARE @CurrentEmail varchar(50)
    DECLARE @CurrentWebsite varchar(50)
    DECLARE @CurrentCreditHold varchar(10)
    DECLARE @CurrentCreditFlag varchar(10)
    DECLARE @CurrentDispAlert varchar(10)
    DECLARE @CurrentCreditReason varchar(1000)
    DECLARE @CurrentRolAddress varchar(255)
    DECLARE @CurrentRolCity varchar(50)
    DECLARE @CurrentRolState varchar(10)
    DECLARE @CurrentRolZip varchar(10)
    DECLARE @CurrentRolCountry varchar(50)
    DECLARE @CurrentRemarks varchar(1000)
    DECLARE @CurrentPrintInvoice varchar(10)
    DECLARE @CurrentEmailInvoice varchar(10)
    DECLARE @CurrentTo varchar(250)
    DECLARE @CurrentCC varchar(250)
	DECLARE @CurrentToInv varchar(250)
	DECLARE @CurrentCCInv varchar(250)
	DECLARE @CurrentZone varchar(250)
	DECLARE @CurrentContractBill varchar(100)
    DECLARE @Currentterms varchar(250)
	DECLARE @CurrentCustom1 varchar(50)
    DECLARE @CurrentCustom2 varchar(50)
	DECLARE @CurrentStax varchar(25)
	DECLARE @CurrentSTax2 varchar(25)
	DECLARE @CurrentUTax varchar(25)
	DECLARE @CurrentBillRate varchar(30)
	DECLARE @CurrentOT varchar(30)
	DECLARE @CurrentNT varchar(30)
	DECLARE @CurrentDT varchar(30)
	DECLARE @CurrentTravel varchar(30)
	DECLARE @CurrentMileage varchar(30)
	DECLARE @CurrentConsult varchar(100)

	SELECT --* ,
		--@ID = l.Rol
		--, 
		@CurrentAccount = l.ID
		, @CurrentType = l.Type
		, @CurrentAddress = l.Address
		, @CurrentCity = l.City
		, @CurrentState = l.State
		, @CurrentZip = l.Zip
		, @CurrentCountry = l.Country
		, @CurrentLat = r.Latt
		, @CurrentLng = r.lng
		, @CurrentContactName = r.Contact
		, @CurrentPhone = r.Phone
		, @CurrentCellular = r.Cellular
		, @Currentfax = r.Fax
		, @CurrentEmail = r.EMail
		, @CurrentWebsite = r.Website
		, @CurrentCreditHold = l.Credit
		, @CurrentCreditFlag = l.CreditFlag
		, @CurrentDispAlert = l.DispAlert
		, @CurrentCreditReason = l.CreditReason
		, @CurrentRolAddress = r.Address
		, @CurrentRolCity = r.City
		, @CurrentRolState = r.State
		, @CurrentRolZip = r.Zip
		, @CurrentRolCountry = r.Country
		, @CurrentRemarks = l.Remarks
		, @CurrentPrintInvoice = l.PrintInvoice
		, @CurrentEmailInvoice = l.EmailInvoice
		, @CurrentTo = l.Custom14
		, @CurrentCC = l.Custom15
        , @CurrentToInv = l.Custom12
		, @CurrentCCInv = l.Custom13
		, @CurrentContractBill =
                               CASE
                                   WHEN l.Billing = 0 THEN 'Separate per Contract'
                                   ELSE 'Combined on One Invoice'
                               END
		, @CurrentCustom1 = l.Custom1
		, @CurrentCustom2 = l.Custom2
        , @CurrentStax = l.STax
        , @CurrentSTax2 = l.STax2
        , @CurrentUTax = l.UTax
        , @CurrentBillRate = l.BillRate
        , @CurrentOT = l.RateOT
        , @CurrentNT = l.RateNT
        , @CurrentDT = l.RateDT
        , @CurrentTravel = l.RateTravel
        , @CurrentMileage = l.RateMileage
		, @Currentstatus = CASE WHEN l.Status = 0 THEN 'Active' ELSE 'Inactive' END
		, @CurrentOwner = ro.Name 
		, @CurrentLocName = l.Tag
		, @CurrentRoute = rt.Name
		, @CurrentTerr = t.Name
		, @CurrentTerr2 = t2.Name
		, @CurrentZone = z.Name 
		, @Currentterms = tt.Name 
		, @CurrentConsult = tc.Name
		--, 
	FROM Loc l INNER JOIN Rol r ON l.Rol = r.ID
	LEFT JOIN Owner o on o.ID = l.Owner
	INNER JOIN Rol ro ON ro.ID = o.Rol
	LEFT JOIN Route rt ON l.Route = rt.ID
	LEFT JOIN Terr t ON t.ID = l.Terr
	LEFT JOIN Terr t2 ON t2.ID = l.Terr2
	LEFT JOIN Zone z ON z.ID = l.Zone
	LEFT JOIN tblterms tt ON tt.ID = l.DefaultTerms
	LEFT JOIN tblConsult tc ON tc.ID = l.Consult
	WHERE l.Loc = @locID
    
	DECLARE @Rol int

    --DECLARE @Val varchar(1000)
    IF (@Owner IS NOT NULL)
    BEGIN
        DECLARE @OwnerName varchar(150)
        SELECT @OwnerName = r.Name FROM Rol r 
        INNER JOIN Owner o ON o.Rol = r.ID WHERE o.ID = @Owner
        IF (@CurrentOwner <> @OwnerName)
        BEGIN
            EXEC log2_insert @UpdatedBy,'Location',@locID,'Customer Name',@CurrentOwner,@OwnerName
        END
    END
    IF (@LocName IS NOT NULL)
    BEGIN
        IF (@CurrentLocName <> @LocName)
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Location Name',
                             @CurrentLocName,
                             @LocName
        END
    END
    IF (@Account IS NOT NULL)
    BEGIN
        IF (@CurrentAccount <> @Account)
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Account',
                             @CurrentAccount,
                             @Account
        END
    END
    IF (@status IS NOT NULL)
    BEGIN
        DECLARE @StatusVal varchar(50)
        SELECT @StatusVal =
                        CASE
                            WHEN @status = 0 THEN 'Active'
                            ELSE 'Inactive'
                        END
        
        IF (@Currentstatus <> @StatusVal)
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Status',
                             @Currentstatus,
                             @StatusVal
        END
    END
    IF (@Type IS NOT NULL)
    BEGIN
        
        IF (@CurrentType <> @Type)
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Type',
                             @CurrentType,
                             @Type
        END
    END
    IF (@Route IS NOT NULL)
    BEGIN
        
        DECLARE @DefaultWorker varchar(150)
        SELECT
            @DefaultWorker = Name
        FROM Route
        WHERE ID = @Route
        
        IF (@CurrentRoute <> @DefaultWorker)
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Default Worker',
                             @CurrentRoute,
                             @DefaultWorker
        END
        ELSE
        IF (@Route != '0'
            AND @CurrentRoute IS NULL)
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Default Worker',
                             @CurrentRoute,
                             @DefaultWorker
        END
    END
    IF (@Terr IS NOT NULL)
    BEGIN
        
        DECLARE @DefaultSalesperson varchar(150)
        SELECT
            @DefaultSalesperson = Name
        FROM Terr
        WHERE ID = @Terr
        
        IF (@CurrentTerr <> @DefaultSalesperson)
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Default Salesperson',
                             @CurrentTerr,
                             @DefaultSalesperson
        END
    END
    IF (@Terr2 IS NOT NULL)
    BEGIN
        
        DECLARE @Salesperson2 varchar(150)
        SELECT
            @Salesperson2 = Name
        FROM Terr
        WHERE ID = @Terr2
        
        IF (@CurrentTerr2 <> @Salesperson2)
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Salesperson2',
                             @CurrentTerr2,
                             @Salesperson2
        END
    END
    IF (@Address IS NOT NULL)
    BEGIN
        
        IF (@CurrentAddress <> @Address)
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Address',
                             @CurrentAddress,
                             @Address
        END
    END
    IF (@City IS NOT NULL)
    BEGIN
        
        IF (@CurrentCity <> @City)
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'City',
                             @CurrentCity,
                             @City
        END
    END
    IF (@State IS NOT NULL)
    BEGIN
        
        IF (@CurrentState <> @State)
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'State',
                             @CurrentState,
                             @State
        END
    END
    IF (@Zip IS NOT NULL)
    BEGIN
        
        IF (@CurrentZip <> @Zip)
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Zip',
                             @CurrentZip,
                             @Zip
        END
    END
    IF (@Country IS NOT NULL)
    BEGIN
        IF (@CurrentCountry <> @Country)
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Country',
                             @CurrentCountry,
                             @Country
        END
    END
    IF (@Lat IS NOT NULL)
    BEGIN
        
        IF (@CurrentLat <> @Lat)
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Latitude',
                             @CurrentLat,
                             @Lat
        END
    END
    IF (@Lng IS NOT NULL)
    BEGIN
        
        IF (@CurrentLng <> @Lng)
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Longitude',
                             @CurrentLng,
                             @Lng
        END
    END
    IF (@ContactName IS NOT NULL)
    BEGIN
        
        IF (@CurrentContactName <> @ContactName)
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Main Contact',
                             @CurrentContactName,
                             @ContactName
        END
    END
    IF (@Phone IS NOT NULL)
    BEGIN
        
        IF (@CurrentPhone <> @Phone)
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Phone',
                             @CurrentPhone,
                             @Phone
        END
    END
    IF (@Cellular IS NOT NULL)
    BEGIN
        
        IF (@CurrentCellular <> @Cellular)
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Cellular',
                             @CurrentCellular,
                             @Cellular
        END
    END
    IF (@fax IS NOT NULL)
    BEGIN
        
        IF (@Currentfax <> @fax)
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Fax',
                             @Currentfax,
                             @fax
        END
    END
    IF (@Email IS NOT NULL)
    BEGIN
        
        IF (@CurrentEmail <> @Email)
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Email',
                             @CurrentEmail,
                             @Email
        END
    END
    IF (@Website IS NOT NULL)
    BEGIN
        
        IF (@CurrentWebsite <> @Website)
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Website',
                             @CurrentWebsite,
                             @Website
        END
    END
    IF (@CreditHold IS NOT NULL)
    BEGIN
        
        IF (@CurrentCreditHold <> CONVERT(varchar(10), @CreditHold))
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Credit Hold',
                             @CurrentCreditHold,
                             @CreditHold
        END
    END 
	IF (@CreditFlag IS NOT NULL)
    BEGIN
        
        IF (@CurrentCreditFlag <> CONVERT(varchar(10), @CreditFlag))
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Credit Flag',
                             @CurrentCreditFlag,
                             @CreditFlag
        END
    END
    IF (@DispAlert IS NOT NULL)
    BEGIN
        
        IF (@CurrentDispAlert <> CONVERT(varchar(10), @DispAlert))
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Dispatch Alert',
                             @CurrentDispAlert,
                             @DispAlert
        END
    END
    IF (@CreditReason IS NOT NULL)
    BEGIN
        
        IF (@CurrentCreditReason <> CONVERT(varchar(1000), @CreditReason))
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Reason',
                             @CurrentCreditReason,
                             @CreditReason
        END
    END
    IF (@RolAddress IS NOT NULL)
    BEGIN
        
        IF (@CurrentRolAddress <> @RolAddress)
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Bill Address',
                             @CurrentRolAddress,
                             @RolAddress
        END
    END
    IF (@RolCity IS NOT NULL)
    BEGIN
        
        IF (@CurrentRolCity <> @RolCity)
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Bill City',
                             @CurrentRolCity,
                             @RolCity
        END
    END
    IF (@RolState IS NOT NULL)
    BEGIN
        
        IF (@CurrentRolState <> @RolState)
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Bill State',
                             @CurrentRolState,
                             @RolState
        END
    END
    IF (@RolZip IS NOT NULL)
    BEGIN
        
        IF (@CurrentRolZip <> @RolZip)
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Bill Zip',
                             @CurrentRolZip,
                             @RolZip
        END
    END
    IF (@RolCountry IS NOT NULL)
    BEGIN
        
        IF (@CurrentRolCountry <> @RolCountry)
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Bill Country',
                             @CurrentRolCountry,
                             @RolCountry
        END
    END
    IF (@Remarks IS NOT NULL)
    BEGIN
        
        IF (@CurrentRemarks <> CONVERT(varchar(1000), @Remarks))
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Remarks',
                             @CurrentRemarks,
                             @Remarks
        END
    END
    IF (@PrintInvoice IS NOT NULL)
    BEGIN
        IF (@CurrentPrintInvoice <> CONVERT(varchar(10), @PrintInvoice))
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Print Invoice',
                             @CurrentPrintInvoice,
                             @PrintInvoice
        END
    END
    IF (@EmailInvoice IS NOT NULL)
    BEGIN
        
        IF (@CurrentEmailInvoice <> CONVERT(varchar(10), @EmailInvoice))
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Email Invoice',
                             @CurrentEmailInvoice,
                             @EmailInvoice
        END
    END
    IF (@To IS NOT NULL)
    BEGIN
        
        IF (@CurrentTo <> @To)
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Service Email To',
                             @CurrentTo,
                             @To
        END
    END
    IF (@CC IS NOT NULL)
    BEGIN
        
        IF (@CurrentCC <> @CC)
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Service Email CC',
                             @CurrentCC,
                             @CC
        END
    END
    IF (@ToInv IS NOT NULL)
    BEGIN
        
        IF (@CurrentToInv <> @ToInv)
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Invoice Email To',
                             @CurrentToInv,
                             @ToInv
        END
    END
    IF (@CCInv IS NOT NULL)
    BEGIN
        
        IF (@CurrentCCInv <> @CCInv)
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Invoice Email CC',
                             @CurrentCCInv,
                             @CCInv
        END
    END
    IF (@Zone IS NOT NULL)
    BEGIN
        
        DECLARE @ZoneVal varchar(250)
        SELECT
            @ZoneVal = Name
        FROM Zone
        WHERE ID = @Zone
        
        IF (@CurrentZone <> @ZoneVal)
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Zone',
                             @CurrentZone,
                             @ZoneVal
        END
    END
    IF (@ContractBill IS NOT NULL)
    BEGIN
        
        DECLARE @ContractBillVal varchar(50)
        SELECT
            @ContractBillVal =
                              CASE
                                  WHEN @ContractBill = 0 THEN 'Separate per Contract'
                                  ELSE 'Combined on One Invoice'
                              END
        
        IF (@CurrentContractBill <> @ContractBillVal)
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Contract Billing',
                             @CurrentContractBill,
                             @ContractBillVal
        END
    END
    IF (@terms IS NOT NULL)
    BEGIN
        
        DECLARE @TermsVal varchar(150)
        SELECT
            @TermsVal = Name
        FROM tblterms
        WHERE ID = @terms
        
        IF (@Currentterms <> @TermsVal)
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Terms',
                             @Currentterms,
                             @TermsVal
        END
    END
    IF (@Custom1 IS NOT NULL)
    BEGIN
        
        IF (@CurrentCustom1 <> @Custom1)
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Custom1',
                             @CurrentCustom1,
                             @Custom1
        END
    END
    IF (@Custom2 IS NOT NULL)
    BEGIN
        
        IF (@CurrentCustom2 <> @Custom2)
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Custom2',
                             @CurrentCustom2,
                             @Custom2
        END
    END
    IF (@Stax IS NOT NULL)
    BEGIN
        
        IF (@CurrentStax <> @Stax)
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Sales Tax',
                             @CurrentStax,
                             @Stax
        END
    END
    IF (@STax2 IS NOT NULL)
    BEGIN
        
        IF (@CurrentSTax2 <> @STax2)
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Sales Tax2',
                             @CurrentSTax2,
                             @STax2
        END
    END
    IF (@UTax IS NOT NULL)
    BEGIN
        
        IF (@CurrentUTax <> @UTax)
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Use Tax',
                             @CurrentUTax,
                             @UTax
        END
    END
    --IF (@CopyToLocAndJob IS NOT NULL)
    --BEGIN
    --    SET @Val = (SELECT TOP 1
    --        newVal
    --    FROM log2
    --    WHERE screen = 'Location'
    --    AND ref = @locID
    --    AND Field = 'Copy To Project'
    --    ORDER BY CreatedStamp DESC)
    --    IF (@Val <> CONVERT(varchar(10), @CopyToLocAndJob))
    --    BEGIN
    --        EXEC log2_insert @UpdatedBy,
    --                         'Location',
    --                         @locID,
    --                         'Copy To Project',
    --                         @Val,
    --                         @CopyToLocAndJob
    --    END
    --END
    IF (@BillRate IS NOT NULL)
    BEGIN
        
        IF (@CurrentBillRate <> CONVERT(varchar(30), @BillRate))
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Billing Rate',
                             @CurrentBillRate,
                             @BillRate
        END
    END
    IF (@OT IS NOT NULL)
    BEGIN
        
        IF (@CurrentOT <> CONVERT(varchar(30), @OT))
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'OT Rate',
                             @CurrentOT,
                             @OT
        END
    END
    IF (@NT IS NOT NULL)
    BEGIN
        
        IF (@CurrentNT <> CONVERT(varchar(30), @NT))
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'NT Rate',
                             @CurrentNT,
                             @NT
        END
    END
    IF (@DT IS NOT NULL)
    BEGIN
        
        IF (@CurrentDT <> CONVERT(varchar(30), @DT))
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'DT Rate',
                             @CurrentDT,
                             @DT
        END
    END
    IF (@Travel IS NOT NULL)
    BEGIN
        
        IF (@CurrentTravel <> CONVERT(varchar(30), @Travel))
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Travel Rate',
                             @CurrentTravel,
                             @Travel
        END
    END
    IF (@Mileage IS NOT NULL)
    BEGIN
        
        IF (@CurrentMileage <> CONVERT(varchar(30), @Mileage))
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Mileage',
                             @CurrentMileage,
                             @Mileage
        END
    END
    IF (@Consult IS NOT NULL)
    BEGIN
        
        DECLARE @ConsultVal varchar(100)
        SELECT
            @ConsultVal = Name
        FROM tblConsult
        WHERE ID = @Consult
        
        IF (@CurrentConsult <> @ConsultVal)
        BEGIN
            EXEC log2_insert @UpdatedBy,
                             'Location',
                             @locID,
                             'Consult',
                             @CurrentConsult,
                             @ConsultVal
        END
    END