CREATE Proc [dbo].[spGetTicketDetails]
@Tickets AS [dbo].[tblTypeTickets] Readonly 
as

declare @sign table([Signature] image, ticketid int) 
declare @ticketID int
declare @worker int
DECLARE db_cursor CURSOR FOR select TicketID, dp.fwork from @Tickets t inner join ticketdpda dp on t.ticketid=dp.id
OPEN db_cursor
FETCH NEXT FROM db_cursor INTO @ticketID,@worker
WHILE @@FETCH_STATUS = 0
BEGIN
		
insert into @sign exec spGetDPDASignature @ticketID, @worker

FETCH NEXT FROM db_cursor INTO @ticketID,@worker
END
CLOSE db_cursor
DEALLOCATE db_cursor		

declare @signed int = 1
--if exists(select * from custom where name ='Ticket1' and Label='signed by')
--begin
--set @signed=1
--end
--else
--begin
--set @signed=0
--end
		
SELECT t.ID,t.WorkOrder,t.Who,t.CDate,t.EDate,t.TimeRoute,t.TimeSite,t.TimeComp,t.fDesc,t.DescRes,
case when @signed=1 then isnull(t.Custom1,'') else '' end as Custom1,
(SELECT unit 
        FROM   elev 
        WHERE  id = t.elev)                                          AS unitname, 
       (SELECT state 
        FROM   elev 
        WHERE  id = t.elev)                                          AS unitstate, 
       Isnull(ClearCheck, 0)                                         AS ClearCheck1, 
       Isnull(Charge, 0)                                             AS chargen, 
       t.Reg, 
       t.OT, 
       t.NT, 
       t.DT, 
       t.TT, 
       t.Total, 
       Upper(w.fDesc)                                                AS dworkup, 
       w.super                                                       AS superv, 
       (SELECT TOP 1 signature 
        FROM   pdaticketsignature 
        WHERE  pdaticketid = t.ID)                                   AS signature, 
       ( reg + NT + OT + TT + DT )                                   AS tottime, 
       (SELECT TOP 1 NAME 
        FROM   rol 
        WHERE  ID = (SELECT TOP 1 Rol 
                     FROM   Owner 
                     WHERE  ID = l.Owner))                           AS customerName, 
       l.tag                                                         AS locname, 
	   l.Type                                                        AS LocType,
	   l.Custom1                                                     AS LocCustom1,
	   l.Custom2                                                     AS LocCustom2,
       l.Owner, 
       l.Loc                                                         AS lid, 
       l.Address                                                     AS ldesc3, 
       ( l.Address + ', ' + l.City + ', ' + l.State + ', ' + l.Zip ) AS ldesc4, 
       ( l.Address + ', ' + l.City + ', ' + l.State + ', ' + l.Zip ) AS Address, 
       cat, 
       l.City, 
       l.State, 
       l.Zip, 
       Elev                                                          AS lelev, 
       r.phone, 
       CPhone, 
       4                                                             AS assigned, 
       Upper(w.fDesc)                                                AS dwork, 
       descres, 
       'Completed'                                                   AS assignname, 
       bremarks, 
       Isnull(t.workcomplete, 0)                                     AS workcmpl, 
       Isnull(invoice, 0)                                            AS invoice, 
       manualinvoice, 
       r.contact, 

       r.cellular, 
       Isnull(QBinvoiceID, '')                                       AS QBinvoiceID, 
       Isnull(transfertime, 0)                                       AS timetransfer, 
       Isnull(Customtick3, 0)                                        AS Customticket3, 
       Isnull(Customtick4, 0)                                        AS Customticket4, 
       0                                                             AS highdecline, 
       t.othere, 
       t.toll, 
       t.zone, 
       t.Smile, 
       t.emile, 
       t.internet ,
	    (select top 1 sageid from owner where id = (select top 1 owner from loc where loc = t.loc)) as sagecust, 
		(select top 1 id from loc where loc = t.loc) as sageloc,
	   (select top 1 type from jobtype where ID = t.type) as department,t.Job
FROM   TicketD t 
       INNER JOIN Loc l 
               ON l.Loc = t.Loc 
       INNER JOIN tblWork w 
               ON t.fWork = w.ID 
       INNER JOIN rol r 
               ON r.id = l.rol  
where t.ID in (select * from @Tickets)
               
UNION ALL
    
