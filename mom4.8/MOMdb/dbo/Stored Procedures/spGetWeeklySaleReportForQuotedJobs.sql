CREATE PROCEDURE [dbo].[spGetWeeklySaleReportForQuotedJobs]

    @StartDate Varchar(50)=null,
	@EndDate Varchar(50)=null

--@ID INT= NULL

 

as
	begin
		SET DATEFIRST 6;

		------Start Code for Quoted Jobs------------------
			Select JobType,SUM(GoalEstimates) AS GoalEstimates,SUM(ActualEstimatesCount) AS ActualEstimates,SUM(TotalPrice) AS GoalPrice , SUM(ConvertedPriceCount) AS ActualPriceCount,
	SUM(Week1Count) AS Week1Count,SUM(Week2Count) AS Week2Count,SUM(Week3Count) AS Week3Count,SUM(Week4Count) AS Week4Count,SUM(Week5Count) AS Week5Count,
	SUM(Week1Price) AS Week1Price,SUM(Week2Price) AS Week2Price,SUM(Week3Price) AS Week3Price,SUM(Week4Price) AS Week4Price,SUM(Week5Price) AS Week5Price 
	FROM 
(
Select  JobType,GoalEstimates,ActualEstimatesCount,TotalPrice,ConvertedPriceCount,
	IsNull([1],0) as 'Week1Count',
        IsNull([2],0) as 'Week2Count',
        IsNull([3],0) as 'Week3Count',
        IsNull([4],0) as 'Week4Count',
        IsNull([5], 0) as 'Week5Count',
		 IsNull([6],0) as 'Week1Price',
        IsNull([7],0) as 'Week2Price',
        IsNull([8],0) as 'Week3Price',
        IsNull([9],0) as 'Week4Price',
        IsNull([10], 0) as 'Week5Price'

From 
(
Select JobType.Type as JobType, Count(*) As GoalEstimates,SUM( CASE WHEN Job IS NULL THEN 1 ELSE 0 END) AS ConvertedEstimates, SUM( CASE WHEN Job IS NULL THEN 1 ELSE 0 END) As ActualEstimatesCount,
SUM(Price) As TotalPrice,SUM( CASE WHEN Job IS NULL THEN Price ELSE 0 END) AS ConvertedPrice, SUM( CASE WHEN Job IS NULL THEN Price ELSE 0 END) As ConvertedPriceCount,
DATEDIFF(week, DATEADD(MONTH, DATEDIFF(MONTH, 0, Estimate.fDate), 0), Estimate.fDate) +1 as [Weeks],
DATEDIFF(week, DATEADD(MONTH, DATEDIFF(MONTH, 0, Estimate.fDate), 0), Estimate.fDate) +1+5 as [Weekp]
FROM Estimate 
LEFT JOIN JobT ON Estimate.Template = JobT.ID
LEFT JOIN JobType ON JobT.Type=JobType.ID
where fDate>=@StartDate and fDate <=@EndDate
group by JobType.Type,DATEDIFF(week, DATEADD(MONTH, DATEDIFF(MONTH, 0, Estimate.fDate), 0), Estimate.fDate) +1
)p 
Pivot (Sum(ConvertedEstimates) for Weeks in ([1],[2],[3],[4],[5])) as pv
Pivot (Sum(ConvertedPrice) for Weekp in ([6],[7],[8],[9],[10])) as pv1

) Tab
Group BY JobType
		
	
	SELECT ISNULL([1],DATEADD(dd, (14 - @@DATEFIRST - DATEPART(dw,DATEADD(month,DATEDIFF(mm,0,@StartDate),0)))%7, DATEADD(month,DATEDIFF(mm,0,@StartDate),0))) AS [1],[2],[3],[4], DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,@StartDate)+1,0)) as [5]
