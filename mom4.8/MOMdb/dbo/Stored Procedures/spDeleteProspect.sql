CREATE procedure [dbo].[spDeleteProspect]
@ProspectID int

as

if not exists (select 1 from TicketO where LID = @ProspectID and Owner is null 
union select 1 from Lead l inner join Prospect p on p.Rol=l.Rol where p.ID=@ProspectID
union select 1 from ToDo t inner join Prospect p on p.Rol=t.Rol where p.ID=@ProspectID
union select 1 from Done d inner join Prospect p on p.Rol=d.Rol where p.ID=@ProspectID
union select 1 from Estimate e inner join Prospect p on p.Rol=e.RolID where p.ID=@ProspectID
)
begin
BEGIN TRANSACTION

update ptype set [count]= [COUNT] - 1 where [type] = (select [Type] from Prospect where ID=@ProspectID )

IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN
	RAISERROR ('Error Occured', 16, 1)
    ROLLBACK TRANSACTION
    RETURN
 END

delete from Rol where ID = (select Rol from Prospect where ID=@ProspectID)
IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN
	RAISERROR ('Error Occured', 16, 1)
    ROLLBACK TRANSACTION
    RETURN
 END

delete from Prospect where ID=@ProspectID
IF @@ERROR <> 0 AND @@TRANCOUNT > 0
 BEGIN
	RAISERROR ('Error Occured', 16, 1)
    ROLLBACK TRANSACTION
    RETURN
 END


COMMIT TRANSACTION

end else
begin
RAISERROR ('Selected Lead is in use. Lead can not be deleted.', 16, 1)
RETURN
end
