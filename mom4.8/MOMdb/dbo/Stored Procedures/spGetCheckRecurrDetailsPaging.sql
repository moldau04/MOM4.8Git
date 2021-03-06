CREATE PROCEDURE [dbo].[spGetCheckRecurrDetailsPaging] 
	@sdate datetime,
	@edate datetime,
	@searchterm varchar(100) ,
	@searchvalue varchar(100),
	@EN int,
	@UserID int		= 0,
	@PageNumber Int = 1,
	@PageSize Int = 50
AS
BEGIN
	declare @Text varchar(max)

	  --Validate pagination parameters
	IF(@PageNumber IS NULL Or @PageNumber <= 0) SET @PageNumber = 1
	IF(@PageSize IS NULL Or @PageSize <= 0) SET @PageSize = 50
	
	--Calculate start and end row to return
	Declare @StartRow Int = ((@PageNumber - 1) * @PageSize) + 1      
	Declare @EndRow Int = @PageNumber * @PageSize

	SET NOCOUNT ON;
	set @Text='
	SELECT * FROM (
   select 
      ROW_NUMBER() OVER(Order By c.ID,c.fDate,c.Ref) RowNumber, COUNT(1) 
OVER() TotalRow,
   
	c.ID, 
	c.TransID, 
	c.fDate, 
	--c.Ref, 
	c.ID as Ref,
	c.fDesc, 
	c.Amount, 
	c.Vendor, 
	r.Name AS VendorName, 
	c.Bank, 
	b.fDesc AS BankName, 
	t.Batch,
	Case c.Type 
							WHEN 0 THEN ''Check''
							WHEN 1 THEN ''Cash''
							WHEN 2 THEN ''Wire Transfer''
							WHEN 3 THEN ''ACH''
							WHEN 4 THEN ''Credit Card''
							END
							As TypeName, 
	c.Status, 
	--c.Vendor, 
	c.French, 
	c.Memo, 
	c.VoidR, 
	c.ACH, 
	c.Frequency,
	c.PJID,
	isnull(tt.Sel,0) as Sel,
	tt.Type,
	case when isnull(tt.sel,0) = 0 then ''Open''
		 when tt.sel = 1 then ''Cleared''
		 when tt.sel = 2 then ''Voided''
		 end as StatusName,r.EN,Br.Name As Company
	
from CDRecurr as c 
	left join Bank as b on c.Bank = b.ID 
	left join Vendor as v on v.ID = c.Vendor
	left join trans as t on c.TransID = t.ID
	left join Rol as r on r.ID = v.Rol'
	IF(@EN = 1)  
      BEGIN  
          SET @Text +=' left outer join tblUserCo UC on UC.CompanyID = r.EN '
		  END
		   SET @Text += ' left outer join Branch Br on Br.ID = r.EN
	left join (
				select ct.ID, t.batch, t.sel, t.type
					from trans t
					inner join (
					select c.ID, t.batch 
					from trans t inner join cd c on t.ID = c.TransID
					) ct on 
					ct.Batch = t.Batch
					 where type = 20 
		) tt on tt.batch = t.Batch and tt.ID = c.ID
		 WHERE c.fdate >= '''
                       + CONVERT(VARCHAR(50), @sdate + '00:00:00')
                       + ''' AND	c.fdate <='''
                       + CONVERT(VARCHAR(50), @edate + '23:59:59')+ ''''
					   
	if(@searchterm='Vendor')
	 begin 
	 set @Text= @Text+'and r.Name like ''%'+@searchvalue +'%'''
	 end
	  if(@searchterm='Checknum')
	 begin 
	 set @Text=@Text+'and c.ref like ''%'+@searchvalue +'%'''
	 end
	  if(@searchterm='Status')
	 begin 
	 set @Text=@Text+'and tt.Sel=' +@searchvalue 
	 end
	  if(@searchterm='PayType')
	 begin 
	 set @Text=@Text+'and c.Type=' +@searchvalue 
	 end
	  if(@searchterm='Bank')
	 begin 
	 set @Text=@Text+'and b.fDesc like ''%'+@searchvalue +'%'''
	 end
	   if(@searchterm='Amount')
	 begin 
	 set @Text=@Text+'and c.Amount ='+ @searchvalue
	 end
	  if(@EN=1)
	 begin 
	 set @Text=@Text+'and UC.IsSel = 1 and UC.UserID ='+convert(nvarchar(50),@UserID) 
	 end
	 set @Text = @Text+') AS p WHERE p.RowNumber BETWEEN '+CONVERT(NVARCHAR(50), @StartRow)+' And '+CONVERT(NVARCHAR(50), @EndRow)
	 print @Text
	 exec(@text)
END
