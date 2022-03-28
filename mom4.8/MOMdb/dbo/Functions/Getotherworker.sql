CREATE FUNCTION [dbo].[Getotherworker]
 (
 @fWork INT,
  @WorkOrder varchar(100),
   @Loc INT,
    @Job INT,
 @EDate datetime
 )
RETURNS NVARCHAR(1000)
AS
  BEGIN
      -- Declare the return variable here
      DECLARE @ResultValue NVARCHAR(1000)
      DECLARE @temp TABLE
        (
           fWork INT
        );

      ----Ticket D
      INSERT INTO @temp
       SELECT DISTINCT t1.fWork
               FROM   TicketD t1
               WHERE 
             --t1.WorkOrder = @WorkOrder
             --         AND 
       t1.loc = @Loc
                      AND 
       isnull(job,0) =isnull( @Job,0)
                      AND Cast(EDate AS DATE) = Cast(@EDate AS DATE)
                      AND fWork != @fWork 
       and isnull(job,0) <> 0
      

      --------Ticket O
      UNION
      SELECT DISTINCT t1.fWork
               FROM   TicketO t1
       WHERE 
                --t1.WorkOrder = @WorkOrder
                --      AND
        t1.LID = @Loc
                      AND 
      isnull(job,0) =isnull( @Job,0)
                      AND Cast(EDate AS DATE) = Cast(@EDate AS DATE)
                      AND fWork != @fWork 
       and isnull(job,0) <> 0

      SELECT @ResultValue = (SELECT ( Stuff((SELECT ', ' + Cast((fDesc) AS VARCHAR(1000)) [text()]
                                             FROM   tblWork
                                             WHERE  id IN (SELECT DISTINCT t1.fWork
                                                           FROM   @temp t1)
                                             FOR XML PATH(''), TYPE) .value('.', 'NVARCHAR(MAX)'), 1, 2, ' ') ) AS OtherWorker)

      RETURN @ResultValue
  END