CREATE PROCEDURE [dbo].[SpUpdatingProjectBudgetAmount]
	@Job int = 0,
	@UpdatedBy varchar(100)
AS
 
 DECLARE @JobT int; 

 SELECT @JobT=isnull(template,0) from job where ID=@Job

  ---------------------------------------------------------------------------------------------------------------->

Declare @CurrBHour    numeric(30, 2)
Declare @CurrBRev     numeric(30, 2)
Declare @CurrBMat     numeric(30, 2)
Declare @CurrBLabor   numeric(30, 2)
Declare @CurrBCost    numeric(30, 2)
Declare @CurrBProfit  numeric(30, 2)
Declare @CurrBRatio   numeric(30, 2)
Declare @CurrBOther   numeric(30, 2)
            

Select 
	@CurrBHour   = BHour   ,
    @CurrBRev    = BRev    ,
    @CurrBMat    = BMat    ,
    @CurrBLabor  = BLabor  ,
    @CurrBCost   = BCost   ,
    @CurrBProfit = BProfit ,
    @CurrBRatio  = BRatio  ,
    @CurrBOther  = IsNULL(BOther,0)  
FROM Job
WHERE ID = @job
			 
------------------------------------------------------------------------------------------------------------------->


UPDATE jc  set jc.BHours = 
(
CASE c.SCycle 
WHEN 0  THEN c.Hours                   --Monthly \n");
WHEN 1  THEN c.Hours / 2               --Bi-Monthly \n");
WHEN 2  THEN c.Hours / 3               --Quarterly \n");
WHEN 3  THEN c.Hours / 6               --Semi-Anually \n");
WHEN 4  THEN c.Hours / 12              --Anually \n");
WHEN 5  THEN (c.Hours * 4.3)           --Weekly \n");
WHEN 6  THEN (c.Hours * (2.15))        --Bi-Weekly \n");
WHEN 7  THEN ( c.Hours / ( 2.9898 ) )  --Every 13 Weeks  \n");
WHEN 10 THEN c.Hours / 12*2            --Every 2 Years \n");
WHEN 8  THEN  c.Hours / 12*3           --Every 3 Years \n");
WHEN 9  THEN c.Hours / 12*5            --Every 5 Years \n");
WHEN 11 THEN c.Hours / 12*7            --Every 7 Years \n");
WHEN 13 THEN (c.Hours * ( CASE c.SWE WHEN 1 THEN 30 ELSE   21.66 END) ) --Daily \n");
WHEN 14 THEN (c.Hours * (2) )          --Twice a Month \n");
WHEN 15 THEN (c.Hours / (4) )          --3 Times/Year \n"); 
else jc.BHours   
END
)  

FROM  jobtitem jc 
INNER JOIN CONTRACT c on c.job=jc.Job
WHERE  jc.Line= ( select  min(jcc.Line) from jobtitem jcc where  jcc.type=1 and jcc.fDesc='Labor' and jcc.Job=@Job )  and jc.type=1 and jc.fDesc='Labor' and jc.Job=@Job

 ---------------------------------------------------------------------------------------------------------------->

UPDATE b set   b.LabRate = 
( CASE (SELECT Isnull(JobCostLabor, 0)  FROM   Control)
WHEN 1 THEN Isnull(PR.CReg, 0)
ELSE 
(
CASE Isnull(PR.Reg, 0)
WHEN 0 THEN Isnull(w.hourlyrate, 0)
ELSE Isnull(PR.Reg, 0)
END
)
END )
FROM CONTRACT c 
INNER JOIN JobTItem jt on c.job=jt.Job 
INNER JOIN bom b on b.JobTItemID=jt.ID 
INNER JOIN job j on c.job=j.id  
INNER JOIN loc l on l.loc=c.Loc 
INNER JOIN Route r on r.ID=l.Route 
INNER JOIN tblWork w on w.ID =r.Mech
INNER JOIN emp e on e.fWork=w.ID INNER JOIN PRWageItem PR on PR.Wage=j.WageC and pr.Emp=e.ID
AND jt.Line= ( select  min(jcc.Line) from jobtitem jcc where  jcc.type=1 and jcc.fDesc='Labor' and jcc.Job=@Job ) and jt.type=1 and jt.fDesc='Labor' and c.job=@Job

 ---------------------------------------------------------------------------------------------------------------->

UPDATE jc  set   jc.ETC= (m.LabRate * jc.BHours) from bom  m 
INNER JOIN jobtitem jc on m.JobTItemID=jc.ID WHERE   jc.Line= ( select  min(jcc.Line) from jobtitem jcc where  jcc.type=1 and jcc.fDesc='Labor' and jcc.Job=@Job ) 
and jc.type=1 and jc.fDesc='Labor' 
AND  isnull(m.LabRate,0) <> 0 
AND  isnull(jc.BHours,0) <> 0 and jc.Job=@Job

