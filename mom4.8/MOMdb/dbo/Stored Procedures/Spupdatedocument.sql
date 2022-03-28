CREATE PROCEDURE [dbo].[Spupdatedocument] @screen   VARCHAR(20),
                                          @TicketID INT,
                                          @TempID   VARCHAR(150)
AS
    UPDATE Documents
    SET    Screen = @screen,
           ScreenID = @TicketID,
           tempid = '0'
    WHERE  tempid = @TempID
