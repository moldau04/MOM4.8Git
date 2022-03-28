--EXEC spGetRunPayrollByEmpID '2020-05-01','2020-05-07','',0,0,143,0,1    
CREATE PROC [dbo].[spGetRunPayrollCheckDetailByEmpID]     
@startdate datetime,    
@enddate datetime,    
@Supervisor varchar(50),    
@department int, @EN int = 0,    
@UserID int = 0,    
@WorkId int = 0,    
@Etimesheet int = -1,    
@HolidayAm decimal(17,2),
@VacAm decimal(17,2),
@ZoneAm decimal(17,2),
@ReimbAm decimal(17,2),
@MilageAm decimal(17,2),
@BonusAm decimal(17,2)
AS    
BEGIN    
      
CREATE table #tempDetailPay    
(    
 ID int null,    
 fDesc varchar(max) null,    
 Quan numeric(30,2) null,    
 Rate numeric(30,2) null,    
 Amount numeric(30,2) null    
)    
    
 SELECT Emp.[Name],Rol.Address AS [Address],Rol.City,Rol.State AS [State],Rol.Zip FROM Emp INNER JOIN Rol ON Emp.Rol = Rol.ID WHERE Emp.ID = @UserID 
    
INSERT INTO #tempDetailPay (ID,fDesc,Quan,Rate,Amount)     
SELECT pd.ID, 'RG '+pd.fDesc,SUM(ISNULL(td.Reg, 0)) Reg,ISNULL(pd.Reg,0),ISNULL(pd.Reg,0)*SUM(ISNULL(td.Reg, 0))      
FROM TicketD td INNER JOIN tblWork w on td.fWork = w.ID INNER JOIN emp e on e.CallSign = w.fDesc     
INNER JOIN PRWage pd ON pd.ID = td.WageC    
WHERE e.ID = @UserID     
AND CAST(EDate AS date) >= CAST(@startdate AS date)     
  AND CAST(EDate AS date) <= CAST(@enddate AS date) GROUP BY pd.ID,pd.fDesc,td.Reg,pd.Reg Having SUM(ISNULL(td.Reg, 0)) > 0    
    
INSERT INTO #tempDetailPay (ID,fDesc,Quan,Rate,Amount)     
SELECT pd.ID, 'OT '+pd.fDesc,SUM(ISNULL(td.OT, 0)) Reg,ISNULL(pd.OT1,0),ISNULL(pd.OT1,0)*SUM(ISNULL(td.OT, 0))      
FROM TicketD td INNER JOIN tblWork w on td.fWork = w.ID INNER JOIN emp e on e.CallSign = w.fDesc     
INNER JOIN PRWage pd ON pd.ID = td.WageC    
WHERE e.ID = @UserID     
AND CAST(EDate AS date) >= CAST(@startdate AS date)    
  AND CAST(EDate AS date) <= CAST(@enddate AS date) GROUP BY pd.ID,pd.fDesc,td.OT,pd.OT1   Having SUM(ISNULL(td.OT, 0)) > 0  
    
      
    
INSERT INTO #tempDetailPay (ID,fDesc,Quan,Rate,Amount)     
SELECT pd.ID, 'NT '+pd.fDesc,SUM(ISNULL(td.NT, 0)) Reg,ISNULL(pd.NT,0),ISNULL(pd.NT,0)*SUM(ISNULL(td.NT, 0))      
FROM TicketD td INNER JOIN tblWork w on td.fWork = w.ID INNER JOIN emp e on e.CallSign = w.fDesc     
INNER JOIN PRWage pd ON pd.ID = td.WageC    
WHERE e.ID = @UserID     
AND CAST(EDate AS date) >= CAST(@startdate AS date)    
  AND CAST(EDate AS date) <= CAST(@enddate AS date) GROUP BY pd.ID,pd.fDesc,td.NT,pd.NT    Having SUM(ISNULL(td.NT, 0)) > 0 
    
