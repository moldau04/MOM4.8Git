-- [Spreadprojectattachment] 'Project',136,2
CREATE PROCEDURE [dbo].[spReadProjectAttachment] @Type  NVARCHAR(100),
                                                @JobId INT,
                                                @Sort  INT,
												@IsSalesAsigned int =0
AS
 DECLARE @SalesAsignedTerrID int = 0
if( @IsSalesAsigned > 0)
BEGIN
SELECT @SalesAsignedTerrID=isnull(id,0) FROM  Terr WHERE Name=(SELECT fUser FROM  tblUser WHERE id=@IsSalesAsigned)
END
  BEGIN
      CREATE TABLE #tempDoc
        (
           [ID]       [INT],
           [Screen]   [VARCHAR](20) NULL,
           [ScreenID] [INT] NULL,
           [Line]     [SMALLINT] NULL,
           [fDesc]    [VARCHAR](255) NULL,
           [Filename] [VARCHAR](1000) NULL,
           [Path]     [VARCHAR](255) NULL,
           [Type]     [SMALLINT] NULL,
           [Remarks]  [VARCHAR](8000) NULL,
           ext        VARCHAR(10),
           msvisible  BIT
        )

      IF EXISTS(SELECT 1
                FROM   Job
                WHERE  Job.ID = @JobId)
        BEGIN
            IF( @Type = 'Tickets' )
              BEGIN
                  INSERT INTO #tempDoc
                  SELECT d.ID,
                         d.Screen,
                         d.ScreenID,
                         d.Line,
                         d.fDesc,
                         d.Filename,
                         d.Path,
                         d.Type,
                         d.Remarks,
                         Reverse(LEFT(Reverse(Filename), Charindex('.', Reverse(Filename)) - 1)) AS ext,
                         Isnull(MSVisible, 0)                                                    AS MSVisible
                  FROM   TicketD
                         INNER JOIN Documents d
                                 ON TicketD.ID = d.ScreenID
                                    AND d.Screen = 'Ticket'
                  WHERE  TicketD.Job = @JobId
                         AND Isnull(Filename, '') <> ''
                  UNION
                  SELECT d.ID,
                         d.Screen,
                         d.ScreenID,
                         d.Line,
                         d.fDesc,
                         d.Filename,
                         d.Path,
                         d.Type,
                         d.Remarks,
                         Reverse(LEFT(Reverse(Filename), Charindex('.', Reverse(Filename)) - 1)) AS ext,
                         Isnull(MSVisible, 0)                                                    MSVisible
                  FROM   TicketO
                         INNER JOIN Documents d
                                 ON TicketO.ID = d.ScreenID
                                    AND d.Screen = 'Ticket'
                  WHERE  TicketO.Job = @JobId
                         AND Isnull(Filename, '') <> ''
                  ORDER  BY ext
              END

            IF( @Type = 'Customer' )
              BEGIN
                  INSERT INTO #tempDoc
                  SELECT d.ID,
                         d.Screen,
                         d.ScreenID,
                         d.Line,
                         d.fDesc,
                         d.Filename,
                         d.Path,
                         d.Type,
                         d.Remarks,
                         Reverse(LEFT(Reverse(Filename), Charindex('.', Reverse(Filename)) - 1)) AS ext,
                         Isnull(MSVisible, 0)                                                    MSVisible
                  FROM   Owner
                         INNER JOIN Documents d
                                 ON Owner.ID = d.ScreenID
                                    AND d.Screen = 'Customer'
                         INNER JOIN Job j
                                 ON j.Owner = Owner.ID
                  WHERE  j.ID = @JobId
                         AND Isnull(Filename, '') <> ''
                  ORDER  BY ext
              END

            IF( @Type = 'Location' )
              BEGIN
                  INSERT INTO #tempDoc
                  SELECT d.ID,
                         d.Screen,
                         d.ScreenID,
                         d.Line,
                         d.fDesc,
                         d.Filename,
                         d.Path,
                         d.Type,
                         d.Remarks,
                         Reverse(LEFT(Reverse(Filename), Charindex('.', Reverse(Filename)) - 1)) AS ext,
                         Isnull(MSVisible, 0)                                                    MSVisible
                  FROM   Loc l
                         INNER JOIN Documents d
                                 ON l.Loc = d.ScreenID
                         INNER JOIN Job j
                                 ON j.Loc = l.Loc
                                    AND d.Screen = 'Location'
                  WHERE  j.ID = @JobId
                         AND Isnull(Filename, '') <> ''
						 and isnull(l.Terr,0) = (case  when(@IsSalesAsigned > 0 and @SalesAsignedTerrID > 0) then convert(nvarchar(10),@SalesAsignedTerrID) else isnull(l.Terr,0)  end )
                  ORDER  BY ext
              END

            IF( @Type = 'Project' )
              BEGIN
                  INSERT INTO #tempDoc
                  SELECT d.ID,
                         d.Screen,
                         d.ScreenID,
                         d.Line,
                         d.fDesc,
                         d.Filename,
                         d.Path,
                         d.Type,
                         d.Remarks,
                         Reverse(LEFT(Reverse(Filename), Charindex('.', Reverse(Filename)) - 1)) AS ext,
                         Isnull(MSVisible, 0)                                                    MSVisible
                  FROM    Documents d
                  WHERE  d.ScreenID = @JobId AND d.Screen = 'Project'  AND Isnull(Filename, '') <> ''
                  ORDER  BY ext
              END
        END

      DECLARE @query VARCHAR(max)

      SET @query = ' select * from #tempDoc '

      IF( @Sort = 1 )
        BEGIN
            SET @query += ' order by filename '
        END
      ELSE IF( @Sort = 2 )
        BEGIN
            SET @query += ' order by ext '
        END

      EXEC (@query)

      DROP TABLE #tempDoc
  END
GO

