CREATE PROCEDURE [dbo].[spGetDefaultTestPriceByYear]
	@ElevId int,
	@TestTypeId int,
	@PriceYear int
AS
BEGIN
Declare @Classification varchar(100)
set @Classification=ISNULL((SELECT TOP 1 Classification FROM Elev WHERE ID=@ElevId),'')
	
	 DECLARE @isExsitTestCover BIT
	 SET @isExsitTestCover = ISNULL((SELECT 1 FROM LoadTestItem WHERE  isnull(Year(Next), Year(GETDATE()))=@PriceYear AND Elev=@ElevId
	 AND ID IN (SELECT TestTypeID FROM TestTypeCover WHERE TestTypeCoverID=@TestTypeId)),0)

 Select 
	 ID,Name  as TestTypeName 
	,@Classification as Classification
	,isnull((select DefaultHour from EquipmentTestPricing where TestTypeId= LoadTest.ID and isnull(PriceYear, Year(GETDATE()))=@PriceYear AND Classification=@Classification),0) as DefaultHour
	,isnull((select Amount from EquipmentTestPricing where TestTypeId= LoadTest.ID and isnull(PriceYear, Year(GETDATE()))=@PriceYear AND Classification=@Classification),0) as Amount
	,isnull((select Override from EquipmentTestPricing where TestTypeId= LoadTest.ID and isnull(PriceYear, Year(GETDATE()))=@PriceYear AND Classification=@Classification),0) as [Override]
	,isnull((select ThirdPartyRequired from EquipmentTestPricing where TestTypeId= LoadTest.ID and isnull(PriceYear, Year(GETDATE()))=@PriceYear AND Classification=@Classification),0) as [ThirdPartyRequired]
	,isnull((select Remarks from EquipmentTestPricing where TestTypeId= LoadTest.ID and isnull(PriceYear, Year(GETDATE()))=@PriceYear AND Classification=@Classification),'') as Remarks
	, isnull((Select SUBSTRING(( 
		 SELECT ',' + Convert(varchar(10),TestTypeCoverID) AS 'data()'  FROM TestTypeCover 
		 WHERE TestTypeID=@TestTypeId FOR XML PATH('')), 2 , 9999)),'') As TestTypeCover
		 , isnull((Select SUBSTRING(( 
		 SELECT ',' + Convert(varchar(50),ltype.Name) AS 'data()'  FROM TestTypeCover LEFT JOIN LoadTest ltype ON ltype.ID= TestTypeCover.TestTypeCoverID
		 WHERE TestTypeCover.TestTypeID=@TestTypeId FOR XML PATH('')), 2 , 9999)),'') As TestTypeCoverName
	,@isExsitTestCover as ExsitTestCover
	
	, isnull((Select SUBSTRING(( 
		 SELECT ',' + Convert(varchar(50),ltype.Name) AS 'data()'  FROM TestTypeCover LEFT JOIN LoadTest ltype ON ltype.ID= TestTypeCover.TestTypeID
		 WHERE TestTypeCover.TestTypeCoverID=@TestTypeId FOR XML PATH('')), 2 , 9999)),'') As CoveredByTestTypeName
		
	 from LoadTest
	 where ID=@TestTypeId

END