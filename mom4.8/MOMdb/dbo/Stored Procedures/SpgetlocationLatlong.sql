CREATE PROCEDURE [dbo].[SpgetlocationLatlong] @tech VARCHAR(100),
                                       @date DATETIME,
                                       @Time DATETIME,
									   @TicketID int,
									   @timestamp VARCHAR(10)
AS

     if exists (select 1 from TicketLocationData where ticket_id=@ticketID and timeStampType in (@timestamp))
	 begin
	 select lat as latitude,lng as longitude from TicketLocationData where ticket_id=@ticketID and timeStampType in (@timestamp)
	 end
	 else
	 begin
    SELECT TOP 1 m.latitude,m.longitude
    FROM   [MSM2_Admin].dbo.mapdata m
    WHERE  m.DATE = (SELECT TOP 1 date
                     FROM   [MSM2_Admin].dbo.mapdata
                     WHERE  deviceId = (SELECT DeviceID
                                        FROM   Emp
                                        WHERE  CallSign = @tech)
                            AND @Time IS NOT NULL
                            AND date BETWEEN Dateadd(MINUTE, -15, Cast(Cast(@date AS DATE) AS DATETIME)  +cast( Cast(@Time AS TIME)as datetime) ) AND Dateadd(MINUTE, 15, Cast(Cast(@date AS DATE) AS DATETIME)
                                                                                                                 +cast( Cast(@Time AS TIME)as datetime))
                     ORDER  BY Abs(Datediff(MI, date, Cast(Cast(@date AS DATE) AS DATETIME)
                                                      +cast( Cast(@Time AS TIME) as datetime))) ASC)
           AND m.deviceid = (SELECT DeviceID
                             FROM   emp
                             WHERE  callsign = @tech)
    ORDER  BY m.DATE DESC

	end
GO

