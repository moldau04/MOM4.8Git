CREATE PROCEDURE [dbo].[spAddEquipmentClassification]
 @Classification varchar(50), 
 @Status BIT
AS 
Begin
 if not exists(select 1 from ElevatorSpec where edesc =@Classification and ecat = 3) 
 BEGIN 
 INSERT into ElevatorSpec (ecat, edesc, status) 
 VALUES (3,@Classification,@Status) 
 END 
 ELSE 
 BEGIN 
	RAISERROR ('Equipment Classification already exists, please use different equipment !',16,1)  RETURN 
 END 

End

