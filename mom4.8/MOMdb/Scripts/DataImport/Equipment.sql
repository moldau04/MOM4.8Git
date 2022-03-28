-- =============================================
-- Author		:	NK
-- Create date	:	10 DEC, 2019
-- Description	:	To insert Equipment
-- =============================================


--- Add Equipment  

 

BEGIN TRAN
-----------commit tran
-----------rollback tran 
----------Equip
DECLARE @EquipLoc INT;
DECLARE @EquipType VARCHAR(20);
DECLARE @EquipCat VARCHAR(20);
DECLARE @EquipManuf VARCHAR(20);
DECLARE @EquipSerial VARCHAR(50);
DECLARE @EquipState VARCHAR(25);
DECLARE @EquipSince DATETIME;
DECLARE @EquipLast DATETIME;
DECLARE @EquipPrice NUMERIC(30, 2);
DECLARE @EquipStatus SMALLINT;
DECLARE @EquipInstall DATETIME;
DECLARE @EquipCategory VARCHAR(50);
DECLARE @Equiptemplate INT;
DECLARE @EquipUpdatedBy VARCHAR(20);
DECLARE @EquipBuilding VARCHAR(20)
DECLARE @EquipfDesc VARCHAR(50);
DECLARE @Remarks nvarchar(MAX);
 
DECLARE @Installation# VARCHAR(50);
DECLARE  @Classification VARCHAR(50);
------------$$$$$$$$$$$---------------
DECLARE @Row INT =1;

DECLARE @RowCount INT =0;

SELECT @RowCount =Max(pk) FROM  dbo.[WestCoastEquipment$]

WHILE( @Row <= @RowCount)
 
  BEGIN
      

      ---------------Create New Equipment-------------
	  DECLARE @tempBuilding VARCHAR(4);

	  DECLARE @equipment_id NVARCHAR(100);

	  SELECT @EquipLoc = e.MOM_LocID,	  @equipment_id= e.[Equipment ID]	
	   FROM   dbo.[WestCoastEquipment$] e    WHERE  e.PK  = @Row
      
	  IF(isnull(@EquipLoc,'') !='')
	  BEGIN---------
      IF  EXISTS(SELECT 1  FROM   dbo.[WestCoastEquipment$] where PK=@Row  and  MOM_EquipID is null)
        BEGIN
            PRINT ( 'Not Exists Equpment' )

            SELECT @equipment_id = e.[Equipment ID],
                   @EquipType =isnull(e.type,'None'),
                   @EquipCategory =e.Category,
                   @EquipManuf =e.[Manufacturer ], 
				   @EquipSerial =NULL,
                   @EquipState = '',
                   @EquipCat = e.[Service Type],
                   @EquipInstall =e.[Installed Date], -- Aequip.installed_date,
                   @EquipfDesc = NULL, --Aequip.description,
				   @EquipSince='',
				   @Remarks=e.Remarks ,
				   @EquipLast= NULL ,
				   @EquipBuilding= e.Building	,
				   @EquipSerial=e.[Serial #]		  
				    
            FROM   dbo.[WestCoastEquipment$] e
            WHERE  PK = @Row  

           

            PRINT ( @Equiploc )

            PRINT( 'Begin EXECUTE [spAddEquipment]' )
			 


            EXECUTE [dbo].[Spaddequipment]
              @Loc =@EquipLoc,
              @Unit =@equipment_id,
              @fDesc =@EquipfDesc,
              @Type =@EquipType,
              @Cat =@EquipCat,
              @Manuf =@EquipManuf,
              @Serial =@EquipSerial,
              @State =@EquipState,
              @Since = @EquipSince,
              @Last =@EquipLast,
              @Price =0.00,
              @Status = 0,
              @Remarks =@Remarks,
              @Install =@EquipInstall,
              @Category =@EquipCategory,
              @template =NULL,
              @UpdatedBy = 'IMPORT FROM EXCELL',
              @EquipIDOut =0,
              @Building = @EquipBuilding,
			  @Classification=NULL,
			  @Shutdown=0,
			  @ShutdownReason=null,
			  @UserID=null,
			  @ShutdownLongDesc=null

              UPDATE e set e.MOM_EquipID=(SELECT MAX (ID)  FROM   Elev) 
			  FROM  dbo.[WestCoastEquipment$] e  WHERE  e.pk = @Row
                 


            PRINT( 'End EXECUTE [spAddEquipment]' )
        END
	  END-----------
	 	  PRINT(@row)

      SET @Row=@Row + 1
  END



--DELETE FROM @CustomItems 


 