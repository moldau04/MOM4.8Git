 
-- =============================================
-- Author:		<NK>
-- Create date: <11 March 2019>
-- Description:	<Get Group Name>
-- =============================================
CREATE PROCEDURE SpGetGroup 
	 @Id int = 0,
	 @SearchValue Varchar(255)=''

AS
BEGIN 
	DECLARE @WOspacialchars Varchar(255)
	SET @WOspacialchars = dbo.RemoveSpecialChars(@SearchValue)
	 --SELECT t1.Id as value, t1.GroupName as label FROM tblEstimateGroup t1   
	SELECT eg.GroupName label, eg.Id as value FROM tblProjectGroup pg INNER JOIN tblEstimateGroup eg ON eg.Id = pg.GroupId
	WHERE pg.ProjectId = @Id and eg.GroupName like '%' + @WOspacialchars + '%'
END
 
