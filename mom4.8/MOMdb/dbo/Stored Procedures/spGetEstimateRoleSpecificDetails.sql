CREATE PROCEDURE [dbo].[spGetEstimateRoleSpecificDetails]
	@ID INT
AS
BEGIN
	-- Table 0
	SELECT  ISNULL(r.Address, '') + ', ' + Char(13) + CHAR(10) + ISNULL(r.City, '') + ', ' + ISNULL(r.State, '') + ' '+ ISNULL(r.Zip, '') AS Address,
			ISNULL(r.Phone, '') AS Phone,
			ISNULL(r.Cellular, '') AS Cellular,
			ISNULL(r.Fax, '') AS Fax,
			ISNULL(r.EMail, '') AS EMail,
			ISNULL(r.Contact, '') AS Contact,
			ISNULL((case r.Type
				when 4 then (select top 1 ISNULL(Stax,'0') from Loc where Rol = r.ID)
				when 3 then (select top 1 ISNULL(Stax,'0') from Prospect where Rol = r.ID)
			end),'0') AS STax,
			(case r.Type when 4 then (select Stax.Rate FROM Loc LEFT JOIN Stax ON Loc.STax = STax.Name WHERE Rol = r.ID)
					when 3 then (select Stax.Rate FROM Prospect LEFT JOIN Stax ON Prospect.STax = STax.Name WHERE Rol = r.ID) 
					end
			) as STaxRate

	FROM ROL r WITH(NOLOCK) WHERE r.ID=@ID

	-- Table 1
	SELECT  ISNULL(l.Address, '') + ', ' + Char(13) + CHAR(10) +
			ISNULL(l.City, '') + ', '+
			ISNULL(l.State, '') + ' ' + ISNULL(l.Zip, '')  AS BillAddress,
			L.LOC AS LOCID
    FROM loc l WITH(NOLOCK) WHERE l.ROL=@ID

	-- Table 2
	SELECT ID, fDesc FROM PHONE WHERE PHONE.ROL=@ID
	--UNION
	--SELECT 0, contact FROM Rol WHERE ID = @ID  AND Contact is not null AND Contact != ''

	-- Table 3
	SELECT p.Terr, B.Name As Company
	FROM   Prospect p 
		INNER JOIN Rol r 
        ON r.ID = p.Rol left Outer join Branch B on B.ID = r.EN  where r.ID = @ID
END