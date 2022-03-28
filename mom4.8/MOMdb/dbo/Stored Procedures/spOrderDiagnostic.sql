CREATE	Procedure  [dbo].[spOrderDiagnostic]
	@Items as dbo.tblTypeDiagnostic readonly
AS
Begin
BEGIN TRY


DECLARE @Cate varchar(100)
DECLARE @desc varchar(200)
DECLARE @step INT
Declare @Type int
select top 1 @Cate=Category , @desc=fDesc , @step= OrderNo, @Type=Type FROM @Items 

DECLARE @maxOrder int
Set @maxOrder= (select max(isnull(OrderNo,0)) from Diagnostic where Category=@Cate)





DECLARE @currentOrder INT
SET @currentOrder = (SELECT TOP 1 isnull(OrderNo,0) FROM Diagnostic  where Category=@Cate and fDesc=@desc and Type=@type)


DECLARE @NewPosition INT
SET @NewPosition =  @currentOrder + @step*-1
print @NewPosition
if @NewPosition<=0
begin
set @NewPosition=1
end

if @NewPosition>@maxOrder
begin
set @NewPosition=@maxOrder
end


Update Diagnostic 
set OrderNo = @NewPosition
where 
Category=@Cate and fDesc =@desc  and Type=@Type


IF @step <0
	BEGIN

		Update Diagnostic
		SET OrderNo = isnull(OrderNo,0) -1
		WHERE isnull(OrderNo,0)>@currentOrder  AND OrderNo<@NewPosition
		and Category=@Cate
	

		Update Diagnostic
		set OrderNo = @NewPosition -1
		from Diagnostic
		Where OrderNo=@NewPosition
		AND  fDesc<>@desc and Category=@Cate

	END
ELSE
	BEGIN
		Update Diagnostic
		SET OrderNo = isnull(OrderNo,0) +1
		WHERE isnull(OrderNo,0) <@currentOrder and  isnull(OrderNo,0)>@NewPosition
		and Category=@Cate
	
		
		Update Diagnostic
		set OrderNo = @NewPosition +1
		from Diagnostic
		Where OrderNo=@NewPosition
		AND  fDesc<>@desc  and Category=@Cate
	END

END TRY
	BEGIN CATCH	
		SELECT ERROR_MESSAGE() AS ErrorMessage; 
	
		IF @@TRANCOUNT>0
		ROLLBACK	
		RAISERROR ('An error has occurred on this page.',16,1)
		RETURN
	
	END CATCH
ENd

