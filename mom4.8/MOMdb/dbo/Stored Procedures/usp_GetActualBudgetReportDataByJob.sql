-----  EXEC [spGetActualBudgetReportDataByJob] 10,'06/01/2019','06/30/2019',1,10    
CREATE PROCEDURE [dbo].usp_GetActualBudgetReportDataByJob     
	@job int,      
	@sdate datetime = null,      
	@edate datetime = null,      
	@PageIndex INT = 1,      
	@PageSize INT = 10     
  
AS   
  
 BEGIN      
       
SET NOCOUNT ON;      
      
 Create table #temp      
(    
  
RowNumber INT,    
Phase INT,   
GroupName varchar(100),  
CodeDesc varchar(100)  ,    
fDesc varchar(255),      
JobType varchar(50),      
Code varchar(10),     
BType varchar(50),    
Type smallint,      
BHours numeric(30,2),      
Actual numeric(30,2),      
Comm numeric(30,2),  
Budget numeric(30,2),    
bomtype varchar(50),      
ReceivePO numeric(30,2) ,   
Variance numeric(30,2),      
Ratio numeric(30,2),    
Total numeric(30,2),       
   
)      
  
-------------  
  
INSERT INTO #temp (RowNumber,Phase,GroupName,CodeDesc,fDesc, JobType, Code, BType, Type, BHours, Actual, Comm,  Budget,bomtype,ReceivePO , Variance,  
Ratio , Total)     
  
 select ROW_NUMBER() OVER(ORDER BY Type ASC) AS RowNumber , X.*  ,  (isnull(x.Budget,0) - isnull(x.Actual,0)) Variance  
 , ( ( (   isnull(x.Actual,0) + isnull(x.Comm,0)) / ( case x.Budget when 0 then 1 else  isnull(x.Budget,1) end )) * 100) Ratio , (isnull(x.Actual,0) + isnull(x.Comm,0) + isnull(x.ReceivePo,0)) as Total  
  
 from (  
  
 -------Labor  
  
 SELECT  isnull(jt.line,0) as Phase   
 ,  (Select top 1 GroupName from tblEstimateGroup where Id=jt.GroupID) GroupName     
 , ((select top 1 JobCodeDesc from tblJobCodeDesc_ByJobType where JobCodeID in ( select jc.ID  FROM JobCode jc where jc.Code=jt.Code) and JobTypeID=  
(select type from job where id =jt.Job) )) as CodeDesc   
 , isNull(jt.fDesc,'')  fDesc  
 , 'Cost' as Jobtype  
 , isnull(jt.Code,'100') as Code  
 , isnull(bt.Type,'Labor') as Btype  
 , isnull(jt.type,0) as type  
 , isnull(jt.BHours,0) Bhours  
 , isnull((select sum(amount) from jobi where jobi.job=jt.job and jobi.Phase=jt.Line and jobi.Type=1    and (Jobi.fDate  > @sdate and  Jobi.fDate <= @edate ) and isnull(jobi.Labor,0)=1),0) as Actual  
 , isnull(jt.Comm,0) as Comm  
 ,  (CASE jt.Type WHEN 0 THEN ISNULL(jt.Budget,0) ELSE ISNULL(jt.Budget,0) +       
ISNULL(jt.Modifier,0) +      
ISNULL(jt.ETC,0) +      
ISNULL(jt.ETCMod,0) END) as Budget  
 , isnull(bt.Type,'Labor') as Bomtype  
 , 0 ReceivePo  
 FROM jobtitem jt  
 inner join bom b on b.JobTItemID=jt.ID  
 inner join BOMT bt on bt.ID=b.Type  
 WHERE jt.job=@job  
 and bt.Type='Labor'  
  
 Union All  
  
  -------Material and Other's  
  
 SELECT   isnull(jt.line,0) as Phase   
 ,  (Select top 1 GroupName from tblEstimateGroup where Id=jt.GroupID) GroupName     
 , ((select top 1 JobCodeDesc from tblJobCodeDesc_ByJobType where JobCodeID in ( select jc.ID  FROM JobCode jc where jc.Code=jt.Code) and JobTypeID=  
(select type from job where id =jt.Job) )) as CodeDesc   
 , jt.fDesc   
 , 'Cost' as Jobtype  
 , isnull(jt.Code,'100') Code  
 , isnull(bt.Type,'Material')  as Btype  
 , isnull(jt.type,0) Type  
 , isnull(jt.BHours,0) Bhours  
 , isnull( (select sum(amount) from jobi where jobi.job=jt.job   
 and jobi.Phase=jt.Line    and (Jobi.fDate  > @sdate and  Jobi.fDate <= @edate )
 and jobi.Type=jt.Type and isnull(jobi.Labor,0)=0) ,0) as Actual  
 ,  isnull( (   select   Sum(ISNULL(p.Balance,0)) FROM POItem p   
INNER JOIN PO on p.po = po.po  WHERE PO.Status in (0,3,4) AND p.Job =jt.job and p.Phase=jt.Line) ,0)   
as Comm ,   
 (CASE jt.Type WHEN 0 THEN ISNULL(jt.Budget,0) ELSE ISNULL(jt.Budget,0) +       
ISNULL(jt.Modifier,0) +      
ISNULL(jt.ETC,0) +      
ISNULL(jt.ETCMod,0) END) as Budget  
 , isnull(bt.Type,'Material')  as Bomtype  
 , isnull( ( select   Sum(ISNULL(RPOI.Amount,0)) FROM POItem p   
INNER JOIN PO on p.po = po.po    
inner join ReceivePO RPO on RPO.PO=PO.PO   
inner join RPOItem RPOI on RPOI.ReceivePO=RPO.ID  
WHERE isnull(RPO.Status,0)=0 AND p.Job =jt.job and p.Phase=jt.Line) ,0 )  
as  ReceivePo  
 FROM jobtitem jt  
 inner join bom b on b.JobTItemID=jt.ID  
 inner join BOMT bt on bt.ID=b.Type  
 WHERE jt.job=@job  
 and bt.Type  <>  'Labor'  
  
 Union All  
 -------Revenue   
  
 SELECT    
   
   isnull(jt.line,0) as Phase   
 ,  (Select top 1 GroupName from tblEstimateGroup where Id=jt.GroupID) GroupName     
  , ((select top 1 JobCodeDesc from tblJobCodeDesc_ByJobType where JobCodeID in ( select jc.ID  FROM JobCode jc where jc.Code=jt.Code) and JobTypeID=  
(select type from job where id =jt.Job) )) as CodeDesc   
 , jt.fDesc   
 , 'Revenue' as Jobtype  
 , isnull(jt.Code,'100') Code  
 , 'Revenue' as Btype  
 , isnull(jt.type,0) Type  
 , isnull(jt.BHours,0) Bhours  
 ,  isnull((select sum(amount) from jobi where jobi.job=jt.job and jobi.Phase=jt.Line and jobi.Type=jt.Type 
		and (Jobi.fDate  > @sdate and  Jobi.fDate <= @edate )),0) as Actual  
 , jt.Comm  
  
, (CASE jt.Type WHEN 0 THEN ISNULL(jt.Budget,0) ELSE ISNULL(jt.Budget,0) +       
ISNULL(jt.Modifier,0) +      
ISNULL(jt.ETC,0) +      
ISNULL(jt.ETCMod,0) END) as  Budget  
  
 , 'Revenue' as Bomtype  
 , 0 ReceivePo  
 FROM jobtitem jt   
 WHERE jt.job=@job  
 and jt.Type=0  
 ) as X  
  
   
       
