create proc GetTicketTime
as
SELECT t.ID, 
        SUBSTRING ( t.DescRes ,0 , 50 )  DescRes,  
       EDate, 
       fUser, 
       QBEmployeeID, 
       QBTimeTxnID, 
       t.LastUpdateDate, 
       reg, 
       ot, 
       dt, 
       tt, 
       nt, 
       Total, 
       (SELECT qblocid 
        FROM   loc 
        WHERE  loc = t.loc) AS QBcustID, 
       CASE 
         WHEN Isnull(QBPayrollItem, '') = '' THEN (SELECT TOP 1 qbwageid 
                                                   FROM   prwage 
                                                   WHERE  fdesc = 'Mobile Service Manager') 
         ELSE qbpayrollitem 
       END                  AS qbwageid, 
       CASE 
         WHEN Isnull(QBServiceItem, '') = '' THEN (SELECT QBInvID 
                                                   FROM   Inv 
                                                   WHERE  Name = 'time spent') 
         ELSE QBServiceItem 
       END                  AS QBitemID 
FROM   TicketD t 
       INNER JOIN tblWork w 
               ON w.ID = t.fWork 
       INNER JOIN tblUser u 
               ON u.fUser = w.fDesc 
WHERE  Isnull(clearcheck, 0) = 1 
       AND Isnull(TransferTime, 0) = 1 
	   and  QBTimeTxnID IS NULL and t.LastUpdateDate >= (select QBLastSync from Control)