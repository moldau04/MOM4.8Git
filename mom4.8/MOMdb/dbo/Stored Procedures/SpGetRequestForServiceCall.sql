CREATE proc [dbo].[SpGetRequestForServiceCall]
		@EN int=0,
		 @UserID int		= 0,
		 @IsSalesAsigned  int=0
AS

DECLARE @SalesAsignedTerrID int = 0
if( @IsSalesAsigned > 0)--If User is  Salesperson
BEGIN
SELECT @SalesAsignedTerrID=isnull(id,0) FROM  Terr WHERE Name=(SELECT fUser FROM  tblUser WHERE id=@IsSalesAsigned)
END
IF (@EN = 1)
BEGIN
SELECT t.who,
       t.lid,
       l.id                                                          AS locid,
       assigned,
       ( l.address + ', ' + l.city + ', ' + l.state + ', ' + l.zip ) AS fulladdress,
       dwork,
       dwork                                                         AS lastname,
       t.ID,
       r.NAME                                                        AS customername,
	   r.EN,
	   B.Name			As Company,
       l.Tag                                                         AS locname,
       l.Address                                                     AS address,
       r.phone,
       t.Cat,
       t.EDate                                                       AS edate,
	   t.cdate,
       CASE
         WHEN assigned = 0 THEN 'Un-Assigned'
         WHEN assigned = 1 THEN 'Assigned'
         WHEN assigned = 2 THEN 'Enroute'
         WHEN assigned = 3 THEN 'Onsite'
         WHEN assigned = 4 THEN 'Completed'
         WHEN assigned = 5 THEN 'Hold'
       END                                                           AS assignname,
       t.Est,
             
       e.id                                                          AS unitid,
       dbo.Ticketequips(t.ID)                                        AS unit,
       dbo.Ticketequipscolumns(t.ID, 'type')                         AS unittype,
       (SELECT type
        FROM   jobtype
        WHERE  id = t.type)                                          AS department
FROM   ticketo t
       LEFT OUTER JOIN TicketDPDA dp
                    ON t.ID = dp.ID
       INNER JOIN Loc l
               ON l.Loc = t.lid
       INNER JOIN Owner o
               ON l.Owner = o.ID
       INNER JOIN Rol r
               ON r.ID = o.Rol
		LEFT OUTER JOIN Branch B on B.ID = r.EN
		LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = r.EN
       LEFT OUTER JOIN Elev e
                    ON e.ID = t.LElev
WHERE  t.fBy = 'portal' 
 and t.Assigned=0 and UC.IsSel = 1 and UC.UserID = @UserID  
 and ( isnull(l.Terr,0) = ( case when @SalesAsignedTerrID > 0 then ( @SalesAsignedTerrID) else 1 end )  or isnull(l.Terr2,0) = ( case when @SalesAsignedTerrID > 0 then ( @SalesAsignedTerrID) else 1 end ) )
END
ELSE

BEGIN

SELECT t.who,
       t.lid,
       l.id                                                          AS locid,
       assigned,
       ( l.address + ', ' + l.city + ', ' + l.state + ', ' + l.zip ) AS fulladdress,
       dwork,
       dwork                                                         AS lastname,
       t.ID,
       r.NAME                                                        AS customername,
	   r.EN,
	   B.Name			As Company,
       l.Tag                                                         AS locname,
       l.Address                                                     AS address,
       r.phone,
       t.Cat,
       t.EDate                                                       AS edate,
	   t.cdate,
       CASE
         WHEN assigned = 0 THEN 'Un-Assigned'
         WHEN assigned = 1 THEN 'Assigned'
         WHEN assigned = 2 THEN 'Enroute'
         WHEN assigned = 3 THEN 'Onsite'
         WHEN assigned = 4 THEN 'Completed'
         WHEN assigned = 5 THEN 'Hold'
       END                                                           AS assignname,
       t.Est,
             
       e.id                                                          AS unitid,
       dbo.Ticketequips(t.ID)                                        AS unit,
       dbo.Ticketequipscolumns(t.ID, 'type')                         AS unittype,
       (SELECT type
        FROM   jobtype
        WHERE  id = t.type)                                          AS department
FROM   ticketo t
       LEFT OUTER JOIN TicketDPDA dp
                    ON t.ID = dp.ID
       INNER JOIN Loc l
               ON l.Loc = t.lid
       INNER JOIN Owner o
               ON l.Owner = o.ID
       INNER JOIN Rol r
               ON r.ID = o.Rol
		LEFT OUTER JOIN Branch B on B.ID = r.EN
       LEFT OUTER JOIN Elev e
                    ON e.ID = t.LElev
WHERE  t.fBy = 'portal'
 
 and t.Assigned=0

END
SELECT count(t.ID) as RequestForServiceCount
FROM   ticketo t
INNER JOIN Loc l  ON l.Loc = t.lid
WHERE  t.fBy = 'portal' 
and t.Assigned=0
 and ( isnull(l.Terr,0) = ( case when @SalesAsignedTerrID > 0 then ( @SalesAsignedTerrID) else isnull(l.Terr,0) end )  or isnull(l.Terr2,0) = ( case when @SalesAsignedTerrID > 0 then ( @SalesAsignedTerrID) else isnull(l.Terr2,0) end ) )
GO