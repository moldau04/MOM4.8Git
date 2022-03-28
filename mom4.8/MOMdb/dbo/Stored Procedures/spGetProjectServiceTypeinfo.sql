CREATE PROC spGetProjectServiceTypeinfo

@ServiceType nchar(100) ,

@DepartmentID int ,

@LocType nchar (100) ,

@RoutID int 

      AS 

      BEGIN 

      SELECT  TOP 1

       isnull(t1.LaborWageC,0) LaborWageValue,

       isnull(PRWage.fDesc,'') LaborWageCNAME,

       isnull(t1.InvID,0) BillingValue,

	   isnull(Inv.Name,'') BillingName,

       isnull(t1.InterestGL,0) InterestGLValue,

	   IGL.Acct  +' : '+ IGL.fDesc InterestGLNAME,

       isnull(t1.ExpenseGL,0) ExpenseGLValue ,

	   EGL.Acct  +' : '+ EGL.fDesc  ExpenseGLNAME ,

       @DepartmentID  DepartmentValue ,

	   @DepartmentID  DepartmentName ,

       @ServiceType ServiceTypeValue ,

	   @ServiceType ServiceTypeName		 

      FROM LType  t1  
	  
	  INNER JOIN INV on Inv.ID=t1.InvID

	  INNER JOIN PRWage on PRWage.ID=t1.LaborWageC	  
	  
	  INNER JOIN tblServicetypeRouteMapping rmap on t1.Type = rmap.type

      INNER JOIN tblServicetypeDepartmentMapping dmap on t1.Type = dmap.type 

	  INNER JOIN Chart IGL on IGL.ID=t1.InterestGL
	  
	  INNER JOIN Chart EGL on EGL.ID=t1.ExpenseGL

	  WHERE t1.LocType = @LocType

      AND dmap.Department = @DepartmentID 

	  AND rmap.route = @RoutID

	  AND t1.Type= @ServiceType  
	  
	  END