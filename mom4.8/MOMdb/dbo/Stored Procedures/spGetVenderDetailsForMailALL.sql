--EXEC spGetVenderDetailsForMailALL '2897,2898,2894'  
CREATE PROCEDURE [dbo].[spGetVenderDetailsForMailALL]
@POIDs VARCHAR(MAX)=NULL  
AS            
BEGIN            
  
 If Object_Id('tempdb.dbo.#VenderData ') Is NOT NULL  
  DROP TABLE #VenderData   
   
 SELECT   
  DISTINCT V.ID AS VenderID,ph.Email   
 INTO #VenderData  
 FROM PO AS P      
 INNER JOIN Vendor AS V ON p.Vendor = v.ID    
 INNER JOIN Rol  AS R ON v.Rol = r.ID        
 INNER JOIN Phone as Ph ON Ph.Rol = R.ID  
 WHERE ISNULL(Ph.Email,'') != ''  AND Ph.EmailRecPO = 1
 AND (P.PO IN (Select SplitValue from [dbo].[fnsplit](@POIDs,','))  OR @POIDs IS NULL)    
  
 SELECT distinct B.[VenderID], LEFT(r.Email, LEN(r.Email)-1) Email  
 FROM #VenderData B  
 CROSS APPLY  
 (  
  SELECT A.[Email] + '; '  
  FROM #VenderData A  
  where A.[VenderID] = B.[VenderID]  
  FOR XML PATH('')  
 ) r (Email)  
  
END 