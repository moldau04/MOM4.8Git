CREATE FUNCTION [dbo].[TicketStatusAsText]
(
  @fStatus INT
) RETURNS VARCHAR(10) AS BEGIN

DECLARE @sStatus VARCHAR(10)

SELECT @sStatus=CASE @fStatus
                   WHEN 0 THEN 'Open'
                   WHEN 1 THEN 'Assigned'
                   WHEN 2 THEN 'En Route'
                   WHEN 3 THEN 'On Site'
                   WHEN 4 THEN 'Completed'
                   WHEN 5 THEN 'On Hold'
                END
RETURN @sStatus
END

GO