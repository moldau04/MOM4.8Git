CREATE PROC [dbo].[spGetTimesheetEmp] 
@startdate datetime,
@enddate datetime,
@Supervisor varchar(50),
@department int, @EN int = 0,
@UserID int = 0,
@WorkId int = 0,
@Etimesheet int = -1

AS

Declare @s_startdate datetime
Declare @s_enddate datetime
Declare @s_Supervisor varchar(50)
Declare @s_department int
Declare @s_EN int = 0
Declare @s_UserID int = 0
Declare @s_WorkId int = 0
Declare @s_Etimesheet int = -1


Set @s_startdate=@startdate
Set @s_enddate=@enddate
Set @s_Supervisor=@Supervisor
Set @s_department=@department
Set @s_EN=@EN
Set @s_UserID=@UserID
Set @s_WorkId=@WorkId
Set @s_Etimesheet=@Etimesheet
	SELECT
		e.ID,
		(e.Last + ' , ' + e.fFirst) AS Name,

		(SELECT top 1
			EN
		FROM tblUser
		WHERE fUser = e.CallSign)
		AS EN,

		(SELECT top 1
			B.Name
		FROM Branch B
		INNER JOIN tblUser U
			ON B.ID = U.EN
		WHERE fUser = e.CallSign)
		AS Company,
		fDesc,
		(CASE
			WHEN e.PFixed = 0 THEN ISNULL(PHour, 0)
			ELSE Reg
		END) AS reg,
		OT,
		DT,
		TT,
		NT,
		ZONE,
		ISNULL(e.MileageRate, 0) AS MileageRate,
		Mileage,
		extra,
		Toll,
		OtherE,
		1 AS pay,
		NULL AS holiday,
		NULL AS vacation,
		NULL AS sicktime,
		NULL AS reimb,
		NULL AS bonus,
		CASE
			WHEN e.PFixed = 0 THEN 'Fixed Hours'
			WHEN e.PMethod = 0 THEN 'Salaried'
			WHEN e.PMethod = 1 THEN 'Hourly'
		END paymethod,
		CASE e.pfixed
			WHEN 0 THEN 2
			ELSE e.pmethod
		END AS pmethod,

		(SELECT top 1
			ID
		FROM tblUser
		WHERE fUser = e.CallSign)
		AS userid,
		CASE
			WHEN ISNULL(e.fWork, '') = '' THEN 'Office'
			ELSE 'Field'
		END AS usertype,
		CASE
			WHEN e.PFixed = 0 THEN ((ISNULL(PHour, 0) * (SELECT
					ISNULL(HourlyRate, 0)
				FROM tblWork wo
				WHERE wo.fDesc = e.CallSign)
				) + (ZONE + extra))
			WHEN e.PMethod = 0 THEN (ZONE + extra)
			WHEN e.PMethod = 1 THEN (((Reg + OT + DT + TT + NT) * (SELECT
					ISNULL(HourlyRate, 0)
				FROM tblWork wo
				WHERE wo.fDesc = e.CallSign)
				) + (ZONE + extra))
		END total,
		ISNULL(PHour, 0) AS phour,
		ISNULL(salary, 0) AS salary,

		(SELECT
			ISNULL(HourlyRate, 0)
		FROM tblWork wo
		WHERE wo.fDesc = e.CallSign)
		AS HourlyRate,
		Customtick1,
		0 AS dollaramount,
		0 Reg1,
		0 OT1,
		0 DT1,
		0 TT1,
		0 NT1,
		0 Zone1,
		0 Mileage1,
		0 Extra1,
		0 Misc1,
		0 Toll1,
		0 HourRate1,

		(SELECT TOP 1
			signature
		FROM PDATimeSign
		WHERE fwork = (SELECT
			ID
		FROM tblWork wo
		WHERE wo.fDesc = e.CallSign)
		AND CAST(EDate AS date) = @s_ENddate
		ORDER BY edate DESC)
		AS signature,
		e.ref,
		0 AS custom,
				(SELECT count(*)
FROM tblWork wo
INNER JOIN TicketD d1 ON wo.ID= d1.fWork
AND convert(datetime, convert(date, d1.EDate)) BETWEEN @s_startdate AND @s_enddate
WHERE wo.Status=0
AND wo.fDesc=(select callsign from Emp where ID= e.ID)
and isnull(d1.TransferTime,-1)=case @s_Etimesheet when 1 then 1 when 0 then 0 else isnull(d1.TransferTime,-1) end ) as countDetail
	FROM (SELECT
		w.fDesc,
		SUM(ISNULL(Reg, 0)) Reg,
		SUM(ISNULL(OT, 0)) OT,
		SUM(ISNULL(DT, 0)) DT,
		SUM(ISNULL(TT, 0)) TT,
		SUM(ISNULL(NT, 0)) NT,
		SUM(ISNULL(ZONE, 0)) ZONE,
		SUM(ISNULL((ISNULL(EMile, 0) - ISNULL(sMile, 0)), 0)) Mileage,
		SUM(CASE ISNUMERIC(dbo.udf_GetNumeric(ISNULL(d.Custom2, '0')))
			WHEN 1 THEN CONVERT(money, dbo.udf_GetNumeric(ISNULL(d.Custom2, '0')))
			ELSE 0
		END) AS extra,
		SUM(ISNULL(Toll, 0)) Toll,
		SUM(ISNULL(OtherE, 0)) OtherE,
		0 AS Customtick1
	FROM tblWork w
	LEFT OUTER JOIN TicketD d
		ON w.ID = d.fWork
		AND ClearCheck = 1
		AND ISNULL(TransferTime, -1) =
																	CASE @s_Etimesheet
																		WHEN 1 THEN 1
																		WHEN 0 THEN 0
																		ELSE ISNULL(TransferTime, -1)
																	END
		AND CAST(EDate AS date) >= CAST(@s_startdate AS date)
		AND CAST(EDate AS date) <= CAST(@s_enddate AS date)
	WHERE w.Status = 0
	GROUP BY w.fDesc
	UNION
	SELECT
		CallSign AS fDesc,
		0 Reg,
		0 OT,
		0 DT,
		0 TT,
		0 NT,
		0 ZONE,
		0 Mileage,
		0 AS extra,
		0 AS Toll,
		0 AS OtherE,
		0 AS Customtick1
	FROM Emp
	WHERE Field = 0
	AND Status = 0) AS tab
	INNER JOIN Emp e
		ON e.CallSign = tab.fDesc
		AND e.id = (CASE @s_department
			WHEN -1 THEN e.id
			ELSE (SELECT TOP 1
					emp
				FROM tblJoinEmpDepartment
				WHERE department = @s_department
				AND emp = e.id)
		END)
		AND e.callsign = (CASE @s_Supervisor
			WHEN '' THEN e.callsign
			ELSE (SELECT TOP 1
					fdesc
				FROM tblwork w
				WHERE super = @s_Supervisor
				AND fdesc = e.callsign)
		END)
		AND e.callsign =
										CASE @s_WorkId
											WHEN 0 THEN e.callsign
											ELSE (SELECT TOP 1
													fdesc
												FROM tblwork w
												WHERE id = @s_WorkId
												AND fdesc = e.callsign)
										END
		AND e.id IN (CASE @s_EN
			WHEN 0 THEN e.id
			ELSE (SELECT
					U.ID
				FROM tblUser U
				INNER JOIN tblUserCo UC
					ON U.ID = UC.USerID
				WHERE UC.IsSel = 1
				AND UC.UserID = @s_UserID)
		END)
	ORDER BY LTRIM(RTRIM(e.Last))