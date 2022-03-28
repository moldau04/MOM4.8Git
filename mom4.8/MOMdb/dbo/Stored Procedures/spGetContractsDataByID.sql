CREATE PROC [dbo].[spGetContractsDataByID] 
	@ContractID INT  
AS 
	BEGIN

	           SELECT j.Loc,      
                       j.Owner,      
                       Custom20,      
                       c.Status,      
                       BStart,      
                       BCycle,      
                       BAmt,      
                       c.SStart,      
                       sCycle,      
                       SDate,      
                       SDay,      
                       STime,      
                       ISNULL(CreditCard, 0) CreditCard,      
                       j.Remarks,      
                       l.tag AS locname,      
                       isnull(l.credit,0) as credit,      
                       swe,      
                       c.hours,      
                       j.ctype,      
                       j.fdesc,      
                       j.id,      
                       ExpirationDate, Expiration, frequencies,      
                       l.Billing,      
                       o.Billing AS CustBilling,      
                       o.Central,      
                       c.Chart,      
                       ch.fDesc as GLAcct,      
                       BEscType,      
                       BEscCycle,      
                       BEscFact,      
                       EscLast,      
                       isnull(j.BillRate,0) as BillRate,      
                       isnull(j.RateOT,0) as RateOT,      
                       isnull(j.RateNT,0) as RateNT,      
                       isnull(j.RateMileage,0) as RateMileage,      
                       isnull(j.RateDT,0) as RateDT,      
                       isnull(j.RateTravel,0) as RateTravel,      
                       isnull(j.PO,'') as PO,      
                       j.SPHandle,      
                       j.SRemarks,      
                       j.IsRenewalNotes,      
                       j.RenewalNotes,      
                       c.Detail,      
                       ISNULL(j.Type,0) as DepartmentID,      
                       isnull(j.TaskCategory,'') as TaskCategory,      
                       isnull(l.route,0) Route,      
                       isnull(l.Terr,0) Terr,      
                       isnull(l.Terr2,0) Terr2      
                FROM   Job j      
                       INNER JOIN Contract c  ON c.Job = j.ID      
                       LEFT JOIN Chart ch     ON ch.ID = c.Chart      
                       INNER JOIN Loc l       ON l.Loc = j.Loc      
                       INNER JOIN Owner o     ON o.ID = l.Owner      
                WHERE  j.ID = @ContractID 

	END