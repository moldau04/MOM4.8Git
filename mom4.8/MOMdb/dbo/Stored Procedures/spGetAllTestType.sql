CREATE PROCEDURE  [dbo].[spGetAllTestType]                
AS          
BEGIN    
DECLARE @Names VARCHAR(8000) 
	SELECT 
		[ID] 
		,[Name]
		,[Authority]
		,[Frequency]
		,[Remarks]
		,[Count]
		,[Level]
		,[Cat] 
		,[fDesc]
		,ISNULL([NextDateCalcMode],0) AS [NextDateCalcMode]
		,ISNULL([Charge],0) AS Charge
		,ISNULL([ThirdParty],0) AS ThirdParty
		,ISNULL([Status],0) AS Status
		, (CASE WHEN Remarks IS Null THEN ' ' ELSE Remarks END) AS Remarks 
		, isnull((Select SUBSTRING(( 
		 SELECT ',' + Convert(varchar(10),TestTypeCoverID) AS 'data()'  FROM TestTypeCover 
		 WHERE TestTypeID=ID FOR XML PATH('')), 2 , 9999)),'') As TestTypeCover
		 ,ISNULL((SELECT TOP 1 TicketCovered FROM TestTypeCover WHERE TestTypeID=ID),0) AS TicketCovered
		  ,ISNULL((SELECT COUNT(1) FROM TestTypeCover WHERE TestTypeCoverID=ID AND TicketCovered=1),0) AS IsTicketCoveredByTestType
	FROM LoadTest
END

