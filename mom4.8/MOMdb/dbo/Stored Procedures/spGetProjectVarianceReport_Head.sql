--EXEC spGetProjectVarianceReport_Head 20
CREATE PROCEDURE spGetProjectVarianceReport_Head
@JobId INT
AS                                        
BEGIN                                        
           
	SELECT 
		ID,
		fDesc,
		Loc,
		(SELECT TOP 1 tag FROM Loc WHERE Loc = j.Loc) AS LocName,             
		Owner,
		(SELECT TOP 1 NAME FROM rol WHERE id=(SELECT TOP 1 Rol FROM OWNER WHERE ID = j.Owner)) AS CustomerName,          
		Type,
		(SELECT Type FROM JobType jt WHERE j.Type = jt.ID) TypeName
	FROM job AS J         
	WHERE J.ID = @JobId


	SELECT *,(ISNULL(QtyReq,0) - ISNULL(QtyIssue,0)) AS QtyVariance,ISNULL(BudgetExt,0) - ISNULL(IssuedExt,0) AS Variance FROM (
		SELECT DISTINCT    
			j.Line AS OpSequence,             
			(select top 1 type from bomt where ID = b.type) as BomType,
			CASE WHEN b.Type=1 THEN (SELECT TOP 1 Name FROM Inv WHERE ID=b.MatItem) WHEN b.Type=2 THEN (SELECT TOP 1 fDesc FROM PRWage WHERE ID=b.MatItem) ELSE (SELECT TOP 1 Name FROM Inv WHERE ID=b.MatItem) END AS MaterialItem,
			j.fDesc AS ItemDesc,             
			j.Code as Code,             
			isnull(b.QtyRequired,0) as QtyReq,             
			ISNULL((SELECT Sum(ISNULL(rp.Quan,0)) AS Comm   
				FROM RPOItem rp   
				INNER JOIN ReceivePO r on r.ID = rp.ReceivePO  
				LEFT JOIN POItem p on r.PO = p.PO AND rp.POLine = p.Line  
				WHERE ISNULL(r.Status,0) = 0 AND p.Job = @JobId and J.Line = p.Phase  
			),0) AS QtyIssue,             
			isnull(b.BudgetUnit,0) as BudgetUnit,
			BudgetExt = isnull(j.Budget,0),
			ISNULL((SELECT Sum(ISNULL(p.Price,0)) AS Comm   
				FROM RPOItem rp   
				INNER JOIN ReceivePO r on r.ID = rp.ReceivePO  
				LEFT JOIN POItem p on r.PO = p.PO AND rp.POLine = p.Line  
				WHERE ISNULL(r.Status,0) = 0 AND p.Job = @JobId and J.Line = p.Phase  
			),0) AS IssuedExt
		 FROM JobTItem j             
		 INNER JOIN Bom b ON b.JobtItemId = j.ID             
		 INNER JOIN Job job ON job.Template = j.JobT            
		 WHERE j.Job=@JobId and j.Type in (1,2)
	 ) AS T
	 ORDER BY OpSequence
END