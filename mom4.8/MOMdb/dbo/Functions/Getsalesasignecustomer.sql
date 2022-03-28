CREATE FUNCTION [dbo].[Getsalesasignecustomer] (@SalesAsignedTerrID INT =0)
RETURNS VARCHAR(max)
AS
  BEGIN
      DECLARE @StrCustId VARCHAR(MAX)
      DECLARE @tblcust TABLE
        (
           CustID INT
        )

      INSERT INTO @tblcust
      SELECT DISTINCT Owner AS ID
      FROM   loc l
      WHERE  (
	   l.Terr = @SalesAsignedTerrID------  Default Salesperson
       OR 
	   Isnull(l.Terr2, 0) = @SalesAsignedTerrID---- Second Salesperson
			)

      SELECT @StrCustId = COALESCE(@StrCustId+',', '')
                          + Cast(CustID AS NVARCHAR(50))
      FROM   @tblcust

      RETURN @StrCustId
  END 
