CREATE PROCEDURE [dbo].[spSetTargetHours]  
@ProjectId	INT ,
@Code  nvarchar(100) ,
@GroupName nvarchar(100) ,
@TargetHours numeric(30,2)  ,
@HoursReduce	INT ,
@isMassupdatetargetedhoursby INT ,
@isCopytargetedhoursoverbudgethours	INT  ,
@isMassupdatetargeted int
 
AS 

BEGIN 

SET NOCOUNT ON;  

IF(@isMassupdatetargetedhoursby=1)  
if(@HoursReduce < 0)
SET @TargetHours= ( @TargetHours - ((@TargetHours * (-1* @HoursReduce)) / 100) ) ;
else 
SET @TargetHours= ( @TargetHours + ((@TargetHours * @HoursReduce) / 100) ) ;

else if (@isCopytargetedhoursoverbudgethours=1)

begin 

select @TargetHours= sum(isnull(j.BHours,0))
FROM JobTItem j 
INNER JOIN tblEstimateGroup g ON g.id=j.GroupID 
WHERE ISNULL(j.GroupID,0) <> 0
AND g.GroupName=@GroupName
AND j.Job=@projectId 
AND j.Code=@code
AND j.Type=1
group by j.GroupID , j.Code  

end

Update j SET  j.TargetHours=   @TargetHours 
FROM JobTItem j 
INNER JOIN tblEstimateGroup g ON g.id=j.GroupID 
WHERE ISNULL(j.GroupID,0) <> 0
AND g.GroupName=@GroupName
AND j.Job=@projectId 
AND j.Code=@code
AND j.Type=1

END