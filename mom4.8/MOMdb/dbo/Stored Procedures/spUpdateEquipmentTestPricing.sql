CREATE PROCEDURE [dbo].[spUpdateEquipmentTestPricing]
  @ID int,
 @Classification varchar(50),
 @TestTypeId int,
 @Amount numeric(30,2),
 @Override numeric(30,2),
 @UpdatedBy varchar(50),
 @UpdateType numeric(30,2),
 @Remarks  varchar(500),
 @DefaultHour  numeric(30,2),
 @PriceYear INT,
 @ThirdPartyRequired BIT
AS 
Begin
Declare @old_Amount numeric(30,2)
Declare @old_Override numeric(30,2)
select @old_Amount=ISNULL([Amount],0),@old_Override=ISNULL([Override],0) from [EquipmentTestPricing] WHERE ID=@ID

UPDATE [EquipmentTestPricing]
   SET [Classification] = @Classification
      ,[TestTypeId] =  @TestTypeId
	  ,[Amount] = @Amount
      ,[Override] = @Override	 
      ,[LastUpdateDate] =GETDATE ( ) 
	  ,[UpdatedBy]=@UpdatedBy
	  ,[Remarks]=@Remarks
	  ,[DefaultHour]=@DefaultHour
	  ,PriceYear=@PriceYear
	  ,ThirdPartyRequired=@ThirdPartyRequired
 WHERE ID=@ID

 --UPDATE FOR ALL Equipment 
 IF @UpdateType=1 
	 BEGIN
	
		UPDATE LoadTestItemHistoryPrice 
		SET DefaultAmount=@Amount,[OverrideAmount]	=@Override
		WHERE LID IN (SELECT item.LID FROM LoadTestItem  item
						LEFT JOIN Elev e ON e.ID=item.Elev
						WHERE item.ID=@TestTypeId AND e.Classification=@Classification ) 
			AND PriceYear=@PriceYear
			AND Chargeable=1
			--AND(( isnull(DefaultAmount,0)= @old_Amount and isnull([OverrideAmount],0)=@old_Override) or(isnull(DefaultAmount,0)=0 and isnull([OverrideAmount],0)=0 ))

		INSERT INTO LoadTestItemHistoryPrice (LID,PriceYear,DefaultAmount,OverrideAmount,Chargeable,ThirdPartyRequired,ThirdPartyName,ThirdPartyPhone)
		SELECT item.LID, @PriceYear,@Amount,@Override,1 ,0,'',''
		FROM LoadTestItem item		
		LEft JOIN Elev e ON e.ID=item.Elev
		WHERE (SELECT COUNT(1) FROM LoadTestItemHistoryPrice p WHERE Item.LID= p.LID AND p.PriceYear=@PriceYear)=0
		AND item.ID=@TestTypeId AND e.Classification=@Classification
	
	 END

End

