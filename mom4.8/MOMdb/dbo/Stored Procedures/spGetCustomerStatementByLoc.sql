CREATE PROCEDURE [dbo].[spGetCustomerStatementByLoc]
	@Loc VARCHAR(200),
	@UserID int		= 0
AS
BEGIN
	
	SET NOCOUNT ON;
	declare @text varchar(max)
	declare @text1 varchar(max)


	set @text = 'SELECT  t.Owner, 
					t.Loc, 
					t.LocID, 
					t.locname, 
					t.locAddress,
					t.customerName, 
					t.custAddress, 
					case when t.Status = 0 then ''Active'' else ''Inactive'' end as Status, 
					t.Type, 
					t.Terr, 
					t.Contact, 
					t.Phone,
					t.EMail,
					sum(isnull(t.balance,0)) as Total,
					sum(isnull(t.Selected,0)) as Selected,
					sum(isnull(t.ZeroDay,0)) as ZeroDay,
					sum(isnull(t.ThirtyOneDay,0)) as ThirtyOneDay,
					sum(isnull(t.SixtyOneDay,0)) as SixtyOneDay,
					sum(isnull(t.NintyOneDay,0)) as NintyOneDay,
					IsExistsEmail,
					t.Custom12,
					t.Custom13
			 FROM 
				(

				SELECT
							Invoice.loc,
							Loc.owner, 
							(SELECT TOP 1 Name 
									FROM   rol 
									WHERE  ID = (SELECT TOP 1 Rol 
												 FROM   Owner 
												 WHERE  ID = Loc.Owner
												 )
							) AS CustomerName, 
						   Loc.ID								   AS LocID,
						   Loc.tag                               AS LocName, 
						   Loc.Address+'', ''+Char(13)+CHAR(10)+Loc.City+'', ''+Loc.State+'', ''+Loc.Zip as LocAddress,
						   (SELECT TOP 1 Rol.Address+'', ''+ Char(13) + CHAR(10) + Rol.City+'', ''+Rol.State+'', ''+Rol.Zip AS CustAddress 
								FROM Rol  
								WHERE Rol.ID=Loc.Rol) as custAddress,
						   Loc.Status,
						   Loc.Type,
						   (SELECT Contact FROM Rol WHERE ID = Loc.Rol) AS Contact,
						   (SELECT Phone FROM Rol WHERE ID = Loc.Rol) AS Phone,
						   (SELECT EMail FROM Rol WHERE ID = Loc.Rol) AS EMail,
						   (SELECT Name FROM Terr WHERE ID = Loc.Terr) As Terr,
						   (CASE WHEN ([dbo].[DateDiff](ISNULL(OpenAR.Due, Invoice.DDate)) >= 0) AND ([dbo].[DateDiff](ISNULL(OpenAR.Due, Invoice.DDate)) < 31)
						   then
								OpenAR.Balance
							else 0 end) as ZeroDay,
							(CASE WHEN ([dbo].[DateDiff](ISNULL(OpenAR.Due, Invoice.DDate)) >= 31) AND ([dbo].[DateDiff](ISNULL(OpenAR.Due, Invoice.DDate)) < 61)
						   then
								OpenAR.Balance
							else 0 end) as ThirtyOneDay,
							(CASE WHEN ([dbo].[DateDiff](ISNULL(OpenAR.Due, Invoice.DDate)) >= 61) AND ([dbo].[DateDiff](ISNULL(OpenAR.Due, Invoice.DDate)) < 91)
						   then

								OpenAR.Balance
							else 0 end) as SixtyOneDay,
							(CASE WHEN ([dbo].[DateDiff](ISNULL(OpenAR.Due, Invoice.DDate)) >= 91)
						   then
								OpenAR.Balance
							else 0 end) as NintyOneDay,
				
							   OpenAR.Balance,
							   OpenAR.Selected,
							   CASE WHEN (Loc.Custom12 = '''' OR Loc.Custom12 IS NULL) 
										THEN 0
										ELSE 1 END
									AS IsExistsEmail,
									Loc.Custom12,
									Loc.Custom13
					FROM   Invoice 
						   INNER JOIN Loc 
								   ON Loc.Loc = Invoice.Loc 
						   INNER JOIN OpenAR 
								   ON OpenAR.Ref = Invoice.Ref AND OpenAR.Type = 0
					WHERE Invoice.Status <> 1 AND Invoice.Status <> 2 
					AND OpenAR.Balance <> 0		'

		
		set @text +=' UNION ALL

					SELECT
							OpenAR.loc,
							Loc.owner, 
							(SELECT TOP 1 Name 
									FROM   rol 
									WHERE  ID = (SELECT TOP 1 Rol 
												 FROM   Owner 
												 WHERE  ID = Loc.Owner
												 )
							) AS CustomerName, 
						   Loc.ID								   AS LocID,
						   Loc.tag                               AS LocName, 
						   Loc.Address+'', ''+Char(13)+CHAR(10)+Loc.City+'', ''+Loc.State+'', ''+Loc.Zip as LocAddress,
						   (SELECT TOP 1 Rol.Address+'', ''+ Char(13) + CHAR(10) + Rol.City+'', ''+Rol.State+'', ''+Rol.Zip AS CustAddress 
								FROM Rol  
								WHERE Rol.ID=Loc.Rol) as custAddress,
						   Loc.Status,
						   Loc.Type,
						   (SELECT Contact FROM Rol WHERE ID = Loc.Rol) AS Contact,
						   (SELECT Phone FROM Rol WHERE ID = Loc.Rol) AS Phone,
						   (SELECT EMail FROM Rol WHERE ID = Loc.Rol) AS EMail,
						   (SELECT Name FROM Terr WHERE ID = Loc.Terr) As Terr,
						   (CASE WHEN ([dbo].[DateDiff](OpenAR.Due) >= 0) AND ([dbo].[DateDiff](OpenAR.Due) < 31)
						   then
								OpenAR.Balance
							else 0 end) as ZeroDay,
							(CASE WHEN ([dbo].[DateDiff](OpenAR.Due) >= 31) AND ([dbo].[DateDiff](OpenAR.Due) < 61)
						   then
								OpenAR.Balance
							else 0 end) as ThirtyOneDay,
							(CASE WHEN ([dbo].[DateDiff](OpenAR.Due) >= 61) AND ([dbo].[DateDiff](OpenAR.Due) < 91)
						   then

								OpenAR.Balance
							else 0 end) as SixtyOneDay,
							(CASE WHEN ([dbo].[DateDiff](OpenAR.Due) >= 91)
						   then
								OpenAR.Balance
							else 0 end) as NintyOneDay,
				
							   OpenAR.Balance,
							   OpenAR.Selected,
							   CASE WHEN (Loc.Custom12 = '''' OR Loc.Custom12 IS NULL) 
										THEN 0
										ELSE 1 END
									AS IsExistsEmail,
									Loc.Custom12,
									Loc.Custom13
					FROM   OpenAR INNER JOIN Loc 
									ON Loc.Loc = OpenAR.Loc
					WHERE OpenAR.Type IN (2,3) AND OpenAR.Balance <> 0  '

	

		set @text += ' ) t
						GROUP BY t.CustomerName, 
								t.Owner, 
								t.Loc,
								t.LocID, 
								t.LocName, 
								t.LocAddress, 
								t.CustAddress, 
								t.Status, 
								t.Type, 
								t.Terr, 
								t.Contact,
								t.Phone,
								t.EMail,
								t.IsExistsEmail,
								t.Custom12,
								t.Custom13 Having t.Loc='+@Loc
		print(@text)
		exec (@text)
	
END