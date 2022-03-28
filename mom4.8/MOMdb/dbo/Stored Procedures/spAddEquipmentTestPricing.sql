CREATE procedure [dbo].[spAddEquipmentTestPricing] 
 @TestTypeId int, 
 @Classification varchar(50),
 @Amount numeric(30,2),
 @Override numeric(30,2),
 @CreatedBy varchar(50),
 @Remarks varchar(500),
 @DefaultHour numeric(30,2),
  @PriceYear INT,
 @ThirdPartyRequired BIT,
 @ID int OUTPUT
AS 
Begin

Declare @priceID int

INSERT INTO [EquipmentTestPricing]
           ([TestTypeId]
           ,[Classification]		  
           ,[Amount]
		   ,[Override]
           ,[LastUpdateDate]
		   ,[CreatedBy]
		   ,[Remarks]
		   ,[DefaultHour]
		   ,[PriceYear]
		   ,[ThirdPartyRequired])
     VALUES
           (
            @TestTypeId
		   ,@Classification
		   ,@Amount
           ,@Override		   
           ,GETDATE ( )
		   ,@CreatedBy
		   ,@Remarks
		   ,@DefaultHour
		   ,@PriceYear
		   ,@ThirdPartyRequired)


Set @priceID= @@IDENTITY



SELECT item.LID, @PriceYear,@Amount,@Override,1 ,0,'',''
		FROM LoadTestItem item		
		LEft JOIN Elev e ON e.ID=item.Elev
		WHERE (SELECT COUNT(1) FROM LoadTestItemHistoryPrice p WHERE Item.LID= p.LID AND p.PriceYear=@PriceYear)=0
		AND item.ID=@TestTypeId AND e.Classification=@Classification


		INSERT INTO LoadTestItemHistoryPrice (LID,PriceYear,DefaultAmount,OverrideAmount,Chargeable,ThirdPartyRequired,ThirdPartyName,ThirdPartyPhone)
		SELECT item.LID, @PriceYear,@Amount,@Override,1 ,0,'',''
		FROM LoadTestItem item		
		LEft JOIN Elev e ON e.ID=item.Elev
		WHERE (SELECT COUNT(1) FROM LoadTestItemHistoryPrice p WHERE Item.LID= p.LID AND p.PriceYear=@PriceYear)=0
		AND item.ID=@TestTypeId AND e.Classification=@Classification

		

Set @ID=@priceID

End