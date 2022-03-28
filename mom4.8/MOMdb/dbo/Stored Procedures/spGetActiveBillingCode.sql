CREATE Procedure [dbo].[spGetActiveBillingCode] 

AS
BEGIN

SELECT Inv.id AS id,
          Inv.Name AS Name,
          Inv.fDesc AS fDesc,
          Inv.type AS TYPE,
          isnull(Hand, 0) Hand,
          CASE Inv.type
              WHEN 1 THEN Inv.Name +' : Service'
              WHEN 0 THEN Inv.Name+ ' : Part'
              ELSE Inv.Name
          END AS BillType,
          Inv.Status AS Status,
          Price1,
          c.Status AS AStatus,
		  inv.SAcct
   FROM Inv
   LEFT JOIN Chart c ON inv.SAcct=c.ID
   WHERE 
      Name IS NOT NULL  AND Inv.TYPE not in (0,2) and Inv.Status=0
	  AND ISNULL(Inv.fDesc,'')<>''
  order by Inv.fDesc
END
