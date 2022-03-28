CREATE PROCEDURE [dbo].[spGetTeamMemberTitleSearch]
	@SearchText varchar(150),
	@ProjectId int
AS
declare @WOspacialchars varchar(50) 
declare @text nvarchar(max)
set @WOspacialchars = dbo.RemoveSpecialChars(@SearchText)
BEGIN
	
	SET NOCOUNT ON;

	if(@ProjectId<>0)
	begin
		set @text = 'SELECT t.RoleName AS value, t.RoleName as label FROM (
					SELECT Distinct t.Title as RoleName FROM Team t WHERE t.JobID = '''+Convert(varchar(50),@ProjectId)+'''
					UNION
					'
	end

	if(@text<>'')
	begin
		--set @text += ' SELECT t.Title
		--		 FROM [dbo].[tblTeamMemberTitle] t) as t 
		--		 WHERE 1=1
		--		 '
		set @text += ' SELECT t.RoleName
				 FROM tblRole t) as t 
				 WHERE 1=1
				 '
	end
	else
	begin
		set @text = 'SELECT t.RoleName AS value, t.RoleName as label
				 FROM tblRole t 
				 WHERE 1=1
				 '
	end			 
	

	if(@SearchText<>'')
	begin
		set @text += ' AND (dbo.RemoveSpecialChars(t.RoleName) LIKE ''%'+@WOspacialchars+'%'') '
	end
	set @text += ' Order by RoleName'
	PRINT @text
	exec(@text)
END

