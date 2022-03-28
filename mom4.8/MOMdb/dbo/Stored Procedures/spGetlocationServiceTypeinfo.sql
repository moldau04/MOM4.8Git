CREATE PROCEDURE [dbo].[spGetlocationServiceTypeinfo] 
@LocType nvarchar (100) ,
@RoutID int ,
@LocationID int 
AS 
 BEGIN    

   	  DECLARE @ServiceTypeName nvarchar (100) ='0';  
	  DECLARE @serviceTypeCount  int=0 ; 
	  DECLARE @ProjectPerDepartmentCount  int=0 ; 

	 select  @serviceTypeCount = count(1) from   ( SELECT    t1.Type  
      FROM LType  t1 	  
	  INNER JOIN tblServicetypeRouteMapping rmap on t1.Type = rmap.type AND rmap.route = @RoutID
      INNER JOIN tblServicetypeDepartmentMapping dmap on t1.Type = dmap.type 
	  WHERE t1.LocType = @LocType  group by t1.Type )  as t 
  
	  
      SELECT  @ServiceTypeName= t1.Type  
      FROM LType  t1 	  
	  INNER JOIN tblServicetypeRouteMapping rmap on t1.Type = rmap.type AND rmap.route = @RoutID
      INNER JOIN tblServicetypeDepartmentMapping dmap on t1.Type = dmap.type 
	  WHERE t1.LocType = @LocType 
	  
	 
	  select  @ProjectPerDepartmentCount =  Count( JOB.ID)    FROM  JOB 	  
	  INNER JOIN Loc  on job.loc=loc.Loc 
	  WHERE loc.Loc=@LocationID 
	  AND job.Type in (  select dmap.Department 
      FROM LType  t1 	  
	  INNER JOIN tblServicetypeRouteMapping rmap on t1.Type = rmap.type AND rmap.route = @RoutID
      INNER JOIN tblServicetypeDepartmentMapping dmap on t1.Type = dmap.type 
	  WHERE t1.LocType = @LocType
	  AND t1.Type=@ServiceTypeName 
	  ) group by job.Type having Count( JOB.ID) = 1

	  select  @ProjectPerDepartmentCount +=  Count( JOB.ID)    FROM  JOB 	  
	  INNER JOIN Loc  on job.loc=loc.Loc 
	  WHERE loc.Loc=@LocationID 
	  AND job.Type in (  select dmap.Department 
      FROM LType  t1 	  
	  INNER JOIN tblServicetypeRouteMapping rmap on t1.Type = rmap.type AND rmap.route = @RoutID
      INNER JOIN tblServicetypeDepartmentMapping dmap on t1.Type = dmap.type 
	  WHERE t1.LocType = @LocType
	  AND t1.Type=@ServiceTypeName 
	  ) group by job.Type having Count( JOB.ID) > 1
	   

	  
	     DECLARE @JodId nvarchar(1000)
	  DECLARE @Table1 TABLE(ID INT, Value INT)
	  INSERT INTO @Table1
      SELECT   1 , JOB.ID    FROM  JOB 	  
	  INNER JOIN Loc  ON job.loc=loc.Loc 
	  WHERE loc.Loc=@LocationID 
	  AND job.Type in (  select dmap.Department 
      FROM LType  t1 	  
	  INNER JOIN tblServicetypeRouteMapping rmap on t1.Type = rmap.type AND rmap.route = @RoutID
      INNER JOIN tblServicetypeDepartmentMapping dmap on t1.Type = dmap.type 
	  WHERE t1.LocType = @LocType
	  AND t1.Type=@ServiceTypeName )
	  
	 SELECT @JodId=  STUFF((SELECT ', ' + CAST(Value AS VARCHAR(10)) [text()]
         FROM @Table1 
         WHERE ID = t.ID
         FOR XML PATH(''), TYPE)
        .value('.','NVARCHAR(MAX)'),1,2,' ')  
     FROM @Table1 t
     GROUP BY ID


	  SELECT    @ServiceTypeName ServiceTypeName ,   @serviceTypeCount as  ServiceTypeCount , @ProjectPerDepartmentCount  ProjectPerDepartmentCount , @JodId as ProjectaregoingtoUpdate
       
	
       
	   
END 
 