-- =============================================
-- Author:		<Harsh Dwivedi>
-- Create date: <16th Dec 2018>
-- Description:	<Get Project, type, wage and equipment>
-- =============================================
create PROCEDURE [dbo].[spGetTimeCardJob]
	@SearchText NVARCHAR(100),
	@IsJob TINYINT=0,
	@JobId INT=0,
	@Loc INT =0,
	@Worker NVARCHAR(125)='',
	@Userid INT =0,
	@EN INT=0
AS
BEGIN 
	SET NOCOUNT ON;
	
	IF @IsJob=0
	 BEGIN
		DECLARE @ID INT=0,@Wid INT=0
		SELECT  @ID=id FROM emp e where e.fWork = 	
		(SELECT ID FROM tblWork WHERE UPPER(fDesc) =@Worker AND Status=0)
		SET @Wid=(SELECT TOP 1 p.ID FROM PRWage p 
		INNER JOIN Job j ON j.WageC=p.ID AND j.ID=@JobId
		INNER JOIN PRWageItem w ON w.Wage=p.ID
		WHERE w.Emp=@ID
		ORDER BY p.fDesc ASC)		
		SELECT DISTINCT p.ID AS id, p.fDesc AS fdesc, 
	    (CASE WHEN p.ID=@Wid THEN 1 ELSE 0 END) AS Selected
		FROM PRWage p
	    INNER JOIN PRWageItem w ON w.Wage=p.ID
		WHERE w.Emp=@ID
		ORDER BY p.fDesc ASC
	 END
	
	IF @IsJob=1
	BEGIN 
		IF (@EN=1)
		BEGIN
			 SELECT TOP 100 j.ID,j.fDesc,c.Loc,c.Tag FROM Job j
					INNER JOIN Loc c ON j.Loc=c.Loc
					INNER JOIN Rol r ON r.ID=c.Rol
					LEFT OUTER JOIN tblUserCo UC on UC.CompanyID = r.EN AND r.EN=@EN
					WHERE UC.IsSel = 1 and UC.UserID =@Userid
					AND j.Status=0 AND (j.fDesc LIKE '%'+@SearchText+'%' OR c.Tag LIKE '%'+@SearchText+'%'
					OR CAST(j.ID AS NVARCHAR)=@SearchText)
		END
		ELSE 
		BEGIN
			SELECT TOP 100 j.ID,j.fDesc,c.Loc,c.Tag FROM Job j
		    INNER JOIN Loc c ON j.Loc=c.Loc
		    INNER JOIN Rol r ON r.ID=c.Rol
		    WHERE j.Status=0 AND (j.fDesc LIKE '%'+@SearchText+'%' 
			OR c.Tag LIKE '%'+@SearchText+'%' OR CAST(j.ID AS NVARCHAR)=@SearchText)		
			END
	END
	IF @IsJob =3
	BEGIN
	        SELECT * FROM (
		    SELECT i.fDesc, i.Code, i.Line as Phase,
			(SELECT Type from BOMT WHERE ID =b.Type) AS BType,
			(select top 1 JobCodeDesc from tblJobCodeDesc_ByJobType where JobCodeID= ( select jc.ID  FROM JobCode jc where jc.Code=i.Code) and JobTypeID=(select type from job where id =@JobId) )  as  CodeDesc ,
			(Select GroupName from tblEstimateGroup where Id=i.GroupID) as	GroupName 
		    FROM JobTItem i			 
			INNER JOIN BOM b on i.ID=b.JobTItemID
			WHERE i.Type=1 AND i.Job=@JobId 
			 ) x 
			WHERE   
			(
			x.fDesc LIKE '%'+@SearchText+'%' 
			OR x.GroupName LIKE '%'+@SearchText+'%' 
			OR x.Code LIKE '%'+@SearchText+'%'
			OR x.CodeDesc LIKE '%'+@SearchText+'%'
			OR x.BType LIKE '%'+@SearchText+'%'
			)
	END
	IF @IsJob =2
	BEGIN
		    SELECT DISTINCT 
			e.state,
			e.cat,
			e.category,
			e.id,
			e.unit,
			e.type,
			e.fdesc,
			e.status,
			e.building
			FROM elev e 
			INNER JOIN loc l ON l.Loc = e.Loc 
			INNER JOIN owner o ON o.id = l.owner INNER JOIN rol r ON o.rol = r.id 
			LEFT OUTER JOIN Branch B on r.EN = B.ID   
			WHERE e.id IS NOT NULL  and e.loc=@Loc order by e.unit
	END
END
		 
