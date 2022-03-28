CREATE Procedure spGetActiveServiceTypeContract 
(@LocType VARCHAR(100), @EditSType VARCHAR(100),@department int ,@route int)
AS 
BEGIN

IF EXISTS (
SELECT 1 FROM ltype l inner join tblServicetypeRouteMapping rmap on l.Type = rmap.type
inner join tblServicetypeDepartmentMapping dmap on l.Type = dmap.type WHERE l.LocType = @LocType AND dmap.Department = @department AND rmap.route = @route)
BEGIN

SELECT
     ISNULL(l.Reg, 0) AS RT,
     ISNULL(l.OT, 0) AS OT,
     ISNULL(l.NT, 0) AS NT,
     ISNULL(l.DT, 0) AS DT,
     l.type,
     l.fdesc,
     l.remarks,
     (SELECT COUNT(1) FROM elev WHERE cat = l.type) AS Count,
     l.InvID,
     ISNULL(i.Name, '') AS Name,
     ISNULL(l.Status,0) AS Status,
     CASE WHEN ISNULL(l.Status,0) = 0 THEN 'Active'
     ELSE 'Inactive' END AS StatusLabel
 FROM ltype l
 LEFT JOIN Inv i ON l.InvID = i.ID
 WHERE 
 ISNULL(l.Status,0) != 1 
 AND ISNULL(l.LocType,'') = CASE WHEN EXISTS(SELECT LocType From LType Where LocType = @LocType) THEN @LocType ELSE '' END
 AND ISNULL(l.Type,'') in (SELECT Type from tblServicetypeRouteMapping WHERE route = @route)
 AND ISNULL(l.Type,'') in (SELECT Type from tblServicetypeDepartmentMapping WHERE Department = @department)
 UNION
 SELECT
     ISNULL(l.Reg, 0) AS RT,
     ISNULL(l.OT, 0) AS OT,
     ISNULL(l.NT, 0) AS NT,
     ISNULL(l.DT, 0) AS DT,
     l.type,
     l.fdesc,
     l.remarks,
     (SELECT COUNT(1) FROM elev WHERE cat = l.type) AS Count,
     l.InvID,
     ISNULL(i.Name, '') AS Name,
     ISNULL(l.Status,0) AS Status,
     CASE WHEN ISNULL(l.Status,0) = 0 THEN 'Active'
     ELSE 'Inactive' END AS StatusLabel
 FROM ltype l
 LEFT JOIN Inv i ON l.InvID = i.ID
 WHERE l.Type = @EditSType
 ORDER BY l.fdesc

END
ELSE
BEGIN
SELECT
     ISNULL(l.Reg, 0) AS RT,
     ISNULL(l.OT, 0) AS OT,
     ISNULL(l.NT, 0) AS NT,
     ISNULL(l.DT, 0) AS DT,
     l.type,
     l.fdesc,
     l.remarks,
     (SELECT COUNT(1) FROM elev WHERE cat = l.type) AS Count,
     l.InvID,
     ISNULL(i.Name, '') AS Name,
     ISNULL(l.Status,0) AS Status,
     CASE WHEN ISNULL(l.Status,0) = 0 THEN 'Active'
     ELSE 'Inactive' END AS StatusLabel
 FROM ltype l
 LEFT JOIN Inv i ON l.InvID = i.ID
 WHERE ISNULL(l.Status,0) != 1 
  UNION
 SELECT
     ISNULL(l.Reg, 0) AS RT,
     ISNULL(l.OT, 0) AS OT,
     ISNULL(l.NT, 0) AS NT,
     ISNULL(l.DT, 0) AS DT,
     l.type,
     l.fdesc,
     l.remarks,
     (SELECT COUNT(1) FROM elev WHERE cat = l.type) AS Count,
     l.InvID,
     ISNULL(i.Name, '') AS Name,
     ISNULL(l.Status,0) AS Status,
     CASE WHEN ISNULL(l.Status,0) = 0 THEN 'Active'
     ELSE 'Inactive' END AS StatusLabel
 FROM ltype l
 LEFT JOIN Inv i ON l.InvID = i.ID
 WHERE l.Type = @EditSType
 ORDER BY l.fdesc
 
END

END

 