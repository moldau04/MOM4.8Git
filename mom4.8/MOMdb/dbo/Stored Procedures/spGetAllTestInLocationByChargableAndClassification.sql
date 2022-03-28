CREATE PROCEDURE [dbo].[spGetAllTestInLocationByChargableAndClassification]
	@Loc INT,
	@TestType Int,
	@YearProposal INT,
	@Chargeable bit,
	@Classification VARCHAR(100)
AS

	Declare @TestTypeChild int
	Declare @HasChild int
	Declare @IsTestCover INT
	SET @TestTypeChild =(SELECT TestTypeCoverID FROM TestTypeCover WHERE TestTypeID=@TestType)
	IF OBJECT_ID('tempdb..#TEMPSAFETYDETAILS') IS NOT NULL DROP TABLE  #TEMPSAFETYDETAILS
	CREATE TABLE #TEMPSAFETYDETAILS
	(
	LID INT NULL,
	Next varchar(10),
	--TestType
	TestType  VARCHAR(300) NULL,
	TestTypeID int NULL,
	--Elev
	Elev Int,
	Unit  VARCHAR(300) NULL,
	Classification NVARCHAR(100) null,
	--Price
	Amount NUMERIC (32,2),
	OverrideAmount NUMERIC (32,2),
	ThirdPartyName VARCHAR(200) NULL,
	ThirdPartyPhone VARCHAR(200) NULL,
	ThirdPartyRequired int NULL,
	Chargeable int null
	)



	Insert into #TEMPSAFETYDETAILS
	select item.LID,
	isnull( convert(varchar(10),DATEADD(year,((@YearProposal-year(item.Next))/ (ttype.Frequency/12.0))* (ttype.Frequency/12.0), item.Next) ,101),'')	
	--TestType
	,ttype.Name, ttype.ID as TestTypeID 
	 --Elev
	,e.ID ,e.Unit, isnull(e.Classification,'')	
	 --Price
	 ,ISNULL(price.DefaultAmount,0)
	,ISNULL(price.OverrideAmount,0) 	
	,isnull(price.ThirdPartyName,'')
	,isnull(price.ThirdPartyPhone,'')
	,isnull(price.ThirdPartyRequired,0)
	,isnull(price.Chargeable,1)
	FROM  LoadTestItem item
	LEFT JOIN LoadTest ttype on ttype.ID=item.ID	
	INNER JOIN Loc l on l.Loc=item.Loc
	INNER JOIN Elev e on e.ID=Item.Elev
	LEFT JOIN LoadTestItemHistoryPrice price ON price.LID=item.LID AND price.PriceYear=@YearProposal
	LEFT JOIN tblJoinElevJob eJob ON eJob.Elev=e.ID AND price.PriceYear=@YearProposal
	--Ticket
	LEFT JOIN LoadTestItemHistory itemHistory ON itemHistory.LID=item.LID AND itemHistory.TestYear=@YearProposal	
	--Proposal
	LEFT JOIN ProposalFormDetail pfd on pfd.TestID=item.LID and pfd.YearProposal=@YearProposal

	WHERE
	l.Status<>1 and e.Status =0 	
	AND (select count (1) from #TEMPSAFETYDETAILS where LID=item.LID and #TEMPSAFETYDETAILS.Next= DATEADD(year,((@YearProposal-year(item.Next))/ (ttype.Frequency/12.0))* (ttype.Frequency/12.0), item.Next) )=0
	AND (Isnull(@TestType,0)=0 or @TestType=ttype.ID)
	AND e.Classification=@Classification
	AND pfd.ProposalID IS NULL 
	AND item.ID=@TestType
		AND item.Loc=@Loc

	SELECT * FROM #TEMPSAFETYDETAILS
	WHERE Chargeable=@Chargeable

	--GET Location
	SELECT
	l.Consult,
	l.ID ,
	Tag,
	LTRIM(RTRIM(l.Address))  as locAddress,
	l.City as locCity,
	l.State as locState,
	l.Zip as locZip,
	l.Rol,
	l.Type ,
	isnull(l.Route,0) Route,
	Terr,
	Terr2,
	r.City,
	r.State,
	r.Zip,
	r.Country,
	LTRIM(RTRIM(r.Address)) as Address,
	l.Remarks,
	r.Contact,
	r.Contact as Name,
	r.Phone,
	r.Website,
	r.EMail,
	r.Cellular,
	r.Fax,
	(Select r.EN  from Rol r inner join Owner o on r.ID = o.Rol inner Join Loc l on l.Owner = o.ID Where l.Loc = @Loc) AS EN,
	(Select Name from  Branch  where ID = (Select r.EN  from Rol r inner join Owner o on r.ID = o.Rol inner Join Loc l on l.Owner = o.ID Where l.Loc = @Loc)) AS Company,
	l.owner,
	(select top 1 name from rol where id=(select top 1 rol from owner where id= l.owner)) as custname,
	l.stax,
	l.STax2,
	l.UTax,
	l.Zone,
	l.Country As locCountry,
	r.Lat,r.Lng,l.custom1,l.custom2,l.custom14,l.custom15,l.custom12,l.custom13,l.status,

	 l.PrintInvoice,
	 l.EmailInvoice,
	 l.Balance,
	 l.Loc,
	 isnull(l.NoCustomerStatement,0) as NoCustomerStatement,
	 l.Address + ', '+ l.City + ', ' + l.State + ' ' + l.Zip   As LocationName,
	 tr.Name AS Salesperson,
	 rt.Name AS RouteName,
	 o.Custom1 AS OwnerName,
	 rl.name as Customer,
	 (select count(1) from  elev e with (nolock) where e.loc=l.loc) as Elevs,
	 l.BusinessType as BusinessTypeID,
	 stax.Type as sTaxType

	from Loc l
	left outer JOIN Owner o ON o.id = l.owner
	left outer join Rol rl ON o.rol = rl.id
	left outer join Rol r on l.Rol=r.ID and r.Type=4
	left outer join stax on stax.name = l.stax
	left outer join Branch B on B.ID = r.EN
	left outer join Terr tr with (nolock)  ON l.Terr = tr.ID 
	left outer join Route rt with (nolock) ON l.Route = rt.ID 
	where l.loc=@loc


	--get Price History
	IF(SELECT COUNT(1) FROM EquipmentTestPricing WHERE Classification=@Classification AND TestTypeId=@TestType AND isnull(PriceYear, Year(GETDATE()))=@YearProposal)=0
	BEGIN
		SELECT 
		0 AS ID
		,@TestType AS TestTypeId
		,@Classification AS Classification
		,0 AS Amount
		,0 AS Override
		,'01/01/1900' AS LastUpdateDate
		,'' AS CreatedBy
		,'' AS UpdatedBy
		,'' AS Remarks
		,0 AS DefaultHour
		,@YearProposal as PriceYear
		, 0 as ThirdPartyRequired 
		,(SELECT top 1 Name FROM LoadTest where ID=@TestType) AS TestTypeName 
		,'' As TestTypeCoverName
		 , '' AS CoveredByTestTypeName
	
    END
    ELSE
    BEGIN
		SELECT TOP 1 
		ID 
		,TestTypeId
		,Classification
		,ISNULL(Amount,0) AS Amount
		,ISNULL(Override,0) AS Override
		,LastUpdateDate
		,CreatedBy
		,UpdatedBy
		,ISNULL(Remarks,'') AS Remarks
		,ISNULL(DefaultHour,0) AS DefaultHour
		,isnull(PriceYear, Year(GETDATE())) as PriceYear
		, isnull(ThirdPartyRequired,0) as ThirdPartyRequired 
		,isnull((SELECT top 1 Name FROM LoadTest where ID=EquipmentTestPricing.TestTypeId),'') as TestTypeName 
		,isnull((Select SUBSTRING(( 
		 SELECT ',' + Convert(varchar(50),ltype.Name) AS 'data()'  FROM TestTypeCover LEFT JOIN LoadTest ltype ON ltype.ID= TestTypeCover.TestTypeCoverID
		 WHERE TestTypeCover.TestTypeID=@TestType FOR XML PATH('')), 2 , 9999)),'') As TestTypeCoverName
		 , isnull((Select SUBSTRING(( 
		 SELECT ',' + Convert(varchar(50),ltype.Name) AS 'data()'  FROM TestTypeCover LEFT JOIN LoadTest ltype ON ltype.ID= TestTypeCover.TestTypeID
		 WHERE TestTypeCover.TestTypeCoverID=@TestType FOR XML PATH('')), 2 , 9999)),'') As CoveredByTestTypeName
	FROM EquipmentTestPricing 
	WHERE 
		Classification=@Classification 
		AND TestTypeId=@TestType 
		AND isnull(PriceYear, Year(GETDATE()))=@YearProposal
		END

