-- uspDedcutionEmpDetail '2020-03-06','2020-03-12',248
CREATE PROCEDURE [dbo].[uspDedcutionEmpDetail]          
 (    
 @startdate datetime,
 @enddate datetime,          
 @ID int ,
 @HolidayAm decimal(17,2),
 @VacAm decimal(17,2),
 @ZoneAm decimal(17,2),
 @ReimbAm decimal(17,2),
 @MilageAm decimal(17,2),
 @BonusAm decimal(17,2)         
     )     
AS          
BEGIN          
     SET NOCount ON;     
DECLARE @Name varchar(max) ;          
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

 --------------------------          
 CREATE TABLE #tempDeduction
 (
	ID int null,            
	fDesc varchar(max) null,            
	Amount numeric(30,2) null  ,
	PaidBy int Null --0-->Company,1-->Emp
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
 
 
 -------------------------------
SET @HolidayAm = (SELECT ISNULL(Rate,0) FROM PROther WHERE Emp = @ID AND Cat = 1)*@HolidayAm
SET @VacAm = (SELECT ISNULL(Rate,0) FROM PROther WHERE Emp = @ID AND Cat = 2)*@VacAm
 -------------------------------

   ---------------- DEDUCTION -------------------        
   DECLARE db_cursor2011 CURSOR FOR           
        SELECT i.Ded,i.Emp,i.BasedOn,i.AccruedOn,i.ByW,i.EmpRate,i.EmpTop,i.EmpGL,i.CompRate,i.CompTop,i.CompGL,i.CompGLE,i.InUse,i.YTD,i.YTDC,d.fDesc,d.Job FROM PRDedItem i INNER JOIN PRDed d ON d.ID = i.Ded  WHERE Emp = @ID        
		
          
 OPEN db_cursor2011            
 FETCH NEXT FROM db_cursor2011 INTO           
   @Ded_Ded,@Ded_Emp,@Ded_BasedOn,@Ded_AccuredOn,@Ded_ByW,@Ded_EmpRate,@Ded_EmpTop,@Ded_EmpGL,@Ded_CompRate,@Ded_CompTop,@Ded_CompGL,@Ded_CompGLE,@Ded_InUse,@Ded_YTD,@Ded_YTDC,@Ded_fDesc,@Ded_JobSpecific          
            
 WHILE @@FETCH_STATUS = 0          
 BEGIN             
  IF @Ded_ByW = 0 --Paid By-Company        
  BEGIN        
 IF @Ded_AccuredOn = 0 -- Accoured on Hour        
 BEGIN
	IF @Ded_JobSpecific = 0 
	BEGIN
		IF @Ded_CompTop =0 OR @Ded_YTD < @Ded_CompTop
		BEGIN
			SELECT @Ded_DeductionAmount = ((SELECT SUM(ISNULL(RegQuan,0)) 
			+SUM(ISNULL(OTQuan,0))+SUM(ISNULL(DTQuan,0))+SUM(ISNULL(NTQuan,0))+SUM(ISNULL(TTQuan,0))			
			FROM #tempDetailPay WHERE EmpID = @ID) * ISNULL(@Ded_CompRate,0))
			+ ISNULL(@HolidayAm,0)+ ISNULL(@VacAm,0) + ISNULL(@ZoneAm,0) +ISNULL(@ReimbAm,0) +
			ISNULL(@MilageAm,0) + ISNULL(@BonusAm,0)
			IF @Ded_DeductionAmount IS NULL
			BEGIN 
				SET @Ded_DeductionAmount = ISNULL(@Ded_DeductionAmount,0)
			END
			INSERT INTO #tempDeduction (ID,fDesc,Amount,PaidBy ) VALUES (@Ded_Ded,@Ded_fDesc,@Ded_DeductionAmount,@Ded_ByW)
		END
     END
	 ELSE 
	 BEGIN
		IF EXISTS ( SELECT * FROM JobDed WHERE Job in (
			SELECT td.Job FROM TicketD td INNER JOIN tblWork w on td.fWork = w.ID INNER JOIN emp e on e.CallSign = w.fDesc
			WHERE e.ID = @ID AND CAST(EDate AS date) >= CAST(@startdate AS date)             
			AND CAST(EDate AS date) <= CAST(@enddate AS date) ) AND Ded = @Ded_Ded)
		BEGIN
			IF @Ded_CompTop =0 OR @Ded_YTD < @Ded_CompTop
			BEGIN
				SELECT @Ded_DeductionAmount = ((SELECT SUM(ISNULL(RegQuan,0))
				+SUM(ISNULL(OTQuan,0))+SUM(ISNULL(DTQuan,0))+SUM(ISNULL(NTQuan,0))+SUM(ISNULL(TTQuan,0))				
				FROM #tempDetailPay WHERE EmpID = @ID) * ISNULL(@Ded_CompRate,0))
				+ ISNULL(@HolidayAm,0)+ ISNULL(@VacAm,0) + ISNULL(@ZoneAm,0) +ISNULL(@ReimbAm,0) +
			ISNULL(@MilageAm,0) + ISNULL(@BonusAm,0)
				IF @Ded_DeductionAmount IS NULL
				BEGIN 
					SET @Ded_DeductionAmount = ISNULL(@Ded_DeductionAmount,0)
				END
				INSERT INTO #tempDeduction (ID,fDesc,Amount ,PaidBy) VALUES (@Ded_Ded,@Ded_fDesc,@Ded_DeductionAmount,@Ded_ByW)				
				
			END
		END
	 END
 END        
 ELSE IF @Ded_AccuredOn = 1 -- Accoured on Dollor Amount        
 BEGIN        
	IF @Ded_JobSpecific = 0 
	BEGIN
		IF @Ded_CompTop =0 OR @Ded_YTD < @Ded_CompTop
		BEGIN
			SELECT @Ded_DeductionAmount = (((SELECT SUM(ISNULL(RegAmt,0))
			+SUM(ISNULL(OTAmt,0))+SUM(ISNULL(DTAmt,0))+SUM(ISNULL(TTAmt,0))+SUM(ISNULL(NTAmt,0))
			FROM #tempDetailPay WHERE EmpID = @ID) * ISNULL(@Ded_CompRate,0))/100)+
			+ ISNULL(@HolidayAm,0)+ ISNULL(@VacAm,0) + ISNULL(@ZoneAm,0) +ISNULL(@ReimbAm,0) +
			ISNULL(@MilageAm,0) + ISNULL(@BonusAm,0)
			IF @Ded_DeductionAmount IS NULL
			BEGIN 
				SET @Ded_DeductionAmount = ISNULL(@Ded_DeductionAmount,0)
			END
			INSERT INTO #tempDeduction (ID,fDesc,Amount,PaidBy ) VALUES (@Ded_Ded,@Ded_fDesc,@Ded_DeductionAmount,@Ded_ByW)

		END
    END
	ELSE 
	 BEGIN
		IF EXISTS ( SELECT * FROM JobDed WHERE Job in (
			SELECT td.Job FROM TicketD td INNER JOIN tblWork w on td.fWork = w.ID INNER JOIN emp e on e.CallSign = w.fDesc
			WHERE e.ID = @ID AND CAST(EDate AS date) >= CAST(@startdate AS date)             
			AND CAST(EDate AS date) <= CAST(@enddate AS date) ) AND Ded = @Ded_Ded)
		BEGIN
			IF @Ded_CompTop =0 OR @Ded_YTD < @Ded_CompTop
			BEGIN
				SELECT @Ded_DeductionAmount = (((SELECT SUM(ISNULL(RegAmt,0))
				+SUM(ISNULL(OTAmt,0))+SUM(ISNULL(DTAmt,0))+SUM(ISNULL(TTAmt,0))+SUM(ISNULL(NTAmt,0))
				FROM #tempDetailPay WHERE EmpID = @ID) * ISNULL(@Ded_CompRate,0))/100)
				+ ISNULL(@HolidayAm,0)+ ISNULL(@VacAm,0) + ISNULL(@ZoneAm,0) +ISNULL(@ReimbAm,0) +
			ISNULL(@MilageAm,0) + ISNULL(@BonusAm,0)
				IF @Ded_DeductionAmount IS NULL
				BEGIN 
					SET @Ded_DeductionAmount = ISNULL(@Ded_DeductionAmount,0)
				END
				INSERT INTO #tempDeduction (ID,fDesc,Amount ,PaidBy) VALUES (@Ded_Ded,@Ded_fDesc,@Ded_DeductionAmount,@Ded_ByW)

			END
		END
	 END
 END        
         
  END        
  IF @Ded_ByW = 1 --Paid By-Employee        
  BEGIN        
 IF @Ded_AccuredOn = 0 -- Accoured on Hour        
 BEGIN 
	IF @Ded_JobSpecific = 0 
	BEGIN
		IF @Ded_EmpTop =0 OR @Ded_YTD < @Ded_EmpTop
		BEGIN
			SELECT @Ded_DeductionAmount = ((SUM(ISNULL(RegQuan,0))
			+SUM(ISNULL(OTQuan,0))+SUM(ISNULL(DTQuan,0))+SUM(ISNULL(NTQuan,0))+SUM(ISNULL(TTQuan,0)))
			* ISNULL(@Ded_EmpRate,0))
			+ ISNULL(@HolidayAm,0)+ ISNULL(@VacAm,0) + ISNULL(@ZoneAm,0) +ISNULL(@ReimbAm,0) +
			ISNULL(@MilageAm,0) + ISNULL(@BonusAm,0)
			FROM #tempDetailPay WHERE EmpID = @ID       
			IF @Ded_DeductionAmount IS NULL
			BEGIN 
				SET @Ded_DeductionAmount = ISNULL(@Ded_DeductionAmount,0)
			END
			INSERT INTO #tempDeduction (ID,fDesc,Amount,PaidBy ) VALUES (@Ded_Ded,@Ded_fDesc,@Ded_DeductionAmount,@Ded_ByW)

		END
	END
	ELSE 
	 BEGIN
		IF EXISTS ( SELECT * FROM JobDed WHERE Job in (
			SELECT td.Job FROM TicketD td INNER JOIN tblWork w on td.fWork = w.ID INNER JOIN emp e on e.CallSign = w.fDesc
			WHERE e.ID = @ID AND CAST(EDate AS date) >= CAST(@startdate AS date)             
			AND CAST(EDate AS date) <= CAST(@enddate AS date) ) AND Ded = @Ded_Ded)
		BEGIN
			IF @Ded_EmpTop =0 OR @Ded_YTD < @Ded_EmpTop
			BEGIN
				SELECT @Ded_DeductionAmount = ((SUM(ISNULL(RegQuan,0))
				+SUM(ISNULL(OTQuan,0))+SUM(ISNULL(DTQuan,0))+SUM(ISNULL(NTQuan,0))+SUM(ISNULL(TTQuan,0)))
				* ISNULL(@Ded_EmpRate,0))
				+ ISNULL(@HolidayAm,0)+ ISNULL(@VacAm,0) + ISNULL(@ZoneAm,0) +ISNULL(@ReimbAm,0) +
			ISNULL(@MilageAm,0) + ISNULL(@BonusAm,0)
				FROM #tempDetailPay WHERE EmpID = @ID       
				IF @Ded_DeductionAmount IS NULL
				BEGIN 
					SET @Ded_DeductionAmount = ISNULL(@Ded_DeductionAmount,0)
				END
				INSERT INTO #tempDeduction (ID,fDesc,Amount,PaidBy ) VALUES (@Ded_Ded,@Ded_fDesc,@Ded_DeductionAmount,@Ded_ByW)

			END
		END
	 END
 END        
 ELSE IF @Ded_AccuredOn = 1 -- Accoured on Dollor Amount        
 BEGIN 
	IF @Ded_JobSpecific = 0 
	BEGIN
		IF @Ded_EmpTop =0 OR @Ded_YTD < @Ded_EmpTop
		BEGIN
			SELECT @Ded_DeductionAmount = (((SELECT SUM(ISNULL(RegAmt,0))
			+SUM(ISNULL(OTAmt,0))+SUM(ISNULL(DTAmt,0))+SUM(ISNULL(TTAmt,0))+SUM(ISNULL(NTAmt,0))
			FROM #tempDetailPay WHERE EmpID = @ID)* ISNULL(@Ded_EmpRate,0))/100)
			+ ISNULL(@HolidayAm,0)+ ISNULL(@VacAm,0) + ISNULL(@ZoneAm,0) +ISNULL(@ReimbAm,0) +
			ISNULL(@MilageAm,0) + ISNULL(@BonusAm,0)
			IF @Ded_DeductionAmount IS NULL
			BEGIN 
				SET @Ded_DeductionAmount = ISNULL(@Ded_DeductionAmount,0)
			END
			INSERT INTO #tempDeduction (ID,fDesc,Amount,PaidBy ) VALUES (@Ded_Ded,@Ded_fDesc,@Ded_DeductionAmount,@Ded_ByW)

		END
    END
	ELSE 
	 BEGIN
		IF EXISTS ( SELECT * FROM JobDed WHERE Job in (
			SELECT td.Job FROM TicketD td INNER JOIN tblWork w on td.fWork = w.ID INNER JOIN emp e on e.CallSign = w.fDesc
			WHERE e.ID = @ID AND CAST(EDate AS date) >= CAST(@startdate AS date)             
			AND CAST(EDate AS date) <= CAST(@enddate AS date) ) AND Ded = @Ded_Ded)
		BEGIN
			IF @Ded_EmpTop =0 OR @Ded_YTD < @Ded_EmpTop
			BEGIN
				SELECT @Ded_DeductionAmount = (((SELECT SUM(ISNULL(RegAmt,0))
				+SUM(ISNULL(OTAmt,0))+SUM(ISNULL(DTAmt,0))+SUM(ISNULL(TTAmt,0))+SUM(ISNULL(NTAmt,0))
				FROM #tempDetailPay WHERE EmpID = @ID)* ISNULL(@Ded_EmpRate,0))/100)
				+ ISNULL(@HolidayAm,0)+ ISNULL(@VacAm,0) + ISNULL(@ZoneAm,0) +ISNULL(@ReimbAm,0) +
			ISNULL(@MilageAm,0) + ISNULL(@BonusAm,0)
				
				IF @Ded_DeductionAmount IS NULL
				BEGIN 
					SET @Ded_DeductionAmount = ISNULL(@Ded_DeductionAmount,0)
				END
				INSERT INTO #tempDeduction (ID,fDesc,Amount,PaidBy ) VALUES (@Ded_Ded,@Ded_fDesc,@Ded_DeductionAmount,@Ded_ByW)

			END
		END
	 END
 END        
  END        
        
 --------------    
 --------------    
  --------RESET------->          
   SET @Ded_Ded= NULL ;SET @Ded_Emp = NULL ;SET @Ded_BasedOn = NULL ;SET @Ded_AccuredOn = NULL ;SET @Ded_ByW = NULL ;SET @Ded_EmpRate = NULL ;SET @Ded_EmpTop = NULL ;        
   SET @Ded_EmpGL = NULL ;SET @Ded_CompRate = NULL ;SET @Ded_CompTop = NULL ;SET @Ded_CompGL = NULL ;SET @Ded_CompGLE = NULL ;SET @Ded_InUse = NULL ;        
   SET @Ded_YTD = NULL ;SET @Ded_YTDC  = NULL ;SET @Ded_fDesc = NULL;        
   --------------->         
        
  FETCH NEXT FROM db_cursor2011 INTO           
   @Ded_Ded,@Ded_Emp,@Ded_BasedOn,@Ded_AccuredOn,@Ded_ByW,@Ded_EmpRate,@Ded_EmpTop,@Ded_EmpGL,@Ded_CompRate,@Ded_CompTop,@Ded_CompGL,@Ded_CompGLE,@Ded_InUse,@Ded_YTD,@Ded_YTDC,@Ded_fDesc,@Ded_JobSpecific        
END            
          
          
    CLOSE db_cursor2011            
 DEALLOCATE db_cursor2011    

SELECT * FROM #tempDeduction

DROP TABLE #tempDeduction
DROP TABLE #tempDetailPay
 END