FROM (
		select dateadd(dd,number,@StartDate) as Week_Day,
		DATEDIFF(week, DATEADD(MONTH, DATEDIFF(MONTH, 0, dateadd(dd,number,@StartDate)), 0), dateadd(dd,number,@StartDate)) + 1 as [Weeks]
		from master..spt_values
		where type = 'p'
		and year(dateadd(dd,number,@StartDate))=year(@StartDate) and month(dateadd(dd,number,@StartDate))=month(@StartDate)
		and DATEPART(dw,dateadd(dd,number,@StartDate)) = 7
) as s
PIVOT
(
    MIN(Week_Day)
    FOR [Weeks] IN ([1], [2], [3], [4])
)AS pvt

------END Code for Quoted Jobs------------------


		------Start Code for Awarded jobs Jobs------------------
			Select AwJobType,SUM(AwGoalEstimates) AS AwGoalEstimates,SUM(AwActualEstimatesCount) AS AwActualEstimates,SUM(AwTotalPrice) AS AwGoalPrice , SUM(AwConvertedPriceCount) AS AwActualPriceCount,
	SUM(Week1Count) AS AwWeek1Count,SUM(Week2Count) AS AwWeek2Count,SUM(Week3Count) AS AwWeek3Count,SUM(Week4Count) AS AwWeek4Count,SUM(Week5Count) AS AwWeek5Count,
	SUM(Week1Price) AS AwWeek1Price,SUM(Week2Price) AS AwWeek2Price,SUM(Week3Price) AS AwWeek3Price,SUM(Week4Price) AS AwWeek4Price,SUM(Week5Price) AS AwWeek5Price 
	FROM 
(
Select  AwJobType,AwGoalEstimates,AwActualEstimatesCount,AwTotalPrice,AwConvertedPriceCount,
	IsNull([1],0) as 'Week1Count',
        IsNull([2],0) as 'Week2Count',
        IsNull([3],0) as 'Week3Count',
        IsNull([4],0) as 'Week4Count',
        IsNull([5], 0) as 'Week5Count',
		 IsNull([6],0) as 'Week1Price',
        IsNull([7],0) as 'Week2Price',
        IsNull([8],0) as 'Week3Price',
        IsNull([9],0) as 'Week4Price',
        IsNull([10], 0) as 'Week5Price'

From 
(
Select JobType.Type as AwJobType, Count(*) As AwGoalEstimates,SUM( CASE WHEN Job IS NULL THEN 0 ELSE 1 END) AS AwConvertedEstimates, SUM( CASE WHEN Job IS NULL THEN 0 ELSE 1 END) As AwActualEstimatesCount,
SUM(Price) As AwTotalPrice,SUM( CASE WHEN Job IS NULL THEN 0 ELSE Price END) AS AwConvertedPrice, SUM( CASE WHEN Job IS NULL THEN 0 ELSE Price END) As AwConvertedPriceCount,
DATEDIFF(week, DATEADD(MONTH, DATEDIFF(MONTH, 0, Estimate.fDate), 0), Estimate.fDate) +1 as [Weeks],
DATEDIFF(week, DATEADD(MONTH, DATEDIFF(MONTH, 0, Estimate.fDate), 0), Estimate.fDate) +1+5 as [Weekp]
FROM Estimate 
LEFT JOIN JobT ON Estimate.Template = JobT.ID
LEFT JOIN JobType ON JobT.Type=JobType.ID
where fDate>=@StartDate and fDate <=@EndDate
group by JobType.Type,DATEDIFF(week, DATEADD(MONTH, DATEDIFF(MONTH, 0, Estimate.fDate), 0), Estimate.fDate) +1
)p 
Pivot (Sum(AwConvertedEstimates) for Weeks in ([1],[2],[3],[4],[5])) as pv
Pivot (Sum(AwConvertedPrice) for Weekp in ([6],[7],[8],[9],[10])) as pv1

) Tab
Group BY AwJobType
		
	



------END Code for Awarded  Jobs------------------






	end
