create PROCEDURE [dbo].[spGetTargetHours]  

@ProjectId	INT  
 
AS 

BEGIN 

SET NOCOUNT ON; 

SELECT distinct 

 j.GroupID ,

 g.GroupName , 

 j.Code ,

 jcdesc.JobCodeDesc  CodeDesc ,

 isnull(j.TargetHours,0) TargetHours ,

isnull((select sum(BHours) from JobTItem jt where jt.job=j.job 
AND jt.Type=1  and j.Type=1
AND jt.Code=j.Code 
AND isnull(jt.GroupID,0)= isnull(j.GroupID,0)),0) BHours
 

FROM JobTItem j 
inner join JobCode jc on jc.Code=j.Code 
INNER JOIN job on job.id=j.Job
INNER JOIN tblEstimateGroup g on g.id=j.GroupID
inner join tblJobCodeDesc_ByJobType jcdesc on jcdesc.JobTypeID=job.Type and jcdesc.JobCodeID=jc.ID
WHERE ISNULL(j.GroupID,0) <> 0
AND g.GroupName is not null
AND j.Job=@projectId 
AND j.Code is not null
AND j.Type=1

END