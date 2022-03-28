CREATE PROC spGetTicketDataByID
@TicketID int
AS 
 --Declare  @TicketID int =1330151;
BEGIN

Declare @IsTicketD int =0;
Declare @IsTicketO int =0;
Declare @IsTicketDPDA int =0;
Declare @IsTicketProspect int =0;

IF EXISTS (select 1 from TicketD where ID=@TicketID) 
BEGIN
SET @IsTicketD=1;
IF NOT EXISTS (SELECT 1 FROM TicketD INNER JOIN Loc ON Loc.Loc =TicketD.Loc WHERE TicketD.ID=@TicketID  ) SET @IsTicketProspect=1;
END

IF EXISTS (SELECT 1 FROM TicketO WHERE ID=@TicketID) 
BEGIN
SET @IsTicketO=1;
IF NOT EXISTS (SELECT 1 FROM TicketO INNER JOIN Loc ON Loc.Loc =TicketO.LID WHERE TicketO.ID=@TicketID and TicketO.LType=0) SET @IsTicketProspect=1;
END

IF EXISTS (select 1 from TicketDPDA where ID=@TicketID) 
BEGIN
SET @IsTicketDPDA=1;
IF NOT EXISTS (SELECT 1 FROM TicketDPDA d INNER JOIN TicketO o on o.ID=d.ID  INNER JOIN Loc ON Loc.Loc =d.Loc WHERE d.ID=@TicketID and o.LType=0 ) SET @IsTicketProspect=1;
END


 
        Declare @Workid nvarchar(100)  ='';   
		
		SELECT  @Workid =t.fWork from (
		 select fWork from TicketD   where ID=@TicketID	
		 Union
         select fWork from TicketO    where ID=@TicketID 
		 Union
         select fWork from TicketDPDA where ID=@TicketID   ) as t

		Declare  @tsignature table ( signature image ,  TicketID  int )

        IF EXISTS(select 1 from sysobjects where name =  'PDA_' + @Workid )  
		
	    BEGIN  insert into  @tsignature  EXEC('  SELECT TOP 1  signature  ,  '+@TicketID+'    FROM    PDA_'+ @Workid +'   WHERE  pdaticketid = '+@TicketID)       END
		ELSE 
		BEGIN  insert into  @tsignature  EXEC('  SELECT TOP 1  signature ,  '+@TicketID+'    FROM    pdaticketsignature    WHERE  pdaticketid = '+@TicketID)  END
	 

-----Completed Ticket

IF (@IsTicketD=1   and @IsTicketProspect=0)

BEGIN

