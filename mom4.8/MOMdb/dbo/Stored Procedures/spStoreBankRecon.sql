CREATE PROCEDURE [dbo].[spStoreBankRecon]
	@bank int=null,
	@endbalance numeric(30,2)=null,
	@ReconDate datetime=null,
	@ServiceChrg numeric(30,2)=null,
	@ServiceAcct int=null,
	@ServiceDate datetime=null,
	@InterestChrg numeric(30,2)=null,
	@InterestAcct int=null,
	@InterestDate datetime=null,
	@BankRecon As [dbo].[tblTypeBankRecon] Readonly

AS
BEGIN
	
	SET NOCOUNT ON;

	UPDATE Trans
	SET 
		Status = 'T'
	WHERE ID IN (SELECT ID FROM @BankRecon WHERE Selected = 'true')
	
	UPDATE Trans
	SET
		Status = ''
	WHERE ID IN (SELECT ID FROM @BankRecon WHERE Selected = 'false')

	--UPDATE Trans
	--SET 
	--	Status = 'T'
	--WHERE Batch IN (SELECT Batch FROM @BankRecon WHERE Selected = 'true') AND Type = 30 AND AcctSub = @Bank
	
	--UPDATE Trans
	--SET
	--	Status = ''
	--WHERE Batch IN (SELECT Batch FROM @BankRecon WHERE Selected = 'false') AND Type = 30 AND AcctSub = @Bank


	UPDATE Control
	SET
		Bank = @Bank,
		SCDate = @ServiceDate,
		IntDate = @InterestDate,
		SCAmount = @ServiceChrg,
		IntAmount = @InterestChrg,
		EndBalance = @endbalance,
		StatementDate = @ReconDate

	UPDATE Custom SET Label = @InterestAcct WHERE Name = 'ReconInt'
	UPDATE Custom SET Label = @ServiceAcct WHERE Name = 'ReconSC'

END