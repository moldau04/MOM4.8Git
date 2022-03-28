CREATE PROCEDURE [dbo].[spRouteTemplate] @Name       VARCHAR(50),
                                        @sequence   VARCHAR(100),
                                        @Remarks    VARCHAR(250),
                                        @Mode       INT,
                                        @TemplateID INT,
                                        --@TemplateData As [dbo].[tblTypeTemplateDetails] Readonly,
                                        @worker int,
                                        @Center varchar(250),
                                        @Radius varchar(50),
                                        @Overlay varchar(15),
                                        @PolygonCoord varchar(max)
                                        

AS
    IF( @Mode = 0 )
      BEGIN
          IF NOT EXISTS (SELECT 1
                         FROM   tblroutetemplate
                         WHERE  name = @Name)
            BEGIN
                INSERT INTO tblroutetemplate
                            (name,
                             sequence,
                             remarks,
                             Center,
                             Radius,
                             worker,
                             overlay,
                             polygoncoord)
                VALUES      ( @Name,
                              @sequence,
                              @Remarks,
                              @Center,
                              @Radius,
                              @worker,
                              @Overlay,
                              @PolygonCoord )

                SET @TemplateID=Scope_identity()

                --INSERT INTO tblTemplateDetails 
                --(TemplateID,Loc,Worker )                      
                --(select @TemplateID,Loc,worker from @TemplateData)
            END
          ELSE
            BEGIN
                RAISERROR ('Template with same name already exists.',16,1)

                RETURN
            END
      END
    ELSE IF( @Mode = 1 )
      BEGIN
          IF NOT EXISTS (SELECT 1
                         FROM   tblroutetemplate
                         WHERE  name = @Name
                                AND templateid <> @TemplateID)
            BEGIN
                UPDATE tblroutetemplate
                SET    name = @Name,
                       sequence = @sequence,
                       remarks = @Remarks,
                       Center=@Center,
                       Radius=@Radius,
                       worker=@worker,
                       overlay=@Overlay,
                       Polygoncoord=@PolygonCoord
                WHERE  TemplateID = @TemplateID

              --  DELETE FROM tblTemplateDetails
              --  WHERE  TemplateID = @TemplateID

              --INSERT INTO tblTemplateDetails 
              --  (TemplateID,Loc,Worker )                      
              --  (select @TemplateID,Loc,worker from @TemplateData)
            END
          ELSE
            BEGIN
                RAISERROR ('Template with same name already exists.',16,1)

                RETURN
            END
      END
    ELSE IF( @Mode = 2 )
      BEGIN
          DELETE FROM tblRouteTemplate
          WHERE  TemplateID = @TemplateID

          --DELETE FROM tblTemplateDetails
          --WHERE  TemplateID = @TemplateID
      END

    SELECT @TemplateID