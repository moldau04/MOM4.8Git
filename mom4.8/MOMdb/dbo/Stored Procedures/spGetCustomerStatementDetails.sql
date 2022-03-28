CREATE PROCEDURE [dbo].[spGetCustomerStatementDetails]
	@owner int = 0
AS
BEGIN
SELECT TOP 1
	Invoice.Loc,
	Loc.Owner, 
	Rol.Name AS CustomerName, 
	Loc.ID AS LocID,
	Loc.Tag AS LocName, 
	Loc.Address+', '+Char(13)+CHAR(10)+Loc.City+', '+Loc.State+', '+Loc.Zip AS LocAddress,
	(SELECT TOP 1 Rol.Address+', '+ Char(13) + CHAR(10) + Rol.City+', '+Rol.State+', '+Rol.Zip AS CustAddress 
		FROM Rol  
		WHERE Rol.ID=Loc.Rol) AS custAddress,
	Rol.Contact AS custContact,
	Rol.Phone AS custPhone,
	Rol.EMail AS custEMail,
	Loc.Status,
	Loc.Type,
	0.00 AS Total,
	LocRol.Contact AS Contact,
	LocRol.Phone AS Phone,
	LocRol.EMail AS EMail,
	(SELECT Name FROM Terr WHERE ID = Loc.Terr) AS Terr,
	(CASE WHEN ([dbo].[DateDiff](ISNULL(OpenAR.Due, Invoice.DDate)) >= 0) AND ([dbo].[DateDiff](ISNULL(OpenAR.Due, Invoice.DDate)) < 31)
		THEN OpenAR.Balance
		ELSE 0 END) AS ZeroDay,
	(CASE WHEN ([dbo].[DateDiff](ISNULL(OpenAR.Due, Invoice.DDate)) >= 31) AND ([dbo].[DateDiff](ISNULL(OpenAR.Due, Invoice.DDate)) < 61)
		THEN OpenAR.Balance
		ELSE 0 END) AS ThirtyOneDay,
	(CASE WHEN ([dbo].[DateDiff](ISNULL(OpenAR.Due, Invoice.DDate)) >= 61) AND ([dbo].[DateDiff](ISNULL(OpenAR.Due, Invoice.DDate)) < 91)
		THEN OpenAR.Balance
		ELSE 0 END) AS SixtyOneDay,
	(CASE WHEN ([dbo].[DateDiff](ISNULL(OpenAR.Due, Invoice.DDate)) >= 91)
		THEN OpenAR.Balance
		ELSE 0 END) AS NintyOneDay,
	OpenAR.Balance,
	OpenAR.Selected,
	CASE WHEN (Loc.Custom12 = '' OR Loc.Custom12 IS NULL) 
		THEN 0
		ELSE 1 END
	AS IsExistsEmail,
	Loc.Custom12,
	Loc.Custom13
FROM   Invoice 
	INNER JOIN Loc ON Loc.Loc = Invoice.Loc 
	INNER JOIN OpenAR ON OpenAR.Ref = Invoice.Ref AND OpenAR.Type = 0
	INNER JOIN Owner ON Owner.ID = Loc.Owner 
	INNER JOIN Rol ON Rol.Id = Owner.Rol
	INNER JOIN Rol AS LocRol ON Loc.Rol= LocRol.ID
	INNER JOIN Branch B ON B.ID= Rol.EN
	INNER JOIN tblUserCo UC ON UC.CompanyID = Rol.EN 
WHERE Loc.Owner =@owner
									 
END
