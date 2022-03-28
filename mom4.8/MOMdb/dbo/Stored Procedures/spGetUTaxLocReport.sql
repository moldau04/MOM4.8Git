CREATE PROCEDURE [dbo].[spGetUTaxLocReport]

 @fromDate datetime,
 @toDate datetime
 

AS
BEGIN
 
Select distinct r.Name As vendor, PJ.Ref As PJRef,t.VInt As JobID,
 p.Amount As PJItemAmount,Loc.Tag As LocationName,jobt.Type AS JobType,PJ.Po As PO,PJ.fDesc As Descp,PJ.fDate As PJfDate,
 Loc.UTax As STaxName,pp.State As State, pp.Fdesc As TaxDesc   ,pp.Rate* t.Amount*0.01  As STaxRate, 
 t.Batch As TransBatch, t.fDate As TransfDate,t.fDesc As LineItemDesc,t.Type As TransType, 
 t.Amount As Amount, 
 (CASE PJ.Status 
					   WHEN 0 THEN 'Open'         
					   WHEN 1 THEN 'Paid'                 
					  
					  
					 	END) AS StatusName
 from PJ PJ

 left outer join Trans t on t.batch= PJ.Batch
  left outer join PJItem P on t.ID=p.TRID
 
  left outer join vendor v on  PJ.Vendor = v.ID
  left outer join Rol  r on v.Rol = r.ID 
  left outer join  job job on job.id=t.vint
  left outer join Loc Loc  on job.loc=Loc.Loc 
  left outer join stax pp on pp.Name=Loc.UTax
  left outer  join jobType jobt on jobt.id=job.type
 WHERE t.Type='41'  and t.VInt is not null and  t.VInt !=0 -- And pp.Fdesc='NC - Alamance Use Tax' 

 And PJ.fDate > @fromDate and 
 PJ.fDate <= @toDate
 --order by Loc.UTax



END