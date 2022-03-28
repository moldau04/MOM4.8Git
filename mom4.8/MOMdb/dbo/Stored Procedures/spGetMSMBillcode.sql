CREATE PROC [dbo].[spGetMSMBillcode]
@mode int
AS

DECLARE @DatabaseName varchar(50);
SELECT @DatabaseName = DB_NAME();

DECLARE @accountid varchar(100) 

SELECT @accountid = QBAccountID from chart where fdesc='Mobile Service Manager' and QBAccountID is not null
-- This custom for Accredited customer
-- We won't Export/Add Billcode-items to QB
IF(@DatabaseName != 'AHE-FL') --AND @DatabaseName != 'AHEI'
BEGIN
	--select @accountid
	IF(@mode=0)
	BEGIN
		SELECT ID, Name, fDesc, Part, Status, SAcct, Measure, Tax, Balance, Price1, Price2, Price3, Price4, Price5, Remarks, Cat, LVendor
			, LCost, AllowZero, Type, InUse, EN, Hand, Aisle, fOrder, Min, Shelf, Bin, Requ, Warehouse, Price6, Committed, QBInvID, LastUpdateDate
			, @accountid as qbaccountid 
		FROM inv
		WHERE QBinvID is null AND 1=0
		ORDER BY name
	END
	ELSE IF(@mode=1)
	BEGIN
		SELECT ID, Name, fDesc, Part, Status, SAcct, Measure, Tax, Balance, Price1, Price2, Price3, Price4, Price5, Remarks, Cat, LVendor
			, LCost, AllowZero, Type, InUse, EN, Hand, Aisle, fOrder, Min, Shelf, Bin, Requ, Warehouse, Price6, Committed, QBInvID, LastUpdateDate
			, @accountid as qbaccountid 
		FROM inv
		WHERE QBinvID is not null 
			AND LastUpdateDate >= (select QBLastSync from Control) 
			AND 1=0
		ORDER BY name
	END
END
ELSE
BEGIN
	--select @accountid
	IF(@mode=0)
	BEGIN
		SELECT ID, Name, fDesc, Part, Status, SAcct, Measure, Tax, Balance, Price1, Price2, Price3, Price4, Price5, Remarks, Cat, LVendor
			, LCost, AllowZero, Type, InUse, EN, Hand, Aisle, fOrder, Min, Shelf, Bin, Requ, Warehouse, Price6, Committed, QBInvID, LastUpdateDate
			, @accountid as qbaccountid 
		FROM inv
		WHERE QBinvID is null
		ORDER BY name
	END
	ELSE IF(@mode=1)
	BEGIN
		SELECT ID, Name, fDesc, Part, Status, SAcct, Measure, Tax, Balance, Price1, Price2, Price3, Price4, Price5, Remarks, Cat, LVendor
			, LCost, AllowZero, Type, InUse, EN, Hand, Aisle, fOrder, Min, Shelf, Bin, Requ, Warehouse, Price6, Committed, QBInvID, LastUpdateDate
			, @accountid as qbaccountid 
		FROM inv
		WHERE QBinvID is not null 
			AND LastUpdateDate >= (select QBLastSync from Control) 
		ORDER BY name
	END
END