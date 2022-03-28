CREATE  PROCEDURE [dbo].[spgetcontactforjob] @Job            INT,
                                           @IsSalesAsigned INT =0
AS
    DECLARE @SalesAsignedTerrID INT = 0

    IF( @IsSalesAsigned > 0 )--If User is  Salesperson
      BEGIN
          SELECT @SalesAsignedTerrID = Isnull(id, 0)
          FROM   Terr
          WHERE  NAME = (SELECT fUser
                         FROM   tblUser
                         WHERE  id = @IsSalesAsigned)
      END

    SELECT *
    FROM   (SELECT @Job                            AS JobID,
                   Phone.ID                        AS PhoneID,
                   Phone.fDesc                     AS NAME,
                   Phone.Phone,
                   Phone.Cell,
                   Phone.Fax,
                   Phone.Title,
                   Phone.Email,
                   CASE
                     WHEN ( (SELECT Count(id)
                             FROM   tblJoinPhoneJob
                             WHERE  JobID = @Job
                                    AND PhoneID = Phone.ID) > 0 ) THEN 1
                     ELSE 0
                   END                             AS IsHighLighted,
                   CASE
                     WHEN ( (SELECT Count(id)
                             FROM   Owner
                             WHERE  Rol = Phone.Rol) > 0 ) THEN 'Customer'
                     ELSE 'Location'
                   END                             AS ContactType,
                   Isnull(Phone.EmailRecTicket, 0) AS EmailRecTicket
            FROM   (-------------------------1
                   SELECT *
                    FROM   Phone
                    WHERE  Rol IN (-----2
                                  SELECT l.rol
                                  FROM   loc l
                                  WHERE  l.loc = (SELECT loc
                                                  FROM   Job
                                                  WHERE  id = @Job)
                                         --If User is  Salesperson
                                         AND ( Isnull(l.Terr, 0) = ( CASE
                                                                       WHEN( @IsSalesAsigned > 0
                                                                             AND @SalesAsignedTerrID > 0 ) THEN CONVERT(NVARCHAR(10), @SalesAsignedTerrID)
                                                                       ELSE Isnull(l.Terr, 0)
                                                                     END )
                                                OR
                                               --Or If User is Second Salesperson
                                               Isnull(l.Terr2, 0) = ( CASE
                                                                        WHEN( @IsSalesAsigned > 0
                                                                              AND @SalesAsignedTerrID > 0 ) THEN CONVERT(NVARCHAR(10), @SalesAsignedTerrID)
                                                                        ELSE Isnull(l.Terr2, 0)
                                                                      END ) )
                                   UNION
                                   SELECT rol
                                   FROM   Owner
                                   WHERE  ID = (SELECT j.owner
                                                FROM   Job j
                                                       INNER JOIN loc l
                                                               ON l.loc = j.Loc
                                                WHERE  j.id = @Job
                                                       AND (
                                                           --If User is  Salesperson
                                                           Isnull(l.Terr, 0) = ( CASE
                                                                                   WHEN( @IsSalesAsigned > 0
                                                                                         AND @SalesAsignedTerrID > 0 ) THEN CONVERT(NVARCHAR(10), @SalesAsignedTerrID)
                                                                                   ELSE Isnull(l.Terr, 0)
                                                                                 END )
                                                            OR
                                                           --If User is secon Salesperson
                                                           Isnull(l.Terr2, 0) = ( CASE
                                                                                    WHEN( @IsSalesAsigned > 0
                                                                                          AND @SalesAsignedTerrID > 0 ) THEN CONVERT(NVARCHAR(10), @SalesAsignedTerrID)
                                                                                    ELSE Isnull(l.Terr2, 0)
                                                                                  END ) )))-----2
                   ) AS Phone ---------------1
           )FinalSelectwithContectType
    WHERE  FinalSelectwithContectType.ContactType = CASE (SELECT Isnull(ContactType, 0)
                                                          FROM   control)
                                                      WHEN 1 THEN 'Customer'
                                                      WHEN 2 THEN 'Location'
                                                      ELSE FinalSelectwithContectType.ContactType
                                                    END 


