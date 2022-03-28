CREATE PROCEDURE [dbo].[spGetPriceHistory] 			
	@LID INT		
AS 
BEGIN 

SELECT
LID
,PriceYear
,ISNULL(Chargeable,0) AS Chargeable
, ISNULL(DefaultAmount,0) AS DefaultAmount
,ISNULL(OverrideAmount,0) AS OverrideAmount
,ISNULL([CreatedBy],'') AS CreatedBy
,ISNULL([CreatedDate] ,GETDATE()) AS CreatedDate
,ISNULL([DueDate] ,GETDATE()) AS DueDate
,ISNULL([ThirdPartyRequired],0) AS  ThirdPartyRequired
,ISNULL([ThirdPartyName],'')AS ThirdPartyName
,ISNULL([ThirdPartyPhone],'') AS ThirdPartyPhone

FROM LoadTestItemHistoryPrice WHERE LID=@LID			

END 