CREATE PROCEDURE [dbo].[spUpdateEquipmentClassification]
 @OldClassification varchar(50), 
 @Classification varchar(50), 
 @Status BIT
AS 
BEGIN
IF @OldClassification<>@Classification
BEGIN 
	IF (select count(1) from elev where Classification=@OldClassification) =0
	BEGIN
		if exists (select 1 from ElevatorSpec where edesc =@Classification and ecat = 3)
		begin
			RAISERROR ('Equipment Classification already exists, please use different equipment !',16,1)  RETURN 
		end
		else
		begin
			UPDATE [ElevatorSpec]
			SET [Status] = @Status,  EDesc=@Classification
			 WHERE  ecat=3 AND edesc=@OldClassification
		end
		
    END
    ELSE
    BEGIN
		RAISERROR ('Equipment Classification is used in Elev, it cannot be updated!',16,1)  RETURN 
    END 
END
ELSE
BEGIN
	
	UPDATE [ElevatorSpec]
	SET [Status] = @Status     
	 WHERE  ecat=3 AND edesc=@Classification
END 
END




