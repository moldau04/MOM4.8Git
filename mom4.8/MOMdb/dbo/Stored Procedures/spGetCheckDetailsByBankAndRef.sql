CREATE PROCEDURE [dbo].[spGetCheckDetailsByBankAndRef]  
 @Bank INT,   
 @Ref1 BIGINT,  
 @Ref2 BIGINT  
AS  
BEGIN  
 SET NOCOUNT ON;  
  
 SELECT p.Ref,p.fDate as InvoiceDate,p.ref as Refrerence,p.Amount as Total,pd.Disc,pd.Paid as AmountPay,pd.fDate as PayDate,cd.Ref as CheckNo,cd.Vendor, r.[Name] AS VendorName,  
'' As [Type],p.fDesc as Description  
FROM PJ as p INNER JOIN Paid pd ON p.TRID = pd.TRID   
INNER JOIN CD cd ON cd.ID = pd.PITR INNER JOIN Vendor v ON v.ID = cd.Vendor 
--INNER JOIN Phone ph ON v.Rol = ph.Rol 
INNER JOIN Rol r ON r.ID = v.Rol WHERE cd.Bank= @Bank and cd.Ref BETWEEN @Ref1 AND @Ref2 ORDER BY cd.Ref  
  
SELECT cd.Amount as Pay,r.[Name] AS ToOrder,cd.fDate as [Date],cd.Amount as CheckAmount,'' as ToOrerAddress,r.City+', '+r.[State]+', '+ISNULL(r.[Zip],'') as [State],r.[Zip],r.[Address] as VendorAddress,  
v.Remit AS RemitAddress,cd.Memo,cd.Vendor,cd.Ref as CheckNo,cd.[Status]  FROM CD cd INNER JOIN Vendor v ON v.ID = cd.Vendor 
--INNER JOIN Phone ph ON v.Rol = ph.Rol 
INNER JOIN Rol r ON r.ID = v.Rol WHERE cd.Bank = @Bank AND cd.Ref BETWEEN @Ref1 AND @Ref2 ORDER BY cd.Ref  
END  
 
GO