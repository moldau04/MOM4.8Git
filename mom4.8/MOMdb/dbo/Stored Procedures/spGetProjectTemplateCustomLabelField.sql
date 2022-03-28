/*--------------------------------------------------------------------
Author: Thurstan
Created On:	 04 Oct 2018
Description: Get Project Template Custom Label Field
Modified By: 09 Oct 2018
Modified On: Include project not exist in tblCustomJob table
--------------------------------------------------------------------*/
CREATE PROCEDURE [dbo].[spGetProjectTemplateCustomLabelField] 	@jobt int, @job int
AS 
BEGIN 
SET NOCOUNT ON;

	DECLARE @IsExist BIT = 0
	DECLARE @text varchar(max)
	DECLARE @CheckDataExist varchar(max)

    IF(@job <> 0)
	BEGIN

		IF EXISTS(SELECT TOP 1 1 FROM tblCustomJob WHERE JobID=@job)
		BEGIN
				SET @IsExist = 1
		END

	END
	
	if(@IsExist = 0)
	begin

	SET @CheckDataExist = (Select count(*) from  tblCustomJobT tbjobt 
								INNER JOIN tblCustomFields tc ON tc.ID = tbjobt.tblCustomFieldsID    
								INNER JOIN JobT jobt ON jobt.ID = tbjobt.JobTID   
									WHERE tbjobt.JobTID = @jobt
											AND (tc.IsDeleted is null OR tc.IsDeleted = 0)   
											AND tbjobt.JobID is null 
											AND tc.tblTabID is not null)
										
			IF(@CheckDataExist = 0)
			BEGIN
			SELECT  @job as JobId,
							null as ID, 
							null as tblTabID, 
							null as Label, 
							null as Line, 
							null as Format, 
							null as IsDeleted,
							null as FieldControl,     
         					null as Value,
							null as UpdatedDate,
							null as Username
			  

			END
			ELSE
			BEGIN
			SELECT  @job as JobId,
							tc.ID, 
							tc.tblTabID, 
							tc.Label, 
							tc.Line, 
							tc.Format, 
							isnull(tc.IsDeleted,0) as IsDeleted,     
         					(CASE tc.Format WHEN 1 THEN 'Currency'    
								WHEN 2 THEN 'Date'                
        						WHEN 3 THEN 'Text'              
     							WHEN 4 THEN 'Dropdown'                    
     							WHEN 5 THEN 'Checkbox' END) AS FieldControl,
							tbjobt.Value as Value,
							null as UpdatedDate,
							'' as Username
			  
       						FROM tblCustomJobT tbjobt 
								INNER JOIN tblCustomFields tc ON tc.ID = tbjobt.tblCustomFieldsID    
								INNER JOIN JobT jobt ON jobt.ID = tbjobt.JobTID   
									WHERE tbjobt.JobTID = @jobt
											AND (tc.IsDeleted is null OR tc.IsDeleted = 0)   
											AND tbjobt.JobID is null 
											AND tc.tblTabID is not null
											order by OrderNo,dbo.SortNumber(tbjobt.Value) 
			END

	end
	else
	begin

	select j.ID as JobId, tc.ID, 
						tc.tblTabID, 
						tc.Label, 
						tc.Line, 
						tc.Format, 
						isnull(tc.IsDeleted,0) as IsDeleted,     
         				(CASE tc.Format WHEN 1 THEN 'Currency'    
							WHEN 2 THEN 'Date'                
        					WHEN 3 THEN 'Text'              
     						WHEN 4 THEN 'Dropdown'                    
     						WHEN 5 THEN 'Checkbox' END) AS FieldControl,
						tbjob.Value as Value,
						tbjob.UpdatedDate,
						tbjob.Username
						
			  
       					FROM tblCustomJobt tbjobt 
							INNER JOIN tblCustomFields tc ON tc.ID = tbjobt.tblCustomFieldsID  
							INNER JOIN JobT jobt ON jobt.ID = tbjobt.JobTID 
							left outer join tblCustomJob tbjob on  tbjobt.tblCustomFieldsID = tbjob.tblCustomFieldsID and tbjob.JobID = @job
							left outer JOIN Job j ON j.ID = tbjob.JobID
								WHERE 
										tbjobt.JobTID = @jobt 
										AND (tc.IsDeleted is null OR tc.IsDeleted = 0)   
										AND tc.tblTabID is not null
										order by OrderNo ,dbo.SortNumber(tbjobt.Value)
										
		
	end
END