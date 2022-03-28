CREATE proc [dbo].[spUpdateTicketTimesheet]
@TicketData AS [dbo].[tblTypeTimesheetTicket] Readonly
as
    update TicketD set	 
	Reg=td.Reg,
	OT =td.OT,
	NT=td.NT,
	DT=td.DT,
	TT=td.TT,
	Total=	td.Reg+
	td.OT +
	td.NT+
	td.DT+
	td.TT,
	Zone=td.Zone,
	Mileage= td.Mileage,
	[OtherE]=td.[Misc],
	[Toll]=td.[Toll],
	Custom2=CONVERT(varchar(50), td.Extra),
	HourlyRate=td.HourlyRate	,
	CustomTick5=td.Custom,
	CustomTick3=td.CustomTick3,
	CustomTick2=td.CustomTick2,
	CustomTick1=td.CustomTick1
	from TicketD d 
	inner join @TicketData td on td.TicketID=d.ID

	      DECLARE @nkID int;  

		 SELECT top 1 @nkID=TicketID FROM @TicketData WHERE TicketID is not null 
    
		 EXEC spReCalCulateLaborexpense  @nkTicketID =@nkID   