CREATE PROCEDURE [dbo].[spGetProjectTemplateCustomFields] @jobt int, @job int
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @IsExist bit = 0
    DECLARE @text varchar(max)
    IF (@job <> 0)
    BEGIN
        IF EXISTS (SELECT TOP 1
                1
            FROM tblCustomJob
            WHERE JobID = @job)
        BEGIN
            SET @IsExist = 1
        END
    END

    IF (@IsExist = 0)
    BEGIN
        SELECT
            tbjobt.JobID,
            tbjobt.JobTID,
            jobt.ID AS JobT,
            jobt.type,
            tc.ID,
            tc.tblTabID,
            tc.Label,
            tc.Line,
            tc.Format,
            ISNULL(tc.IsDeleted, 0) AS IsDeleted,
            (CASE tc.Format
                WHEN 1 THEN 'Currency'
                WHEN 2 THEN 'Date'
                WHEN 3 THEN 'Text'
                WHEN 4 THEN 'Dropdown'
                WHEN 5 THEN 'Checkbox'
                WHEN 6 THEN 'Notes'
				WHEN 7 THEN 'CheckboxWithComment'
            END) AS FieldControl,
            tbjobt.Value AS Value,
            NULL AS UpdatedDate,
            '' AS Username,
			tc.IsAlert,
            tc.IsTask,
			tc.TeamMember,
			tc.TeamMemberDisplay,
            tc.UserRole,
			tc.UserRoleDisplay
			--'' as TeamMember,
			--'' as TeamMemberDisplay
        FROM tblCustomJobT tbjobt
        INNER JOIN tblCustomFields tc
            ON tc.ID = tbjobt.tblCustomFieldsID
        INNER JOIN JobT jobt
            ON jobt.ID = tbjobt.JobTID
        WHERE tbjobt.JobTID = @jobt
        AND (tc.IsDeleted IS NULL
        OR tc.IsDeleted = 0)
        AND tbjobt.JobID IS NULL
        AND tc.tblTabID IS NOT NULL
        ORDER BY OrderNo,
        dbo.SortNumber(tbjobt.Value)
        SELECT
            t.ID,
            t.tblCustomFieldsID,
            t.Line,
            t.Value,
            tc.Label,
            tc.Format,
            tc.tblTabID,
            tbjobt.JobID,
            tbjobt.JobTID,
            jobt.ID AS JobT,
            (CASE tc.Format
                WHEN 1 THEN 'Currency'
                WHEN 2 THEN 'Date'
                WHEN 3 THEN 'Text'
                WHEN 4 THEN 'Dropdown'
                WHEN 5 THEN 'Checkbox'
                WHEN 6 THEN 'Notes'
				WHEN 7 THEN 'CheckboxWithComment'
            END) AS FieldControl
        FROM tblCustomJobT tbjobt
        INNER JOIN tblCustomFields tc
            ON tc.ID = tbjobt.tblCustomFieldsID
        RIGHT JOIN tblCustom t
            ON tc.ID = t.tblCustomFieldsID
        INNER JOIN JobT jobt
            ON jobt.ID = tbjobt.JobTID
        WHERE jobt.ID = @jobt
        AND (tc.IsDeleted IS NULL
        OR tc.IsDeleted = 0)
        AND tbjobt.JobID IS NULL
        AND tc.tblTabID IS NOT NULL
        ORDER BY OrderNo,
        dbo.SortNumber(t.Value)
    END
    ELSE
    BEGIN
       SELECT
            tc.ID,
            tc.tblTabID,
            tc.Label,
            tc.Line,
            tc.Format,
            ISNULL(tc.IsDeleted, 0) AS IsDeleted,
            (CASE tc.Format
                WHEN 1 THEN 'Currency'
                WHEN 2 THEN 'Date'
                WHEN 3 THEN 'Text'
                WHEN 4 THEN 'Dropdown'
                WHEN 5 THEN 'Checkbox'
                WHEN 6 THEN 'Notes'
				WHEN 7 THEN 'CheckboxWithComment'
            END) AS FieldControl,
            tbjob.Value AS Value,
            tbjob.UpdatedDate,
            tbjob.Username,
			CASE WHEN tbjob.tblCustomFieldsID is null OR (tbjob.UpdatedDate is null AND tbjob.Username is null AND Isnull(tbjob.Value,'') = '') THEN tc.IsAlert ELSE tbjob.IsAlert END AS IsAlert,
            CASE WHEN tbjob.tblCustomFieldsID is null OR (tbjob.UpdatedDate is null AND tbjob.Username is null AND Isnull(tbjob.Value,'') = '')  THEN tc.IsTask ELSE tbjob.IsTask END AS IsTask,
			CASE WHEN tbjob.tblCustomFieldsID is null OR (tbjob.UpdatedDate is null AND tbjob.Username is null AND Isnull(tbjob.Value,'') = '')   THEN tc.TeamMember ELSE tbjob.TeamMember END AS TeamMember,
			CASE WHEN tbjob.tblCustomFieldsID is null OR (tbjob.UpdatedDate is null AND tbjob.Username is null AND Isnull(tbjob.Value,'') = '')   THEN tc.TeamMemberDisplay ELSE tbjob.TeamMemberDisplay END AS TeamMemberDisplay,
            CASE WHEN tbjob.tblCustomFieldsID is null OR (tbjob.UpdatedDate is null AND tbjob.Username is null AND Isnull(tbjob.Value,'') = '')   THEN tc.UserRole ELSE tbjob.UserRole END AS UserRole,
			CASE WHEN tbjob.tblCustomFieldsID is null OR (tbjob.UpdatedDate is null AND tbjob.Username is null AND Isnull(tbjob.Value,'') = '')   THEN tc.UserRoleDisplay ELSE tbjob.UserRoleDisplay END AS UserRoleDisplay
			--ISNULL(tbjob.IsAlert,tc.IsAlert) AS IsAlert,
			--ISNULL(tbjob.TeamMember,tc.TeamMember) AS TeamMember,
			--ISNULL(tbjob.TeamMemberDisplay,tc.TeamMemberDisplay) as TeamMemberDisplay
        FROM tblCustomJobt tbjobt
        INNER JOIN tblCustomFields tc
            ON tc.ID = tbjobt.tblCustomFieldsID
        INNER JOIN JobT jobt
            ON jobt.ID = tbjobt.JobTID
        LEFT OUTER JOIN tblCustomJob tbjob
            ON tbjobt.tblCustomFieldsID = tbjob.tblCustomFieldsID
            AND tbjob.JobID = @job
        LEFT OUTER JOIN Job j
            ON j.ID = tbjob.JobID
        WHERE tbjobt.JobTID = @jobt
        AND (tc.IsDeleted IS NULL
        OR tc.IsDeleted = 0)
        AND tc.tblTabID IS NOT NULL
        ORDER BY OrderNo,
        dbo.SortNumber(tbjobt.Value)

        SELECT
            t.ID,
            t.tblCustomFieldsID,
            t.Line,
            t.Value,
            tc.Label,
            tc.Format,
            tc.tblTabID,
            tbjob.JobID,
            (CASE tc.Format
                WHEN 1 THEN 'Currency'
                WHEN 2 THEN 'Date'
                WHEN 3 THEN 'Text'
                WHEN 4 THEN 'Dropdown'
                WHEN 5 THEN 'Checkbox'
                WHEN 6 THEN 'Notes'
				WHEN 7 THEN 'CheckboxWithComment'
            END) AS FieldControl
        FROM tblCustomJobt tbjobt
        INNER JOIN tblCustomFields tc
            ON tc.ID = tbjobt.tblCustomFieldsID
        RIGHT JOIN tblCustom t
            ON tc.ID = t.tblCustomFieldsID
        LEFT OUTER JOIN tblCustomJob tbjob
            ON tbjobt.tblCustomFieldsID = tbjob.tblCustomFieldsID
            AND tbjob.JobID = @job
        LEFT OUTER JOIN Job j
            ON j.ID = tbjob.JobID
        WHERE tbjobt.JobTID = @jobt
        AND (tc.IsDeleted IS NULL
        OR tc.IsDeleted = 0)
        AND tc.tblTabID IS NOT NULL
        ORDER BY OrderNo,
        dbo.SortNumber(t.Value)
    END
END