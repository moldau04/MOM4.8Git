--SELECT [dbo].[Contract_FrequencyDays](0)
CREATE FUNCTION [dbo].[Contract_FrequencyDays] (@BillingFrequency as SMALLINT)
RETURNS INT
AS
BEGIN
	DECLARE @FreqDays INT;
	IF(@BillingFrequency = 0)
		SET @FreqDays = 30
	ELSE IF(@BillingFrequency = 1)
		SET @FreqDays = 15
	ELSE IF(@BillingFrequency = 2)
		SET @FreqDays = 91
	ELSE IF(@BillingFrequency = 3)
		SET @FreqDays = 121
	ELSE IF(@BillingFrequency = 4)
		SET @FreqDays = 182
	ELSE IF(@BillingFrequency = 5)
		SET @FreqDays = 365
	ELSE IF(@BillingFrequency = 7)
		SET @FreqDays = 1095
	ELSE IF(@BillingFrequency = 8)
		SET @FreqDays = 1825
	ELSE IF(@BillingFrequency = 9)
		SET @FreqDays = 730

	RETURN @FreqDays
END