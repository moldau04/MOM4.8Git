GO
/****** Object:  StoredProcedure [dbo].[spUpdateCompany]    Script Date: 4/4/2018 8:54:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[spUpdateCompany] @CompanyName   VARCHAR(75),
                                        @Address       VARCHAR(255),
                                        @City          VARCHAR(50),
                                        @State         VARCHAR(2),
                                        @Zip           VARCHAR(10),
                                        @Tel           VARCHAR(20),
                                        @fax           VARCHAR(20),
                                        @Email         VARCHAR(50),
                                        @Web           VARCHAR(50),
                                        @Contact       VARCHAR(50),
                                        @Remarks       VARCHAR(200),
                                        @logo          IMAGE,
                                        @custRegister  SMALLINT,
                                        @QBPath        VARCHAR(500),
                                        @MultiLang     SMALLINT,
                                        @Qbintegration SMALLINT,
                                        @EmailMS smallint,
                                        @QBFirstSync smallint,
                                        @ServiceItem varchar(100),
                                        @ServiceItemLabor varchar(100),
                                        @ServiceItemExp varchar(100),
										@YE int,
										@GSTreg varchar(20),
										@Lat varchar(50),
										@Lng varchar(50)
                                        --,
                                        --@TransferTimesheet smallint,
                                        --@TransferInvoice smallint
                                        --,
                                        --@MerchantID    VARCHAR(100),
                                        --@LoginID       VARCHAR(100),
                                        --@Username      VARCHAR(20),
                                        --@Password      VARCHAR(100)
AS
    BEGIN TRANSACTION

    UPDATE Control
    SET    Name = @CompanyName,
           Address = @Address,
           City = @City,
           State = @State,
           Zip = @Zip,
           Phone = @Tel,
           Fax = @fax,
           Email = @Email,
           WebAddress = @Web,
           Contact = @Contact,
           Remarks = @Remarks,
           Logo = @logo,
           Custweb = @custRegister,
           QBPath = @QBPath,
           MultiLang = @MultiLang,
           QBIntegration = @Qbintegration,
           MSEmail=@EmailMS,
           QBFirstSync=@QBFirstSync,
           QBServiceItem=@ServiceItem,
           QBServiceItemLabor=@ServiceItemLabor,
           QBServiceItemExp=@ServiceItemExp,
		   YE=@YE,
		   GSTreg = @GSTreg,
		   Lat = @Lat,
		   Lng = @Lng
           --,
           --SyncInvoice=@TransferInvoice,
           --SyncTimesheet=@TransferTimesheet

    IF @@ERROR <> 0
       AND @@TRANCOUNT > 0
      BEGIN
          RAISERROR ('Error Occured',16,1)

          ROLLBACK TRANSACTION

          RETURN
      END

    --IF NOT EXISTS (SELECT 1
    --               FROM   tblGatewayInfo)
    --  BEGIN
    --      INSERT INTO tblGatewayInfo
    --                  (MerchantId,
    --                   LoginId,
    --                   Username,
    --                   Password)
    --      VALUES      ( @MerchantID,
    --                    @LoginID,
    --                    @Username,
    --                    @Password )
    --  END
    --ELSE
    --  BEGIN
    --      UPDATE tblGatewayInfo
    --      SET    MerchantId = @MerchantID,
    --             LoginId = @LoginID,
    --             Username = @Username,
    --             Password = @Password
    --  END
      
    --  IF @@ERROR <> 0
    --   AND @@TRANCOUNT > 0
    --  BEGIN
    --      RAISERROR ('Error Occured',16,1)

    --      ROLLBACK TRANSACTION

    --      RETURN
    --  END

    COMMIT TRANSACTION

