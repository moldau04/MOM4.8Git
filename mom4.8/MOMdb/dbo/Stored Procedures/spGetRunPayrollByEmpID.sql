--EXEC spGetRunPayrollByEmpID '2020-05-01','2020-05-07','',0,0,143,0,1  
CREATE PROC [dbo].[spGetRunPayrollByEmpID]   
@startdate datetime,  
@enddate datetime,  
@Supervisor varchar(50),  
@department int, @EN int = 0,  
@UserID int = 0,  
@WorkId int = 0,  
@Etimesheet int = -1  
  
AS  
BEGIN  
  
SELECT pd.ID, pd.fDesc,  
SUM(ISNULL(td.Reg, 0)) Reg,  
  SUM(ISNULL(td.OT, 0)) OT,  
  SUM(ISNULL(td.DT, 0)) DT,  
  SUM(ISNULL(td.TT, 0)) TT,  
  SUM(ISNULL(td.NT, 0)) NT,  
  Count(ISNULL(td.ID,0)) TotalTicket INTO #TempPayrollDetail  
FROM TicketD td INNER JOIN tblWork w on td.fWork = w.ID INNER JOIN emp e on e.CallSign = w.fDesc   
INNER JOIN PRWage pd ON pd.ID = td.WageC  
WHERE e.ID = @UserID   
AND CAST(EDate AS date) >= CAST(@startdate AS date)  
  AND CAST(EDate AS date) <= CAST(@enddate AS date) GROUP BY pd.ID,pd.fDesc  
    
  SELECT ID,fDesc,Reg,OT,DT,TT,NT,TotalTicket FROM #TempPayrollDetail  
  UNION  
  SELECT wg.ID,wg.fDesc,0 AS Reg,0 AS OT,0 AS DT,0 AS TT,0 AS NT,0 AS TotalTicket FROM PRWageItem itm INNER JOIN PRWage wg ON wg.ID = itm.Wage  
  WHERE Emp = @UserID AND ID NOT IN (SELECT ID FROM #TempPayrollDetail)   
  
  DROP TABLE #TempPayrollDetail  
  
  
CREATE table #tempDetailPay  
(  
 ID int null,  
 fDesc varchar(max) null,  
 Quan numeric(30,2) null,  
 Rate numeric(30,2) null,  
 Amount numeric(30,2) null  
)  
  
CREATE table #tempDetailDeduction  
(  
 ID int null,  
 fDesc varchar(max) null,   
 Amount numeric(30,2) null  
)  
  
INSERT INTO #tempDetailPay (ID,fDesc,Quan,Rate,Amount)   
SELECT pd.ID, pd.fDesc,SUM(ISNULL(td.Reg, 0)) Reg,ISNULL(pd.Reg,0),ISNULL(pd.Reg,0)*SUM(ISNULL(td.Reg, 0))    
FROM TicketD td INNER JOIN tblWork w on td.fWork = w.ID INNER JOIN emp e on e.CallSign = w.fDesc   
INNER JOIN PRWage pd ON pd.ID = td.WageC  
WHERE e.ID = @UserID   
AND CAST(EDate AS date) >= CAST(@startdate AS date)   
  AND CAST(EDate AS date) <= CAST(@enddate AS date) GROUP BY pd.ID,pd.fDesc,td.Reg,pd.Reg  
  
INSERT INTO #tempDetailPay (ID,fDesc,Quan,Rate,Amount)   
SELECT pd.ID, pd.fDesc,SUM(ISNULL(td.OT, 0)) Reg,ISNULL(pd.OT1,0),ISNULL(pd.OT1,0)*SUM(ISNULL(td.OT, 0))    
FROM TicketD td INNER JOIN tblWork w on td.fWork = w.ID INNER JOIN emp e on e.CallSign = w.fDesc   
INNER JOIN PRWage pd ON pd.ID = td.WageC  
WHERE e.ID = @UserID   
AND CAST(EDate AS date) >= CAST(@startdate AS date)  
  AND CAST(EDate AS date) <= CAST(@enddate AS date) GROUP BY pd.ID,pd.fDesc,td.OT,pd.OT1  
  
    
  
INSERT INTO #tempDetailPay (ID,fDesc,Quan,Rate,Amount)   
SELECT pd.ID, pd.fDesc,SUM(ISNULL(td.NT, 0)) Reg,ISNULL(pd.NT,0),ISNULL(pd.NT,0)*SUM(ISNULL(td.NT, 0))    
FROM TicketD td INNER JOIN tblWork w on td.fWork = w.ID INNER JOIN emp e on e.CallSign = w.fDesc   
INNER JOIN PRWage pd ON pd.ID = td.WageC  
WHERE e.ID = @UserID   
AND CAST(EDate AS date) >= CAST(@startdate AS date)  
  AND CAST(EDate AS date) <= CAST(@enddate AS date) GROUP BY pd.ID,pd.fDesc,td.NT,pd.NT  
  
INSERT INTO #tempDetailPay (ID,fDesc,Quan,Rate,Amount)   
SELECT pd.ID, pd.fDesc,SUM(ISNULL(td.DT, 0)) Reg,ISNULL(pd.OT2,0),ISNULL(pd.OT2,0)*SUM(ISNULL(td.DT, 0))    
FROM TicketD td INNER JOIN tblWork w on td.fWork = w.ID INNER JOIN emp e on e.CallSign = w.fDesc   
INNER JOIN PRWage pd ON pd.ID = td.WageC  
WHERE e.ID = @UserID   
AND CAST(EDate AS date) >= CAST(@startdate AS date)  
  AND CAST(EDate AS date) <= CAST(@enddate AS date) GROUP BY pd.ID,pd.fDesc,td.NT,pd.OT2  
  
INSERT INTO #tempDetailPay (ID,fDesc,Quan,Rate,Amount)   
SELECT pd.ID, pd.fDesc,SUM(ISNULL(td.TT, 0)) Reg,ISNULL(pd.TT,0),ISNULL(pd.TT,0)*SUM(ISNULL(td.TT, 0))    
FROM TicketD td INNER JOIN tblWork w on td.fWork = w.ID INNER JOIN emp e on e.CallSign = w.fDesc   
INNER JOIN PRWage pd ON pd.ID = td.WageC  
WHERE e.ID = @UserID   
AND CAST(EDate AS date) >= CAST(@startdate AS date)  
  AND CAST(EDate AS date) <= CAST(@enddate AS date) GROUP BY pd.ID,pd.fDesc,td.TT,pd.TT  
  
INSERT INTO #tempDetailPay (ID,fDesc,Quan,Rate,Amount)   
SELECT 0, CASE WHEN pd.Cat = 0 THEN 'Bonus'  
WHEN pd.Cat = 1 THEN 'Holiday'  
WHEN pd.Cat = 2 THEN 'Vacation'  
WHEN pd.Cat = 3 THEN 'Zone'  WHEN pd.Cat = 4 THEN 'Reimbursement'  
WHEN pd.Cat = 5 THEN 'Mileage'  
WHEN pd.Cat = 6 THEN 'Sick'  
ELSE '' END  
,0 Reg,ISNULL(pd.Rate,0),0  
FROM PROther pd  INNER JOIN emp e ON pd.Emp = e.ID   
WHERE e.ID = @UserID   
  
SELECT * FROM #tempDetailPay  
  
  
INSERT INTO #tempDetailDeduction (ID,fDesc,Amount)  
SELECT 0,'',0.00  
  
SELECT * FROM #tempDetailDeduction  
  
  
  
DROP TABLE #tempDetailPay  
  
DROP TABLE #tempDetailDeduction  
    
    
END
