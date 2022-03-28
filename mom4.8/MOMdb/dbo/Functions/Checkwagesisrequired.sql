CREATE  FUNCTION [dbo].[Checkwagesisrequired] (@fWork nvarchar(50),@IsTicketADD_EDITSp INT)
RETURNS BIT
AS
  BEGIN
      -- Declare the return variable here
      DECLARE @ResultValue BIT=0;

      IF EXISTS(SELECT 1
                FROM   Control
                WHERE  JobCostLabor = 1)
        BEGIN
		   if(@IsTicketADD_EDITSp = 1)
		   BEGIN
            IF NOT EXISTS(SELECT *
                      FROM   PRWageItem
                      WHERE  emp = (select Top 1 ID from emp where CallSign=@fWork))
              BEGIN
                  SELECT @ResultValue = 1;
              END
	      END
		  ELSE 
		      BEGIN
                  SELECT @ResultValue = 1;
              END
        END

      RETURN @ResultValue
  END 