---------------------------ES-4418--------------------------------------------------------------------------->

IF NOT EXISTS(SELECT 1 FROM JobTItem WHERE job=@Job AND TYPE=0)
BEGIN 
INSERT [dbo].[JobTItem] (  [JobT], [Job], [Type], [fDesc], [Code], [Actual], [Budget], [Line], [Percent], [Comm], 
[Stored], [Modifier], [ETC], [ETCMod], [THours], [FC], [Labor], [BHours], [GL], [OrderNo]  ) 
VALUES (  @JobT, @Job, 0, N'Revenue', N'200', CAST(0.00 AS Numeric(30, 2)), CAST(0.00 AS Numeric(30, 2)), 1, CAST(0.00 AS Numeric(30, 2)), CAST(0.00 AS Numeric(30, 2)), CAST(0.00 AS Numeric(30, 2)), CAST(0.00 AS Numeric(30, 2)), CAST(0.00 AS Numeric(30, 2)), CAST(0.00 AS Numeric(30, 2)), NULL, NULL, CAST(0.00 AS Numeric(30, 2)), CAST(0.00 AS Numeric(30, 2)), NULL, 1 )

INSERT INTO Milestone(JobTItemID,Type,MilestoneName ,RequiredBy , CreationDate)

SELECT   ID , 1 ,'Revenue' , GETDATE() , GETDATE()  from JobTItem where Job =@Job and type=0

END

DECLARE @L int =0;

SELECT  @L= min(line) FROM JobTItem WHERE Job =@Job AND TYPE=0;

SET     @L= isnull(@L,0)

 ---------------------------------------------------------------------------------------------------------------->

update m set m.amount=   
case c.BCycle
WHEN 0 THEN c.BAmt             --Monthly  \n");
WHEN 1 THEN c.BAmt / 2         --Bi-Monthly  \n");
WHEN 2 THEN c.BAmt / 3         --Quarterly  \n");
WHEN 3 THEN c.BAmt / 4         --3 Times/Year  \n");
WHEN 4 THEN c.BAmt / 6         --Semi-Annually   \n");
WHEN 5 THEN c.BAmt / 12        --Annually \n"); 
WHEN 7 THEN c.BAmt / (12*3)    --'3 Years'  \n");
WHEN 8 THEN c.BAmt / (12*5)    --'5 Years'  \n");
WHEN 9 THEN c.BAmt / (12*2)    --'2 Years'  \n");
else m.Amount end 
FROM Milestone  m 
INNER JOIN jobtitem jc ON m.JobTItemID=jc.ID 
INNER JOIN contract c on c.job =jc.Job
WHERE jc.Line= @L AND jc.type=0 and c.Job=@Job

 ---------------------------------------------------------------------------------------------------------------->

UPDATE jc set jc.Budget= m.amount
FROM Milestone  m   
INNER JOIN jobtitem jc ON m.JobTItemID=jc.ID   
INNER JOIN contract c on c.job =jc.Job  
WHERE jc.Line= @L AND jc.type=0  and c.Job=@Job 

EXEC [dbo].[spUpdateJobcostByJob] 	@job =@job

---------------------------------------------------------------------------------------------------------------->
/*** Logs for updating buget values ***/
DECLARE @bHour numeric(30, 2)
DECLARE @bRev numeric(30, 2)
DECLARE @bCost numeric(30, 2)
DECLARE @bMat numeric(30, 2)
DECLARE @bother numeric(30, 2);
DECLARE @bLabor numeric(30, 2)
DECLARE @bRatio numeric(30, 2)
DECLARE @bProfit numeric(30, 2)
Declare @Screen varchar(100) = 'Project';
Declare @RefId int = @Job;

SET @bHour = ISNULL((SELECT SUM(ISNULL(BHours, 0)) FROM jobtitem WHERE type = 1 AND job = @job), 0)

SET @brev = ISNULL((SELECT SUM(ISNULL(Budget, 0)) FROM JobTItem WHERE Type = 0 AND Job = @job), 0)

SET @bcost = ISNULL((SELECT (SUM(ISNULL(Budget, 0)) + SUM(ISNULL(Modifier, 0)) + SUM(ISNULL(ETC, 0)) + SUM(ISNULL(ETCMod, 0)))
					FROM JobTItem WHERE Type = 1 AND Job = @job), 0)

