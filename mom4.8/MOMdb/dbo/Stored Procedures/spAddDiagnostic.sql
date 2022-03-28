CREATE PROC [dbo].[spAddDiagnostic] @fdesc    VARCHAR(255),  
                                   @category VARCHAR(15),  
                                   @type     SMALLINT,  
                                   @mode     SMALLINT ,
                                   @oldcategory VARCHAR(15),  
                                   @oldtype     SMALLINT
AS  
DECLARE @maxOrder INT
SET @maxOrder= (SELECT MAX(ISNULL(OrderNo,0)) + 1 FROM Diagnostic where Category=@category)
    IF( @mode = 0 )  
      BEGIN  
          IF NOT EXISTS(SELECT 1  
                        FROM   Diagnostic  
                        WHERE  fDesc = @fdesc  
                               AND Type = @type  
                               AND Category = @category)  
            BEGIN  
                INSERT INTO Diagnostic  
                            (category,  
                             type,  
                             fdesc,OrderNo)  
                VALUES     (@category,  
                            @type,  
                            @fdesc,@maxOrder)  
            END  
          ELSE  
            BEGIN  
                RAISERROR ('Call Code already exists for selected type !',16,1)  
  
                RETURN  
            END  
      END  
    ELSE IF( @mode = 1 )  
      BEGIN  
          IF NOT EXISTS(SELECT 1  
                        FROM   Diagnostic  
                        WHERE  fDesc = @fdesc  
                               AND Type = @type  
                               AND Category = @category)  
            BEGIN  
                UPDATE Diagnostic  
                SET    category = @category,  
						Type = @type,
                        OrderNo = @maxOrder
                WHERE  fdesc = @fdesc  
                               AND Type = @oldtype  
                               AND Category = @oldcategory
                       
            END  
          ELSE
            BEGIN  
                RAISERROR ('Call Code already exists for selected type !',16,1)  
  
                RETURN  
            END  
      END   
