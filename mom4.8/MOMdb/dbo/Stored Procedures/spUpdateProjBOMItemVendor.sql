CREATE PROCEDURE [dbo].[spUpdateProjBOMItemVendor]
	@jobId int,
	--@jobTItemId int,
	@vendorId int,
	@GanttTaskId int
AS
IF(@vendorId = 0) set @vendorId = null;

UPDATE b set b.Vendor=@vendorId 
FROM JobTItem ji
INNER JOIN BOM b on ji.id = b.JobTItemID
WHERE ji.job = @jobId 
	AND ji.GanttTaskID = @GanttTaskId
	--and b.JobTItemID = @jobTItemId
