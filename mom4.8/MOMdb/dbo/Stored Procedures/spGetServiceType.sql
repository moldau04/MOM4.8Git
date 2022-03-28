CREATE PROCEDURE [dbo].[spGetServiceType]  @Type varchar(100)  
AS
BEGIN
  
    if(isnull(@Type,'0')<>'0' and @Type<>'')
	SELECT TYPE, 
	FDESC, 
	REMARKS, 
	MATCHARGE,
	FREE,
	Department as Department,
	isnull(REG,0) as RT,
	(SELECT Name FROM INV WHERE TYPE = 1 AND ID = isnull(REG,0)) As RTNAME,
	isnull(OT,0)  as OT ,
	(SELECT Name FROM INV WHERE TYPE = 1 AND ID = isnull(OT,0)) As OTNAME,
	isnull(NT,0)  as NT,
	(SELECT Name FROM INV WHERE TYPE = 1 AND ID = isnull(NT,0)) As NTNAME,
	isnull(DT,0)  as DT,
	(SELECT Name FROM INV WHERE TYPE = 1 AND ID = isnull(DT,0)) As DTNAME,
	isnull(STATUS,0) as STATUS,	
	isnull(LocType,'0') as LocType, 
	(SELECT Type FROM LocType WHERE Type = isnull(LocType,0)) As LocTypeNAME,
	isnull(ExpenseGL,0)  as ExpenseGL , 	
	(SELECT  C.Acct  +' : '+ C.fDesc as Name FROM Chart C left join Bank B on C.ID=b.Chart where C.Status = 0 AND C.Type <> 7  AND C.ID = isnull(ExpenseGL,0)) AS ExpenseGLNAME,
	isnull(InterestGL,0) as InterestGL,
	(SELECT  C.Acct  +' : '+ C.fDesc as Name FROM Chart C left join Bank B on C.ID=b.Chart where C.Status = 0 AND C.Type <> 7  AND C.ID = isnull(InterestGL,0)) AS InterestGLNAME,
	isnull(LaborWageC,0) as LaborWageC,
	(SELECT fDesc FROM PRWage WHERE ID = isnull(LaborWageC,0)) As LaborWageCNAME,
	isnull(InvID,0) as InvID, 
	(SELECT Name FROM INV WHERE TYPE = 1 AND ID = isnull(InvID,0)) As InvIDNAME,
	Route as Route,  
	(SELECT top 1 Label FROM tblSchedule WHERE Type='DefaultWorker')
	routelabel  
	from LType
	WHERE type= @Type
	else 
	begin
	SELECT top 1 Label routelabel FROM tblSchedule WHERE Type='DefaultWorker'  
	End
END







