CREATE PROCEDURE [dbo].[spGetCheckDetails] 
	@sdate datetime,
	@edate datetime,
	@searchterm varchar(100) ,
	@searchvalue varchar(100),
	@EN int,
	@UserID int		= 0

AS
BEGIN
	declare @Text varchar(max)
	SET NOCOUNT ON;
	set @Text='
   select 
	c.ID, 
	c.TransID, 
	c.fDate, 
	c.Ref, 
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
	c.Vendor, 
	c.French, 
	c.Memo, 
	c.VoidR, 
	c.ACH, 
	isnull(tt.Sel,0) as Sel,
	tt.Type,
	case when isnull(tt.sel,0) = 0 then ''Open''
		 when tt.sel = 1 then ''Cleared''
		 when tt.sel = 2 then ''Voided''
		 end as StatusName,
		 --0 as Sel,0 as Type, ''Open'' as StatusName,
		 --(SELECT TOP 1 isnull(Sel,0) FROM TRANS WHERE BATCH IN (SELECT BATCH FROM TRANS WHERE ID = c.TransID) AND type = 20 AND ISNULL(AcctSub,0) = c.Vendor)  as Sel,
		 --(SELECT TOP 1 isnull(Type,0) FROM TRANS WHERE BATCH IN (SELECT BATCH FROM TRANS WHERE ID = c.TransID) AND type = 20 AND ISNULL(AcctSub,0) = c.Vendor)  as Type,
		 --(SELECT TOP 1 case when isnull(Sel,0) = 0 then ''Open'' when isnull(Sel,1) = 1 then ''Cleared'' when isnull(Sel,2) = 2 then ''Voided'' END FROM TRANS WHERE BATCH IN (SELECT BATCH FROM TRANS WHERE ID = c.TransID) AND type = 20 AND ISNULL(AcctSub,0) = c.Bank)  as StatusName,

		 r.EN,Br.Name As Company
	
from CD as c 
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
				select DISTINCT ct.ID, t.batch, t.sel, t.type
					from trans t
					inner join (
					select c.ID, t.batch 
					from trans t inner join cd c on t.ID = c.TransID
					) ct on 
					ct.Batch = t.Batch
					--ct.ID = t.ID
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

	print @text
	 exec(@text)
END
