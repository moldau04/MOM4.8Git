CREATE PROCEDURE [dbo].[spGetCustomerStatementCollection]
	@LocationIDs Varchar(2000),
	@CustomerIDs Varchar(2000),
	@IsOverDue bit ,
	@EN int,
	@UserID int = 0,
	@IncludeCredit bit,
	@IncludeCustomerCredit bit
AS
BEGIN
	
	SET NOCOUNT ON;
	declare @text varchar(max)
	declare @text1 varchar(max)

	IF @EN = 0
	BEGIN
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
						   (SELECT Name FROM Terr WHERE ID = Loc.Terr) As Terr,
						   (CASE WHEN ([dbo].[DateDiff](ISNULL(OpenAR.Due, Invoice.DDate)) < 31)
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
					AND OpenAR.Balance <> 0	AND Loc.NoCustomerStatement <> 1	'

		if (@IncludeCustomerCredit = 0)
		begin
			set @text += ' AND OpenAR.loc NOT IN (SELECT DISTINCT Loc from OpenAR WHERE (Balance <> 0 AND Type = 2) OR Balance < 0) '
		end
		else
		begin
			if(@IncludeCredit  = 0)
			begin
				set @text += ' AND OpenAR.Balance > 0 '
			end
		end

		if (@IsOverDue = '1')
		begin
			set @text += '	AND [dbo].[DateDiff](ISNULL(OpenAR.Due, Invoice.DDate)) > 0		'
		end
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
						   (SELECT Name FROM Terr WHERE ID = Loc.Terr) As Terr,
						   (CASE WHEN ([dbo].[DateDiff](OpenAR.Due) < 31)
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
					WHERE OpenAR.Balance <> 0 AND Loc.NoCustomerStatement <> 1 '

		if (@IncludeCustomerCredit = 0)
		begin
			set @text += ' AND OpenAR.loc NOT IN (SELECT DISTINCT Loc from OpenAR WHERE (Balance <> 0 AND Type = 2) OR Balance < 0) '
		end
		else
		begin
			if(@IncludeCredit  = 0)
			begin
				set @text += ' AND OpenAR.Balance > 0 '
			end
		end

		if(@IncludeCredit  = 1)
		begin
			set @text += ' AND OpenAR.Type IN (2,3) '
		end
		else
		begin
			set @text += ' AND OpenAR.Type IN (3) '
		end
		
		if (@IsOverDue = '1')
		begin
			set @text += '	AND [dbo].[DateDiff](OpenAR.Due) > 0		'
		end

		---------------------------------------------------------------
		if (@IncludeCustomerCredit = 1 AND @IncludeCredit  = 1)
		begin
			set @text += '	UNION ALL

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
				(SELECT Name FROM Terr WHERE ID = Loc.Terr) As Terr,
				(CASE WHEN ([dbo].[DateDiff](OpenAR.Due) < 31)
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
			FROM   OpenAR 
			INNER JOIN Loc ON Loc.Loc = OpenAR.Loc
			INNER JOIN Dep  ON Dep.Ref = OpenAR.Ref                  
			WHERE Loc.NoCustomerStatement <> 1  
			AND OpenAR.Balance <> 0  
			AND OpenAR.Type = 1 AND OpenAR.InvoiceID IS NULL'

			if (@IsOverDue = '1')
			begin
				set @text += '	AND [dbo].[DateDiff](OpenAR.Due) > 0		'
			end
		end

		set @text += ' ) t'
		SET @text= @text+' WHERE 1=1 '

		IF @CustomerIDs <> ''
		BEGIN
			SET @text= @text+' and t.Owner IN('+@CustomerIDs+') '
		END

		IF @LocationIDs <> ''
		BEGIN
			SET @text= @text+' and t.Loc IN('+@LocationIDs+') '
		END
		SET @text= @text+'	GROUP BY t.CustomerName, 
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
								t.IsExistsEmail,
								t.Custom12,
								t.Custom13'
		exec (@text)
		END
		ELSE IF @EN = 1
		BEGIN
		set @text1 = 'SELECT  t.Owner, 
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
						   (SELECT Name FROM Terr WHERE ID = Loc.Terr) As Terr,
						   (CASE WHEN ([dbo].[DateDiff](ISNULL(OpenAR.Due, Invoice.DDate)) < 31)
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
								   INNER JOIN rol lr
										ON Loc.Rol = lr.id 
										inner join Branch B on B.ID= lr.EN
										inner join tblUserCo UC on UC.CompanyID = lr.EN 
									 
					WHERE Invoice.Status <> 1 AND Invoice.Status <> 2 
					and UC.IsSel = 1 and UC.UserID ='+convert(nvarchar(50),@UserID) +'
					AND OpenAR.Balance <> 0	AND Loc.NoCustomerStatement <> 1	'

		if (@IncludeCustomerCredit = 0)
		begin
			set @text1 += ' AND OpenAR.loc NOT IN (SELECT DISTINCT Loc from OpenAR WHERE (Balance <> 0 AND Type = 2) OR Balance < 0) '
		end
		else
		begin
			if(@IncludeCredit  = 0)
			begin
				set @text1 += ' AND OpenAR.Balance > 0 '
			end
		end

		if (@IsOverDue = '1')
		begin
			set @text1 += '	AND [dbo].[DateDiff](ISNULL(OpenAR.Due, Invoice.DDate)) > 0		'
		end
		set @text1 +=' UNION ALL

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
						   (SELECT Name FROM Terr WHERE ID = Loc.Terr) As Terr,
						   (CASE WHEN ([dbo].[DateDiff](OpenAR.Due) < 31)
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
									INNER JOIN rol lr
										ON Loc.Rol = lr.id 
										inner join Branch B on B.ID= lr.EN
										inner join tblUserCo UC on UC.CompanyID = lr.EN 
		WHERE OpenAR.Balance <> 0 AND Loc.NoCustomerStatement <> 1 ' +
		' and UC.IsSel = 1 and UC.UserID ='+convert(nvarchar(50),@UserID) 

		if (@IncludeCustomerCredit = 0)
		begin
			set @text1 += ' AND OpenAR.loc NOT IN (SELECT DISTINCT Loc from OpenAR WHERE (Balance <> 0 AND Type = 2) OR Balance < 0) '
		end
		else
		begin
			if(@IncludeCredit  = 0)
			begin
				set @text1 += ' AND OpenAR.Balance > 0 '
			end
		end

		if(@IncludeCredit  = 1)
		begin
			set @text1 += ' AND OpenAR.Type IN (2,3) '
		end
		else
		begin
			set @text1 += ' AND OpenAR.Type IN (3) '
		end
	
		if (@IsOverDue = '1')
		begin
			set @text1 += '	AND [dbo].[DateDiff](OpenAR.Due) > 0		'
		end

		---------------------------------------------------------------
		if (@IncludeCustomerCredit = 1 AND @IncludeCredit  = 1)
		begin
			set @text1 += '	UNION ALL

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
				(SELECT Name FROM Terr WHERE ID = Loc.Terr) As Terr,
				(CASE WHEN ([dbo].[DateDiff](OpenAR.Due) < 31)
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
			FROM   OpenAR 
			INNER JOIN Loc ON Loc.Loc = OpenAR.Loc
			INNER JOIN Dep  ON Dep.Ref = OpenAR.Ref           
			INNER JOIN Rol lr on lr.Id = Loc.Rol      
			inner join Branch B on B.ID= lr.EN            
			inner join tblUserCo UC on UC.CompanyID = lr.EN 
			WHERE Loc.NoCustomerStatement <> 1  
			AND OpenAR.Balance <> 0  
			AND OpenAR.Type = 1 AND OpenAR.InvoiceID IS NULL
			and UC.IsSel = 1 
			and UC.UserID =' + Convert(nvarchar(50),@UserID) 

			if (@IsOverDue = '1')
			begin
				set @text1 += '	AND [dbo].[DateDiff](OpenAR.Due) > 0		'
			end
		end

		set @text1 += ' ) t'
		SET @text1= @text1+' WHERE 1=1 '
		IF @CustomerIDs <> ''
		BEGIN
			SET @text1= @text1+' and t.Owner IN('+@CustomerIDs+') '
		END

		IF @LocationIDs <> ''
		BEGIN
			SET @text1= @text1+' and t.Loc IN('+@LocationIDs+') '
		END
		SET @text1= @text1+'	GROUP BY t.CustomerName, 
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
								t.IsExistsEmail,
								t.Custom12,
								t.Custom13'
		exec (@text1)
		END
END