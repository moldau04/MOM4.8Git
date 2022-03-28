CREATE PROCEDURE [dbo].[spUpdateTaskToClose]

	  @TaskID  INT,
	  @DueDate    DATETIME,
      @TimeDue    DATETIME,
	  @Username VARCHAR(50),
	  @desc VARCHAR(500)
AS
BEGIN
								INSERT INTO done
                                  (ID,
                                   Type,
                                   Rol,
                                   fDate,
                                   fTime,
                                   Datedone,
                                   Timedone,
                                   Subject,
                                   Remarks,
                                   Keyword,
                                   fUser,
                                   fBy,
                                   Duration,
                                   Contact,
                                   Source,
                                   Result,
                                   CreateDate,
                                   CreatedBy,
                                   LastUpdateDate,
                                   LastUpdatedBy)
                      VALUES      (@TaskID,
                                    0,
                                   (SELECT Rol
                                     FROM   todo
                                     WHERE  id = @TaskID),

                                    (SELECT fDate
                                     FROM   todo
                                     WHERE  id = @TaskID),

                                    (SELECT fTime
                                     FROM   todo
                                     WHERE  id = @TaskID),

                                    @DueDate,
                                    @TimeDue,
                                      (SELECT Subject
                                     FROM   todo
                                     WHERE  id = @TaskID),
                                    @desc,
                                    'To Do',
                                     (SELECT fUser
                                     FROM   todo
                                     WHERE  id = @TaskID),
                                    @Username,
                                    0.00,
                                  (SELECT Contact
                                     FROM   todo
                                     WHERE  id = @TaskID),
                                    '',
                                    '',
                                    (SELECT createdate
                                     FROM   todo
                                     WHERE  id = @TaskID),
                                    (SELECT createdby
                                     FROM   todo
                                     WHERE  id = @TaskID),
                                    Getdate(),
                                    @Username
                                     )

								DELETE FROM ToDo WHERE  ID = @TaskID
END
