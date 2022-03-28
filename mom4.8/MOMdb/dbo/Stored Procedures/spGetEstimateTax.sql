CREATE PROCEDURE [dbo].[spGetEstimateTax]
	@UType VARCHAR(50)
AS
BEGIN
	DECLARE @Country AS VARCHAR(10)
	DECLARE @GST_RATE AS DECIMAL
	SET @Country=(SELECT Label FROM Custom WHERE NAME='Country')
	IF @Country='1'
		BEGIN
			SET @GST_RATE=(SELECT  CAST(Label as decimal) FROM Custom WHERE NAME='GSTRate')

			SELECT 
				Name +' / ' + CONVERT(VARCHAR(50),Rate+@GST_RATE) AS NameRate
				, Name
				,fDesc
				,Rate+@GST_RATE AS Rate
				,Remarks
				,Count
				,GL
				,Type
				,UType
				,PSTReg
				,QBStaxID
				,LastUpdateDate
				,IsTaxable 
			FROM STax WHERE UType<>@UType

		END
	ELSE
		BEGIN
			SELECT 
				Name +' / ' + CONVERT(VARCHAR(50),Rate) AS NameRate
				, Name
				,fDesc
				,Rate
				,Remarks
				,Count
				,GL
				,Type
				,UType
				,PSTReg
				,QBStaxID
				,LastUpdateDate
				,IsTaxable 
			FROM STax WHERE UType<>@UType
		END
END
GO