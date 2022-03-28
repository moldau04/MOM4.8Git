CREATE Procedure [dbo].[spGetInvoicesPaging]
	@strSdate Datetime,
	@strEdate Datetime,
	@SearchBy varchar(200),
	@SearchValue varchar(200),	
	@EN bit,
	@isGridFilterInvoice bit,
	@UserID Int ,
	@CustID Int =0,
	@Loc Int=0,
	@jobid Int=0,
	@Paid Int=0,
	@RoleId int=0,
	@isShowAll bit,
	@SearchAmtPaidUnpaid varchar(200),
	@SearchPrintMail varchar(200),
	@FilterByColumn as dbo.tblTypeFilterByColumn READONLY,
	@PageNumber Int = 1,
	@PageSize Int = 50,
	@SortOrderBy varchar(200) ='',
	@SortType varchar(200) ='desc'
AS
BEGIN
	IF OBJECT_ID('tempdb..#tempInvoice') IS NOT NULL DROP TABLE #tempInvoice
	Create Table #tempInvoice(	
		REF         INT , 
		RowNumber int,
		TotalRow INt
	)
	
	if @SortOrderBy='' 	BEGIN
		SET @SortOrderBy=' i.Ref '
	END
	if LOWER(@SortOrderBy)='ref' 	BEGIN
		SET @SortOrderBy=' i.Ref '
	END
	if LOWER(@SortOrderBy)='paymentreceiveddate'
	BEGIN
		SET @SortOrderBy=' rp.PaymentReceivedDate '
	END
	if LOWER(@SortOrderBy)='sdesc'
	BEGIN
		SET @SortOrderBy=' te.SDesc '
	END
	if LOWER(@SortOrderBy)='fDate'
	BEGIN
		SET @SortOrderBy=' i.fDate '
	END
	if LOWER(@SortOrderBy)='loc'
	BEGIN
		SET @SortOrderBy='l.Loc '
	END
	if LOWER(@SortOrderBy)='id'
	BEGIN
		SET @SortOrderBy='l.ID '
	END
	if LOWER(@SortOrderBy)='tag'
	BEGIN
		SET @SortOrderBy=' l.Tag '
	END
	
	if LOWER(@SortOrderBy)='fdesc'
	BEGIN
		SET @SortOrderBy=' i.fdesc '
	END

	if LOWER(@SortOrderBy)='job'
	BEGIN
		SET @SortOrderBy=' i.Job '
	END
	--The text, ntext, and image data types cannot be compared or sorted, except when using IS NULL or LIKE operator.
      if LOWER(@SortOrderBy)='locremarks'
	BEGIN
		--SET @SortOrderBy=' l.Remarks '
		SET @SortOrderBy=' i.loc '
	END 
	
      if LOWER(@SortOrderBy)='jobremarks'
	BEGIN
		--SET @SortOrderBy=' j.Remarks '
		SET @SortOrderBy=' i.Job '
	END   
         if LOWER(@SortOrderBy)='amount'
	BEGIN
		SET @SortOrderBy=' i.Amount '
	END      
          if LOWER(@SortOrderBy)='stax'
	BEGIN
		SET @SortOrderBy=' i.STax '
	END     
           if LOWER(@SortOrderBy)='Total'
	BEGIN
		SET @SortOrderBy=' i.Total '
	END 
	        if LOWER(@SortOrderBy)='status'
	BEGIN
		SET @SortOrderBy=' i.status '
	END 
            if LOWER(@SortOrderBy)='type'
	BEGIN
		SET @SortOrderBy=' jt.Type '
	END

	if LOWER(@SortOrderBy)='batch'
	BEGIN
		SET @SortOrderBy=' i.Batch '
	END

	if LOWER(@SortOrderBy)='createdby'
	BEGIN
		SET @SortOrderBy=' i.fUser '
	END

	if LOWER(@SortOrderBy)='jobstatus'
	BEGIN
		SET @SortOrderBy=' j.Status '
	END        
if LOWER(@SortOrderBy)='customername'
	BEGIN
		SET @SortOrderBy=' r.Name '
	END   
