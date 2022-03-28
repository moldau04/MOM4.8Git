CREATE PROCEDURE [dbo].[spGetDataOnInitialEstimate]
	@UType VARCHAR(50)='1',
	@IsSalesAsigned int = 0,
	@EstimateId INT = 0
AS
Declare @empQuery varchar(max) = ''

-- Table 0: Category
SELECT DISTINCT Category  FROM Estimate
-- Table 1: stage
SELECT ID, [Description] FROM Stage
-- Table 2: status
SELECT ID, [Name] FROM OEStatus
-- Table 3: 
EXEC spGetEstimateTax @UType
-- Table 4: employee
SET @empQuery = 'select t.Name,t.ID,t.SDesc from terr t where (select count(*) from emp where id = SMan)>0 '--'SELECT t.Name,t.ID,t.SDesc FROM terr t INNER JOIN emp ON emp.ID = t.SMan WHERE emp.status = 0 '
IF @EstimateId > 0
BEGIN
	SET @empQuery = @empQuery + ' or t.id in (  select isNull(EstimateUserId,0) from Estimate where id = ' + Convert(varchar(50), @EstimateId) + ' )'
END
IF @IsSalesAsigned > 0
BEGIN
	SET @empQuery = @empQuery + ' and t.Name =(SELECT fUser FROM tblUser WHERE id=' + Convert(varchar(50),@IsSalesAsigned) + ' )'
END

SET @empQuery = @empQuery + ' ORDER BY t.SDesc'
EXEC (@empQuery)
