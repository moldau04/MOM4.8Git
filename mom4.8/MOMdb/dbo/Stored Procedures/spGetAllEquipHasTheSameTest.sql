CREATE PROCEDURE [dbo].[spGetAllEquipHasTheSameTest]
@Loc INT,
@TestType Int,
@YearProposal INT,
@Chargeable bit,
@Classification VARCHAR(100),
@ElevID Int
AS
IF OBJECT_ID('tempdb..#tempElev') IS NOT NULL DROP TABLE #tempElev
CREATE TABLE #tempElev
(	
	Elev int
)
--TestType is covered by another type
DECLARE @TestCover INT
DECLARE @CoverByTest INT
DECLARE @IsExistTestCover INT	
SET @TestCover =isnull((SELECT TOP 1 TestTypeCoverID FROM TestTypeCover WHERE TestTypeID=@TestType),0)
SET @CoverByTest =isnull((SELECT TOP 1 TestTypeID FROM TestTypeCover WHERE TestTypeCoverID=@TestType),0)
IF @TestCover =0--TestType not corver 
BEGIN
	print 'TestType not corver '
	IF @CoverByTest=0
	BEGIN		
		print 'Test Type is not covered by another test'
			INSERT into #tempElev
			SELECT Elev from LoadTestItem item
			INNER JOIN Elev e ON e.ID= item.Elev
			WHERE  
			item.ID=@TestType and 
			item.Loc=@loc 
			AND isnull(Chargeable,0)=@Chargeable 		
			AND LID in( select LID from LoadTestItemHistoryPrice where Year(DueDate)=@YearProposal AND isnull(Chargeable,0)=@Chargeable )
			AND e.Classification=@Classification		
			AND ISNULL((SELECT count (1) FROM LoadTestItem WHERE LoadTestItem.ID in(SELECT TestTypeID FROM TestTypeCover WHERE TestTypeCoverID=@TestType) AND Loc=@Loc AND e.ID=LoadTestItem.Elev AND YEAR(Next)=@YearProposal),0)=0				
			AND LID NOT IN (SELECT TestID FROM ProposalFormDetail pfd WHERE pfd.TestID=item.LID AND YearProposal=@YearProposal AND EquipmentId=e.ID ) 			
	END
	ELSE
	BEGIN
		print 'Test Type is covered by test type'		
		SET @IsExistTestCover=ISNULL((SELECT COUNT(1) FROM LoadTestItem WHERE   Elev=@ElevID and
			(select count(1) from LoadTestItem lsub where ID=@TestType and isnull(Year(Next), Year(GETDATE()))=@YearProposal and loc=@Loc and lsub.Elev=@ElevID)=1
			and
			(select count(1) from LoadTestItem lsub where ID=@CoverByTest and isnull(Year(Next), Year(GETDATE()))=@YearProposal and loc=@Loc and lsub.Elev=@ElevID)=1
		),0)
		IF @IsExistTestCover=0
		BEGIN
			print 'This test does not have test cover'
			Insert into #tempElev
			SELECT Elev from LoadTestItem item
			INNER JOIN Elev e ON e.ID= item.Elev
			WHERE  
			item.ID=@TestType and 
			item.Loc=@loc 
			AND isnull(Chargeable,0)=@Chargeable 		
			AND LID in( select LID from LoadTestItemHistoryPrice where Year(DueDate)=@YearProposal AND isnull(Chargeable,0)=@Chargeable )
			AND e.Classification=@Classification
			AND ISNULL((SELECT count (1) FROM LoadTestItem WHERE LoadTestItem.ID in(SELECT TestTypeID FROM TestTypeCover WHERE TestTypeCoverID=@TestType) AND Loc=@Loc AND e.ID=LoadTestItem.Elev AND YEAR(Next)=@YearProposal),0)=0				
			AND LID NOT IN (SELECT TestID FROM ProposalFormDetail pfd WHERE pfd.TestID=item.LID AND YearProposal=@YearProposal AND EquipmentId=e.ID ) 	
			AND e.ID IN (SELECT distinct item2.elev from LoadTestItem item2
			where (select count(1) from LoadTestItem lsub where lsub.ID=@TestType and isnull(Year(lsub.Next), Year(GETDATE()))=@YearProposal and lsub.loc=@Loc and lsub.Elev=item2.elev)=1
			and (select count(1) from LoadTestItem lsub where lsub.ID=@CoverByTest and isnull(Year(lsub.Next), Year(GETDATE()))=@YearProposal and lsub.loc=@Loc and lsub.Elev=item2.elev)=0)

			
		END
		ELSE
		BEGIN
		    DECLARE @newCharagable BIT
            SET @newCharagable=(select Chargeable from LoadTestItem lsub where ID=@CoverByTest and isnull(Year(Next), Year(GETDATE()))=@YearProposal and loc=@Loc and lsub.Elev=@ElevID)
			print 'This test has test cover'
			Insert into #tempElev
			SELECT Elev from LoadTestItem item
			INNER JOIN Elev e ON e.ID= item.Elev
			WHERE  
			item.ID=@TestType and 
			item.Loc=@loc 			
			AND LID in( select LID from LoadTestItemHistoryPrice where Year(DueDate)=@YearProposal )
			AND e.Classification=@Classification
			AND ISNULL((SELECT count (1) FROM LoadTestItem WHERE LoadTestItem.ID in(SELECT TestTypeID FROM TestTypeCover WHERE TestTypeCoverID=@TestType) AND Loc=@Loc AND e.ID=LoadTestItem.Elev AND YEAR(Next)=@YearProposal),0)=0				
			AND LID NOT IN (SELECT TestID FROM ProposalFormDetail pfd WHERE pfd.TestID=item.LID AND YearProposal=@YearProposal AND EquipmentId=e.ID ) 	
			AND e.ID IN (SELECT distinct item2.elev from LoadTestItem item2
			where (select count(1) from LoadTestItem lsub where lsub.ID=@TestType and isnull(Year(lsub.Next), Year(GETDATE()))=@YearProposal and lsub.loc=@Loc and lsub.Elev=item2.elev)=1
			and (select count(1) from LoadTestItem lsub where lsub.ID=@CoverByTest and isnull(Year(lsub.Next), Year(GETDATE()))=@YearProposal and lsub.loc=@Loc and lsub.Elev=item2.elev  AND lsub.Chargeable=@newCharagable)=1)

		END
	END
