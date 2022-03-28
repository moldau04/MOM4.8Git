CREATE PROCEDURE [dbo].[spUpdateTestPriceByYear]
	@LID varchar(500),	
	@PriceYear INT,
	@Charagable INT,
	@DefaultAmount numeric(30,2),
	@OverrideAmount numeric(30,2),
	@ThirdParty INT,
	@ThirdPartyName VARCHAR(200),
	@ThirdPartyPhone VARCHAR(50),
		@UpdateBy VARCHAR(200),
	@UpdateAll INT,
	@IsNew INT,
	@ReturnValue INT OUTPUT
AS
BEGIN
DECLARE @TestTypeId INT
DECLARE @Loc Int
DECLARE @Classification VARCHAR(200)
--0:ERROR
--1:Success
--2:PriceYear exist
	SET @ReturnValue=0
IF @IsNew=1
	BEGIN
		IF (SELECT COUNT(*) FROM LoadTestItemHistoryPrice WHERE LID=@LID AND PriceYear=@PriceYear)=0
		BEGIN		
			INSERT INTO LoadTestItemHistoryPrice (LID,PriceYear,Chargeable,DefaultAmount,OverrideAmount,ThirdPartyRequired,ThirdPartyName,ThirdPartyPhone,CreatedBy,CreatedDate)
			VALUES(@LID, @PriceYear,@Charagable,@DefaultAmount,@OverrideAmount,@ThirdParty,@ThirdPartyName,@ThirdPartyPhone,@UpdateBy,GETDATE()) 
			SET @ReturnValue=1
		END 
		ELSE 
		BEGIN
			SET @ReturnValue=2
        END 
	
	END 
ELSE
	BEGIN
		UPDATE LoadTestItemHistoryPrice
		SET DefaultAmount=@DefaultAmount , OverrideAmount=@OverrideAmount , ThirdPartyName=@ThirdPartyName,ThirdPartyPhone=@ThirdPartyPhone,Chargeable=@Charagable,ThirdPartyRequired=@ThirdParty
		,CreatedBy=@UpdateBy, CreatedDate=GETDATE()
		WHERE LID=@LID  AND PriceYear=@PriceYear
		SET @ReturnValue=1
	END 

IF @UpdateAll=1 and @Charagable=1
BEGIN 

	 SELECT @Classification= Elev.Classification,@Loc=item.Loc,@TestTypeId=item.ID FROM LoadTestItem item
	 INNER JOIN Elev ON Elev.ID=item.Elev
	 WHERE LID=@LID

	DECLARE @c_TestID int
	DECLARE cur_Loc CURSOR FOR 	
		SELECT LID FROM LoadTestItem WHERE ID=@TestTypeId AND Elev in(select ID from Elev where Classification=@Classification) AND Loc=@Loc
	OPEN cur_Loc  
	FETCH NEXT FROM cur_Loc INTO @c_TestID
	WHILE @@FETCH_STATUS = 0  
		BEGIN
			IF(SELECT COUNT(1) FROM LoadTestItemHistoryPrice WHERE LID=@c_TestID AND  PriceYear=@PriceYear) =0
			BEGIN	
				 INSERT INTO LoadTestItemHistoryPrice (LID,PriceYear,DefaultAmount,OverrideAmount,CreatedBy,Chargeable,CreatedDate)
				 VALUES( @c_TestID,@PriceYear,@DefaultAmount,@OverrideAmount,@UpdateBy,1,GETDATE()) 
            END
            ELSE
            BEGIN
				UPDATE LoadTestItemHistoryPrice
				SET DefaultAmount=@DefaultAmount , OverrideAmount=@OverrideAmount, CreatedBy=@UpdateBy, CreatedDate=GETDATE()
				WHERE LID=@c_TestID  AND PriceYear=@PriceYear AND Chargeable=1
            END 
		FETCH NEXT FROM cur_Loc INTO @c_TestID
		END	
	CLOSE cur_Loc  
	DEALLOCATE cur_Loc  

END



return @ReturnValue

END