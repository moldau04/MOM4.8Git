--exec spAddCentral 'ABC',NULL  
CREATE PROCEDURE [dbo].[spAddCentral]    
@CentralName VARCHAR(150),  
@SortOrder SMALLINT = NULL  
AS    
BEGIN    
     
SET NOCOUNT ON;    
   
	 IF(@SortOrder IS NULL OR @SortOrder = 0)  
		SET @SortOrder = ISNULL((SELECT MAX(SortOrder) FROM CENTRAL),0) + 1  
  
	 INSERT INTO Central  
	 VALUES(@CentralName,@SortOrder)  
  
	 SELECT * FROM CENTRAL  
    
END 