select 'TicketD' tablename, @IsTicketProspect as IsTicketProspect , (select  charge from job where ID = t.job) as isJobChargeable ,dbo.TicketEquips(t.ID) as unit,(select unit from elev where id =t.elev)as unitname,
(select state from elev where id =t.elev)as unitstate,isnull(ClearCheck ,0 ) as ClearCheck1 , isnull(ClearPR ,0 )  ClearPR , 
                         Isnull(t.Reg,0) as Reg, Isnull(t.OT,0) as OT,Isnull(t.NT,0) as NT ,Isnull(t.DT,0) as DT ,Isnull(t.TT,0) as TT,Isnull(t.break_time,0)  as BT  
						 ,t.Comments as Comments, t.PartsUsed as PartsUsed,  t.Total,UPPER(w.fDesc)as dworkup, w.super as superv, (reg + NT + OT + TT + DT)as tottime, (select top 1 Name from rol where ID=(select top 1 Rol from Owner where ID=l.Owner))
						 as customerName,l.tag as locname , l.Type AS LocType, l.Custom1 AS LocCustom1, l.Custom2 AS LocCustom2, l.Owner,l.Loc as lid, l.Address 
						 as ldesc3,(l.Address+', '+l.City+', '+ l.State+', '+ l.Zip) as ldesc4,(l.Address+', '+l.City+', '+ l.State+', '+ l.Zip) as Address, cat, l.City, l.State, l.Zip,
						 Elev as lelev, (select top 1 Phone from rol where ID=l.Rol) as phone,
						 CPhone,4 as assigned,UPPER( w.fDesc )as dwork, descres, 'Completed' as assignname,bremarks, isnull( t.workcomplete,0) as workcmpl, isnull(invoice,0) as 
						 invoice, manualinvoice,r.contact , r.phone, r.cellular,r.remarks, isnull( QBinvoiceID,'') as QBinvoiceID, isnull( transfertime,0) as timetransfer , 
						 isnull(Customtick3,0)AS Customticket3, isnull(Customtick4,0)AS Customticket4, 0 as highdecline ,  (select top 1 sageid from owner where id =
						 (select top 1 owner from loc where loc = t.loc)) as sagecust, (select top 1 id from loc where loc = t.loc) as sageloc,
						 (select top 1 type from jobtype where ID = t.type) as department ,(select convert(varchar(20),t.job ) +'-'+ fdesc from job where ID = t.job) as jobdesc,
						 jobitemdesc as jobitemdesc1, dbo.Afterhours(t.timeroute,t.timecomp) as afterhours, dbo.weekends(t.edate) as weekends, 0 as EmailNotified, null as EmailTime   ,
						 t.JobCode as JobCode1 ,
						       t.[ID]
      ,t.[CDate]
      ,t.[DDate]
      ,t.[EDate]
      ,t.[fWork]
      ,t.[Job]
      ,t.[Loc]
      ,t.[Elev]
      ,t.[Type]
      ,t.[Charge]
      ,t.[fDesc]
      ,t.[DescRes]
      ,t.[ClearCheck] 
      ,t.[Total]
      ,t.[Who]
      ,t.[OT]
      ,t.[DT]
      ,t.[TT]
      ,t.[Zone]
      ,t.[Toll]
      ,t.[OtherE] as othere
      ,t.[Status]
      ,t.[Invoice]
      ,t.[Level]
      ,t.[Est]
      ,t.[Cat]
      ,t.[Who]
      ,t.[fBy]
      ,t.[SMile]
      ,t.[EMile]
      ,t.[fLong]
      ,t.[Latt]
      ,t.[WageC]
      ,t.[Phase]
      ,t.[Car]
      ,t.[CallIn]
      ,t.[Mileage]
      ,t.[NT]
      ,t.[CauseID]
      ,t.[CauseDesc]
      ,t.[fGroup]
      ,t.[PriceL]
      ,t.[WorkOrder]
      ,t.[TimeRoute]
      ,t.[TimeSite]
      ,t.[TimeComp]
      ,t.[Source]
      ,t.[Internet]
      ,t.[RBy]
      ,t.[Custom1]
      ,t.[Custom2]
      ,t.[Custom3]
      ,t.[Custom4]
      ,t.[Custom5]
      ,t.[CTime]
      ,t.[DTime]
      ,t.[ETime]
      ,t.[BRemarks]
      ,t.[WorkComplete]
      ,t.[BReview]
      ,t.[PRWBR]
      ,t.[pdaticketid]
      ,t.[AID]
      ,t.[Custom6]
      ,t.[Custom7]
      ,t.[Custom8]
      ,t.[Custom9]
      ,t.[Custom10]
      ,t.[CPhone]
      ,t.[RegTrav]
      ,t.[OTTrav]
      ,t.[DTTrav]
      ,t.[NTTrav]
      ,t.[Email]
      ,t.[ManualInvoice]
      ,t.[QBInvoiceID]
      ,t.[LastUpdateDate]
      ,t.[QBTimeTxnID]
      ,t.[TransferTime]
      ,t.[QBServiceItem]
      ,t.[QBPayrollItem]
      ,t.[CustomTick1]
      ,t.[CustomTick2]
      ,t.[CustomTick3]
      ,t.[CustomTick4]
      ,t.[TimesheetID]
      ,t.[HourlyRate]
      ,t.[CustomTick5]
      ,t.[JobCode]
      ,t.[Import1]
      ,t.[Import2]
      ,t.[Import3]
      ,t.[Import4]
      ,t.[Import5]
      ,t.[Recurring]
      ,t.[JobItemDesc]
      ,t.[PrimarySyncID]
      ,t.[FMSEtid]
      ,t.[PrevEquipLoc]
      ,t.[fmsimportdate]
      ,t.[break_time]
      ,t.[Comments]
      ,t.[PartsUsed]
      ,t.[TimeCheckOut]
      ,t.[TimeCheckOutFlag]
      ,t.[Assigned]
	  , l.Status as locStatus 
	  , ts.signature    AS signature
    from TicketD t WITH(NOLOCK) 
    INNER JOIN LOC l          WITH(NOLOCK) on l.Loc=t.Loc  
    LEFT OUTER JOIN tblWork w WITH(NOLOCK) on t.fWork=w.ID  
    INNER JOIN ROL r          WITH(NOLOCK) on r.id=l.rol
	left join @tsignature ts on ts.TicketID=t.ID 
	where t.ID=@TicketID
END


---- Completed Ticket DPDA
IF (@IsTicketDPDA=1 and @IsTicketProspect=0)

