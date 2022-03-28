CREATE VIEW [dbo].[vw_InvoiceReportDetails]
	AS 
	SELECT       i.Ref As Invoice#, CONVERT(VARCHAR(10),i.fDate,101) As "Invoice Date",
                l.ID As Location, 
                l.Tag As [Location Name], 
                i.fdesc As Description, 
                isnull(l.Remarks,'') As [Location Remarks], 
  	            isnull(j.Remarks,'') As [Job Remarks], 
                i.Amount As [Pre Tax Amount] , 
                i.STax As [Sales Tax], 
                i.Total, 
                i.custom1 as [Manual Invoice], 
                (CASE i.status 
                  WHEN 0 THEN 'Open' 
                  WHEN 1 THEN 'Paid' 
                  WHEN 2 THEN 'Voided' 
                  WHEN 4 THEN 'Marked as Pending' 
                  WHEN 5 THEN 'Paid by Credit Card' 
                  WHEN 3 THEN 'Partially Paid' 
                END + case isnull( ip.paid ,0) WHEN 1 THEN '/Paid by MOM' else '' end )                    AS Status, 
                i.PO, 
                r.Name                  AS [Customer Name], 
                (SELECT Type 
                 FROM   JobType jt 
                 WHERE  jt.ID = i.Type) AS [Department Type],
                 isnull(ar.Balance, 0) AS [Amount Due],CONVERT(VARCHAR(10),ar.due,101) as [Due Date] 
FROM   Invoice i 
       INNER JOIN Loc l 
               ON l.Loc = i.Loc 
       INNER JOIN owner o 
               ON o.id = l.owner 
       INNER JOIN rol r 
               ON o.rol = r.id 
       LEFT OUTER JOIN tblInvoicePayment ip 
               ON i.ref = ip.ref 
       LEFT OUTER JOIN Job j ON i.Job=j.ID 
       LEFT JOIN OpenAR ar  
               ON ar.Ref = i.Ref AND ar.Type = 0   
       WHERE i.ref is not null


Go