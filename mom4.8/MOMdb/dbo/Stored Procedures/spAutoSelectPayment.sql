CREATE  PROCEDURE [dbo].[spAutoSelectPayment]
	@UpdateBy varchar(100),
	@UpdateByValue date,
	@IsVH bit ,
	@IsDisc bit
AS
BEGIN
declare @text as varchar(1000)	
declare @PJID as int
create table #temp(
PJID int)
set @text='SELECT OpenAP.PJID FROM ((OpenAP INNER JOIN Vendor ON OpenAP.Vendor = Vendor.ID) INNER JOIN PJ ON OpenAP.PJID = PJ.ID) INNER JOIN Rol ON Vendor.Rol = Rol.ID WHERE Vendor.Status =0 and OpenAp.Type = 0 AND OpenAp.Original<>OpenAp.Selected+OpenAp.Disc '
if(@UpdateBy='Due Date')
set @text= @text +'AND OpenAP.Due<=''' + CONVERT(VARCHAR(50), @UpdateByValue)+ ''''
if(@UpdateBy='Dated')
set @text= @text +'AND OpenAP.fDate<=''' + CONVERT(VARCHAR(50), @UpdateByValue)+ ''''
if(@UpdateBy='Due Before')
set @text= @text +'AND OpenAP.Due<=''' + CONVERT(VARCHAR(50), @UpdateByValue)+ ''''
if(@IsVH=1)
set @text= @text + 'AND PJ.Spec>3'
SET NOCOUNT ON;

BEGIN TRY
insert into #temp 
exec(@text)

DECLARE db_cursor CURSOR local FOR 
select * from #temp
open db_cursor

fetch next from db_cursor into @PJID

while @@FETCH_STATUS = 0
BEGIN
UPDATE OpenAP SET OpenAP.IsSelected=0 WHERE OpenAP.PJID=@PJID 
if(@IsDisc=1)
begin 
--UPDATE OpenAP SET OpenAP.Disc=0, OpenAP.Selected= Case when balance =0.00 then OpenAP.Original
--else OpenAp.Balance end WHERE OpenAP.PJID=@PJID
UPDATE OpenAP SET OpenAP.IsSelected=1 WHERE OpenAP.PJID=@PJID AND (SELECT Spec FROM PJ WHERE ID = @PJID) = 0
end
else if(@UpdateBy='Clear')
begin 
--UPDATE OpenAP SET OpenAP.Disc=0, OpenAP.Selected=0 WHERE OpenAP.PJID=@PJID
UPDATE OpenAP SET OpenAP.IsSelected=0, Balance = ISNULL(Original,0)-(ISNUll(Selected,0)+ISNUll(Disc,0)) WHERE OpenAP.PJID=@PJID 
end
else
begin
--UPDATE OpenAP SET OpenAP.Disc=0, OpenAP.Selected= Case when balance=0.00 then OpenAP.Original
--else OpenAp.Balance end
--WHERE OpenAP.PJID=@PJID
if(@IsVH=1)
BEGIN
UPDATE OpenAP SET OpenAP.IsSelected=1,Balance=0.00 WHERE OpenAP.PJID=@PJID AND (SELECT Spec FROM PJ WHERE ID = @PJID) > 3
END
ELSE
BEGIN
UPDATE OpenAP SET OpenAP.IsSelected=1,Balance=0.00 WHERE OpenAP.PJID=@PJID AND (SELECT Spec FROM PJ WHERE ID = @PJID) = 0
END
end
fetch next from db_cursor into @PJID
END
close db_cursor
deallocate db_cursor

if(@UpdateBy='Clear')  
begin   
--UPDATE OpenAP SET OpenAP.Disc=0, OpenAP.Selected=0 WHERE OpenAP.PJID=@PJID  
UPDATE OpenAP SET OpenAP.IsSelected=0 WHERE OpenAp.Original=OpenAp.Selected AND OpenAp.Balance=0
end  

SELECT r.Name, o.Vendor, 
       o.fDate, 
       o.Due, 
       o.Type, 
       o.fDesc, 
       o.Original, 
       --o.Balance, 
	   o.Original - (o.Selected + o.Disc) as Balance,
       o.Selected + o.Disc as Selected,
       --o.Selected, 
       o.Disc, 
       o.PJID, 
       o.TRID, 
       CASE WHEN p.Disc = 0 THEN 0  WHEN p.Disc > 0  AND GETDATE() <= DATEADD(D,ISNULL(IfPaid,0),o.fDate) THEN ISNull(o.Original,0)*ISNull(p.Disc,0)/100 ELSE 0 END as Discount,
       o.Ref, 
       p.Status, p.Spec, (CASE p.Spec WHEN 0 THEN 'Input Only'        WHEN 1 THEN 'Hold - No Invoices'        WHEN 2 THEN 'Hold - No Materials'        WHEN 3 THEN 'Hold - Other'        WHEN 4 THEN 'Verified'        WHEN 5 THEN 'Selected' END) as StatusName, 
      '0.00' AS Payment,       p.fDesc AS billDesc  , IsNull(o.IsSelected,0)   As IsSelected ,v.[Type] AS VendorType  FROM OpenAP o, PJ p ,vendor v  LEFT JOIN Rol r ON r.ID = v.Rol
	   WHERE p.ID=o.PJID and o.Vendor=v.id and o.isSelected=1
      AND o.PJID in(select PJID from #temp)
	  group by  r.Name,o.Vendor ,o.fdate, 
       o.Due, 
       o.Type, 
       o.fDesc, 
       o.Original, 
       o.Balance, 
       o.Selected, 
       o.Disc, 
       o.PJID, 
       o.TRID, 
       o.Disc,
       o.Ref, 
       p.Status, p.Spec, p.fDesc ,o.isSelected,p.Disc,p.IfPaid,v.[Type]
	   SELECT count(OpenAP.Ref) as NCount, Sum(OpenAP.Selected) as NAmt FROM OpenAP INNER JOIN (Vendor INNER JOIN Rol ON Vendor.Rol = Rol.ID) ON OpenAP.Vendor = Vendor.ID WHERE OpenAP.IsSelected=1
	END TRY
	BEGIN CATCH
	 DECLARE @ErrorMessage NVARCHAR(4000);
	 SELECT @ErrorMessage = ERROR_MESSAGE()

    IF @@TRANCOUNT>0
        ROLLBACK	
		RAISERROR (@ErrorMessage,16,1)
        RETURN

END CATCH
DROP TABLE #temp
END