if LOWER(@SortOrderBy)='balance'
	BEGIN
		SET @SortOrderBy=' ar.Balance '
	END  

	if LOWER(@SortOrderBy)='po'
	BEGIN
		SET @SortOrderBy=' i.PO '
	END 

	--Validate pagination parameters
	IF(@PageNumber IS NULL Or @PageNumber <= 0) SET @PageNumber = 1
	IF(@PageSize IS NULL Or @PageSize <= 0) SET @PageSize = 50
	
	--Calculate start and end row to return
	Declare @StartRow Int = ((@PageNumber - 1) * @PageSize) + 1      
	Declare @EndRow Int = @PageNumber * @PageSize

	Declare @sql varchar(max)
	set @sql='Insert into #tempInvoice(Ref,RowNumber,TotalRow)'
	set @sql= @sql + ' SELECT '
	set @sql= @sql + ' 	i.Ref as Ref '	
	set @sql= @sql + ' 	, ROW_NUMBER() OVER(Order By '+@SortOrderBy+' ' + @SortType +' ) RowNumber, COUNT(1) OVER() TotalRow '
	set @sql= @sql + ' FROM   Invoice i '
	set @sql= @sql + '   LEFT OUTER JOIN Terr te '
	set @sql= @sql + ' 		   ON te.ID = i.AssignedTo '
	set @sql= @sql + '   INNER JOIN Loc l '
	set @sql= @sql + ' 		   ON l.Loc = i.Loc '
	set @sql= @sql + '   INNER JOIN owner o '
	set @sql= @sql + ' 		   ON o.id = l.owner '
	set @sql= @sql + '   INNER JOIN rol r '
	set @sql= @sql + ' 		   ON o.rol = r.id '
	--set @sql= @sql + ' 		LEFT OUTER JOIN PaymentDetails p ON p.InvoiceID = i.Ref AND IsInvoice = 1 '
	--set @sql= @sql + ' 		   LEFT OUTER join ReceivedPayment rp ON rp.ID = p.ReceivedPaymentID '
	set @sql= @sql + '   LEFT OUTER JOIN Branch B on B.ID = r.EN '
	set @sql= @sql + '   LEFT OUTER JOIN tblInvoicePayment ip '
	set @sql= @sql + ' 		   ON i.ref = ip.ref '
	set @sql= @sql + '   LEFT OUTER JOIN Job j ON i.Job=j.ID '
	set @sql= @sql + '   LEFT JOIN OpenAR ar  '
	set @sql= @sql + ' 		   ON ar.Ref = i.Ref AND ar.Type = 0   '
	set @sql= @sql + '   LEFT JOIN WIPHeader wip  '
	set @sql= @sql + ' 		   ON wip.InvoiceId=i.Ref  '
	set @sql= @sql + '   LEFT JOIN JobType jt  '
	set @sql= @sql + ' 		   ON jt.ID=i.Type  '

	
	set @sql= @sql + '   WHERE  i.ref is not null '
if (@EN = 1)
BEGIN
	set @sql= @sql + '   and UC.IsSel = 1 and UC.UserID = ' + CONVERT(VARCHAR(50),@UserID)  
