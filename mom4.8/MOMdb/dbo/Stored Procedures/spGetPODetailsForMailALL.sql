--EXEC spGetPODetailsForMailALL '9246'    
CREATE PROCEDURE [dbo].[spGetPODetailsForMailALL]        
@POIDs VARCHAR(MAX)=NULL    
AS              
BEGIN              
        
 SELECT     
 *     
 ,(SELECT Signature FROM vw_ApprovalStatus AS A WHERE A.PO = T.PO) AS  Signature    
 FROM       
 (    
  SELECT      
  DISTINCT     
  PO,        
  P.fDesc,        
  (SELECT Name FROM Control) AS ControlName,        
  (SELECT Address + '    ' + City + ', ' + State + ' ' + Zip FROM Control) AS ControAddress,        
  R.Name AS RollName,        
  ISNULL(R.Address,'') + '    ' + R.City + ', ' + R.State + ' ' + R.Zip + '    ' + ' Phone: ' + ISNULL(R.Phone,'') + '    ' + 'Fax: ' + ISNULL(R.Fax,'') AS RolAddress,        
  P.Amount,        
  P.fDate,    
  P.Due,    
  P.Terms,        
  P.ShipVia,        
  P.FOB,      
  V.ID AS VenderID      
  FROM PO AS P        
  INNER JOIN Vendor AS V ON p.Vendor = v.ID         
  INNER JOIN Rol  AS R ON v.Rol = r.ID     
  LEFT JOIN Phone as Ph ON Ph.Rol = R.ID AND ISNULL(Ph.Email,'') != ''  AND Ph.EmailRecPO = 1       
  WHERE (P.PO IN (Select SplitValue from [dbo].[fnsplit](@POIDs,','))  OR @POIDs IS NULL)      
  ) AS t    
        
 SELECT     
  DISTINCT         
  PItem.PO,        
  PItem.Line,        
  PItem.Quan,        
  PItem.fDesc,        
  PItem.Price,        
  PItem.Amount,        
  PItem.Job        
 FROM POItem AS PItem        
 INNER JOIN PO AS P ON P.PO = PItem.PO       
 INNER JOIN Vendor AS V ON p.Vendor = v.ID         
 INNER JOIN Rol  AS R ON v.Rol = r.ID     
 LEFT JOIN Phone as Ph ON Ph.Rol = R.ID AND ISNULL(Ph.Email,'') != '' AND Ph.EmailRecPO = 1      
 WHERE (P.PO IN (Select SplitValue from [dbo].[fnsplit](@POIDs,','))  OR @POIDs IS NULL)      
    
 If Object_Id('tempdb.dbo.#VenderData ') Is NOT NULL    
  DROP TABLE #VenderData     
     
 SELECT     
  DISTINCT V.ID AS VenderID,ph.Email     
 INTO #VenderData    
 FROM PO AS P        
 INNER JOIN Vendor AS V ON p.Vendor = v.ID      
 INNER JOIN Rol  AS R ON v.Rol = r.ID          
 LEFT JOIN Phone as Ph ON Ph.Rol = R.ID AND ISNULL(Ph.Email,'') != '' AND Ph.EmailRecPO = 1    
 WHERE (P.PO IN (Select SplitValue from [dbo].[fnsplit](@POIDs,','))  OR @POIDs IS NULL)      
    
 SELECT distinct B.[VenderID], LEFT(r.Email, LEN(r.Email)-1) Email    
 FROM #VenderData B    
 CROSS APPLY    
 (    
  SELECT ISNULL(A.[Email],'') + '; '    
  FROM #VenderData A    
  where A.[VenderID] = B.[VenderID]    
  FOR XML PATH('')    
 ) r (Email)    
    
END 