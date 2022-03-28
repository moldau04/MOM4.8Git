CREATE PROC spGetFinance_Budget_Grid_Popup_ByJob
 
@job INT=0,
@phase INT=0,                        -- jobtitem.line
@type SMALLINT=0,				     -- 0 Revenue, 1 Cost jobtitem.type
@sdate NVARCHAR(10) = 'NA',  -- jobi.fdate
@edate NVARCHAR(10) = 'NA'   -- jobi.fdate
AS 
	
SET NOCOUNT ON;  	 
 

IF(@type=0)--------------------------------------->

BEGIN

------ AR INVOICE  

SELECT 
isnull(ji.Ref, '')as Ref, ( 'addinvoice.aspx?uid='+ ji.Ref ) as Url, ( ji.fDate ) as fdate	,  ji.fDesc, isnull(ji.Amount,0) as Amount 
, '' AssignedWorker , '' VendorName
FROM 
jobi ji
INNER JOIN jobtitem j on ji.Job = j.Job AND ji.Phase = j.Line AND ji.Type = j.Type
INNER JOIN Job job ON job.ID = j.Job
INNER JOIN Invoice i on i.Ref=ji.Ref
WHERE j.Job=@job and j.Type = @type  -- job item revenue          
and ji.Job = @job AND j.Type = 0 AND 0= @type AND j.line = @phase
AND cast ( ji.fDate as date )  >=  Case @sdate when 'NA' then cast ( ji.fDate as date )   else  cast  (  @sdate as date) end 
AND cast ( ji.fDate as date )  <=  Case @edate when 'NA' then cast ( ji.fDate as date )   else  cast  (  @edate as date) end
	
 ---- AP INVOICE

 SELECT     '' ref,  ''  url, GETDATE() fDate , '' fdesc , 0 Amount		, '' AssignedWorker , '' VendorName	 

------TICKET

 SELECT     '' ref,  ''  url, GETDATE() fDate , '' fdesc , 0 Amount , '' AssignedWorker , '' VendorName
	
----- PO

 SELECT     '' ref,  ''  url, GETDATE() fDate , '' fdesc , 0 Amount , '' AssignedWorker , '' VendorName

-------R PO 	
 
 SELECT     '' ref,  ''  url, GETDATE() fDate , '' fdesc , 0 Amount , '' AssignedWorker , '' VendorName

------JE 
SELECT	 
g.Ref,'addjournalentry.aspx?id='+convert(varchar(50),g.ref) as Url,g.fDate,t.fDesc, j.Amount 
, '' AssignedWorker , '' VendorName
FROM JobI j
INNER JOIN Trans t on t.ID = j.TransID AND t.VInt = j.Job AND Convert(int,t.VDoub) = j.Phase
INNER JOIN GLA g on g.Batch = t.Batch
INNER JOIN JobTItem jobitem on jobitem.Job = j.Job AND jobitem.Line = j.Phase and jobitem.Type = j.Type 
WHERE		j.Type = 0 --   
AND j.Job =@job AND j.Phase = @phase
AND cast ( j.fDate as date )  >=  Case @sdate when 'NA' then cast ( j.fDate as date )   else  cast  (  @sdate as date) end 
AND cast ( j.fDate as date )  <=  Case @edate when 'NA' then cast ( j.fDate as date )   else  cast  (  @edate as date) end



SELECT TOP 1  l.Tag LocName ,    'Revenue' PhaseName, j.Code as CodeName ,j.Code as Codedesc , '' GroupName
FROM   jobtitem j
INNER JOIN Job job ON job.ID =  @job 
INNER JOIN loc l on l.loc=job.Loc
INNER JOIN Milestone m ON m.JobtItemId = j.ID  
WHERE j.job=@job and j.Line= @phase and j.Type=0
END

ELSE---------------------------------------------->

BEGIN


------ AR INVOICE  

 SELECT     '' ref,  ''  url, GETDATE() fDate , '' fdesc , 0 Amount , '' AssignedWorker , '' VendorName
	
 ---- AP INVOICE

 SELECT       jb.ref, 	'addbills.aspx?id='+cast(p.ID as varchar(100) )  url,jb.fDate,	jb.fdesc,	jb.Amount , 
 
   '' AssignedWorker , 

 ( select top 1 r.Name 
from Vendor v
inner join rol r on r.id=v.Rol
where v.ID=p.Vendor ) VendorName 
				FROM Trans as t 
				INNER JOIN PJ p on p.Batch=t.Batch
				INNER JOIN Job as j ON t.VInt = j.ID
				INNER join JobI as jb on jb.TransID=t.ID 
				INNER JOIN JobTItem as jt ON isnull(jt.Job,0) = isnull(j.ID,0) AND (t.VDoub = jt.Line and isnull(j.ID,0) > 0) AND jt.Type not in ( 0)				 
				WHERE t.Type = 41 AND jb.job=@job and jb.Phase=@phase  AND t.fdesc<>'Use Tax Payable'
				AND cast ( jb.fDate as date )  >=  Case @sdate when 'NA' then cast ( jb.fDate as date )   else  cast  (  @sdate as date) end 
                AND cast ( jb.fDate as date )  <=  Case @edate when 'NA' then cast ( jb.fDate as date )   else  cast  (  @edate as date) end 

------TICKET

