CREATE PROC [dbo].[spGetPayrollCheckDetail]       
@CheckID int           
AS    
BEGIN  
   

SELECT b.fDesc AS BankName,Reg.*,Emp.Name, Rol.Address, Rol.City, Rol.State, Rol.Zip,(SELECT DISTINCT Batch FROM Trans WHERE ID = Reg.TransID)  AS Batch
FROM PRReg Reg INNER JOIN EMP Emp ON Reg.EmpID = Emp.ID 
INNER JOIN Rol ON Emp.Rol = Rol.ID  
INNER JOIN Bank b ON b.ID = Reg.Bank
WHERE Reg.ID = @CheckID
  
CREATE table #tempCheckDetail  
(    
 ID int null,    
 fDesc varchar(max) null,    
 Quan numeric(30,2) null,    
 Rate numeric(30,2) null,    
 Amount numeric(30,2) null,    
 YTD numeric(30,2) null    
)  
  
CREATE table #tempCheckDeductionDetail  
(    
 ID int null,    
 fDesc varchar(max) null,     
 Amount numeric(30,2) null,    
 YTD numeric(30,2) null    
)   
CREATE table #tempDetailPay    
(    
 ID int null,    
 WageID Int NULL,  
 WageCategory varchar(max) null,    
 WageType varchar(max) null,    
 Quan numeric(30,2) null,    
 Rate numeric(30,2) null,    
 Amount numeric(30,2) null    
)   
  
INSERT INTO #tempCheckDetail (ID,fDesc,Quan,Rate,Amount,YTD)   
SELECT 1,'Regular',PRReg.Hreg,ISNULL(PRReg.Reg/NULLIF(PRReg.Hreg, 0),0),PRReg.Reg,PRReg.YReg FROM PRReg WHERE PRReg.ID = @CheckID  
INSERT INTO #tempCheckDetail (ID,fDesc,Quan,Rate,Amount,YTD)   
SELECT 2,'OverTime',PRReg.HOT,ISNULL(PRReg.OT/NULLIF(PRReg.HOT, 0),0),PRReg.OT,PRReg.YOT FROM PRReg WHERE PRReg.ID = @CheckID  
INSERT INTO #tempCheckDetail (ID,fDesc,Quan,Rate,Amount,YTD)   
SELECT 3,'1.7 Time',PRReg.HNT,ISNULL(PRReg.NT/NULLIF(PRReg.HNT, 0),0),PRReg.NT,PRReg.YNT FROM PRReg WHERE PRReg.ID = @CheckID  
INSERT INTO #tempCheckDetail (ID,fDesc,Quan,Rate,Amount,YTD)   
SELECT 4,'Double Time',PRReg.HDT,ISNULL(PRReg.DT/NULLIF(PRReg.HDT, 0),0),PRReg.DT,PRReg.YDT FROM PRReg WHERE PRReg.ID = @CheckID  
INSERT INTO #tempCheckDetail (ID,fDesc,Quan,Rate,Amount,YTD)   
SELECT 5,'Travel Time',PRReg.HTT,ISNULL(PRReg.TT/NULLIF(PRReg.HTT, 0),0),PRReg.TT,PRReg.YTT FROM PRReg WHERE PRReg.ID = @CheckID  
INSERT INTO #tempCheckDetail (ID,fDesc,Quan,Rate,Amount,YTD)   
SELECT 6,'Holiday',PRReg.HHol,ISNULL(PRReg.Hol/NULLIF(PRReg.HHol, 0),0),PRReg.Hol,PRReg.YHol FROM PRReg WHERE PRReg.ID = @CheckID  
INSERT INTO #tempCheckDetail (ID,fDesc,Quan,Rate,Amount,YTD)   
SELECT 4,'Vacation',PRReg.HVac,ISNULL(PRReg.Vac/NULLIF(PRReg.HVac, 0),0),PRReg.Vac,PRReg.YVac FROM PRReg WHERE PRReg.ID = @CheckID  
INSERT INTO #tempCheckDetail (ID,fDesc,Quan,Rate,Amount,YTD)   
SELECT 5,'Zone',NULL,NULL,PRReg.Zone,PRReg.YZone FROM PRReg WHERE PRReg.ID = @CheckID  
INSERT INTO #tempCheckDetail (ID,fDesc,Quan,Rate,Amount,YTD)   
SELECT 6,'Reimbursement',NULL,NULL,PRReg.Reimb,PRReg.YReimb FROM PRReg WHERE PRReg.ID = @CheckID  
INSERT INTO #tempCheckDetail (ID,fDesc,Quan,Rate,Amount,YTD)   
SELECT 7,'Bonus',NULL,NULL,PRReg.Bonus,PRReg.YBonus FROM PRReg WHERE PRReg.ID = @CheckID  
INSERT INTO #tempCheckDetail (ID,fDesc,Quan,Rate,Amount,YTD)   
SELECT 8,'Mileage',PRReg.HMile,ISNULL(PRReg.Mile/NULLIF(PRReg.HMile, 0),0),PRReg.Mile,PRReg.YMile FROM PRReg WHERE PRReg.ID = @CheckID  
INSERT INTO #tempCheckDetail (ID,fDesc,Quan,Rate,Amount,YTD)   
SELECT 9,'Sick Leave',PRReg.HSick,ISNULL(PRReg.Sick/NULLIF(PRReg.HSick, 0),0),PRReg.Sick,PRReg.YSick FROM PRReg WHERE PRReg.ID = @CheckID  
  
