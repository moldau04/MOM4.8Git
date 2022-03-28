CREATE PROCEDURE [dbo].[spSearchTicketInLocation]        (   
           @Loc    INT 		  
		   ,@prefix VARCHAR(100)
		   ,@year int 
)
AS 
BEGIN

IF @prefix<>''
BEGIN
	SELECT 
		d.ID as ID, 
		d.workorder, 
		ISNULL(tblwork.fDesc,'')  AS dwork, 
		d.fWork as fWork,  
		Case d.Assigned  
			WHEN 0 then 'Un-Assigned' 
			WHEN 1 then 'Assigned' 
			WHEN 2 then 'Enroute'
			WHEN 3 then 'Onsite' 
			WHEN 4 then 'Completed' 
			WHEN 5 then 'Hold' 
			WHEN 6 then 'Voided' 
		END AS TicketStatus,
		CONVERT(varchar, d.EDate, 101)  AS EDate
		 
	FROM   TicketD d 
	LEFT JOIN tblwork  on tblwork.ID= d.fWork 
	WHERE  d.loc = @Loc
	AND CONVERT(VARCHAR(50), d.ID) LIKE '%'+@prefix+'%'
	
	UNION ALL 
  
	SELECT
	o.ID, 
	o.WorkOrder, 
	tblwork.fDesc  AS dwork, 
	o.fWork, 
	'Assigned',
	
		CONVERT(varchar, o.EDate, 101)  AS EDate
	FROM   TicketO o 
	LEFT OUTER JOIN TicketDPDA dp ON dp.ID=o.ID 
	LEFT JOIN tblwork  on tblwork.ID= o.fWork 
	WHERE  o.LID = @Loc
	AND CONVERT(VARCHAR(50), o.ID) LIKE  '%'+@prefix+'%'
END 
ELSE
BEGIN
	SELECT 
		d.ID as ID, 
		d.workorder, 
		ISNULL(tblwork.fDesc,'')  AS dwork, 
		d.fWork as fWork,  
		Case d.Status  
			WHEN 0 then 'Un-Assigned' 
			WHEN 1 then 'Assigned' 
			WHEN 2 then 'Enroute'
			WHEN 3 then 'Onsite' 
			WHEN 4 then 'Completed' 
			WHEN 5 then 'Hold' 
			WHEN 6 then 'Voided' 
		END AS TicketStatus,
		CONVERT(varchar, d.EDate, 101)  AS EDate
	FROM   TicketD d 
	LEFT JOIN tblwork  on tblwork.ID= d.fWork 
	WHERE  d.loc = @Loc

	
	UNION ALL 
  
	SELECT
	o.ID, 
	o.WorkOrder, 
	tblwork.fDesc  AS dwork, 
	o.fWork, 
	'Assigned',
		CONVERT(varchar, o.EDate, 101)  AS EDate
	FROM   TicketO o 
	LEFT OUTER JOIN TicketDPDA dp ON dp.ID=o.ID 
	LEFT JOIN tblwork  on tblwork.ID= o.fWork 
	WHERE  o.LID = @Loc

END 




END



