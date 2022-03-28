CREATE PROCEDURE [dbo].[uspProcessPayroll]          
 (    
 @PayrollEmp tblTypeProcessPayrollEmp readonly,          
 @startdate datetime,          
 @enddate datetime,          
 @Bank int,          
 @Memo varchar(max),          
 @Week varchar(max),          
 @PeriodDescription varchar(max),          
 @ProcessMethod varchar(max),          
 @Supervisor varchar(max),          
 @ProcessDed int,          
 @Check int,          
 @CDate datetime,    
 @MOMUser VARCHAR(MAX))          
          
AS          
BEGIN          
          
     SET NOCount ON;     
           
 DECLARE @ID int ;          
 DECLARE @Name varchar(max) ;          
 DECLARE @Reg numeric(30, 4) ;          
 DECLARE @OT numeric(30, 4) ;          
 DECLARE @DT numeric(30, 4) ;          
 DECLARE @TT numeric(30, 4) ;          
 DECLARE @NT numeric(30, 4) ;          
 DECLARE @Zone numeric(30, 4) ;          
 DECLARE @Milage numeric(30, 4) ;          
 DECLARE @Toll numeric(30, 4) ;          
 DECLARE @OtherE numeric(30, 4) ;          
 DECLARE @pay numeric(30, 4) ;          
 DECLARE @holiday numeric(30, 4) ;          
 DECLARE @vacation numeric(30, 4) ;          
 DECLARE @sicktime numeric(30, 4) ;          
 DECLARE @reimb numeric(30, 4) ;          
 DECLARE @bonus numeric(30, 4) ;          
 DECLARE @paymethod varchar(max) ;          
 DECLARE @pmethod int ;          
 DECLARE @userid int ;          
 DECLARE @usertype varchar(max) ;          
 DECLARE @total numeric(30, 4) ;          
 DECLARE @phour numeric(30, 4) ;          
 DECLARE @salary numeric(30, 4) ;          
 DECLARE @HourlyRate numeric(30, 4) ;       
 DECLARE @FIT numeric(30, 4) ;       
 DECLARE @SIT numeric(30, 4) ;       
 DECLARE @LOCAL numeric(30, 4) ;
 DECLARE @MEDI numeric(30, 4) ;       
 DECLARE @FICA numeric(30, 4) ;
 
          
 DECLARE @TransId BigInt          
 DECLARE @MAXBatch BigInt          
 Declare @PayRegRef Int       
 DECLARE @PayRegMaxID int    
 DECLARE @BankAcctID int    
 DECLARE @TransDesc VARCHAR(MAX)          
 DECLARE @WhileLoopCount INT           
 DECLARE @tAmount numeric(30, 4)           
 DECLARE @tGL int         
 DECLARE @Payroll_GL int        
 DECLARE @PayrollTax_GL int 
 DECLARE @NetTotal numeric(30, 4)           
        
 DECLARE @A_ID int         
 DECLARE @A_GL int        
 DECLARE @A_TotalAmt numeric(30, 4)           
        
DECLARE @BonusGL int          
DECLARE @HolidayGL int          
DECLARE @VacationGL int        
DECLARE @ZoneGL int          
DECLARE @ReimbursementGL int          
DECLARE @MileageGL int          
DECLARE @SickGL int         
        
DECLARE @Ded_Ded int        
DECLARE @Ded_Emp int        
DECLARE @Ded_BasedOn int        
DECLARE @Ded_AccuredOn int        
DECLARE @Ded_ByW int        
DECLARE @Ded_EmpRate NUMERIC(19,4)        
DECLARE @Ded_EmpTop NUMERIC(19,4)        
DECLARE @Ded_EmpGL INT        
DECLARE @Ded_CompRate NUMERIC(19,4)        
DECLARE @Ded_CompTop NUMERIC(19,4)        
DECLARE @Ded_CompGL int        
DECLARE @Ded_CompGLE int        
DECLARE @Ded_InUse int        
DECLARE @Ded_YTD NUMERIC(19,4)        
DECLARE @Ded_YTDC NUMERIC(19,4)        
DECLARE @Ded_fDesc VARCHAR(MAX)        
DECLARE @Ded_DeductionAmount NUMERIC(19,4)        
DECLARE @Ded_JobSpecific int


 BEGIN TRY          
 BEGIN TRANSACTION          
 SELECT @Payroll_GL=ID FROM Chart WHERE DefaultNo='D3100'         
 --SELECT @PayrollTax_GL=ID FROM Chart WHERE DefaultNo='D3200'         
     
         
 --------------------------          
 CREATE table #tempprol          
 (          
 [ID] [int] NULL,          
 [Name] [varchar](max) NULL,          
 [Reg] [numeric](30, 4) NULL,          
 [OT] [numeric](30, 4) NULL,          
 [DT] [numeric](30, 4) NULL,          
 [TT] [numeric](30, 4) NULL,          
 [NT] [numeric](30, 4) NULL,          
 [Zone] [numeric](30, 4) NULL,          
 [Milage] [numeric](30, 4) NULL,          
 [Toll] [numeric](30, 4) NULL,          
 [OtherE] [numeric](30, 4) NULL,          
 [pay] [numeric](30, 4) NULL,          
 [holiday] [numeric](30, 4) NULL,          
 [vacation] [numeric](30, 4) NULL,          
 [sicktime] [numeric](30, 4) NULL,          
 [reimb] [numeric](30, 4) NULL,          
 [bonus] [numeric](30, 4) NULL,          
 [paymethod] [varchar](max) NULL,           [pmethod] [int] NULL,          
 [userid] [int] NULL,          
 [usertype] [varchar](max) NULL,          
 [total] [numeric](30, 4) NULL,          
 [phour] [numeric](30, 4) NULL,          
 [salary] [numeric](30, 4) NULL,          
 [HourlyRate] [numeric](30, 4) NULL ,      
 [FIT] [numeric](30, 4) NULL ,      
 [SIT] [numeric](30, 4) NULL ,      
 [LOCAL] [numeric](30, 4) NULL       
 )          
 CREATE table #tempDetailPay            
 (            
  ID int null,            
  fDesc varchar(max) null,            
  RegQuan numeric(30,2) null,            
  RegRate numeric(30,2) null,            
  RegAmt numeric(30,2) null,          
  OTQuan numeric(30,2) null,            
  OTRate numeric(30,2) null,            
  OTAmt numeric(30,2) null,          
  NTQuan numeric(30,2) null,            
  NTRate numeric(30,2) null,            
  NTAmt numeric(30,2) null,          
  DTQuan numeric(30,2) null,            
  DTRate numeric(30,2) null,            
  DTAmt numeric(30,2) null,          
  TTQuan numeric(30,2) null,            
  TTRate numeric(30,2) null,            
  TTAmt numeric(30,2) null,          
  GL int NULL,
  EmpID int NULL
 )   
 
  --------------------- Revenue-------------          
  INSERT INTO #tempDetailPay(ID ,fDesc,RegQuan,RegRate,RegAmt,OTQuan,OTRate,OTAmt,NTQuan,NTRate,NTAmt,DTQuan,DTRate,DTAmt ,TTQuan ,TTRate ,TTAmt ,GL,EmpID )          
 SELECT pd.ID, pd.fDesc,SUM(ISNULL(td.Reg, 0)) RegQuan,ISNULL(PRItem.Reg,0) RegRate,ISNULL(PRItem.Reg,0)*SUM(ISNULL(td.Reg, 0)) RegAmt ,          
  SUM(ISNULL(td.OT, 0)) OTQuan,ISNULL(PRItem.OT,0) OTRate,ISNULL(PRItem.OT,0)*SUM(ISNULL(td.OT, 0)) OTAmt,          
  SUM(ISNULL(td.NT, 0)) NTQuan,ISNULL(PRItem.NT,0) NTRate,ISNULL(PRItem.NT,0)*SUM(ISNULL(td.NT, 0)) NTAmt,          
  SUM(ISNULL(td.DT, 0)) DTQuan,ISNULL(PRItem.DT,0) DTRate,ISNULL(PRItem.DT,0)*SUM(ISNULL(td.DT, 0)) DTAmt,          
  SUM(ISNULL(td.TT, 0)) TTQuan,ISNULL(PRItem.TT,0) TTRate,ISNULL(PRItem.TT,0)*SUM(ISNULL(td.TT, 0)) TTAmt,           
  PRItem.GL,e.ID               
  FROM TicketD td INNER JOIN tblWork w on td.fWork = w.ID INNER JOIN emp e on e.CallSign = w.fDesc             
  INNER JOIN PRWage pd ON pd.ID = td.WageC    
  LEFT JOIN PRWageItem PRItem ON pd.ID = PRItem.Wage AND PRItem.Emp = e.ID    
  WHERE e.ID = @ID             
  AND CAST(EDate AS date) >= CAST(@startdate AS date)             
  AND CAST(EDate AS date) <= CAST(@enddate AS date) GROUP BY pd.ID,pd.fDesc,PRItem.Reg ,PRItem.OT,PRItem.NT,PRItem.DT,PRItem.TT,PRItem.GL,e.ID           
  HAVING SUM(ISNULL(td.Reg, 0))+SUM(ISNULL(td.OT, 0))+SUM(ISNULL(td.NT, 0))+SUM(ISNULL(td.DT, 0))+SUM(ISNULL(td.TT, 0)) >0          
          
             
  UPDATE PRWageItem SET           
  PRWageItem.YTD = ISNULL(prwg.YTD,0) +ISNULL(ttemp.RegAmt,0),          
  PRWageItem.YTDH = ISNULL(prwg.YTDH,0) +ISNULL(ttemp.RegQuan,0),          
  PRWageItem.OYTD = ISNULL(prwg.OYTD,0) +ISNULL(ttemp.OTAmt,0),          
  PRWageItem.OYTDH = ISNULL(prwg.OYTDH,0) +ISNULL(ttemp.OTQuan,0),          
  PRWageItem.DYTD = ISNULL(prwg.DYTD,0) +ISNULL(ttemp.DTAmt,0),          
  PRWageItem.DYTDH = ISNULL(prwg.DYTDH,0) +ISNULL(ttemp.DTQuan,0),          
  PRWageItem.TYTD = ISNULL(prwg.TYTD,0) +ISNULL(ttemp.TTAmt,0),          
  PRWageItem.TYTDH = ISNULL(prwg.TYTDH,0) +ISNULL(ttemp.TTQuan,0),          
  PRWageItem.NYTD = ISNULL(prwg.NYTD,0) +ISNULL(ttemp.NTAmt,0),          
  PRWageItem.NYTDH = ISNULL(prwg.NYTDH,0) +ISNULL(ttemp.NTQuan,0)          
  FROM  PRWageItem prwg  INNER JOIN #tempDetailPay ttemp ON prwg.Wage = ttemp.ID          
  WHERE prwg.Emp = @ID          
          
  -----------------------------------   

 DECLARE db_cursor1 CURSOR FOR           
           
 SELECT ID,[Name],Reg,OT,DT,TT,NT,[Zone],Milage,Toll,OtherE,pay,holiday,vacation,sicktime,reimb,bonus,paymethod,pmethod,userid, usertype,total,phour,salary,HourlyRate,FIT,SIT,[LOCAL],[MEDI],[FICA]  FROM @PayrollEmp           
          
 OPEN db_cursor1            
 FETCH NEXT FROM db_cursor1 INTO           
   @ID,@Name,@Reg,@OT,@DT,@TT,@NT,@Zone,@Milage,@Toll,@OtherE,@pay,@holiday,@vacation,@sicktime,@reimb,@bonus,@paymethod,@pmethod,@userid,@usertype,@total,@phour,@salary,@HourlyRate      
   ,@FIT,@SIT,@LOCAL,@MEDI,@FICA      
            
 WHILE @@FETCH_STATUS = 0          
 BEGIN             
          
  SELECT @MAXBatch = ISNULL(MAX(Batch),0)+1 FROM Trans           
  SELECT @PayRegRef = ISNULL(MAX(Ref),0)+1 FROM PRReg          
  SELECT @PayRegMaxID = ISNULL(MAX(ID),0)+1 FROM PRReg     
  SELECT @BankAcctID = ISNULL(Chart,0) FROM Bank WHERE ID = @Bank    
  SELECT @PayrollTax_GL=PRTaxE FROM Emp WHERE ID=@ID 
  
  SET @holiday = ISNULL((SELECT ISNULL(Rate,0) FROM PROther WHERE Emp = @ID AND Cat = 1),0)*@holiday
