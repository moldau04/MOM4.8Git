CREATE PROCEDURE spAddBillCode
 @BillCode int
,@ContactName varchar(MAX)  
,@SalesDescription varchar(MAX)
,@CatStatus  int
,@Balance decimal(13,2)
,@Measure VARCHAR(50)
,@Type VARCHAR(50)
,@sAcct int
,@Remarks VARCHAR(MAX)
,@WarehouseID VARCHAR(50)

AS
BEGIN
IF ISNULL(@BillCode,0) = 0
BEGIN
	INSERT INTO Inv (Name, fDesc, status, Price1, Measure, tax, AllowZero, inuse, type, sacct,  Remarks, lastupdatedate, cat, warehouse) 
    VALUES (@ContactName,@SalesDescription,@CatStatus,@Balance,@Measure,0,0,0,@Type,@sAcct,@Remarks,GETDATE(),@CatStatus,@WarehouseID )
END
ELSE 
BEGIN
	UPDATE Inv  SET    Name = @ContactName, fDesc = @SalesDescription, 
    status = @CatStatus, cat = @CatStatus, Price1 = @Balance, Measure = @Measure, sacct = @sAcct, 
    Remarks = @Remarks, lastupdatedate = getdate(), warehouse = @WarehouseID  WHERE  ID = @BillCode
END

END