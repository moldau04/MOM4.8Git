CREATE PROCEDURE [dbo].[spUpdateTestByYear]		         	
	@typeID int
	,@Loc int
	,@Elev int
	,@Last datetime =null
	,@Next datetime =null
	,@Status int
	,@Ticket int =null
	,@Remarks ntext=null
	,@LastDue datetime=null
	,@JobId int=null
	,@fWork int=null
	,@DWork nvarchar=null
	,@Custom1 varchar(50)
	,@Custom2 varchar(50)
	,@Custom3 varchar(50)
	,@Custom4 varchar(50)		  
	,@Amount numeric(32,2)
	,@OverrideAmount numeric(32,2)
	,@ThirdPartyName varchar(50)
	,@ThirdPartyPhone varchar(50)  
	,@TestDueBy int
	,@Charge INT
	,@ThirdParty INT
	,@PriceYear INT 
	,@CreatedBy VARCHAR(100)
	,@TestCustomItemValue    tblTypeTestCustomItemValue readonly
	,@isDefautTest INT=0
	,@id INT
	,@CreateTestHistory BIT
    ,@UpdateThirdPartyForAll Bit
AS 
BEGIN 		
DECLARE @TestTypeParent INT
DECLARE @TestTypeChild INT
DECLARE @currentTicketStatus INT

IF (SELECT COUNT(1) FROM LoadTestItemHistory WHERE LID=@id AND TestYear=@PriceYear)=0
	BEGIN
		INSERT INTO LoadTestItemHistory ([LID],[TestYear],[TestStatus],Next,IsActive)
		SELECT @id, @PriceYear,0,@Next ,1 FROM LoadTestItem WHERE LID=@ID
		
    end


IF @Ticket !=NULL
BEGIN
SELECT TOP 1 @currentTicketStatus=Status
	FROM (
			SELECT Assigned AS Status
			FROM TicketD   
			LEFT OUTER JOIN tblWork w WITH(NOLOCK) on TicketD.fWork=w.ID  
			WHERE TicketD.ID=@Ticket	
			UNION
			SELECT Assigned  FROM TicketO    
			LEFT OUTER JOIN tblWork w WITH(NOLOCK) on TicketO.fWork=w.ID  
			WHERE TicketO.ID=@Ticket 
			UNION
			SELECT 0 FROM TicketDPDA 
			LEFT OUTER JOIN tblWork w WITH(NOLOCK) on TicketDPDA.fWork=w.ID  
		WHERE TicketDPDA.ID=@Ticket 
	) t
END



IF(@isDefautTest=1)
BEGIN
	--Update default Test
	DECLARE @oldYear INT
    SET @oldYear= (SELECT YEAR(Next) FROM LoadTestItem WHERE LID=@id)
	UPDATE  [dbo].[LoadTestItem]
	SET   
        [ID]=@typeID
        ,[Loc]=@Loc
        ,[Elev]=@Elev
        ,[Last]=@Last
        ,[Next]=@Next
        ,[Status]=@Status
        ,[Ticket]=@Ticket
        ,[Remarks]=@Remarks
        ,[LastDue]=@LastDue
		,[JobId]=@JobId 
		,[Custom1]=@Custom1
		,[Custom2]=@Custom2
		,[Custom3]=@Custom3
		,[Custom4]=@Custom4
		,[Amount]=@Amount
		,[OverrideAmount]=@OverrideAmount
		,[ThirdPartyName]=@ThirdPartyName
		,[ThirdPartyPhone]=@ThirdPartyPhone
		,[TestDueBy]=@TestDueBy
		,[Chargeable]=@Charge
		,[ThirdParty]=@ThirdParty			
	WHERE  [LID]=@id

	UPDATE LoadTestItemHistory
	SET TestStatus=@Status
		,[TicketID]=@Ticket
		,[LastDue]=@LastDue
		,[Last]=@Last
		,[Next]=@Next
		,[JobId]=@JobId 
		WHERE LID=@ID AND TestYear=@oldYear
