 
-----  EXEC [spGetJobCostByJob] 419,'','',1,10  

CREATE PROCEDURE [dbo].[spGetJobCostByJob]  

@job int=419,    
@sdate datetime = null,    
@edate datetime = null,    
@PageIndex INT  = 1,    
@PageSize INT   = 10    

AS 

BEGIN    
     
	SET NOCOUNT ON;    

	DECLARE @DateRageActivity int=0;
	IF(@sdate!='' and @edate!='') SET @DateRageActivity=1
    
	CREATE TABLE #temp    
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
		NonTicketlabor  numeric(30,2),
		Ticketlabor numeric(30,2),
		ActualLabor numeric(30,2),  
		ActualMaterial numeric(30,2),
		ActualOther numeric(30,2),	
		NonTicketOtherExp  numeric(30,2),
		TicketOtherExp numeric(30,2),
		Comm numeric(30,2),
		Budget numeric(30,2),  
		bomtype varchar(50),    
		ReceivePO numeric(30,2) , 
		Variance numeric(30,2),    
		Ratio numeric(30,2),  
		Total numeric(30,2),    
		TargetHours numeric(30,2),
		THours numeric(30,2),
		GroupID int
	) 

	-------------

	INSERT INTO #temp 
	( RowNumber
	, Phase
	, GroupName
	, CodeDesc
	, fDesc
	, JobType
	, Code
	, BType
	, Type
	, BHours
	, Actual  
	, NonTicketlabor   
	, Ticketlabor  
	, ActualMaterial  
	, NonTicketOtherExp   
	, TicketOtherExp  
	, Comm
	, Budget
	, bomtype
	, ReceivePO 
	, TargetHours 
	, THours
	, GroupID
	, Variance
	, Ratio 
	, Total 
	, ActualLabor 
	, ActualOther 
	)   
	SELECT ROW_NUMBER() OVER(ORDER BY Type ASC) AS RowNumber 
		, X.*  
		, (isnull(x.Budget,0) - isnull(x.Actual,0)) Variance
		, (((isnull(x.Actual,0) + isnull(x.Comm,0)) / ( case x.Budget when 0 then 1 else  isnull(x.Budget,1) end )) * 100) Ratio 
		, (isnull(x.Actual,0) + isnull(x.Comm,0) + isnull(x.ReceivePo,0)) as Total  
		, (isnull(x.Ticketlabor,0) + isnull(x.NonTicketlabor,0) ) as ActualLabor
		, (isnull(x.TicketOtherExp,0) + isnull(x.NonTicketOtherExp,0)) as ActualOther
	FROM (

		-------Labor
		SELECT  isnull(jt.line,0) as Phase 
			, isnull((Select top 1 GroupName from tblEstimateGroup where Id=jt.GroupID),'NA') GroupName   
			, ((select top 1 JobCodeDesc from tblJobCodeDesc_ByJobType where JobCodeID in ( select jc.ID  FROM JobCode jc where jc.Code=jt.Code) and JobTypeID=
				(select type from job where id =jt.Job) )) as CodeDesc 
			, isNull(jt.fDesc,'')  fDesc
			, 'Cost' as Jobtype
			, isnull(jt.Code,'100') as Code
			, isnull(bt.Type,'Labor') as Btype
			, isnull(jt.type,0) as type
			, isnull(jt.BHours,0) Bhours 
			, isnull((select sum(amount) from jobi where jobi.job=jt.job and jobi.Phase=jt.Line   
				AND cast ( jobi.fDate as date ) >= case @DateRageActivity when 1 then cast ( @sdate as date ) else  cast ( jobi.fDate as date ) end
				AND cast (jobi.fDate as date ) <= case @DateRageActivity  when 1 then  cast ( @edate as date ) else  cast ( jobi.fDate as date ) end
				AND isnull(jobi.Type,0) <> 0   ),0) as Actual

				 

	  , isnull((select sum(amount) from jobi 
	 	        inner join  JobTItem on JobTItem.Line=jobi.Phase and JobTItem.Job=@job and JobTItem.Line=jt.Line   
		        inner join bom on bom.JobTItemID=JobTItem.ID
		        inner join BOMT on bomt.ID =bom.Type    
	            WHERE jobi.job=jt.job  
	            AND jobi.Phase=jt.Line  
	            AND jobi.Job = @job and jobi.fDesc not in ('Mileage on Ticket','Expenses on Ticket')
				AND isnull(jobi.Type,0) <> 0  and  isnull(Jobi.Labor,0) <> 1  and BOMT.Type='labor'
				AND cast ( jobi.fDate as date ) > = case @DateRageActivity  when 1 then  cast ( @sdate as date ) else  cast ( jobi.fDate as date ) end
				AND cast (jobi.fDate as date )  < = case @DateRageActivity  when 1 then  cast ( @edate as date ) else  cast ( jobi.fDate as date ) end
				AND isnull(jobi.Type,0) <> 0   ),0) NonTicketlabor  
				
         , isnull((select sum(amount) from jobi 
	 	        inner join  JobTItem on JobTItem.Line=jobi.Phase and JobTItem.Job=@job  and JobTItem.Line=jt.Line  
		        inner join bom on bom.JobTItemID=JobTItem.ID
		        inner join BOMT on bomt.ID =bom.Type  
	            WHERE jobi.job=jt.job  
	            AND jobi.Phase=jt.Line  
	            AND jobi.Job = @job and jobi.fDesc not in ('Mileage on Ticket','Expenses on Ticket')
				AND isnull(jobi.Type,0) <> 0  AND     isnull(Jobi.Labor,0) = 1
				AND cast ( jobi.fDate as date ) >= case @DateRageActivity when 1 then cast ( @sdate as date ) else  cast ( jobi.fDate as date ) end
				AND cast (jobi.fDate as date ) <= case @DateRageActivity  when 1 then  cast ( @edate as date ) else  cast ( jobi.fDate as date ) end
				AND isnull(jobi.Type,0) <> 0   ),0)Ticketlabor  

			 
					
	   ,   isnull((select sum(amount) from jobi 
	            inner join  JobTItem on JobTItem.Line=jobi.Phase and JobTItem.Job=@job   and JobTItem.Line=jt.Line 
		        inner join bom on bom.JobTItemID=JobTItem.ID  
		        inner join BOMT on bomt.ID =bom.Type  AND ( bomt.Type='Materials'  or bomt.Type='Inventory')
	            where 
				jobi.Job = @job  
				and jobi.fDesc not in ('Mileage on Ticket','Expenses on Ticket')
			    AND isnull(jobi.Type,0) <> 0  and  isnull(Jobi.Labor,0) <> 1 
				AND jobi.job=jt.job 
				and jobi.Phase=jt.Line   
				AND cast ( jobi.fDate as date ) >= case @DateRageActivity when 1 then cast ( @sdate as date ) else  cast ( jobi.fDate as date ) end
				AND cast (jobi.fDate as date ) <= case @DateRageActivity  when 1 then  cast ( @edate as date ) else  cast ( jobi.fDate as date ) end
				AND isnull(jobi.Type,0) <> 0   ) , 0 )   ActualMaterial   

		  ,    isnull((select sum(amount) from jobi 
		        inner join  JobTItem on JobTItem.Line=jobi.Phase  and JobTItem.Job=@job   and JobTItem.Line=jt.Line
		        inner join bom on bom.JobTItemID=JobTItem.ID and  isnull(Jobi.Labor,0) <> 1
		        inner join BOMT on bomt.ID =bom.Type  AND (bomt.Type<>'Materials' and bomt.Type<>'Inventory')
		        where jobi.job=jt.job and jobi.Phase=jt.Line   
				AND cast ( jobi.fDate as date ) >= case @DateRageActivity when 1 then cast ( @sdate as date ) else  cast ( jobi.fDate as date ) end
				AND cast (jobi.fDate as date ) <= case @DateRageActivity  when 1 then  cast ( @edate as date ) else  cast ( jobi.fDate as date ) end
				and jobi.fDesc not in ('Mileage on Ticket','Expenses on Ticket') 
				AND isnull(jobi.Type,0) <> 0 and bomt.Type<>'labor'
				AND (jobi.TransID > 0 or isnull(jobi.Labor,0) = 0)
				),0)  as  NonTicketOtherExp 

		   ,    isnull((select sum(amount) from jobi 
				inner join  JobTItem on JobTItem.Line=jobi.Phase   and JobTItem.Job=@job and JobTItem.Line=jt.Line
		        inner join bom on bom.JobTItemID=JobTItem.ID
		        inner join BOMT on bomt.ID =bom.Type  	
			    where jobi.job=jt.job and jobi.Phase=jt.Line 
				AND jobi.Job = @job and jobi.fDesc  in ('Mileage on Ticket','Expenses on Ticket')
				AND cast ( jobi.fDate as date ) > = case @DateRageActivity when 1 then cast ( @sdate as date ) else  cast ( jobi.fDate as date ) end
				AND cast (jobi.fDate as date ) < = case @DateRageActivity  when 1 then  cast ( @edate as date ) else  cast ( jobi.fDate as date ) end
				AND isnull(jobi.Type,0) <> 0   ),0) as TicketOtherExp 
				
			, isnull(jt.Comm,0) as Comm
			, (CASE jt.Type WHEN 0 THEN ISNULL(jt.Budget,0) ELSE ISNULL(jt.Budget,0) +     
				ISNULL(jt.Modifier,0) +    
				ISNULL(jt.ETC,0) +    
				ISNULL(jt.ETCMod,0) END) as Budget
			, isnull(bt.Type,'Labor') as Bomtype
			, 0 ReceivePo
			, isnull(jt.TargetHours,0) TargetHours
			, isnull(jt.THours,0) THours
			, isnull(jt.GroupID,0) GroupID  
		FROM jobtitem jt
		inner join bom b on b.JobTItemID=jt.ID
		inner join BOMT bt on bt.ID=b.Type
		WHERE jt.job=@job
		and bt.Type='Labor'

		UNION All

		-------Material and Other's

		SELECT   isnull(jt.line,0) as Phase 
			, isnull((Select top 1 GroupName from tblEstimateGroup where Id=jt.GroupID),'NA') GroupName   
			, ((select top 1 JobCodeDesc from tblJobCodeDesc_ByJobType where JobCodeID in ( select jc.ID  FROM JobCode jc where jc.Code=jt.Code) and JobTypeID=
			(select type from job where id =jt.Job) )) as CodeDesc 
			, jt.fDesc 
			, 'Cost' as Jobtype
			, isnull(jt.Code,'100') Code
			, isnull(bt.Type,'Material')  as Btype
			, isnull(jt.type,0) Type
			, isnull(jt.BHours,0) Bhours
			, isnull( (select sum(amount) from jobi where jobi.job=jt.job 
				 and jobi.Phase=jt.Line 
				 and cast ( jobi.fDate as date ) >= case @DateRageActivity when 1 then cast ( @sdate as date ) else  cast ( jobi.fDate as date ) end
				 and cast (jobi.fDate as date ) <= case @DateRageActivity when 1 then  cast ( @edate as date ) else  cast ( jobi.fDate as date ) end
				 and jobi.Type=jt.Type ) ,0) as Actual 				

	     			 

	    , isnull((select sum(amount) from jobi 
	 	        inner join  JobTItem on JobTItem.Line=jobi.Phase and JobTItem.Job=@job  and JobTItem.Line=jt.Line  
		        inner join bom on bom.JobTItemID=JobTItem.ID
	            inner join BOMT on bomt.ID =bom.Type       
	            WHERE jobi.job=jt.job and JobTItem.Line=jt.Line
	            AND jobi.Phase=jt.Line  
	            AND jobi.Job = @job and jobi.fDesc not in ('Mileage on Ticket','Expenses on Ticket')
				AND isnull(jobi.Type,0) <> 0  and  isnull(Jobi.Labor,0) <> 1 and BOMT.Type='labor'
				AND cast ( jobi.fDate as date ) >= case @DateRageActivity when 1 then cast ( @sdate as date ) else  cast ( jobi.fDate as date ) end
				AND cast (jobi.fDate as date ) <= case @DateRageActivity  when 1 then  cast ( @edate as date ) else  cast ( jobi.fDate as date ) end
				AND isnull(jobi.Type,0) <> 0   ),0) NonTicketlabor  
				
         ,  isnull((select sum(amount) from jobi 
	 	        inner join  JobTItem on JobTItem.Line=jobi.Phase and JobTItem.Job=@job   and JobTItem.Line=jt.Line
		        inner join bom on bom.JobTItemID=JobTItem.ID
		        inner join BOMT on bomt.ID =bom.Type  
	            WHERE jobi.job=jt.job and JobTItem.Line=jt.Line
	            AND jobi.Phase=jt.Line  
	            AND jobi.Job = @job and jobi.fDesc not in ('Mileage on Ticket','Expenses on Ticket')
				AND isnull(jobi.Type,0) <> 0  AND     isnull(Jobi.Labor,0) = 1
				AND cast ( jobi.fDate as date ) >= case @DateRageActivity when 1 then cast ( @sdate as date ) else  cast ( jobi.fDate as date ) end
				AND cast (jobi.fDate as date ) <= case @DateRageActivity  when 1 then  cast ( @edate as date ) else  cast ( jobi.fDate as date ) end
				AND isnull(jobi.Type,0) <> 0   ),0)  Ticketlabor 
					
					--------- Material
	       ,    isnull((select sum(amount) from jobi 
	            inner join  JobTItem on JobTItem.Line=jobi.Phase and JobTItem.Job=@job  and JobTItem.Line=jt.Line 
		        inner join bom on bom.JobTItemID=JobTItem.ID 
		        inner join BOMT on bomt.ID =bom.Type  
	            where 
				jobi.Job = @job and JobTItem.Line=jt.Line
				and jobi.fDesc not in ('Mileage on Ticket','Expenses on Ticket')
			    AND isnull(jobi.Type,0) <> 0    and  isnull(Jobi.Labor,0) <> 1
				AND jobi.job=jt.job 
				and jobi.Phase=jt.Line   
				AND cast ( jobi.fDate as date ) > = case @DateRageActivity when 1 then cast ( @sdate as date ) else  cast ( jobi.fDate as date ) end
				AND cast (jobi.fDate as date )  < = case @DateRageActivity  when 1 then  cast ( @edate as date ) else  cast ( jobi.fDate as date ) end
				AND isnull(jobi.Type,0) <> 0   ) , 0 )   ActualMaterial  

	      

		   ,    isnull((select sum(amount) from jobi 
		        inner join  JobTItem on JobTItem.Line=jobi.Phase  and JobTItem.Job=@job   and JobTItem.Line=jt.Line
		        inner join bom on bom.JobTItemID=JobTItem.ID 
		        inner join BOMT on bomt.ID =bom.Type  AND (bomt.Type<>'Materials' and bomt.Type<>'Inventory')
		        where jobi.job=jt.job and jobi.Phase=jt.Line  and  isnull(Jobi.Labor,0) <> 1 
				AND cast ( jobi.fDate as date ) >= case @DateRageActivity when 1 then cast ( @sdate as date ) else  cast ( jobi.fDate as date ) end
				AND cast (jobi.fDate as date ) <= case @DateRageActivity  when 1 then  cast ( @edate as date ) else  cast ( jobi.fDate as date ) end
				and jobi.fDesc not in ('Mileage on Ticket','Expenses on Ticket') 
				AND isnull(jobi.Type,0) <> 0 and bomt.Type<>'labor'
				AND (jobi.TransID > 0 or isnull(jobi.Labor,0) = 0)
				),0)  as  NonTicketOtherExp 

		 ,    isnull((select sum(amount) from jobi 
				inner join  JobTItem on JobTItem.Line=jobi.Phase   and JobTItem.Job=@job and JobTItem.Line=jt.Line
		        inner join bom on bom.JobTItemID=JobTItem.ID
		        inner join BOMT on bomt.ID =bom.Type  	
			    where jobi.job=jt.job and jobi.Phase=jt.Line 
				AND jobi.Job = @job and jobi.fDesc  in ('Mileage on Ticket','Expenses on Ticket')
				AND cast ( jobi.fDate as date ) >= case @DateRageActivity when 1 then cast ( @sdate as date ) else  cast ( jobi.fDate as date ) end
				AND cast (jobi.fDate as date ) <= case @DateRageActivity  when 1 then  cast ( @edate as date ) else  cast ( jobi.fDate as date ) end
				AND isnull(jobi.Type,0) <> 0   ),0) as TicketOtherExp 

			,   isnull( (select   Sum(ISNULL(p.Balance,0)) FROM POItem p 
				INNER JOIN PO on p.po = po.po  WHERE PO.Status in (0,3,4) AND p.Job =jt.job 
				and cast ( PO.fDate as date ) >= case @DateRageActivity when 1 then cast ( @sdate as date ) else  cast ( PO.fDate as date ) end
				and cast (PO.fDate as date ) <= case @DateRageActivity when 1 then  cast ( @edate as date ) else  cast ( PO.fDate as date ) end
				and p.Phase=jt.Line) ,0) 
				as Comm 
			, (CASE jt.Type WHEN 0 THEN ISNULL(jt.Budget,0) ELSE ISNULL(jt.Budget,0) +     
				ISNULL(jt.Modifier,0) +    
				ISNULL(jt.ETC,0) +    
				ISNULL(jt.ETCMod,0) END) as Budget
			, isnull(bt.Type,'Material')  as Bomtype
			, isnull( ( select   Sum(ISNULL(RPOI.Amount,0)) FROM POItem p 
				INNER JOIN PO on p.po = po.po  
				inner join ReceivePO RPO on RPO.PO=PO.PO 
				inner join RPOItem RPOI on RPOI.ReceivePO=RPO.ID
				WHERE isnull(RPO.Status,0)=0 AND p.Job =jt.job 
					and cast ( RPO.fDate as date ) >= case @DateRageActivity when 1 then cast ( @sdate as date ) else  cast ( RPO.fDate as date ) end
					and cast (RPO.fDate as date ) < = case @DateRageActivity when 1 then  cast ( @edate as date ) else  cast ( RPO.fDate as date ) end
					and p.Phase=jt.Line) ,0 )
				as ReceivePo
			, isnull(jt.TargetHours,0) TargetHours 
			, isnull(jt.THours,0) THours 
			, isnull(jt.GroupID,0) GroupID  
		FROM jobtitem jt
		inner join bom b on b.JobTItemID=jt.ID
		inner join BOMT bt on bt.ID=b.Type
		WHERE jt.job=@job
		and bt.Type  <>  'Labor'

		Union All

		-------Revenue 

		SELECT  isnull(jt.line,0) as Phase 
			, isnull((Select top 1 GroupName from tblEstimateGroup where Id=jt.GroupID),'NA') GroupName   
			, ((select top 1 JobCodeDesc from tblJobCodeDesc_ByJobType where JobCodeID in ( select jc.ID  FROM JobCode jc where jc.Code=jt.Code) and JobTypeID=
				(select type from job where id =jt.Job) )) as CodeDesc 
			, jt.fDesc 
			, 'Revenue' as Jobtype
			, isnull(jt.Code,'100') Code
			, 'Revenue' as Btype
			, isnull(jt.type,0) Type
			, isnull(jt.BHours,0) Bhours

			, isnull((select sum(amount) from jobi where jobi.job=jt.job and jobi.Phase=jt.Line 
		    and cast ( jobi.fDate as date ) >= case @DateRageActivity when 1 then cast ( @sdate as date ) else  cast ( jobi.fDate as date ) end
		    and cast (jobi.fDate as date ) <= case @DateRageActivity when 1 then  cast ( @edate as date ) else  cast ( jobi.fDate as date ) end
			and jobi.Type=jt.Type ),0) as Actual

		   

	        , 0 NonTicketlabor  				
               
		    , 0 Ticketlabor  					

	        , 0 ActualMaterial  	      

	        , 0	NonTicketOtherExp

		    , 0	 TicketOtherExp

			, isnull(jt.Comm,0) as Comm
			, (CASE jt.Type WHEN 0 THEN ISNULL(jt.Budget,0) ELSE ISNULL(jt.Budget,0) +     
				ISNULL(jt.Modifier,0) +    
				ISNULL(jt.ETC,0) +    
				ISNULL(jt.ETCMod,0) END) as  Budget
			, 'Revenue' as Bomtype
			, 0 ReceivePo 
			, isnull(jt.TargetHours,0) TargetHours
			, isnull(jt.THours,0) THours 
			, isnull(jt.GroupID,0) GroupID  
		FROM jobtitem jt 
		WHERE jt.job=@job
		and jt.Type=0
	) as X
     
	SELECT @job as ID
		, isnull(t.TargetHours,0)-isnull(t.THours,0)  TVH
		, isnull(t.BHours,0)-isnull(t.THours,0) as BVH
		, isnull((select sum(TargetHours) from JobTItem jt where jt.Job=@job
			AND jt.Type=1  and t.Type=1
			AND jt.Code=t.Code  
			AND isnull(jt.GroupID,0)= isnull(t.GroupID,0)),0) AS GTargetHours
		, isnull((select sum(BHours) from JobTItem jt where jt.Job=@job 
			AND jt.Type=1  and t.Type=1
			AND jt.Code=t.Code  
			AND isnull(jt.GroupID,0) = isnull(t.GroupID,0)),0) AS GBudgetHours
		, *
	FROM #temp t
    
	SELECT 
	sum(case type when 0 then (isnull(Actual,0) * -1)          else isnull(Actual,0) end) Actual ,
	sum(case type when 0 then (isnull(ActualLabor,0) * -1)     else isnull(ActualLabor,0) end) ActualLabor ,
	sum(case type when 0 then (isnull(ActualMaterial,0) * -1)  else isnull(ActualMaterial,0) end) ActualMaterial ,
	sum(case type when 0 then (isnull(ActualOther,0) * -1)     else isnull(ActualOther,0) end) ActualOther ,
	sum(case type when 0 then (isnull(Comm,0) * -1)            else isnull(Comm,0) end) Comm , 
    sum(case type when 0 then (isnull(Total,0) * -1)           else isnull(Total,0) end) Total ,
	sum(case type when 0 then (isnull(Budget,0) * -1)          else isnull(Budget,0) end) Budget ,
	sum(case type when 0 then (isnull(Variance,0) * -1)        else isnull(Variance,0) end) Variance ,
	  (case when sum(case type when 0 then (isnull(Budget,0) * -1) else isnull(Budget,0) end) <> 0 then    
  			((sum(case type when 0 then (isnull(Variance,0) * -1)  else isnull(Variance,0) end)/sum(case type when 0 then (isnull(Budget,0) * -1) else isnull(Budget,0) end))*100)     
  			else  convert(numeric(30,2),0) end) as Ratio ,
	 sum(case type when 0 then (isnull(ReceivePO,0) * -1) else isnull(ReceivePO,0) end) ReceivePO    
	FROM #temp    
    
	SELECT count(*) AS RecordCount FROM #temp    
    
	DROP TABLE #temp 

END


---- Targeted Var Hours = targeted hours-actual hours 

---- Budgeted Var Hours = budgeted hours-actual hours






