CREATE PROCEDURE [dbo].[spGetJobCostPOByJob] 
	@job int,
	@phase int,
	@sdate datetime = NULL,
	@edate datetime = NULL
AS
BEGIN
	
	SET NOCOUNT ON;

	DECLARE @text VARCHAR(MAX)


	SET @text = 'SELECT * FROM (
							SELECT
							 ( select top 1 GroupName from tblEstimateGroup where ID= j.GroupId ) as GroupName,
							
							PO.PO, 
								PO.fDate, 
								PO.fDesc, 
								PO.Vendor, 
								Rol.Name AS VendorName, 
								ISNULL(p.Balance,0) AS Balance, 
								j.Type, j.Code,
								(case j.Type when 0 then ''Revenue'' else isnull(BOMT.Type, '''') end)	  as TypeDesc,
								ISNULL((case j.Type when 0 then ''Revenue'' when 1 then ''Cost'' end),'''') as TypeName, 
								ISNULL(j.Budget,0)+ISNULL(j.Modifier,0) AS BMatExp,
								ISNULL(j.ETC,0)+ISNULL(j.ETCMod,0) AS BLabExp,
								ISNULL(b.MatItem,0) AS MatItem, ISNULL(i.Name,'''') AS MatDesc, ISNULL(i.Name,'''') AS ItemDesc, 
								ISNULL(b.LabItem,0) AS LabItem,
								''addpo.aspx?id=''+convert(varchar(50),PO.PO) AS Url
								FROM POItem p INNER JOIN PO ON p.PO = PO.PO
											LEFT JOIN Vendor v ON v.ID = PO.Vendor
											LEFT JOIN Rol ON Rol.ID = v.Rol
											LEFT JOIN JobTItem j ON j.Line = p.Phase AND j.Job = p.Job   and j.type <> 0 
											LEFT JOIN BOM b ON b.JobTItemID = j.ID
											LEFT JOIN Inv i ON i.ID = b.MatItem
											LEFT JOIN BOMT ON BOMT.ID = b.Type
								WHERE PO.Status not IN (1) 
									AND p.Job = '''+ CONVERT(VARCHAR(50), @job) +''' AND p.Phase = ''' + CONVERT(VARCHAR(50),@phase) +'''
		
						UNION

						SELECT
						
						 ( select top 1 GroupName from tblEstimateGroup where ID= j.GroupId ) as GroupName,
						
						PO.PO, PO.fDate, PO.fDesc, PO.Vendor, Rol.Name AS VendorName, 
								ISNULL(rp.Amount,0) AS Balance,
								j.Type, j.Code,
								(case j.Type when 0 then ''Revenue'' else isnull(BOMT.Type, '''') end)	  as TypeDesc,
								ISNULL((case j.Type when 0 then ''Revenue'' when 1 then ''Cost'' end),'''') as TypeName, 
								ISNULL(j.Budget,0)+ISNULL(j.Modifier,0) AS BMatExp,
								ISNULL(j.ETC,0)+ISNULL(j.ETCMod,0) AS BLabExp,
								ISNULL(b.MatItem,0) AS MatItem, ISNULL(i.Name,'''') AS MatDesc, ISNULL(i.Name,'''') AS ItemDesc, 
								ISNULL(b.LabItem,0) AS LabItem,
								''addreceivepo.aspx?id=''+convert(varchar(50),r.ID) AS Url
						FROM RPOItem rp 
								INNER JOIN ReceivePO r ON r.ID = rp.ReceivePO
								INNER JOIN POItem p ON r.PO = p.PO AND rp.POLine = p.Line
										INNER JOIN PO ON p.PO = PO.PO		
										LEFT JOIN Vendor v ON v.ID = PO.Vendor
										LEFT JOIN Rol ON Rol.ID = v.Rol
										LEFT JOIN JobTItem j ON j.Line = p.Phase AND j.Job = p.Job  and j.type <> 0
										LEFT JOIN BOM b ON b.JobTItemID = j.ID
										LEFT JOIN Inv i ON i.ID = b.MatItem
										LEFT JOIN BOMT  ON BOMT.ID = b.Type
									WHERE r.Status  in ( 1 )
										AND p.Job = '''+ CONVERT(VARCHAR(50), @job) +''' AND p.Phase = ''' + CONVERT(VARCHAR(50),@phase) +'''
						)
					AS t '

		IF(@sdate IS NOT NULL and @edate IS NOT NULL and    @edate <> '1/1/0001 12:00:00 AM')
		BEGIN
			SET @text += ' WHERE t.fDate >= '''+ CONVERT(VARCHAR(50),@sdate) +''' AND t.fDate <= '''+ CONVERT(VARCHAR(50),@edate) +''''
		END

			SET @text += ' ORDER BY t.PO	'

		EXEC (@text)

END