END
ELSE
BEGIN
	--Update for history
	IF (SELECT COUNT(1) FROM LoadTestItemHistory WHERE LID=@ID AND TestYear=@PriceYear)=0
		BEGIN			
			INSERT INTO LoadTestItemHistory ([LID],[TestYear],[TestStatus],[Last],[Next],[TicketID],[TicketStatus],[Worker],[Schedule] ,[UpdatedBy],[LastDue],fWork,JobId,IsActive)
			SELECT @ID,@PriceYear,@Status,@Last,@Next ,@Ticket,null,@DWork,null,@CreatedBy,@LastDue,@fwork ,@JobId,1 FROM LoadTestItem WHERE LID=@id
		END 
		ELSE
		BEGIN
			UPDATE LoadTestItemHistory
			SET TestStatus=@Status
			,[TicketID]=@Ticket
			,[LastDue]=@LastDue
			 ,[Last]=@Last
			 ,[Next]=@Next
			 WHERE LID=@ID AND TestYear=@PriceYear
		END

		IF @CreateTestHistory=1
		BEGIN       
		INSERT INTO [dbo].[TestHistory]
           ( [idTest] ,[StatusDate],[UserName],[TestStatus],[LastDate],[idTestStatus],[ActualDate],[TicketID],[TicketStatus],[NextDueDate],[LastDueDate] )
		VALUES
           (@ID ,GETDATE(),@CreatedBy ,@Status ,@Last,@Status,null ,@Ticket, @currentTicketStatus,@Next,@LastDue		   )
		 END 
END


	

IF (@Ticket is not null)
BEGIN 
	update TicketO set fWork=@fWork,DWork=(select Top 1 fDesc from tblWork where id=@fWork) where TicketO.ID=@Ticket
END    

--price			
IF (SELECT COUNT(1) FROM LoadTestItemHistoryPrice WHERE LID=@id AND PriceYear=@PriceYear)!=1
BEGIN		
	INSERT INTO LoadTestItemHistoryPrice (LID,PriceYear,CreatedBy,CreatedDate,[DefaultAmount],[OverrideAmount],DueDate,Chargeable,ThirdPartyRequired,ThirdPartyName,ThirdPartyPhone)
VALUES (@Id,@PriceYear,@CreatedBy,GETDATE(),@Amount,@OverrideAmount,@Next,@Charge,@ThirdParty,@ThirdPartyName,@ThirdPartyPhone)	
END 
ELSE
BEGIN
	UPDATE LoadTestItemHistoryPrice 
	SET DefaultAmount=@Amount,[OverrideAmount]	=@OverrideAmount, DueDate=@Next, Chargeable=@Charge,ThirdPartyRequired=@ThirdParty,[ThirdPartyName]=@ThirdPartyName,[ThirdPartyPhone]=@ThirdPartyPhone
	WHERE LID=@id AND PriceYear=@PriceYear
END 

DECLARE @Classification VARCHAR(200)
SET @Classification =(SELECT  Classification FROM Elev WHERE ID=@Elev)

IF @UpdateThirdPartyForAll=1 
BEGIN
	UPDATE LoadTestItemHistoryPrice
	SET ThirdPartyName= @ThirdPartyName , ThirdPartyPhone=@ThirdPartyPhone, ThirdPartyRequired=@ThirdParty
	WHERE PriceYear=@PriceYear AND LID IN (SELECT LID FROM LoadTestItem item INNER JOIN Elev e ON e.ID=item.Elev
											WHERE e.Classification=@Classification AND item.ID=@typeID AND item.Loc=@Loc)

	--SET @TestTypeParent=(SELECT TOP 1 TestTypeID FROM TestTypeCover WHERE TestTypeCoverID=@typeID)


	--UPDATE LoadTestItemHistoryPrice
	--SET ThirdPartyName= @ThirdPartyName , ThirdPartyPhone=@ThirdPartyPhone, ThirdPartyRequired=@ThirdParty
	--WHERE PriceYear=@PriceYear AND LID IN (SELECT LID FROM LoadTestItem item INNER JOIN Elev e ON e.ID=item.Elev
	--										WHERE e.Classification=@Classification AND item.ID=@TestTypeParent AND item.Loc=@Loc)
											

	--SET @TestTypeChild=(SELECT TOP 1 TestTypeCoverID FROM TestTypeCover WHERE TestTypeID=@typeID)
	--UPDATE LoadTestItemHistoryPrice
	--SET ThirdPartyName= @ThirdPartyName , ThirdPartyPhone=@ThirdPartyPhone, ThirdPartyRequired=@ThirdParty
	--WHERE PriceYear=@PriceYear AND LID IN (SELECT LID FROM LoadTestItem item INNER JOIN Elev e ON e.ID=item.Elev
	--										WHERE e.Classification=@Classification AND item.ID=@TestTypeChild AND item.Loc=@Loc)

											
END 


END 