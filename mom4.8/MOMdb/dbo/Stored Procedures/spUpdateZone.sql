CREATE PROCEDURE [dbo].[spUpdateZone]
@ID	int,
@Name	varchar(50),
@fDesc	varchar(75),
@Bonus	numeric(30, 2),
@Price1	numeric(30, 2),
@Count	int,
@Tax	tinyint,
@Remarks	varchar(8000)

As

BEGIN TRANSACTION
  
UPDATE Zone
SET

fDesc			=	@fDesc,
Bonus			=	@Bonus,
Price1			=	@Price1,
Count			=	@Count,
Tax				=	@Tax,
Remarks			=	@Remarks

Where ID = @ID

IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END

 COMMIT TRANSACTION
 
 return (@ID)
GO