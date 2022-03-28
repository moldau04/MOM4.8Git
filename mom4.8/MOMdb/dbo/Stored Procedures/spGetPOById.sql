CREATE PROCEDURE [dbo].[spGetPOById] 
	@PO int,
	@EN int,
	@UserID int = 0
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @count int,
            @InventoryHasAvailable int = 0,
            @CustomProgram varchar(150) = '',
            @POApprovalStatus int = 0;

    IF ((SELECT TOP 1 ISNULL(inv, 0) FROM POitem WHERE PO = @PO AND ISNULL(inv, 0) > 0) > 0)
    BEGIN
        SELECT @InventoryHasAvailable = 1;
    END

    SELECT TOP 1 @CustomProgram = ISNULL(Label, '')
    FROM [Custom]
    WHERE ISNULL(Label, '') <> ''
		AND [Name] = 'ProgramCustom';

    SELECT TOP 1 @POApprovalStatus = ISNULL(Status, 0)
    FROM ApprovalStatus
    WHERE PO = @PO;


    CREATE TABLE #temp (
        RowID int IDENTITY (1, 1),
        ID int,
        Line smallint,
        AcctID int,
        fDesc varchar(max),
        Quan numeric(30, 2),
        Price numeric(30, 2),
        Amount numeric(30, 2),
        JobID int,
        JobName varchar(150),
        PhaseID smallint,
        Phase varchar(255),
        Inv int,
        Freight numeric(30, 2),
        Rquan numeric(30, 2),
        Billed int,
        Ticket int,
        Loc varchar(150),
        AcctNo varchar(150),
        Due datetime,
        Usetax numeric(30, 2),
        UName varchar(20),
        UtaxGL varchar(20),
        ItemID int,
        ItemDesc varchar(max),
        TypeID int,
        LocName varchar(75),
        WarehouseID varchar(5),
        LocationID int,
        Warehousefdesc varchar(100),
        Locationfdesc varchar(100),
        OpSq varchar(150),
        ForceClose int

    )

    CREATE TABLE #temp1 (
        Line smallint,
        Job int,
        Phase varchar(255),
        Item varchar(255),
        TypeID int,
        Code varchar(255)
    )

	-- Table 0
    IF @EN = 0
    BEGIN
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
            p.SalesOrderNo,
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
            END) AS StatusName,
            '' AS TC,
            ISNULL(r.EN, 0) EN,
            @InventoryHasAvailable InventoryHasAvailable,
            v.Acct,
            @CustomProgram CustomProgram,
            ISNULL(@POApprovalStatus, 0) POApprovalStatus
			,p.RequestedBy
			,p.Custom1
			,p.Custom2,
            v.Type VendorType
        --FROM PO AS p,
        --     Vendor AS v,
        --     Rol AS r,
        --     tblterms AS t,
        --     Control AS c
		 --WHERE p.Vendor = v.ID
   --     AND v.Rol = r.ID
   --     AND t.ID = p.Terms
   --     AND p.PO = @PO
		FROM PO AS p LEFT JOIN Vendor AS v ON p.Vendor = v.ID LEFT JOIN Rol AS r ON v.Rol = r.ID LEFT JOIN tblterms AS t ON t.ID = p.Terms , Control AS c 
        WHERE p.PO = @PO
    END
    ELSE
    IF @EN = 1
    BEGIN
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
            b.PORemit AS PORemit,
            (CASE p.Status
                WHEN 0 THEN 'Open'
                WHEN 1 THEN 'Closed'
                WHEN 2 THEN 'Void'
                WHEN 3 THEN 'Partial-Quantity'
                WHEN 4 THEN 'Partial-Amount'
                WHEN 5 THEN 'Closed At Received PO'
            END) AS StatusName,
            '' AS TC,
            ISNULL(r.EN, 0) EN,
            @InventoryHasAvailable InventoryHasAvailable,
            v.Acct,
            @CustomProgram CustomProgram,
            ISNULL(@POApprovalStatus, 0) POApprovalStatus,
			 p.RequestedBy
			 ,p.Custom1
			,p.Custom2,
            v.Type VendorType
        --FROM PO AS p,
        --     Vendor AS v,
        --     Rol AS r,
        --     tblterms AS t,
        --     Branch AS b,
        --     tblUserCo AS UC
        --WHERE p.Vendor = v.ID
        --AND v.Rol = r.ID
        --AND UC.IsSel = 1
        --AND UC.UserID = CONVERT(nvarchar(50), @UserID)
        --AND r.EN = b.ID
        --AND t.ID = p.Terms
        --AND p.PO = @PO
        --AND r.Type = 1
		FROM PO AS p LEFT JOIN Vendor AS v ON  p.Vendor = v.ID LEFT JOIN Rol AS r ON v.Rol = r.ID LEFT JOIN tblterms AS t ON t.ID = p.Terms LEFT JOIN Branch AS b ON r.EN = b.ID,
		tblUserCo AS UC
		WHERE p.PO = @PO
        AND r.Type = 1
		AND UC.IsSel = 1
        AND UC.UserID = CONVERT(nvarchar(50), @UserID)
		
    END

    ----------------------$$$$$$$$$$$ GET PO ITEMS $$$$$$$$$$$--------------------------------------------    

    ---------------------Insert data for #temp1-----------------------------------------------------------    
    --Thomas: data in this table will be used for joining code to resolve the performance issue of this query    
    INSERT INTO #temp1 (Line, Job, Phase, Item, TypeID, Code)
        SELECT DISTINCT
            jt.Line,
            jt.Job,
            CASE ISNULL(p.TypeID, 0)
                WHEN 0 THEN (SELECT
                        Type
                    FROM BOMT
                    WHERE ID = b.Type)
                ELSE (SELECT
                        Type
                    FROM BOMT
                    WHERE ID = p.TypeID)
            END AS Phase --bom.Phase    
            -- Thomas: updated 14-Nov-2018    
            --, CASE b.type WHEN 1 THEN isnull(i.Name,'') WHEN 2 THEN isnull(pr.fdesc,'') ELSE isnull(i.Name,'') END AS Item    
            ,
            CASE b.type
                WHEN 2 THEN ISNULL(pr.fdesc, '')
                ELSE ISNULL(i.Name, '')
            END AS Item
            -- End update    
            ,
            ISNULL(p.TypeID, b.Type) AS TypeID,
            jt.Code
        FROM POItem AS p
        LEFT JOIN Job AS j
            ON p.Job = j.ID
        LEFT JOIN JobTItem AS jt
            ON jt.Line = p.Phase
            AND ISNULL(jt.Job, 0) = ISNULL(j.ID, 0)
        INNER JOIN BOM AS b
            ON b.JobTItemID = jt.ID
            AND jt.Type != 2
        -- Rustam: updated 22-Jan-2019    
        LEFT JOIN Inv AS i
            ON i.ID = p.Inv
            AND b.matitem = i.id
        --INNER JOIN Inv as i on i.ID = p.Inv and b.matitem =i.id    
        -- End update    
        LEFT JOIN PRWage AS pr
            ON pr.ID = p.Inv
        WHERE p.PO = @PO

    INSERT INTO #temp (ID, Line, AcctID, fDesc, Quan, Price, Amount, JobID, JobName, PhaseID,
    Phase, Inv, Freight, Rquan, Billed, Ticket, Loc, AcctNo, Due, Usetax, UName, UtaxGL,
    ItemID, ItemDesc, TypeID, LocName, WarehouseID, LocationID, Warehousefdesc, Locationfdesc, OpSq,ForceClose)
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
            CASE ISNULL(p.TypeID, 0)
                WHEN 0 THEN (SELECT
                        Type
                    FROM BOMT
                    WHERE ID = bom.TypeID)
                ELSE (SELECT
                        Type
                    FROM BOMT
                    WHERE ID = p.TypeID)
            END AS Phase --bom.Phase    
            ,
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
            Inv AS ItemID
            -- Thomas: updated 14-Nov-2018    
            --, CASE WHEN bom.Item is null or  bom.Item = '' THEN i.Name Else bom.Item  end AS Item    
            ,
            CASE
                WHEN bom.TypeID = 2 THEN bom.Item COLLATE SQL_Latin1_General_CP1_CS_AS
                ELSE i.Name COLLATE SQL_Latin1_General_CP1_CS_AS
            END AS Item
            -- End update    
            --, bom.Item    
            ,
            ISNULL(p.TypeID, bom.TypeID) AS TypeID--bom.TypeID    
            ,
            (SELECT TOP 1
                tag
            FROM Loc
            WHERE Loc = j.Loc)
            AS locname,
            p.WarehouseID,
            p.LocationID,
            (SELECT TOP 1   Wh.Name AS WarehouseName
            FROM  Warehouse wh
            WHERE  wh.ID=  p.WarehouseID),
            (SELECT TOP 1
                Name
            FROM WHLoc WH
            WHERE WH.WareHouseID = p.WarehouseID
            AND id = p.LocationID),
            bom.Code,
            p.ForceClose
        FROM POItem AS p
        LEFT JOIN Job AS j
            ON p.Job = j.ID
        --LEFT JOIN JobTItem as jt ON jt.Line = p.Phase and jt.Job = j.ID    
        --LEFT JOIN BOM as b ON b.JobTItemID = jt.ID    
        LEFT JOIN Chart AS c
            ON c.ID = p.GL
        LEFT JOIN Loc AS l
            ON l.Loc = j.Loc
        LEFT JOIN Rol AS r
            ON l.Rol = r.ID
        LEFT JOIN Inv AS i
            ON i.ID = p.Inv
        LEFT JOIN PRWage AS pr
            ON pr.ID = p.Inv
        LEFT JOIN
        --(SELECT distinct jt.Line    
        --  , jt.Job    
        --  , isnull((select Type from BOMT where ID = b.Type),'') as Phase    
        --  , CASE b.type WHEN 1 THEN isnull(i.Name,'') WHEN 2 THEN isnull(pr.fdesc,'') ELSE isnull(i.Name,'') END AS Item    
        --  , isnull(b.Type,0) as TypeID    
        --  , jt.Code    
        -- FROM POItem as p     
        -- LEFT JOIN Job as j ON p.Job=j.ID     
        -- LEFT JOIN JobTItem as jt ON jt.Line = p.Phase and isnull(jt.Job,0) = isnull(j.ID,0)     
        -- INNER JOIN BOM as b ON b.JobTItemID = jt.ID    
        -- LEFT JOIN Inv as i on i.ID = p.Inv and b.matitem =i.id    
        -- LEFT JOIN PRWage as pr ON pr.ID = p.Inv    
        -- WHERE p.PO = @PO    
        --)    
        -- Thomas: replaced code above by #temp1 table to resolve performance issue    
        #temp1
        AS bom
            ON bom.Line = p.Phase
            AND ISNULL(bom.Job, 0) = ISNULL(j.ID, 0)
        WHERE p.PO = @PO
        ORDER BY p.Line

    SELECT @count = COUNT(*) FROM #temp

    IF (@count = 0)
    BEGIN
        INSERT INTO #temp (ID, Line, AcctID, fDesc, Quan, Price, Amount, JobID, JobName, PhaseID, Phase,
        Inv, Freight, Rquan, Billed, Ticket, Loc, AcctNo, Due, UseTax, Uname, UtaxGL, ItemID, ItemDesc, TypeID, locname, WarehouseID, LocationID,ForceClose)
            VALUES (0, (@count + 1), 0, '', NULL, NULL, NULL, 0, '', 0, '', 0, 0, 0, 0, NULL, '', '', NULL, NULL, '', '', NULL, '', 0, '', '', 0,0);
    END

	-- Table 1
    SELECT * FROM #temp
END