CREATE PROCEDURE [dbo].[spGetReDrawingsSubmittedForApproval]
AS 

SELECT 
	* 
FROM information_schema.columns 
WHERE table_name = 'vw_ReDrawingsSubmittedForApproval'