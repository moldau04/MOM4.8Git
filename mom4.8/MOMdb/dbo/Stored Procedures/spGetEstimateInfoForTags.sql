CREATE PROCEDURE [dbo].[spGetEstimateInfoForTags] @EstimateNo int
AS
BEGIN
	DECLARE @CustomerSageID AS varchar(100)
    DECLARE @CustomerAddress AS varchar(100)
    DECLARE @CustomerZip AS varchar(100)
    DECLARE @CustomerState AS varchar(100)
    DECLARE @CustomerCity AS varchar(100)
    DECLARE @CustomerFax AS varchar(100)
    DECLARE @LocationName AS varchar(100)
	DECLARE @LocationAcct AS varchar(100)
    DECLARE @LocationAddress AS varchar(100)
    DECLARE @LocationZip AS varchar(100)
    DECLARE @LocationState AS varchar(100)
    DECLARE @LocationCity AS varchar(100)

    DECLARE @LocBillAddress AS varchar(100)
    DECLARE @LocBillZip AS varchar(100)
    DECLARE @LocBillState AS varchar(100)
    DECLARE @LocBillCity AS varchar(100)

    DECLARE @ProjectDesc AS varchar(max)
    DECLARE @LOCID AS int
    DECLARE @ROLDID AS int
    SET @LOCID = (SELECT TOP 1 LocID FROM Estimate WHERE ID = @EstimateNo)
    SET @ROLDID = (SELECT TOP 1  RolID FROM Estimate WHERE ID = @EstimateNo)

    IF (@LOCID = 0)
    BEGIN
        SELECT
			@CustomerSageID = '',
            @CustomerAddress = Address,
            @CustomerZip = Zip,
            @CustomerState = State,
            @CustomerCity = City,
            @CustomerFax = Fax
        FROM Rol WHERE ID = @ROLDID
		
		SELECT
			@LocationAcct = '',
            @LocationAddress = Address,
            @LocationZip = Zip,
            @LocationState = State,
            @LocationCity = City
        FROM Rol WHERE ID = @ROLDID
    END
    ELSE
    BEGIN
   
		SELECT
			@CustomerSageID = o.SageID,
            @CustomerAddress = r.Address,
            @CustomerZip = r.Zip,
            @CustomerState = r.State,
            @CustomerCity = r.City,
            @CustomerFax = r.Fax,
			@LocationAcct = l.ID,
			@LocationAddress = l.Address,
            @LocationZip = l.Zip,
            @LocationState = l.State,
            @LocationCity = l.City,
            @LocationName = l.Tag,
            @LocBillZip = rl.Zip,
            @LocBillState = rl.State,
            @LocBillCity = rl.City,
            @LocBillAddress = rl.Address
        FROM Loc l
		INNER JOIN Owner o ON l.Owner = o.ID
		INNER JOIN rol r ON r.ID = o.Rol
        LEFT JOIN rol rl ON rl.ID = l.Rol
		WHERE l.Loc = @LOCID
		
		--SELECT
        --    @CustomerAddress = Address,
        --    @CustomerZip = Zip,
        --    @CustomerState = State,
        --    @CustomerCity = City
        --FROM Rol WHERE ID IN (SELECT Rol FROM Owner WHERE ID IN (SELECT Owner FROM Loc WHERE Loc = @LOCID))

		--Select @LocationAddress=Address,@LocationZip=Zip,@LocationState=State,@LocationCity=City from Rol Where ID=@ROLDID
        --SELECT
        --    @LocationAddress = Address,
        --    @LocationZip = Zip,
        --    @LocationState = State,
        --    @LocationCity = City
        --FROM Loc WHERE Loc = @LOCID
    END

    DECLARE @TEMP_ID AS int
    DECLARE @TEMP_NAME AS varchar(200)
    SET @TEMP_ID = (SELECT Template FROM Estimate WHERE ID = @EstimateNo)
    IF @TEMP_ID = 0
    BEGIN
        SET @TEMP_NAME = ''
    END
    ELSE
    BEGIN
        SET @TEMP_NAME = (SELECT
            fDesc
        FROM JobT
        WHERE ID = @TEMP_ID)
    END

    SELECT @ProjectDesc = j.fDesc FROM Job j INNER JOIN Estimate e ON e.Job = j.ID WHERE e.id = @EstimateNo

    --DECLARE @CONTACT_ID AS int
    --DECLARE @CONTACT_NAME AS varchar(200)

    --SET @CONTACT_NAME = ISNULL((SELECT
    --    Contact
    --FROM Estimate
    --WHERE ID = @EstimateNo),'')

    --IF @CONTACT_NAME = ''
    --BEGIN
    --    SET @CONTACT_NAME = ''
    --END
    --ELSE
    --BEGIN
    --    SET @CONTACT_ID = (SELECT
    --        @CONTACT_ID
    --    FROM Phone
    --    WHERE Phone.fDesc = @CONTACT_NAME)
    --END

    Declare @taxable numeric(30,4) = 0
    --select @taxable = @taxable + (case STax when 1 then Cost else 0 end)
    select @taxable = @taxable + (case STax when 1 then MMUAmt else 0 end)
    from EstimateI ei where ei.Estimate = @estimateno


    --select @taxable = @taxable + (case LStax when 1 then Labor else 0 end)
    select @taxable = @taxable + (case LStax when 1 then LMUAmt else 0 end)
    from EstimateI ei where ei.Estimate = @estimateno

    --select @taxable

    SELECT
        es.ID AS EstimateNo,
        es.Category AS Category,
        es.CompanyName AS CompanyName,
        @TEMP_NAME AS Template,
        es.fDate AS EstimateDate,
        es.BDate AS BidCloseDate,
        ISNULL(es.EstimateUserId, 0) AS SalespersonID,
        es.Status AS EstimateStatus,
        es.EstimateBillAddress AS Location,
        --@CONTACT_NAME AS ContactName,
        es.Contact AS ContactName,
        es.EstimateEmail AS Email,
        es.Phone AS Phone,
        --es.EstimateCell as Mobile, 
        (SELECT TOP 1 Cellular FROM Rol WHERE ID = es.RolID) AS Mobile,
        es.Fax AS Fax,
        es.fDesc AS [Desc],
        l.fDesc AS OpportunityName,
        l.ID AS OpportunityNumber,
        ISNULL(l.OpportunityStageID, 0) AS OpportunityStageID,
        --Cast(CONVERT(DECIMAL(10,2),es.Quoted) as nvarchar) AS FinalBidPrice,
        CASE es.Quoted
            WHEN NULL THEN ''
            ELSE CAST(CONVERT(decimal(10, 2), es.Quoted) AS nvarchar)
        END AS FinalBidPrice,
        CAST(CONVERT(decimal(10, 2), es.Price) AS nvarchar) AS BidPrice,
		@CustomerSageID AS CustomerSageID,
        @CustomerAddress AS CustomerAddress,
        @CustomerZip AS CustomerZip,
        @CustomerState AS CustomerState,
        @CustomerCity AS CustomerCity,
		@LocationAcct AS LocationAcct,
        @LocationAddress AS LocationAddress,
        @LocationZip AS LocationZip,
        @LocationState AS LocationState,
        @LocationCity AS LocationCity,
        @LocationName AS LocationName,
        @LocBillAddress AS LocBillingAddress,
        @LocBillZip AS LocBillingZip,
        @LocBillState AS LocBillingState,
        @LocBillCity AS LocBillingCity,
        ISNULL((CASE ffor
            WHEN 'ACCOUNT' THEN (SELECT ISNULL(STax, '0') FROM Loc WHERE Rol = es.RolID)
            WHEN 'PROSPECT' THEN (SELECT ISNULL(STax, '0') FROM Prospect WHERE Rol = es.RolID)
        END), '0') AS STax,
        es.Remarks,
        JobT.fDesc AS Template,
        JobType.Type AS Department,
        MarkupVal,
        STaxVal,
        MatExp,
        LabExp,
        OtherExp,
        SubToalVal as SubTotal,
        TotalCostVal,
        PretaxTotalVal,
        --es.Price - STaxVal as PretaxTotalVal,
        CommissionVal,
        es.OHPer,
        es.MarkupPer,
        CommissionPer,
        Cont,
        Overhead,
        BillRate,
        OT,
        RateTravel,
        RateNT,
        DT,
        RateMileage,
        PType,
        Amount,   
        @taxable as Taxable,
        es.Price - STaxVal - @taxable as NonTaxable,
        --es.Price - STaxVal as SubTotal,
		es.Price - STaxVal as SubToalVal,
        ISNULL(@ProjectDesc,'') ProjectDesc,
		@CustomerFax CustomerFax,
        ISNULL(es.EstimateType,'bid') EstimateType
        --,
		--CASE ISNULL(es.Discounted, 0) WHEN 0 THEN 'No'
		--ELSE 'Yes' END AS Discounted,
		--CASE WHEN ISNULL(es.Discounted, 0) = 1 THEN es.DiscountedNotes
		--ELSE '' END AS DiscountedNotes,
		--ISNULL(eg.GroupName, ''),
		--STUFF(REPLACE((SELECT '#!' + Unit as 'data()' FROM 
		--	(Select LTRIM(RTRIM(eq.Unit)) as Unit from Elev eq WHERE eq.ID in (select ISNULL(ege.EquipmentId, 0) from tblEstimateGroupEquipment ege)) as NameList FOR XML PATH('')),' #!',', '), 1, 2, '') as Equiments
    FROM Estimate es
    LEFT JOIN Lead l ON l.ID = es.Opportunity 
    LEFT JOIN JobT ON es.Template = JobT.ID
    LEFT JOIN JobType ON JobT.Type = JobType.ID
	--LEFT JOIN tblEstimateGroup eg ON eg.Id = es.GroupId
	--LEFT JOIN tblEstimateGroupEquipment ege ON ege.GroupId = eg.Id
	--LEFT JOIN Elev eq ON eq.ID = ege.EquipmentId
    WHERE es.ID = @EstimateNo

    -- Table[1]: Billing Table 
    EXEC spGetEstimateMilestone @EstimateNo
    -- Table[2], Table[3] Get Estimate custom fields
    EXEC spGetScreenCustomFields 'Estimate', @EstimateNo
    -- Table[4]: BOM Table 
    EXEC spGetEstimateBOM @EstimateNo

    -- Table[5]: Custom BOM Table 
    SELECT   
					 EstimateI.fDesc,
					 EstimateI.Line,
				     EstimateI.Quan AS Quan, 
					 BOM.UM, 
					 EstimateI.MMUAmt/EstimateI.Quan AS Unit,
					 EstimateI.MMUAmt AS MarkupPrice,
					 EstimateI.STax as Tax,
					 EstimateI.ID AS EstimateItemID, 
					 'Material' [Type]
		FROM EstimateI 
		INNER JOIN BOM ON BOM.EstimateIId = EstimateI.ID AND EstimateI.Type = 1		
		LEFT JOIN Vendor v ON v.ID = EstimateI.Vendor
		LEFT JOIN ROL r ON r.ID = v.Rol
		WHERE  EstimateI.Estimate = @EstimateNo AND EstimateI.Type = 1
		AND EstimateI.Quan is not null AND EstimateI.Quan != 0-- AND EstimateI.Price != 0
	UNION ALL
	SELECT   
					 EstimateI.fDesc,
					 EstimateI.Line,
					 EstimateI.Hours AS Quan,
					 BOM.UM, 
					 EstimateI.LMUAmt/EstimateI.Hours AS Unit, 
					 EstimateI.LMUAmt AS MarkupPrice,
					 EstimateI.LStax AS Tax,
					 EstimateI.ID AS EstimateItemID,
					 'Labor' [Type]
	FROM EstimateI 
	INNER JOIN BOM ON BOM.EstimateIId = EstimateI.ID AND EstimateI.Type = 1		
	LEFT JOIN Vendor v ON v.ID = EstimateI.Vendor
	LEFT JOIN ROL r ON r.ID = v.Rol
	WHERE  EstimateI.Estimate = @EstimateNo AND EstimateI.Type = 1
	AND EstimateI.Hours is not null AND EstimateI.Hours != 0-- AND EstimateI.Rate != 0
	ORDER BY EstimateI.Line, EstimateI.ID
END