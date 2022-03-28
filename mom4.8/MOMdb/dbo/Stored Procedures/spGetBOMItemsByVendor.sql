CREATE PROCEDURE [dbo].[spGetBOMItemsByVendor]
	@VendorId int
AS
select 

(SELECT min(ISNULL(b1.SDate,GETDATE())) FROM JobTItem jt1 
	INNER JOIN Bom b1 ON b1.JobtItemId = jt1.ID 
	--INNER JOIN Job job1 ON job1.ID = jt1.Job 
	WHERE jt1.Job=job.ID and jt1.Type in (1) 
	and isnull(jt1.GroupId,0) = isnull(j.GroupID,0)
	and b1.Vendor = v.id
) GroupSDate,

(SELECT min(isnull(b1.SDate, GETDATE())) FROM JobTItem jt1 
	INNER JOIN Bom b1 ON b1.JobtItemId = jt1.ID 
	--INNER JOIN Job job1 ON job1.ID = jt1.Job 
	WHERE jt1.Job=job.id and jt1.Type in (1) 
		and isnull(jt1.GroupId,0) = isnull(j.GroupId,0)
		and jt1.Code = j.Code
		and b1.Vendor = v.id
) CodeSDate,

isnull(b.SDate, GETDATE()) SDate, 

(SELECT MAX(DATEADD(Hour,isnull(jt1.BHours,0)%8,DATEADD(DAY,isnull(jt1.BHours,0)/8,isnull(b1.SDate, GETDATE())))) FROM JobTItem jt1 
	INNER JOIN Bom b1 ON b1.JobtItemId = jt1.ID 
	--INNER JOIN Job job1 ON job1.ID = jt1.Job 
	WHERE jt1.Job=job.id and jt1.Type in (1) 
		and isnull(jt1.GroupId,0) = isnull(j.GroupId,0)
		and b1.Vendor = v.id
) GroupEDate,

(SELECT MAX(DATEADD(Hour,isnull(jt1.BHours,0)%8,DATEADD(DAY,isnull(jt1.BHours,0)/8,isnull(b1.SDate, GETDATE())))) FROM JobTItem jt1 
		INNER JOIN Bom b1 ON b1.JobtItemId = jt1.ID 
		--INNER JOIN Job job ON job.ID = jt.Job 
	WHERE jt1.Job=job.id and jt1.Type in (1) 
		and isnull(jt1.GroupId,0) = isnull(j.GroupId,0)
		and jt1.Code = j.Code
		and b1.Vendor = v.id
) CodeEDate,
DATEADD(Hour,isnull(j.BHours,0)%8,DATEADD(DAY,isnull(j.BHours,0)/8,isnull(b.SDate, GETDATE()))) EDate,
j.ID,
j.ID AS JobTItemID, 
j.Code as Code, 
(select top 1
(select top 1 JobCodeDesc from tblJobCodeDesc_ByJobType where JobCodeID= jc.ID and JobTypeID=job.Type ) 
FROM JobCode jc where jc.Code=j.Code) as CodeDesc , 
j.fDesc, 
j.Type as jType, 
--b.Type as BType,
ISNULL((Case b.Type WHEN 2 THEN 2 When 1 Then 1 ELSE
(Select top 1 PO.TypeID From BOMT bt 
inner Join POItem PO on PO.TypeID = bt.ID Where bt.ID = ISNUll(PO.TypeID,b.Type) 
and PO.Phase=j.Line AND j.Type != 0 and PO.Job = j.id) END ),b.Type) AS BType, 
isnull(b.QtyRequired,0) as QtyReq, 
b.UM, 
isnull(b.BudgetUnit,0) as BudgetUnit, 
isnull(j.Budget,0) as BudgetExt, 
j.Line, 
b.LabItem,
b.MatItem, 
isnull(j.Modifier,0) AS MatMod, 
isnull(j.ETCMod,0) as LabMod, 
isnull(j.ETC,0) as LabExt, 
isnull(b.LabRate,0) as LabRate, 
isnull(j.BHours,0) as LabHours, 

b.Vendor VendorId, 
(select r.Name from vendor v inner join rol r on v.Rol = r.ID where v.Id=b.vendor) as Vendor, 
isnull(j.Budget,0) + isnull(j.ETC,0) as TotalExt, 
CASE WHEN b.Type=2 THEN (SELECT TOP 1 fDesc FROM PRWage WHERE ID=b.MatItem) 
ELSE (SELECT TOP 1 Name FROM Inv WHERE ID=b.MatItem) END AS MatDesc,
j.OrderNo ,
isNull(j.GroupId,0) GroupId ,

isnull((select GroupName from tblEstimateGroup where id=j.GroupID ),'NA') GroupName,
(select top 1 fDesc as LabDesc from PRWage where ID=b.LabItem) as txtLabItem,
 

isnull((select sum(TargetHours) from JobTItem jt where jt.job=j.job 
AND jt.Type=1  and j.Type=1
AND jt.Code=j.Code 
AND isnull(jt.GroupID,0)= isnull(j.GroupID,0)),0) TargetHours,


isnull((select sum(BHours) from JobTItem jt where jt.job=j.job 
AND jt.Type=1  and j.Type=1
AND jt.Code=j.Code 
AND isnull(jt.GroupID,0)= isnull(j.GroupID,0)),0) BudgetHours,

job.ID ProjectId,
job.fDesc ProjectName

from job
--inner join jobt jt on jt.ID = j.Template
inner join JobTItem j on j.Job = job.ID
inner join bom b on b.JobtItemId = j.ID
inner join vendor v on v.ID = b.Vendor
inner join rol r on v.Rol = r.id
where 
v.id = @VendorId