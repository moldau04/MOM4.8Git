Create procedure [dbo].[spGetAllEquipmentTestPricing]
AS 
Begin
SELECT  p.[ID]    
      ,[TestTypeId]	 
	  ,[Classification]
      ,[Amount]
	  ,[Override]
      ,[LastUpdateDate]
	  ,[CreatedBy]
	  ,[UpdatedBy]	 
	  ,l.Name as TestType
	  ,ISNULL(p.Remarks,'') AS TestPricingRemarks
	  ,ISNULL(p.DefaultHour,0) AS [DefaultHour]
	  ,ISNULL(p.PriceYear,YEAR(GETDATE())) AS PriceYear
	  ,ISNULL(p.ThirdPartyRequired,0) AS ThirdPartyRequired
  FROM [EquipmentTestPricing] p
  inner join [LoadTest] l on l.ID=p.[TestTypeId]
  ORDER BY PriceYear,l.Name,Classification desc

End