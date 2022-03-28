CREATE Procedure [dbo].[SpGetContractInfo]  
(
@LocID int ,
@EquipID int ,
@Type nvarchar(50)
)
AS
if(@Type ='Equipment')
Begin
 Select job.ctype As ContractType , 
             case CONTRACT.SCycle
             WHEN 0 THEN 'Monthly'  
             WHEN 1 THEN 'Bi-Monthly'  
             WHEN 2 THEN 'Quarterly'  
             WHEN 3 THEN 'Semi-Annually'  
             WHEN 4 THEN 'Annually'  
             WHEN 5 THEN 'Weekly'  
             WHEN 6 THEN 'Bi-Weekly' 
             WHEN 7 THEN 'Every 13 Weeks' 
             WHEN 10 THEN 'Every 2 Years'  
             WHEN 8 THEN 'Every 3 Years'  
             WHEN 9 THEN 'Every 5 Years'  
             WHEN 11 THEN 'Every 7 Years'  
             WHEN 12 THEN 'On-Demand'  
             WHEN 14 THEN 'Twice a Month'
			 ELSE ''
			 END  AS  ScheduleFrequency  
  FROM CONTRACT
  inner join job on job.id=CONTRACT.Job 
  where CONTRACT.job in (  select Job from tblJoinElevJob where Elev=@EquipID)
End
ELSE
Begin
Select  job.ctype As ContractType , 
             case CONTRACT.SCycle
             WHEN 0 THEN 'Monthly'  
             WHEN 1 THEN 'Bi-Monthly'  
             WHEN 2 THEN 'Quarterly'  
             WHEN 3 THEN 'Semi-Annually'  
             WHEN 4 THEN 'Annually'  
             WHEN 5 THEN 'Weekly'  
             WHEN 6 THEN 'Bi-Weekly' 
             WHEN 7 THEN 'Every 13 Weeks' 
             WHEN 10 THEN 'Every 2 Years'  
             WHEN 8 THEN 'Every 3 Years'  
             WHEN 9 THEN 'Every 5 Years'  
             WHEN 11 THEN 'Every 7 Years'  
             WHEN 12 THEN 'On-Demand'  
             WHEN 14 THEN 'Twice a Month'
			 ELSE ''
			 END  AS  ScheduleFrequency  
  FROM CONTRACT
  inner join job on job.id=CONTRACT.Job 
  where CONTRACT.Loc=@LocID 
End
GO