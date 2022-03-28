CREATE PROCEDURE [dbo].[spGetPhaseByJob]
	@JobId int = 0,
	@Type smallint,
	@SearchText varchar(150)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @WOspacialchars varchar(50) 
	DECLARE @text1 nvarchar(max)
	SET @WOspacialchars = dbo.RemoveSpecialChars(@SearchText)

	SET @text1 = 'SELECT bt.ID as Type
			, bt.Type as TypeName
			, isnull(b.Type,0) as Bomtype 
			, (Select GroupName from tblEstimateGroup where Id=b.GroupID) As GroupName
			, b.Code 
			, (select top 1 JobCodeDesc from tblJobCodeDesc_ByJobType 
					where JobCodeID = ( select  top 1 jc.ID  FROM JobCode jc where jc.Code=b.Code) 
					and JobTypeID = (select type from job where id ='''+ convert(nvarchar(50),@JobId) +''')
			) as  CodeDesc
		FROM BOMT as bt 
		LEFT JOIN (SELECT distinct b.Type,jt.Code,jt.GroupId from Job as j 
					INNER JOIN JobTItem as jt ON jt.Job = j.ID
					INNER JOIN BOM as b ON b.JobTItemID = jt.ID
					WHERE j.ID = '''+ convert(nvarchar(50),@JobId) +''' AND jt.Type in (1,2)) as b
		ON bt.ID = b.Type '
	IF(@WOspacialchars != '' or @WOspacialchars != null)
	BEGIN
		SET @text1 +=' WHERE (dbo.RemoveSpecialChars(bt.Type) LIKE ''%'+ @WOspacialchars +'%'') '
	END

	EXEC(@text1)
END
GO