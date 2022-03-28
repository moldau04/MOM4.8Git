CREATE PROCEDURE [dbo].[spGetListEmailByListByTypeAndUserID]
	@lsUser	varchar(Max)
AS


BEGIN TRY
	IF OBJECT_ID('tempdb..#tblEmail') IS NOT NULL DROP TABLE #tblEmail
	Create table #tblEmail(
		email varchar(100)
	)
	DECLARE @item varchar(100)
	DECLARE @strEmail VARCHAR (200)
	DECLARE @UserID Int
	DECLARE @type  VARCHAR (10)
	DECLARE curItems CURSOR FOR 	
		select Item from dbo.SplitString( @lsUser,';')
	OPEN curItems  
	FETCH NEXT FROM curItems INTO @item
	WHILE @@FETCH_STATUS = 0  
		BEGIN
		IF (SELECT CHARINDEX('_', @item) AS MatchPosition)=0			
			BEGIN
				SET @UserID=@item
				SET @type=	0
			END 
		ELSE
			BEGIN
				SET @UserID=Convert(int,(SELECT SUBSTRING(@item, 3,len(@item)-2) ))
				SET @type=	(SELECT SUBSTRING(@item, 1,1) )
			END		

		IF (@type='2')
			BEGIN
				SET @strEmail=ISNULL((SELECT TOP 1 ISNULL(r.email,'') as email FROM OWNER o 
						LEFT OUTER JOIN Rol r on o.Rol=r.ID 
						WHERE o.ID=@userID),'')
			END	
		ELSE
		BEGIN
			IF (@type='5')
			BEGIN
				SET @strEmail=ISNULL((SELECT TOP 1 t.Email FROM Team t	where t.ID = @userID),'')
			END 
			ELSE
			BEGIN
				SET @strEmail=ISNULL ((SELECT TOP 1	isnull(r.email,'') as email				
					FROM tblUser u 
					LEFT OUTER JOIN Emp e  on u.fUser=e.CallSign
					LEFT OUTER JOIN tblwork w on u.fuser=w.fdesc
					LEFT OUTER JOIN Rol r on e.Rol=r.ID 
					LEFT OUTER JOIN tblUserRole ur on ur.UserId = u.ID
					LEFT OUTER JOIN tblRole rur on rur.Id = ur.RoleId 
					WHERE u.ID=@userID),'')
			END 
		END

			insert into #tblEmail(email)values(@strEmail)
		FETCH NEXT FROM curItems INTO @item
		END	
	CLOSE curItems  
	DEALLOCATE curItems  
	
	select email from #tblEmail
		IF OBJECT_ID('tempdb..#tblEmail') IS NOT NULL DROP TABLE #tblEmail
END TRY
BEGIN CATCH	

	CLOSE curItems  
	DEALLOCATE curItems 	
	

END CATCH