END
-- Search by
if (@SearchBy != '')
	BEGIN

		if (@SearchBy  = 'i.fdate')
			BEGIN
				-- Comment this code because
				--set @sql= @sql + '   and ' +@SearchBy + ' between ''' + CONVERT(VARCHAR(50),@strSdate) + ''' and ''' + CONVERT(VARCHAR(50),@strEdate) + ''' '	
				set @sql= @sql + '    and ' + 	 @SearchBy  +' = ''' + @SearchValue + ''' '			
			END
		ELSE
			BEGIN
				IF (@SearchBy ='l.owner')
					BEGIN
						set @sql= @sql + '    and ' + 	 @SearchBy  +' = ''' + @SearchValue + ''' '				 
					END
				ELSE	
					BEGIN
						if (@SearchBy = 'i.loc')
							BEGIN
							 set @sql= @sql + '    and ' + 	 @SearchBy  +' = ''' + @SearchValue + ''' '	
							END
						ELSE
							BEGIN
								if (@SearchBy = 'l.loc')
									BEGIN
										set @sql= @sql + '    and ' + 	 @SearchBy  +' = ''' + @SearchValue + ''' '	
									END
								ELSE	
									BEGIN
										if (@SearchBy = 'i.ref')
											BEGIN											
												set @sql= @sql + '    and ' + 	 @SearchBy  + @SearchValue 
												 
											END
										ELSE
											BEGIN
												if (@SearchBy = 'i.Type')
												BEGIN
													IF (@SearchValue != '')
														BEGIN
															set @sql= @sql + '    and ' + 	 @SearchBy  +' = ''' + @SearchValue + ''' '	
														END
													ELSE
														BEGIN												
															set @sql= @sql + '    and ' + 	 @SearchBy  +' = -1' 	
														END
												END
												ELSE
													BEGIN												
														set @sql= @sql + '    and ' + 	 @SearchBy  +' like ''%' 	+ @SearchValue + '%'' '														
													END
											END
									END
							END
						
					END
			END          
					   
		END

-- NEED to check again 
   -- if (@isFilterHasCommaDelimited=0)
   -- BEGIN
		-- if (@StartDate != System.DateTime.MinValue)
			-- BEGIN
				-- varname1.Append(" and i.fdate >='" + objPropContracts.StartDate + "'\n");
			-- END
		-- if (objPropContracts.EndDate != System.DateTime.MinValue)
			-- BEGIN
				-- varname1.Append(" and i.fdate <'" + objPropContracts.EndDate.AddDays(1) + "'");
			-- END
   -- END              
		
if (@CustID != 0)
BEGIN
	set @sql= @sql + '  and l.owner =' + CONVERT(VARCHAR(50),@CustID) 
END 

if (@Loc != 0)
BEGIN	
	set @sql= @sql + '  and l.loc =' + CONVERT(VARCHAR(50),@Loc) 
END
if (@jobid != 0)
BEGIN	
	set @sql= @sql + '  and l.job =' + CONVERT(VARCHAR(50),@jobid) 
END
if (@Paid = 1)
BEGIN
	set @sql= @sql + '  and i.ref not in (SELECT isnull(InvoiceID,0) FROM  tblPaymentHistory where Approved=''sent'' and isnull(PayType,'''')=''ACH'' )  '
	set @sql= @sql + '  and isnull( ip.paid,0) = 0 and i.status = 0'
END
if (@RoleId != 0)
BEGIN
	set @sql= @sql + '  and isnull(l.roleid,0)= ' + CONVERT(VARCHAR(50),@RoleId) 	
END
if (@SearchAmtPaidUnpaid<>'')
BEGIN	
	set @sql= @sql + '   and i.Status in ' + CONVERT(VARCHAR(50),@SearchAmtPaidUnpaid) 
END
if (@SearchPrintMail<>'')
BEGIN
	if (Lower(LTRIM(RTRIM(@SearchPrintMail))  ) = 'p')
   BEGIN
	
		set @sql= @sql + '   and l.PrintInvoice =''True'' ' 
	END
	else 
	BEGIN
		IF (Lower( LTRIM(RTRIM(@SearchPrintMail))) = 'm')
		BEGIN			
			set @sql= @sql + '   and l.EmailInvoice =''True'' ' 
		END
	END
END

