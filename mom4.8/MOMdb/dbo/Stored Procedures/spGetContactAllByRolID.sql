CREATE PROC [dbo].[spGetContactAllByRolID] @Rol INT
AS
BEGIN
    SELECT ID    AS contactid,
           fDesc AS name,
           Phone,
           Fax,
           Cell,
           Email,
		   Title
    FROM   Phone
    WHERE  Rol = @Rol
	--UNION
	--SELECT 0, 
	--	contact, 
	--	Phone, 
	--	Fax, 
	--	Cellular, 
	--	EMail, 
	--	''
	--FROM rol r
	--WHERE r.id =@Rol AND r.Contact is not null AND r.Contact != ''

	select 
		r.ID,
		r.Name,
		r.EN,
		B.Name As Company,
		r.City,
		r.State,
		r.Zip,
		r.Address
	From Rol r left Outer join Branch B on B.ID = r.EN
	WHERE  r.ID = @Rol
END
