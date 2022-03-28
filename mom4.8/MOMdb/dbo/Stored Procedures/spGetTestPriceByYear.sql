
CREATE PROCEDURE [dbo].[spGetTestPriceByYear]
	@Classification varchar(500),
	@TestTypeId int,
	@PriceYear int
AS
BEGIN
	IF(SELECT COUNT(1) FROM EquipmentTestPricing WHERE Classification=@Classification AND TestTypeId=@TestTypeId AND isnull(PriceYear, Year(GETDATE()))=@PriceYear)=0
	BEGIN
		SELECT 
		0 AS ID
		,@TestTypeId AS TestTypeId
		,@Classification AS Classification
		,0 AS Amount
		,0 AS Override
		,'01/01/1900' AS LastUpdateDate
		,'' AS CreatedBy
		,'' AS UpdatedBy
		,'' AS Remarks
		,0 AS DefaultHour
		,@PriceYear as PriceYear
		, 0 as ThirdPartyRequired 
		,(SELECT top 1 Name FROM LoadTest where ID=@TestTypeId) AS TestTypeName 
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
		 WHERE TestTypeCover.TestTypeID=@TestTypeId FOR XML PATH('')), 2 , 9999)),'') As TestTypeCoverName
		 , isnull((Select SUBSTRING(( 
		 SELECT ',' + Convert(varchar(50),ltype.Name) AS 'data()'  FROM TestTypeCover LEFT JOIN LoadTest ltype ON ltype.ID= TestTypeCover.TestTypeID
		 WHERE TestTypeCover.TestTypeCoverID=@TestTypeId FOR XML PATH('')), 2 , 9999)),'') As CoveredByTestTypeName
	FROM EquipmentTestPricing 
	WHERE 
		Classification=@Classification 
		AND TestTypeId=@TestTypeId 
		AND isnull(PriceYear, Year(GETDATE()))=@PriceYear
	END
	

END