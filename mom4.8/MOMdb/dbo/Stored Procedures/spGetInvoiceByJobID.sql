-- =============================================
-- Author:		<Harsh Dwivedi>
-- Create date: <17th Jan 2019>
-- Description:	<Auto complete project by id,desc>
-- =============================================
CREATE PROCEDURE [dbo].[spGetInvoiceByJobID]
	@SearchText NVARCHAR(100)
AS
BEGIN
	--  SET NOCOUNT ON added to prevent extra result sets from
	--  interfering with SELECT statements.
	    SET NOCOUNT ON;
	    SELECT DISTINCT TOP 100 j.ID,j.fDesc,c.Loc,c.Tag,
	    (SELECT TOP 1 Name FROM   rol
		WHERE  ID = (SELECT TOP 1 Rol
        FROM   Owner
        WHERE  ID = c.Owner)) AS Name,
	    (ISNULL( c.Address,'') + ', ' +ISNULL( c.City,'') + ', '+ISNULL( c.[State],'') + ', ' +ISNULL( c.Zip,''))  AS  Address	
		, isnull(( select 1 from Contract where job=j.ID ),0) as IsContract
		FROM Job j
		INNER JOIN Loc c ON j.Loc=c.Loc
		INNER JOIN Rol r ON r.ID=c.Rol		  
		LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = r.EN
		WHERE j.Status in (0,3) AND 
		(j.fDesc LIKE '%'+@SearchText+'%' OR c.Tag LIKE '%'+@SearchText+'%'
		OR CAST(c.Loc AS NVARCHAR)=@SearchText OR CAST(j.ID AS NVARCHAR)=@SearchText)
END
