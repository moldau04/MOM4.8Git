CREATE FUNCTION [dbo].[Afterhours] (@enroute DATETIME,
                                    @comp    DATETIME)
RETURNS BIT
AS
  BEGIN
      DECLARE @StartDate DATETIME,
              @EndDate   DATETIME,
              @response  BIT=0

      SELECT @StartDate = BusinessStart,
             @EndDate = BusinessEnd
      FROM   control

      IF ( @enroute IS NOT NULL
           AND @StartDate IS NOT NULL )
        BEGIN
            IF ( convert(time,@enroute )<convert(time, @StartDate ))
              SET @response = 1
        END

      IF ( @enroute IS NOT NULL
           AND @EndDate IS NOT NULL )
        BEGIN
            IF ( convert(time, @enroute )>convert(time, @EndDate ))
              SET @response = 1
        END

      IF ( @comp IS NOT NULL
           AND @EndDate IS NOT NULL )
        BEGIN
            IF (convert(time, @comp )>convert(time, @EndDate ))
              SET @response = 1
        END

      IF ( @comp IS NOT NULL
           AND @StartDate IS NOT NULL )
        BEGIN
            IF (convert(time, @comp )<convert(time, @StartDate ))
              SET @response = 1
        END

      RETURN @response
  END 