SET @vacation = ISNULL((SELECT ISNULL(Rate,0) FROM PROther WHERE Emp = @ID AND Cat = 2),0)*@vacation
 -- --------------------- Revenue-------------          
 -- INSERT INTO #tempDetailPay(ID ,fDesc,RegQuan,RegRate,RegAmt,OTQuan,OTRate,OTAmt,NTQuan,NTRate,NTAmt,DTQuan,DTRate,DTAmt ,TTQuan ,TTRate ,TTAmt ,GL,EmpID )          
 --SELECT pd.ID, pd.fDesc,SUM(ISNULL(td.Reg, 0)) RegQuan,ISNULL(PRItem.Reg,0) RegRate,ISNULL(PRItem.Reg,0)*SUM(ISNULL(td.Reg, 0)) RegAmt ,          
 -- SUM(ISNULL(td.OT, 0)) OTQuan,ISNULL(PRItem.OT,0) OTRate,ISNULL(PRItem.OT,0)*SUM(ISNULL(td.OT, 0)) OTAmt,          
 -- SUM(ISNULL(td.NT, 0)) NTQuan,ISNULL(PRItem.NT,0) NTRate,ISNULL(PRItem.NT,0)*SUM(ISNULL(td.NT, 0)) NTAmt,          
 -- SUM(ISNULL(td.DT, 0)) DTQuan,ISNULL(PRItem.DT,0) DTRate,ISNULL(PRItem.DT,0)*SUM(ISNULL(td.DT, 0)) DTAmt,          
 -- SUM(ISNULL(td.TT, 0)) TTQuan,ISNULL(PRItem.TT,0) TTRate,ISNULL(PRItem.TT,0)*SUM(ISNULL(td.TT, 0)) TTAmt,           
 -- PRItem.GL,e.ID               
 -- FROM TicketD td INNER JOIN tblWork w on td.fWork = w.ID INNER JOIN emp e on e.CallSign = w.fDesc             
 -- INNER JOIN PRWage pd ON pd.ID = td.WageC    
 -- LEFT JOIN PRWageItem PRItem ON pd.ID = PRItem.Wage AND PRItem.Emp = e.ID    
 -- WHERE e.ID = @ID             
 -- AND CAST(EDate AS date) >= CAST(@startdate AS date)             
 -- AND CAST(EDate AS date) <= CAST(@enddate AS date) GROUP BY pd.ID,pd.fDesc,PRItem.Reg ,PRItem.OT,PRItem.NT,PRItem.DT,PRItem.TT,PRItem.GL,e.ID           
 -- HAVING SUM(ISNULL(td.Reg, 0))+SUM(ISNULL(td.OT, 0))+SUM(ISNULL(td.NT, 0))+SUM(ISNULL(td.DT, 0))+SUM(ISNULL(td.TT, 0)) >0          
          
             
 -- UPDATE PRWageItem SET           
 -- PRWageItem.YTD = ISNULL(prwg.YTD,0) +ISNULL(ttemp.RegAmt,0),          
 -- PRWageItem.YTDH = ISNULL(prwg.YTDH,0) +ISNULL(ttemp.RegQuan,0),          
 -- PRWageItem.OYTD = ISNULL(prwg.OYTD,0) +ISNULL(ttemp.OTAmt,0),          
 -- PRWageItem.OYTDH = ISNULL(prwg.OYTDH,0) +ISNULL(ttemp.OTQuan,0),          
 -- PRWageItem.DYTD = ISNULL(prwg.DYTD,0) +ISNULL(ttemp.DTAmt,0),          
 -- PRWageItem.DYTDH = ISNULL(prwg.DYTDH,0) +ISNULL(ttemp.DTQuan,0),          
 -- PRWageItem.TYTD = ISNULL(prwg.TYTD,0) +ISNULL(ttemp.TTAmt,0),          
 -- PRWageItem.TYTDH = ISNULL(prwg.TYTDH,0) +ISNULL(ttemp.TTQuan,0),          
 -- PRWageItem.NYTD = ISNULL(prwg.NYTD,0) +ISNULL(ttemp.NTAmt,0),          
 -- PRWageItem.NYTDH = ISNULL(prwg.NYTDH,0) +ISNULL(ttemp.NTQuan,0)          
 -- FROM  PRWageItem prwg  INNER JOIN #tempDetailPay ttemp ON prwg.Wage = ttemp.ID          
 -- WHERE prwg.Emp = @ID          
          
 -- -----------------------------------        
      
 SET @TransDesc = @Name+' - Week '+@Week         
 EXEC @TransId = [dbo].[AddJournal] null,@MAXBatch,@CDate,90,0,@PayRegRef,@TransDesc,0 ,@BankAcctID,@Bank,NULL,0,NULL,0,NULL,NULL    
     
   INSERT INTO PRReg([ID],[fDate],[Ref],[fDesc],[EmpID],[Bank],[TransID],[Reg] ,[YReg],[HReg],[HYReg],[OT] ,[YOT] ,[HOT] ,[HYOT] ,[DT] ,[YDT] ,[HDT] ,[HYDT] ,[TT] ,    
 [YTT] ,[HTT] ,[HYTT] ,[Hol] ,[YHol] ,[HHol] ,[HYHol] ,[Vac] ,[YVac] ,[HVac] ,[HYVac] ,[Zone] ,[YZone] ,[Reimb] ,[YReimb] ,[Mile] ,[YMile] ,[HMile] ,[HYMile] ,    
 [Bonus] ,[YBonus] ,[WFIT] ,[WFica] ,[WMedi] ,[WFuta] ,[WSit] ,[WVac] ,[WWComp] ,[WUnion] ,[FIT] ,[YFIT] ,[FICA] ,[YFICA] ,[MEDI] ,[YMEDI] ,[FUTA] , [YFUTA] ,    
 [SIT] ,[YSIT] ,[Local] ,[YLocal] ,[TOTher] ,[NT] ,[YTOTher] ,[TInc] ,[YNT] ,[HNT] ,[TDed] ,[HYNT] ,[Net] ,[State] ,[VThis] ,[REIMJE] ,[WELF] ,[SDI] ,[401K] ,    
 [GARN] ,[WeekNo] ,[Remarks] ,[ELast] ,[EThis] , [CompMedi] ,[WMediOverTH] ,[Sick] ,[YSick] ,[WSick] ,[HSick] ,[HYSick] ,[HSickAccrued] ,[HYSickAccrued] ,[HVacAccrued],[HYVacAccrued]  )    