BEGIN

        
      
	   
            SELECT 'TicketDPDA' tablename,@IsTicketProspect as IsTicketProspect ,  dp.Recommendations as bremarks,   
            Isnull(dp.Charge, 0)                   AS charge,   t.*,  
            dbo.TicketEquips(t.ID) as unit ,  
            (SELECT unit    FROM   elev  WHERE  id = t.lelev)   AS unitname,  
            (SELECT state   FROM   elev  WHERE  id = t.lelev)   AS unitstate,   
            0                                     AS ClearCheck1,  
            0                   AS ClearPR,  
            Isnull(dp.Reg,0) as Reg,  
            Isnull(dp.OT,0) as OT,  
            Isnull(dp.NT,0) as NT,  
            Isnull(dp.DT,0) as DT,  
            Isnull(dp.TT,0) as TT,  
            Isnull(dp.break_time,0) as BT,  
            dp.Comments as Comments,  
            dp.PartsUsed as PartsUsed,  
            dp.Total,  
             Upper(DWork)                        AS dworkup,  
            (SELECT Super   FROM   tblWork w       WHERE  w.fdesc = DWork)             AS superv, 
            dp.Total                            AS tottime,  
            0                                   AS Reg,  
            0                                   AS NT,  
            0                                   AS OT,  
            0                                   AS TT,  
            0                                   AS DT,  
            t.LDesc2                            AS locname,  
            t.LID                               AS LID,  
            l.Type                              AS LocType,  
            l.Custom1                           AS LocCustom1,  
            l.Custom2                           AS LocCustom2,  
            (SELECT TOP 1 NAME  
            FROM   rol  
            WHERE  ID = (SELECT TOP 1 Rol               FROM   Owner             WHERE  ID = t.Owner)) AS customerName,  
            dp.descres,  
            CASE  
            WHEN Assigned = 1 THEN 'Assigned'  
            WHEN Assigned = 2 THEN 'Enroute'  
            WHEN Assigned = 3 THEN 'Onsite'  
            WHEN Assigned = 4 THEN 'Completed'  
            WHEN Assigned = 5 THEN 'Hold'  
            END                                 AS assignname,  
            dp.Recommendations as bremarks,  
            ( ldesc3 + ' ' + ldesc4 )           AS address,  
            l.Address as ldesc3,  
            l.City,  
            l.State,  
            l.Zip,  
            Isnull(dp.workcomplete, 0)          AS workcmpl,  
            dp.othere,  
            dp.toll,  
            dp.zone,  
            dp.Smile,  
            dp.emile,  
            dp.internet,  
            dp.TimeCheckOut,  
            dp.Invoice                                   AS invoice,  
            ''                                           AS manualinvoice,  
            ( CASE  
                WHEN t.Owner IS NULL THEN (SELECT r.contact  FROM   Rol r  INNER JOIN Prospect p  ON p.Rol = r.ID     WHERE  p.ID = t.LID)  
                ELSE (SELECT r.contact  FROM   Rol r  INNER JOIN Loc l ON l.Rol = r.ID  
                    WHERE  l.Loc = t.LID)  
            END )                             AS contact,  
            ( CASE  
                WHEN t.Owner IS NULL THEN (SELECT r.Phone FROM   Rol r INNER JOIN Prospect p   ON p.Rol = r.ID  
                                            WHERE  p.ID = t.LID)  
                ELSE (SELECT r.contact FROM   Rol r INNER JOIN Loc l   ON l.Rol = r.ID  
                    WHERE  l.Loc = t.LID)  
            END )                             AS Phone,  
            ( CASE  
                WHEN t.Owner IS NULL THEN (SELECT r.Cellular  
                                            FROM   Rol r INNER JOIN Prospect p ON p.Rol = r.ID WHERE  p.ID = t.LID)  
                ELSE (SELECT r.contact FROM   Rol r INNER JOIN Loc l ON l.Rol = r.ID WHERE  l.Loc = t.LID)  
            END )                             AS Cellular,  
            ( CASE  
                WHEN t.Owner IS NULL THEN (SELECT r.Remarks  
                                            FROM   Rol r  
                                                INNER JOIN Prospect p  
                                                        ON p.Rol = r.ID  
                                            WHERE  p.ID = t.LID)  
                ELSE (SELECT l.Remarks  
                    FROM   Rol r  
                            INNER JOIN Loc l  
                                    ON l.Rol = r.ID  
                    WHERE  l.Loc = t.LID)  
            END )                               AS Remarks,  
            ''                                  AS QBinvoiceID,  
            0                                   AS timetransfer,  
            ''                                  AS QBServiceItem,  
            ''                                  AS QBPayrollItem,  
            0                                   AS highdecline,  
            Isnull(Customtick3, 0)              AS Customticket3,  
            Isnull(Customtick4, 0)              AS Customticket4,  
            (select top 1 sageid from owner where id = t.owner) as sagecust,   
            (select top 1 id from loc where loc = t.lid) as sageloc,   
            dp.wagec, isnull(dp.Phase,(select top 1  line  from JobTItem where Job=t.job and 
			Type=1 and fDesc='Labor' and job <> 0))
			Phase, (select top 1 type from jobtype where ID = t.type) as department, 
			(select convert(varchar(20),t.job ) +'-'+ fdesc 
			from job where ID = t.job) as jobdesc,
            case isnull(t.jobitemdesc, '') when   '' then isnull(t.jobitemdesc, 
			(select top 1  fDesc  from JobTItem where Job = t.job and Type = 1 and fDesc = 'Labor' and job <> 0))   
			else t.jobitemdesc end  jobitemdesc1
    , dbo.Afterhours(t.timeroute,t.timecomp) as afterhours, dbo.weekends(t.edate) as weekends, isnull(EmailNotified,0) 
	as EmailNotified, EmailTime   
    ,    (select  charge from job where ID = t.job) as isJobChargeable  ,isnull(t.jobCode,(select top 1  Code  
	from JobTItem where Job=t.job and Type=1 and fDesc='Labor' and job <>0)) JobCode1 
	, l.Status as locStatus
	,  ts.signature                        AS signature 
FROM TicketO t            WITH(NOLOCK) 
INNER JOIN TicketDPDA dp WITH(NOLOCK) ON dp.ID = t.ID  
LEFT JOIN LOC l           WITH(NOLOCK) on l.Loc=t.LID  
LEFT JOIN tblWork w       WITH(NOLOCK) on t.fWork=w.ID  
INNER JOIN ROL r          WITH(NOLOCK) on r.id=l.rol
left join @tsignature ts on ts.TicketID=t.ID
WHERE t.ID=@TicketID
      
                                 
END
 

---- Open Ticket
IF (@IsTicketO=1    and @IsTicketProspect=0)

BEGIN