SELECT @job as ID, * FROM #temp      
  
  
-- WHERE RowNumber BETWEEN(@PageIndex -1) * @PageSize + 1 AND(((@PageIndex -1) * @PageSize + 1) + @PageSize) - 1      
      
SELECT sum(case type when 0 then (isnull(Actual,0) * -1)  else isnull(Actual,0) end) Actual,       
sum(case type when 0 then (isnull(Comm,0) * -1) else isnull(Comm,0) end) Comm,      
sum(case type when 0 then (isnull(Total,0) * -1)  else isnull(Total,0) end) Total,      
sum(case type when 0 then (isnull(Budget,0) * -1) else isnull(Budget,0) end) Budget,      
sum(case type when 0 then (isnull(Variance,0) * -1) else isnull(Variance,0) end) Variance,      
(case when sum(case type when 0 then (isnull(Budget,0) * -1) else isnull(Budget,0) end) <> 0 then      
((sum(case type when 0 then (isnull(Variance,0) * -1) else isnull(Variance,0) end)/sum(case type when 0 then (isnull(Budget,0) * -1) else isnull(Budget,0) end))*100)       
else        
convert(numeric(30,2),0) end) as Ratio,      
sum(case type when 0 then (isnull(ReceivePO,0) * -1) else isnull(ReceivePO,0) end) ReceivePO      
FROM #temp      
      
SELECT count(*) AS RecordCount FROM #temp      
      
DROP TABLE #temp      
  
  
  
 end  