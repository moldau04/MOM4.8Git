CREATE FUNCTION [dbo].[Weekends] (@Scheculedate DATETIME)
RETURNS BIT
AS
  BEGIN
      DECLARE @response BIT=0

      IF ( @Scheculedate IS NOT NULL )
        BEGIN
            IF ( Datepart(dw, @Scheculedate) = 7
                  OR Datepart(dw, @Scheculedate) = 1 )
              SET @response = 1
        END

      RETURN @response
  END 
