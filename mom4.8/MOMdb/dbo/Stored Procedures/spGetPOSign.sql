CREATE PROCEDURE [dbo].[spGetPOSign]  
@PO INT        
AS        
BEGIN        
 SELECT * FROM vw_ApprovalStatus WHERE PO = @PO    
END