CREATE PROCEDURE  [dbo].[spGetjobcodeInfo]   
@projectId	INT,          
@type		VARCHAR(75)          
AS          
BEGIN          
	SET NOCOUNT ON;   
	
	SELECT j.id,
	j.fdesc,
	j.type,
	j.WageC wage,
	j.Charge,
	j.taskcategory
	FROM   Job j   
	WHERE  j.id=@projectId
	           
	SELECT          
	 bt.Type bomtype,  
	j.Job,    
	j.fDesc,           
	j.Code,           
	j.Line, 
	(select GroupName from tblEstimateGroup where id=j.GroupID ) GroupName  ,
	j.code CodeDesc
	FROM   jobtitem j  
	inner join job on job.id=j.job
	Inner join bom b on b.JobtItemId = j.ID  
	inner join bomt bt on bt.ID = b.type
	WHERE  j.job = @projectId  
	and j.type = 1    
    
END
