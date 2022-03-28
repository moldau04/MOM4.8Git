CREATE PROCEDURE [dbo].[spGetLocationDocuments]
	@Id int,
	@IsShowAll bit,
	@IsLoc bit
AS

--Declare @Id int = 3305
--Declare @IsShowAll bit = 1
--Declare @IsLoc bit = 0

If @IsShowAll = 1 AND @IsLoc = 1
Begin
	-- Location
	select Filename
		, case when isnull(filename,'') <> '' then 
				case when reverse(left(reverse(Filename),charindex('.',reverse(Filename))-1)) in ('jpg', 'jpeg', 'bmp', 'png', 'gif') then 'Picture'
				else 'Document' end
		  else '' end as doctype 
		, null as Project
		, '' as ProjectName
		, null as Ticket
		, '' as AssignedTo
		, null as Date
		, Path
		, Screen
		, Remarks
		, ID
		, Portal
	from Documents Where Screen = 'Location' and ScreenID = @Id
	Union
	-- Project
	select d.Filename
		, case when isnull(filename,'') <> '' then 
				case when reverse(left(reverse(Filename),charindex('.',reverse(Filename))-1)) in ('jpg', 'jpeg', 'bmp', 'png', 'gif') then 'Picture'
				else 'Document' end
		  else '' end as doctype 
		, d.ScreenID as Project
		, j.fDesc as ProjectName
		, null as Ticket
		, '' as AssignedTo
		, null as Date
		, Path
		, Screen 
		, d.Remarks
		, d.ID
		, d.Portal
	from (Select * from Documents Where Screen = 'Project') as d 
	inner join Job j on j.ID = d.ScreenID 
	inner join loc l on l.Loc = j.Loc
	where l.Loc = @Id
	Union
	-- TicketD
	select d.Filename
		, case when isnull(filename,'') <> '' then 
				case when reverse(left(reverse(Filename),charindex('.',reverse(Filename))-1)) in ('jpg', 'jpeg', 'bmp', 'png', 'gif') then 'Picture'
				else 'Document' end
		  else '' end as doctype 
		, null as Project
		, '' as ProjectName
		, td.ID as Ticket
		, tw.fDesc as AssignedTo
		, td.EDate as Date
		, Path
		, Screen 
		, d.Remarks
		, d.ID
		, d.Portal
	from (Select * from Documents Where Screen = 'Ticket') as d
	inner join TicketD td on d.ScreenID = td.ID
	left join tblWork tw on td.fWork = tw.ID
	WHERE td.Loc = @Id
	Union
	-- TicketO
	select d.Filename
		, case when isnull(filename,'') <> '' then 
				case when reverse(left(reverse(Filename),charindex('.',reverse(Filename))-1)) in ('jpg', 'jpeg', 'bmp', 'png', 'gif') then 'Picture'
				else 'Document' end
		  else '' end as doctype 
		, null as Project
		, '' as ProjectName
		, t.ID as Ticket
		, tw.fDesc as AssignedTo
		, t.EDate
		, Path
		, Screen
		, d.Remarks
		, d.ID
		, d.Portal
	from (Select * from Documents Where Screen = 'Ticket') as d
	inner join TicketO t on d.ScreenID = t.ID
	left join tblWork tw on t.fWork = tw.ID
	WHERE t.LID = @Id
	Union
	-- ProposalForm
	select pf.FileName 
,'pdf' as doctype 
, lt.JobId as Project
,j.fDesc  as ProjectName
,lt.Ticket as Ticket
,'' as AssignedTo
,lt.Next as EDate
,pf.PdfFilePath as Path
,'SafetyTest' as screen
,Convert(varchar(800),lt.Remarks) Remarks
,pd.ProposalID as ID
,null as Portal
from LoadTestItem lt
left join Job j on lt.JobId=j.ID
left join ProposalFormDetail pd on pd.TestID=lt.LID
left join ProposalForm pf on pf.ID=pd.ProposalID
where lt.Loc=@Id
end
Else
Begin
	Declare @screen varchar(50)
	if @IsLoc = 1
		set @screen = 'Location'
	else set @screen = 'SalesLead'
	select Filename
		, case when isnull(filename,'') <> '' then 
				case when reverse(left(reverse(Filename),charindex('.',reverse(Filename))-1)) in ('jpg', 'jpeg', 'bmp', 'png', 'gif') then 'Picture'
				else 'Document' end
		  else '' end as doctype 
		, null as Project
		, '' as ProjectName
		, null as Ticket
		, '' as AssignedTo
		, null as Date
		, Path
		, Screen
		, Remarks
		, ID
		, Portal
	from Documents Where Screen = @Screen and ScreenID = @Id
End