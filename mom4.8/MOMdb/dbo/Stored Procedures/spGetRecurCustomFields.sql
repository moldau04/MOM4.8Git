
CREATE PROCEDURE spGetRecurCustomFields
	@JobId int
AS
BEGIN
	
	SET NOCOUNT ON;
	
	DECLARE @text VARCHAR(MAX)
	DECLARE @IsExist BIT = 0

    IF(@JobId <> 0)
	BEGIN

		IF EXISTS(SELECT TOP 1 1 FROM JobT j INNER JOIN tblCustomJobT t ON t.JobTID = j.ID WHERE t.JobID=@JobId)
		BEGIN
				SET @IsExist = 1
		END

	END
	SET @text =' SELECT j.JobTID AS JobT, t.ID, t.tblTabID, t.Label, t.Line, t.Format, t.IsDeleted ,     
					(CASE t.Format WHEN 1 THEN ''Currency''   
					WHEN 2 THEN ''Date''
					WHEN 3 THEN ''Text''     
					WHEN 4 THEN ''Dropdown''  
					WHEN 5 THEN ''Checkbox'' END) AS FieldControl,   
					ISNULL(j.Value,'''') AS Value     
				FROM tblCustomJobT j INNER JOIN tblCustomFields t ON t.ID = j.tblCustomFieldsID 
				INNER JOIN JobT jt ON jt.ID = j.JobTID    
				WHERE jt.Type = 0 AND (t.IsDeleted is null OR t.IsDeleted = 0)    '
				IF(@IsExist = 1)
				BEGIN
					SET @text += ' AND j.JobID = '''+convert(varchar(10),@JobId)+''''	
				END
				ELSE
				BEGIN
					SET @text += ' AND (j.JobID IS NULL OR j.JobID = 0) '
				END

	SET @text += ' SELECT j.JobTID AS JobT, t.*, tc.Label, tc.Format, tc.tblTabID, 
					(CASE tc.Format WHEN 1 THEN ''Currency'' 
					WHEN 2 THEN ''Date''			 
					WHEN 3 THEN ''Text''  
					WHEN 4 THEN ''Dropdown'' 
					WHEN 5 THEN ''Checkbox'' END) AS FieldControl 
				FROM tblCustomJobT j INNER JOIN tblCustomFields tc ON tc.ID = j.tblCustomFieldsID 
				RIGHT JOIN tblCustom t ON tc.ID = t.tblCustomFieldsID  
				INNER JOIN JobT jt ON jt.ID = j.JobTID 
				WHERE jt.Type = 0 AND (tc.IsDeleted is null OR tc.IsDeleted = 0) '
				IF(@IsExist = 1)
				BEGIN
					SET @text += ' AND j.JobID = '''+convert(varchar(10),@JobId)+''''
				END
				ELSE
				BEGIN
					SET @text += ' AND (j.JobID IS NULL OR j.JobID = 0) '
				END

    SET @text += ' SELECT distinct tc.JobTID  as JobT
					 FROM tblCustomJobT tc 
					 INNER JOIN JobT j ON j.ID = tc.JobTID   
					 INNER JOIN tblCustomFields c ON c.ID = tc.tblCustomFieldsID   
					 WHERE j.Type=0 AND (c.IsDeleted is null OR c.IsDeleted = 0)     '
                IF(@IsExist = 1)
				BEGIN
					SET @text += ' AND tc.JobID = '''+convert(varchar(10),@JobId)+''''
				END  
				ELSE
				BEGIN
					SET @text += ' AND (tc.JobID IS NULL OR tc.JobID = 0) '
				END
     
	 EXEC(@text)

END