SELECT 'ticketo' tablename,  @IsTicketProspect as IsTicketProspect ,
                          dbo.TicketEquips(t.ID) as unit ,   
                            (SELECT unit   FROM   elev  WHERE  id = t.lelev)               AS unitname,   
                            (SELECT state  FROM   elev  WHERE  id = t.lelev)               AS unitstate,   
                            0                                   AS ClearCheck1,   
                            0                                   AS ClearPR,   
                            0                                   AS Charge,   
                            0.00                                AS Reg,   
                            0                                   AS OT,   
                            0                                   AS NT,   
                            0                                   AS DT,   
                            0                                   AS TT,   
                            0                                   AS BT,   
                            '' as Comments,   
                            '' as PartsUsed,   
                            0                                   AS Total,   
                            Upper(t.DWork)                      AS dworkup,   
                            (SELECT Super   
                             FROM   tblWork w   
                             WHERE  w.fdesc = t.dwork)          AS superv, 
                            0                                   AS tottime,   
                            0                                   AS Reg,   
                            0                                   AS NT,   
                            0                                   AS OT,   
                            0                                   AS TT,   
                            0                                   AS DT,   
                            t.LDesc2                            AS locname,   
                            t.LID                               AS LID,   
                            l.Type                              AS LocType,   
                            l.Custom1                           AS LocCustom1,   
                            l.Custom2                           AS LocCustom2,   
                            (SELECT TOP 1 Name   
                             FROM   rol   
                             WHERE  ID = (SELECT TOP 1 Rol FROM   Owner WHERE  ID = t.Owner)) AS customerName,   
                            ''                                  AS descres,   
                            CASE   
                              WHEN Assigned = 1 THEN 'Assigned'   
                              WHEN Assigned = 2 THEN 'Enroute'   
                              WHEN Assigned = 3 THEN 'Onsite'   
                              WHEN Assigned = 4 THEN 'Completed'   
                              WHEN Assigned = 5 THEN 'Hold'   
                            END                                 AS assignname,   
                            t.bremarks,   
                            ( ldesc3 + ' ' + ldesc4 )           AS address,   
                            l.Address as ldesc3,   
                            l.City,   
                            l.State,   
                            l.Zip,   
                            1                                   AS workcmpl,   
                            0                                   AS othere,   
                            0                                   AS toll,   
                            0                                   AS zone,   
                            0                                   AS Smile,   
                            0                                   AS emile,   
                            0                                   AS internet,   
                            0                                   AS invoice,   
                            ''                                  AS manualinvoice,   
                            ( CASE   
                                WHEN t.Owner IS NULL THEN (SELECT r.contact FROM   Rol r  INNER JOIN Prospect p   ON p.Rol = r.ID   
                                                           WHERE  p.ID = t.LID)   
                                ELSE (SELECT r.contact   
                                      FROM   Rol r   INNER JOIN Loc l  ON l.Rol = r.ID   
                                      WHERE  l.Loc = t.LID)   
                              END )                             AS contact,   
                            ( CASE   
                                WHEN t.Owner IS NULL THEN (SELECT r.Phone   
                                                           FROM   Rol r  INNER JOIN Prospect p ON p.Rol = r.ID   
                                                           WHERE  p.ID = t.LID)   
                                ELSE (SELECT r.contact   
                                      FROM   Rol r   
                                             INNER JOIN Loc l   
                                                     ON l.Rol = r.ID   
                                      WHERE  l.Loc = t.LID)   
                              END )                             AS Phone,   
                            ( CASE   
                                WHEN t.Owner IS NULL THEN (SELECT r.Cellular   
                                                           FROM   Rol r  INNER JOIN Prospect p  ON p.Rol = r.ID   
                                                           WHERE  p.ID = t.LID)   
                                ELSE (SELECT r.contact   
                                      FROM   Rol r   INNER JOIN Loc l  ON l.Rol = r.ID   
                                      WHERE  l.Loc = t.LID)   
                              END )                             AS Cellular,   
                           (SELECT l.Remarks   
                                      FROM   Rol r   
                                             INNER JOIN Loc l   
                                                     ON l.Rol = r.ID   
                                      WHERE  l.Loc = t.LID)   
                              AS Remarks,   
                            ''                                   AS QBinvoiceID,   
                            0                                    AS timetransfer,   
                            ''                                   AS QBServiceItem,   
                            ''                                   AS QBPayrollItem,   
                            isnull(t.high,0) as highdecline,   
                            isnull(Customtick3,0)AS Customticket3,   
                            isnull(Customtick4,0)AS Customticket4,   
                            (select top 1 sageid from owner where id = t.owner) as sagecust,  
                            (select top 1 id from loc where loc = t.lid) as sageloc,  
                            0 as wagec,0 as Phase, (select top 1 type from jobtype where ID = t.type) as department,   
                            (select convert(varchar(20),t.job )+'-'+ fdesc from job where ID = t.job) as jobdesc,   
                            jobitemdesc as jobitemdesc1  
                      , dbo.Afterhours(t.timeroute,t.timecomp) as afterhours, dbo.weekends(t.edate) as weekends, isnull(EmailNotified,0) as EmailNotified, EmailTime  
                      ,    (select  charge from job where ID = t.job) as isJobChargeable , t.JobCode as JobCode1 


      ,t.[ID]
      ,t.[CDate]
      ,t.[DDate]
      ,t.[EDate]
      ,t.[Level]
      ,t.[Est]
      ,t.[fWork]
      ,t.[DWork]
      ,t.[Type]
      ,t.[Cat]
      ,t.[fDesc]
      ,t.[Who]
      ,t.[fBy]
      ,t.[LType]
      ,t.[LID]
      ,t.[LElev]
      ,t.[LDesc1]
      ,t.[LDesc2]
      ,t.[LDesc3]
      ,t.[LDesc4]
      ,t.[Nature]
      ,t.[Job]
      ,t.[Assigned]
      ,t.[City]
      ,t.[State]
      ,t.[Zip]
      ,t.[Owner]
      ,t.[Route]
      ,t.[Terr]
      ,t.[fLong]
      ,t.[Latt]
      ,t.[CallIn]
      ,t.[SpecType]
      ,t.[SpecID]
      ,t.[EN]
      ,t.[Notes]
      ,t.[fGroup]
      ,t.[Source]
      ,t.[High]
      ,t.[Confirmed]
      ,t.[Phone]
      ,t.[Phone2]
      ,t.[PriceL]
      ,t.[Locked]
      ,t.[Custom1]
      ,t.[Custom2]
      ,t.[Custom3]
      ,t.[Custom4]
      ,t.[Custom5]
      ,t.[WorkOrder]
      ,t.[TimeRoute]
      ,t.[TimeSite]
      ,t.[TimeComp]
      ,t.[Follow]
      ,t.[HandheldFieldsUpdated]
      ,t.[AID]
      ,t.[BRemarks]
      ,t.[Custom6]
      ,t.[Custom7]
      ,t.[Custom8]
      ,t.[Custom9]
      ,t.[Custom10]
      ,t.[CPhone]
      ,t.[SMile]
      ,t.[EMile]
      ,t.[QBServiceItem]
      ,t.[QBPayrollItem]
      ,t.[CustomTick1]
      ,t.[CustomTick2]
      ,t.[CustomTick3]
      ,t.[CustomTick4]
      ,t.[create_token]
      ,t.[CustomTick5]
      ,t.[JobCode]
      ,t.[Recurring]
      ,t.[JobItemDesc]
      ,t.[is_work_order]
      ,t.[EmailNotified]
      ,t.[EmailTime]
      ,t.[additional_worker]
      ,t.[Charge] 
	  , l.Status as locStatus 
	  ,  ts.signature                AS signature
    from TicketO t WITH(NOLOCK) 
    INNER JOIN LOC l          WITH(NOLOCK) on l.Loc=t.LID  
    LEFT OUTER JOIN tblWork w WITH(NOLOCK) on t.fWork=w.ID  
    INNER JOIN ROL r          WITH(NOLOCK) on r.id=l.rol 
	left join @tsignature ts on ts.TicketID=t.ID
	where t.ID=@TicketID
