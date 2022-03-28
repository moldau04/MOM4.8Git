Create procedure spDeleteEquipmentTestPricingById
 @ID int
AS 
Begin
Delete from [EquipmentTestPricing] Where ID=@ID
End
