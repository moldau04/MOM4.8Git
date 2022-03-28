CREATE PROCEDURE [dbo].[Report_GetCompanyDetails]
AS
	SET NOCOUNT ON;
	SELECT Name,Address,Contact,Email,ISNULL(Logo,'') AS Logo,City,State,Zip,Phone,Fax,GSTreg,YE,Version,CDesc,MSM,DSN,Username,Password,Remarks,BusinessStart,BusinessEnd FROM Control
RETURN
