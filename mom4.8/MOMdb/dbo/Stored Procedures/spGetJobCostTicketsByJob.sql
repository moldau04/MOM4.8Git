CREATE PROCEDURE [dbo].[spGetJobCostTicketsByJob]
	@job int,
	@phase int,
	@type smallint,
	@sdate datetime = null,
	@edate datetime = null
AS
BEGIN
	
	SET NOCOUNT ON;

	DECLARE @text VARCHAR(MAX) 

	SET @text = 'SELECT   ( select top 1 GroupName from tblEstimateGroup where ID= jobt.GroupId ) as GroupName,
						t.Job,
						t.Phase,
						t.fDesc,
						jobt.Code,
						b.Type, 
						b.Item, 
						b.ItemID, 
						b.Type, 
						b.TypeID, 
						b.TypeName, 
						b.TypeValue,
						t.ID AS TicketID,
						t.Est, 
						((isnull(t.Reg,0) + isnull(t.RegTrav,0)) + 
						((isnull(t.OT,0) + isnull(t.OTTrav,0))) + 
						((isnull(t.NT,0) + isnull(t.NTTrav,0))) + 
						((isnull(t.DT,0) + isnull(t.DTTrav,0))) + 
						(isnull(t.TT,0))) AS ActualHr,
						isnull(jobt.BHours,0) as BudgetHr,
						isnull(jobt.Budget,0)+isnull(jobt.Modifier,0) as BMatExp,
						isnull(jobt.ETC,0)+isnull(jobt.ETCMod,0) as BLabExp,
						isnull(jobt.Budget,0)+isnull(jobt.Modifier,0)+isnull(jobt.ETC,0)+isnull(jobt.ETCMod,0) as BudgetExp,
						isnull((SELECT sum(isnull(amount,0)) FROM JobI WHERE TransID = -(t.ID) AND isnull(Labor,0) = 0),0) AS Expenses,
		
						isnull((SELECT sum(isnull(amount,0)) FROM JobI WHERE TransID = -(t.ID) AND Labor = 1),0) AS LaborExp,

						(isnull((SELECT sum(isnull(amount,0)) FROM JobI WHERE TransID = -(t.ID) AND isnull(Labor,0) = 0),0) + 
						isnull((SELECT sum(isnull(amount,0)) FROM JobI WHERE TransID = -(t.ID) AND Labor = 1),0)) as TotalExp
		
						FROM TicketD AS t
						LEFT JOIN tblWork AS w ON t.fWork = w.ID
						INNER JOIN JobTItem AS jobt on jobt.Job = t.Job AND jobt.Line = t.Phase 
						LEFT JOIN (SELECT jobt.ID, 
										 1 AS Type,
										 ''Cost'' AS TypeName,
										 isnull(b.Type,0) AS TypeID,
										 isnull(bomt.Type,'''') as TypeValue,
										 case b.type when 2 then isnull(b.LabItem,0) else isnull(b.MatItem,0) end as ItemID,
										 (CASE b.type WHEN 1 THEN  isnull(i.Name,'''')
													  WHEN 2 THEN isnull(p.fdesc,'''')
													  ELSE '''' END) AS Item
									FROM JobTItem AS jobt
										LEFT JOIN BOM AS b ON jobt.ID = b.JobTItemID
										LEFT JOIN BOMT AS bomt ON b.Type = bomt.ID
										LEFT JOIN Inv AS i ON i.ID = b.MatItem
										LEFT JOIN PRWage AS p ON p.ID = b.LabItem
										WHERE jobt.Type <> 0
						)AS b ON b.ID = jobt.ID

						WHERE		jobt.Type <> 0 
								AND t.Job = '''+ CONVERT(VARCHAR(50),@job) +''' 
								AND jobt.Line = '''+ CONVERT(VARCHAR(50),@phase) +''''

			IF(@sdate IS NOT NULL and @edate IS NOT NULL and    @edate <> '1/1/0001 12:00:00 AM') 
			BEGIN
				SET @text +='   AND t.EDate >= '''+ CONVERT(VARCHAR(50), @sdate) +'''
								AND t.EDate <= '''+ CONVERT(VARCHAR(50),@edate) +''''
			END 	SET @text +='	ORDER BY t.ID'
	EXEC (@text)
	 
		
END