INSERT INTO #tempDetailPay (ID,fDesc,Quan,Rate,Amount)     
SELECT pd.ID, 'DT '+pd.fDesc,SUM(ISNULL(td.DT, 0)) Reg,ISNULL(pd.OT2,0),ISNULL(pd.OT2,0)*SUM(ISNULL(td.DT, 0))      
FROM TicketD td INNER JOIN tblWork w on td.fWork = w.ID INNER JOIN emp e on e.CallSign = w.fDesc     
INNER JOIN PRWage pd ON pd.ID = td.WageC    
WHERE e.ID = @UserID     
AND CAST(EDate AS date) >= CAST(@startdate AS date)    
  AND CAST(EDate AS date) <= CAST(@enddate AS date) GROUP BY pd.ID,pd.fDesc,td.DT,pd.OT2    Having SUM(ISNULL(td.DT, 0)) > 0 
    
INSERT INTO #tempDetailPay (ID,fDesc,Quan,Rate,Amount)     
SELECT pd.ID, 'TT '+pd.fDesc,SUM(ISNULL(td.TT, 0)) Reg,ISNULL(pd.TT,0),ISNULL(pd.TT,0)*SUM(ISNULL(td.TT, 0))      
FROM TicketD td INNER JOIN tblWork w on td.fWork = w.ID INNER JOIN emp e on e.CallSign = w.fDesc     
INNER JOIN PRWage pd ON pd.ID = td.WageC    
WHERE e.ID = @UserID     
AND CAST(EDate AS date) >= CAST(@startdate AS date)    
  AND CAST(EDate AS date) <= CAST(@enddate AS date) GROUP BY pd.ID,pd.fDesc,td.TT,pd.TT    Having SUM(ISNULL(td.TT, 0)) > 0 
    
INSERT INTO #tempDetailPay (ID,fDesc,Quan,Rate,Amount)     
--SELECT 0, CASE WHEN pd.Cat = 0 THEN 'Bonus'    
--WHEN pd.Cat = 1 THEN 'Holiday'    
--WHEN pd.Cat = 2 THEN 'Vacation'    
--WHEN pd.Cat = 3 THEN 'Zone'  WHEN pd.Cat = 4 THEN 'Reimbursement'    
--WHEN pd.Cat = 5 THEN 'Mileage'    
--WHEN pd.Cat = 6 THEN 'Sick'    
--ELSE '' END    
--,0 Reg,ISNULL(pd.Rate,0),0    
--FROM PROther pd  INNER JOIN emp e ON pd.Emp = e.ID     
--WHERE e.ID = @UserID   

SELECT 0, CASE WHEN pd.Cat = 0 THEN 'Bonus'    
WHEN pd.Cat = 1 THEN 'Holiday'    
WHEN pd.Cat = 2 THEN 'Vacation'    
WHEN pd.Cat = 3 THEN 'Zone'  WHEN pd.Cat = 4 THEN 'Reimbursement'    
WHEN pd.Cat = 5 THEN 'Mileage'    
WHEN pd.Cat = 6 THEN 'Sick'    
ELSE '' END    
,CASE WHEN pd.Cat = 0 THEN @BonusAm    
WHEN pd.Cat = 1 THEN ISNULL(pd.Rate,0)*@HolidayAm
WHEN pd.Cat = 2 THEN ISNULL(pd.Rate,0)*@VacAm    
WHEN pd.Cat = 3 THEN @ZoneAm  WHEN pd.Cat = 4 THEN @ReimbAm   
WHEN pd.Cat = 5 THEN @MilageAm    
WHEN pd.Cat = 6 THEN 0   
ELSE '' END
,ISNULL(pd.Rate,0),0    
FROM PROther pd  INNER JOIN emp e ON pd.Emp = e.ID     
WHERE e.ID = @UserID 
    
SELECT ID,fDesc,SUM(Quan) AS Quan,Rate,SUM(Quan)*Rate AS Amount FROM #tempDetailPay  GROUP BY ID,fDesc ,Rate ORDER BY ID DESC
    
    
 
    
    
    
DROP TABLE #tempDetailPay    
    
  
      
      
END  