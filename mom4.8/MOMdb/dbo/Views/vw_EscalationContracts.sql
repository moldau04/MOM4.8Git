CREATE VIEW [dbo].[vw_EscalationContracts]
	AS 

	SELECT 
		loc.id AS [Location Id],
		Loc.Tag  AS [Location Name],
		Job.CType As [Service Type],
		job.fdesc [Description],
	    CASE Contract.bcycle 
         WHEN 0 THEN 'Monthly' 
         WHEN 1 THEN 'Bi-Monthly' 
         WHEN 2 THEN 'Quarterly' 
         WHEN 3 THEN '3 Times/Year' 
         WHEN 4 THEN 'Semi-Annually' 
         WHEN 5 THEN 'Anually' 
         WHEN 6 THEN 'Never' 
       END                    [Billing Freqency],
	   CASE Contract.BEscType 
         WHEN 0 THEN 'Commodity Index' 
         WHEN 1 THEN 'Escalation' 
         WHEN 2 THEN 'Return' 
         WHEN 3 THEN 'Manual' 
       END                    [Esc Type],
	    CASE Contract.BEscType 
         WHEN 1 THEN 'Escalate'
		 else 'Renew' 
       END                    Action,
       Contract.BEscCycle As [Esc Cycle],       
       --Contract.BEscType As [Esc Type],
	   Contract.BEscFact As [Esc Factor],
	   CONVERT(VARCHAR(10),isnull(Contract.EscLast,'1900-1-1'),101) as [Last Esc],
	   CONVERT(VARCHAR(10),isnull(Contract.BStart,'1900-1-1'),101) as [Start Esc],
	   CONVERT(VARCHAR(10),isnull(Contract.Bfinish,'1900-1-1'),101) as [Finish Esc],
	   CONVERT(VARCHAR(10),dateadd(month,BEscCycle,isnull(Contract.EscLast,'1900-1-1')),101) as [Next Due],
	   Contract.Bamt As Amount, 
	   (case BEscType when 1 then convert(numeric(30,2), (BAmt + ((BAmt * BEscFact)/100))) else BAmt end) as [New Amount],
       Contract.BLenght As [Length],
       Contract.Job   As [Contract] ,
	   CONVERT(VARCHAR(10),isnull(Contract.ExpirationDate,'1900-1-1'),101) as [Expiration Date],
	   case IsRenewalNotes when 1 then RenewalNotes else '' end as [Renewal Notes]
FROM   ((Loc
         INNER JOIN Rol
                 ON Loc.Rol = Rol.ID)
        INNER JOIN Job
                ON Job.Loc = Loc.Loc)
       INNER JOIN Contract
               ON Job.ID = Contract.Job
WHERE  
--(( ( Dateadd(month, Contract.BEscCycle, Contract.Esclast) ) <= @EscDate ))
	--	or ( ExpirationDate <= @EscDate )
       --AND 
	   Contract.Status = 0
--ORDER  BY dateadd(month,BEscCycle,Contract.EscLast)


GO