END
 

 ------------------------------
 ------------------------------ PROSPECT TICKET
 ------------------------------

---- Prospect Ticket
IF (@IsTicketO=1    and @IsTicketProspect=1)

BEGIN
 
SELECT 'ticketo' tablename,  @IsTicketProspect as IsTicketProspect ,
                          dbo.TicketEquips(t.ID) as unit ,   
                            (SELECT unit   
                             FROM   elev   
                             WHERE  id = t.lelev)               AS unitname,   
                            (SELECT state   
                             FROM   elev   
                             WHERE  id = t.lelev)               AS unitstate,   
                            0                                   AS ClearCheck1,   
                            0                                   AS ClearPR,   
                            0                                   AS Charge,   
                            0.00                                AS Reg,   
                            0                                   AS OT,   
                            0                                   AS NT,   
                            0                                   AS DT,   
                            0                                   AS TT,   
                            0                                   AS BT,   
                            '' as Comments,   
                            '' as PartsUsed,   
                            0                                   AS Total,   
                            Upper(t.DWork)                      AS dworkup,   
                            (SELECT Super   
                             FROM   tblWork w   
                             WHERE  w.fdesc = t.dwork)          AS superv, 
                            0                                   AS tottime,   
                            0                                   AS Reg,   
                            0                                   AS NT,   
                            0                                   AS OT,   
                            0                                   AS TT,   
                            0                                   AS DT,   
                            t.LDesc2                            AS locname,   
                            t.LID                               AS LID,   
                            l.Type                              AS LocType,   
                            l.Custom1                           AS LocCustom1,   
                            l.Custom2                           AS LocCustom2,   
                            (SELECT TOP 1 Name   
                             FROM   rol   
                             WHERE  ID = (SELECT TOP 1 Rol   
                                          FROM   Owner   
                                          WHERE  ID = t.Owner)) AS customerName,   
                            ''                                  AS descres,   
                            CASE   
                              WHEN Assigned = 1 THEN 'Assigned'   
                              WHEN Assigned = 2 THEN 'Enroute'   
                              WHEN Assigned = 3 THEN 'Onsite'   
                              WHEN Assigned = 4 THEN 'Completed'   
                              WHEN Assigned = 5 THEN 'Hold'   
                            END                                 AS assignname,   
                            t.bremarks,   
                            ( ldesc3 + ' ' + ldesc4 )           AS address,   
                            l.Address as ldesc3,   
                            l.City,   
                            l.State,   
                            l.Zip,   
                            1                                   AS workcmpl,   
                            0                                   AS othere,   
                            0                                   AS toll,   
                            0                                   AS zone,   
                            0                                   AS Smile,   
                            0                                   AS emile,   
                            0                                   AS internet,   
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
                           (SELECT l.Remarks   
                                      FROM   Rol r   
                                             INNER JOIN Loc l   
                                                     ON l.Rol = r.ID   
                                      WHERE  l.Loc = t.LID)   
                              AS Remarks,   
                            ''                                  AS QBinvoiceID,   
                            0                                   AS timetransfer,   
                            ''                                   AS QBServiceItem,   
                            ''                                   AS QBPayrollItem,   
                            isnull(t.high,0) as highdecline,   
                            isnull(Customtick3,0)AS Customticket3,   
                            isnull(Customtick4,0)AS Customticket4,   
                            (select top 1 sageid from owner where id = t.owner) as sagecust,  
                            (select top 1 id from loc where loc = t.lid) as sageloc,  
                            0 as wagec,0 as Phase, (select top 1 type from jobtype where ID = t.type) as department,   
                            (select convert(varchar(20),t.job )+'-'+ fdesc from job where ID = t.job) as jobdesc,   
                            jobitemdesc as jobitemdesc1  
                      , dbo.Afterhours(t.timeroute,t.timecomp) as afterhours, dbo.weekends(t.edate) as weekends, isnull(EmailNotified,0) as EmailNotified, EmailTime  
                      ,    (select  charge from job where ID = t.job) as isJobChargeable , t.JobCode as JobCode1 


      ,t.[ID]
      ,t.[CDate]
      ,t.[DDate]
      ,t.[EDate]
      ,t.[Level]
      ,t.[Est]
      ,t.[fWork]
      ,t.[DWork]
      ,t.[Type]
      ,t.[Cat]
      ,t.[fDesc]
      ,t.[Who]
      ,t.[fBy]
      ,t.[LType]
      ,t.[LID]
      ,t.[LElev]
      ,t.[LDesc1]
      ,t.[LDesc2]
      ,t.[LDesc3]
      ,t.[LDesc4]
      ,t.[Nature]
      ,t.[Job]
      ,t.[Assigned]
      ,t.[City]
      ,t.[State]
      ,t.[Zip]
      ,t.[Owner]
      ,t.[Route]
      ,t.[Terr]
      ,t.[fLong]
      ,t.[Latt]
      ,t.[CallIn]
      ,t.[SpecType]
      ,t.[SpecID]
      ,t.[EN]
      ,t.[Notes]
      ,t.[fGroup]
      ,t.[Source]
      ,t.[High]
      ,t.[Confirmed]
      ,t.[Phone]
      ,t.[Phone2]
      ,t.[PriceL]
      ,t.[Locked]
      ,t.[Custom1]
      ,t.[Custom2]
      ,t.[Custom3]
      ,t.[Custom4]
      ,t.[Custom5]
      ,t.[WorkOrder]
      ,t.[TimeRoute]
      ,t.[TimeSite]
      ,t.[TimeComp]
      ,t.[Follow]
      ,t.[HandheldFieldsUpdated]
      ,t.[AID]
      ,t.[BRemarks]
      ,t.[Custom6]
      ,t.[Custom7]
      ,t.[Custom8]
      ,t.[Custom9]
      ,t.[Custom10]
      ,t.[CPhone]
      ,t.[SMile]
      ,t.[EMile]
      ,t.[QBServiceItem]
      ,t.[QBPayrollItem]
      ,t.[CustomTick1]
      ,t.[CustomTick2]
      ,t.[CustomTick3]
      ,t.[CustomTick4]
      ,t.[create_token]
      ,t.[CustomTick5]
      ,t.[JobCode]
      ,t.[Recurring]
      ,t.[JobItemDesc]
      ,t.[is_work_order]
      ,t.[EmailNotified]
      ,t.[EmailTime]
      ,t.[additional_worker]
      ,t.[Charge]
	  ,0 as locStatus
	  ,ts.signature                AS signature
    from TicketO t WITH(NOLOCK) 
    INNER JOIN prospect l          WITH(NOLOCK) on l.ID=t.LID       
	left join @tsignature ts on ts.TicketID=t.ID
	where t.ID=@TicketID
