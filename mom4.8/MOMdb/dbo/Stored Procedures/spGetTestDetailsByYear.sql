CREATE PROCEDURE [dbo].[spGetTestDetailsByYear] 			
	@LID INT=NULL,
	@testYear int
AS 
BEGIN
	DECLARE @IsExistData int
	DECLARE @IsDefault int
	declare @currentNext Datetime
	declare @newLast Datetime
	declare @newNext Datetime
	declare @newLastDue Datetime
	
	DECLARE @ElevID INT
	
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
	 Declare @TicketID nvarchar(100)  ='';   
	 SET @IsExistData=1;
	 if @testYear=0
	Begin
		set @testYear= (select year(next) from LoadTestItem where LID=@LID)
		--set @IsDefault=1;
	END


	SELECT TOP 1 @TestType=item.ID,@ElevID=item.Elev,@ticketID=itemHistory.TicketID
	,@newLast=isnull( convert(varchar(10),DATEADD(year,(((@testYear-year(item.Next))/ (ttype.Frequency/12.0))* (ttype.Frequency/12.0))-(ttype.Frequency/12.0), item.Next) ,101),'')
	,@newLastDue=isnull( convert(varchar(10),DATEADD(year,(((@testYear-year(item.Next))/ (ttype.Frequency/12.0))*(ttype.Frequency/12.0))-(ttype.Frequency/12.0), item.Next) ,101),'')
	,@newNext=isnull( convert(varchar(10),DATEADD(year,((@testYear-year(item.Next))/ (ttype.Frequency/12.0))* (ttype.Frequency/12.0), item.Next) ,101),'')

	FROM LoadTestItem item
	LEFT JOIN LoadTestItemHistory  itemHistory ON item.LID=itemHistory.LID
	LEFT join LoadTest ttype on ttype.ID=item.ID
	WHERE item.LID=@LID
	ORDER BY itemHistory.TestYear desc


	
	SELECT TOP 1 @ticketID=itemHistory.TicketID	
	FROM LoadTestItem item
	LEFT JOIN LoadTestItemHistory  itemHistory ON item.LID=itemHistory.LID
	LEFT join LoadTest ttype on ttype.ID=item.ID
	WHERE item.LID=@LID AND itemHistory.TestYear=@testYear
	ORDER BY itemHistory.TicketID desc
    
	IF (SELECT COUNT(1) FROM LoadTestItemHistory WHERE LID=@LID AND TestYear=@testYear)=0
	BEGIN
		--INSERT INTO LoadTestItemHistory ([LID],[TestYear],[TestStatus],Next,IsActive)
		--SELECT @LID, @testYear,0,@newNext ,1 FROM LoadTestItem WHERE LID=@LID
		SET @IsExistData=0
    end




	

	SET @isParentTestType=ISNULL((SELECT COUNT(1) FROM TestTypeCover WHERE TestTypeID=@TestType),0)
	IF (@isParentTestType=1)
	BEGIN
		    
		SET @TestTypeChildID=(SELECT TOP 1 TestTypeCoverID FROM TestTypeCover WHERE TestTypeID=@TestType)
		SET @TestTypeChildName=ISNULL( (SELECT Name FROM LoadTest WHERE ID =@TestTypeChildID),'')
		SET @HasChildTest= ISNULL((   SELECT  COUNT(1)
									FROM LoadTestItem item
									INNER JOIN LoadTestItemHistory  itemHistory ON item.LID=itemHistory.LID AND itemHistory.TestYear=@testYear
									WHERE item.Elev=@ElevID AND item.ID=@TestTypeChildID AND ISNULL(itemHistory.TestStatus,0)<>3),0)	
 
    END
	ELSE
    BEGIN
	
		SET @TestTypeParentID=(SELECT TOP 1 TestTypeID FROM TestTypeCover WHERE TestTypeCoverID=@TestType)
		
		SET @TestTypeParentName=ISNULL( (SELECT Name FROM LoadTest WHERE ID =@TestTypeParentID),'')		
		SET @HasParentTest=0
		SET @ParentTestID= ISNULL((   SELECT  item.LId
									FROM LoadTestItem item
									INNER JOIN LoadTestItemHistory  itemHistory ON item.LID=itemHistory.LID  AND TestYear=@testYear
									WHERE item.Elev=@ElevID	AND item.ID=@TestTypeParentID AND ISNULL(itemHistory.TestStatus,0)<>3),0)
		IF @ParentTestID <>0
		BEGIN 
			SET @HasParentTest=1
		END

		SET @TestTypeParentChargable=isnull((SELECT  item.LId
											FROM LoadTestItem item
											INNER JOIN LoadTestItemHistoryPrice  itemHistory ON item.LID=itemHistory.LID    AND PriceYear=@testYear	
											WHERE item.Elev=@ElevID AND item.ID=@TestTypeParentID ),0)

		
	END


	 Declare @Workid nvarchar(100)  ='';   
	 Declare @Cat varchar(100)  ='';   
		
		SELECT  @Workid =t.fWork, @Cat=Cat from (
		 select fWork , Cat AS Cat FROM TicketD   where ID=@TicketID	
		 Union
         select fWork, Cat AS Cat from TicketO    where ID=@TicketID 
		 Union
         select fWork , Cat AS Cat FROM TicketDPDA where ID=@TicketID   ) as t

	SELECT 	
	LoadTestItem.LID
	,Rol.Name
	,Loc.Tag
	,Loc.Loc
	,Loc.ID
	,Elev.Unit
	,Elev.ID as NID
	,LoadTest.Name as NTest
	,LoadTest.ID as LTID
	,itemHistory.Last 
	,isnull(itemHistory.LastDue,@newLastDue) as LastDue
	,isnull(itemHistory.Next,@newNext) as Next
	,isnull(itemHistory.TestStatus,0) StatusValue
	--,ListConfig.ItemName Status      
	,LoadTestItem.Custom1
	,LoadTestItem.Custom2, LoadTestItem.Custom3, LoadTestItem.Custom4, LoadTestItem.Custom5, LoadTestItem.Custom6, LoadTestItem.Custom7, LoadTestItem.Custom8,
	LoadTestItem.Custom9, LoadTestItem.Custom10, LoadTestItem.idRolCustomContact, LoadTestItem.Remarks       ,Elev.State      ,Loc.Route, 



	--Ticket 
	itemHistory.TicketID AS idTicket
	,itemHistory.TestStatus AS TicketStatus
	,CASE itemHistory.TicketStatus
					   WHEN 0 THEN 'Open'
					   WHEN 1 THEN 'Assigned'
					   WHEN 2 THEN 'En Route'
					   WHEN 3 THEN 'On Site'
					   WHEN 4 THEN 'Completed'
					   WHEN 5 THEN 'On Hold'
					END AS TicketStatusText
	,itemHistory.Worker
	,@Workid idWorker
	,(	SELECT TOP 1 items FROM [dbo].[Split]( itemHistory.Schedule , ',')) as EDate
	,isnull(itemHistory.Worker,'') CallSign
	,isnull(itemHistory.TestStatus,'0')
	--,ListConfig.ItemName Status  

	----Ticket	
	--TicketO.ID idTicket, TicketO.Assigned TicketStatus,dbo.TicketStatusAsText(TicketO.Assigned) TicketStatusText      ,tblWork.ID idWorker, TicketO.EDate,
	,tblWork.fDesc CallSign,j.ID as JobID,j.fDesc as JobName ,@Cat as Cat,itemHistory.Who

	--Job


	--Price History
	,priceHistory.DefaultAmount as Amount
	,priceHistory.OverrideAmount as OverrideAmount
	,priceHistory.ThirdPartyName,priceHistory.ThirdPartyPhone,
	ISNULL(priceHistory.Chargeable,isnull(LoadTestItem.Chargeable,1)) as Chargeable
	,priceHistory.ThirdPartyRequired AS ThirdPartyRequired

	,LoadTest.Charge,LoadTest.ThirdParty,LoadTest.NextDateCalcMode
	,Loc.Rol AS RolID, Elev.Type ,LoadTestItem.TestDueBy
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
	
	,itemHistory.isTestDefault
	,@IsExistData AS ExistData
	FROM LoadTestItem       
	INNER JOIN Elev ON LoadTestItem.Elev = Elev.ID       
	INNER JOIN Loc ON LoadTestItem.Loc = Loc.Loc       
	INNER JOIN Owner ON Loc.Owner = Owner.ID       
	INNER JOIN Rol ON Owner.Rol = Rol.ID       
	INNER JOIN LoadTest ON LoadTestItem.ID = LoadTest.ID  
	LEFT JOIN LatestInstanceOfEachStatusPerTest x ON LoadTestItem.LID=x.idTest AND LoadTestItem.Status=x.idTestStatus and x.StatusRank = 1   	
	LEFT JOIN LoadTestItemHistory  itemHistory ON LoadTestItem.LID= itemHistory.LID AND itemHistory.TestYear=@testYear
	LEFT JOIN LoadTestItemHistoryPrice priceHistory ON LoadTestItem.LID= priceHistory.LID AND priceHistory.PriceYear=@testYear
	LEFT JOIN TicketO ON itemHistory.TicketID=TicketO.Id 
	LEFT JOIN tblWork ON itemHistory.Worker=tblWork.fDesc
	LEFT JOIN Job j ON isnull(itemHistory.JobId,TicketO.Job)=j.ID 
	WHERE LoadtestItem.LID=@LID 


	
END 