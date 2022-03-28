CREATE PROCEDURE [dbo].[spGetTicketCountByCategoryAndDateRange] (
  @StartDate DATETIME = NULL, 
  @EndDate DATETIME = NULL, 
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
	Cat VARCHAR(150), 
    ContractNumber VARCHAR(50), 
    CType VARCHAR(50),
	EN INT
  )
  
  INSERT INTO #TroubleCallsByEquipment (TicketID, Unit, Type, Tag, ContractNumber, CType, Cat, EN)
  (
	SELECT 
		tk.ID as TicketID,
		el.Unit, 
		el.Type, 
		l.Tag, 
		j.ID AS ContractNumber,
		j.CType, 
		tk.Cat, 
		r.EN
	FROM TicketO  tk
	LEFT JOIN Elev el on tk.LElev = el.ID
	LEFT OUTER JOIN Loc l on l.Loc = tk.LID 
	LEFT JOIN job j on j.ID = tk.Job 
	LEFT JOIN Owner o on l.Owner = o.ID 
	LEFT JOIN Rol r  ON r.ID = o.Rol
	WHERE tk.EDate >= @StartDate AND tk.EDate <= @EndDate
		AND (tk.Cat IN (SELECT SplitValue FROM [dbo].[fnSplit](@Categories,',')))
  )

  INSERT INTO #TroubleCallsByEquipment (TicketID, Unit, Type, Tag, ContractNumber, CType, Cat, EN)
  (
	SELECT 
		tk.ID as TicketID,
		el.Unit, 
		el.Type, 
		l.Tag, 
		j.ID AS ContractNumber,
		j.CType,
		tk.Cat,
		r.EN
	FROM TicketD  tk
	LEFT JOIN Elev el on tk.Elev = el.ID
	LEFT OUTER JOIN Loc l on l.Loc = tk.Loc 
	LEFT JOIN Job j on j.ID = tk.Job
	LEFT JOIN Owner o on l.Owner = o.ID 
	LEFT JOIN Rol r  ON r.ID = o.Rol
	WHERE tk.EDate >= @StartDate AND tk.EDate <= @EndDate
		AND (tk.Cat IN (SELECT SplitValue FROM [dbo].[fnSplit](@Categories,',')))
  )

  IF @EN = 1
	BEGIN
	  SELECT
		temp.Cat AS Category,
		COUNT(*) as Count
	  FROM #TroubleCallsByEquipment temp
	  INNER JOIN tblUserCo UC on UC.CompanyID = temp.EN
	  WHERE UC.IsSel = 1 and UC.UserID = @UserID
	  GROUP BY temp.Cat
	  ORDER BY temp.Cat
	END
  ELSE
    BEGIN
	  SELECT
		temp.Cat AS Category,
		COUNT(*) as Count
	  FROM #TroubleCallsByEquipment temp
	   GROUP BY temp.Cat
	   ORDER BY temp.Cat
	END
END