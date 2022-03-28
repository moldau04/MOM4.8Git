CREATE PROCEDURE [dbo].[spUpdateLocRoute]
@Locations As [dbo].[tblTypeTemplateDetails] Readonly
AS
update l set l.Route=lw.Worker
from @Locations lw inner join Loc l on l.Loc=lw.Loc

update j set j.Custom20 = lw.Worker
from @Locations lw inner join Job j on j.Loc=lw.Loc