END

-----Prospect Completed Ticket

IF (@IsTicketD=1   and @IsTicketProspect=1)

BEGIN

select 'TicketD' tablename, @IsTicketProspect as IsTicketProspect ,  (select  charge from job where ID = t.job) as isJobChargeable ,dbo.TicketEquips(t.ID) as unit,(select unit from elev where id =t.elev)as unitname,
(select state from elev where id =t.elev)as unitstate,isnull(ClearCheck ,0 ) as ClearCheck1 , isnull(ClearPR ,0 )  ClearPR , 
                         Isnull(t.Reg,0) as Reg, Isnull(t.OT,0) as OT,Isnull(t.NT,0) as NT ,Isnull(t.DT,0) as DT ,Isnull(t.TT,0) as TT,Isnull(t.break_time,0)  as BT  
						 ,t.Comments as Comments, t.PartsUsed as PartsUsed,  t.Total,UPPER(w.fDesc)as dworkup, w.super as superv, (reg + NT + OT + TT + DT)as tottime,  
						l.CustomerName as customerName,r.Name  as locname,  l.Type AS LocType, l.Custom1 AS LocCustom1, l.Custom2 AS LocCustom2,  0 Owner,t.Loc as lid, l.Address 
						 as ldesc3,(l.Address+', '+l.City+', '+ l.State+', '+ l.Zip) as ldesc4,(l.Address+', '+l.City+', '+ l.State+', '+ l.Zip) as Address, cat, l.City, l.State, l.Zip,
						 Elev as lelev, (select top 1 Phone from rol where ID=l.Rol) as phone,
						 CPhone,4 as assigned,UPPER( w.fDesc )as dwork, descres, 'Completed' as assignname,bremarks, isnull( t.workcomplete,0) as workcmpl, isnull(invoice,0) as 
						 invoice, manualinvoice,r.contact , r.phone, r.cellular,r.remarks, isnull( QBinvoiceID,'') as QBinvoiceID, isnull( transfertime,0) as timetransfer , 
						 isnull(Customtick3,0)AS Customticket3, isnull(Customtick4,0)AS Customticket4, 0 as highdecline ,  (select top 1 sageid from owner where id =
						 (select top 1 owner from loc where loc = t.loc)) as sagecust, (select top 1 id from loc where loc = t.loc) as sageloc,
						 (select top 1 type from jobtype where ID = t.type) as department ,(select convert(varchar(20),t.job ) +'-'+ fdesc from job where ID = t.job) as jobdesc,
						 jobitemdesc as jobitemdesc1, dbo.Afterhours(t.timeroute,t.timecomp) as afterhours, dbo.weekends(t.edate) as weekends, 0 as EmailNotified, null as EmailTime   ,
						 t.JobCode as JobCode1 ,
						       t.[ID]
      ,t.[CDate]
      ,t.[DDate]
      ,t.[EDate]
      ,t.[fWork]
      ,t.[Job]
      ,t.[Loc]
      ,t.[Elev]
      ,t.[Type]
      ,t.[Charge]
      ,t.[fDesc]
      ,t.[DescRes]
      ,t.[ClearCheck]    
      ,t.[Total]
      ,t.[Who]
      ,t.[OT]
      ,t.[DT]
      ,t.[TT]
      ,t.[Zone]
      ,t.[Toll]
      ,t.[OtherE] as othere
      ,t.[Status]
      ,t.[Invoice]
      ,t.[Level]
      ,t.[Est]
      ,t.[Cat]
      ,t.[Who]
      ,t.[fBy]
      ,t.[SMile]
      ,t.[EMile]
      ,t.[fLong]
      ,t.[Latt]
      ,t.[WageC]
      ,t.[Phase]
      ,t.[Car]
      ,t.[CallIn]
      ,t.[Mileage]
      ,t.[NT]
      ,t.[CauseID]
      ,t.[CauseDesc]
      ,t.[fGroup]
      ,t.[PriceL]
      ,t.[WorkOrder]
      ,t.[TimeRoute]
      ,t.[TimeSite]
      ,t.[TimeComp]
      ,t.[Source]
      ,t.[Internet]
      ,t.[RBy]
      ,t.[Custom1]
      ,t.[Custom2]
      ,t.[Custom3]
      ,t.[Custom4]
      ,t.[Custom5]
      ,t.[CTime]
      ,t.[DTime]
      ,t.[ETime]
      ,t.[BRemarks]
      ,t.[WorkComplete]
      ,t.[BReview]
      ,t.[PRWBR]
      ,t.[pdaticketid]
      ,t.[AID]
      ,t.[Custom6]
      ,t.[Custom7]
      ,t.[Custom8]
      ,t.[Custom9]
      ,t.[Custom10]
      ,t.[CPhone]
      ,t.[RegTrav]
      ,t.[OTTrav]
      ,t.[DTTrav]
      ,t.[NTTrav]
      ,t.[Email]
      ,t.[ManualInvoice]
      ,t.[QBInvoiceID]
      ,t.[LastUpdateDate]
      ,t.[QBTimeTxnID]
      ,t.[TransferTime]
      ,t.[QBServiceItem]
      ,t.[QBPayrollItem]
      ,t.[CustomTick1]
      ,t.[CustomTick2]
      ,t.[CustomTick3]
      ,t.[CustomTick4]
      ,t.[TimesheetID]
      ,t.[HourlyRate]
      ,t.[CustomTick5]
      ,t.[JobCode]
      ,t.[Import1]
      ,t.[Import2]
      ,t.[Import3]
      ,t.[Import4]
      ,t.[Import5]
      ,t.[Recurring]
      ,t.[JobItemDesc]
      ,t.[PrimarySyncID]
      ,t.[FMSEtid]
      ,t.[PrevEquipLoc]
      ,t.[fmsimportdate]
      ,t.[break_time]
      ,t.[Comments]
      ,t.[PartsUsed]
      ,t.[TimeCheckOut]
      ,t.[TimeCheckOutFlag]
      ,t.[Assigned]
	  , 0 as locStatus
	  ,  ts.signature    AS signature
    from TicketD t WITH(NOLOCK) 
    INNER JOIN prospect l     WITH(NOLOCK) on l.ID=t.Loc     
    INNER JOIN ROL r          WITH(NOLOCK) on r.id=l.rol
	LEFT OUTER JOIN tblWork w WITH(NOLOCK) on t.fWork=w.ID 
	LEFT JOIN @tsignature ts on ts.TicketID=t.ID 
	where t.ID=@TicketID
