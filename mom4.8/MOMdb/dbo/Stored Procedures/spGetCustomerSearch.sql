CREATE PROC [dbo].[spGetCustomerSearch] @SearchText varchar(50),
	@Prospects int,
	@EN int = 0,
	@UserID int = 0,
	@IsSalesAsigned int = 0

AS

    DECLARE @WOspacialchars varchar(50),
            @SalesAsignedTerrID int = 0
    IF (@IsSalesAsigned > 0)----If USER IS Salesperson
        SELECT @SalesAsignedTerrID = ISNULL(id, 0)
        FROM Terr
        WHERE Name = (SELECT fUser FROM tblUser WHERE id = @IsSalesAsigned)

    SET @WOspacialchars = dbo.RemoveSpecialChars(@SearchText)

    SELECT TOP 50
        *,
        --(select top 1 Rol from loc where Owner=nk.value) as rolid,
        CASE nk.prospect
            WHEN 1 THEN (SELECT TOP 1 Rol FROM Prospect WHERE ID = nk.value)
            ELSE (SELECT TOP 1 Rol FROM loc WHERE Owner = nk.value)
        END AS rolid,
        (SELECT CONVERT(varchar(50), Rate) FROM STax WHERE UType = 0 AND NAME = (SELECT TOP 1 STax FROM loc WHERE Owner = nk.value)) AS STaxRate,
        (SELECT TOP 1 STax FROM loc WHERE Owner = nk.value) AS STax,
        '' AS lName

    FROM (
		SELECT TOP 50 0 AS prospect
		, o.ID AS value
		, r.Name AS label
		, (ISNULL(r.Contact, '') + ', ' + ISNULL(r.Address, '') + ', ' + ISNULL(r.City, '') + ', ' + ISNULL(r.[State], '') + ', ' + ISNULL(r.Zip, '') + ', Phone: ' + ISNULL(r.Phone, '') + ', Email: ' + ISNULL(r.EMail, '')) AS [desc]
		, Case When (select count(*) from Loc where Owner = o.ID ) = 1 then l.Tag
			else '' end as lName
		, Case When (select count(*) from Loc where Owner = o.ID ) = 1 then ISNULL(bt.Description,'')
			else '' end as BusinessType
		FROM [Owner] o
		LEFT OUTER JOIN Rol r ON o.Rol = r.ID
		LEFT OUTER JOIN Loc l ON l.Owner = o.ID
		LEFT OUTER JOIN BusinessType bt ON bt.ID = l.BusinessType
		LEFT OUTER JOIN Rol rl ON l.Rol = rl.ID
		LEFT OUTER JOIN tblUserCo UC ON UC.CompanyID = r.EN
		WHERE o.status = 0
			--If User is company Access
			AND ISNULL(UC.IsSel, 0) = CASE @EN WHEN 1 THEN 1 ELSE ISNULL(UC.IsSel, 0) END
			AND ISNULL(UC.UserID, 0) = CASE @EN WHEN 1 THEN @UserID ELSE ISNULL(UC.UserID, 0) END
			AND (
				-------For Default Salesperson
				ISNULL(l.Terr, 0) = (CASE
										WHEN (@IsSalesAsigned > 0 AND
											@SalesAsignedTerrID > 0) THEN CONVERT(nvarchar(10), @SalesAsignedTerrID)
										ELSE ISNULL(l.Terr, 0)
										END)
				OR
				-------For Second Salesperson
				ISNULL(l.Terr2, 0) = (CASE
										WHEN (@IsSalesAsigned > 0 AND
											@SalesAsignedTerrID > 0) THEN CONVERT(nvarchar(10), @SalesAsignedTerrID)
										ELSE ISNULL(l.Terr2, 0)
										END)
			)
			AND (
				(dbo.RemoveSpecialChars(r.NAME) LIKE '%' + @WOspacialchars + '%')
				OR (r.Contact LIKE '%' + @SearchText + '%')
				OR (dbo.RemoveSpecialChars(r.Address) LIKE '%' + @WOspacialchars + '%')
				OR (r.City LIKE '%' + @SearchText + '%')
				OR (r.Zip LIKE '%' + @SearchText + '%')
				OR (dbo.RemoveSpecialChars(r.Phone) LIKE '%' + @SearchText + '%')
				OR (r.EMail LIKE '%' + @SearchText + '%')
				OR (r.state = @SearchText)
				OR (dbo.RemoveSpecialChars(l.tag) LIKE '%' + @WOspacialchars + '%')
				OR (dbo.RemoveSpecialChars(l.ID) LIKE '%' + @WOspacialchars + '%')
				OR (dbo.RemoveSpecialChars(l.Address) LIKE '%' + @WOspacialchars + '%')
				OR (l.City LIKE '%' + @SearchText + '%')
				OR (l.Zip LIKE '%' + @SearchText + '%')
				OR (l.State = @SearchText)
				OR (rl.Contact LIKE '%' + @SearchText + '%')
				OR (dbo.RemoveSpecialChars(rl.Address) LIKE '%' + @WOspacialchars + '%')
				OR (rl.City LIKE '%' + @SearchText + '%')
				OR (rl.Zip LIKE '%' + @SearchText + '%')
				OR (dbo.RemoveSpecialChars(rl.Phone) LIKE '%' + @SearchText + '%')
				OR (rl.EMail LIKE '%' + @SearchText + '%')
				OR (rl.state = @SearchText)
			)
		UNION
		SELECT TOP 50
			1 AS prospect
			, o.ID AS value
			, o.CustomerName AS label --r.Name as label
			, (ISNULL(r.Contact, '') + ', ' + ISNULL(r.Address, '') + ', ' + ISNULL(r.City, '') + ', ' + ISNULL(r.[State], '') + ', ' + ISNULL(r.Zip, '') + ', Phone: ' + ISNULL(r.Phone, '') + ', Email: ' + ISNULL(r.EMail, '')) AS [desc]
			, r.Name as lName
			, ISNULL(o.BusinessType,'') BusinessType
		FROM Prospect o
		LEFT OUTER JOIN Rol r ON o.Rol = r.ID
		LEFT OUTER JOIN tblUserCo UC ON UC.CompanyID = r.EN
		WHERE o.status = 0
			--If User is company Access
			AND ISNULL(UC.IsSel, 0) = CASE @EN WHEN 1 THEN 1 ELSE ISNULL(UC.IsSel, 0) END
			AND ISNULL(UC.UserID, 0) = CASE @EN WHEN 1 THEN @UserID ELSE ISNULL(UC.UserID, 0) END
			AND (
				(dbo.RemoveSpecialChars(NAME) LIKE '%' + @WOspacialchars + '%')
				OR (Contact LIKE '%' + @SearchText + '%')
				OR (dbo.RemoveSpecialChars(r.Address) LIKE '%' + @WOspacialchars + '%')
				OR (r.City LIKE '%' + @SearchText + '%')
				OR (r.City LIKE '%' + @SearchText + '%')
				OR (o.CustomerName LIKE '%' + @SearchText + '%')
				OR (dbo.RemoveSpecialChars(r.Phone) LIKE '%' + @WOspacialchars + '%')
				OR (r.EMail LIKE '%' + @SearchText + '%')
				OR (r.State = @SearchText)
			)
			AND ISNULL(o.Terr, 0) = (CASE
				WHEN (@IsSalesAsigned > 0 AND
					@SalesAsignedTerrID > 0) THEN CONVERT(nvarchar(10), @SalesAsignedTerrID)
				ELSE ISNULL(o.Terr, 0)
				END)
		ORDER BY r.name
	) NK
	WHERE ISNULL(nk.prospect, 0) = CASE @Prospects WHEN 0 THEN 0 ELSE ISNULL(nk.prospect, 0) END