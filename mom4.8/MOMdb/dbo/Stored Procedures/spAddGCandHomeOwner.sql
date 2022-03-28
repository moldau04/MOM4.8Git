create PROCEDURE [dbo].[spAddGCandHomeOwner] ( @tblGCandHomeOwner AS TBLGCANDHOMEOWNER1 READONLY,
                                              @GContractorID     INT output,
                                              @HomeOwnerID       INT output )
AS
  BEGIN
      set @GContractorID=0;
	  set @HomeOwnerID=0;

	  ------gc
      DECLARE @GC_RolName VARCHAR(75);
      DECLARE @GC_city VARCHAR(50);
      DECLARE @GC_state VARCHAR(2);
      DECLARE @GC_zip VARCHAR(10);
      DECLARE @GC_country VARCHAR(50);
      DECLARE @GC_phone VARCHAR(28);
      DECLARE @GC_cellular VARCHAR(28);
      DECLARE @GC_fax VARCHAR(28);
      DECLARE @GC_contact VARCHAR(50);
      DECLARE @GC_email VARCHAR(50);
      DECLARE @GC_rolRemarks VARCHAR(MAX);
	  DECLARE @GC_Address VARCHAR(MAX);
	  -------ho
	   DECLARE @HO_RolName VARCHAR(75);
      DECLARE @HO_city VARCHAR(50);
      DECLARE @HO_state VARCHAR(2);
      DECLARE @HO_zip VARCHAR(10);
      DECLARE @HO_country VARCHAR(50);
      DECLARE @HO_phone VARCHAR(28);
      DECLARE @HO_cellular VARCHAR(28);
      DECLARE @HO_fax VARCHAR(28);
      DECLARE @HO_contact VARCHAR(50);
      DECLARE @HO_email VARCHAR(50);
      DECLARE @HO_rolRemarks VARCHAR(MAX);
	  DECLARE @HO_Address VARCHAR(MAX);
      -------------------------------------Add new GC info ---------------------------------------------     

      IF EXISTS( SELECT 1
                 FROM   @tblGCandHomeOwner
                 WHERE  Type = 1 ) BEGIN 

            SELECT @GContractorID=id,
			       @GC_RolName = NAME,
                   @GC_city = City,
                   @GC_state = State,
                   @GC_zip = Zip,
                   @GC_phone = Phone,
                   @GC_fax = Fax,
                   @GC_contact = Contact,
                   @GC_email = EMail,
                   @GC_country = Country,
                   @GC_cellular = Cellular,
                   @GC_rolRemarks = Remarks,
				   @GC_Address = Address
            FROM   @tblGCandHomeOwner
            WHERE  Type = 1

            IF ( @GContractorID = 0 ) ----Add
            BEGIN
                  EXEC @GContractorID = Spaddroldetails
                    @GC_RolName,
                    @GC_city,
                    @GC_state,
                    @GC_zip,
                    @GC_phone,
                    @GC_fax,
                    @GC_contact,
                    @GC_Address,
                    @GC_email,
                    '',
                    @GC_country,
                    @GC_cellular,
                    @GC_rolRemarks,
                    0

                  INSERT INTO tblLocAddlContact
                                (RolID,
                               LocContactTypeID)
                  VALUES      (@GContractorID,
                               1)
              END
            ELSE------------------update
            BEGIN
                EXEC Spupdateroldetails
                  @GContractorID,
                  @GC_RolName,
                  @GC_city,
                  @GC_state,
                  @GC_zip,
                  @GC_phone,
                  @GC_fax,
                  @GC_contact,
                  @GC_Address,
                  @GC_email,
                  '',
                  @GC_country,
                  @GC_cellular,
                  0

				  IF NOT EXISTS(select 1 from tblLocAddlContact where  RolID=@GContractorID and LocContactTypeID=1)
				  INSERT INTO tblLocAddlContact
                              (RolID,
                               LocContactTypeID)
                  VALUES      (@GContractorID,
                               1)
            END
        END

      ------------------------------------------------------
   

      --- add new hOME contractor info
      IF EXISTS( SELECT 1
                 FROM   @tblGCandHomeOwner
                 WHERE  Type = 2 ) BEGIN
            

			  SELECT     @HomeOwnerID = ID,
			             @HO_RolName = NAME,
                         @HO_city = City,
                         @HO_state = State,
                         @HO_zip = Zip,
                         @HO_phone = Phone,
                         @HO_fax = Fax,
                         @HO_contact = Contact,
                         @HO_email = EMail,
                         @HO_country = Country,
                         @HO_cellular = Cellular,
                         @HO_rolRemarks = Remarks,
						 @HO_Address=Address
                  FROM   @tblGCandHomeOwner
                  WHERE  Type = 2

            IF ( @HomeOwnerID = 0 )
			 BEGIN
                

                  EXEC @HomeOwnerID = Spaddroldetails
                    @HO_RolName,
                    @HO_city,
                    @HO_state,
                    @HO_zip,
                    @HO_phone,
                    @HO_fax,
                    @HO_contact,
                    @HO_Address,
                    @HO_email,
                    '',
                    @HO_country,
                    @HO_cellular,
                    @HO_rolRemarks,
                    0

            INSERT INTO tblLocAddlContact
                              (RolID,
                               LocContactTypeID)
             VALUES      ( @HomeOwnerID,
                                2)
            END
            ELSE BEGIN
             

                EXEC Spupdateroldetails
                  @HomeOwnerID,
                  @HO_RolName,
                  @HO_city,
                  @HO_state,
                  @HO_zip,
                  @HO_phone,
                  @HO_fax,
                  @HO_contact,
                  @HO_Address,
                  @HO_email,
                  '',
                  @HO_country,
                  @HO_cellular,
                  0

				  if not exists(select 1 from tblLocAddlContact where  RolID=@HomeOwnerID and LocContactTypeID=2)
				  INSERT INTO tblLocAddlContact
                              (RolID,
                               LocContactTypeID)
                  VALUES      (@HomeOwnerID,
                               2)
            END
        END
  ------------------------------------------------------------------------------------------------------
  END 
