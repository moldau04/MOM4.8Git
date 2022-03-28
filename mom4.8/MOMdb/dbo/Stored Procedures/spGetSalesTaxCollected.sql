create Procedure [dbo].[spGetSalesTaxCollected] (          
	@fromDate   DATETIME,
	@toDate   DATETIME           
)
AS 
BEGIN
select I.fDate,
       I.Ref,	
       l.tag AS locName,
       r.Name AS CustName,
       I.fDesc,
       I.Amount,
       I.Stax,
       I.Total,
       I.TaxRegion,
       I.TaxRate,
       I.TaxFactor,
       I.Taxable,
       L.ID,
       S.State,
       S.fDesc AS sDesc,
       S.Name AS sName,
       S.Rate AS sRate ,
	   isnull((select sum(ISNULL(Amount,0)* -1) from trans t2 where t2.type =99 and t2.Ref=I.Ref and t2.fdate>=@fromDate and t2.fdate<=@toDate),0) as Paid
	   from Invoice I
left JOIN Loc L ON L.Loc = I.Loc
left JOIN OWNER o ON o.id=l.owner
left JOIN rol r ON r.id=o.Rol
left JOIN Trans t ON t.ref = I.Ref
inner JOIN STax S ON I.TaxRegion = S.Name
where 
t.fdate>=@fromDate and t.fdate<=@toDate and t.Type=99 AND S.UType=0
order by I.Ref
END