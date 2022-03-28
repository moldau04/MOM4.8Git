-- =============================================
-- Author:		<Harsh Dwivedi>
-- Create date: <16th Dec 2018>
-- Description:	<Get supervisor, worker and category for timesheet screen>
-- =============================================
CREATE PROCEDURE [dbo].[spGetTimeCardInput]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT DISTINCT UPPER(Super) AS Super FROM tblwork WHERE STATUS=0 AND Super IS NOT NULL AND Super <>'' ORDER BY Super
	--SELECT fUser FROM tblUser
	 select x.fDesc ,x.id,x.Status,Super from ( select upper(w.fDesc)as fdesc, w.id ,w.Status,w.Super from tblwork w where w.id is not null   and w.status=0  ) x order by x.Status asc ,x.fDesc asc
	SELECT type FROM category ORDER BY TYPE
END

