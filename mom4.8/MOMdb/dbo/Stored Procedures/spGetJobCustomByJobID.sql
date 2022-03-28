CREATE PROCEDURE [dbo].[spGetJobCustomByJobID] 
	@JobID  int
	
AS
BEGIN
	
BEGIN TRY

Declare @ID int
Declare @CustomLabel   		  VARCHAR(200)
Declare @CustomValue	VARCHAR(200)		
set @ID=1
Declare @text	VARCHAR(max)	

IF OBJECT_ID('tempdb..#tempJobCustom') IS NOT NULL DROP TABLE #tempJobCustom
CREATE TABLE #tempJobCustom
	(
		ID      	INT,       
		CustomLabel   		  VARCHAR(200),	
		CustomValue	VARCHAR(200)		
	)
	
Insert into #tempJobCustom (ID,CustomLabel,customValue)Values(1,isnull((select top 1 CASE Label WHEN '' THEN 'Custom 1' ELSE Label END from Custom where Name='Job1'),'Custom 1'),isnull((select Custom1 from Job where Id=@JobID),''))
Insert into #tempJobCustom (ID,CustomLabel,customValue)Values(2,isnull((select top 1 CASE Label WHEN '' THEN 'Custom 2' ELSE Label END FROM Custom where Name='Job2'),'Custom 2'),isnull((select Custom2 from Job where Id=@JobID),''))
Insert into #tempJobCustom (ID,CustomLabel,customValue)Values(3,isnull((select top 1 CASE Label WHEN '' THEN 'Custom 3' ELSE Label END FROM Custom where Name='Job3'),'Custom 3'),isnull((select Custom3 from Job where Id=@JobID),''))
Insert into #tempJobCustom (ID,CustomLabel,customValue)Values(4,isnull((select top 1 CASE Label WHEN '' THEN 'Custom 4' ELSE Label END FROM Custom where Name='Job4'),'Custom 4'),isnull((select Custom4 from Job where Id=@JobID),''))
Insert into #tempJobCustom (ID,CustomLabel,customValue)Values(5,isnull((select top 1 CASE Label WHEN '' THEN 'Custom 5' ELSE Label END FROM Custom where Name='Job5'),'Custom 5'),isnull((select Custom5 from Job where Id=@JobID),''))
Insert into #tempJobCustom (ID,CustomLabel,customValue)Values(6,isnull((select top 1 CASE Label WHEN '' THEN 'Custom 6' ELSE Label END FROM Custom where Name='Job6'),'Custom 6'),isnull((select Custom6 from Job where Id=@JobID),''))
Insert into #tempJobCustom (ID,CustomLabel,customValue)Values(7,isnull((select top 1 CASE Label WHEN '' THEN 'Custom 7' ELSE Label END FROM Custom where Name='Job7'),'Custom 7'),isnull((select Custom7 from Job where Id=@JobID),''))
Insert into #tempJobCustom (ID,CustomLabel,customValue)Values(8,isnull((select top 1 CASE Label WHEN '' THEN 'Custom 8' ELSE Label END FROM Custom where Name='Job8'),'Custom 8'),isnull((select Custom8 from Job where Id=@JobID),''))
Insert into #tempJobCustom (ID,CustomLabel,customValue)Values(9,isnull((select top 1 CASE Label WHEN '' THEN 'Custom 9' ELSE Label END FROM  Custom where Name='Job9'),'Custom 9'),isnull((select Custom9 from Job where Id=@JobID),''))
Insert into #tempJobCustom (ID,CustomLabel,customValue)Values(10,isnull((select top 1 CASE Label WHEN '' THEN 'Custom 10' ELSE Label END FROM Custom where Name='Job10'),'Custom 10'),isnull((select Custom10 from Job where Id=@JobID),''))
Insert into #tempJobCustom (ID,CustomLabel,customValue)Values(11,isnull((select top 1 CASE Label WHEN '' THEN 'Custom 11' ELSE Label END FROM Custom where Name='Job11'),'Custom 11'),isnull((select Custom11 from Job where Id=@JobID),''))
Insert into #tempJobCustom (ID,CustomLabel,customValue)Values(12,isnull((select top 1 CASE Label WHEN '' THEN 'Custom 12' ELSE Label END FROM Custom where Name='Job12'),'Custom 12'),isnull((select Custom12 from Job where Id=@JobID),''))
Insert into #tempJobCustom (ID,CustomLabel,customValue)Values(13,isnull((select top 1 CASE Label WHEN '' THEN 'Custom 13' ELSE Label END FROM Custom where Name='Job13'),'Custom 13'),isnull((select Custom13 from Job where Id=@JobID),''))
Insert into #tempJobCustom (ID,CustomLabel,customValue)Values(14,isnull((select top 1 CASE Label WHEN '' THEN 'Custom 14' ELSE Label END FROM Custom where Name='Job14'),'Custom 14'),isnull((select Custom14 from Job where Id=@JobID),''))
Insert into #tempJobCustom (ID,CustomLabel,customValue)Values(15,isnull((select top 1 CASE Label WHEN '' THEN 'Custom 15' ELSE Label END FROM Custom where Name='Job15'),'Custom 15'),isnull((select Custom15 from Job where Id=@JobID),''))
Insert into #tempJobCustom (ID,CustomLabel,customValue)Values(16,isnull((select top 1 CASE Label WHEN '' THEN 'Custom 16' ELSE Label END FROM Custom where Name='Job16'),'Custom 16'),isnull((select Custom16 from Job where Id=@JobID),''))
Insert into #tempJobCustom (ID,CustomLabel,customValue)Values(17,isnull((select top 1 CASE Label WHEN '' THEN 'Custom 17' ELSE Label END FROM Custom where Name='Job17'),'Custom 17'),isnull((select Custom17 from Job where Id=@JobID),''))
Insert into #tempJobCustom (ID,CustomLabel,customValue)Values(18,isnull((select top 1 CASE Label WHEN '' THEN 'Custom 18' ELSE Label END FROM Custom where Name='Job18'),'Custom 18'),isnull((select Custom18 from Job where Id=@JobID),''))
Insert into #tempJobCustom (ID,CustomLabel,customValue)Values(19,isnull((select top 1 CASE Label WHEN '' THEN 'Custom 19' ELSE Label END FROM  Custom where Name='Job19'),'Custom 19'),isnull((select Custom19 from Job where Id=@JobID),''))
Insert into #tempJobCustom (ID,CustomLabel,customValue)Values(20,isnull((select top 1 CASE Label WHEN '' THEN 'Custom 20' ELSE Label END FROM Custom where Name='Job20'),'Custom 20'),isnull((select Custom20 from Job where Id=@JobID),''))
select * from #tempJobCustom order by ID
IF OBJECT_ID('tempdb..#tempJobCustom') IS NOT NULL DROP TABLE #tempJobCustom
END TRY
BEGIN CATCH	

	
	SELECT ERROR_MESSAGE() AS ErrorMessage; 
	IF @@TRANCOUNT>0	
	RAISERROR ('An error has occurred on this page.',16,1)
	RETURN

END CATCH
END
