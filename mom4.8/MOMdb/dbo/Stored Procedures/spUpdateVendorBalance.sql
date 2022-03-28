CREATE PROCEDURE [dbo].[spUpdateVendorBalance]   
 @Vendor int  
AS  
BEGIN  
   
 SET NOCOUNT ON;  
  
 --  UPDATE Vendor  
 --SET Balance = (SELECT sum(Amount) FROM Trans   
 --     WHERE Type in (21, 40) AND AcctSub = @Vendor  
 --     GROUP BY AcctSub ) WHERE ID = @Vendor  
  
 UPDATE Vendor  
 SET Balance = ISNULL((SELECT ISNULL(sum(Amount),0) FROM Trans   
      WHERE Type in (21, 40) AND AcctSub = @Vendor  
      GROUP BY AcctSub ),0)+ISNULL((SELECT ISNULL(sum(Amount),0) FROM Trans   
      WHERE Type in (31) AND AcctSub = @Vendor AND fDesc='AP Credit Apply Discount Taken'  
      GROUP BY AcctSub),0) 
	  -ISNULL((SELECT ISNULL(sum(Amount),0) FROM CD WHERE Vendor = 346 AND Status = 2),0)
	  FROM Vendor WHERE ID = @Vendor  
   
END  
  
        
  
  
  