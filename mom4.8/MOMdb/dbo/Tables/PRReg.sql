CREATE TABLE [dbo].[PRReg](
	[ID] [int] NOT NULL,
	[fDate] [datetime] NULL,
	[Ref] [int] NULL,
	[fDesc] [varchar](50) NULL,
	[EmpID] [int] NULL,
	[Bank] [int] NULL,
	[TransID] [int] NULL,
	[Reg] [numeric](30, 2) NULL,
	[YReg] [numeric](30, 2) NULL,
	[HReg] [numeric](30, 2) NULL,
	[HYReg] [numeric](30, 2) NULL,
	[OT] [numeric](30, 2) NULL,
	[YOT] [numeric](30, 2) NULL,
	[HOT] [numeric](30, 2) NULL,
	[HYOT] [numeric](30, 2) NULL,
	[DT] [numeric](30, 2) NULL,
	[YDT] [numeric](30, 2) NULL,
	[HDT] [numeric](30, 2) NULL,
	[HYDT] [numeric](30, 2) NULL,
	[TT] [numeric](30, 2) NULL,
	[YTT] [numeric](30, 2) NULL,
	[HTT] [numeric](30, 2) NULL,
	[HYTT] [numeric](30, 2) NULL,
	[Hol] [numeric](30, 2) NULL,
	[YHol] [numeric](30, 2) NULL,
	[HHol] [numeric](30, 2) NULL,
	[HYHol] [numeric](30, 2) NULL,
	[Vac] [numeric](30, 2) NULL,
	[YVac] [numeric](30, 2) NULL,
	[HVac] [numeric](30, 2) NULL,
	[HYVac] [numeric](30, 2) NULL,
	[Zone] [numeric](30, 2) NULL,
	[YZone] [numeric](30, 2) NULL,
	[Reimb] [numeric](30, 2) NULL,
	[YReimb] [numeric](30, 2) NULL,
	[Mile] [numeric](30, 2) NULL,
	[YMile] [numeric](30, 2) NULL,
	[HMile] [numeric](30, 2) NULL,
	[HYMile] [numeric](30, 2) NULL,
	[Bonus] [numeric](30, 2) NULL,
	[YBonus] [numeric](30, 2) NULL,
	[WFIT] [numeric](30, 2) NULL,
	[WFica] [numeric](30, 2) NULL,
	[WMedi] [numeric](30, 2) NULL,
	[WFuta] [numeric](30, 2) NULL,
	[WSit] [numeric](30, 2) NULL,
	[WVac] [numeric](30, 2) NULL,
	[WWComp] [numeric](30, 2) NULL,
	[WUnion] [numeric](30, 2) NULL,
	[FIT] [numeric](30, 2) NULL,
	[YFIT] [numeric](30, 2) NULL,
	[FICA] [numeric](30, 2) NULL,
	[YFICA] [numeric](30, 2) NULL,
	[MEDI] [numeric](30, 2) NULL,
	[YMEDI] [numeric](30, 2) NULL,
	[FUTA] [numeric](30, 2) NULL,
	[YFUTA] [numeric](30, 2) NULL,
	[SIT] [numeric](30, 2) NULL,
	[YSIT] [numeric](30, 2) NULL,
	[Local] [numeric](30, 2) NULL,
	[YLocal] [numeric](30, 2) NULL,
	[TOTher] [numeric](30, 2) NULL,
	[NT] [numeric](30, 2) NULL,
	[YTOTher] [numeric](30, 2) NULL,
	[TInc] [numeric](30, 2) NULL,
	[YNT] [numeric](30, 2) NULL,
	[HNT] [numeric](30, 2) NULL,
	[TDed] [numeric](30, 2) NULL,
	[HYNT] [numeric](30, 2) NULL,
	[Net] [numeric](30, 2) NULL,
	[State] [varchar](2) NULL,
	[VThis] [numeric](30, 2) NULL,
	[REIMJE] [numeric](30, 4) NULL,
	[WELF] [numeric](30, 4) NULL,
	[SDI] [numeric](30, 4) NULL,
	[401K] [numeric](30, 4) NULL,
	[GARN] [numeric](30, 4) NULL,
	[WeekNo] [int] NULL,
	[Remarks] [varchar](255) NULL,
	[ELast] [numeric](30, 4) NULL,
	[EThis] [numeric](30, 4) NULL,
	[CompMedi] [numeric](30, 2) NOT NULL,
	[WMediOverTH] [numeric](30, 2) NOT NULL,
	[Sick] [numeric](30, 2) NOT NULL,
	[YSick] [numeric](30, 2) NOT NULL,
	[WSick] [numeric](30, 2) NOT NULL,
	[HSick] [numeric](30, 2) NOT NULL,
	[HYSick] [numeric](30, 2) NOT NULL,
	[HSickAccrued] [numeric](30, 2) NOT NULL,
	[HYSickAccrued] [numeric](30, 2) NOT NULL,
	[HVacAccrued] [numeric](30, 2) NOT NULL,
	[HYVacAccrued] [numeric](30, 2) NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PRReg] ADD  DEFAULT ((0)) FOR [CompMedi]
GO
ALTER TABLE [dbo].[PRReg] ADD  DEFAULT ((0)) FOR [WMediOverTH]
GO
ALTER TABLE [dbo].[PRReg] ADD  DEFAULT ((0)) FOR [Sick]
GO
ALTER TABLE [dbo].[PRReg] ADD  DEFAULT ((0)) FOR [YSick]
GO
ALTER TABLE [dbo].[PRReg] ADD  DEFAULT ((0)) FOR [WSick]
GO
ALTER TABLE [dbo].[PRReg] ADD  DEFAULT ((0)) FOR [HSick]
GO
ALTER TABLE [dbo].[PRReg] ADD  DEFAULT ((0)) FOR [HYSick]
GO
ALTER TABLE [dbo].[PRReg] ADD  DEFAULT ((0)) FOR [HSickAccrued]
GO
ALTER TABLE [dbo].[PRReg] ADD  DEFAULT ((0)) FOR [HYSickAccrued]
GO
ALTER TABLE [dbo].[PRReg] ADD  DEFAULT ((0)) FOR [HVacAccrued]
GO
ALTER TABLE [dbo].[PRReg] ADD  DEFAULT ((0)) FOR [HYVacAccrued]
GO