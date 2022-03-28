CREATE PROCEDURE [dbo].[spGetJobCostInvoicesByJob]
	@job int,
	@phase int,
	@type smallint,				-- 0 Revenue, 1 Expense jobtitem.type
	@sdate datetime = null,
	@edate datetime = null
AS
BEGIN
	
	SET NOCOUNT ON;

	
	declare @text varchar(max)

	set @text = 'select   ( select top 1 GroupName from tblEstimateGroup where ID= jobitem.GroupId ) as GroupName, jobi.Job, jobi.Phase,  jobitem.code+'' : ''+jobitem.fdesc as Code,  
					jobi.Type, 
					isnull((case jobi.Type when 0 then ''Revenue'' when 1 then ''Cost'' end),'''') as TypeName, 
					isnull(b.Type,0)																	as BTypeID, 
					isnull(m.Type,0)																	as MTypeID, 
					isnull(o.Department,'''')															as MTypeDesc,
					isnull(b.MatItem,0)																	as MatItem, 
					isnull(i.Name,'''')																	as MatDesc,

					(case jobi.Type when 0 then 0 else isnull(b.Type,0) end)							as TypeID,
					(case jobi.Type when 0 then ''Revenue'' else isnull(bt.Type, '''') end)				as TypeDesc, 
					(case jobi.Type when 0 then isnull(m.Type,0) else isnull(b.MatItem,0) end)			as ItemID,
					(case jobi.type when 0 then isnull(o.Department,'''') else isnull(i.Name,'''') end) as ItemDesc,
					
					isnull(pj.ID, 0)																	as BillID, 
					isnull((case jobi.type when 0 then jobi.Ref else 0 end),0)							as InvoiceID, 
					jobi.fDate, isnull(jobi.Ref, '''')													as Ref, 
					isnull(r.Name, '''')																as VendorName, 
					jobi.fDesc, 
					jobi.Amount, 
					jobi.TransID,
					jobi.Amount																			as Actual, 
					isnull(jobitem.Budget,0)															as Budget,
					isnull(jobitem.Budget,0)+isnull(jobitem.Modifier,0) as BMatExp,
					isnull(jobitem.ETC,0)+isnull(jobitem.ETCMod,0) as BLabExp,
					(case jobi.type when 0 then ''addinvoice.aspx?uid=''+ jobi.Ref else ''addbills.aspx?id=''+ convert(varchar(50),pj.ID) end) as Url
						 from jobi
							inner join jobtitem jobitem on jobi.Job = jobitem.Job AND jobi.Phase = jobitem.Line AND jobi.Type = jobitem.Type
							left join bom b on b.JobTItemID = jobitem.ID 
							left join bomt bt on bt.ID = b.Type
							left join Inv i on i.ID = b.MatItem 
							left join pj on pj.Batch = isnull((select top 1 batch from trans where id = jobi.TransID),0)
							left join Vendor v on v.ID = pj.Vendor
						   left join Rol r on r.ID = v.Rol
							left join milestone m on m.JobTItemID = jobitem.ID 
							left join OrgDep o on o.ID = m.Type
							
				
							WHERE		jobi.TransID > 0
									AND jobi.Job = '''+ convert(varchar(50),@job) +'''
									AND jobitem.Type = '''+ convert(varchar(50),@type) +'''
									AND jobitem.line = ''' + CONVERT(VARCHAR(50),@phase) +''''

					IF(@sdate IS NOT NULL and @edate IS NOT NULL and    @edate <> '1/1/0001 12:00:00 AM')
					BEGIN
						SET @text +=' AND jobi.fDate >= '''+ convert(varchar(50),@sdate) +'''
									  AND jobi.fDate <= '''+ convert(varchar(50),@edate) +''' '
					END

					SET @text +='	ORDER BY jobi.fDate, jobi.Ref '

	exec (@text)
END