SELECT * FROM #tempCheckDetail  
DROP TABLE #tempCheckDetail     
  
  
INSERT INTO #tempCheckDeductionDetail (ID,fDesc,Amount,YTD)  
SELECT 1,'Federal Tax',FIT,YFIT FROM PRReg WHERE ID = @CheckID  
INSERT INTO #tempCheckDeductionDetail (ID,fDesc,Amount,YTD)  
SELECT 2,'FICA',FICA,YFICA FROM PRReg WHERE ID = @CheckID  
INSERT INTO #tempCheckDeductionDetail (ID,fDesc,Amount,YTD)  
SELECT 3,'MEDI',MEDI,YMEDI FROM PRReg WHERE ID = @CheckID  
INSERT INTO #tempCheckDeductionDetail (ID,fDesc,Amount,YTD)  
SELECT 4,'SIT',SIT,YSIT FROM PRReg WHERE ID = @CheckID  
INSERT INTO #tempCheckDeductionDetail (ID,fDesc,Amount,YTD)  
SELECT 5,'Local',Local,YLocal FROM PRReg WHERE ID = @CheckID  
INSERT INTO #tempCheckDeductionDetail (ID,fDesc,Amount,YTD)  
SELECT ROW_NUMBER() OVER (ORDER BY PRDed.ID )+5 row_num,PRDed.fDesc,PRRegDItem.Amount, PRRegDItem.YAmount  FROM PRRegDItem INNER JOIN PRDed ON PRRegDItem.PRDID=PRDed.ID WHERE PRRegDItem.CheckID=@CheckID ORDER BY PRDed.ID  
  
SELECT * FROM #tempCheckDeductionDetail ORDER BY ID  
DROP TABLE #tempCheckDeductionDetail  
  
  
  
  
INSERT INTO #tempDetailPay (ID,WageID,WageCategory,WageType,Quan,Rate,Amount)     
SELECT 1,PRWage.ID,PRWage.fDesc,'Regular', PRRegWItem.Quan, PRRegWItem.Rate, PRRegWItem.Amount FROM PRRegWItem INNER JOIN PRWage ON PRRegWItem.PRWID=PRWage.ID WHERE PRRegWItem.CheckID=@CheckID  
  
INSERT INTO #tempDetailPay (ID,WageID,WageCategory,WageType,Quan,Rate,Amount)     
SELECT 2,PRWage.ID,PRWage.fDesc,'Overtime', PRRegWItem.OQuan, PRRegWItem.ORate, PRRegWItem.OAmount FROM PRRegWItem INNER JOIN PRWage ON PRRegWItem.PRWID=PRWage.ID WHERE PRRegWItem.CheckID=@CheckID  
  
INSERT INTO #tempDetailPay (ID,WageID,WageCategory,WageType,Quan,Rate,Amount)     
SELECT 3,PRWage.ID,PRWage.fDesc,'1.7 Time', PRRegWItem.NQuan,PRRegWItem.NRate, PRRegWItem.NAmount FROM PRRegWItem INNER JOIN PRWage ON PRRegWItem.PRWID=PRWage.ID WHERE PRRegWItem.CheckID=@CheckID  
  
INSERT INTO #tempDetailPay (ID,WageID,WageCategory,WageType,Quan,Rate,Amount)     
SELECT 4,PRWage.ID,PRWage.fDesc,'Double Time', PRRegWItem.DQuan,PRRegWItem.DRate, PRRegWItem.DAmount FROM PRRegWItem INNER JOIN PRWage ON PRRegWItem.PRWID=PRWage.ID WHERE PRRegWItem.CheckID=@CheckID  
  
INSERT INTO #tempDetailPay (ID,WageID,WageCategory,WageType,Quan,Rate,Amount)     
SELECT 5,PRWage.ID,PRWage.fDesc,'Travel Time', PRRegWItem.TQuan,PRRegWItem.TRate, PRRegWItem.TAmount FROM PRRegWItem INNER JOIN PRWage ON PRRegWItem.PRWID=PRWage.ID WHERE PRRegWItem.CheckID=@CheckID  
  
SELECT * FROM #tempDetailPay ORDER BY WageID,ID  
DROP TABLE #tempDetailPay  
END