SET @bmat = ISNULL((SELECT (SUM(ISNULL(Budget, 0)) + SUM(ISNULL(Modifier, 0)))
					FROM JobTItem
					INNER JOIN bom
						ON bom.JobTItemID = JobTItem.ID
						INNER JOIN BOMT
							ON bomt.ID = bom.Type
					WHERE (bomt.Type = 'Materials'
						OR bomt.Type = 'Inventory')
						AND Job = @job), 0)

SET @bOther = ISNULL((SELECT (SUM(ISNULL(Budget, 0)) + SUM(ISNULL(Modifier, 0)))
					FROM JobTItem
					INNER JOIN bom
						ON bom.JobTItemID = JobTItem.ID
						INNER JOIN BOMT
							ON bomt.ID = bom.Type
					WHERE bomt.Type <> 'Materials'
						AND bomt.Type <> 'Labor'
						AND bomt.Type <> 'Inventory'
						AND Job = @job), 0)

SET @blabor = ISNULL((SELECT (SUM(ISNULL(j.ETC, 0)) + SUM(ISNULL(j.ETCMod, 0)))
FROM JobTItem j
WHERE Type = 1 AND Job = @job), 0)

SET @bprofit = @brev - @bcost

IF @brev <> 0
BEGIN
    SET @bratio = CONVERT(numeric(30, 2), ((@bprofit / @brev) * 100))
END
ELSE
BEGIN
    SET @bratio = 0
END
-- Budget - Hours
IF @CurrBHour != @bHour
BEGIN
	DECLARE @strCurrBHour varchar(50)
	SET @strCurrBHour = Convert(varchar(50),@CurrBHour)
	DECLARE @strbHour varchar(50)
	SET @strbHour = Convert(varchar(50),@bHour)
	EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Budget - Hours',@strCurrBHour,@strbHour
END
-- Budget - Revenue
IF @CurrBRev != @bRev
BEGIN
	DECLARE @strCurrBRev varchar(50)
	SET @strCurrBRev = Convert(varchar(50),@CurrBRev)
	DECLARE @strbRev varchar(50)
	SET @strbRev = Convert(varchar(50),@bRev)
	EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Budget - Revenue',@strCurrBRev,@strbRev
END

-- Budget - Material Expense
IF @CurrBMat != @bMat
BEGIN
	DECLARE @strCurrBMat varchar(50)
	SET @strCurrBMat = Convert(varchar(50),@CurrBMat)
	DECLARE @strbMat varchar(50)
	SET @strbMat = Convert(varchar(50),@bMat)
	EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Budget - Material Expense',@strCurrBMat,@strbMat
END

-- Budget - Labor Expense
IF @CurrBLabor != @bLabor
BEGIN
	DECLARE @strCurrBLabor varchar(50)
	SET @strCurrBLabor = Convert(varchar(50),@CurrBLabor)
	DECLARE @strbLabor varchar(50)
	SET @strbLabor = Convert(varchar(50),@bLabor)
	EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Budget - Labor Expense',@strCurrBLabor,@strbLabor
END

-- Budget - Total Expense
IF @CurrBCost != @bCost
BEGIN
	DECLARE @strCurrBCost varchar(50)
	SET @strCurrBCost = Convert(varchar(50),@CurrBCost)
	DECLARE @strbCost varchar(50)
	SET @strbCost = Convert(varchar(50),@bCost)
	EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Budget - Total Expense',@strCurrBCost,@strbCost
END

-- Budget - Profit
IF @CurrBProfit != @bProfit
BEGIN
	DECLARE @strCurrBProfit varchar(50)
	SET @strCurrBProfit = Convert(varchar(50),@CurrBProfit)
	DECLARE @strbProfit varchar(50)
	SET @strbProfit = Convert(varchar(50),@bProfit)
	EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Budget - Profit',@strCurrBProfit,@strbProfit
END

-- Budget - % in Profit
IF @CurrBRatio != @bRatio
BEGIN
	DECLARE @strCurrBRatio varchar(50)
	SET @strCurrBRatio = Convert(varchar(50),@CurrBRatio)
	DECLARE @strbRatio varchar(50)
	SET @strbRatio = Convert(varchar(50),@bRatio)
	EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Budget - % in Profit',@strCurrBRatio,@strbRatio
END

-- Budget - Other Expense
IF @CurrBOther != @BOther
BEGIN
	DECLARE @strCurrBOther varchar(50)
	SET @strCurrBOther = Convert(varchar(50),@CurrBOther)
	DECLARE @strBOther varchar(50)
	SET @strBOther = Convert(varchar(50),@BOther)
	EXEC log2_insert @UpdatedBy,@Screen,@RefId,'Budget - Other Expense',@strCurrBOther,@strBOther
END

/*** End logs ***/
