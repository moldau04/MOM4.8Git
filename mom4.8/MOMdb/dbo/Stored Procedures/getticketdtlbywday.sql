Create PROCEDURE [dbo].[Spgetticketdtlbywday] (@TicketID INT)
AS
    CREATE TABLE #Temp
      (
         WorkOrder NVARCHAR(100),
         EDate     DATETIME,
         fDesc     NVARCHAR(max)
      );

  BEGIN
      INSERT INTO #Temp
      SELECT WorkOrder,
             EDate,
             Cast(fDesc AS NVARCHAR(max)) AS fDesc
      FROM   TicketO
      WHERE  id = @TicketID
      UNION
      SELECT WorkOrder,
             EDate,
             Cast(fDesc AS NVARCHAR(max)) AS fDesc
      FROM   TicketD
      WHERE  id = @TicketID
  END

  BEGIN
      SELECT id,
             WorkOrder,
             (SELECT fDesc
              FROM   tblWork
              WHERE  ID = fWork)      AS WorkerName,
             fWork,
             EDate,
             Datename(WEEKDAY, EDate) AS WEEKDAY,
             '0'                      reg,
             '0'                      ot,
             '0'                      dt,
             '0'                      nt,
             '0'                      Total
      FROM   TicketO
      WHERE  WorkOrder = (SELECT WorkOrder
                          FROM   #Temp)
             AND id <> @TicketID
             AND Cast(EDate AS DATE) = Cast((SELECT EDate
                                             FROM   #Temp) AS DATE)
      UNION
      SELECT id,
             WorkOrder,
             (SELECT fDesc
              FROM   tblWork
              WHERE  ID = fWork)      AS WorkerName,
             fWork,
             EDate,
             Datename(WEEKDAY, EDate) AS WEEKDAY,
             reg,
             ot,
             dt,
             nt,
             Total
      FROM   TicketD
      WHERE  WorkOrder = (SELECT WorkOrder
                          FROM   #Temp)
             AND id <> @TicketID
             AND Cast(EDate AS DATE) = Cast((SELECT EDate
                                             FROM   #Temp) AS DATE)
  END

  BEGIN      
    SELECT et.fdesc AS Category,
       eti.fDesc,
       CASE
         WHEN (SELECT count(ticketid)
               FROM   RepDetail rd 
      inner join EquipTItem etim on etim.id = rd.EquipTItem and etim.fDesc = eti.fDesc
               WHERE  rd.ticketid = @TicketID) <> 0 THEN 'YES'
         ELSE ''
       END   AS IsYes,
	     (SELECT rd.comment FROM   RepDetail rd 
      inner join EquipTItem etim on etim.id = rd.EquipTItem and etim.fDesc = eti.fDesc
               WHERE  rd.ticketid = @TicketID)  AS Comment
      -- Notes    AS Comment
FROM   EquipTItem eti
       INNER JOIN EquipTemp et
               ON et.ID = eti.EquipT
WHERE  Elev = 0
       AND Upper(et.fdesc) IN ( 'ASCENSEUR A CABLES', 'HYDRAULIC', 'HYDRAULIC A CABLES' )
  END 