VALUES (@PayRegMaxID,@CDate,@PayRegRef,@PeriodDescription,@ID,@Bank,@TransId,    
ISNULL((SELECT SUM(ISNULL(RegAmt,0)) FROM #tempDetailPay WHERE EmpID = @ID),0),    
ISNULL((SELECT SUM(ISNULL(Reg,0)) FROM PRReg WHERE EmpID = @ID AND fDate >= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0) AND fDate <= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()) + 1, -1)),0),    
ISNULL((SELECT SUM(ISNULL(RegQuan,0)) FROM #tempDetailPay WHERE EmpID = @ID),0),    
ISNULL((SELECT SUM(ISNULL(HReg,0)) FROM PRReg WHERE EmpID = @ID AND fDate >= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0) AND fDate <= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()) + 1, -1)),0),    
ISNULL((SELECT SUM(ISNULL(OTAmt,0)) FROM #tempDetailPay WHERE EmpID = @ID),0),    
ISNULL((SELECT SUM(ISNULL(OT,0)) FROM PRReg WHERE EmpID = @ID AND fDate >= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0) AND fDate <= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()) + 1, -1)),0),    
ISNULL((SELECT SUM(ISNULL(OTQuan,0)) FROM #tempDetailPay WHERE EmpID = @ID),0),    
ISNULL((SELECT SUM(ISNULL(HOT,0)) FROM PRReg WHERE EmpID = @ID AND fDate >= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0) AND fDate <= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()) + 1, -1)),0),    
ISNULL((SELECT SUM(ISNULL(DTAmt,0)) FROM #tempDetailPay WHERE EmpID = @ID),0),    
ISNULL((SELECT SUM(ISNULL(DT,0)) FROM PRReg WHERE EmpID = @ID AND fDate >= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0) AND fDate <= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()) + 1, -1)),0),    
ISNULL((SELECT SUM(ISNULL(DTQuan,0)) FROM #tempDetailPay WHERE EmpID = @ID),0),    
ISNULL((SELECT SUM(ISNULL(HDT,0)) FROM PRReg WHERE EmpID = @ID AND fDate >= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0) AND fDate <= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()) + 1, -1)),0),    
ISNULL((SELECT SUM(ISNULL(TTAmt,0)) FROM #tempDetailPay WHERE EmpID = @ID),0),    
ISNULL((SELECT SUM(ISNULL(TT,0)) FROM PRReg WHERE EmpID = @ID AND fDate >= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0) AND fDate <= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()) + 1, -1)),0),    
ISNULL((SELECT SUM(ISNULL(TTQuan,0)) FROM #tempDetailPay WHERE EmpID = @ID),0),    
ISNULL((SELECT SUM(ISNULL(HTT,0)) FROM PRReg WHERE EmpID = @ID AND fDate >= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0) AND fDate <= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()) + 1, -1)),0),    
@holiday,    
ISNULL((SELECT SUM(ISNULL(Hol,0)) FROM PRReg WHERE EmpID = @ID AND fDate >= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0) AND fDate <= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()) + 1, -1)),0),    
0,    
ISNULL((SELECT SUM(ISNULL(HHol,0)) FROM PRReg WHERE EmpID = @ID AND fDate >= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0) AND fDate <= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()) + 1, -1)),0),    
@vacation,    
ISNULL((SELECT SUM(ISNULL(Vac,0)) FROM PRReg WHERE EmpID = @ID AND fDate >= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0) AND fDate <= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()) + 1, -1)),0),    
0,    
ISNULL((SELECT SUM(ISNULL(HVac,0)) FROM PRReg WHERE EmpID = @ID AND fDate >= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0) AND fDate <= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()) + 1, -1)),0),    
@Zone,    
ISNULL((SELECT SUM(ISNULL(Zone,0)) FROM PRReg WHERE EmpID = @ID AND fDate >= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0) AND fDate <= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()) + 1, -1)),0),    
@reimb,    
ISNULL((SELECT SUM(ISNULL(Reimb,0)) FROM PRReg WHERE EmpID = @ID AND fDate >= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0) AND fDate <= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()) + 1, -1)),0),    
@Milage,    
ISNULL((SELECT SUM(ISNULL(Mile,0)) FROM PRReg WHERE EmpID = @ID AND fDate >= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0) AND fDate <= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()) + 1, -1)),0),    
0,    
ISNULL((SELECT SUM(ISNULL(HMile,0)) FROM PRReg WHERE EmpID = @ID AND fDate >= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0) AND fDate <= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()) + 1, -1)),0),    
@bonus,    
ISNULL((SELECT SUM(ISNULL(Bonus,0)) FROM PRReg WHERE EmpID = @ID AND fDate >= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0) AND fDate <= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()) + 1, -1)),0),    
--2250.22,2250.22,2250.22,2250.22,2250.22,2069.20,2250.22,2069.20,    
ISNULL((SELECT  SUM(ISNULL(RegAmt,0))+SUM(ISNULL(OTAmt,0))+SUM(ISNULL(DTAmt,0))+SUM(ISNULL(TTAmt,0))+ SUM(ISNULL(NTAmt,0))+ISNULL(@holiday,0)+ISNULL(@vacation,0)+ISNULL(@Zone,0)+ISNULL(@reimb,0)+ISNULL(@bonus,0)+ISNULL(@Milage,0)+ISNULL(@sicktime,0) FROM   
#tempDetailPay WHERE EmpID = @ID),0),    
ISNULL((SELECT  SUM(ISNULL(RegAmt,0))+SUM(ISNULL(OTAmt,0))+SUM(ISNULL(DTAmt,0))+SUM(ISNULL(TTAmt,0))+ SUM(ISNULL(NTAmt,0))+ISNULL(@holiday,0)+ISNULL(@vacation,0)+ISNULL(@Zone,0)+ISNULL(@reimb,0)+ISNULL(@bonus,0)+ISNULL(@Milage,0)+ISNULL(@sicktime,0) FROM   
#tempDetailPay WHERE EmpID = @ID),0),    
ISNULL((SELECT  SUM(ISNULL(RegAmt,0))+SUM(ISNULL(OTAmt,0))+SUM(ISNULL(DTAmt,0))+SUM(ISNULL(TTAmt,0))+ SUM(ISNULL(NTAmt,0))+ISNULL(@holiday,0)+ISNULL(@vacation,0)+ISNULL(@Zone,0)+ISNULL(@reimb,0)+ISNULL(@bonus,0)+ISNULL(@Milage,0)+ISNULL(@sicktime,0) FROM   
#tempDetailPay WHERE EmpID = @ID),0),    
ISNULL((SELECT  SUM(ISNULL(RegAmt,0))+SUM(ISNULL(OTAmt,0))+SUM(ISNULL(DTAmt,0))+SUM(ISNULL(TTAmt,0))+ SUM(ISNULL(NTAmt,0))+ISNULL(@holiday,0)+ISNULL(@vacation,0)+ISNULL(@Zone,0)+ISNULL(@reimb,0)+ISNULL(@bonus,0)+ISNULL(@Milage,0)+ISNULL(@sicktime,0) FROM   
#tempDetailPay WHERE EmpID = @ID),0),    
ISNULL((SELECT  SUM(ISNULL(RegAmt,0))+SUM(ISNULL(OTAmt,0))+SUM(ISNULL(DTAmt,0))+SUM(ISNULL(TTAmt,0))+ SUM(ISNULL(NTAmt,0))+ISNULL(@holiday,0)+ISNULL(@vacation,0)+ISNULL(@Zone,0)+ISNULL(@reimb,0)+ISNULL(@bonus,0)+ISNULL(@Milage,0)+ISNULL(@sicktime,0) FROM   
#tempDetailPay WHERE EmpID = @ID),0),    
ISNULL((SELECT  SUM(ISNULL(RegAmt,0))+SUM(ISNULL(OTAmt,0))+SUM(ISNULL(DTAmt,0))+SUM(ISNULL(TTAmt,0))+ SUM(ISNULL(NTAmt,0)) FROM #tempDetailPay WHERE EmpID = @ID),0),    
ISNULL((SELECT  SUM(ISNULL(RegAmt,0))+SUM(ISNULL(OTAmt,0))+SUM(ISNULL(DTAmt,0))+SUM(ISNULL(TTAmt,0))+ SUM(ISNULL(NTAmt,0))+ISNULL(@holiday,0)+ISNULL(@vacation,0)+ISNULL(@Zone,0)+ISNULL(@reimb,0)+ISNULL(@bonus,0)+ISNULL(@Milage,0)+ISNULL(@sicktime,0) FROM   
#tempDetailPay WHERE EmpID = @ID),0),    
ISNULL((SELECT  SUM(ISNULL(RegAmt,0))+SUM(ISNULL(OTAmt,0))+SUM(ISNULL(DTAmt,0))+SUM(ISNULL(TTAmt,0))+ SUM(ISNULL(NTAmt,0)) FROM #tempDetailPay WHERE EmpID = @ID),0),    
    
@FIT,--FIT    
ISNULL((SELECT SUM(ISNULL(FIT,0)) FROM PRReg WHERE EmpID = @ID AND fDate >= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0) AND fDate <= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()) + 1, -1)),0),    
@FICA,--FICA    
ISNULL((SELECT SUM(ISNULL(FICA,0)) FROM PRReg WHERE EmpID = @ID AND fDate >= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0) AND fDate <= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()) + 1, -1)),0),    
@MEDI,--MEDI    
ISNULL((SELECT SUM(ISNULL(MEDI,0)) FROM PRReg WHERE EmpID = @ID AND fDate >= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0) AND fDate <= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()) + 1, -1)),0),    
0,--FUTA    
ISNULL((SELECT SUM(ISNULL(FUTA,0)) FROM PRReg WHERE EmpID = @ID AND fDate >= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0) AND fDate <= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()) + 1, -1)),0),    
@SIT,--SIT    
ISNULL((SELECT SUM(ISNULL(SIT,0)) FROM PRReg WHERE EmpID = @ID AND fDate >= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0) AND fDate <= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()) + 1, -1)),0),    
@LOCAL,--LOCAL    
ISNULL((SELECT SUM(ISNULL(Local,0)) FROM PRReg WHERE EmpID = @ID AND fDate >= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0) AND fDate <= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()) + 1, -1)),0),    
@OtherE,--Other    
ISNULL((SELECT SUM(ISNULL(NTAmt,0)) FROM #tempDetailPay WHERE EmpID = @ID),0),--NT    
ISNULL((SELECT SUM(ISNULL(TOther,0)) FROM PRReg WHERE EmpID = @ID AND fDate >= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0) AND fDate <= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()) + 1, -1)),0),    
0,--Inc    
ISNULL((SELECT SUM(ISNULL(NT,0)) FROM PRReg WHERE EmpID = @ID AND fDate >= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0) AND fDate <= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()) + 1, -1)),0),    
ISNULL((SELECT SUM(ISNULL(NTQuan,0)) FROM #tempDetailPay WHERE EmpID = @ID),0),--HNT    
0,--TDed (Total Deduction)    
ISNULL((SELECT SUM(ISNULL(HNT,0)) FROM PRReg WHERE EmpID = @ID AND fDate >= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0) AND fDate <= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()) + 1, -1)),0),    
0, --Net     
(SELECT TOP 1 [State] FROM Emp WHERE ID = @ID),--STate    
0,--Vacation Accured this Check    
NULL,NULL,NULL,NULL,NULL,NULL,@Memo,    
0,--Vacation Accured this Check    
0, 0 ,-- MEDI COMP    
0,    
@sicktime,--Sick    
ISNULL((SELECT SUM(ISNULL(Sick,0)) FROM PRReg WHERE EmpID = @ID AND fDate >= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0) AND fDate <= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()) + 1, -1)),0),    
0,--Wsick    
0,    
ISNULL((SELECT SUM(ISNULL(HSick,0)) FROM PRReg WHERE EmpID = @ID AND fDate >= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0) AND fDate <= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()) + 1, -1)),0),    
0,    
ISNULL((SELECT SUM(ISNULL(HSickAccrued,0)) FROM PRReg WHERE EmpID = @ID AND fDate >= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0) AND fDate <= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()) + 1, -1)),0),    
0,    
ISNULL((SELECT SUM(ISNULL(HVacAccrued,0)) FROM PRReg WHERE EmpID = @ID AND fDate >= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()), 0) AND fDate <= DATEADD(yy, DATEDIFF(yy, 0, GETDATE()) + 1, -1)),0)    
)    
    
    
 -----------------------------------        
    DECLARE db_cursor101 CURSOR FOR           
    SELECT ID,GL,ISNULL(td.RegAmt, 0)+ISNULL(td.OTAmt, 0)+ISNULL(td.NTAmt, 0)+ISNULL(td.DTAmt, 0)+ISNULL(td.TTAmt, 0) AS TotAmt FROM #tempDetailPay td WHERE EmpID = @ID       
          
 OPEN db_cursor101            
 FETCH NEXT FROM db_cursor101 INTO           
 @A_ID,@A_GL,@A_TotalAmt          
            
 WHILE @@FETCH_STATUS = 0          
 BEGIN         
  SET @TransDesc = @Name+' - '+@PeriodDescription+' Week '+@Week  
  IF @A_TotalAmt IS NULL
  BEGIN 
	SET @A_TotalAmt = ISNULL(@A_TotalAmt,0)
  END
  EXEC @TransId = [dbo].[AddJournal] null,@MAXBatch,@CDate,91,0,@PayRegRef,@TransDesc,@A_TotalAmt ,@A_GL,NULL,NULL,0,NULL,0,NULL,NULL         
 --------RESET------->          
   SET @A_ID= NULL ;SET @A_GL = NULL ;SET @A_TotalAmt = NULL ;        
   --------------->         
        
 FETCH NEXT FROM db_cursor101 INTO           
 @A_ID,@A_GL,@A_TotalAmt         
 END            
          
    CLOSE db_cursor101            
 DEALLOCATE db_cursor101          
 -----------------------------------  
 -----------------------------------                  
  --------------------- Bonus-------------        
  
  IF ISNULL(@bonus,0) >0         
  BEGIN        
  SELECT @BonusGL= GL FROM PROther WHERE  PROther.Emp=@ID AND PROther.Cat = 0        
 SET @TransDesc = @Name+' - Bonus pay Week '+@Week  
 IF @bonus IS NULL
  BEGIN 
	SET @bonus = 0
  END
 EXEC @TransId = [dbo].[AddJournal] null,@MAXBatch,@CDate,91,0,@PayRegRef,@TransDesc,@bonus ,@BonusGL,NULL,NULL,0,NULL,0,NULL,NULL           
 
  END        
  --------------------- Bonus-------------        
  --------------------- Holiday-------------        
  
  IF ISNULL(@holiday,0) >0         
  BEGIN        
  SELECT @HolidayGL = GL FROM PROther WHERE  PROther.Emp=@ID AND PROther.Cat = 1        
 SET @TransDesc = @Name+' - Holiday pay Week '+@Week 
 IF @holiday IS NULL
  BEGIN 
	SET @holiday = 0
  END
 EXEC @TransId = [dbo].[AddJournal] null,@MAXBatch,@CDate,91,0,@PayRegRef,@TransDesc,@holiday ,@HolidayGL,NULL,NULL,0,NULL,0,NULL,NULL           
 
  END        
  --------------------- Holiday-------------        
  --------------------- Vacation-------------        
  
  IF ISNULL(@vacation,0) >0         
  BEGIN        
  SELECT @VacationGL = GL FROM PROther WHERE  PROther.Emp=@ID AND PROther.Cat = 2        
 SET @TransDesc = @Name+' - Vacation pay Week '+@Week   
 IF @vacation IS NULL
  BEGIN 
	SET @vacation = 0
  END
 EXEC @TransId = [dbo].[AddJournal] null,@MAXBatch,@CDate,91,0,@PayRegRef,@TransDesc,@vacation ,@VacationGL,NULL,NULL,0,NULL,0,NULL,NULL           
  
  END        
  --------------------- Vacation-------------        
  --------------------- Zone-------------        
  
  IF ISNULL(@Zone,0) >0         
  BEGIN        
  SELECT @ZoneGL= GL FROM PROther WHERE  PROther.Emp=@ID AND PROther.Cat = 3        
 SET @TransDesc = @Name+' - Zone pay Week '+@Week  
 IF @Zone IS NULL
  BEGIN 
	SET @Zone = 0
  END
 EXEC @TransId = [dbo].[AddJournal] null,@MAXBatch,@CDate,91,0,@PayRegRef,@TransDesc,@Zone ,@ZoneGL,NULL,NULL,0,NULL,0,NULL,NULL 
  
  END        
  --------------------- Zone-------------        
  --------------------- Reimbursement-------------        
  
  IF ISNULL(@reimb,0) >0         
  BEGIN        
  SELECT @ReimbursementGL= GL FROM PROther WHERE  PROther.Emp=@ID AND PROther.Cat = 4        
 SET @TransDesc = @Name+' - Reimbursement pay Week '+@Week    
 IF @reimb IS NULL
  BEGIN 
	SET @reimb = 0
  END
 EXEC @TransId = [dbo].[AddJournal] null,@MAXBatch,@CDate,91,0,@PayRegRef,@TransDesc,@reimb ,@ReimbursementGL,NULL,NULL,0,NULL,0,NULL,NULL           
 
  END        
  --------------------- Reimbursement-------------        
  --------------------- Mileage-------------        
  
  IF ISNULL(@Milage,0) >0         
  BEGIN        
  SELECT @MileageGL= GL FROM PROther WHERE  PROther.Emp=@ID AND PROther.Cat = 5        
 SET @TransDesc = @Name+' - Milage pay Week '+@Week  
 IF @Milage IS NULL
  BEGIN 
	SET @Milage = 0
  END
 EXEC @TransId = [dbo].[AddJournal] null,@MAXBatch,@CDate,91,0,@PayRegRef,@TransDesc,@Milage ,@MileageGL,NULL,NULL,0,NULL,0,NULL,NULL           
 
  END        
  --------------------- Mileage-------------        
  --------------------- Sick-------------        
  
  IF ISNULL(@sicktime,0) >0         
  BEGIN        
  SELECT @SickGL= GL FROM PROther WHERE  PROther.Emp=@ID AND PROther.Cat = 6        
 SET @TransDesc = @Name+' - Sick pay Week '+@Week  
 IF @sicktime IS NULL
  BEGIN 
	SET @sicktime =0
  END
 EXEC @TransId = [dbo].[AddJournal] null,@MAXBatch,@CDate,91,0,@PayRegRef,@TransDesc,@sicktime ,@SickGL,NULL,NULL,0,NULL,0,NULL,NULL           
 
  END        
  --------------------- Sick-------------        
          
        
   --------------------- FEDRAL TAX-------------        
          
  IF ISNULL(@FIT,0) >0         
  BEGIN        
 SET @TransDesc = @Name+' - FIT Week '+@Week         
 SET @FIT = @FIT*-1      
 EXEC @TransId = [dbo].[AddJournal] null,@MAXBatch,@CDate,91,0,@PayRegRef,@TransDesc,@FIT ,@Payroll_GL,NULL,NULL,0,NULL,0,NULL,NULL 
 
  END        
  --------------------- FEDRAL TAX-------------        
   --------------------- STATE TAX-------------        
        
  IF ISNULL(@SIT,0) >0         
  BEGIN        
 SET @TransDesc = @Name+' - SIT Week '+@Week         
 SET @SIT = @SIT*-1      
 EXEC @TransId = [dbo].[AddJournal] null,@MAXBatch,@CDate,91,0,@PayRegRef,@TransDesc,@SIT ,@Payroll_GL,NULL,NULL,0,NULL,0,NULL,NULL           

  END        
  --------------------- STATE TAX-------------        
   --------------------- LOCAL TAX-------------        
        
  IF ISNULL(@LOCAL,0) >0         
  BEGIN        
 SET @TransDesc = @Name+' - Local Week '+@Week         
 SET @LOCAL = @LOCAL*-1       EXEC @TransId = [dbo].[AddJournal] null,@MAXBatch,@CDate,91,0,@PayRegRef,@TransDesc,@LOCAL ,@Payroll_GL,NULL,NULL,0,NULL,0,NULL,NULL           

  END        
  --------------------- LOCAL TAX-------------        
   --------------------- MEDI-------------    
   IF ISNULL(@MEDI,0) >0         
  BEGIN        
 SET @TransDesc = @Name+' - MEDI Week '+@Week         
 EXEC @TransId = [dbo].[AddJournal] null,@MAXBatch,@CDate,91,0,@PayRegRef,@TransDesc,@MEDI ,@PayrollTax_GL,NULL,NULL,0,NULL,0,NULL,NULL           

 SET @Ded_DeductionAmount = (@MEDI*2)*-1        
 EXEC @TransId = [dbo].[AddJournal] null,@MAXBatch,@CDate,91,0,@PayRegRef,@TransDesc,@Ded_DeductionAmount ,@Payroll_GL,NULL,NULL,0,NULL,0,NULL,NULL           

 SET @Ded_DeductionAmount = NULL;
  END 

 -- SELECT  @Ded_DeductionAmount = SUM(ISNULL(td.RegAmt, 0))+SUM(ISNULL(td.OTAmt, 0))+SUM(ISNULL(td.NTAmt, 0))+SUM(ISNULL(td.DTAmt, 0))+SUM(ISNULL(td.TTAmt, 0)) FROM #tempDetailPay td        
 -- SET @Ded_DeductionAmount = ISNULL(@Ded_DeductionAmount,0)+ISNULL(@bonus,0)+ISNULL(@holiday,0)+ISNULL(@vacation,0)+ISNULL(@Zone,0)+ISNULL(@reimb,0)+ISNULL(@Milage,0)+ISNULL(@sicktime,0)        
 -- SELECT @Ded_DeductionAmount = (ISNULL(@Ded_DeductionAmount,0)*ISNULL(EERate,0))/100 FROM TaxTable WHERE Tax='MEDI'          
 -- IF ISNULL(@Ded_DeductionAmount,0) >0         
 -- BEGIN        
 --SET @TransDesc = @Name+' - MEDI Week '+@Week         
 --EXEC @TransId = [dbo].[AddJournal] null,@MAXBatch,@CDate,91,0,@PayRegRef,@TransDesc,@Ded_DeductionAmount ,@PayrollTax_GL,NULL,NULL,0,NULL,0,NULL,NULL           
 --SET @Ded_DeductionAmount = (@Ded_DeductionAmount*2)*-1        
 --EXEC @TransId = [dbo].[AddJournal] null,@MAXBatch,@CDate,91,0,@PayRegRef,@TransDesc,@Ded_DeductionAmount ,@Payroll_GL,NULL,NULL,0,NULL,0,NULL,NULL           
 -- END        
  --------------------- MEDI-------------        
   --------------------- FICA------------- 
   IF ISNULL(@FICA,0) >0         
  BEGIN        
 SET @TransDesc = @Name+' - FICA Week '+@Week         
 EXEC @TransId = [dbo].[AddJournal] null,@MAXBatch,@CDate,91,0,@PayRegRef,@TransDesc,@FICA ,@PayrollTax_GL,NULL,NULL,0,NULL,0,NULL,NULL           

 SET @Ded_DeductionAmount = (@FICA*2)*-1        
 EXEC @TransId = [dbo].[AddJournal] null,@MAXBatch,@CDate,91,0,@PayRegRef,@TransDesc,@Ded_DeductionAmount ,@Payroll_GL,NULL,NULL,0,NULL,0,NULL,NULL           
 
 SET @Ded_DeductionAmount = NULL;
  END

 -- SELECT  @Ded_DeductionAmount = SUM(ISNULL(td.RegAmt, 0))+SUM(ISNULL(td.OTAmt, 0))+SUM(ISNULL(td.NTAmt, 0))+SUM(ISNULL(td.DTAmt, 0))+SUM(ISNULL(td.TTAmt, 0)) FROM #tempDetailPay td        
 -- SET @Ded_DeductionAmount = ISNULL(@Ded_DeductionAmount,0)+ISNULL(@bonus,0)+ISNULL(@holiday,0)+ISNULL(@vacation,0)+ISNULL(@Zone,0)+ISNULL(@reimb,0)+ISNULL(@Milage,0)+ISNULL(@sicktime,0)        
 -- SELECT @Ded_DeductionAmount = (ISNULL(@Ded_DeductionAmount,0)*ISNULL(EERate,0))/100 FROM TaxTable WHERE Tax='FICA'          
 -- IF ISNULL(@Ded_DeductionAmount,0) >0         
 -- BEGIN        
 --SET @TransDesc = @Name+' - FICA Week '+@Week         
 --EXEC @TransId = [dbo].[AddJournal] null,@MAXBatch,@CDate,91,0,@PayRegRef,@TransDesc,@Ded_DeductionAmount ,@PayrollTax_GL,NULL,NULL,0,NULL,0,NULL,NULL         
 --SET @Ded_DeductionAmount = (@Ded_DeductionAmount*2)*-1        
 --EXEC @TransId = [dbo].[AddJournal] null,@MAXBatch,@CDate,91,0,@PayRegRef,@TransDesc,@Ded_DeductionAmount ,@Payroll_GL,NULL,NULL,0,NULL,0,NULL,NULL           
 -- END        
  --------------------- FICA-------------        
   --------------------- FUTA-------------        
 -- SELECT  @Ded_DeductionAmount = SUM(ISNULL(td.RegAmt, 0))+SUM(ISNULL(td.OTAmt, 0))+SUM(ISNULL(td.NTAmt, 0))+SUM(ISNULL(td.DTAmt, 0))+SUM(ISNULL(td.TTAmt, 0)) FROM #tempDetailPay td        
 -- SET @Ded_DeductionAmount = ISNULL(@Ded_DeductionAmount,0)+ISNULL(@bonus,0)+ISNULL(@holiday,0)+ISNULL(@vacation,0)+ISNULL(@Zone,0)+ISNULL(@reimb,0)+ISNULL(@Milage,0)+ISNULL(@sicktime,0)        
 -- SELECT @Ded_DeductionAmount = (ISNULL(@Ded_DeductionAmount,0)*ISNULL(ERRate,0))/10 FROM TaxTable WHERE Tax='FUTA'          
 -- IF ISNULL(@Ded_DeductionAmount,0) >0         
 -- BEGIN        
 --SET @TransDesc = @Name+' - FUTA Week '+@Week         
 --EXEC @TransId = [dbo].[AddJournal] null,@MAXBatch,@CDate,91,0,@PayRegRef,@TransDesc,@Ded_DeductionAmount ,@PayrollTax_GL,NULL,NULL,0,NULL,0,NULL,NULL         
 --SET @Ded_DeductionAmount = (@Ded_DeductionAmount*2)*-1        
 --EXEC @TransId = [dbo].[AddJournal] null,@MAXBatch,@CDate,91,0,@PayRegRef,@TransDesc,@Ded_DeductionAmount ,@Payroll_GL,NULL,NULL,0,NULL,0,NULL,NULL           
 -- END        
  --------------------- FUTA-------------        
  SET @Ded_DeductionAmount = NULL;        
        
            
  INSERT INTO PRRegWItem (CheckID,PRWID,Quan,Rate,Amount,YQuan,YAmount,OQuan,ORate,OAmount,OYQuan,OYAmount,DQuan,DRate,DAmount,DYQuan,DYAmount,TQuan,TRate,TAmount,          
  TYQuan,TYAmount,NQuan,NRate,NAmount,NYQuan,NYAmount)          
  SELECT @PayRegMaxID,t.ID,t.RegQuan,t.RegRate,t.RegAmt,p.YTDH,p.YTD,t.OTQuan,t.OTRate,t.OTAmt,p.OYTDH,p.OYTD,t.DTQuan,t.DTRate,t.DTAmt,p.DYTDH,p.DYTD,          
  t.TTQuan,t.TTRate,t.TTAmt,p.TYTDH,p.TYTD,t.NTQuan,t.NTRate,t.NTAmt,p.NYTDH,p.NYTD          
  FROM #tempDetailPay t INNER JOIN PRWageItem p ON p.Wage = t.ID AND p.Emp = @ID          
          
  --------------------- Revenue-------------        
          
        
          
  --INSERT INTO #tempprol (ID,[Name],Reg,OT,DT,TT,NT,[Zone],Milage,Toll,OtherE,pay,holiday,vacation,sicktime,reimb,bonus,paymethod,pmethod,userid, usertype,total,phour,salary,HourlyRate,FIT,SIT,[LOCAL])          
  --VALUES (@ID,@Name,@Reg,@OT,@DT,@TT,@NT,@Zone,@Milage,@Toll,@OtherE,@pay,@holiday,@vacation,@sicktime,@reimb,@bonus,@paymethod,@pmethod,@userid,@usertype,@total,@phour,        
  --@salary,@HourlyRate,@FIT,@SIT,@LOCAL)          
          


 -------------------------------------------------------------------
 INSERT INTO Trans ([Batch] ,[fDate] ,[Type] ,[Line] ,[Ref] ,[fDesc] ,[Amount] ,[Acct] ,[AcctSub] ,[Status] ,[Sel] ,[VInt] ,[VDoub]
           ,[EN] ,[strRef])
			SELECT @MAXBatch,@CDate,91,0,@PayRegRef,@Name+' '+'Contribution '+d.fDesc+' -  Week '+@Week, 
			ISNULL(ISNULL((SELECT SUM(ISNULL(RegQuan,0)) +SUM(ISNULL(OTQuan,0))+SUM(ISNULL(DTQuan,0))+SUM(ISNULL(NTQuan,0))+SUM(ISNULL(TTQuan,0))
			FROM #tempDetailPay WHERE EmpID = i.Emp),0) * ISNULL(i.CompRate,0),0)
			+ ISNULL(@holiday,0)+ ISNULL(@vacation,0) + ISNULL(@Zone,0) +ISNULL(@reimb,0) +
			ISNULL(@Milage,0) + ISNULL(@bonus,0),i.CompGLE,NULL,NULL,0,NULL,0,NULL,NULL
			--SELECT i.Ded,i.Emp,i.BasedOn,i.AccruedOn,i.ByW,i.EmpRate,i.EmpTop,i.EmpGL,i.CompRate,i.CompTop,i.CompGL,i.CompGLE,i.InUse,i.YTD,i.YTDC,d.fDesc,d.Job 
			--,(SELECT SUM(ISNULL(RegQuan,0)) FROM #tempDetailPay WHERE EmpID = i.Emp) * ISNULL(i.CompRate,0)
			FROM PRDedItem i INNER JOIN PRDed d ON d.ID = i.Ded  	 	 
			WHERE Emp = @ID AND i.ByW = 0 AND i.AccruedOn = 0 AND d.Job = 0 AND (i.CompTop =0 OR i.YTD < i.CompTop)--Paid By-Company and Accoured on Hour AND JobSpecific is false AND (comptop =0 OR YTD < Comptop)

			INSERT INTO Trans ([Batch] ,[fDate] ,[Type] ,[Line] ,[Ref] ,[fDesc] ,[Amount] ,[Acct] ,[AcctSub] ,[Status] ,[Sel] ,[VInt] ,[VDoub]
           ,[EN] ,[strRef])
			SELECT @MAXBatch,@CDate,91,0,@PayRegRef,@Name+' '+''+d.fDesc+' -  Week '+@Week, 
			(ISNULL((ISNULL((SELECT SUM(ISNULL(RegQuan,0))+SUM(ISNULL(OTQuan,0))+SUM(ISNULL(DTQuan,0))+SUM(ISNULL(NTQuan,0))+SUM(ISNULL(TTQuan,0)) FROM #tempDetailPay WHERE EmpID = i.Emp),0) * ISNULL(i.CompRate,0)),0)
			+ ISNULL(@holiday,0)+ ISNULL(@vacation,0) + ISNULL(@Zone,0) +ISNULL(@reimb,0) +
			ISNULL(@Milage,0) + ISNULL(@bonus,0))			
			*-1 ,i.CompGL,NULL,NULL,0,NULL,0,NULL,NULL
			--SELECT i.Ded,i.Emp,i.BasedOn,i.AccruedOn,i.ByW,i.EmpRate,i.EmpTop,i.EmpGL,i.CompRate,i.CompTop,i.CompGL,i.CompGLE,i.InUse,i.YTD,i.YTDC,d.fDesc,d.Job 
			--,(SELECT SUM(ISNULL(RegQuan,0)) FROM #tempDetailPay WHERE EmpID = i.Emp) * ISNULL(i.CompRate,0)
			FROM PRDedItem i INNER JOIN PRDed d ON d.ID = i.Ded  	 	 
			WHERE Emp = @ID AND i.ByW = 0 AND i.AccruedOn = 0 AND d.Job = 0 AND (i.CompTop =0 OR i.YTD < i.CompTop)--Paid By-Company and Accoured on Hour AND JobSpecific is false AND (comptop =0 OR YTD < Comptop)

	 ------------------- Job Specific Comp ------------------
	 IF EXISTS ( SELECT 1 FROM JobDed jd WHERE Job in (
			SELECT td.Job FROM TicketD td INNER JOIN tblWork w on td.fWork = w.ID INNER JOIN emp e on e.CallSign = w.fDesc
			WHERE e.ID = @ID AND CAST(EDate AS date) >= CAST(@startdate AS date)             
			AND CAST(EDate AS date) <= CAST(@enddate AS date) ) AND Ded  in (SELECT i.Ded 	 
	 FROM PRDedItem i INNER JOIN PRDed d ON d.ID = i.Ded  	 	 
	 WHERE Emp = @ID AND i.ByW = 0 AND i.AccruedOn = 0 AND d.Job = 1 AND (i.CompTop =0 OR i.YTD < i.CompTop)) )
	 BEGIN
		INSERT INTO Trans ([Batch] ,[fDate] ,[Type] ,[Line] ,[Ref] ,[fDesc] ,[Amount] ,[Acct] ,[AcctSub] ,[Status] ,[Sel] ,[VInt] ,[VDoub]
           ,[EN] ,[strRef])
		--SELECT i.Ded,i.Emp,i.BasedOn,i.AccruedOn,i.ByW,i.EmpRate,i.EmpTop,i.EmpGL,i.CompRate,i.CompTop,i.CompGL,i.CompGLE,i.InUse,i.YTD,i.YTDC,d.fDesc,d.Job 
		--,(SELECT SUM(ISNULL(RegQuan,0)) FROM #tempDetailPay WHERE EmpID = i.Emp) * ISNULL(i.CompRate,0)
		SELECT @MAXBatch,@CDate,91,0,@PayRegRef,@Name+' '+'Contribution '+d.fDesc+' -  Week '+@Week, 
			(ISNULL(ISNULL((SELECT SUM(ISNULL(RegQuan,0))+SUM(ISNULL(OTQuan,0))+SUM(ISNULL(DTQuan,0))+SUM(ISNULL(NTQuan,0))+
			SUM(ISNULL(TTQuan,0)) FROM #tempDetailPay WHERE EmpID = i.Emp),0) * ISNULL(i.CompRate,0),0)
			+ ISNULL(@holiday,0)+ ISNULL(@vacation,0) + ISNULL(@Zone,0) +ISNULL(@reimb,0) +
			ISNULL(@Milage,0) + ISNULL(@bonus,0))
			,i.CompGLE,NULL,NULL,0,NULL,0,NULL,NULL			
		FROM PRDedItem i INNER JOIN PRDed d ON d.ID = i.Ded  	 	 
		WHERE Emp = @ID AND i.ByW = 0 AND i.AccruedOn = 0 AND d.Job = 1 AND (i.CompTop =0 OR i.YTD < i.CompTop)--Paid By-Company and Accoured on Hour AND JobSpecific is true AND (comptop =0 OR YTD < Comptop)

		INSERT INTO Trans ([Batch] ,[fDate] ,[Type] ,[Line] ,[Ref] ,[fDesc] ,[Amount] ,[Acct] ,[AcctSub] ,[Status] ,[Sel] ,[VInt] ,[VDoub]
           ,[EN] ,[strRef])
		--SELECT i.Ded,i.Emp,i.BasedOn,i.AccruedOn,i.ByW,i.EmpRate,i.EmpTop,i.EmpGL,i.CompRate,i.CompTop,i.CompGL,i.CompGLE,i.InUse,i.YTD,i.YTDC,d.fDesc,d.Job 
		--,(SELECT SUM(ISNULL(RegQuan,0)) FROM #tempDetailPay WHERE EmpID = i.Emp) * ISNULL(i.CompRate,0)
		SELECT @MAXBatch,@CDate,91,0,@PayRegRef,@Name+' '+''+d.fDesc+' -  Week '+@Week, 
			(((ISNULL((SELECT SUM(ISNULL(RegQuan,0))+SUM(ISNULL(OTQuan,0))+SUM(ISNULL(DTQuan,0))+
			SUM(ISNULL(NTQuan,0))+SUM(ISNULL(TTQuan,0)) FROM #tempDetailPay WHERE EmpID = i.Emp),0) * ISNULL(i.CompRate,0))*-1)
			+ ISNULL(@holiday,0)+ ISNULL(@vacation,0) + ISNULL(@Zone,0) +ISNULL(@reimb,0) +
			ISNULL(@Milage,0) + ISNULL(@bonus,0))
			
			,i.CompGL,NULL,NULL,0,NULL,0,NULL,NULL
		FROM PRDedItem i INNER JOIN PRDed d ON d.ID = i.Ded  	 	 
		WHERE Emp = @ID AND i.ByW = 0 AND i.AccruedOn = 0 AND d.Job = 1 AND (i.CompTop =0 OR i.YTD < i.CompTop)--Paid By-Company and Accoured on Hour AND JobSpecific is true AND (comptop =0 OR YTD < Comptop)
	 END
	 ------------------- Job Specific Comp ------------------
	 ------------------- Accoured on Dollor Amount  ------------------
	 INSERT INTO Trans ([Batch] ,[fDate] ,[Type] ,[Line] ,[Ref] ,[fDesc] ,[Amount] ,[Acct] ,[AcctSub] ,[Status] ,[Sel] ,[VInt] ,[VDoub]
           ,[EN] ,[strRef])
	 --SELECT i.Ded,i.Emp,i.BasedOn,i.AccruedOn,i.ByW,i.EmpRate,i.EmpTop,i.EmpGL,i.CompRate,i.CompTop,i.CompGL,i.CompGLE,i.InUse,i.YTD,i.YTDC,d.fDesc,d.Job 
	 --,((SELECT SUM(ISNULL(RegAmt,0)) FROM #tempDetailPay WHERE EmpID = i.Emp) * ISNULL(i.CompRate,0))/100
	 SELECT @MAXBatch,@CDate,91,0,@PayRegRef,@Name+' '+'Contribution '+d.fDesc+' -  Week '+@Week, 
			(((ISNULL((SELECT SUM(ISNULL(RegAmt,0))
			+SUM(ISNULL(OTAmt,0))+SUM(ISNULL(DTAmt,0))+SUM(ISNULL(TTAmt,0))+SUM(ISNULL(NTAmt,0))
			FROM #tempDetailPay WHERE EmpID = i.Emp),0) * ISNULL(i.CompRate,0))/100)
			+ ISNULL(@holiday,0)+ ISNULL(@vacation,0) + ISNULL(@Zone,0) +ISNULL(@reimb,0) +
			ISNULL(@Milage,0) + ISNULL(@bonus,0))
			,i.CompGLE,NULL,NULL,0,NULL,0,NULL,NULL
	 FROM PRDedItem i INNER JOIN PRDed d ON d.ID = i.Ded  	 	 
	 WHERE Emp = @ID AND i.ByW = 0 AND i.AccruedOn = 1 AND d.Job = 0 AND (i.CompTop =0 OR i.YTD < i.CompTop)--Paid By-Company and Accoured on Dollor Amount AND JobSpecific is false AND (comptop =0 OR YTD < Comptop)

	 INSERT INTO Trans ([Batch] ,[fDate] ,[Type] ,[Line] ,[Ref] ,[fDesc] ,[Amount] ,[Acct] ,[AcctSub] ,[Status] ,[Sel] ,[VInt] ,[VDoub]
           ,[EN] ,[strRef])
	 --SELECT i.Ded,i.Emp,i.BasedOn,i.AccruedOn,i.ByW,i.EmpRate,i.EmpTop,i.EmpGL,i.CompRate,i.CompTop,i.CompGL,i.CompGLE,i.InUse,i.YTD,i.YTDC,d.fDesc,d.Job 
	 --,((SELECT SUM(ISNULL(RegAmt,0)) FROM #tempDetailPay WHERE EmpID = i.Emp) * ISNULL(i.CompRate,0))/100
	 SELECT @MAXBatch,@CDate,91,0,@PayRegRef,@Name+' '+''+d.fDesc+' -  Week '+@Week, 
			((((ISNULL((SELECT SUM(ISNULL(RegAmt,0))
			+SUM(ISNULL(OTAmt,0))+SUM(ISNULL(DTAmt,0))+SUM(ISNULL(TTAmt,0))+SUM(ISNULL(NTAmt,0)) 
			FROM #tempDetailPay WHERE EmpID = i.Emp),0) * ISNULL(i.CompRate,0))/100))
			+ ISNULL(@holiday,0)+ ISNULL(@vacation,0) + ISNULL(@Zone,0) +ISNULL(@reimb,0) +
			ISNULL(@Milage,0) + ISNULL(@bonus,0))
			*-1 ,i.CompGL,NULL,NULL,0,NULL,0,NULL,NULL
	 FROM PRDedItem i INNER JOIN PRDed d ON d.ID = i.Ded  	 	 
	 WHERE Emp = @ID AND i.ByW = 0 AND i.AccruedOn = 1 AND d.Job = 0 AND (i.CompTop =0 OR i.YTD < i.CompTop)--Paid By-Company and Accoured on Dollor Amount AND JobSpecific is false AND (comptop =0 OR YTD < Comptop)
	 ------------------- Accoured on Dollor Amount  ------------------
	 ------------------- Job Specific Comp Dollor Amount------------------
	 IF EXISTS ( SELECT 1 FROM JobDed jd WHERE Job in (
			SELECT td.Job FROM TicketD td INNER JOIN tblWork w on td.fWork = w.ID INNER JOIN emp e on e.CallSign = w.fDesc
			WHERE e.ID = @ID AND CAST(EDate AS date) >= CAST(@startdate AS date)             
			AND CAST(EDate AS date) <= CAST(@enddate AS date) ) AND Ded  in (SELECT i.Ded 	 
	 FROM PRDedItem i INNER JOIN PRDed d ON d.ID = i.Ded  	 	 
	 WHERE Emp = @ID AND i.ByW = 0 AND i.AccruedOn = 1 AND d.Job = 1 AND (i.CompTop =0 OR i.YTD < i.CompTop)) )
	 BEGIN
		INSERT INTO Trans ([Batch] ,[fDate] ,[Type] ,[Line] ,[Ref] ,[fDesc] ,[Amount] ,[Acct] ,[AcctSub] ,[Status] ,[Sel] ,[VInt] ,[VDoub]
           ,[EN] ,[strRef])
		--SELECT i.Ded,i.Emp,i.BasedOn,i.AccruedOn,i.ByW,i.EmpRate,i.EmpTop,i.EmpGL,i.CompRate,i.CompTop,i.CompGL,i.CompGLE,i.InUse,i.YTD,i.YTDC,d.fDesc,d.Job 
		--,((SELECT SUM(ISNULL(RegAmt,0)) FROM #tempDetailPay WHERE EmpID = i.Emp) * ISNULL(i.CompRate,0))/100
		SELECT @MAXBatch,@CDate,91,0,@PayRegRef,@Name+' '+'Contribution '+d.fDesc+' -  Week '+@Week, 
			(((ISNULL((SELECT SUM(ISNULL(RegAmt,0)) +SUM(ISNULL(OTAmt,0))+SUM(ISNULL(DTAmt,0))+SUM(ISNULL(TTAmt,0))+
			SUM(ISNULL(NTAmt,0)) FROM #tempDetailPay WHERE EmpID = i.Emp),0) * ISNULL(i.CompRate,0))/100)
			+ ISNULL(@holiday,0)+ ISNULL(@vacation,0) + ISNULL(@Zone,0) +ISNULL(@reimb,0) +
			ISNULL(@Milage,0) + ISNULL(@bonus,0))
			,i.CompGLE,NULL,NULL,0,NULL,0,NULL,NULL
		FROM PRDedItem i INNER JOIN PRDed d ON d.ID = i.Ded  	 	 
		WHERE Emp = @ID AND i.ByW = 0 AND i.AccruedOn = 1 AND d.Job = 1 AND (i.CompTop =0 OR i.YTD < i.CompTop)--Paid By-Company and Accoured on Hour AND JobSpecific is true AND (comptop =0 OR YTD < Comptop)

				INSERT INTO Trans ([Batch] ,[fDate] ,[Type] ,[Line] ,[Ref] ,[fDesc] ,[Amount] ,[Acct] ,[AcctSub] ,[Status] ,[Sel] ,[VInt] ,[VDoub]
           ,[EN] ,[strRef])
		--SELECT i.Ded,i.Emp,i.BasedOn,i.AccruedOn,i.ByW,i.EmpRate,i.EmpTop,i.EmpGL,i.CompRate,i.CompTop,i.CompGL,i.CompGLE,i.InUse,i.YTD,i.YTDC,d.fDesc,d.Job 
		--,((SELECT SUM(ISNULL(RegAmt,0)) FROM #tempDetailPay WHERE EmpID = i.Emp) * ISNULL(i.CompRate,0))/100
		SELECT @MAXBatch,@CDate,91,0,@PayRegRef,@Name+' '+''+d.fDesc+' -  Week '+@Week, 
			(((ISNULL((SELECT SUM(ISNULL(RegAmt,0)) +SUM(ISNULL(OTAmt,0))+SUM(ISNULL(DTAmt,0))+SUM(ISNULL(TTAmt,0))+
			SUM(ISNULL(NTAmt,0))
			FROM #tempDetailPay WHERE EmpID = i.Emp),0) * ISNULL(i.CompRate,0))/100)
			+ ISNULL(@holiday,0)+ ISNULL(@vacation,0) + ISNULL(@Zone,0) +ISNULL(@reimb,0) +
			ISNULL(@Milage,0) + ISNULL(@bonus,0))
			*-1 ,i.CompGL,NULL,NULL,0,NULL,0,NULL,NULL
		FROM PRDedItem i INNER JOIN PRDed d ON d.ID = i.Ded  	 	 
		WHERE Emp = @ID AND i.ByW = 0 AND i.AccruedOn = 1 AND d.Job = 1 AND (i.CompTop =0 OR i.YTD < i.CompTop)--Paid By-Company and Accoured on Hour AND JobSpecific is true AND (comptop =0 OR YTD < Comptop)
	 END
	 ------------------- Job Specific Comp Dollor Amount------------------



	 ---------------------------- Employee ----------------------------------
	 INSERT INTO Trans ([Batch] ,[fDate] ,[Type] ,[Line] ,[Ref] ,[fDesc] ,[Amount] ,[Acct] ,[AcctSub] ,[Status] ,[Sel] ,[VInt] ,[VDoub]
           ,[EN] ,[strRef])
	 --SELECT i.Ded,i.Emp,i.BasedOn,i.AccruedOn,i.ByW,i.EmpRate,i.EmpTop,i.EmpGL,i.CompRate,i.CompTop,i.CompGL,i.CompGLE,i.InUse,i.YTD,i.YTDC,d.fDesc,d.Job
	 --,(SELECT  SUM(ISNULL(RegQuan,0)) * ISNULL(i.EmpRate,0) FROM #tempDetailPay WHERE EmpID = i.Emp)    
	 SELECT @MAXBatch,@CDate,91,0,@PayRegRef,@Name+' '+''+d.fDesc+' -  Week '+@Week, 
			((ISNULL((SELECT  (SUM(ISNULL(RegQuan,0))+SUM(ISNULL(OTQuan,0))+SUM(ISNULL(DTQuan,0))+SUM(ISNULL(NTQuan,0))+SUM(ISNULL(TTQuan,0)))			
			* ISNULL(i.EmpRate,0) FROM #tempDetailPay WHERE EmpID = i.Emp),0) *-1)
			+ ISNULL(@holiday,0)+ ISNULL(@vacation,0) + ISNULL(@Zone,0) +ISNULL(@reimb,0) +
			ISNULL(@Milage,0) + ISNULL(@bonus,0))
			,i.EmpGL,NULL,NULL,0,NULL,0,NULL,NULL
	 FROM PRDedItem i INNER JOIN PRDed d ON d.ID = i.Ded  
	 WHERE Emp = @ID AND i.ByW = 1 AND i.AccruedOn = 0 AND d.Job = 0 AND (i.EmpTop =0 OR i.YTD < i.EmpTop)--Paid By-Employee and Accoured on Hour AND JobSpecific is false AND (emptop =0 OR YTD < emptop)  
	 ------------------- Job Specific Employee ------------------
	 IF EXISTS ( SELECT 1 FROM JobDed jd WHERE Job in (
			SELECT td.Job FROM TicketD td INNER JOIN tblWork w on td.fWork = w.ID INNER JOIN emp e on e.CallSign = w.fDesc
			WHERE e.ID = @ID AND CAST(EDate AS date) >= CAST(@startdate AS date)             
			AND CAST(EDate AS date) <= CAST(@enddate AS date) ) AND Ded  in (SELECT i.Ded 	 
	 FROM PRDedItem i INNER JOIN PRDed d ON d.ID = i.Ded  	 	 
	 WHERE Emp = @ID AND i.ByW = 1 AND i.AccruedOn = 0 AND d.Job = 1 AND (i.EmpTop =0 OR i.YTD < i.EmpTop)) )
	 BEGIN
	 INSERT INTO Trans ([Batch] ,[fDate] ,[Type] ,[Line] ,[Ref] ,[fDesc] ,[Amount] ,[Acct] ,[AcctSub] ,[Status] ,[Sel] ,[VInt] ,[VDoub]
           ,[EN] ,[strRef])
		--SELECT i.Ded,i.Emp,i.BasedOn,i.AccruedOn,i.ByW,i.EmpRate,i.EmpTop,i.EmpGL,i.CompRate,i.CompTop,i.CompGL,i.CompGLE,i.InUse,i.YTD,i.YTDC,d.fDesc,d.Job 
		--,(SELECT  SUM(ISNULL(RegQuan,0)) * ISNULL(i.EmpRate,0) FROM #tempDetailPay WHERE EmpID = i.Emp)
		SELECT @MAXBatch,@CDate,91,0,@PayRegRef,@Name+' '+''+d.fDesc+' -  Week '+@Week, 
			((ISNULL((SELECT  (SUM(ISNULL(RegQuan,0))+SUM(ISNULL(OTQuan,0))+SUM(ISNULL(DTQuan,0))+SUM(ISNULL(NTQuan,0))+
			SUM(ISNULL(TTQuan,0)))			
			* ISNULL(i.EmpRate,0) FROM #tempDetailPay WHERE EmpID = i.Emp),0) *-1)
			+ ISNULL(@holiday,0)+ ISNULL(@vacation,0) + ISNULL(@Zone,0) +ISNULL(@reimb,0) +
			ISNULL(@Milage,0) + ISNULL(@bonus,0))
			,i.EmpGL,NULL,NULL,0,NULL,0,NULL,NULL
		FROM PRDedItem i INNER JOIN PRDed d ON d.ID = i.Ded  	 	 
		WHERE Emp = @ID AND i.ByW = 1 AND i.AccruedOn = 0 AND d.Job = 1 AND (i.EmpTop =0 OR i.YTD < i.EmpTop)--Paid By-Employee and Accoured on Hour AND JobSpecific is true AND (emptop =0 OR YTD < Emptop)
	 END
	 ------------------- Job Specific Employee ------------------
	  ------------------- Accoured on Dollor Amount Employee ------------------
	  INSERT INTO Trans ([Batch] ,[fDate] ,[Type] ,[Line] ,[Ref] ,[fDesc] ,[Amount] ,[Acct] ,[AcctSub] ,[Status] ,[Sel] ,[VInt] ,[VDoub]
           ,[EN] ,[strRef])
	 --SELECT i.Ded,i.Emp,i.BasedOn,i.AccruedOn,i.ByW,i.EmpRate,i.EmpTop,i.EmpGL,i.CompRate,i.CompTop,i.CompGL,i.CompGLE,i.InUse,i.YTD,i.YTDC,d.fDesc,d.Job 
	 --,((SELECT SUM(ISNULL(RegAmt,0)) FROM #tempDetailPay WHERE EmpID = i.Emp) * ISNULL(i.EmpRate,0))/100
	 SELECT @MAXBatch,@CDate,91,0,@PayRegRef,@Name+' '+''+d.fDesc+' -  Week '+@Week, 
			((ISNULL((SELECT SUM(ISNULL(RegAmt,0)) +SUM(ISNULL(OTAmt,0))+SUM(ISNULL(DTAmt,0))+SUM(ISNULL(TTAmt,0))+SUM(ISNULL(NTAmt,0)) 
			FROM #tempDetailPay WHERE EmpID = i.Emp),0) * ISNULL(i.EmpRate,0))/100) *-1 ,i.EmpGL,NULL,NULL,0,NULL,0,NULL,NULL
	 FROM PRDedItem i INNER JOIN PRDed d ON d.ID = i.Ded  	 	 
	 WHERE Emp = @ID AND i.ByW = 1 AND i.AccruedOn = 1 AND d.Job = 0 AND (i.EmpTop =0 OR i.YTD < i.EmpTop)--Paid By-Employee and Accoured on Dollor Amount AND JobSpecific is false AND (emptop =0 OR YTD < emptop)
	 ------------------- Accoured on Dollor Amount Employee ------------------
	 ------------------- Job Specific Emp Dollor Amount------------------
	 IF EXISTS ( SELECT 1 FROM JobDed jd WHERE Job in (
			SELECT td.Job FROM TicketD td INNER JOIN tblWork w on td.fWork = w.ID INNER JOIN emp e on e.CallSign = w.fDesc
			WHERE e.ID = @ID AND CAST(EDate AS date) >= CAST(@startdate AS date)             
			AND CAST(EDate AS date) <= CAST(@enddate AS date) ) AND Ded  in (SELECT i.Ded 	 
	 FROM PRDedItem i INNER JOIN PRDed d ON d.ID = i.Ded  	 	 
	 WHERE Emp = @ID AND i.ByW = 1 AND i.AccruedOn = 1 AND d.Job = 1 AND (i.CompTop =0 OR i.YTD < i.CompTop)) )
	 BEGIN
	 INSERT INTO Trans ([Batch] ,[fDate] ,[Type] ,[Line] ,[Ref] ,[fDesc] ,[Amount] ,[Acct] ,[AcctSub] ,[Status] ,[Sel] ,[VInt] ,[VDoub]
           ,[EN] ,[strRef])
		--SELECT i.Ded,i.Emp,i.BasedOn,i.AccruedOn,i.ByW,i.EmpRate,i.EmpTop,i.EmpGL,i.CompRate,i.CompTop,i.CompGL,i.CompGLE,i.InUse,i.YTD,i.YTDC,d.fDesc,d.Job 
		--,((SELECT SUM(ISNULL(RegAmt,0)) FROM #tempDetailPay WHERE EmpID = i.Emp) * ISNULL(i.EmpRate,0))/100
		SELECT @MAXBatch,@CDate,91,0,@PayRegRef,@Name+' '+''+d.fDesc+' -  Week '+@Week, 
			(((ISNULL((SELECT SUM(ISNULL(RegAmt,0)) +SUM(ISNULL(OTAmt,0))+SUM(ISNULL(DTAmt,0))+SUM(ISNULL(TTAmt,0))+SUM(ISNULL(NTAmt,0))
			FROM #tempDetailPay WHERE EmpID = i.Emp),0) * ISNULL(i.EmpRate,0))/100)
			+ ISNULL(@holiday,0)+ ISNULL(@vacation,0) + ISNULL(@Zone,0) +ISNULL(@reimb,0) +
			ISNULL(@Milage,0) + ISNULL(@bonus,0))
			*-1 ,i.EmpGL,NULL,NULL,0,NULL,0,NULL,NULL
		FROM PRDedItem i INNER JOIN PRDed d ON d.ID = i.Ded  	 	 
		WHERE Emp = @ID AND i.ByW = 1 AND i.AccruedOn = 1 AND d.Job = 1 AND (i.CompTop =0 OR i.YTD < i.CompTop)--Paid By-Employee and Accoured on Dollor Amount AND JobSpecific is true AND (EmpTop =0 OR YTD < EmpTop)
	 END
	 ------------------- Job Specific Emp Dollor Amount------------------          
--   ---------------- DEDUCTION -------------------        
--   DECLARE db_cursor2011 CURSOR FOR           
--        SELECT i.Ded,i.Emp,i.BasedOn,i.AccruedOn,i.ByW,i.EmpRate,i.EmpTop,i.EmpGL,i.CompRate,i.CompTop,i.CompGL,i.CompGLE,i.InUse,i.YTD,i.YTDC,d.fDesc,d.Job FROM PRDedItem i INNER JOIN PRDed d ON d.ID = i.Ded  WHERE Emp = @ID        
		
          
-- OPEN db_cursor2011            
-- FETCH NEXT FROM db_cursor2011 INTO           
--   @Ded_Ded,@Ded_Emp,@Ded_BasedOn,@Ded_AccuredOn,@Ded_ByW,@Ded_EmpRate,@Ded_EmpTop,@Ded_EmpGL,@Ded_CompRate,@Ded_CompTop,@Ded_CompGL,@Ded_CompGLE,@Ded_InUse,@Ded_YTD,@Ded_YTDC,@Ded_fDesc,@Ded_JobSpecific          
            
-- WHILE @@FETCH_STATUS = 0          
-- BEGIN             
--  IF @Ded_ByW = 0 --Paid By-Company        
--  BEGIN        
-- IF @Ded_AccuredOn = 0 -- Accoured on Hour        
-- BEGIN
--	IF @Ded_JobSpecific = 0 
--	BEGIN
--		IF @Ded_CompTop =0 OR @Ded_YTD < @Ded_CompTop
--		BEGIN
--			--SELECT @Ded_DeductionAmount = SUM(ISNULL(RegQuan,0)) * ISNULL(@Ded_CompRate,0) FROM #tempDetailPay  WHERE EmpID = @ID
--			SELECT @Ded_DeductionAmount = (SELECT SUM(ISNULL(RegQuan,0)) FROM #tempDetailPay WHERE EmpID = @ID) * ISNULL(@Ded_CompRate,0)
--			SET @TransDesc = @Name+' '+'Contribution '+@Ded_fDesc+' -  Week '+@Week   
--			IF @Ded_DeductionAmount IS NULL
--			BEGIN 
--				SET @Ded_DeductionAmount = ISNULL(@Ded_DeductionAmount,0)
--			END
--			EXEC @TransId = [dbo].[AddJournal] null,@MAXBatch,@CDate,91,0,@PayRegRef,@TransDesc,@Ded_DeductionAmount ,@Ded_CompGLE,NULL,NULL,0,NULL,0,NULL,NULL           
			
--			SET @TransDesc = @Name+' '+@Ded_fDesc+' -  Week '+@Week         
--			SET @Ded_DeductionAmount = (@Ded_DeductionAmount)*-1 
--			IF @Ded_DeductionAmount IS NULL
--			BEGIN 
--				SET @Ded_DeductionAmount = ISNULL(@Ded_DeductionAmount,0)
--			END
--			EXEC @TransId = [dbo].[AddJournal] null,@MAXBatch,@CDate,91,0,@PayRegRef,@TransDesc,@Ded_DeductionAmount ,@Ded_CompGL,NULL,NULL,0,NULL,0,NULL,NULL           
			
--		END
--     END
--	 ELSE 
--	 BEGIN
--		IF EXISTS ( SELECT * FROM JobDed WHERE Job in (
--			SELECT td.Job FROM TicketD td INNER JOIN tblWork w on td.fWork = w.ID INNER JOIN emp e on e.CallSign = w.fDesc
--			WHERE e.ID = @ID AND CAST(EDate AS date) >= CAST(@startdate AS date)             
--			AND CAST(EDate AS date) <= CAST(@enddate AS date) ) AND Ded = @Ded_Ded)
--		BEGIN
--			IF @Ded_CompTop =0 OR @Ded_YTD < @Ded_CompTop
--			BEGIN
--				SELECT @Ded_DeductionAmount = (SELECT SUM(ISNULL(RegQuan,0)) FROM #tempDetailPay WHERE EmpID = @ID) * ISNULL(@Ded_CompRate,0)
--				SET @TransDesc = @Name+' '+'Contribution '+@Ded_fDesc+' -  Week '+@Week  
--				IF @Ded_DeductionAmount IS NULL
--				BEGIN 
--					SET @Ded_DeductionAmount = ISNULL(@Ded_DeductionAmount,0)
--				END
--				EXEC @TransId = [dbo].[AddJournal] null,@MAXBatch,@CDate,91,0,@PayRegRef,@TransDesc,@Ded_DeductionAmount ,@Ded_CompGLE,NULL,NULL,0,NULL,0,NULL,NULL           
				
--				SET @TransDesc = @Name+' '+@Ded_fDesc+' -  Week '+@Week         
--				SET @Ded_DeductionAmount = (@Ded_DeductionAmount)*-1   
--				IF @Ded_DeductionAmount IS NULL
--				BEGIN 
--					SET @Ded_DeductionAmount = ISNULL(@Ded_DeductionAmount,0)
--				END
--				EXEC @TransId = [dbo].[AddJournal] null,@MAXBatch,@CDate,91,0,@PayRegRef,@TransDesc,@Ded_DeductionAmount ,@Ded_CompGL,NULL,NULL,0,NULL,0,NULL,NULL           
				
--			END
--		END
--	 END
-- END        
-- ELSE IF @Ded_AccuredOn = 1 -- Accoured on Dollor Amount        
-- BEGIN        
--	IF @Ded_JobSpecific = 0 
--	BEGIN
--		IF @Ded_CompTop =0 OR @Ded_YTD < @Ded_CompTop
--		BEGIN
--			--SELECT @Ded_DeductionAmount = (SUM(ISNULL(RegRate,0)) * ISNULL(@Ded_CompRate,0))/100 FROM #tempDetailPay  WHERE EmpID = @ID 
--			SELECT @Ded_DeductionAmount = ((SELECT SUM(ISNULL(RegAmt,0)) FROM #tempDetailPay WHERE EmpID = @ID) * ISNULL(@Ded_CompRate,0))/100
--			SET @TransDesc = @Name+' '+'Contribution '+@Ded_fDesc+' -  Week '+@Week   
--			IF @Ded_DeductionAmount IS NULL
--			BEGIN 
--				SET @Ded_DeductionAmount = ISNULL(@Ded_DeductionAmount,0)
--			END
--			EXEC @TransId = [dbo].[AddJournal] null,@MAXBatch,@CDate,91,0,@PayRegRef,@TransDesc,@Ded_DeductionAmount ,@Ded_CompGLE,NULL,NULL,0,NULL,0,NULL,NULL           
--			--INSERT INTO Trans ([ID] ,[Batch] ,[fDate] ,[Type] ,[Line] ,[Ref] ,[fDesc] ,[Amount] ,[Acct] ,[AcctSub] ,[Status] ,[Sel] ,[VInt] ,[VDoub]
--			--,[EN] ,[strRef])
--			--SELECT (ISNULL((SELECT MAX(ISNULL(ID,0)+1) FROM Trans),1)),@MAXBatch,@CDate,91,0,@PayRegRef,@TransDesc, 
--			--@Ded_DeductionAmount ,@Ded_CompGLE,NULL,NULL,0,NULL,0,NULL,NULL
--			SET @Ded_DeductionAmount = (@Ded_DeductionAmount)*-1        
--			SET @TransDesc = @Name+' '+@Ded_fDesc+' -  Week '+@Week   
--			IF @Ded_DeductionAmount IS NULL
--			BEGIN 
--				SET @Ded_DeductionAmount = ISNULL(@Ded_DeductionAmount,0)
--			END
--			EXEC @TransId = [dbo].[AddJournal] null,@MAXBatch,@CDate,91,0,@PayRegRef,@TransDesc,@Ded_DeductionAmount ,@Ded_CompGL,NULL,NULL,0,NULL,0,NULL,NULL           
--			--INSERT INTO Trans ([ID] ,[Batch] ,[fDate] ,[Type] ,[Line] ,[Ref] ,[fDesc] ,[Amount] ,[Acct] ,[AcctSub] ,[Status] ,[Sel] ,[VInt] ,[VDoub]
--			--,[EN] ,[strRef])
--			--SELECT (ISNULL((SELECT MAX(ISNULL(ID,0)+1) FROM Trans),1)),@MAXBatch,@CDate,91,0,@PayRegRef,@TransDesc, 
--			--@Ded_DeductionAmount ,@Ded_CompGL,NULL,NULL,0,NULL,0,NULL,NULL
--		END
--    END
--	ELSE 
--	 BEGIN
--		IF EXISTS ( SELECT * FROM JobDed WHERE Job in (
--			SELECT td.Job FROM TicketD td INNER JOIN tblWork w on td.fWork = w.ID INNER JOIN emp e on e.CallSign = w.fDesc
--			WHERE e.ID = @ID AND CAST(EDate AS date) >= CAST(@startdate AS date)             
--			AND CAST(EDate AS date) <= CAST(@enddate AS date) ) AND Ded = @Ded_Ded)
--		BEGIN
--			IF @Ded_CompTop =0 OR @Ded_YTD < @Ded_CompTop
--			BEGIN
--				--SELECT @Ded_DeductionAmount = (SUM(ISNULL(RegRate,0)) * ISNULL(@Ded_CompRate,0))/100 FROM #tempDetailPay  WHERE EmpID = @ID 
--				SELECT @Ded_DeductionAmount = ((SELECT SUM(ISNULL(RegAmt,0)) FROM #tempDetailPay WHERE EmpID = @ID) * ISNULL(@Ded_CompRate,0))/100
--				SET @TransDesc = @Name+' '+'Contribution '+@Ded_fDesc+' -  Week '+@Week  
--				IF @Ded_DeductionAmount IS NULL
--				BEGIN 
--					SET @Ded_DeductionAmount = ISNULL(@Ded_DeductionAmount,0)
--				END
--				EXEC @TransId = [dbo].[AddJournal] null,@MAXBatch,@CDate,91,0,@PayRegRef,@TransDesc,@Ded_DeductionAmount ,@Ded_CompGLE,NULL,NULL,0,NULL,0,NULL,NULL           
--				--INSERT INTO Trans ([ID] ,[Batch] ,[fDate] ,[Type] ,[Line] ,[Ref] ,[fDesc] ,[Amount] ,[Acct] ,[AcctSub] ,[Status] ,[Sel] ,[VInt] ,[VDoub]
--				--,[EN] ,[strRef])
--				--SELECT (ISNULL((SELECT MAX(ISNULL(ID,0)+1) FROM Trans),1)),@MAXBatch,@CDate,91,0,@PayRegRef,@TransDesc, 
--				--@Ded_DeductionAmount ,@Ded_CompGLE,NULL,NULL,0,NULL,0,NULL,NULL
--				SET @Ded_DeductionAmount = (@Ded_DeductionAmount)*-1        
--				SET @TransDesc = @Name+' '+@Ded_fDesc+' -  Week '+@Week  
--				IF @Ded_DeductionAmount IS NULL
--				BEGIN 
--					SET @Ded_DeductionAmount = ISNULL(@Ded_DeductionAmount,0)
--				END
--				EXEC @TransId = [dbo].[AddJournal] null,@MAXBatch,@CDate,91,0,@PayRegRef,@TransDesc,@Ded_DeductionAmount ,@Ded_CompGL,NULL,NULL,0,NULL,0,NULL,NULL           
--				--INSERT INTO Trans ([ID] ,[Batch] ,[fDate] ,[Type] ,[Line] ,[Ref] ,[fDesc] ,[Amount] ,[Acct] ,[AcctSub] ,[Status] ,[Sel] ,[VInt] ,[VDoub]
--				--,[EN] ,[strRef])
--				--SELECT (ISNULL((SELECT MAX(ISNULL(ID,0)+1) FROM Trans),1)),@MAXBatch,@CDate,91,0,@PayRegRef,@TransDesc, 
--				--@Ded_DeductionAmount ,@Ded_CompGL,NULL,NULL,0,NULL,0,NULL,NULL
--			END
--		END
--	 END
-- END        
         
--  END        
--  IF @Ded_ByW = 1 --Paid By-Employee        
--  BEGIN        
-- IF @Ded_AccuredOn = 0 -- Accoured on Hour        
-- BEGIN 
--	IF @Ded_JobSpecific = 0 
--	BEGIN
--		IF @Ded_EmpTop =0 OR @Ded_YTD < @Ded_EmpTop
--		BEGIN
--			SELECT @Ded_DeductionAmount = SUM(ISNULL(RegQuan,0)) * ISNULL(@Ded_EmpRate,0) FROM #tempDetailPay WHERE EmpID = @ID       
--			SET @TransDesc = @Name+' '+@Ded_fDesc+' -  Week '+@Week         
--			SET @Ded_DeductionAmount = (@Ded_DeductionAmount)*-1   
--			IF @Ded_DeductionAmount IS NULL
--			BEGIN 
--				SET @Ded_DeductionAmount = ISNULL(@Ded_DeductionAmount,0)
--			END
--			EXEC @TransId = [dbo].[AddJournal] null,@MAXBatch,@CDate,91,0,@PayRegRef,@TransDesc,@Ded_DeductionAmount ,@Ded_EmpGL,NULL,NULL,0,NULL,0,NULL,NULL           
--			--INSERT INTO Trans ([ID] ,[Batch] ,[fDate] ,[Type] ,[Line] ,[Ref] ,[fDesc] ,[Amount] ,[Acct] ,[AcctSub] ,[Status] ,[Sel] ,[VInt] ,[VDoub]
--			--,[EN] ,[strRef])
--			--SELECT (ISNULL((SELECT MAX(ISNULL(ID,0)+1) FROM Trans),1)),@MAXBatch,@CDate,91,0,@PayRegRef,@TransDesc, 
--			--@Ded_DeductionAmount ,@Ded_EmpGL,NULL,NULL,0,NULL,0,NULL,NULL
--		END
--	END
--	ELSE 
--	 BEGIN
--		IF EXISTS ( SELECT * FROM JobDed WHERE Job in (
--			SELECT td.Job FROM TicketD td INNER JOIN tblWork w on td.fWork = w.ID INNER JOIN emp e on e.CallSign = w.fDesc
--			WHERE e.ID = @ID AND CAST(EDate AS date) >= CAST(@startdate AS date)             
--			AND CAST(EDate AS date) <= CAST(@enddate AS date) ) AND Ded = @Ded_Ded)
--		BEGIN
--			IF @Ded_EmpTop =0 OR @Ded_YTD < @Ded_EmpTop
--			BEGIN
--				SELECT @Ded_DeductionAmount = SUM(ISNULL(RegQuan,0)) * ISNULL(@Ded_EmpRate,0) FROM #tempDetailPay WHERE EmpID = @ID       
--				SET @TransDesc = @Name+' '+@Ded_fDesc+' -  Week '+@Week         
--				SET @Ded_DeductionAmount = (@Ded_DeductionAmount)*-1 
--				IF @Ded_DeductionAmount IS NULL
--				BEGIN 
--					SET @Ded_DeductionAmount = ISNULL(@Ded_DeductionAmount,0)
--				END
--				EXEC @TransId = [dbo].[AddJournal] null,@MAXBatch,@CDate,91,0,@PayRegRef,@TransDesc,@Ded_DeductionAmount ,@Ded_EmpGL,NULL,NULL,0,NULL,0,NULL,NULL           
--				--INSERT INTO Trans ([ID] ,[Batch] ,[fDate] ,[Type] ,[Line] ,[Ref] ,[fDesc] ,[Amount] ,[Acct] ,[AcctSub] ,[Status] ,[Sel] ,[VInt] ,[VDoub]
--				--,[EN] ,[strRef])
--				--SELECT (ISNULL((SELECT MAX(ISNULL(ID,0)+1) FROM Trans),1)),@MAXBatch,@CDate,91,0,@PayRegRef,@TransDesc, 
--				--@Ded_DeductionAmount ,@Ded_EmpGL,NULL,NULL,0,NULL,0,NULL,NULL
--			END
--		END
--	 END
-- END        
-- ELSE IF @Ded_AccuredOn = 1 -- Accoured on Dollor Amount        
-- BEGIN 
--	IF @Ded_JobSpecific = 0 
--	BEGIN
--		IF @Ded_EmpTop =0 OR @Ded_YTD < @Ded_EmpTop
--		BEGIN
--			--SELECT @Ded_DeductionAmount = (SUM(ISNULL(RegRate,0)) * ISNULL(@Ded_EmpRate,0))/100 FROM #tempDetailPay WHERE EmpID = @ID        
--			SELECT @Ded_DeductionAmount = ((SELECT SUM(ISNULL(RegAmt,0)) FROM #tempDetailPay WHERE EmpID = @ID)* ISNULL(@Ded_EmpRate,0))/100
--			SET @TransDesc = @Name+' '+@Ded_fDesc+' -  Week '+@Week         
--			SET @Ded_DeductionAmount = (@Ded_DeductionAmount)*-1  
--			IF @Ded_DeductionAmount IS NULL
--			BEGIN 
--				SET @Ded_DeductionAmount = ISNULL(@Ded_DeductionAmount,0)
--			END
--			EXEC @TransId = [dbo].[AddJournal] null,@MAXBatch,@CDate,91,0,@PayRegRef,@TransDesc,@Ded_DeductionAmount ,@Ded_EmpGL,NULL,NULL,0,NULL,0,NULL,NULL           
--			--INSERT INTO Trans ([ID] ,[Batch] ,[fDate] ,[Type] ,[Line] ,[Ref] ,[fDesc] ,[Amount] ,[Acct] ,[AcctSub] ,[Status] ,[Sel] ,[VInt] ,[VDoub]
--			--,[EN] ,[strRef])
--			--SELECT (ISNULL((SELECT MAX(ISNULL(ID,0)+1) FROM Trans),1)),@MAXBatch,@CDate,91,0,@PayRegRef,@TransDesc, 
--			--@Ded_DeductionAmount ,@Ded_EmpGL,NULL,NULL,0,NULL,0,NULL,NULL
--		END
--    END
--	ELSE 
--	 BEGIN
--		IF EXISTS ( SELECT * FROM JobDed WHERE Job in (
--			SELECT td.Job FROM TicketD td INNER JOIN tblWork w on td.fWork = w.ID INNER JOIN emp e on e.CallSign = w.fDesc
--			WHERE e.ID = @ID AND CAST(EDate AS date) >= CAST(@startdate AS date)             
--			AND CAST(EDate AS date) <= CAST(@enddate AS date) ) AND Ded = @Ded_Ded)
--		BEGIN
--			IF @Ded_EmpTop =0 OR @Ded_YTD < @Ded_EmpTop
--			BEGIN
--				--SELECT @Ded_DeductionAmount = (SUM(ISNULL(RegRate,0)) * ISNULL(@Ded_EmpRate,0))/100 FROM #tempDetailPay WHERE EmpID = @ID        
--				SELECT @Ded_DeductionAmount = ((SELECT SUM(ISNULL(RegAmt,0)) FROM #tempDetailPay WHERE EmpID = @ID)* ISNULL(@Ded_EmpRate,0))/100
--				SET @TransDesc = @Name+' '+@Ded_fDesc+' -  Week '+@Week         
--				SET @Ded_DeductionAmount = (@Ded_DeductionAmount)*-1
--				IF @Ded_DeductionAmount IS NULL
--				BEGIN 
--					SET @Ded_DeductionAmount = ISNULL(@Ded_DeductionAmount,0)
--				END
--				EXEC @TransId = [dbo].[AddJournal] null,@MAXBatch,@CDate,91,0,@PayRegRef,@TransDesc,@Ded_DeductionAmount ,@Ded_EmpGL,NULL,NULL,0,NULL,0,NULL,NULL           
--				--INSERT INTO Trans ([ID] ,[Batch] ,[fDate] ,[Type] ,[Line] ,[Ref] ,[fDesc] ,[Amount] ,[Acct] ,[AcctSub] ,[Status] ,[Sel] ,[VInt] ,[VDoub]
--				--,[EN] ,[strRef])
--				--SELECT (ISNULL((SELECT MAX(ISNULL(ID,0)+1) FROM Trans),1)),@MAXBatch,@CDate,91,0,@PayRegRef,@TransDesc, 
--				--@Ded_DeductionAmount ,@Ded_EmpGL,NULL,NULL,0,NULL,0,NULL,NULL
--			END
--		END
--	 END
-- END        
--  END        
        
-- --------------    
-- --UPDATE TRANS SET Amount = (SELECT SUM(AMOUNT)*-1 FROM Trans WHERE Batch = @MAXBatch AND Type = 91) WHERE Batch = @MAXBatch AND Type = 90    
-- --UPDATE PRReg SET Net = (SELECT AMOUNT*-1 FROM Trans WHERE Batch = @MAXBatch AND Type = 90) WHERE ID = @PayRegMaxID    
     
-- --------------    
     
--  --------RESET------->          
--   SET @Ded_Ded= NULL ;SET @Ded_Emp = NULL ;SET @Ded_BasedOn = NULL ;SET @Ded_AccuredOn = NULL ;SET @Ded_ByW = NULL ;SET @Ded_EmpRate = NULL ;SET @Ded_EmpTop = NULL ;        
--   SET @Ded_EmpGL = NULL ;SET @Ded_CompRate = NULL ;SET @Ded_CompTop = NULL ;SET @Ded_CompGL = NULL ;SET @Ded_CompGLE = NULL ;SET @Ded_InUse = NULL ;        
--   SET @Ded_YTD = NULL ;SET @Ded_YTDC  = NULL ;SET @Ded_fDesc = NULL;        
--   --------------->         
        
--  FETCH NEXT FROM db_cursor2011 INTO           
--   @Ded_Ded,@Ded_Emp,@Ded_BasedOn,@Ded_AccuredOn,@Ded_ByW,@Ded_EmpRate,@Ded_EmpTop,@Ded_EmpGL,@Ded_CompRate,@Ded_CompTop,@Ded_CompGL,@Ded_CompGLE,@Ded_InUse,@Ded_YTD,@Ded_YTDC,@Ded_fDesc,@Ded_JobSpecific        
--END            
          
          
--    CLOSE db_cursor2011            
-- DEALLOCATE db_cursor2011          
--  ----------------- DEDUCTION ----------------------        
          
    
	SELECT @NetTotal =SUM(AMOUNT) FROM Trans WHERE Batch = @MAXBatch
    UPDATE Trans SET Amount = @NetTotal*-1 FROM Trans WHERE Batch = @MAXBatch AND Type = 90
	Update PRReg SET Net = @NetTotal WHERE ID = @PayRegMaxID
    
    
  --------RESET------->          
   SET  @ID= NULL ;  SET  @Name= NULL ;  SET  @Reg= NULL ;  SET  @OT= NULL ;  SET  @DT= NULL ;  SET  @TT= NULL ;  SET  @NT= NULL ;  SET  @Zone= NULL ;  SET  @Milage= NULL ;  SET  @Toll= NULL ;  SET  @OtherE= NULL ;  SET  @pay= NULL;           
   SET @holiday= NULL; SET @vacation= NULL; SET @sicktime= NULL;set @reimb=null; set @bonus=null; set @paymethod=null; SET @pmethod =null; SET @userid=null;SET @usertype=null; SET @total=null; SET @phour=null;  SET @salary = NULL; SET @HourlyRate=null;   
 
    
       
       
   SET @PayRegRef= NULL; SET @TransId= NULL; SET @PayRegRef= NULL; SET @NetTotal =NULL;        
   --------------->          
   --TRUNCATE TABLE   #tempDetailPay     
 FETCH NEXT FROM db_cursor1 INTO           
   @ID,@Name,@Reg,@OT,@DT,@TT,@NT,@Zone,@Milage,@Toll,@OtherE,@pay,@holiday,@vacation,@sicktime,@reimb,@bonus,@paymethod,@pmethod,@userid,@usertype,@total,@phour,@salary,@HourlyRate,@FIT,@SIT,@LOCAL,@MEDI,@FICA          
 END            
          
          
    CLOSE db_cursor1            
 DEALLOCATE db_cursor1          
          
 --------------------------          
          
          
 COMMIT           
           
 END TRY          
          
 BEGIN CATCH          
          
 SELECT ERROR_MESSAGE()          
 DECLARE @error varchar(1000)=(SELECT ERROR_MESSAGE())          
    IF @@TRANCOUNT>0          
        ROLLBACK           
  RAISERROR ( @error,16,1)          
        RETURN           
 END CATCH           
 RETURN 0    
          
END 