SELECT  t.ID ref,'addticket.aspx?id='+cast(t.ID as varchar(100) )+'&comp=1&pop=1' url,t.edate fDate, t.fDesc,  
isnull((select sum(amount) from jobi where ref= cast(t.id as varchar(100)) 
--and phase in (t.Phase) -- Updated by Thomas for ES-2990: Project Budget Detail on ProjectType: Cost
and TransID < 0  
AND jobi.Phase = @phase
AND jobi.Type <> 0
AND cast ( jobi.fDate as date )  >=  Case @sdate when 'NA' then cast ( jobi.fDate as date )   else  cast  (  @sdate as date) end 
AND cast (jobi.fDate as date )  <=  Case @edate when 'NA' then cast ( jobi.fDate as date )   else  cast  (  @edate as date) end 
),0) as Amount  , w.fDesc AssignedWorker   , '' VendorName
 
FROM TicketD AS t
INNER JOIN tblWork AS w ON t.fWork = w.ID
--INNER JOIN JobTItem AS jobt on jobt.Job = t.Job AND jobt.Line = t.Phase  
INNER JOIN JobTItem AS jobt on jobt.Job = t.Job AND jobt.Line = @phase -- Updated by Thomas for ES-2990: Project Budget Detail on ProjectType: Cost
WHERE jobt.Type <> 0 
AND t.Job = @job
AND 1= @type
AND jobt.Line = @phase
AND cast ( t.EDate as date )  >=  Case @sdate when 'NA' then cast ( t.EDate as date )   else  cast  (  @sdate as date) end 
AND cast ( t.EDate as date )  <=  Case @edate when 'NA' then cast ( t.EDate as date )   else  cast  (  @edate as date) end
	
	
----- PO
SELECT    PO.PO as ref, 'addpo.aspx?id='+convert(varchar(50),PO.PO) AS Url,PO.fDate, PO.fDesc,ISNULL(p.Balance,0) AS   Amount 
, '' AssignedWorker ,  

 ( select top 1 r.Name 
from Vendor v
inner join rol r on r.id=v.Rol
where v.ID=po.Vendor ) VendorName

FROM POItem p 
INNER JOIN PO ON p.PO = PO.PO  
INNER JOIN JobTItem j ON j.Line = p.Phase AND j.Job = p.Job   and j.type <> 0  
WHERE 
PO.Status not IN (1) 
AND j.Job   = @job
AND j.Line = @phase
AND cast ( po.fDate as date )  >=  Case @sdate when 'NA' then cast ( po.fDate as date )   else  cast  (  @sdate as date) end 
AND cast ( po.fDate as date )  <=  Case @edate when 'NA' then cast ( po.fDate as date )   else  cast  (  @edate as date) end


-------R PO 	
 
SELECT 
p.PO as ref ,'addreceivepo.aspx?id='+convert(varchar(50),r.ID) AS Url,r.fDate,p.fDesc,r.Amount		 

, '' AssignedWorker , 

 ( select top 1 r.Name 
from Vendor v
inner join rol r on r.id=v.Rol
where v.ID= PO.Vendor ) VendorName 

FROM RPOItem rp   
INNER JOIN ReceivePO r on r.ID = rp.ReceivePO 
INNER JOIN PO ON r.PO = PO.PO 
inner JOIN POItem p on r.PO = p.PO AND rp.POLine = p.Line 
INNER JOIN JobTItem j ON j.Line = p.Phase AND j.Job = p.Job   and j.type <> 0  
WHERE ISNULL(r.Status,0) = 0 
AND j.Job   = @job
AND j.Line = @phase
AND cast ( r.fDate as date )  >=  Case @sdate when 'NA' then cast ( r.fDate as date )   else  cast  (  @sdate as date) end 
AND cast ( r.fDate as date )  <=  Case @edate when 'NA' then cast ( r.fDate as date )   else  cast  (  @edate as date) end


------JE 

SELECT	 
g.Ref,'addjournalentry.aspx?id='+convert(varchar(50),g.ref) as Url,g.fDate,t.fDesc, t.Amount 
, '' AssignedWorker , '' VendorName
FROM JobI j
INNER JOIN Trans t on t.ID = j.TransID AND t.VInt = j.Job AND Convert(int,t.VDoub) = j.Phase
INNER JOIN GLA g on g.Batch = t.Batch
INNER JOIN JobTItem jobitem on jobitem.Job = j.Job AND jobitem.Line = j.Phase and jobitem.Type = j.Type 
WHERE		j.Type = 1 -- and t.type =30  
AND j.Job =@job AND j.Phase = @phase
AND cast ( j.fDate as date )  >=  Case @sdate when 'NA' then cast ( j.fDate as date )   else  cast  (  @sdate as date) end 
AND cast ( j.fDate as date )  <=  Case @edate when 'NA' then cast ( j.fDate as date )   else  cast  (  @edate as date) end



SELECT top 1  l.Tag LocName ,   o.Type PhaseName, j.Code as CodeName ,j.Code as Codedesc , '' GroupName
FROM   jobtitem j
INNER JOIN Job job ON job.ID =  @job 
inner join loc l on l.loc=job.Loc
INNER JOIN bom m ON m.JobtItemId = j.ID           
INNER JOIN bomt	 o ON o.ID = m.Type 
where j.job=@job and j.Line= @phase and j.Type=1
END