CREATE PROCEDURE [dbo].[spGetPhaseExpByJobType] 
	@JobId int ,
	@type smallint ,
	@SearchText varchar(150) 
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @WOspacialchars varchar(50) 
	DECLARE @text1 nvarchar(max)
	SET @WOspacialchars = dbo.RemoveSpecialChars(@SearchText)

	
	SET @text1 = ' SELECT top 50 * FROM ('

	
	IF(@type = 2)		-- Get wage items
	BEGIN
		SET @text1 += ' SELECT	i.ID as ItemID, 
								CASE WHEN it.fDesc != '' THEN  it.fDesc 
														ELSE i.Name
														END AS ItemDesc,
								i.fDesc as ItemDesc1, 
								isnull(it.LocName,'''') as LocName, 
								isnull(it.Line,0) as Line, 
								isnull(it.fDesc,i.fdesc) as fDesc,
								(select count(distinct code) from Jobtitem where fdesc=it.fDesc and job='+CONVERT(varchar(100),@JobId) +') AS CountData
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
								(select count(distinct code) from Jobtitem where fdesc=it.fDesc and job='+ CONVERT(varchar(100),@JobId)+') AS CountData,
								i.type as INVtype, 
								i.LCost as Price ,
								isnull(i.Hand,0) OnHand
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

    SET @text1 +='	WHERE 1=1  and i.Status = 0	 '

	IF(@type != 2)		-- Get inventory items from Inv table.
	BEGIN
	----Ref ESS-122 In the Item column when Inventory is selected in the code show Inv.Type = 0 
	 IF((SELECT Type from BOMT where id=@Type)='Inventory')
	  Begin

	-- if(ISNULL(@JobId,0)=0)

	  SET @text1 += '	AND i.Type IN (0)  '  

	  End 
	 
	END

	SET @text1 += '	) AS t '

	IF(@WOspacialchars != '' or @WOspacialchars != null)
	BEGIN
		 
	SET @text1 +='	WHERE 
		 (
		   dbo.RemoveSpecialChars(t.ItemDesc1) LIKE ''%'+ @WOspacialchars +'%''  
		   OR 
		   dbo.RemoveSpecialChars(t.fDesc) LIKE ''%'+ @WOspacialchars +'%''   
		   OR
		   dbo.RemoveSpecialChars(t.ItemID) LIKE ''%'+ @WOspacialchars +'%''  
		) '
	
	END
	SET @text1 += '		ORDER BY t.Line	DESC	'
	print @text1
	exec(@text1)

END
GO

