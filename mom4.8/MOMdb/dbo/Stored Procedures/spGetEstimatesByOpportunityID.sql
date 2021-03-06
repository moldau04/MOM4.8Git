/*--------------------------------------------------------------------
Created By: Thomas
Created On: 21 Feb 2019	
Description: Get all estimates related to an oppotunity 
--------------------------------------------------------------------*/

CREATE PROCEDURE [dbo].[spGetEstimatesByOpportunityID]
	@OpportunityNo INT
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT 
		e.ID EstimateNo, 
		e.Category, 
		(select TOP 1 fDesc from Phone Where CAST(ID AS NVARCHAR(100)) =cast(e.Contact as NVARCHAR(100))) as Contact,
		e.fdesc [Description], 
		e.fDate, 
		e.job ProjectNo, 
		s.Name as [Status], 
		ISNULL(e.Price,0) As EstimatePrice,   
		ISNULL(e.Quoted,0) As QuotedPrice,  
		--CASE e.Quoted WHEN null THEN ''
		--	ELSE Cast(CONVERT(DECIMAL(10,2),e.Quoted) as nvarchar) END
		--AS QuotedPrice,
		CASE ISNULL(e.Discounted,0) WHEN 0 THEN 'No' ELSE 'Yes' END As Discounted     
	FROM Estimate e 
	LEFT OUTER JOIN OEStatus s ON e.[Status]= s.ID 
	--LEFT OUTER JOIN terr t ON E.EstimateUserId=t.ID 
	--LEFT OUTER JOIN Rol r on e.RolID = r.ID 
	--LEFT OUTER JOIN Branch B on B.ID = r.EN  
	--LEFT OUTER JOIN Lead l on l.Estimate = e.ID 
	WHERE e.Opportunity = @OpportunityNo
	Order by e.ID

	-- Get current Opportunity amount
	--SELECT ISNULL(Revenue,0) Revenue FROM Lead WHERE ID = @OpportunityNo

	SELECT TOP 1 ISNULL(l.Revenue,0) Revenue
		, Case WHEN l.Department is not null THEN l.Department
			ELSE j.Type END AS Department
	FROM Lead l
	LEFT JOIN Estimate e ON l.ID = e.Opportunity
	LEFT JOIN JobT j ON j.ID = e.Template 
	WHERE l.ID = @OpportunityNo
END