END 
ELSE 
BEGIN
	print 'TestType not corver '
	if @ElevID=0
	BEGIN
		print 'ElevID=0'
		Insert into #tempElev
		SELECT Elev from LoadTestItem item
			INNER JOIN Elev e ON e.ID= item.Elev
			WHERE  
			item.ID=@TestType and 
			item.Loc=@loc 			
			AND isnull(Chargeable,0)=@Chargeable 	
			AND LID in( select LID from LoadTestItemHistoryPrice where Year(DueDate)=@YearProposal  AND isnull(Chargeable,0)=@Chargeable )
			AND e.Classification=@Classification
			AND ISNULL((SELECT count (1) FROM LoadTestItem WHERE LoadTestItem.ID in(SELECT TestTypeID FROM TestTypeCover WHERE TestTypeCoverID=@TestType) AND Loc=@Loc AND e.ID=LoadTestItem.Elev AND YEAR(Next)=@YearProposal),0)=0				
			AND LID NOT IN (SELECT TestID FROM ProposalFormDetail pfd WHERE pfd.TestID=item.LID AND YearProposal=@YearProposal AND EquipmentId=e.ID ) 		
	END
	ELSE
	BEGIN	
		print 'Has ElevID '
		SET @IsExistTestCover=ISNULL((SELECT COUNT(1) FROM LoadTestItem WHERE    Elev=@ElevID and
		(select count(1) from LoadTestItem lsub where ID=@TestType and isnull(Year(Next), Year(GETDATE()))=@YearProposal and loc=@Loc and lsub.Elev=@ElevID)=1
		and
		(select count(1) from LoadTestItem lsub where ID=@TestCover and isnull(Year(Next), Year(GETDATE()))=@YearProposal and loc=@Loc and lsub.Elev=@ElevID)=1
		),0)
	
		IF @IsExistTestCover=0
		BEGIN 
			print 'This test does not have Test cover'
			Insert into #tempElev
			SELECT Elev from LoadTestItem item
			INNER JOIN Elev e ON e.ID= item.Elev
			WHERE  
			item.ID=@TestType and 
			item.Loc=@loc 			
			AND isnull(Chargeable,0)=@Chargeable 	
			AND LID in( select LID from LoadTestItemHistoryPrice where Year(DueDate)=@YearProposal  AND isnull(Chargeable,0)=@Chargeable )
			AND e.Classification=@Classification					
			AND LID NOT IN (SELECT TestID FROM ProposalFormDetail pfd WHERE pfd.TestID=item.LID AND YearProposal=@YearProposal AND EquipmentId=e.ID ) 		
			AND e.ID IN (SELECT distinct item2.elev from LoadTestItem item2
				where (select count(1) from LoadTestItem lsub where ID=@TestType and isnull(Year(lsub.Next), Year(GETDATE()))=@YearProposal and lsub.loc=@Loc and lsub.Elev=item2.elev)=1
				AND (select count(1) from LoadTestItem lsub where ID=@TestCover and isnull(Year(lsub.Next), Year(GETDATE()))=@YearProposal and lsub.loc=@Loc and lsub.Elev=item2.elev)=0)	


			

		END
		ELSE
		BEGIN
			print 'This test has Test cover'
			Insert into #tempElev
			SELECT Elev from LoadTestItem item
			INNER JOIN Elev e ON e.ID= item.Elev
			WHERE  
			item.ID=@TestType and 
			item.Loc=@loc 			
			AND isnull(Chargeable,0)=@Chargeable 	
			AND LID in( select LID from LoadTestItemHistoryPrice where Year(DueDate)=@YearProposal  AND isnull(Chargeable,0)=@Chargeable )
			AND e.Classification=@Classification					
			AND LID NOT IN (SELECT TestID FROM ProposalFormDetail pfd WHERE pfd.TestID=item.LID AND YearProposal=@YearProposal AND EquipmentId=e.ID ) 		
			AND e.ID IN (SELECT distinct item2.elev from LoadTestItem item2
				where (select count(1) from LoadTestItem lsub where ID=@TestType and isnull(Year(lsub.Next), Year(GETDATE()))=@YearProposal and lsub.loc=@Loc and lsub.Elev=item2.elev)=1
				AND (select count(1) from LoadTestItem lsub where ID=@TestCover and isnull(Year(lsub.Next), Year(GETDATE()))=@YearProposal and lsub.loc=@Loc and lsub.Elev=item2.elev)=1)				

		END
	END

	
END

SELECT (SELECT tag
            FROM   Loc
            WHERE  Loc = e.Loc) AS location,
            (SELECT ID
            FROM   Loc
            WHERE  Loc = e.Loc) AS locationID,
           e.Loc,
           e.Owner,
		   r.EN,
		   B.Name As Company,
           e.Unit,
           e.fDesc,
           e.Type,
           e.Cat,
           e.Manuf,
           e.Serial,
           e.State,
           e.Since,
           e.Last,
           e.Price,
           e.Status,
           e.Building,
           e.Remarks,
           e.fGroup,
           e.Template,
           e.InstallBy,
           e.install,
           e.category,
           e.ID as unitid,
		   e.Classification,
		   e.shut_down,
		   e.ShutdownReason
	FROM   Elev e INNER JOIN loc l ON l.Loc = e.Loc INNER JOIN owner o ON o.id = l.owner
	INNER JOIN rol r ON o.rol = r.id left Outer join Branch B on r.EN = B.ID
	WHERE  e.ID IN (select Elev from #tempElev)