CREATE PROCEDURE  [dbo].[spGetAllTestByEquipmentID] 
 @ID INT
 
AS
BEGIN
	SELECT 
	item.Elev as idUnit
	,item.LID as idTestItem
	,t.Name as Name
	,s.ItemName as   Status
	, itemHistory.Last as Last
	,itemHistory.Next as Next	
	,itemHistory.TestYear as TestYear
	,itemHistory.TestStatus TestStatusID
	,CASE WHEN itemHistory.TicketID IS NULL THEN 0 ELSE 1 END Ticketed 
	,itemHistory.TicketID as Ticket
 FROM LoadTestItemHistory itemHistory 
 INNER JOIN  LoadTestItem item on item.LID=itemHistory.LID
 INNER JOIN  LoadTest t ON item.ID = t.ID  
 INNER JOIN  v_TestStatus s ON itemHistory.TestStatus=s.ItemValue
 WHERE item.Elev=@ID

END