END
END

---- Completed Ticket DPDA
IF (@IsTicketDPDA=1 and @IsTicketProspect=1)

BEGIN

        
      
	   
            SELECT 'TicketDPDA' tablename, @IsTicketProspect as IsTicketProspect ,  dp.Recommendations as bremarks,   
            Isnull(dp.Charge, 0)                   AS charge,   t.*,  
            dbo.TicketEquips(t.ID) as unit ,  
            (SELECT unit    FROM   elev  WHERE  id = t.lelev)   AS unitname,  
            (SELECT state   FROM   elev  WHERE  id = t.lelev)   AS unitstate,   
            0                                     AS ClearCheck1,  
            0                   AS ClearPR,  
            Isnull(dp.Reg,0) as Reg,  
            Isnull(dp.OT,0) as OT,  
            Isnull(dp.NT,0) as NT,  
            Isnull(dp.DT,0) as DT,  
            Isnull(dp.TT,0) as TT,  
            Isnull(dp.break_time,0) as BT,  
            dp.Comments as Comments,  
            dp.PartsUsed as PartsUsed,  
            dp.Total,  
             Upper(DWork)                        AS dworkup,  
            (SELECT Super   FROM   tblWork w       WHERE  w.fdesc = DWork)             AS superv, 
            dp.Total                            AS tottime,  
            0                                   AS Reg,  
            0                                   AS NT,  
            0                                   AS OT,  
            0                                   AS TT,  
            0                                   AS DT,  
            t.LDesc2                            AS locname,  
            t.LID                               AS LID,  
            l.Type                              AS LocType,  
            l.Custom1                           AS LocCustom1,  
            l.Custom2                           AS LocCustom2,  
            (SELECT TOP 1 NAME  
            FROM   rol  
            WHERE  ID = (SELECT TOP 1 Rol               FROM   Owner             WHERE  ID = t.Owner)) AS customerName,  
            dp.descres,  
            CASE  
            WHEN Assigned = 1 THEN 'Assigned'  
            WHEN Assigned = 2 THEN 'Enroute'  
            WHEN Assigned = 3 THEN 'Onsite'  
            WHEN Assigned = 4 THEN 'Completed'  
            WHEN Assigned = 5 THEN 'Hold'  
            END                                 AS assignname,  
            dp.Recommendations as bremarks,  
            ( ldesc3 + ' ' + ldesc4 )           AS address,  
            l.Address as ldesc3,  
            l.City,  
            l.State,  
            l.Zip,  
            Isnull(dp.workcomplete, 0)          AS workcmpl,  
            dp.othere,  
            dp.toll,  
            dp.zone,  
            dp.Smile,  
            dp.emile,  
            dp.internet,  
            dp.TimeCheckOut,  
            dp.Invoice                                   AS invoice,  
            ''                                           AS manualinvoice,  
            ( CASE  
                WHEN t.Owner IS NULL THEN (SELECT r.contact  FROM   Rol r  INNER JOIN Prospect p  ON p.Rol = r.ID     WHERE  p.ID = t.LID)  
                ELSE (SELECT r.contact  FROM   Rol r  INNER JOIN Loc l ON l.Rol = r.ID  
                    WHERE  l.Loc = t.LID)  
            END )                             AS contact,  
            ( CASE  
                WHEN t.Owner IS NULL THEN (SELECT r.Phone FROM   Rol r INNER JOIN Prospect p   ON p.Rol = r.ID  
                                            WHERE  p.ID = t.LID)  
                ELSE (SELECT r.contact FROM   Rol r INNER JOIN Loc l   ON l.Rol = r.ID  
                    WHERE  l.Loc = t.LID)  
            END )                             AS Phone,  
            ( CASE  
                WHEN t.Owner IS NULL THEN (SELECT r.Cellular  
                                            FROM   Rol r INNER JOIN Prospect p ON p.Rol = r.ID WHERE  p.ID = t.LID)  
                ELSE (SELECT r.contact FROM   Rol r INNER JOIN Loc l ON l.Rol = r.ID WHERE  l.Loc = t.LID)  
            END )                             AS Cellular,  
            ( CASE  
                WHEN t.Owner IS NULL THEN (SELECT r.Remarks  
                                            FROM   Rol r  
                                                INNER JOIN Prospect p  
                                                        ON p.Rol = r.ID  
                                            WHERE  p.ID = t.LID)  
                ELSE (SELECT l.Remarks  
                    FROM   Rol r  
                            INNER JOIN Loc l  
                                    ON l.Rol = r.ID  
                    WHERE  l.Loc = t.LID)  
            END )                               AS Remarks,  
            ''                                  AS QBinvoiceID,  
            0                                   AS timetransfer,  
            ''                                  AS QBServiceItem,  
            ''                                  AS QBPayrollItem,  
            0                                   AS highdecline,  
            Isnull(Customtick3, 0)              AS Customticket3,  
            Isnull(Customtick4, 0)              AS Customticket4,  
            (select top 1 sageid from owner where id = t.owner) as sagecust,   
            (select top 1 id from loc where loc = t.lid) as sageloc,   
            dp.wagec, isnull(dp.Phase,(select top 1  line  from JobTItem where Job=t.job and 
			Type=1 and fDesc='Labor' and job <> 0))
			Phase, (select top 1 type from jobtype where ID = t.type) as department, 
			(select convert(varchar(20),t.job ) +'-'+ fdesc 
			from job where ID = t.job) as jobdesc,
            case isnull(t.jobitemdesc, '') when   '' then isnull(t.jobitemdesc, 
			(select top 1  fDesc  from JobTItem where Job = t.job and Type = 1 and fDesc = 'Labor' and job <> 0))   
			else t.jobitemdesc end  jobitemdesc1
    , dbo.Afterhours(t.timeroute,t.timecomp) as afterhours, dbo.weekends(t.edate) as weekends, isnull(EmailNotified,0) 
	as EmailNotified, EmailTime   
    ,    (select  charge from job where ID = t.job) as isJobChargeable  ,isnull(t.jobCode,(select top 1  Code  
	from JobTItem where Job=t.job and Type=1 and fDesc='Labor' and job <>0)) JobCode1 
	 , 0 as locStatus
	,  ts.signature                        AS signature 
FROM TicketO t             WITH(NOLOCK) 
INNER JOIN TicketDPDA dp   WITH(NOLOCK) ON dp.ID = t.ID  
INNER JOIN prospect l      WITH(NOLOCK) on l.ID=t.LID  
INNER JOIN ROL r           WITH(NOLOCK) on r.id=l.rol
LEFT  JOIN tblWork w       WITH(NOLOCK) on t.fWork=w.ID
LEFT  JOIN @tsignature ts on ts.TicketID=t.ID
WHERE t.ID=@TicketID
      
                                 
END

 
