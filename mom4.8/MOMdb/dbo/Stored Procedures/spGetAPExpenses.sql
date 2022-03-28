CREATE PROCEDURE [dbo].[spGetAPExpenses]    
 @vendor int,    
 @fromDate datetime,    
 @toDate datetime,    
 @SearchValue Varchar(50),    
 @SearchBy Varchar(50)    
    
AS    
BEGIN    
     
 SET NOCOUNT ON;    
 DECLARE @text varchar(max)    
 DECLARE @textPrev varchar(max)    
 DECLARE @prevRunTotal varchar(max)    
    
 CREATE Table #TempTable (    
 PK int identity(1,1),    
    ID INT NOT NULL,    
 fDate Datetime,    
 Ref VARCHAR(100),    
 fDesc VARCHAR(MAX),    
 Vendor int,    
 VendorName VARCHAR(MAX),    
 Status int,    
 StatusName VARCHAR(100),    
 Amount NUMERIC(30,2),    
 Debit NUMERIC(30,2),   
 Credit NUMERIC(30,2),   
 Disc NUMERIC(30,2),   
 Balance NUMERIC(30,2),    
 Type VARCHAR(50),  
 TRID int  
);    
    
 SET  @textPrev =  'Select ISNULL(SUM(Amount),0) from (    
 (SELECT     
  p.ID,    
  p.fDate,     
  p.Amount,    
  (CASE p.Status     
   WHEN 0 THEN ''Open''    
   WHEN 1 THEN ''Closed''                                 
   WHEN 2 THEN ''Void''    
   WHEN 2 THEN ''Partial''    
  END) AS StatusName    
  FROM PJ AS p          
  inner join Vendor AS v on p.Vendor = v.ID             
  where p.Vendor ='+  convert(nvarchar(150),@vendor)+ ')    
    
 UNION    
    
 (SELECT     
   p.ID,    
   CP.fDate,    
   cp.Disc*-1 as Amount,    
   (CASE p.Status WHEN 0 THEN ''Open''    
       WHEN 1 THEN ''Closed''                                 
       WHEN 2 THEN ''Void''    
       WHEN 3 THEN ''Partial''    
     END) AS StatusName    
  FROM CreditPaid CP INNER JOIN PJ AS p  ON CP.TRID = P.TRID        
  inner join Vendor AS v on p.Vendor = v.ID         
  inner join Rol AS r on v.Rol = r.ID        
  left join openAP AS o on p.ID = o.PJID        
  where p.Vendor ='+  convert(nvarchar(150),@vendor)+' AND CP.Disc>0)    
    
 UNION    
    
   (SELECT     
  c.ID,     
  c.fDate,     
  c.Amount * -1 AS Amount,    
  (CASE     
   WHEN isnull(tt.sel,0) = 0 THEN ''Open''    
   WHEN tt.sel = 1 THEN ''Cleared''    
   WHEN tt.sel = 2 THEN ''Voided''    
  END) AS StatusName    
 FROM CD as c     
  left join Bank as b on c.Bank = b.ID     
  left join trans as t on c.TransID = t.ID    
  left join (    
   select ct.ID, t.batch, t.sel, t.type    
    from trans t    
    inner join (    
    select c.ID, t.batch     
    from trans t inner join cd c on t.ID = c.TransID    
    ) ct on     
    ct.Batch = t.Batch    
     where type = 20     
   ) tt on tt.batch = t.Batch and tt.ID = c.ID    
 where c.Vendor ='+  convert(nvarchar(150),@vendor) +' ))as ven where '    
 SET @textPrev += ' ven.fdate < '''    
                + CONVERT(VARCHAR(50), @fromDate) + ''''    
                                 
 IF (@SearchValue  ='All')    
  BEGIN    
   if (@SearchBy ='Charges')    
  begin    
     set  @textPrev += '     
    and ven.Amount<0  '    
  end    
  else If(@SearchBy='Credit')    
  begin    
   set  @textPrev += '     
    and ven.Amount>=0  '    
  end    
  END    
    
  IF (@SearchValue  ='Open')    
  BEGIN    
   if (@SearchBy ='Charges')    
  begin    
     set  @textPrev += '     
   and ven.Amount<0  and ven.StatusName=''Open'' '    
  end    
  else If(@SearchBy='Credits')    
  begin    
   set  @textPrev += '     
    and ven.Amount>=0 and ven.StatusName=''Open'''    
  end    
  else    
  begin    
  set  @textPrev += '     
  and  ven.StatusName in (''Open'',''Partial'') '    
  end    
  END    
    
   IF (@SearchValue  ='Closed')    
  BEGIN    
   if (@SearchBy ='Charges')    
  begin    
     set  @textPrev += '     
   and ven.Amount<0  and ven.StatusName =''Closed''  '    
  end    
  else If(@SearchBy='Credits')    
  begin    
   set  @textPrev += '    
   and ven.Amount>=0 and ven.StatusName =''Closed''  '    
  end    
  else    
  begin    
  set  @textPrev += '    
    and   ven.StatusName =''Closed''  '    
  end    
  END    
    
  set  @text=  '  INSERT INTO #TempTable (ID,fDate,Ref,fDesc,Vendor,VendorName,Status,StatusName,Amount,Debit,Credit,Disc,Balance,Type,TRID)  Select ID,fDate,Ref,fDesc,Vendor,VendorName,Status,StatusName,Amount,Debit,Credit,Disc,Balance,Type,TRID from ((SELECT
   --Select ID,fDate,Ref,fDesc,Vendor,VendorName,Status,StatusName,Amount,Type,SUM(Amount) OVER (ORDER BY TRID,fDate, ID) AS RunTotal from ((SELECT     
   p.ID,    
   p.fDate,    
   p.Ref,    
   p.fDesc,    
   p.Vendor,     
   r.Name AS VendorName,    
   isnull(p.Status,0) as Status,     
   (CASE p.Status WHEN 0 THEN ''Open''    
       WHEN 1 THEN ''Closed''                                 
       WHEN 2 THEN ''Void''    
       WHEN 3 THEN ''Partial''    
     END) AS StatusName,    
   p.Amount,    
   CASE WHEN p.Amount > 0 THEN p.Amount ELSE NULL END AS Debit,  
   CASE WHEN p.Amount < 0 THEN p.Amount ELSE NULL END AS Credit,  
   0 AS Disc,  
   IsNull((SELECT (ISNull(o.Original,0) -  (ISNULL(o.Selected,0)+ISNULL(o.Disc,0))) FROM OpenAp OA WHERE OA.PJID = p.ID),0) AS Balance,  
   ''Bill'' as Type,    
   p.TRID    
  FROM PJ AS p          
  inner join Vendor AS v on p.Vendor = v.ID         
  inner join Rol AS r on v.Rol = r.ID        
  left join openAP AS o on p.ID = o.PJID        
  where p.Vendor ='+  convert(nvarchar(150),@vendor)+    
  ')    
    
    
 UNION    
    
   (select     
  c.ID,     
  c.fDate,     
  Convert(nvarchar(50),c.Ref) as Ref,     
  --c.fDesc,     
  tt.fDesc,    
  c.Vendor,     
  r.Name AS VendorName,     
  isnull(tt.Sel,0) as Status,    
  case when isnull(tt.sel,0) = 0 then ''Open''    
    when tt.sel = 1 then ''Cleared''    
    when tt.sel = 2 then ''Voided''    
    end as StatusName,    
  --c.Amount * -1 AS Amount,    
  tt.Amount  AS Amount,  
   CASE WHEN tt.Amount > 0 THEN tt.Amount ELSE NULL END AS Debit,  
   CASE WHEN tt.Amount < 0 THEN tt.Amount ELSE NULL END AS Credit,  
   0 AS Disc,  
  0 AS Balance,  
  ''Check'' as Type,    
  tt.TRID as TRID     
        
     
 from CD as c     
  left join Bank as b on c.Bank = b.ID     
  left join Vendor as v on v.ID = c.Vendor    
  left join trans as t on c.TransID = t.ID    
  left join Rol as r on r.ID = v.Rol    
  left join (    
   select ct.ID, t.batch, t.sel, t.type,t.Amount,t.fDesc,t.ID as TRID    
    from trans t    
    inner join (    
    select c.ID, t.batch     
    from trans t inner join cd c on t.ID = c.TransID    
    ) ct on     
    ct.Batch = t.Batch    
   where type = 20  AND t.Acctsub IS NOT NULL   
  ) tt on tt.batch = t.Batch and tt.ID = c.ID    
 where c.Vendor ='+  convert(nvarchar(150),@vendor) +' AND  c.Status <> 2))as ven where '    
 SET @text += ' ven.fdate >= '''    
                + CONVERT(VARCHAR(50), @fromDate)    
                + ''' AND ven.fdate <='''    
                + CONVERT(VARCHAR(50), @toDate)+ ''''    
    
 IF (@SearchValue  ='All')    
  BEGIN    
   if (@SearchBy ='Charges')    
  begin    
     set  @text += '     
    and ven.Amount<0  '    
  end    
  else If(@SearchBy='Credit')    
  begin    
   set  @text += '     
    and ven.Amount>=0  '    
  end    
  END    
    
  IF (@SearchValue  ='Open')    
  BEGIN    
   if (@SearchBy ='Charges')    
  begin    
     set  @text += '     
   and ven.Amount<0  and ven.StatusName=''Open'' '    
  end    
  else If(@SearchBy='Credits')    
  begin    
   set  @text += '     
    and ven.Amount>=0 and ven.StatusName=''Open'''    
  end    
  else    
  begin    
  set  @text += '     
  and  ven.StatusName in (''Open'',''Partial'') '    
  end    
  END    
    
   IF (@SearchValue  ='Closed')    
  BEGIN    
   if (@SearchBy ='Charges')    
  begin    
     set  @text += '     
   and ven.Amount<0  and ven.StatusName =''Closed''  '    
  end    
  else If(@SearchBy='Credits')    
  begin    
   set  @text += '    
   and ven.Amount>=0 and ven.StatusName =''Closed''  '    
  end    
  else    
  begin    
  set  @text += '    
    and   ven.StatusName =''Closed''  '    
  end    
  END    
   set  @text += ' ORDER BY ven.fDate, ven.ID '    
    
  
  SET  @text += ' UPDATE m  
SET m.Disc = ISNULL(f.valsum,0)  
FROM #TempTable m  
INNER JOIN  
(  
  SELECT TRID,  SUM(Disc) valsum  
  FROM Paid  
  GROUP BY  TRID   
) f ON m.TRID = f.TRID AND m.Amount>0 WHERE m.Type=''Bill''  
  
 UPDATE m  
SET m.Disc = m.Disc+ISNULL(f.valsum,0)  
FROM #TempTable m  
INNER JOIN  
(  
  SELECT TRID,  SUM(Disc) valsum  
  FROM CreditPaid  
  GROUP BY  TRID   
) f ON m.TRID = f.TRID AND m.Amount > 0 WHERE m.Type=''Bill''  
  
UPDATE #TempTable SET Credit = Disc * -1 WHERE Type=''Bill'' AND ISNULL(Disc,0) > 0 '  
  
  
IF  @SearchValue ='Open' AND  @SearchBy ='All'  
BEGIN  
SET @text+= ' SELECT ID,fDate,Ref,fDesc,Vendor,VendorName,Status,StatusName,Amount,Debit,Credit,Disc,Balance,Type,TRID,SUM(Balance) OVER (ORDER BY fDate,ID) AS RunTotal FROM #TempTable ORDER BY fDate,ID  DROP TABLE #TempTable'    
END  
ELSE  
BEGIN  
   --SET @text+= ' SELECT ID,fDate,Ref,fDesc,Vendor,VendorName,Status,StatusName,Amount,Debit,Credit,Disc,Balance,Type,TRID,SUM(Amount) OVER (ORDER BY fDate,ID) AS RunTotal FROM #TempTable ORDER BY fDate,ID  DROP TABLE #TempTable'    
   SET @text+= ' SELECT ID,fDate,Ref,fDesc,Vendor,VendorName,Status,StatusName,Amount,Debit,Credit,Disc,Balance,Type,TRID,SUM(ISNULL(Debit,0)+ISNULL(Credit,0)) OVER (ORDER BY fDate,ID) AS RunTotal FROM #TempTable ORDER BY fDate,ID  DROP TABLE #TempTable' 
   
   END  
   print @text    
 exec(@text)    
 exec(@textPrev)    
    
END  