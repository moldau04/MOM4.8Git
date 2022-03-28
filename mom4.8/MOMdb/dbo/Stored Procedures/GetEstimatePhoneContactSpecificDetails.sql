/*--------------------------------------------------------------------
Modified By: Thurstan
Modified On: 13 Dec 2018	
Description: Change select statement from Phone to Estimate table
--------------------------------------------------------------------*/
CREATE PROCEDURE [dbo].[GetEstimatePhoneContactSpecificDetails]
	@ID INT,
	@estimateId int
AS
BEGIN
	DECLARE @ROL_ID AS INTEGER
	DECLARE @LOC_ID AS INTEGER
	SET @ROL_ID=(SELECT TOP 1 Rol FROM Phone where ID=@ID)
	SET @LOC_ID=(SELECT TOP 1 LOC FROM Loc where Rol=@ROL_ID)

	SELECT @LOC_ID AS LOCID, Phone,EstimateEmail,Fax,EstimateCell FROM Estimate WHERE ID=@estimateId 
END
