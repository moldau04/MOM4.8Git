
CREATE PROC [dbo].[spSalesDashboard]
AS
    DECLARE @Month CHAR(2)
    SELECT @Month = [MONTH]    FROM   Control

    DECLARE @YR CHAR(4)
    SET @YR = Datepart(YEAR, Getdate())

    DECLARE @EYR CHAR(4)
    SET @EYR = Datepart(YEAR, Dateadd(year, 1, Getdate()))

    DECLARE @StartDate DATETIME
    SET @StartDate = CONVERT(DATETIME, @Month + '/01/' + @Yr)

    DECLARE @EndDate DATETIME
    SET @EndDate = CONVERT(DATETIME, @Month + '/01/' + @EYR)
    
    --select @StartDate select @EndDate

    SELECT [Month],
           Isnull(SalesAnnual, 0) AS SalesAnnual,
           Isnull(GrossInc, 0)    AS GrossInc
    FROM   [Control]

    SELECT Isnull(Sum(Isnull(Revenue, 0)), 0) AS Revenue
    FROM   Lead l
           INNER JOIN Rol r
                   ON r.ID = l.Rol
    WHERE  r.type IN ( 3, 4 )
           AND l.Closed = 1
           AND l.CloseDate >= @StartDate AND l.CloseDate < @EndDate
           
           

    SELECT TOP 10 r.Name + ' : ' + fDesc AS opp,
                  Isnull(Revenue, 0)     AS revenue,
                  fuser, l.ID
    FROM   Lead l
           INNER JOIN Rol r
                   ON r.ID = l.Rol
    WHERE  r.type IN ( 3, 4 )
           AND l.closed = 0
           AND l.CloseDate >= @StartDate AND l.CloseDate < @EndDate
    ORDER  BY Isnull(Revenue, 0) DESC
    
    

    SELECT TOP 10 Isnull(Sum(Isnull(Revenue, 0)), 0) AS Revenue,
                  fuser
    FROM   Lead l
           INNER JOIN Rol r
                   ON r.ID = l.Rol
    WHERE  r.type IN ( 3, 4 )
           AND l.closed = 1
           AND l.CloseDate >= @StartDate AND l.CloseDate < @EndDate
    GROUP  BY fuser
    ORDER  BY Isnull(Sum(Isnull(Revenue, 0)), 0) DESC
    
    

    SELECT TOP 10 r.Name                             AS opp,
                  Isnull(Sum(Isnull(Revenue, 0)), 0) AS Revenue
    FROM   Lead l
           INNER JOIN Rol r
                   ON r.ID = l.Rol
    WHERE  r.type IN ( 3, 4 )
           AND l.closed = 1
           AND l.CloseDate >= @StartDate AND l.CloseDate < @EndDate
    GROUP  BY l.Rol,
              r.Name
    ORDER  BY Isnull(Sum(Isnull(Revenue, 0)), 0) DESC 
