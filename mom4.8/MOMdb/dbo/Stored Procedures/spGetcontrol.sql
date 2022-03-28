CREATE PROCEDURE [dbo].[spGetcontrol]
AS
DECLARE @sqlQuery VARCHAR(MAX) 
BEGIN
	SET @sqlQuery = + 'select c.Name,c.City,c.State,c.Zip,c.Phone,c.Fax,c.fLong,c.Latt,c.GeoLock,c.Version,c.CDesc,c.Build,c.Minor,c.Address,
	c.AgeRemark,c.SDate,c.EDate,c.YDate,c.GSTreg,c.IDesc,c.PortalsID,c.PrContractRemark,c.RepUser,c.RepTitle,c.Logo,c.LogoPath,c.ExeBuildDate_Max,
	c.ExeBuildDate_Min,c.ExeVersion_Min,c.ExeVersion_Max,c.MerchantServicesConfig,c.Email,c.WebAddress,c.MSM,c.DSN,c.Username,c.Password,c.DBName,
	c.Contact,c.Remarks,c.Map,c.Custweb,c.QBPath,c.MultiLang,c.QBIntegration,c.QBLastSync,c.MSSignTime,c.GrossInc,
	c.Month,c.SalesAnnual,c.Payment,c.QBServiceItem,c.QBServiceItemLabor,c.QBServiceItemExp,c.GPS,c.SageLastSync,c.SageIntegration,c.MSAttachReport,
	c.MSRTLabel,c.MSOTLabel,c.MSNTLabel,c.MSDTLabel,c.MSTTLabel,c.MSTRTLabel,c.MSTOTLabel,c.MSTNTLabel,c.MSTDTLabel,c.MSTimeDataFieldVisibility,
	c.TsIntegration,c.SyncLast,c.SCDate,c.IntDate,c.SCAmount,c.IntAmount,c.EndBalance,c.StatementDate,c.Bank,c.Codes,c.ISshowHomeowner,
	c.IsLocAddressBlank,c.PGUsername,c.PGPassword,c.PGSecretKey,c.MSAppendMCPText,c.MSSHAssignedTicket,c.MSHistoryShowLastTenTickets,
	c.MS,c.ContactType,c.Lat,c.Lng,c.consultAPI,c.ShutdownAlert,c.MSIsEquipRequired,c.CoCode,c.MSCategoryPermission,c.ApplyPasswordRules,
	c.ApplyPwRulesToFieldUser,c.ApplyPwRulesToOfficeUser,c.ApplyPwRulesToCustomerUser,c.ApplyPwReset,c.PwResetDays,c.PwResetting,
	c.PwResetUserID,c.PR,isnull(JobCostLabor,0) as JobCostLabor1,isnull(msemail,0) as msemailnull,
	isnull(QBFirstSync,1) as EmpSync,isnull(msrep,0) as msreptemp, isnull(tinternet,0) as tinternett,businessstart, businessend,
	isnull(MSIsTaskCodesRequired,0) as TaskCode,c.YE As Year,ISNULL(IsUseTaxAPBill,0) as IsUseTaxAPBills,
	ISNULL(IsSalesTaxAPBill,0) as IsSalesTaxAPBills, TargetHPermission,r.Email as PwResetAdminEmail, u.fUser as PwResetUsername 
	from control  c left join tblUser u on u.ID = c.PwResetUserID left join emp e on e.CallSign = u.fUser left join Rol r on r.id = e.Rol';
END

PRINT @sqlQuery
EXEC (@sqlQuery);
