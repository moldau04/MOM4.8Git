CREATE proc [dbo].[spGetOpportunity]
@SearchBy varchar(50),
@SearchValue varchar(250),
@startdate varchar(50),
@enddate varchar(50),
@rol int,
@IsSalesAsigned INT =0,
@EN INT				=0,
@UserID int			=0
AS
 
declare @Query varchar(max)

SET @Query = '

SELECT l.ID,
       r.Name,
       l.fDesc,
       l.RolType,
	   r.EN, 
	   B.Name As Company,
        s.Name as Status,      
       case l.Probability
       when 0 then ''Excellent''
       when 1 then ''Very Good''
       when 2 then ''Good''
       when 3 then ''Average''
       when 4 then ''Poor''
       end as Probability,
       l.Profit,
	   l.CreateDate,
       l.Rol,
		l.closedate,
		l.Remarks,
		case isnull(l.closed,0) when 1 then ''Yes'' else ''No'' end as closed, 
		l.revenue, 
		l.fuser, 
		l.CompanyName,
		(select top 1 (select top 1 name from terr where ID = lo.terr) from loc lo where lo.rol = r.ID) as defsales,
		(select count(d.Filename) from documents d  where screenid = (isnull(l.TicketID,0)) and screen = ''Ticket'') as DocumentCount,
		Estimate,
		(select job from Estimate where id = l.Estimate) as job,
		p.Referral,
		st.Description AS Stage,
		e.fFor,
		IIF(e.fFor=''ACCOUNT'', ISNULL(temp.Balance,0), 0) AS SixtyDay,
		CASE (select ISNULL(Discounted, 0) Discounted from Estimate where id = l.Estimate) WHEN 0 THEN ''No'' 
			WHEN 1 THEN ''Yes''
			ELSE '''' 
		END AS EstimateDiscounted,
		e.GroupName
FROM   Lead l
       INNER JOIN Rol r
               ON l.Rol = r.ID 
	   LEFT OUTER JOIN Prospect p on p.Rol = l.Rol
	   LEFT OUTER JOIN OEStatus s ON l.Status= s.ID
	   LEFT OUTER JOIN Stage st ON l.OpportunityStageID = st.ID
       LEFT JOIN Branch B on B.ID = r.EN   
	   LEFT JOIN Loc lc on lc.Rol = l.Rol
	   LEFT JOIN Estimate e on e.ID = l.Estimate

	   LEFT JOIN (
			SELECT ID, 
			ISNULL(t.Amount,0) AS Amount, 
			DATEDIFF(D,dbo.GetDueDate(ISNULL(i.fDate,t.fDate),-1),GETDATE()) AS DaysPastDue,
			(ISNULL(t.Amount,0) - ISNULL((SELECT sum(isnull(Amount,0)) 
				FROM Trans tr 
				INNER JOIN PaymentDetails p on tr.ID = p.TransID 
				INNER JOIN OpenAR o ON o.Ref = p.InvoiceID AND IsInvoice = 0 
				WHERE o.Type=3 AND o.TransID = tr.ID),0))	AS Balance, 
			ISNULL(AcctSub,0) AS Loc 
			FROM Trans t
			INNER JOIN Invoice i ON i.TransID = t.ID 
			WHERE	t.Type IN (6,5) 
				AND Acct = ISNULL((SELECT TOP 1 ID FROM Chart WHERE DefaultNo=''D1200''),0) 
				AND t.Amount <> 0 
				AND (t.Status = '''' or t.Status is null) 

			UNION
			SELECT			t.ID, 
			ISNULL(t.Amount,0) AS Amount, 
			DATEDIFF(D,dbo.GetDueDate(ISNULL(i.fDate,t.fDate),-1),GETDATE()) AS DaysPastDue,
			ISNULL(t.Amount,0) - ISNULL((SELECT Sum(ISNULL(amount,0)) FROM Trans t 
			INNER JOIN PaymentDetails p on t.ID =p.TransID 
			WHERE	p.InvoiceID = i.Ref
			AND ISNULL(p.IsInvoice, 1) = 1 ),0) AS Balance, 
			ISNULL(AcctSub,0) AS Loc 
			FROM Trans t 
			INNER JOIN Invoice i ON i.TransID = t.ID 
			WHERE (t.Status = '''' or t.Status is null) 
			AND t.Amount <> 0 
			AND ISNULL(t.Amount,0) - ISNULL((select sum(isnull(amount,0)) from trans t 
			inner join paymentdetails p on t.ID =p.TransID 
			where p.InvoiceID = i.Ref
			and Isnull(p.IsInvoice, 1) = 1 
			),0) <> 0


			UNION 
			SELECT t.ID, 
			ISNULL(t.Amount,0)*-1  AS Amount, 
			DATEDIFF(D,dbo.GetDueDate(ISNULL(i.fDate,t.fDate),-1),GETDATE()) AS DaysPastDue,
			ISNULL(t.Amount,0)*-1 AS Balance, 
			Invoice.Loc 
			FROM			PaymentDetails p  
			INNER JOIN  Trans t on t.ID = p.TransID 
			INNER JOIN Invoice i ON i.TransID = t.ID 
			INNER JOIN  ReceivedPayment r on r.ID = p.ReceivedPaymentID 
			LEFT JOIN Invoice ON Invoice.Ref = p.InvoiceID AND ISNULL(p.IsInvoice,1) = 1 
			WHERE isnull(p.IsInvoice,1) =1 
			AND p.InvoiceID NOT IN (SELECT Ref FROM Invoice) 
			AND ISNULL(t.Amount,0) <> 0 

			UNION 
			SELECT			t.ID,	
			ISNULL(t.Amount,0) AS Amount, 
			DATEDIFF(D,dbo.GetDueDate(ISNULL(i.fDate,t.fDate),-1),GETDATE()) AS DaysPastDue,
			(ISNULL(t.Amount,0) - ISNULL((SELECT SUM(ISNULL(t.Amount,0)) 
			FROM PaymentDetails p LEFT JOIN Trans t on p.TransID = t.ID 
			WHERE		InvoiceID = o.Ref 
			AND ISNULL(IsInvoice,1) = 0),0)) AS Balance, 
			t.AcctSub As Loc 
			FROM			OpenAR o 
			INNER JOIN  Trans t ON o.TransID = t.ID 
			INNER JOIN Invoice i ON i.TransID = t.ID 
			WHERE		o.Type = 2
			AND ISNULL(t.Amount,0) - ISNULL((SELECT SUM(ISNULL(t.Amount,0)) 
			FROM PaymentDetails p LEFT JOIN Trans t on p.TransID = t.ID 
			WHERE		InvoiceID = o.Ref 
			AND ISNULL(IsInvoice,1) = 0),0) <> 0 


			UNION 
			SELECT  t.ID, 
			ISNULL(t.Amount,0) AS Amount,
			DATEDIFF(D,dbo.GetDueDate(ISNULL(i.fDate,t.fDate),-1),GETDATE()) AS DaysPastDue,
			ISNULL(t.Amount,0) - 0 AS Balance, 
			t.AcctSub AS Loc
			FROM		Trans t
			INNER JOIN Invoice i ON i.TransID = t.ID 
			WHERE	Acct = (SELECT ID FROM Chart WHERE DefaultNo = ''D1200'') 
			AND t.Type NOT IN (99, 98, 5, 6, 1, 2, 3) 
			AND (t.Status = '''' OR t.Status IS NULL)  AND t.AcctSub IS NOT NULL  
			AND Sel <> 2 
		   ) AS temp On temp.Loc = lc.Loc AND temp.DaysPastDue BETWEEN 61  AND 90
	'                          
       IF(@EN = 1)  
      BEGIN  
          SET @Query +=' LEFT JOIN tblUserCo UC on UC.CompanyID = r.EN '
		  END
		   SET @Query += ' where r.type in (3,4)'         
           
	
	if(@SearchBy ='l.CloseDate')
	begin
	 if(@startdate <> '')
     begin		
		set @Query +=' and  CONVERT(date,l.CloseDate,101) >= CONVERT(date,'''+@startdate+''',101)'
     end
       
    if(@enddate <> '')
     begin		
		set @Query +=' and  CONVERT(date,l.CloseDate,101) <= CONVERT(date,'''+@enddate+''',101)'
     end     
	end
	else 
     begin


    if(@startdate <> '')
     begin		
		set @Query +=' and  CONVERT(date,l.CreateDate,101) >= CONVERT(date,'''+@startdate+''',101)'
     end
       
    if(@enddate <> '')
     begin		
		set @Query +=' and  CONVERT(date,l.CreateDate,101) <= CONVERT(date,'''+@enddate+''',101)'
     end 
	 end
	 
	IF(@EN = 1)
      BEGIN
          SET @Query+=' and UC.IsSel = 1 and UC.UserID ='+convert(nvarchar(50),@UserID)                    
      END  	             
      if(@rol <> 0)
     begin		
		set @Query +=' and  l.rol = '+convert(varchar(25),@rol)    
     end     
               
if(@SearchBy<>'')
begin
	if(@SearchBy = 'r.Name' or @SearchBy = 'l.fdesc' or  @SearchBy= 'l.CompanyName' or @SearchBy = 'r.Phone' or @SearchBy = 'r.Cellular' or @SearchBy = 'r.City' )
	begin
		set @Query +=' and ' +@SearchBy +' like '''+@SearchValue+'%'''
	end
	else if(@SearchBy = 'r.State' or @SearchBy = 'l.type' or @SearchBy = 'l.Probability')
	begin
		set @Query +=' and ' +@SearchBy +' = '''+@SearchValue+''''
	end
	else if(@SearchBy = 'l.status')
	begin
		set @Query +=' and ' +@SearchBy +' = '+@SearchValue
	end
	else if(@SearchBy = 'r.Address' or @SearchBy = 'r.Email')
	begin
		set @Query +=' and ' +@SearchBy +' like ''%'+@SearchValue+'%'''
	end	
	else if(@SearchBy = 'l.rol')
	begin
		set @Query +=' and ' +@SearchBy +' = '+@SearchValue
	end
	else if(@SearchBy = 'l.fuser')
	begin
		set @Query +=' and ' +@SearchBy +' = '''+@SearchValue+''''
	end
	else if(@SearchBy = 'l.ID'or @SearchBy = 'l.estimate')
	begin
		set @Query +=' and ' +@SearchBy +' = '''+@SearchValue+''''
	end

	else if(@SearchBy = 'job')
	begin
		set @Query +=' and (select job from Estimate where id = l.Estimate)  = '''+@SearchValue+''''
	end
	 
end

IF( @IsSalesAsigned > 0  )

      BEGIN
      SET @Query+=' and  l.AssignedToID = '+ CONVERT(NVARCHAR(10), (@IsSalesAsigned)) 
      END 
              
set @Query +=' ORDER  BY l.CreateDate, Ltrim(Rtrim(r.Name)) '

exec( @Query)