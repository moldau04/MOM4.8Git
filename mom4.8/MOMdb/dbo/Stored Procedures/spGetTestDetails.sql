CREATE PROCEDURE [dbo].[spGetTestDetails] 			
	@LID INT=NULL 		
AS 
BEGIN
	
	
	DECLARE @ElevID INT
	DECLARE @Year INT
	DECLARE @TestType INT
	--TestType cover another
	DECLARE @isParentTestType INT
    DECLARE @TestTypeChild INT
	DECLARE @TestTypeChildID INT
	DECLARE @TestTypeChildName VARCHAR(100)
	DECLARE @HasChildTest INT 
	-- TestType is cover by another TestType
	DECLARE @TestTypeParentName VARCHAR(100)
	DECLARE	@TestTypeParentID INT 
	DECLARE @HasParentTest INT 
	DECLARE @ParentTestID INT
	DECLARE	@TestTypeParentChargable BIT 

	SELECT @TestType=ID,@ElevID=Elev,@Year=YEAR(Next) FROM LoadTestItem WHERE LID=@LID

	SET @isParentTestType=ISNULL((SELECT COUNT(1) FROM TestTypeCover WHERE TestTypeID=@TestType),0)
	IF (@isParentTestType=1)
	BEGIN
		    
		SET @TestTypeChildID=(SELECT TOP 1 TestTypeCoverID FROM TestTypeCover WHERE TestTypeID=@TestType)
		SET @TestTypeChildName=ISNULL( (SELECT Name FROM LoadTest WHERE ID =@TestTypeChildID),'')
		SET @HasChildTest= ISNULL((SELECT COUNT(1) FROM LoadTestItem WHERE Elev=@ElevID AND YEAR(Next)=@Year AND ID=@TestTypeChildID),0)
    END
	ELSE
    BEGIN
	
		SET @TestTypeParentID=(SELECT TOP 1 TestTypeID FROM TestTypeCover WHERE TestTypeCoverID=@TestType)
		
		SET @TestTypeParentName=ISNULL( (SELECT Name FROM LoadTest WHERE ID =@TestTypeParentID),'')
		SET @HasParentTest= ISNULL((SELECT COUNT(1) FROM LoadTestItem WHERE Elev=@ElevID AND YEAR(Next)=@Year AND ID=@TestTypeParentID),0)
		SET @ParentTestID=isnull((SELECT TOP 1 LId FROM LoadTestItem WHERE Elev=@ElevID AND YEAR(Next)= @year AND ID=@TestTypeParentID),0)
		SET @TestTypeParentChargable=isnull((SELECT TOP 1 Chargeable FROM LoadTestItem WHERE Elev=@ElevID AND YEAR(Next)= @year AND ID=@TestTypeParentID),0)
	END


	SELECT 
	j.Charge as LTest
	,LoadTestItem.LID
	,Rol.Name
	,Loc.Tag
	,Loc.Loc
	,Loc.ID
	,Elev.Unit
	,Elev.ID as NID
	,LoadTest.Name as NTest
	,LoadTest.ID as LTID
	,LoadTestItem.Last
	,LoadTestItem.LastDue
	,LoadTestItem.Next
	,LoadTestItem.Status StatusValue
	,ListConfig.ItemName Status      
	,LoadTestItem.Custom1
	,LoadTestItem.Custom2, LoadTestItem.Custom3, LoadTestItem.Custom4, LoadTestItem.Custom5, LoadTestItem.Custom6, LoadTestItem.Custom7, LoadTestItem.Custom8,
	LoadTestItem.Custom9, LoadTestItem.Custom10, LoadTestItem.idRolCustomContact, LoadTestItem.Remarks       ,Elev.State      ,Loc.Route, 
	TicketO.ID idTicket, TicketO.Assigned TicketStatus,dbo.TicketStatusAsText(TicketO.Assigned) TicketStatusText      ,tblWork.ID idWorker, TicketO.EDate,
	tblWork.fDesc CallSign,j.ID as JobID,j.fDesc as JobName , TicketO.Cat,TicketO.Who,LoadTestItem.Amount,LoadTestItem.OverrideAmount,LoadTestItem.ThirdPartyName,LoadTestItem.ThirdPartyPhone,LoadTest.Charge,LoadTest.ThirdParty,LoadTest.NextDateCalcMode
	,Loc.Rol AS RolID, Elev.Type ,LoadTestItem.TestDueBy,LoadTestItem.Chargeable,LoadTestItem.ThirdParty AS ThirdPartyRequired
	,Elev.Classification
	--Parent Test Type
	,ISNULL(@isParentTestType,0) AS IsParentTestType
	,ISNULL(@TestTypeChildID,0) AS TestTypeChildID
	,ISNULL(@TestTypeChildName,'') AS TestTypeChildName
	,ISNULL(@HasChildTest,0) AS HasChildTest
	-- Child Test Type
	,ISNULL(@TestTypeParentName,'') AS TestTypeParentName
	,ISNULL(@TestTypeParentID ,0) AS TestTypeParentID
	,ISNULL(@HasParentTest,0) AS HasParentTest
	,ISNULL(@ParentTestID,0) AS ParentTestID
	,ISNULL(@TestTypeParentChargable,0) AS TestTypeParentChargable
	--,@IsExistTestCover AS IsExistTestCover,@CoveredByLID AS CoveredByLID	,@CoveredByTestType	 AS CoveredByTestType

	FROM LoadTestItem       INNER JOIN Elev ON LoadTestItem.Elev = Elev.ID       
	INNER JOIN Loc ON LoadTestItem.Loc = Loc.Loc       
	INNER JOIN Owner ON Loc.Owner = Owner.ID       
	INNER JOIN Rol ON Owner.Rol = Rol.ID       
	INNER JOIN LoadTest ON LoadTestItem.ID = LoadTest.ID       
	INNER JOIN ListConfig ON ListConfig.ListName='Test.Status' AND LoadTestItem.Status = ListConfig.ItemValue       
	LEFT JOIN LatestInstanceOfEachStatusPerTest x ON LoadTestItem.LID=x.idTest AND LoadTestItem.Status=x.idTestStatus and x.StatusRank = 1   
	LEFT JOIN TicketO ON LoadTestItem.Ticket=TicketO.Id 
	LEFT JOIN Job j ON isnull(LoadTestItem.JobId,TicketO.Job)=j.ID 
	LEFT JOIN tblWork ON TicketO.DWork=tblWork.fDesc
	WHERE LoadtestItem.LID=@LID 

	--

	--Update for exsit data
	DECLARE @PriceYear INT 
	DECLARE @Amount numeric(32,2)
	DECLARE @OverrideAmount numeric(32,2)
	DECLARE @Next datetime 
	DECLARE @Chargeable SMALLINT 
	DECLARE @ThirdParty SMALLINT 
	DECLARE @ThirdPartyName VARCHAR(50) 
	DECLARE @ThirdPartyPhone VARCHAR(50) 

	SELECT @PriceYear=YEAR([Next]), @Amount=ISNULL(Amount,0),@OverrideAmount= ISNULL(OverrideAmount,0),@Next=[Next],@Chargeable=ISNULL(Chargeable,0)
	,@ThirdParty=ISNULL(ThirdParty,0), @ThirdPartyName=ISNULL(ThirdPartyName,''), @ThirdPartyPhone=ISNULL(ThirdPartyPhone,'')
	FROM LoadTestItem WHERE LID=@LID 

	IF (SELECT COUNT(1) FROM LoadTestItemHistoryPrice WHERE LID=@LID  AND PriceYear=@PriceYear)=0
	BEGIN
		if @Amount IS NOT NULL 
		begin
		INSERT INTO LoadTestItemHistoryPrice (LID,PriceYear,CreatedBy,CreatedDate,[DefaultAmount],[OverrideAmount],DueDate,Chargeable,ThirdPartyRequired,ThirdPartyName,ThirdPartyPhone)		
		VALUES (@LID,@PriceYear,'Maintenance',GETDATE(),@Amount,@OverrideAmount,@Next,@Chargeable,@ThirdParty,@ThirdPartyName,@ThirdPartyPhone)	
		end

    END 
END 