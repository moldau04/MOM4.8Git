CREATE PROCEDURE spGetAvgEstimate
	@startDate  DATETIME,
	@endDate  DATETIME,
	@startDate1  DATETIME,
	@endDate1  DATETIME
AS
BEGIN
	 ;with aa as
	 ( 
        Select isnull(Avg( DATEDIFF(day, e.Fdate, j.fdate)),0)  days 
		from estimate e join job j on e.job = j.id join terr t on t.ID = e.estimateUserId join emp em on em.id = t.sman where j.fdate is not null and 
		e.Fdate >= @startDate and e.Fdate<= @endDate
		Group by em.Name
	 )
	 select ISNULL(AVG(days),0) avgFirstYear from aa 

     ;with bb as
	 ( 
	    Select isnull(Avg( DATEDIFF(day, e.Fdate, j.fdate)),0)  days from estimate e join job j on e.job = j.id join terr t on t.ID = e.estimateUserId join emp em on em.id = t.sman where j.fdate is not null and
        e.Fdate >= @startDate1 and e.Fdate<=@endDate1 
        Group by em.Name 
      )
      select ISNULL(AVG(days),0) avgSecondYear from bb 
END