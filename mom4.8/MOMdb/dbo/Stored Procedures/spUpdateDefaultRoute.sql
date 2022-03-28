-- =============================================
-- Author:		<Harsh>
-- Create date: <12/28/2018>
-- Description:	<procedure to update dynamic route>
-- =============================================
CREATE PROCEDURE [dbo].[spUpdateDefaultRoute] 
	@Label NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	IF EXISTS(SELECT 1 FROM tblSchedule WHERE Type='DefaultWorker')
	BEGIN
		UPDATE tblSchedule SET Label=@Label WHERE Type='DefaultWorker' 
	END
	ELSE
	BEGIN
		INSERT INTO tblSchedule(Label,Description,Type)VALUES(@Label,'DefaultWorker','DefaultWorker') 
	END
END
