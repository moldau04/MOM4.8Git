CREATE  PROCEDURE [dbo].[spGetVendorSearch]      
    @SearchText VARCHAR(50),    
    @EN int    
AS    
DECLARE @WOspacialchars VARCHAR(50)     
SET @WOspacialchars = dbo.RemoveSpecialChars(@SearchText)    
BEGIN     
    SET NOCOUNT ON;    
    if(@EN = 1)    
    Begin    
        SELECT DISTINCT Top 100 
            v.ID As ID 
            ,r.Name As Name
            , v.Terms,B.Name As Company
            ,('ID: '+v.Acct+', '+'Vendor: '+r.Name+', '+r.Contact+', '+r.Address+', '+r.City+', '+r.[State]+', '+r.Zip+', Phone: '+r.Phone+', Email: '+r.EMail) as [desc],v.Days as [Days]
            , r.[State] as VState,   
            CASE WHEN ISNUMERIC(REPLACE(REPLACE(t.Name,'Net',''),'Days','')) <> 1 THEN 30 ELSE (REPLACE(REPLACE(t.Name,'Net',''),'Days','')) END as [Term],    
            CASE WHEN ISNULL(v.STax,'') ='' THEN 0 WHEN ISNULL(v.STax,'') <> '' THEN (SELECT Rate FROM STax WHERE Name = v.STax AND Utype = 0)  END as STaxRate,  
            CASE WHEN ISNULL(v.UTax,'') ='' THEN 0 WHEN ISNULL(v.UTax,'') <> '' THEN (SELECT Rate FROM STax WHERE Name = v.UTax AND Utype = 1) END as UTaxRate,  
            CASE WHEN ISNULL(v.STax,'') ='' THEN Null WHEN ISNULL(v.STax,'') <> '' THEN (SELECT Type FROM STax WHERE Name = v.STax AND Utype = 0)  END as STaxType,  
            CASE WHEN ISNULL(v.UTax,'') ='' THEN Null WHEN ISNULL(v.UTax,'') <> '' THEN (SELECT Type FROM STax WHERE Name = v.UTax AND Utype = 1) END as UTaxType,  
            v.STax as STaxName,v.UTax as UtaxName,  
            ISNULL((SELECT GL FROM STax WHERE Name = v.STax AND Utype = 0),0) AS STaxGL
            ,ISNULL((SELECT GL FROM STax WHERE Name = v.STax AND Utype = 1),0) AS SUaxGL,  
            ISNULL(v.STax,'') +'  '+ISNULL(CASE WHEN ISNULL(v.STax,'') ='' THEN '' WHEN ISNULL(v.STax,'') <> '' THEN CAST((SELECT Rate FROM STax WHERE Name = v.STax AND Utype = 0) AS VARCHAR(50))  END,'') as STax,  
            ISNULL(v.UTax,'') +'  '+ISNULL(CASE WHEN ISNULL(v.UTax,'') ='' THEN '' WHEN ISNULL(v.UTax,'') <> '' THEN CAST((SELECT Rate FROM STax WHERE Name = v.UTax AND Utype = 1) AS VARCHAR(50))  END,'') as UTax
            ,V.Type VendorType
        FROM [dbo].[Vendor] v    
            Join Rol r on v.Rol=r.ID    
            join tblTerms t ON v.Terms = t.ID    
            left outer join tblUserCo UC on UC.CompanyID = r.EN     
            left outer join Branch B on B.ID = r.EN
        Where UC.IsSel = 1 and     
            v.Status = 0    
            AND ((dbo.RemoveSpecialChars(r.Name) LIKE '%'+isnull(@WOspacialchars,'')+'%')    
                OR (dbo.RemoveSpecialChars( V.Acct) LIKE '%'+@WOspacialchars+'%')     
                OR (r.Contact LIKE '%'+@SearchText+'%')     
                OR (dbo.RemoveSpecialChars ( r.Address) LIKE '%'+@WOspacialchars+'%')      
                OR (r.City LIKE '%'+@SearchText+'%')      
                OR (r.State = +@SearchText)      
                OR (r.Zip LIKE '%'+@SearchText+'%')     
                OR (dbo.RemoveSpecialChars(r.Phone) LIKE '%'+@WOspacialchars+'%')     
                OR (r.EMail LIKE '%'+@SearchText+'%')
            )      
      
        ORDER BY r.Name    
        select @WOspacialchars    
    end     
    else    
    begin     
        SELECT DISTINCT Top 100 v.ID As ID ,r.Name As Name, v.Terms,B.Name As Company
            , ('ID: '+v.Acct+', '+'Vendor: '+r.Name+', '+r.Contact+', '+r.Address+', '+r.City+', '+r.[State]+', '+r.Zip+', Phone: '+r.Phone+', Email: '+r.EMail) as [desc]
            , v.Days as [Days]
            , r.[State] as VState
            , CASE WHEN ISNUMERIC(REPLACE(REPLACE(t.Name,'Net',''),'Days','')) <> 1 THEN 30 ELSE (REPLACE(REPLACE(t.Name,'Net',''),'Days','')) END as [Term],  
            CASE WHEN ISNULL(v.STax,'') ='' THEN 0 WHEN ISNULL(v.STax,'') <> '' THEN (SELECT Rate FROM STax WHERE Name = v.STax AND Utype = 0) END as STaxRate,  
            CASE WHEN ISNULL(v.UTax,'') ='' THEN 0 WHEN ISNULL(v.UTax,'') <> '' THEN (SELECT Rate FROM STax WHERE Name = v.UTax AND Utype = 1) END as UTaxRate,  
            CASE WHEN ISNULL(v.STax,'') ='' THEN Null WHEN ISNULL(v.STax,'') <> '' THEN (SELECT Type FROM STax WHERE Name = v.STax AND Utype = 0)  END as STaxType,  
            CASE WHEN ISNULL(v.UTax,'') ='' THEN Null WHEN ISNULL(v.UTax,'') <> '' THEN (SELECT Type FROM STax WHERE Name = v.UTax AND Utype = 1) END as UTaxType,  
            v.STax as STaxName
            ,v.UTax as UtaxName,  
            ISNULL((SELECT GL FROM STax WHERE Name = v.STax AND Utype = 0),0) AS STaxGL
            ,ISNULL((SELECT GL FROM STax WHERE Name = v.STax AND Utype = 1),0) AS SUaxGL,  
            --v.STax +'  '+CASE WHEN ISNULL(v.STax,'') ='' THEN '' WHEN ISNULL(v.STax,'') <> '' THEN CAST((SELECT Rate FROM STax WHERE Name = v.STax AND Utype = 0) AS VARCHAR(50))  END as STax,  
            ISNULL(v.STax,'') +'  '+ISNULL(CASE WHEN ISNULL(v.STax,'') ='' THEN '' WHEN ISNULL(v.STax,'') <> '' THEN CAST((SELECT Rate FROM STax WHERE Name = v.STax AND Utype = 0) AS VARCHAR(50))  END,'') as STax,  
            --v.UTax +'  '+CASE WHEN ISNULL(v.UTax,'') ='' THEN '' WHEN ISNULL(v.UTax,'') <> '' THEN CAST((SELECT Rate FROM STax WHERE Name = v.UTax AND Utype = 1) AS VARCHAR(50))  END as UTax  
            ISNULL(v.UTax,'') +'  '+ISNULL(CASE WHEN ISNULL(v.UTax,'') ='' THEN '' WHEN ISNULL(v.UTax,'') <> '' THEN CAST((SELECT Rate FROM STax WHERE Name = v.UTax AND Utype = 1) AS VARCHAR(50))  END,'') as UTax
            ,V.Type VendorType
  
        FROM [dbo].[Vendor] v    
            Join Rol r on v.Rol=r.ID    
            join tblTerms t ON v.Terms = t.ID    
            left outer join tblUserCo UC on UC.CompanyID = r.EN     
            left outer join Branch B on B.ID = r.EN 
        Where      
       
            v.Status = 0    
            AND ((dbo.RemoveSpecialChars(r.Name) LIKE '%'+isnull(@WOspacialchars,'')+'%')    
                OR (dbo.RemoveSpecialChars( V.Acct) LIKE '%'+@WOspacialchars+'%')     
                OR (r.Contact LIKE '%'+@SearchText+'%')     
                OR (dbo.RemoveSpecialChars ( r.Address) LIKE '%'+@WOspacialchars+'%')      
                OR (r.City LIKE '%'+@SearchText+'%')      
                OR (r.State = +@SearchText)      
                OR (r.Zip LIKE '%'+@SearchText+'%')     
                OR (dbo.RemoveSpecialChars(r.Phone) LIKE '%'+@WOspacialchars+'%')     
                OR (r.EMail LIKE '%'+@SearchText+'%')
            )      
        ORDER BY r.Name    
    end    
END 