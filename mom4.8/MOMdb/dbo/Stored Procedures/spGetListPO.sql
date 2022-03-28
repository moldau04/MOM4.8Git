CREATE PROCEDURE [dbo].[spGetListPO] 
	@POs NVARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;

	----------------------$$$$$$$$$$$ GET PO $$$$$$$$$$$--------------------------------------------
    SELECT DISTINCT
        p.PO,
        p.fDate,
        p.fDesc,
        p.Amount,
        p.Vendor,
        p.Status,
        CONVERT(varchar(50), p.Due, 101) AS Due,
        p.ShipVia,
        p.Terms AS PaymentTerms,
        p.FOB,
        p.ShipTo,
        p.Approved,
        p.Custom1,
        p.Custom2,
        p.ApprovedBy,
        p.ReqBy,
        p.fBy,
        p.PORevision,
        p.CourrierAcct,
        p.POReasonCode,
        r.Name AS VendorName,
        ISNULL(r.Address,'') + ', ' + CHAR(13) + ISNULL(r.City,'') + ', ' + ISNULL(r.State,'') + ' ' + ISNULL(r.Zip,'') AS Address,
        r.EMail,
        r.City AS VendorCity,
        r.State AS VendorState,
        r.Zip AS VendorZip,
        r.Address AS VendorAddress,
        t.Name AS Terms,
        ISNULL(c.Name,'') + CHAR(13) + CHAR(10) + ISNULL(c.Address,'') + ', ' + CHAR(13) + CHAR(10) + ISNULL(c.City,'') + ', ' + ISNULL(c.State,'') + ' ' + ISNULL(c.Zip,'') AS PORemit,
        (CASE p.Status
            WHEN 0 THEN 'Open'
            WHEN 1 THEN 'Closed'
            WHEN 2 THEN 'Void'
            WHEN 3 THEN 'Partial-Quantity'
            WHEN 4 THEN 'Partial-Amount'
            WHEN 5 THEN 'Closed At Received PO'
        END) AS StatusName
    FROM PO AS p,
            Vendor AS v,
            Rol AS r,
            tblterms AS t,
            Control AS c
    WHERE p.Vendor = v.ID
		AND v.Rol = r.ID
		AND t.ID = p.Terms
		AND p.PO IN (SELECT SplitValue FROM [dbo].[fnSplit](@POs,','))

    ----------------------$$$$$$$$$$$ GET PO ITEMS $$$$$$$$$$$--------------------------------------------    

    SELECT DISTINCT
        p.PO AS ID,
        p.Line,
        p.GL AS AcctID,
        p.fDesc AS fDesc,
        p.Quan,
        p.Price,
        p.Amount,
        p.Job AS JobID,
        CONVERT(nvarchar(50), p.Job) + ', ' + j.fdesc AS JobName,
        p.Phase AS PhaseID,
        p.Inv,
        p.Freight,
        p.Rquan,
        p.Billed,
        CASE p.Ticket
            WHEN 0 THEN NULL
            ELSE p.Ticket
        END AS Ticket,
        r.Name AS Loc,
        c.Acct + ' - ' + c.fDesc AS AcctNo,
        p.Due,
        0.00 AS UseTax,
        '' AS Uname,
        '' AS UtaxGL,
        Inv AS ItemID,
        (SELECT TOP 1 tag FROM Loc WHERE Loc = j.Loc) AS locname,
        p.WarehouseID,
        p.LocationID
    FROM POItem AS p
		LEFT JOIN Job AS j ON p.Job = j.ID   
		LEFT JOIN Chart AS c ON c.ID = p.GL
		LEFT JOIN Loc AS l ON l.Loc = j.Loc
		LEFT JOIN Rol AS r ON l.Rol = r.ID
		LEFT JOIN Inv AS i ON i.ID = p.Inv
		LEFT JOIN PRWage AS pr ON pr.ID = p.Inv    
    WHERE p.PO IN (SELECT SplitValue FROM [dbo].[fnSplit](@POs,','))
END