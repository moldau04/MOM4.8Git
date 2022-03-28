CREATE PROCEDURE [dbo].[spGetWorkflowFields]    
	AS
    select [ID],[Line],[OrderNo],[Label],[IsAlert],[TeamMember],[Format],TeamMemberDisplay
	from tblWorkflowFields tb
	order by [OrderNo]

	SELECT [ID],[tblWorkflowFieldsID],[Line],[Value] FROM tblWorkflow
