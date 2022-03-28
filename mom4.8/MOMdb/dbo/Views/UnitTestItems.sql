CREATE VIEW UnitTestItems AS  
SELECT i.Elev idUnit  
      ,i.LID idTestItem  
      ,t.Name  
      ,s.ItemName [Status]  
      ,i.Last  
      ,i.Next  
      ,CASE WHEN i.Ticket IS NULL THEN 0 ELSE 1 END Ticketed  
      ,i.Ticket  
  FROM LoadTestItem i  
       INNER JOIN  
       LoadTest t ON i.ID = t.ID  
       INNER JOIN  
       v_TestStatus s ON i.Status=s.ItemValue