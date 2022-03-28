CREATE PROCEDURE [dbo].[GetTroubleCallsByEquipmentData] (
  @StartDate DATETIME = NULL, 
  @EndDate DATETIME = NULL, 
  @Top INT = 5,
  @CallTimes INT = 3,
  @UserID INT,
  @EN INT,
  @Categories VARCHAR(500)
) AS BEGIN 

  CREATE TABLE #TroubleCallsByEquipment
  (
	TicketID INT,
    Unit VARCHAR(50), 
    Type VARCHAR(150), 
    Tag VARCHAR(150), 
    ContractNumber VARCHAR(50), 
    CType VARCHAR(50),
	EN INT
  )
  
  INSERT INTO #TroubleCallsByEquipment (TicketID, Unit, Type, Tag,  ContractNumber, CType, EN)
  (
	SELECT 
		tk.ID as TicketID,
		el.Unit, 
		el.Type, 
		l.Tag, 
		j.ID AS ContractNumber,
		j.CType,  
		r.EN
	FROM TicketO  tk
	INNER JOIN Elev el on tk.LElev = el.ID
	LEFT OUTER JOIN Loc l on l.Loc = tk.LID 
	INNER JOIN job j on j.ID = tk.Job 
	INNER JOIN Owner o on l.Owner = o.ID 
	INNER JOIN Rol r  ON r.ID = o.Rol
	WHERE (tk.Cat IN (SELECT SplitValue FROM [dbo].[fnSplit](@Categories,',')))
		AND tk.EDate >= @StartDate AND tk.EDate <= @EndDate
  )

  INSERT INTO #TroubleCallsByEquipment (TicketID, Unit, Type, Tag,  ContractNumber, CType, EN)
  (
	SELECT 
		tk.ID as TicketID,
		el.Unit, 
		el.Type, 
		l.Tag, 
		j.ID AS ContractNumber,
		j.CType,
		r.EN
	FROM TicketD  tk
	INNER JOIN Elev el on tk.Elev = el.ID
	LEFT OUTER JOIN Loc l on l.Loc = tk.Loc 
	INNER JOIN Job j on j.ID = tk.Job
	INNER JOIN Owner o on l.Owner = o.ID 
	INNER JOIN Rol r  ON r.ID = o.Rol
	WHERE (tk.Cat IN (SELECT SplitValue FROM [dbo].[fnSplit](@Categories,',')))
		AND tk.EDate >= @StartDate AND tk.EDate <= @EndDate
  )

  INSERT INTO #TroubleCallsByEquipment (TicketID, Unit, Type, Tag,  ContractNumber, CType, EN)
  (
	SELECT 
		tk.ID as TicketID,
		el.Unit, 
		el.Type, 
		l.Tag, 
		j.ID AS ContractNumber,
		j.CType,
		r.EN
	FROM TicketDPDA  tk
	INNER JOIN Elev el on tk.Elev = el.ID
	LEFT OUTER JOIN Loc l on l.Loc = tk.Loc 
	INNER JOIN job j on j.ID = tk.Job 
	INNER JOIN Owner o on l.Owner = o.ID 
	INNER JOIN Rol r  ON r.ID = o.Rol
	WHERE (tk.Cat IN (SELECT SplitValue FROM [dbo].[fnSplit](@Categories,',')))
		AND tk.EDate >= @StartDate AND tk.EDate <= @EndDate
  )

  IF @EN = 1
	BEGIN
	  SELECT
		temp.Unit, 
		temp.Type,
		temp.Tag, 
		temp.ContractNumber, 
		temp.CType,
		COUNT(*) as Count
	  FROM #TroubleCallsByEquipment temp
	  INNER JOIN tblUserCo UC on UC.CompanyID = temp.EN
	  WHERE UC.IsSel = 1 and UC.UserID = @UserID
	  GROUP BY temp.Unit, temp.Type, temp.Tag, temp.ContractNumber, temp.CType
	  HAVING COUNT(*) >= @CallTimes
	  ORDER BY COUNT(*) DESC
	END
  ELSE
    BEGIN
	  SELECT
		temp.Unit, 
		temp.Type,
		temp.Tag, 
		temp.ContractNumber, 
		temp.CType,
		COUNT(*) as Count
	  FROM #TroubleCallsByEquipment temp
	  GROUP BY temp.Unit, temp.Type, temp.Tag, temp.ContractNumber, temp.CType
	  HAVING COUNT(*) >= @CallTimes
	  ORDER BY COUNT(*) DESC
	END
END