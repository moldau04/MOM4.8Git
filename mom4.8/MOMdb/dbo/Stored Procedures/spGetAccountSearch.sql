create PROCEDURE [dbo].[spGetAccountSearch]
	
	@SearchText varchar(150)

AS
declare @WOspacialchars varchar(50) 
declare @text nvarchar(max)
set @WOspacialchars = dbo.RemoveSpecialChars(@SearchText)
BEGIN
	
	SET NOCOUNT ON;

	--set @text = 'SELECT ID AS value , Acct AS acct, fDesc as label FROM [dbo].[Chart] where Status = 0 AND Type <> 7 ' 

	set @text = 'SELECT top 100 C.ID AS value , C.Acct AS acct, C.fDesc as label,Br.Name As Company , isnull(B.ID,0) as BankID 
					FROM Chart C
					left join Bank B on C.ID=b.Chart
					left join Rol  r on  B.Rol=r.ID
					left outer join Branch br on br.ID = r.EN or br.ID=C.EN
					where C.Status = 0 AND C.Type <> 7  '


	if(@SearchText<>'')
	begin
		set @text += ' AND ((dbo.RemoveSpecialChars(c.Acct) LIKE ''%'+@WOspacialchars+'%'') OR (dbo.RemoveSpecialChars(c.fDesc) LIKE ''%'+@WOspacialchars+'%'')) '
	end
	set @text += ' Order by c.Acct'



	exec(@text)

END
