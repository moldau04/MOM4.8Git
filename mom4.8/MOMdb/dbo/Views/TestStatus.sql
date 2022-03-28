CREATE VIEW v_TestStatus AS  
  SELECT idListConfig  
        ,ItemName  
        ,ItemValue  
        ,ItemCode  
        ,ItemDesc  
        ,IsDefault  
    FROM ListConfig  
   WHERE ListName ='Test.Status' 