-- FILTER ON GRID
DECLARE @ColName varchar(100)
DECLARE @ColValue varchar(500)
DECLARE cur_Filter CURSOR FOR 	
		
	SELECT ColumnName,ColumnValue FROM  @FilterByColumn
	
	OPEN cur_Filter  
	FETCH NEXT FROM cur_Filter INTO @ColName,@ColValue
	WHILE @@FETCH_STATUS = 0  
		BEGIN
		
			 if ( LTRIM(RTRIM(@ColValue)) <>'')
				BEGIN
				
					IF @ColName='InvoiceRef' 
					BEGIN
						set @isGridFilterInvoice =1
						set @sql= @sql + '    and i.ref  in (' + @ColValue + ') '
					END					
							
					IF @ColName='ManualInv' 
					BEGIN	
						set @sql= @sql + '    and i.custom1 like ''%' + REPLACE(@ColValue,'''','''''') + '%'' '
					END

					IF @ColName='ID' 
					BEGIN	
						set @sql= @sql + '    and l.ID like ''%' + REPLACE(@ColValue,'''','''''') + '%'' '		
					END

					IF @ColName='Tag' 
					BEGIN	
						set @sql= @sql + '    and l.Tag like ''%' + REPLACE(@ColValue,'''','''''') + '%'' '				
					END
						
					IF @ColName='Amount' 
					BEGIN	
						set @sql= @sql + '    and i.Amount =' + 	CONVERT(VARCHAR(50),@ColValue) 
					END

					--IF @ColName='SalesTax' 
					--BEGIN	
					--	set @sql= @sql + '    and (i.STax + ISNULL(i.GTax,0))  =' + 	CONVERT(VARCHAR(50),@ColValue) 
					--END

					IF @ColName='GSTTax' 
					BEGIN	
						set @sql= @sql + '    and ( ISNULL(i.GTax,0))  =' + 	CONVERT(VARCHAR(50),@ColValue) 
					END

						IF @ColName='SalesTax' 
					BEGIN	
						set @sql= @sql + '    and (i.STax)  =' + 	CONVERT(VARCHAR(50),@ColValue) 
					END


					IF @ColName='Total' 
					BEGIN	
						set @sql= @sql + '    and i.Total =   ' + 	CONVERT(VARCHAR(50),@ColValue) 
					END
							
					IF @ColName='Status' 	
					BEGIN
						set @ColValue= REPLACE(@ColValue,'open', '0')
						set @ColValue= REPLACE(@ColValue,'voided', '2')
						set @ColValue= REPLACE(@ColValue,'marked as pending', '4')
						set @ColValue= REPLACE(@ColValue,'paid by credit card', '5')
						set @ColValue= REPLACE(@ColValue,'partially paid', '3')
						set @ColValue= REPLACE(@ColValue,'paid', '1')
						set @sql= @sql + '    and i.status  in (' + @ColValue + ') '	
					END
										
					IF @ColName='PO' 
					BEGIN	
							set @sql= @sql + '    and i.PO like ''%' + REPLACE(@ColValue,'''','''''') + '%'' '	
					END
				
					IF @ColName='Job' 
					BEGIN	
					
							set @sql= @sql + '    and i.Job =' + 	CONVERT(VARCHAR(50),@ColValue) 	
					END
										
					IF @ColName='CustomerName' 
					BEGIN	
							set @sql= @sql + '    and r.Name like ''%' + REPLACE(@ColValue,'''','''''') + '%'' '	
					END
					
					IF @ColName='DType' 
					BEGIN	
							set @sql= @sql + '    and jt.Type like ''%' + REPLACE(@ColValue,'''','''''') + '%'' '	
					END
					
					
					IF @ColName='SDesc' 
					BEGIN	
							set @sql= @sql + '    and te.SDesc like ''%' + REPLACE(@ColValue,'''','''''') + '%'' '	
					END

					IF @ColName='LocRemarks' 
					BEGIN	
							set @sql= @sql + '    and l.Remarks like ''%' + REPLACE(@ColValue,'''','''''') + '%'' '	
					END

					IF @ColName='JobRemarks' 
					BEGIN	
							set @sql= @sql + '    and j.Remarks like ''%' + REPLACE(@ColValue,'''','''''') + '%'' '	
					END
					
					IF @ColName='Balance' 
					BEGIN	
							set @sql= @sql + '    and ar.Balance =' + 	CONVERT(VARCHAR(50),@ColValue) 	
					END

					if LOWER(@ColName)='createdby'
					BEGIN					
						set @sql= @sql + '    and i.fUser like ''%'+  + REPLACE(@ColValue,'''','''''') + '%'' '	
					END
				END
				FETCH NEXT FROM cur_Filter INTO @ColName,@ColValue
		END   
			
	CLOSE cur_Filter  
	DEALLOCATE cur_Filter  

	IF @isGridFilterInvoice =0
	BEGIN
	    IF @isShowAll=0
		BEGIN
			set @sql= @sql + ' and i.fdate >=''' +CONVERT(VARCHAR(50),@strSdate) +''' '
			set @sql= @sql + ' and i.fdate <=''' +CONVERT(VARCHAR(50),@strEdate) +''' '
		END
	END	
	SET @sql=@sql +' Order By '+@SortOrderBy+' ' + @SortType 
	--select (@sql)
	exec (@sql)

	

	 SELECT RowNumber,i.Ref AS REF,
           CONVERT(varchar(50), (select top 1  PaymentReceivedDate from ReceivedPayment where ID in(select ReceivedPaymentID from PaymentDetails where InvoiceID=i.Ref AND IsInvoice = 1) order by PaymentReceivedDate desc), 101) AS PaymentReceivedDate,
          te.SDesc,
          CONVERT(varchar(50), i.fDate, 101) AS fDate,
          l.Loc,
          l.ID,
          l.Tag,
          i.fdesc,
          i.Job,
          isnull(l.Remarks, '') AS locRemarks,
          isnull(j.Remarks, '') AS JobRemarks,
          i.Amount,
          i.STax+ISNULL(i.GTax, 0) AS STax,
		   i.STax AS PSTTax,
		    i.GTax AS GSTTax,
          i.Total,
          isnull(i.status, 0) AS InvStatus,
          i.custom1 AS manualInv,
          (CASE i.status
               WHEN 0 THEN 'Open'
               WHEN 1 THEN 'Paid'
               WHEN 2 THEN 'Voided'
               WHEN 4 THEN 'Marked as Pending'
               WHEN 5 THEN 'Paid by Credit Card'
               WHEN 3 THEN 'Partially Paid'
           END + CASE isnull(ip.paid, 0)
                     WHEN 1 THEN '/Paid by MOM'
                     ELSE ''
                 END) AS status,
          r.EN,
          isnull(B.Name, '') AS Company,
          i.PO,
          r.Name AS customername,
          jt.Type AS TYPE,
          i.Batch,
          CASE isnull(i.status, 0)
              WHEN 1 THEN 0
              ELSE convert(numeric(30, 2), (isnull(i.total, 0) - isnull(ip.balance, 0)))
          END AS Invbalance,
          isnull(ar.Balance, 0) AS balance,
          ar.due AS ddate,
          DATEADD(dd, -1, DATEADD(wk, DATEDIFF(wk, 0, ar.due), 0)) AS WeekDate,
          isnull(wip.id, 0) AS WipInvoice,
          isnull(j.Status, 0) AS JobStatus,
          isnull(i.fUser, '') AS CreatedBy,TotalRow ,
		   isnull(i.IsRecurring, 0) AS IsRecurring,
		   i.Remarks AS  Remarks
		  from #tempInvoice temp
		  inner join Invoice i on temp.Ref=i.Ref
 LEFT OUTER JOIN Terr te ON te.ID = i.AssignedTo
   INNER JOIN Loc l ON l.Loc = i.Loc
   INNER JOIN OWNER o ON o.id = l.owner
   INNER JOIN rol r ON o.rol = r.id
   --LEFT OUTER JOIN PaymentDetails p ON p.InvoiceID = i.Ref
   --AND IsInvoice = 1
   --LEFT OUTER JOIN ReceivedPayment rp ON rp.ID = p.ReceivedPaymentID
   LEFT OUTER JOIN Branch B ON B.ID = r.EN
   LEFT OUTER JOIN tblInvoicePayment ip ON i.ref = ip.ref
   LEFT OUTER JOIN Job j ON i.Job=j.ID
   LEFT JOIN OpenAR ar ON ar.Ref = i.Ref
   AND ar.Type = 0
   LEFT JOIN WIPHeader wip ON wip.InvoiceId=i.Ref
   LEFT JOIN JobType jt ON jt.ID=i.Type
   
WHERE RowNumber BETWEEN @StartRow AND @EndRow
order by RowNumber
 SELECT
         isnull( SUM(i.Amount),0) AS TotalAmount,		  
         isnull( SUM(i.STax+ISNULL(i.GTax, 0)),0) AS TotalSTax,
		  isnull( SUM(i.STax),0) AS TotalPSTTax,
		   isnull( SUM(i.GTax),0) AS TotalGSTTax,
          isnull(SUM(i.Total),0) AS TotalAll,      
		  isnull(sum( isnull(ar.Balance, 0)),0) AS TotalBalance
		  ,isnull(sum( CASE isnull(i.status, 0)
              WHEN 1 THEN 0
              ELSE convert(numeric(30, 2), (isnull(i.total, 0) - isnull(ip.balance, 0)))

          END),0) AS TotalInvbalance
		  from #tempInvoice temp
		  inner join Invoice i on temp.Ref=i.Ref
	LEFT OUTER JOIN Terr te ON te.ID = i.AssignedTo
   INNER JOIN Loc l ON l.Loc = i.Loc
   INNER JOIN OWNER o ON o.id = l.owner
   INNER JOIN rol r ON o.rol = r.id  
   LEFT OUTER JOIN Branch B ON B.ID = r.EN
   LEFT OUTER JOIN tblInvoicePayment ip ON i.ref = ip.ref
   LEFT OUTER JOIN Job j ON i.Job=j.ID
   LEFT JOIN OpenAR ar ON ar.Ref = i.Ref
   AND ar.Type = 0
   LEFT JOIN WIPHeader wip ON wip.InvoiceId=i.Ref
   LEFT JOIN JobType jt ON jt.ID=i.Type



IF OBJECT_ID('tempdb..#tempInvoice') IS NOT NULL DROP TABLE #tempInvoice
END

