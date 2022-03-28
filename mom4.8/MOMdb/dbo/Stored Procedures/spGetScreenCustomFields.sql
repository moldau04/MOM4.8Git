CREATE PROCEDURE [dbo].[spGetScreenCustomFields]
	@Screen Varchar(50),
	@RefID int 
AS
IF(@RefID is null OR @RefID = 0)
BEGIN
	SELECT ID
		, t.Label
		, t.Line
		, t.Format
		, t.OrderNo
		, t.IsAlert
		, t.TeamMember
		, t.TeamMemberDisplay
		, '' UpdatedDate
		, '' Username
		, '' Value
		, (CASE t.Format
                WHEN 1 THEN 'Currency'
                WHEN 2 THEN 'Date'
                WHEN 3 THEN 'Text'
                WHEN 4 THEN 'Dropdown'
                WHEN 5 THEN 'Checkbox'
				WHEN 6 THEN 'Notes'
				WHEN 7 THEN 'CheckboxWithComment'
            END) AS FieldControl
	FROM tblCommonCustomFields t WHERE ISNULL(IsDeleted, 0) = 0 AND Screen = @Screen
	--Table[1] --> Default values
	SELECT dv.ID, cf.Label, dv.Line, dv.Value, cf.Format FROM tblCommonCustomDefaultValues dv 
	INNER JOIN tblCommonCustomFields cf ON cf.ID = dv.tblCommonCustomFieldsID 
	WHERE cf.Screen = @Screen AND ISNULL(cf.IsDeleted, 0) = 0
	--Table[2] --> Max line of Custom field on screen
	SELECT max(isnull(t.Line,0)) as [MaxLine] FROM tblCommonCustomFields t
	WHERE t.Screen = @Screen
END
ELSE
BEGIN
	SELECT t.ID
		, t.Label
		, t.Line
		, t.Format
		, t.OrderNo
		, CASE WHEN tv.tblCommonCustomFieldsID is not null OR tv.tblCommonCustomFieldsID != 0 THEN ISNULL(tv.IsAlert,0) 
		ELSE ISNULL(t.IsAlert,0) END IsAlert
		, CASE WHEN tv.tblCommonCustomFieldsID is not null OR tv.tblCommonCustomFieldsID != 0 THEN tv.TeamMember 
		ELSE t.TeamMember END TeamMember
		, CASE WHEN tv.tblCommonCustomFieldsID is not null OR tv.tblCommonCustomFieldsID != 0 THEN tv.TeamMemberDisplay 
		ELSE t.TeamMemberDisplay END TeamMemberDisplay
		, tv.UpdatedDate
		, tv.Username
		, tv.Value
		, (CASE t.Format
                WHEN 1 THEN 'Currency'
                WHEN 2 THEN 'Date'
                WHEN 3 THEN 'Text'
                WHEN 4 THEN 'Dropdown'
                WHEN 5 THEN 'Checkbox'
				WHEN 6 THEN 'Notes'
				WHEN 7 THEN 'CheckboxWithComment'
            END) AS FieldControl
	FROM tblCommonCustomFieldsValue tv
	RIGHT JOIN tblCommonCustomFields t ON t.ID = tv.tblCommonCustomFieldsID AND t.Screen = tv.Screen
	WHERE ISNULL(t.IsDeleted, 0) = 0 
		AND tv.Screen = @Screen
		AND tv.Ref = @RefID

	UNION		
	SELECT t.ID
			, t.Label
			, t.Line
			, t.Format
			, t.OrderNo
			, t.IsAlert
			, t.TeamMember
			, t.TeamMemberDisplay
			, ''
			, ''
			, ''
			, (CASE t.Format
                WHEN 1 THEN 'Currency'
                WHEN 2 THEN 'Date'
                WHEN 3 THEN 'Text'
                WHEN 4 THEN 'Dropdown'
                WHEN 5 THEN 'Checkbox'
				WHEN 6 THEN 'Notes'
				WHEN 7 THEN 'CheckboxWithComment'
            END) AS FieldControl
	FROM tblCommonCustomFields t
	WHERE ISNULL(t.IsDeleted, 0) = 0 AND t.ID NOT IN (SELECT tblCommonCustomFieldsID FROM tblCommonCustomFieldsValue WHERE Screen = @Screen AND Ref = @RefID)
	
	--Table[1] --> Default values
	SELECT dv.ID, cf.Label, dv.Line, dv.Value, cf.Format FROM tblCommonCustomDefaultValues dv 
	INNER JOIN tblCommonCustomFields cf ON cf.ID = dv.tblCommonCustomFieldsID 
	WHERE cf.Screen = @Screen AND ISNULL(cf.IsDeleted, 0) = 0
END