SELECT  t.ID,t.WorkOrder,t.Who,t.CDate,t.EDate,t.TimeRoute,t.TimeSite,t.TimeComp,t.fDesc,dp.DescRes,
		case when @signed=1 then isnull(t.Custom1,'') else '' end as Custom1,
       (SELECT unit 
        FROM   elev 
        WHERE  id = t.LElev)               AS unitname, 
       (SELECT state 
        FROM   elev 
        WHERE  id = t.LElev)               AS unitstate, 
       0                                   AS ClearCheck1, 
       Isnull(dp.Charge, 0)                   AS chargen, 
       dp.Reg, 
       dp.OT, 
       dp.NT, 
       dp.DT, 
       dp.TT, 
       dp.Total, 
       Upper(DWork)                        AS dworkup, 
       (SELECT Super 
        FROM   tblWork w 
        WHERE  w.fdesc = DWork)            AS superv, 
        
        case t.Assigned when 4 then
		(SELECT TOP 1 signature 
        FROM   @sign 
        WHERE  ticketid = t.ID)  
        else
        (SELECT TOP 1 signature 
        FROM   pdaticketsignature 
        WHERE  pdaticketid = t.ID) 
        end                                  AS signature,
         
       (isnull( reg ,0)+ isnull(  NT,0) + isnull(  OT ,0)+ isnull(  TT,0) +isnull(  DT,0) )AS tottime, 
       (SELECT TOP 1 NAME 
        FROM   rol 
        WHERE  ID = (SELECT TOP 1 Rol 
                     FROM   Owner 
                     WHERE  ID = t.Owner)) AS customerName, 
       t.LDesc2                            AS locname, 
	   l.Type							   AS LocType,
	   l.Custom1                           AS LocCustom1,
	   l.Custom1                           AS LocCustom2,
       t.Owner,
       lid, 
       ldesc3, 
       ldesc4, 
       ( ldesc3 + ' ' + ldesc4 )           AS address, 
       t.Cat, 
       (select city from loc where loc = t.lid) as City, 
       (select State from loc where loc = t.lid) as State, 
       (select Zip from loc where loc = t.lid) as Zip, 
       Elev                                                          AS lelev, 
       ( CASE 
           WHEN t.Owner IS NULL THEN (SELECT r.Phone 
                                      FROM   Rol r 
                                             INNER JOIN Prospect p 
                                                     ON p.Rol = r.ID 
                                      WHERE  p.ID = t.LID) 
           ELSE (SELECT r.contact 
                 FROM   Rol r 
                        INNER JOIN Loc l 
                                ON l.Rol = r.ID 
                 WHERE  l.Loc = t.LID) 
         END )                             AS Phone, 
       t.CPhone, 
       assigned, 
       dwork, 
       dp.descres, 
       CASE 
         WHEN Assigned = 1 THEN 'Assigned' 
         WHEN Assigned = 2 THEN 'Enroute' 
         WHEN Assigned = 3 THEN 'Onsite' 
         WHEN Assigned = 4 THEN 'Completed' 
         WHEN Assigned = 5 THEN 'Hold' 
       END                                 AS assignname, 
       t.bremarks, 
       Isnull(dp.workcomplete, 0)          AS workcmpl, 
       0                                   AS invoice, 
       ''                                  AS manualinvoice, 
       ( CASE 
           WHEN t.Owner IS NULL THEN (SELECT r.contact 
                                      FROM   Rol r 
                                             INNER JOIN Prospect p 
                                                     ON p.Rol = r.ID 
                                      WHERE  p.ID = t.LID) 
           ELSE (SELECT r.contact 
                 FROM   Rol r 
                        INNER JOIN Loc l 
                                ON l.Rol = r.ID 
                 WHERE  l.Loc = t.LID) 
         END )                             AS contact, 
       ( CASE 
      
           WHEN t.Owner IS NULL THEN (SELECT r.Cellular 
                                      FROM   Rol r 
                                             INNER JOIN Prospect p 
                                                     ON p.Rol = r.ID 
                                      WHERE  p.ID = t.LID) 
           ELSE (SELECT r.contact 
                 FROM   Rol r 
                        INNER JOIN Loc l 
                                ON l.Rol = r.ID 
                 WHERE  l.Loc = t.LID) 
         END )                             AS Cellular, 
       ''                                  AS QBinvoiceID, 
       0                                   AS timetransfer, 
       Isnull(Customtick3, 0)              AS Customticket3, 
       Isnull(Customtick4, 0)              AS Customticket4 ,
       0                                  AS highdecline, 
       dp.othere, 
       dp.toll, 
       dp.zone, 
       dp.Smile, 
       dp.emile, 
       dp.internet ,
	   (select top 1 sageid from owner where id = t.owner) as sagecust,       
	   (select top 1 id from loc where loc = t.lid) as sageloc,
	   (select top 1 type from jobtype where ID = t.type) as department,t.Job
FROM   TicketO t 
       left outer JOIN TicketDPDA dp  ON dp.ID = t.ID 
	   INNER JOIN Loc l ON l.Loc = t.LID 
WHERE  t.ID in (select * from @Tickets)
order by t.ID

SELECT Quan, fDesc, Ticket FROM POItem where Ticket in  (select * from @Tickets)
SELECT Quan, fDesc, Ticket FROM TicketI where Ticket in  (select * from @Tickets)  