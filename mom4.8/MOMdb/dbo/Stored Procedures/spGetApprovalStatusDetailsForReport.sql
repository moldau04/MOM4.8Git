CREATE PROCEDURE [dbo].[spGetApprovalStatusDetailsForReport]      
@PO INT            
AS            
BEGIN            
       
 SELECT       
  P.PO,      
  P.fDesc,      
  (SELECT Name FROM Control) AS ControlName,      
  (SELECT Address + char(10) + City + ', ' + State + ' ' + Zip FROM Control) AS ControAddress,      
  R.Name AS RollName,      
  ISNULL(R.Address,'') + char(10) + R.City + ', ' + R.State + ' ' + R.Zip + char(10) + ' Phone: ' + ISNULL(R.Phone,'') + char(10) + 'Fax: ' + ISNULL(R.Fax,'') AS RolAddress,      
  P.Amount,      
  P.fDate,--FORMAT(P.fDate, 'M/d/yyyy') AS fDate,      
  P.Due,--FORMAT(P.Due, 'M/d/yyyy') AS Due,      
  (SELECT Name FROM tblTerms where ID = P.Terms) as Terms,      
  P.ShipVia,   
  P.ShipTo,     
  P.FOB,    
  P.Custom2,  
  P.fBy,  
  ap.Signature,  
  ap.ApproveDate,
  ISNULL(w.fDesc,u.fUser) AS ApproveBy,
  ISNULL(B.Address,'') + char(10) + ISNULL(B.City,'') + ', ' + ISNULL(B.State,'') + ' ' + ISNULL(B.Zip,'') + char(10) + ISNULL(B.Phone,'') AS BranchAddress,  
  V.ID  AS VendorID,  
  V.Acct AS VendorAcct,  
  V.Acct# AS VendorAcct#  
 FROM PO AS P     
 LEFT JOIN vw_ApprovalStatus ap ON ap.PO = P.PO
 LEFT JOIN Vendor AS V ON p.Vendor = v.ID     
 LEFT JOIN Rol  AS R ON v.Rol = r.ID        
 LEFT JOIN Branch AS B ON B.ID = R.EN 
 LEFT JOIN tblUser u ON u.ID = ap.UserID
 LEFT JOIN tblWork w ON w.fDesc = u.fUser
 WHERE P.PO = @PO      
      
      
 SELECT         
  PO,        
  Line,        
  Quan,        
  PItem.fDesc,        
  Price,        
  Amount,        
  Job,    
  C.Acct AS ChartAcct  ,  
  isnull(I.Part ,'') Part ,
   isnull(I.[Name]  + '-' + Cast(W.ID as  varchar(20)),'') ItemCode
 FROM POItem AS PItem        
 LEFT JOIN Chart AS C ON C.ID = PItem.GL    
  LEFT JOIN Inv AS I ON C.ID = PItem.Inv    
  LEFT JOIN Warehouse AS W ON C.ID = PItem.WarehouseID    
 WHERE PItem.PO = @PO      
END