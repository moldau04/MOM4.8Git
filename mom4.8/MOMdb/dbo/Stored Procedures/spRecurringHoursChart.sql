CREATE PROCEDURE [dbo].[spRecurringHoursChart]
	  @startDate DATETIME,
	  @endDate DATETIME,
	  @startDate1 DATETIME,
	  @endDate1 DATETIME
AS
BEGIN


	 Select em.Name as SalesPerson ,Avg( DATEDIFF(day, e.Fdate, j.fdate)) Avg from estimate e join job j on e.job = j.id join terr t on t.ID = e.estimateUserId 
     join emp em on em.id = t.sman where j.fdate is not null and  e.Fdate >=@startDate and e.Fdate <= @endDate 
      Group by em.Name;
  
      Select em.Name as SalesPerson ,Avg( DATEDIFF(day, e.Fdate, j.fdate)) Avg from estimate e join job j on e.job = j.id join terr t 
	  on t.ID = e.estimateUserId join emp em on em.id = t.sman where j.fdate is not null and  
      e.Fdate >=@startDate1 and e.Fdate <= @endDate1
	  Group by em.Name;
	  
	  

END