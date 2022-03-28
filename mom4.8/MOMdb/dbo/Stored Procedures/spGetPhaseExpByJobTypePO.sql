CREATE PROCEDURE [dbo].[spGetPhaseExpByJobTypePO] 
	@JobId int,
	@type smallint,
	@SearchText varchar(150)
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @WOspacialchars varchar(50) 
	DECLARE @text1 nvarchar(max)
	SET @WOspacialchars = dbo.RemoveSpecialChars(@SearchText)

	
	SET @text1 = ' SELECT top 100 * FROM ('

	
	IF(@type = 2)		-- Get wage items
	BEGIN
		SET @text1 += ' SELECT	i.ID as ItemID, 
								CASE WHEN it.fDesc != '' THEN  it.fDesc 
														ELSE i.Name
														END AS ItemDesc,
								i.fDesc as ItemDesc1, 
								isnull(it.LocName,'''') as LocName, 
								isnull(it.Line,0) as Line, 
								isnull(it.fDesc,i.fdesc) as fDesc 
									FROM PRWage as i '
	END
	ELSE				-- Get inventory
	BEGIN
		SET @text1 += ' SELECT  i.ID as ItemID, 
								CASE WHEN it.fDesc != '''' THEN  it.fDesc 
									ELSE i.Name
									END AS ItemDesc,
								i.Name as ItemDesc1, 
								isnull(it.LocName,'''') as LocName, 
								isnull(it.Line,0) as Line, 
								isnull(replace(i.fdesc,''<br />'',''''),0) as fDesc, 
								i.LCost as Price 
									FROM Inv as i '
	END
	SET @text1 += 'full outer join 

					(SELECT jt.ID, jt.JobT, jt.Job, jt.Type as jobType, jt.fDesc, jt.Code, jt.Actual, jt.Line , j.fDesc as JobName, 
											r.Name as LocName,
											(case b.type when 2 then isnull(b.LabItem,0) else isnull(b.MatItem,0) end) as ItemID, 
											CASE b.type WHEN 1 THEN  isnull(i.Name,'''')
											WHEN 2 THEN isnull(p.fdesc,'''')
											ELSE '''' END as ItemDesc, 
											b.type 
												FROM JobTItem as jt INNER JOIN Job as j ON jt.Job = j.ID
												INNER JOIN Loc as l ON j.Loc = l.Loc
												INNER JOIN Rol as r ON r.ID = l.Rol
												INNER JOIN BOM as b ON b.JobTItemID = jt.ID
												LEFT JOIN Inv as i ON i.ID = b.MatItem
												LEFT JOIN PRWage as p on p.ID = b.LabItem
													WHERE  jt.Job = '''+ convert(nvarchar(50), @JobId) +'''
													AND b.Type = '''+ convert(nvarchar(50), @Type) +'''
						) as it 
						ON i.ID = it.ItemID AND it.Type = '''+ convert(nvarchar(50), @Type) +''''
	--IF(@type != 2)		-- Get inventory items from Inv table.
	--BEGIN
	--	SET @text1 += '	WHERE i.Type IN (0,2)	'		-- inv.type = 0 Inventory and inv.type = 2 Non inventory
	--END

	SET @text1 += '	) as t '

	IF(@WOspacialchars != '' or @WOspacialchars != null)
	BEGIN
		SET @text1 +='	WHERE  (dbo.RemoveSpecialChars(t.ItemDesc1) LIKE '''+ @WOspacialchars +''') '
		--SET @text1 +='	WHERE  (dbo.RemoveSpecialChars(t.ItemDesc) LIKE ''%'+ @WOspacialchars +'%'') '
	
	END
	SET @text1 += '		ORDER BY t.Line	DESC	'
	print @text1
	exec(@text1)

END