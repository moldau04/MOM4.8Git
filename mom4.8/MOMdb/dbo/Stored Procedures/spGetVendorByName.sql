CREATE PROCEDURE [dbo].[spGetVendorByName]
@VendorName varchar(75),
@EN int
AS
    DECLARE @WOspacialchars varchar(50)
    SET @WOspacialchars = dbo.RemoveSpecialChars(@VendorName)
    BEGIN
        SET NOCOUNT ON;
        IF (@EN = 1)
        BEGIN
            SELECT DISTINCT TOP 1
                v.ID AS ID,
                r.Name AS Name--,
                --v.Terms,
                --B.Name AS Company,
                --('ID: ' + v.Acct + ', ' + 'Vendor: ' + r.Name + ', ' + r.Contact + ', ' + r.Address + ', ' + r.City + ', ' + r.[State] + ', ' + r.Zip + ', Phone: ' + r.Phone + ', Email: ' + r.EMail) AS [desc]
            FROM [dbo].[Vendor] v
            JOIN Rol r
                ON v.Rol = r.ID
            LEFT OUTER JOIN tblUserCo UC
                ON UC.CompanyID = r.EN
            LEFT OUTER JOIN Branch B
                ON B.ID = r.EN
            WHERE UC.IsSel = 1
            AND v.Status = 0
            AND dbo.RemoveSpecialChars(r.Name) = ISNULL(@WOspacialchars, '')
            
            ORDER BY r.Name
            --SELECT
            --    @WOspacialchars
        END
        ELSE
        BEGIN
            SELECT DISTINCT TOP 1
                v.ID AS ID,
                r.Name AS Name--,
                --v.Terms,
                --B.Name AS Company,
                --('ID: ' + v.Acct + ', ' + 'Vendor: ' + r.Name + ', ' + r.Contact + ', ' + r.Address + ', ' + r.City + ', ' + r.[State] + ', ' + r.Zip + ', Phone: ' + r.Phone + ', Email: ' + r.EMail) AS [desc]
            FROM [dbo].[Vendor] v
            JOIN Rol r
                ON v.Rol = r.ID
            LEFT OUTER JOIN tblUserCo UC
                ON UC.CompanyID = r.EN
            LEFT OUTER JOIN Branch B
                ON B.ID = r.EN
            WHERE v.Status = 0
            AND dbo.RemoveSpecialChars(r.Name) = ISNULL(@WOspacialchars, '')
            --OR (dbo.RemoveSpecialChars(V.Acct) LIKE '%' + @WOspacialchars + '%')
            --OR (r.Contact LIKE '%' + @VendorName + '%')
            --OR (dbo.RemoveSpecialChars(r.Address) LIKE '%' + @WOspacialchars + '%')
            --OR (r.City LIKE '%' + @VendorName + '%')
            --OR (r.State = +@SearchText)
            --OR (r.Zip LIKE '%' + @VendorName + '%')
            --OR (dbo.RemoveSpecialChars(r.Phone) LIKE '%' + @WOspacialchars + '%')
            --OR (r.EMail LIKE '%' + @VendorName + '%')

            ORDER BY r.Name
        END
    END
GO