CREATE PROCEDURE [dbo].[SpAddZone]
@Name	varchar(50),
@fDesc	varchar(75),
@Bonus	numeric(30, 2),
@Price1	numeric(30, 2),
@Count	int,
@Tax	tinyint,
@Remarks	varchar(8000)

As

declare @ZoneID int
declare @DucplicateZoneName int = 0

select @DucplicateZoneName = COUNT(1) from Zone  where Name =@Name 

		set @ZoneID=(SELECT ISNULL(MAX(ID),0)+1 AS ID FROM Zone)

if(@DucplicateZoneName=0)
begin

BEGIN TRANSACTION
  
Insert into Zone
(
ID,Name,fDesc,Bonus,Price1,Count,Tax,Remarks
)
values
(
@ZoneID,@Name,@fDesc,@Bonus,@Price1,@Count,@Tax,@Remarks
)


IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN  
	RAISERROR ('Error Occured', 16, 1)  
    ROLLBACK TRANSACTION    
    RETURN
 END

 COMMIT TRANSACTION
 
   end 
else
begin
 RAISERROR ('Zone Name already exists, please use different Zone Name !', 16, 1)  
 RETURN
end
 
 
 return (@ZoneID)
GO