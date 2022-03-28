CREATE PROCEDURE [dbo].[spGetJobLocSearch]
	@SearchText varchar(75)=NULL,
	@IsJob bit = 0,
	@EN INT		=0,
	@UserID int	=0
AS
declare @WOspacialchars varchar(75) 
set @WOspacialchars = dbo.RemoveSpecialChars(@SearchText)
declare @text varchar(max)
BEGIN
	SET NOCOUNT ON;
				 
	IF(@SearchText='')
		BEGIN
			SET @text = '	SELECT top 100 j.ID, j.fDesc AS fDesc, l.Tag AS Tag, Chart.Acct, Chart.fDesc AS DefaultAcct, j.GL AS GLExp
								FROM [dbo].[Job] as j LEFT JOIN [dbo].[Loc] as l ON j.[Loc]=l.[Loc] 
										LEFT JOIN Chart ON j.GL = Chart.ID
										INNER JOIN Rol ON l.Rol = Rol.ID
				 LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = Rol.EN
								WHERE  j.[Status] in  (0,3) AND l.Tag is not null and j.fDesc is not null and Chart.Acct is not null and j.GL is not null  '

								IF(@EN = 1)  --If Company feature is Active
				BEGIN
					set @text+=' and UC.IsSel = 1 and UC.UserID ='+convert(nvarchar(50),@UserID)
				END

			IF(@IsJob = 1)
			BEGIN
				SET @text += '	ORDER BY j.ID	'
			END
			ELSE
			BEGIN
				SET @text += '	ORDER BY l.Tag	'
			END
			
		END
	ELSE
		BEGIN
			SET @text = '	SELECT top 100 j.ID, j.fDesc AS fDesc, l.Tag AS Tag, Chart.Acct, Chart.fDesc AS DefaultAcct, j.GL AS GLExp
								FROM [dbo].[Job] as j LEFT JOIN [dbo].[Loc] as l ON j.[Loc]=l.[Loc]
									LEFT JOIN Chart ON j.GL = Chart.ID
									INNER JOIN Rol ON l.Rol = Rol.ID
				 LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = Rol.EN 
								WHERE   j.[Status] in  (0,3) AND l.Tag is not null and j.fDesc is not null and Chart.Acct is not null and j.GL is not null  AND ((dbo.RemoveSpecialChars(j.fDesc) LIKE ''%'+@WOspacialchars+'%'') OR 
								(dbo.RemoveSpecialChars(l.Tag) LIKE ''%'+ @WOspacialchars +'%'')	OR	
								(dbo.RemoveSpecialChars(j.ID) LIKE ''%'+ @WOspacialchars +'%''))	'

								IF(@EN = 1)  --If Company feature is Active
				BEGIN
					set @text+=' and UC.IsSel = 1 and UC.UserID ='+convert(nvarchar(50),@UserID)
				END

			IF(@IsJob = 1)
			BEGIN
				SET @text += '	ORDER BY j.ID	'
			END
			ELSE
			BEGIN
				SET @text += '	ORDER BY l.Tag	'
			END

		END

		
		EXEC(@text)
END
