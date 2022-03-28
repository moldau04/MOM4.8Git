CREATE PROC [dbo].[spGetEmpTimeCard]         

@Supervisor varchar(50),        
@department int, @EN int = 0,        
@UserID int = 0,        
@WorkId int = 0,        
@Etimesheet int = -1        
        
AS        
        
      
Declare @s_Supervisor varchar(50)        
Declare @s_department int        
Declare @s_EN int = 0        
Declare @s_UserID int = 0        
Declare @s_WorkId int = 0        
Declare @s_Etimesheet int = -1        
        
        
     
Set @s_Supervisor=CASE WHEN @Supervisor = '0' THEN '' ELSE @Supervisor END     
Set @s_department=@department        
Set @s_EN=@EN        
Set @s_UserID=@UserID        
Set @s_WorkId=@WorkId        
Set @s_Etimesheet=@Etimesheet        
 SELECT        
  e.ID,        
  (e.Last + ' , ' + e.fFirst) AS Name,        
        
  (SELECT top 1        
   EN        
  FROM tblUser        
  WHERE fUser = e.CallSign)        
  AS EN,        
        
  (SELECT top 1        
   B.Name        
  FROM Branch B        
  INNER JOIN tblUser U        
   ON B.ID = U.EN        
  WHERE fUser = e.CallSign)        
  AS Company,        
  tab.fDesc,        
  --(CASE        
  -- WHEN e.PFixed = 0 THEN ISNULL(PHour, 0)        
  -- ELSE tab.Reg        
  --END) AS reg,        
  --OT,        
  --DT,        
  --tab.TT,        
  --tab.NT,        
  --ZONE,        
  ISNULL(0, 0) AS MileageRate,        
  --Mileage,        
  
  

  0 Reg,        
  0 OT,        
  0 DT,        
  0 TT,        
  0 NT,        
  0 ZONE,        
  0 Mileage,        
  0 AS extra,        
  0 AS Toll,        
  0 AS OtherE,        
  0 AS Customtick1,  
  0 AS WageC, 



  --Toll,        
  --OtherE,        
  1 AS pay,        
  0.00 AS holiday,        
  0.00 AS vacation,        
  0.00 AS sicktime,        
  0.00 AS reimb,        
  0.00 AS bonus,
   0 as Project,
  1 AS Type,
  0 AS Wage,
  0 AS Unit,0 AS [Group],
  0 AS WO,
  NULL AS Date,
  NULL AS Start,
  'Job Time' AS Resolution,
  CASE        
   WHEN e.PFixed = 0 THEN 'Fixed Hours'        
   WHEN e.PMethod = 0 THEN 'Salaried'        
   WHEN e.PMethod = 1 THEN 'Hourly'        
  END paymethod,        
  CASE e.pfixed        
   WHEN 0 THEN 2        
   ELSE e.pmethod        
  END AS pmethod,        
        
  (SELECT top 1        
   ID        
  FROM tblUser        
  WHERE fUser = e.CallSign)        
  AS userid,        
  CASE        
   WHEN ISNULL(e.fWork, '') = '' THEN 'Office'        
   ELSE 'Field'        
  END AS usertype,        
  CASE        
   WHEN e.PFixed = 0 THEN ((ISNULL(PHour, 0) * ISNULL((SELECT        
     ISNULL(0, 0)        
    FROM tblWork wo        
    WHERE wo.fDesc = e.CallSign),0)        
    ) + (ISNULL(ZONE,0) + 0))        
   WHEN e.PMethod = 0 THEN (ISNULL(ZONE,0) + 0)        
   --WHEN e.PMethod = 1 THEN (((ISNULL(Reg,0) + ISNULL(OT,0) + ISNULL(DT,0) + ISNULL(TT,0) + ISNULL(NT,0)) * ISNULL((SELECT        
   --  ISNULL(0, 0)        
   -- FROM tblWork wo        
   -- WHERE wo.fDesc = e.CallSign),0)        
   -- ) + (ISNULL(ZONE,0) + 0))        
   WHEN e.PMethod = 1 THEN (((ISNULL(pd.Reg,0)*ISNULL(tab.Reg, 0) +   ISNULL(pd.OT1,0)*ISNULL(tab.OT, 0)+ ISNULL(pd.OT2,0)*ISNULL(tab.DT, 0)  
   + ISNULL(pd.TT,0)*ISNULL(tab.TT, 0) + ISNULL(pd.NT,0)*ISNULL(tab.NT, 0))   
    ) + (ISNULL(ZONE,0) + 0))        
  END total,        
  ISNULL(PHour, 0) AS phour,        
  ISNULL(salary, 0) AS salary,        
        
  ISNULL((SELECT        
   ISNULL(0, 0)        
  FROM tblWork wo        
  WHERE wo.fDesc = e.CallSign),0)        
  AS HourlyRate
 
  --Customtick1
  
          
  INTO #TempPayroll  
 FROM (SELECT        
  w.fDesc,        
  SUM(ISNULL(d.Reg, 0)) Reg,        
  SUM(ISNULL(OT, 0)) OT,        
  SUM(ISNULL(DT, 0)) DT,        
  SUM(ISNULL(d.TT, 0)) TT,        
  SUM(ISNULL(d.NT, 0)) NT,        
  SUM(ISNULL(ZONE, 0)) ZONE,        
  SUM(ISNULL((ISNULL(EMile, 0) - ISNULL(sMile, 0)), 0)) Mileage,        
  SUM(CASE ISNUMERIC(dbo.udf_GetNumeric(ISNULL(d.Custom2, '0')))        
   WHEN 1 THEN CONVERT(money, dbo.udf_GetNumeric(ISNULL(d.Custom2, '0')))        
   ELSE 0        
  END) AS extra,        
          
  SUM(ISNULL(Toll, 0)) Toll,        
  SUM(ISNULL(OtherE, 0)) OtherE,        
  0 AS Customtick1,
  d.WageC       
 FROM tblWork w        
 LEFT OUTER JOIN TicketD d    
     
  ON w.ID = d.fWork        
    
  --AND CAST(EDate AS date) >= CAST(@s_startdate AS date)        
  --AND CAST(EDate AS date) <= CAST(@s_enddate AS date)        
    
 WHERE w.Status = 0        
 GROUP BY w.fDesc ,d.WageC         
 UNION        
 SELECT        
  CallSign AS fDesc,        
  0 Reg,        
  0 OT,        
  0 DT,        
  0 TT,        
  0 NT,        
  0 ZONE,        
  0 Mileage,        
  0 AS extra,        
  0 AS Toll,        
  0 AS OtherE,        
  0 AS Customtick1,  
  0 AS WageC  
 FROM Emp        
 WHERE Field = 0        
 AND Status = 0) AS tab        
 INNER JOIN Emp e        
  ON e.CallSign = tab.fDesc        
      
  AND e.callsign = (CASE @s_Supervisor        
   WHEN '' THEN e.callsign        
   ELSE (SELECT TOP 1        
     fdesc        
    FROM tblwork w        
    WHERE super = @s_Supervisor        
    AND fdesc = e.callsign)        
  END)        
  AND e.callsign =        
          CASE @s_WorkId        
           WHEN 0 THEN e.callsign        
           ELSE (SELECT TOP 1        
             fdesc        
            FROM tblwork w        
            WHERE id = @s_WorkId        
            AND fdesc = e.callsign)        
          END   
 LEFT JOIN PRWage pd ON pd.ID = tab.WageC     
        
  WHERE e.Status=0   --AND e.ID=    220  
 ORDER BY --e.ID        
 e.Name   
   

 SELECT DISTINCT ID,Name,EN,Company,fDesc,SUM(Reg) AS Reg,SUM(OT) AS OT,SUM(DT) AS DT,SUM(TT) AS TT,SUM(NT) AS NT,SUM(Zone) AS Zone,MileageRate,SUM(Mileage) AS Mileage,  
 SUM(OtherE) AS OtherE ,pay,holiday,vacation,sicktime,reimb,bonus,paymethod,pmethod,userid,usertype,SUM(total) as total,phour,salary,HourlyRate,Customtick1,
 Project,  [Type],  Wage,  Unit,[Group],  WO,  [Date],  [Start],  Resolution  FROM #TempPayroll GROUP BY ID,Name,EN,Company,fDesc,MileageRate,pay,holiday,vacation,sicktime,reimb,bonus,paymethod,pmethod,userid,usertype,phour,salary,HourlyRate,Customtick1,
 Project,  [Type],  Wage,  Unit,[Group],  WO,  [Date],  [Start],  Resolution  ORDER BY Name  
  
 DROP TABLE #TempPayroll  
        
        
        
         
        
