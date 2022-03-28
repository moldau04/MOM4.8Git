CREATE  PROC [dbo].[UpdateLoadTestItemDates] (
  @idTestItem INT
 ,@jLast SMALLDATETIME
 ,@UserName VARCHAR(50)
) AS BEGIN

 UPDATE LoadTestItem
    SET Last = @jLast
       ,LastDue = Next
       ,Next = DATEADD(m,t.Frequency,CASE t.NextDateCalcMode
                                       WHEN 0 THEN @jLast
                                       WHEN 1 THEN i.Next
                                       WHEN 2 THEN CASE WHEN @jLast <= i.Next THEN @jLast ELSE i.Next END
                                       WHEN 3 THEN CASE WHEN @jLast > i.Next THEN @jLast ELSE i.Next END
                                     END)		
   FROM LoadTestItem i INNER JOIN LoadTest t ON i.id=t.ID
  WHERE i.lId=@idTestItem

 	-- Create Price History
	DECLARE @PriceYear INT 
	DECLARE @Amount numeric(32,2)
	DECLARE @OverrideAmount numeric(32,2)
	DECLARE @Next datetime 
	DECLARE @Chargeable SMALLINT 
	DECLARE @ThirdParty SMALLINT 
	DECLARE @ThirdPartyName VARCHAR(50) 
	DECLARE @ThirdPartyPhone VARCHAR(50) 

	SELECT @PriceYear=YEAR([Next]), @Amount=ISNULL(Amount,0),@OverrideAmount=ISNULL(OverrideAmount,0),@Next=[Next],@Chargeable=ISNULL(Chargeable,0)
	,@ThirdParty=ISNULL(ThirdParty,0), @ThirdPartyName=ISNULL(ThirdPartyName,''), @ThirdPartyPhone=ISNULL(ThirdPartyPhone,'')
	FROM LoadTestItem WHERE LID=@idTestItem 

	IF (SELECT COUNT(1) FROM LoadTestItemHistoryPrice WHERE LID=@idTestItem  AND PriceYear=@PriceYear)!=1
	BEGIN
		if @Amount IS NOT null
		begin
				INSERT INTO LoadTestItemHistoryPrice (LID,PriceYear,CreatedBy,CreatedDate,[DefaultAmount],[OverrideAmount],DueDate,Chargeable,ThirdPartyRequired,ThirdPartyName,ThirdPartyPhone)		
		VALUES (@idTestItem,@PriceYear,@UserName,GETDATE(),@Amount,@OverrideAmount,@Next,@Chargeable,@ThirdParty,@ThirdPartyName,@ThirdPartyPhone)	
		end

    END 

END


