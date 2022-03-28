using System;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using BusinessEntity;


namespace DataLayer
{
    public class DL_ReportsData
    {
        public DataSet GetCustomerDetails(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "sp_GetCustomerDetails_Report", objPropUser.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetCustomerDetailsTest(User objPropUser)
        {
            try
            {
                DataSet ds = new DataSet();
                StringBuilder varname1 = new StringBuilder();
                //varname1.Append("select r.Name, r.City, r.State, r.Zip, r.Phone, r.Fax, r.Contact, r.Address, r.Email, r.Country, r.Website, r.Cellular, \n");
                //varname1.Append("o.[Type], o.Balance, o.Status, \n");
                //varname1.Append("l.ID AS LocationId, l.Tag AS LocationName, l.Address AS LocationAddress, l.City AS LocationCity, l.State AS LocationState, l.Zip AS LocationZip, l.Type AS LocationType, \n");
                //varname1.Append("l.STax AS LocationSTax, l.Elevs AS EquipmentCounts, l.Status AS LocationStatus, \n");
                //varname1.Append("l.Balance AS LocationBalance, t.Name AS DefaultSalesPerson, l.prospect AS LocationProspect, e.Unit AS EquipmentName, \n");
                //varname1.Append("e.Manuf, e.Type AS EquipmentType, e.Cat AS ServiceType, e.Price AS EquipmentPrice, e.Install AS InstalledOn, e.State AS EquipmentState, e.Building AS BuildingType, \n");
                //varname1.Append("(select count(1) from loc where owner=o.id) as loc, \n");
                //varname1.Append("(select count(1) from elev where owner=o.id) as equip, \n");
                //varname1.Append("(select count(1) from ticketo where owner=o.id) as opencall \n");
                //varname1.Append("from Rol r inner join owner o on r.Id = o.Rol ");
                //varname1.Append("INNER JOIN Loc AS l ON o.ID = l.Owner ");
                //varname1.Append("INNER JOIN Terr AS t ON t.ID = l.Terr ");
                //varname1.Append("INNER JOIN Elev AS e ON l.Loc = e.Loc ");
                //varname1.Append("order by r.name");

                varname1.Append("select r.Name, r.City, r.State, r.Zip, r.Phone, r.Fax, r.Contact, r.Address, r.Email, r.Country, r.Website, r.Cellular, \n");
                varname1.Append("o.[Type], o.Balance, o.Status, \n");
                varname1.Append("(select count(1) from loc where owner=o.id) as loc, \n");
                varname1.Append("(select count(1) from elev where owner=o.id) as equip, \n");
                varname1.Append("(select count(1) from ticketo where owner=o.id) as opencall \n");
                varname1.Append("from Rol r inner join owner o on r.Id = o.Rol \n");


                //varname1.Append("select l.ID AS LocationId, l.Tag AS LocationName, l.Address AS LocationAddress, l.City AS LocationCity, l.State AS LocationState, l.Zip AS LocationZip, l.Type AS LocationType, \n");
                //varname1.Append("l.STax AS LocationSTax, l.Elevs AS EquipmentCounts, l.Status AS LocationStatus, \n");
                //varname1.Append("l.Balance AS LocationBalance, t.Name AS DefaultSalesPerson, l.prospect AS LocationProspect, ro.Name as Route \n");
                //varname1.Append("from Terr AS t INNER JOIN Loc AS l ON t.ID = l.Terr \n");
                //varname1.Append("INNER JOIN route AS ro ON ro.ID = l.Route \n");
                //varname1.Append("select e.Unit AS EquipmentName, e.Manuf, e.Type AS EquipmentType, e.Cat AS ServiceType, e.Price AS EquipmentPrice, e.Install AS InstalledOn, e.State AS EquipmentState, e.Building AS BuildingType \n");
                //varname1.Append("from Elev as e \n");




                //varname1.Append("select v.ID VendorID, v.Acct AS VendorName, v.Type AS VendorType, v.Status As VendorStatus, v.Balance As VBalance, v.CLimit, v.DA, v.Terms,v.Days, v.InUse \n");
                //varname1.Append("from Rol r inner join Vendor v on r.Id = v.Rol and r.Type = 1 \n");

               /*
                varname1.Append("Select OpenAP.fDate,OpenAp.Due,PJ.Ref,Rol.Name As RolName,PJ.fDesc,PJ.Amount from OpenAP inner join PJ on OpenAP.Ref = PJ.Ref \n");
                varname1.Append("inner join Vendor on PJ.Vendor = Vendor.ID inner Join Rol on Rol.ID = Vendor.Rol \n");

                varname1.Append("SELECT c.expirationdate, c.Job, \n");
                varname1.Append("       j.ctype, \n");
                varname1.Append("       j.fdesc, \n");
                varname1.Append("       c.BAmt, \n");
                varname1.Append("       c.Hours, \n");
                varname1.Append("       l.ID                   AS locid, \n");
                varname1.Append("       l.Tag, \n");
                varname1.Append("       (SELECT TOP 1 name \n");
                varname1.Append("        FROM   rol r \n");
                varname1.Append("        WHERE  o.rol = r.id \n");
                varname1.Append("               )AS name, \n");
                varname1.Append("       Round (CASE c.BCycle \n");
                varname1.Append("                WHEN 0 THEN c.BAmt \n");
                varname1.Append("                WHEN 1 THEN c.BAmt / 2 \n");
                varname1.Append("                WHEN 2 THEN c.BAmt / 3 \n");
                varname1.Append("                WHEN 3 THEN c.BAmt / 6 \n");
                varname1.Append("                WHEN 4 THEN c.BAmt / 12 \n");
                varname1.Append("              END, 2)         AS MonthlyBill, \n");
                varname1.Append("       Round (CASE c.SCycle \n");
                varname1.Append("                WHEN 0 THEN c.Hours --Monthly \n");
                varname1.Append("                WHEN 1 THEN c.Hours / 2 --Bi-Monthly \n");
                varname1.Append("                WHEN 2 THEN c.Hours / 3 --Quarterly \n");
                varname1.Append("                WHEN 3 THEN c.Hours / 6 --Semi-Anually \n");
                varname1.Append("                WHEN 4 THEN c.Hours / 12 --Anually \n");
                varname1.Append("                --WHEN 5 THEN c.Hours * 4.3 / 12 --Weekly \n");
                varname1.Append("                --WHEN 6 THEN c.Hours * 2.15 / 12 --Bi-Weekly \n");
                varname1.Append("                WHEN 10 THEN c.Hours / 12*2 --Every 2 Years \n");
                varname1.Append("                WHEN 8 THEN c.Hours / 12*3 --Every 3 Years \n");
                varname1.Append("                WHEN 9 THEN c.Hours / 12*5 --Every 5 Years \n");
                varname1.Append("                WHEN 11 THEN c.Hours / 12*7 --Every 7 Years \n");
                varname1.Append("              END, 2)         AS MonthlyHours, \n");
                varname1.Append("       CASE c.bcycle \n");
                varname1.Append("         WHEN 0 THEN 'Monthly' \n");
                varname1.Append("         WHEN 1 THEN 'Bi-Monthly' \n");
                varname1.Append("         WHEN 2 THEN 'Quarterly' \n");
                varname1.Append("         WHEN 3 THEN '3 Times/Year' \n");
                varname1.Append("         WHEN 4 THEN 'Semi-Annually' \n");
                varname1.Append("         WHEN 5 THEN 'Anually' \n");
                varname1.Append("         WHEN 6 THEN 'Never' \n");
                varname1.Append("       END                    Freqency, \n");
                varname1.Append("       CASE c.scycle \n");
                varname1.Append("         WHEN 0 THEN 'Monthly' \n");
                varname1.Append("         WHEN 1 THEN 'Bi-Monthly' \n");
                varname1.Append("         WHEN 2 THEN 'Quarterly' \n");
                varname1.Append("         WHEN 3 THEN 'Semi-Anually' \n");
                varname1.Append("         WHEN 4 THEN 'Anually' \n");
                varname1.Append("        -- WHEN 5 THEN 'Weekly' \n");
                varname1.Append("       --  WHEN 6 THEN 'Bi-Weekly' \n");
                varname1.Append("         WHEN 7 THEN 'Every 13 Weeks' \n");
                varname1.Append("         WHEN 10 THEN 'Every 2 Years' \n");
                varname1.Append("         WHEN 8 THEN 'Every 3 Years' \n");
                varname1.Append("         WHEN 9 THEN 'Every 5 Years' \n");
                varname1.Append("         WHEN 11 THEN 'Every 7 Years' \n");
                varname1.Append("         WHEN 12 THEN 'On-Demand' \n");
                varname1.Append("       END                    TicketFreq, \n");
                varname1.Append("       CASE j.Status \n");
                varname1.Append("         WHEN 0 THEN 'Active' \n");
                varname1.Append("         WHEN 1 THEN 'Closed' \n");
                varname1.Append("         WHEN 2 THEN 'Hold' \n");
                varname1.Append("         WHEN 3 THEN 'Completed' \n");
                varname1.Append("       END                    Status \n");
                varname1.Append("FROM   job j \n");
                varname1.Append("       INNER JOIN Contract c \n");
                varname1.Append("               ON j.id = c.Job \n");
                varname1.Append("       LEFT OUTER JOIN Loc l \n");
                varname1.Append("                    ON l.Loc = c.Loc \n");
                varname1.Append("       LEFT OUTER JOIN owner o \n");
                varname1.Append("                    ON o.id = l.owner \n");
                varname1.Append("       LEFT OUTER JOIN rol r \n");
                varname1.Append("                    ON o.rol = r.id \n");
                varname1.Append("WHERE  j.type = 0 ");


                //varname1.Append("from loc as l ");
                //varname1.Append("inner join rol as r ");
                //varname1.Append("on r.ID = l.Rol ");
                //varname1.Append("inner join route as ro ");
                //varname1.Append("on ro.ID = l.route ");
                

                //if (!(string.IsNullOrEmpty(LocName)))
                //{
                //    varname1.Append("where l.type like '%" + LocName + "%'");
                //}
                */
                

                ds = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //api
        public DataSet GetCustomerDetailsTest(getCustomerDetailsTestParam _getCustomerDetailsTest, string ConnectionString)
        {
            try
            {
                DataSet ds = new DataSet();
                StringBuilder varname1 = new StringBuilder();
                //varname1.Append("select r.Name, r.City, r.State, r.Zip, r.Phone, r.Fax, r.Contact, r.Address, r.Email, r.Country, r.Website, r.Cellular, \n");
                //varname1.Append("o.[Type], o.Balance, o.Status, \n");
                //varname1.Append("l.ID AS LocationId, l.Tag AS LocationName, l.Address AS LocationAddress, l.City AS LocationCity, l.State AS LocationState, l.Zip AS LocationZip, l.Type AS LocationType, \n");
                //varname1.Append("l.STax AS LocationSTax, l.Elevs AS EquipmentCounts, l.Status AS LocationStatus, \n");
                //varname1.Append("l.Balance AS LocationBalance, t.Name AS DefaultSalesPerson, l.prospect AS LocationProspect, e.Unit AS EquipmentName, \n");
                //varname1.Append("e.Manuf, e.Type AS EquipmentType, e.Cat AS ServiceType, e.Price AS EquipmentPrice, e.Install AS InstalledOn, e.State AS EquipmentState, e.Building AS BuildingType, \n");
                //varname1.Append("(select count(1) from loc where owner=o.id) as loc, \n");
                //varname1.Append("(select count(1) from elev where owner=o.id) as equip, \n");
                //varname1.Append("(select count(1) from ticketo where owner=o.id) as opencall \n");
                //varname1.Append("from Rol r inner join owner o on r.Id = o.Rol ");
                //varname1.Append("INNER JOIN Loc AS l ON o.ID = l.Owner ");
                //varname1.Append("INNER JOIN Terr AS t ON t.ID = l.Terr ");
                //varname1.Append("INNER JOIN Elev AS e ON l.Loc = e.Loc ");
                //varname1.Append("order by r.name");

                varname1.Append("select r.Name, r.City, r.State, r.Zip, r.Phone, r.Fax, r.Contact, r.Address, r.Email, r.Country, r.Website, r.Cellular, \n");
                varname1.Append("o.[Type], o.Balance, o.Status, \n");
                varname1.Append("(select count(1) from loc where owner=o.id) as loc, \n");
                varname1.Append("(select count(1) from elev where owner=o.id) as equip, \n");
                varname1.Append("(select count(1) from ticketo where owner=o.id) as opencall \n");
                varname1.Append("from Rol r inner join owner o on r.Id = o.Rol \n");


                //varname1.Append("select l.ID AS LocationId, l.Tag AS LocationName, l.Address AS LocationAddress, l.City AS LocationCity, l.State AS LocationState, l.Zip AS LocationZip, l.Type AS LocationType, \n");
                //varname1.Append("l.STax AS LocationSTax, l.Elevs AS EquipmentCounts, l.Status AS LocationStatus, \n");
                //varname1.Append("l.Balance AS LocationBalance, t.Name AS DefaultSalesPerson, l.prospect AS LocationProspect, ro.Name as Route \n");
                //varname1.Append("from Terr AS t INNER JOIN Loc AS l ON t.ID = l.Terr \n");
                //varname1.Append("INNER JOIN route AS ro ON ro.ID = l.Route \n");
                //varname1.Append("select e.Unit AS EquipmentName, e.Manuf, e.Type AS EquipmentType, e.Cat AS ServiceType, e.Price AS EquipmentPrice, e.Install AS InstalledOn, e.State AS EquipmentState, e.Building AS BuildingType \n");
                //varname1.Append("from Elev as e \n");




                //varname1.Append("select v.ID VendorID, v.Acct AS VendorName, v.Type AS VendorType, v.Status As VendorStatus, v.Balance As VBalance, v.CLimit, v.DA, v.Terms,v.Days, v.InUse \n");
                //varname1.Append("from Rol r inner join Vendor v on r.Id = v.Rol and r.Type = 1 \n");

                /*
                 varname1.Append("Select OpenAP.fDate,OpenAp.Due,PJ.Ref,Rol.Name As RolName,PJ.fDesc,PJ.Amount from OpenAP inner join PJ on OpenAP.Ref = PJ.Ref \n");
                 varname1.Append("inner join Vendor on PJ.Vendor = Vendor.ID inner Join Rol on Rol.ID = Vendor.Rol \n");

                 varname1.Append("SELECT c.expirationdate, c.Job, \n");
                 varname1.Append("       j.ctype, \n");
                 varname1.Append("       j.fdesc, \n");
                 varname1.Append("       c.BAmt, \n");
                 varname1.Append("       c.Hours, \n");
                 varname1.Append("       l.ID                   AS locid, \n");
                 varname1.Append("       l.Tag, \n");
                 varname1.Append("       (SELECT TOP 1 name \n");
                 varname1.Append("        FROM   rol r \n");
                 varname1.Append("        WHERE  o.rol = r.id \n");
                 varname1.Append("               )AS name, \n");
                 varname1.Append("       Round (CASE c.BCycle \n");
                 varname1.Append("                WHEN 0 THEN c.BAmt \n");
                 varname1.Append("                WHEN 1 THEN c.BAmt / 2 \n");
                 varname1.Append("                WHEN 2 THEN c.BAmt / 3 \n");
                 varname1.Append("                WHEN 3 THEN c.BAmt / 6 \n");
                 varname1.Append("                WHEN 4 THEN c.BAmt / 12 \n");
                 varname1.Append("              END, 2)         AS MonthlyBill, \n");
                 varname1.Append("       Round (CASE c.SCycle \n");
                 varname1.Append("                WHEN 0 THEN c.Hours --Monthly \n");
                 varname1.Append("                WHEN 1 THEN c.Hours / 2 --Bi-Monthly \n");
                 varname1.Append("                WHEN 2 THEN c.Hours / 3 --Quarterly \n");
                 varname1.Append("                WHEN 3 THEN c.Hours / 6 --Semi-Anually \n");
                 varname1.Append("                WHEN 4 THEN c.Hours / 12 --Anually \n");
                 varname1.Append("                --WHEN 5 THEN c.Hours * 4.3 / 12 --Weekly \n");
                 varname1.Append("                --WHEN 6 THEN c.Hours * 2.15 / 12 --Bi-Weekly \n");
                 varname1.Append("                WHEN 10 THEN c.Hours / 12*2 --Every 2 Years \n");
                 varname1.Append("                WHEN 8 THEN c.Hours / 12*3 --Every 3 Years \n");
                 varname1.Append("                WHEN 9 THEN c.Hours / 12*5 --Every 5 Years \n");
                 varname1.Append("                WHEN 11 THEN c.Hours / 12*7 --Every 7 Years \n");
                 varname1.Append("              END, 2)         AS MonthlyHours, \n");
                 varname1.Append("       CASE c.bcycle \n");
                 varname1.Append("         WHEN 0 THEN 'Monthly' \n");
                 varname1.Append("         WHEN 1 THEN 'Bi-Monthly' \n");
                 varname1.Append("         WHEN 2 THEN 'Quarterly' \n");
                 varname1.Append("         WHEN 3 THEN '3 Times/Year' \n");
                 varname1.Append("         WHEN 4 THEN 'Semi-Annually' \n");
                 varname1.Append("         WHEN 5 THEN 'Anually' \n");
                 varname1.Append("         WHEN 6 THEN 'Never' \n");
                 varname1.Append("       END                    Freqency, \n");
                 varname1.Append("       CASE c.scycle \n");
                 varname1.Append("         WHEN 0 THEN 'Monthly' \n");
                 varname1.Append("         WHEN 1 THEN 'Bi-Monthly' \n");
                 varname1.Append("         WHEN 2 THEN 'Quarterly' \n");
                 varname1.Append("         WHEN 3 THEN 'Semi-Anually' \n");
                 varname1.Append("         WHEN 4 THEN 'Anually' \n");
                 varname1.Append("        -- WHEN 5 THEN 'Weekly' \n");
                 varname1.Append("       --  WHEN 6 THEN 'Bi-Weekly' \n");
                 varname1.Append("         WHEN 7 THEN 'Every 13 Weeks' \n");
                 varname1.Append("         WHEN 10 THEN 'Every 2 Years' \n");
                 varname1.Append("         WHEN 8 THEN 'Every 3 Years' \n");
                 varname1.Append("         WHEN 9 THEN 'Every 5 Years' \n");
                 varname1.Append("         WHEN 11 THEN 'Every 7 Years' \n");
                 varname1.Append("         WHEN 12 THEN 'On-Demand' \n");
                 varname1.Append("       END                    TicketFreq, \n");
                 varname1.Append("       CASE j.Status \n");
                 varname1.Append("         WHEN 0 THEN 'Active' \n");
                 varname1.Append("         WHEN 1 THEN 'Closed' \n");
                 varname1.Append("         WHEN 2 THEN 'Hold' \n");
                 varname1.Append("         WHEN 3 THEN 'Completed' \n");
                 varname1.Append("       END                    Status \n");
                 varname1.Append("FROM   job j \n");
                 varname1.Append("       INNER JOIN Contract c \n");
                 varname1.Append("               ON j.id = c.Job \n");
                 varname1.Append("       LEFT OUTER JOIN Loc l \n");
                 varname1.Append("                    ON l.Loc = c.Loc \n");
                 varname1.Append("       LEFT OUTER JOIN owner o \n");
                 varname1.Append("                    ON o.id = l.owner \n");
                 varname1.Append("       LEFT OUTER JOIN rol r \n");
                 varname1.Append("                    ON o.rol = r.id \n");
                 varname1.Append("WHERE  j.type = 0 ");


                 //varname1.Append("from loc as l ");
                 //varname1.Append("inner join rol as r ");
                 //varname1.Append("on r.ID = l.Rol ");
                 //varname1.Append("inner join route as ro ");
                 //varname1.Append("on ro.ID = l.route ");


                 //if (!(string.IsNullOrEmpty(LocName)))
                 //{
                 //    varname1.Append("where l.type like '%" + LocName + "%'");
                 //}
                 */


                ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, varname1.ToString());

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get customer summary listing details. By Yashasvi Jadav
        /// </summary>
        /// <param name="cstData"></param>
        /// <param name="LocName"></param>
        /// <returns></returns>
        public DataSet GetAccSummaryDetail(User cstData)
        {
            try
            {
                DataSet ds = new DataSet();
                StringBuilder varname1 = new StringBuilder();
                varname1.Append("select \n");
                varname1.Append("Name as Route \n");
                //varname1.Append("from loc as l ");
                //varname1.Append("inner join rol as r ");
                //varname1.Append("on r.ID = l.Rol ");
                //varname1.Append("inner join route as ro ");
                //varname1.Append("on ro.ID = l.route ");
                varname1.Append("from route");

                //if (!(string.IsNullOrEmpty(LocName)))
                //{
                //    varname1.Append("where l.type like '%" + LocName + "%'");
                //}

                ds = SqlHelper.ExecuteDataset(cstData.ConnConfig, CommandType.Text, varname1.ToString());

                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public DataSet GetAccSummaryDetail(GetAccountSummaryListingDetailParam _GetAccountSummaryListingDetailParam, string ConnectionString)
        {
            try
            {
                DataSet ds = new DataSet();
                StringBuilder varname1 = new StringBuilder();
                varname1.Append("select \n");
                varname1.Append("Name as Route \n");
                //varname1.Append("from loc as l ");
                //varname1.Append("inner join rol as r ");
                //varname1.Append("on r.ID = l.Rol ");
                //varname1.Append("inner join route as ro ");
                //varname1.Append("on ro.ID = l.route ");
                varname1.Append("from route");

                //if (!(string.IsNullOrEmpty(LocName)))
                //{
                //    varname1.Append("where l.type like '%" + LocName + "%'");
                //}

                ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, varname1.ToString());

                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataSet InsertCustomerReport(CustomerReport objCustReport)
        {
            var para = new SqlParameter[23];

            para[0] = new SqlParameter
            {
                ParameterName = "@ReportName",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.ReportName
            };
            para[1] = new SqlParameter
            {
                ParameterName = "@ReportType",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.ReportType
            };
            para[2] = new SqlParameter
            {
                ParameterName = "@UserId",
                SqlDbType = SqlDbType.Int,
                Value = objCustReport.UserId
            };
            para[3] = new SqlParameter
            {
                ParameterName = "@IsGlobal",
                SqlDbType = SqlDbType.Bit,
                Value = objCustReport.IsGlobal
            };
            para[4] = new SqlParameter
            {
                ParameterName = "@IsAscendingOrder",
                SqlDbType = SqlDbType.Bit,
                Value = objCustReport.IsAscending
            };
            para[5] = new SqlParameter
            {
                ParameterName = "@SortBy",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.SortBy
            };
            para[6] = new SqlParameter
            {
                ParameterName = "@ColumnName",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.ColumnName
            };
            para[7] = new SqlParameter
            {
                ParameterName = "@FilterColumns",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.FilterColumns
            };
            para[8] = new SqlParameter
            {
                ParameterName = "@FilterValues",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.FilterValues
            };
            para[9] = new SqlParameter
            {
                ParameterName = "@CompanyName",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.CompanyName
            };
            para[10] = new SqlParameter
            {
                ParameterName = "@ReportTitle",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.ReportTitle
            };
            para[11] = new SqlParameter
            {
                ParameterName = "@SubTitle",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.SubTitle
            };
            para[12] = new SqlParameter
            {
                ParameterName = "@DatePrepared",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.DatePrepared
            };
            para[13] = new SqlParameter
            {
                ParameterName = "@TimePrepared",
                SqlDbType = SqlDbType.Bit,
                Value = objCustReport.TimePrepared
            };
            para[14] = new SqlParameter
            {
                ParameterName = "@PageNumber",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.PageNumber
            };
            para[15] = new SqlParameter
            {
                ParameterName = "@ExtraFooterLine",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.ExtraFooterLine
            };
            para[16] = new SqlParameter
            {
                ParameterName = "@Alignment",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.Alignment
            };
            para[17] = new SqlParameter
            {
                ParameterName = "@ColumnWidth",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.ColumnWidth
            };
            para[18] = new SqlParameter
            {
                ParameterName = "@MainHeader",
                SqlDbType = SqlDbType.Bit,
                Value = objCustReport.MainHeader
            };
            para[19] = new SqlParameter
            {
                ParameterName = "@PDFSize",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.PDFSize
            };
            para[20] = new SqlParameter
            {
                ParameterName = "@IsStock",
                SqlDbType = SqlDbType.Bit,
                Value = objCustReport.IsStock
            };
            para[21] = new SqlParameter
            {
                ParameterName = "@Module",
                SqlDbType = SqlDbType.VarChar,
                Value = objCustReport.Module
            };

            para[22] = new SqlParameter
            {
                ParameterName = "@Condition",
                SqlDbType = SqlDbType.VarChar,
                Value = objCustReport.Condition
            };

            try
            {
                // SqlHelper.ExecuteNonQuery(objCustReport.ConnConfig, CommandType.StoredProcedure, "spAddCustomerReportDetails", para);
                return objCustReport.DsCustomer = SqlHelper.ExecuteDataset(objCustReport.ConnConfig, "spAddCustomerReportDetails", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet InsertCustomerReport(InsertCustomerReportParam _InsertCustomerReportParam, string ConnectionString)
        {
            var para = new SqlParameter[23];

            para[0] = new SqlParameter
            {
                ParameterName = "@ReportName",
                SqlDbType = SqlDbType.NVarChar,
                Value = _InsertCustomerReportParam.ReportName
            };
            para[1] = new SqlParameter
            {
                ParameterName = "@ReportType",
                SqlDbType = SqlDbType.NVarChar,
                Value = _InsertCustomerReportParam.ReportType
            };
            para[2] = new SqlParameter
            {
                ParameterName = "@UserId",
                SqlDbType = SqlDbType.Int,
                Value = _InsertCustomerReportParam.UserId
            };
            para[3] = new SqlParameter
            {
                ParameterName = "@IsGlobal",
                SqlDbType = SqlDbType.Bit,
                Value = _InsertCustomerReportParam.IsGlobal
            };
            para[4] = new SqlParameter
            {
                ParameterName = "@IsAscendingOrder",
                SqlDbType = SqlDbType.Bit,
                Value = _InsertCustomerReportParam.IsAscending
            };
            para[5] = new SqlParameter
            {
                ParameterName = "@SortBy",
                SqlDbType = SqlDbType.NVarChar,
                Value = _InsertCustomerReportParam.SortBy
            };
            para[6] = new SqlParameter
            {
                ParameterName = "@ColumnName",
                SqlDbType = SqlDbType.NVarChar,
                Value = _InsertCustomerReportParam.ColumnName
            };
            para[7] = new SqlParameter
            {
                ParameterName = "@FilterColumns",
                SqlDbType = SqlDbType.NVarChar,
                Value = _InsertCustomerReportParam.FilterColumns
            };
            para[8] = new SqlParameter
            {
                ParameterName = "@FilterValues",
                SqlDbType = SqlDbType.NVarChar,
                Value = _InsertCustomerReportParam.FilterValues
            };
            para[9] = new SqlParameter
            {
                ParameterName = "@CompanyName",
                SqlDbType = SqlDbType.NVarChar,
                Value = _InsertCustomerReportParam.CompanyName
            };
            para[10] = new SqlParameter
            {
                ParameterName = "@ReportTitle",
                SqlDbType = SqlDbType.NVarChar,
                Value = _InsertCustomerReportParam.ReportTitle
            };
            para[11] = new SqlParameter
            {
                ParameterName = "@SubTitle",
                SqlDbType = SqlDbType.NVarChar,
                Value = _InsertCustomerReportParam.SubTitle
            };
            para[12] = new SqlParameter
            {
                ParameterName = "@DatePrepared",
                SqlDbType = SqlDbType.NVarChar,
                Value = _InsertCustomerReportParam.DatePrepared
            };
            para[13] = new SqlParameter
            {
                ParameterName = "@TimePrepared",
                SqlDbType = SqlDbType.Bit,
                Value = _InsertCustomerReportParam.TimePrepared
            };
            para[14] = new SqlParameter
            {
                ParameterName = "@PageNumber",
                SqlDbType = SqlDbType.NVarChar,
                Value = _InsertCustomerReportParam.PageNumber
            };
            para[15] = new SqlParameter
            {
                ParameterName = "@ExtraFooterLine",
                SqlDbType = SqlDbType.NVarChar,
                Value = _InsertCustomerReportParam.ExtraFooterLine
            };
            para[16] = new SqlParameter
            {
                ParameterName = "@Alignment",
                SqlDbType = SqlDbType.NVarChar,
                Value = _InsertCustomerReportParam.Alignment
            };
            para[17] = new SqlParameter
            {
                ParameterName = "@ColumnWidth",
                SqlDbType = SqlDbType.NVarChar,
                Value = _InsertCustomerReportParam.ColumnWidth
            };
            para[18] = new SqlParameter
            {
                ParameterName = "@MainHeader",
                SqlDbType = SqlDbType.Bit,
                Value = _InsertCustomerReportParam.MainHeader
            };
            para[19] = new SqlParameter
            {
                ParameterName = "@PDFSize",
                SqlDbType = SqlDbType.NVarChar,
                Value = _InsertCustomerReportParam.PDFSize
            };
            para[20] = new SqlParameter
            {
                ParameterName = "@IsStock",
                SqlDbType = SqlDbType.Bit,
                Value = _InsertCustomerReportParam.IsStock
            };
            para[21] = new SqlParameter
            {
                ParameterName = "@Module",
                SqlDbType = SqlDbType.VarChar,
                Value = _InsertCustomerReportParam.Module
            };

            para[22] = new SqlParameter
            {
                ParameterName = "@Condition",
                SqlDbType = SqlDbType.VarChar,
                Value = _InsertCustomerReportParam.Condition
            };

            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, "spAddCustomerReportDetails", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CheckForDelete(CustomerReport objCustReport)
        {
            try
            {
                objCustReport.DsCustomer = SqlHelper.ExecuteDataset(objCustReport.ConnConfig, CommandType.Text, "select * from tblReports where UserId = " + objCustReport.UserId + " and Id = " + objCustReport.ReportId + "");
                if (objCustReport.DsCustomer.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteCustomerReport(CustomerReport objCustReport)
        {
            var para = new SqlParameter[1];

            para[0] = new SqlParameter
            {
                ParameterName = "@ReportId",
                SqlDbType = SqlDbType.Int,
                Value = objCustReport.ReportId
            };

            try
            {
                SqlHelper.ExecuteNonQuery(objCustReport.ConnConfig, CommandType.StoredProcedure, "spDeleteCustomerReport", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //api
        public void DeleteCustomerReport(DeleteCustomerReportParam objCustReport,string ConnectionString)
        {
            var para = new SqlParameter[1];

            para[0] = new SqlParameter
            {
                ParameterName = "@ReportId",
                SqlDbType = SqlDbType.Int,
                Value = objCustReport.ReportId
            };

            try
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spDeleteCustomerReport", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public bool CheckExistingReport(CustomerReport objCustReport, string reportAction)
        {
            try
            {
                objCustReport.DsCustomer = SqlHelper.ExecuteDataset(objCustReport.ConnConfig, CommandType.Text, "select * from tblReports where ReportName = '" + objCustReport.ReportName + "'");
                if (objCustReport.DsCustomer.Tables[0].Rows.Count > 0)
                {
                    if (reportAction != "Save")
                    {
                        if (objCustReport.ReportId == Convert.ToInt32(objCustReport.DsCustomer.Tables[0].Rows[0]["Id"]))
                        {
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool CheckExistingReport(CheckExistingReportParam _CheckExistingReportParam, string ConnectionString)
        {
            try
            {
                _CheckExistingReportParam.DsCustomer = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select * from tblReports where ReportName = '" + _CheckExistingReportParam.ReportName + "'");
                if (_CheckExistingReportParam.DsCustomer.Tables[0].Rows.Count > 0)
                {
                    if (_CheckExistingReportParam.reportAction != "Save")
                    {
                        if (_CheckExistingReportParam.ReportId == Convert.ToInt32(_CheckExistingReportParam.DsCustomer.Tables[0].Rows[0]["Id"]))
                        {
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsStockReportExist(CustomerReport objCustReport, string reportAction)
        {
            try
            {
                objCustReport.DsCustomer = SqlHelper.ExecuteDataset(objCustReport.ConnConfig, CommandType.Text, "select * from tblReports where ReportName = '" + objCustReport.ReportName + "' and IsStock='true'");
                if (objCustReport.DsCustomer.Tables[0].Rows.Count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool IsStockReportExist(IsStockReportExistParam _IsStockReportExistParam, string ConnectionString)
        {
            try
            {
                _IsStockReportExistParam.DsCustomer = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select * from tblReports where ReportName = '" + _IsStockReportExistParam.ReportName + "' and IsStock='true'");
                if (_IsStockReportExistParam.DsCustomer.Tables[0].Rows.Count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateCustomerReport(CustomerReport objCustReport)
        {
            var para = new SqlParameter[24];

            para[0] = new SqlParameter
            {
                ParameterName = "@RptId",
                SqlDbType = SqlDbType.Int,
                Value = objCustReport.ReportId
            };
            para[1] = new SqlParameter
            {
                ParameterName = "@ReportName",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.ReportName
            };
            para[2] = new SqlParameter
            {
                ParameterName = "@ReportType",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.ReportType
            };
            para[3] = new SqlParameter
            {
                ParameterName = "@UserId",
                SqlDbType = SqlDbType.Int,
                Value = objCustReport.UserId
            };
            para[4] = new SqlParameter
            {
                ParameterName = "@IsGlobal",
                SqlDbType = SqlDbType.Bit,
                Value = objCustReport.IsGlobal
            };
            para[5] = new SqlParameter
            {
                ParameterName = "@IsAscendingOrder",
                SqlDbType = SqlDbType.Bit,
                Value = objCustReport.IsAscending
            };
            para[6] = new SqlParameter
            {
                ParameterName = "@SortBy",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.SortBy
            };
            para[7] = new SqlParameter
            {
                ParameterName = "@ColumnName",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.ColumnName
            };
            para[8] = new SqlParameter
            {
                ParameterName = "@FilterColumns",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.FilterColumns
            };
            para[9] = new SqlParameter
            {
                ParameterName = "@FilterValues",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.FilterValues
            };
            para[10] = new SqlParameter
            {
                ParameterName = "@CompanyName",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.CompanyName
            };
            para[11] = new SqlParameter
            {
                ParameterName = "@ReportTitle",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.ReportTitle
            };
            para[12] = new SqlParameter
            {
                ParameterName = "@SubTitle",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.SubTitle
            };
            para[13] = new SqlParameter
            {
                ParameterName = "@DatePrepared",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.DatePrepared
            };
            para[14] = new SqlParameter
            {
                ParameterName = "@TimePrepared",
                SqlDbType = SqlDbType.Bit,
                Value = objCustReport.TimePrepared
            };
            para[15] = new SqlParameter
            {
                ParameterName = "@PageNumber",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.PageNumber
            };
            para[16] = new SqlParameter
            {
                ParameterName = "@ExtraFooterLine",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.ExtraFooterLine
            };
            para[17] = new SqlParameter
            {
                ParameterName = "@Alignment",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.Alignment
            };
            para[18] = new SqlParameter
            {
                ParameterName = "@ColumnWidth",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.ColumnWidth
            };
            para[19] = new SqlParameter
            {
                ParameterName = "@MainHeader",
                SqlDbType = SqlDbType.Bit,
                Value = objCustReport.MainHeader
            };
            para[20] = new SqlParameter
            {
                ParameterName = "@PDFSize",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.PDFSize
            };
            para[21] = new SqlParameter
            {
                ParameterName = "@IsStock",
                SqlDbType = SqlDbType.Bit,
                Value = objCustReport.IsStock
            };
            para[22] = new SqlParameter
            {
                ParameterName = "@Module",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.Module
            };
            para[23] = new SqlParameter
            {
                ParameterName = "@Condition",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.Condition
            };

            try
            {
                SqlHelper.ExecuteNonQuery(objCustReport.ConnConfig, CommandType.StoredProcedure, "spUpdateCustomerReportDetails", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //api
        public void UpdateCustomerReport(UpdateCustomerReportParam _UpdateCustomerReportParam, string ConnectionString)
        {
            var para = new SqlParameter[24];

            para[0] = new SqlParameter
            {
                ParameterName = "@RptId",
                SqlDbType = SqlDbType.Int,
                Value = _UpdateCustomerReportParam.ReportId
            };
            para[1] = new SqlParameter
            {
                ParameterName = "@ReportName",
                SqlDbType = SqlDbType.NVarChar,
                Value = _UpdateCustomerReportParam.ReportName
            };
            para[2] = new SqlParameter
            {
                ParameterName = "@ReportType",
                SqlDbType = SqlDbType.NVarChar,
                Value = _UpdateCustomerReportParam.ReportType
            };
            para[3] = new SqlParameter
            {
                ParameterName = "@UserId",
                SqlDbType = SqlDbType.Int,
                Value = _UpdateCustomerReportParam.UserId
            };
            para[4] = new SqlParameter
            {
                ParameterName = "@IsGlobal",
                SqlDbType = SqlDbType.Bit,
                Value = _UpdateCustomerReportParam.IsGlobal
            };
            para[5] = new SqlParameter
            {
                ParameterName = "@IsAscendingOrder",
                SqlDbType = SqlDbType.Bit,
                Value = _UpdateCustomerReportParam.IsAscending
            };
            para[6] = new SqlParameter
            {
                ParameterName = "@SortBy",
                SqlDbType = SqlDbType.NVarChar,
                Value = _UpdateCustomerReportParam.SortBy
            };
            para[7] = new SqlParameter
            {
                ParameterName = "@ColumnName",
                SqlDbType = SqlDbType.NVarChar,
                Value = _UpdateCustomerReportParam.ColumnName
            };
            para[8] = new SqlParameter
            {
                ParameterName = "@FilterColumns",
                SqlDbType = SqlDbType.NVarChar,
                Value = _UpdateCustomerReportParam.FilterColumns
            };
            para[9] = new SqlParameter
            {
                ParameterName = "@FilterValues",
                SqlDbType = SqlDbType.NVarChar,
                Value = _UpdateCustomerReportParam.FilterValues
            };
            para[10] = new SqlParameter
            {
                ParameterName = "@CompanyName",
                SqlDbType = SqlDbType.NVarChar,
                Value = _UpdateCustomerReportParam.CompanyName
            };
            para[11] = new SqlParameter
            {
                ParameterName = "@ReportTitle",
                SqlDbType = SqlDbType.NVarChar,
                Value = _UpdateCustomerReportParam.ReportTitle
            };
            para[12] = new SqlParameter
            {
                ParameterName = "@SubTitle",
                SqlDbType = SqlDbType.NVarChar,
                Value = _UpdateCustomerReportParam.SubTitle
            };
            para[13] = new SqlParameter
            {
                ParameterName = "@DatePrepared",
                SqlDbType = SqlDbType.NVarChar,
                Value = _UpdateCustomerReportParam.DatePrepared
            };
            para[14] = new SqlParameter
            {
                ParameterName = "@TimePrepared",
                SqlDbType = SqlDbType.Bit,
                Value = _UpdateCustomerReportParam.TimePrepared
            };
            para[15] = new SqlParameter
            {
                ParameterName = "@PageNumber",
                SqlDbType = SqlDbType.NVarChar,
                Value = _UpdateCustomerReportParam.PageNumber
            };
            para[16] = new SqlParameter
            {
                ParameterName = "@ExtraFooterLine",
                SqlDbType = SqlDbType.NVarChar,
                Value = _UpdateCustomerReportParam.ExtraFooterLine
            };
            para[17] = new SqlParameter
            {
                ParameterName = "@Alignment",
                SqlDbType = SqlDbType.NVarChar,
                Value = _UpdateCustomerReportParam.Alignment
            };
            para[18] = new SqlParameter
            {
                ParameterName = "@ColumnWidth",
                SqlDbType = SqlDbType.NVarChar,
                Value = _UpdateCustomerReportParam.ColumnWidth
            };
            para[19] = new SqlParameter
            {
                ParameterName = "@MainHeader",
                SqlDbType = SqlDbType.Bit,
                Value = _UpdateCustomerReportParam.MainHeader
            };
            para[20] = new SqlParameter
            {
                ParameterName = "@PDFSize",
                SqlDbType = SqlDbType.NVarChar,
                Value = _UpdateCustomerReportParam.PDFSize
            };
            para[21] = new SqlParameter
            {
                ParameterName = "@IsStock",
                SqlDbType = SqlDbType.Bit,
                Value = _UpdateCustomerReportParam.IsStock
            };
            para[22] = new SqlParameter
            {
                ParameterName = "@Module",
                SqlDbType = SqlDbType.NVarChar,
                Value = _UpdateCustomerReportParam.Module
            };
            para[23] = new SqlParameter
            {
                ParameterName = "@Condition",
                SqlDbType = SqlDbType.NVarChar,
                Value = _UpdateCustomerReportParam.Condition
            };

            try
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spUpdateCustomerReportDetails", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetReports(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select * from tblReports where UserId = " + objPropUser.UserID + " union select * from tblReports where IsGlobal = 'true'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetDynamicReports(User objPropUser ,string type)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select * from tblReports where UserId = " + objPropUser.UserID + " and ReportType = '" + type + "' union select * from tblReports where IsGlobal = 'true' and ReportType = '" + type + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //api
        public DataSet GetDynamicReports(GetDynamicReportsParam objPropUser, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select * from tblReports where UserId = " + objPropUser.UserID + " and ReportType = '" + objPropUser.Type + "' union select * from tblReports where IsGlobal = 'true' and ReportType = '" + objPropUser.Type + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetStockReports(User objPropUser)
        {
            try
            {
                //if (objPropUser.UserID == 0)
                //{
                    return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select * from tblReports where ReportType = '" + objPropUser.Type + "' and IsGlobal = 'true'");
                //}
                //else
                //{
                //    return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select * from tblReports where UserId = " + objPropUser.UserID + " and ReportType = '" + objPropUser.Type + "' and IsGlobal = 'true'");
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetStockReports(string ConnectionString, GetStockReportsParam _GetStockReportsParam)
        {
            try
            {
                //if (objPropUser.UserID == 0)
                //{
                return _GetStockReportsParam.DsUserAuthorization = SqlHelper.ExecuteDataset(_GetStockReportsParam.ConnConfig, CommandType.Text, "select * from tblReports where ReportType = '" + _GetStockReportsParam.Type + "' and IsGlobal = 'true'");
                //}
                //else
                //{
                //    return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select * from tblReports where UserId = " + objPropUser.UserID + " and ReportType = '" + objPropUser.Type + "' and IsGlobal = 'true'");
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetStockReports(GetStockReportsParam _GetStockReportsParam, string connectionString)
        {
            try
            {
                //if (objPropUser.UserID == 0)
                //{
                return SqlHelper.ExecuteDataset(connectionString, CommandType.Text, "select * from tblReports where ReportType = '" + _GetStockReportsParam.Type + "' and IsGlobal = 'true'");
                //}
                //else
                //{
                //    return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select * from tblReports where UserId = " + objPropUser.UserID + " and ReportType = '" + objPropUser.Type + "' and IsGlobal = 'true'");
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetMultipleStockReports(User objPropUser)
        {
            try
            {
                //if (objPropUser.UserID == 0)
                //{
                    return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select * from tblReports where ReportType in ( " + objPropUser.Type + ") and IsGlobal = 'true'");
                //}
                //else
                //{
                //    return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select * from tblReports where UserId = " + objPropUser.UserID + " and ReportType = '" + objPropUser.Type + "' and IsGlobal = 'true'");
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetMultipleStockReports(GetMultipleStockReportsParam _GetMultipleStockReports, string ConnectionString)
        {
            try
            {
                //if (objPropUser.UserID == 0)
                //{
                return _GetMultipleStockReports.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select * from tblReports where ReportType in ( " + _GetMultipleStockReports.Type + ") and IsGlobal = 'true'");
                //}
                //else
                //{
                //    return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select * from tblReports where UserId = " + objPropUser.UserID + " and ReportType = '" + objPropUser.Type + "' and IsGlobal = 'true'");
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetReportColByRepId(CustomerReport objCustReport)
        {
            try
            {
                return objCustReport.DsCustomer = SqlHelper.ExecuteDataset(objCustReport.ConnConfig, CommandType.Text, "select ColumnName, ColumnWidth from tblReportColumnsMapping where ReportId = " + objCustReport.ReportId + "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //api
        public DataSet GetReportColByRepId(GetReportColByRepIdParam objCustReport,string ConnectionString)
        {
            try
            {
                return objCustReport.DsCustomer = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select ColumnName from tblReportColumnsMapping where ReportId = " + objCustReport.ReportId + "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetReportFiltersByRepId(CustomerReport objCustReport)
        {
            try
            {
                return objCustReport.DsCustomer = SqlHelper.ExecuteDataset(objCustReport.ConnConfig, CommandType.Text, "select FilterColumn, FilterSet from tblReportFilters where ReportId = " + objCustReport.ReportId + "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //api
        public DataSet GetReportFiltersByRepId(GetReportFiltersByRepIdParam objCustReport,string ConnectionString)
        {
            try
            {
                return objCustReport.DsCustomer = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select FilterColumn, FilterSet from tblReportFilters where ReportId = " + objCustReport.ReportId + "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetOwners(string query, User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //api
        public DataSet GetOwners(GetOwnersParam _GetOwnersParam, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, _GetOwnersParam.query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetReportDetailById(CustomerReport objCustReport)
        {
            try
            {
                return objCustReport.DsCustomer = SqlHelper.ExecuteDataset(objCustReport.ConnConfig, CommandType.Text, "select * from tblReports where Id = " + objCustReport.ReportId + "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetReportDetailById(GetReportDetailByIdParam _GetReportDetailByIdParam, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select * from tblReports where Id = " + _GetReportDetailByIdParam.ReportId + "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //api

        public DataSet GetReportDetailById(CustomerReportParam objCustReport, string ConnectionString)
        {
            try
            {
                return objCustReport.DsCustomer = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select * from tblReports where Id = " + objCustReport.ReportId + "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetControlForReports(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select Name, Address, City, State, Zip, Phone, Fax, Email, WebAddress, Logo ,dbname from control");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetControlForReports(getConnectionConfigParam objPropUser,string ConnectionString)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select Name, Address, City, State, Zip, Phone, Fax, Email, WebAddress, Logo ,dbname from control");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //Payroll Menu
        public DataSet GetControlForPayroll(User objUser)
        {
            try
            {
                return objUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objUser.ConnConfig, CommandType.Text, "select PR from control");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetCustomerType(CustomerReport objCustReport)
        {
            try
            {
                return objCustReport.DsCustomer = SqlHelper.ExecuteDataset(objCustReport.ConnConfig, CommandType.Text, "select distinct Type from OType");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //api
        public DataSet GetCustomerType(GetCustomerTypeParam _GetCustomerType, string ConnectionString)
        {
            try
            {
                return _GetCustomerType.DsCustomer = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select distinct Type from OType");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetGroupedCustomersLocation(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetGroupedCustomersLocation", objPropUser.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCustomerName(CustomerReport objCustReport)
        {
            try
            {
                return objCustReport.DsCustomer = SqlHelper.ExecuteDataset(objCustReport.ConnConfig, CommandType.Text, "select distinct Name from CustomerReportDetails where Name != '' order by Name");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //api
        public DataSet GetCustomerName(GetCustomerNameParam objCustReport,string ConnectionString)
        {
            try
            {
                return objCustReport.DsCustomer = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select distinct Name from CustomerReportDetails where Name != '' order by Name");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCustomerAddress(CustomerReport objCustReport)
        {
            try
            {
                return objCustReport.DsCustomer = SqlHelper.ExecuteDataset(objCustReport.ConnConfig, CommandType.Text, "select distinct Address from CustomerReportDetails where Address != '' order by Address");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCustomerAddress(GetCustomerAddressParam objCustReport,string ConnectionString)
        {
            try
            {
                return objCustReport.DsCustomer = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select distinct Address from CustomerReportDetails where Address != '' order by Address");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCustomerCity(CustomerReport objCustReport)
        {
            try
            {
                return objCustReport.DsCustomer = SqlHelper.ExecuteDataset(objCustReport.ConnConfig, CommandType.Text, "select distinct City from CustomerReportDetails where City != '' order by City");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //api
        public DataSet GetCustomerCity(GetCustomerCityParam objCustReport,string ConnectionString)
        {
            try
            {
                return objCustReport.DsCustomer = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select distinct City from CustomerReportDetails where City != '' order by City");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetHeaderFooterDetail(CustomerReport objCustReport)
        {
            try
            {
                return objCustReport.DsCustomer = SqlHelper.ExecuteDataset(objCustReport.ConnConfig, CommandType.Text, "select * from tblReportHeaderFooterDetail where ReportId = " + objCustReport.ReportId + " ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //api
        public DataSet GetHeaderFooterDetail(GetHeaderFooterDetailParam objCustReport,string ConnectionString)
        {
            try
            {
                return objCustReport.DsCustomer = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select * from tblReportHeaderFooterDetail where ReportId = " + objCustReport.ReportId + " ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetColumnWidthByReportId(CustomerReport objCustReport)
        {
            try
            {
                return objCustReport.DsCustomer = SqlHelper.ExecuteDataset(objCustReport.ConnConfig, CommandType.Text, "select ColumnWidth from tblReportColumnsMapping where ReportId = " + objCustReport.ReportId + "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetColumnWidthByReportId(GetColumnWidthByReportIdParam objCustReport,string ConnectionString)
        {
            try
            {
                return objCustReport.DsCustomer = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, "select ColumnWidth from tblReportColumnsMapping where ReportId = " + objCustReport.ReportId + "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateCustomerReportResizedWidth(CustomerReport objCustReport)
        {
            var para = new SqlParameter[3];

            para[0] = new SqlParameter
            {
                ParameterName = "@ReportId",
                SqlDbType = SqlDbType.Int,
                Value = objCustReport.ReportId
            };
            para[1] = new SqlParameter
            {
                ParameterName = "@ColumnName",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.ColumnName
            };
            para[2] = new SqlParameter
            {
                ParameterName = "@ColumnWidth",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.ColumnWidth
            };

            try
            {
                SqlHelper.ExecuteNonQuery(objCustReport.ConnConfig, CommandType.StoredProcedure, "spUpdateCustReportResizedWidth", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void UpdateCustomerReportResizedWidth(UpdateCustomerReportResizedWidthParam objCustReport,string ConnectionString)
        {
            var para = new SqlParameter[3];

            para[0] = new SqlParameter
            {
                ParameterName = "@ReportId",
                SqlDbType = SqlDbType.Int,
                Value = objCustReport.ReportId
            };
            para[1] = new SqlParameter
            {
                ParameterName = "@ColumnName",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.ColumnName
            };
            para[2] = new SqlParameter
            {
                ParameterName = "@ColumnWidth",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.ColumnWidth
            };

            try
            {
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, "spUpdateCustReportResizedWidth", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCustReportFiltersValue(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetCustReportFiltersValue", objPropUser.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //api
        public DataSet GetCustReportFiltersValue(GetCustReportFiltersValueParam _GetCustReportFiltersValue, string ConnestionString)
        {
            try
            {
                return _GetCustReportFiltersValue.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnestionString, "spGetCustReportFiltersValue", _GetCustReportFiltersValue.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetDeliveryReportFiltersValue(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetDeliveryReportFiltersValue", objPropUser.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetDrawingsReportFiltersValue(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetDrawingsSentNotRcvdBackReportFiltersValue", objPropUser.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetReDrawingsReportFiltersValue(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spReDrawingsSubmittedForApprovalFiltersValue", objPropUser.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetApprovalReportFiltersValue(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetDCALocalApprovalNoPermitReportFiltersValue", objPropUser.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetOpenJobReportFiltersValue(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetOpenJobFiltersValue", objPropUser.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetInspectionReportFiltersValue(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetInspectedPaymentReportFiltersValue", objPropUser.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetEquipReportFiltersValue(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetEquipmentFiltersValue");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetEquipReportFiltersValue(GetEquipReportFiltersValueParam _GetEquipReportFiltersValue, string ConnectionString)
        {
            try
            {
                return _GetEquipReportFiltersValue.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, "spGetEquipmentFiltersValue");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetTicketReportFiltersValue(string SqlQuery,User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, SqlQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetRecReportFiltersValue(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetRecReportFiltersValue", objPropUser.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetEscalationReportFiltersValue(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetEscalateReportFiltersValue", objPropUser.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetEscalationReportFiltersValue(GetEscalationReportFiltersValueParam _GetEscalationReportFiltersValue, string ConnectionString)
        {
            try
            {
                return _GetEscalationReportFiltersValue.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, "spGetEscalateReportFiltersValue", _GetEscalationReportFiltersValue.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetVendorReportFiltersValue(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spVendorReportFiltersValue", objPropUser.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetEstimateReportFiltersValue(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetEstimateReportFiltersValue", objPropUser.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetLeadReportFiltersValue(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetLeadReportFiltersValue", objPropUser.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTaskReportFiltersValue(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetTaskReportFiltersValue", objPropUser.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetJournalReportFiltersValue(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetJournalEntryReportFiltersValue", objPropUser.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetOpportunityReportFiltersValue(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetOpportunityReportFiltersValue", objPropUser.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public DataSet GetBillReportFiltersValue(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetBillReportFiltersValue", objPropUser.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetBillReportFiltersValue(GetBillReportFiltersValueParam _GetBillReportFiltersValueParam, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, "spGetBillReportFiltersValue", _GetBillReportFiltersValueParam.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetRouteReportFiltersValue(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spRouteReportFiltersValue", objPropUser.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetInvoiceReportFiltersValue(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetInvoiceReportFiltersValue", objPropUser.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetProjectReportFiltersValue(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetJobProjectFiltersValue", objPropUser.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetUseTax(PJ objPJ)
        {
            try
            {               
                DataSet ds = new DataSet();
                StringBuilder varname1 = new StringBuilder();
                varname1.Append("Select PJ.fDate As PJfDate,PJ.Ref As PJRef,PJ.Batch As PJBatch, \n");
                varname1.Append(" PJItem.Amount As PJItemAmount,Rol.Name As RolName, \n");
                varname1.Append(" STax.Name As STaxName, STax.Rate As STaxRate, \n");
                varname1.Append(" Trans.Batch As TransBatch, Trans.fDate As TransfDate,Trans.fDesc As TransfDesc,Trans.Type As TransType, \n");
                varname1.Append(" Trans.Amount As TransAmount \n");
                varname1.Append(" from PJ, Trans, PJItem, STax, Vendor, Rol \n");
                varname1.Append(" WHERE PJ.Batch=Trans.Batch and Trans.Type='41' AND Trans.ID=PJItem.TRID and PJItem.Stax=STax.Name \n");
                varname1.Append(" and PJ.Vendor = Vendor.ID And Vendor.Rol = Rol.ID \n");
                varname1.Append(" And PJ.fDate >= '" + objPJ.StartDate + "' and \n");
                varname1.Append(" PJ.fDate <= '" + objPJ.EndDate + "'  \n");
                ds = SqlHelper.ExecuteDataset(objPJ.ConnConfig, CommandType.Text, varname1.ToString());
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetUseTax(GetUseTaxForReportsParam _GetUseTaxForReportsParam, string ConnectionString)
        {
            try
            {
                DataSet ds = new DataSet();
                StringBuilder varname1 = new StringBuilder();
                varname1.Append("Select PJ.fDate As PJfDate,PJ.Ref As PJRef,PJ.Batch As PJBatch, \n");
                varname1.Append(" PJItem.Amount As PJItemAmount,Rol.Name As RolName, \n");
                varname1.Append(" STax.Name As STaxName, STax.Rate As STaxRate, \n");
                varname1.Append(" Trans.Batch As TransBatch, Trans.fDate As TransfDate,Trans.fDesc As TransfDesc,Trans.Type As TransType, \n");
                varname1.Append(" Trans.Amount As TransAmount \n");
                varname1.Append(" from PJ, Trans, PJItem, STax, Vendor, Rol \n");
                varname1.Append(" WHERE PJ.Batch=Trans.Batch and Trans.Type='41' AND Trans.ID=PJItem.TRID and PJItem.Stax=STax.Name \n");
                varname1.Append(" and PJ.Vendor = Vendor.ID And Vendor.Rol = Rol.ID \n");
                varname1.Append(" And PJ.fDate >= '" + _GetUseTaxForReportsParam.StartDate + "' and \n");
                varname1.Append(" PJ.fDate <= '" + _GetUseTaxForReportsParam.EndDate + "'  \n");
                ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, varname1.ToString());
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetUTaxLocReport(Invoices objInvoice)
        {
            try
            {
                var para = new SqlParameter[2];

           
                para[0] = new SqlParameter
                {
                    ParameterName = "fromDate",
                    SqlDbType = SqlDbType.Date,
                    Value = objInvoice.StartDate
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "toDate",
                    SqlDbType = SqlDbType.Date,
                    Value = objInvoice.EndDate
                };
            
                return objInvoice.Ds = SqlHelper.ExecuteDataset(objInvoice.ConnConfig, CommandType.StoredProcedure, "spGetUTaxLocReport", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetUTaxLocReport(GetUTaxLocReportParam _GetUTaxLocReportParam, string ConnectionString)
        {
            try
            {
                var para = new SqlParameter[2];


                para[0] = new SqlParameter
                {
                    ParameterName = "fromDate",
                    SqlDbType = SqlDbType.Date,
                    Value = _GetUTaxLocReportParam.StartDate
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "toDate",
                    SqlDbType = SqlDbType.Date,
                    Value = _GetUTaxLocReportParam.EndDate
                };

                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetUTaxLocReport", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getRecurringDetailsTest(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetRecurringDetails");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetDeliveryDetails(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetDeliveryDetails");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetDrawingsDetails(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetDrawingsSentNotRcvdBackDetails");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetReDrawingsColumns(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetReDrawingsSubmittedForApproval");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetApprovalDetails(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetDCALocalApprovalNoPermitDetails");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetOpenJobDetails(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetOpenReportDetails");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetInspectedDetails(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetInspectedPaymentDetails");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetRecurringDetails(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spRecurringDetails");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetRecurringDetails(GetRecurringDetailsParam _GetRecurringDetails, string ConnectionString)
        {
            try
            {
                return _GetRecurringDetails.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spRecurringDetails");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetEscalationDetails(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetEscalateDetails");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetEscalationDetails(GetEscalationDetailsParam _GetEscalationDetails, string ConnectionString)
        {
            try
            {
                return _GetEscalationDetails.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetEscalateDetails");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetVendorDetails(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetVendorDetails");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetOpportunityDetails(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetOpportunityDetails");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetEstimateDetails(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetEstimateDetails");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetLeadDetails(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetLeadDetails");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTaskDetails(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetTaskDetails");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetJournalDetails(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetJournalEntryDetails");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetBillDetails(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetBillDetails");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetBillDetails(GetBillDetailsParam _GetBillDetailsParam, string ConnectionString)
        {
            try
            {
                return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetBillDetails");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetRouteDetails(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spRouteDetails");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetProjectDetails(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetJobProjectDetails");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetInvoiceDetails(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetInvoiceDetails");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       
        public DataSet getTicketDetails(User objPropUser,string query)
        {
            try
            {               
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        

        public DataSet GetTicketList(PJ objPJ)
        {
            try
            {
                DataSet ds = new DataSet();
                StringBuilder varname1 = new StringBuilder();
                varname1.Append("Select t.ID As TID,t.WorkOrder As TWorkOrder,t.CDate As TCDATE,t.DDate As TDDATE,t.EDate As TEDate, \n");
                varname1.Append("jt.Type As JType,w.fDesc As WfDesc,t.fDesc As TfDesc,t.Total As TTotal,pr.Reg As WIReg,     \n");
                varname1.Append(" t.Loc, r.Name as LocName   \n");
                varname1.Append("from TicketD t                                 \n");
                varname1.Append("Inner Join tblWork w On t.fWork = w.ID         \n");
                varname1.Append("Inner Join Emp e On t.fWork = e.fWork          \n");
                varname1.Append("Inner Join JobType jt On t.Type = jt.ID        \n");
                varname1.Append("Inner Join PRWage pw On t.WageC = pw.ID        \n");
                varname1.Append("Inner Join PRWageItem pr On (e.ID = pr.EMP And pw.GL = pr.GL And t.WageC =pr.Wage) \n");
                //varname1.Append("LEFT JOIN Job j ON j.ID = t.Job                \n");
                //varname1.Append("INNER JOIN Jobt jobt ON j.Template = jobt.ID   \n");
                //varname1.Append("LEFT JOIN PRWage pw1 ON pw1.ID = jobt.Wage     \n");
                varname1.Append("LEFT JOIN Loc l ON l.Loc = t.Loc                \n");
                varname1.Append("LEFT JOIN Rol r ON l.Rol = r.ID                \n");
                varname1.Append("WHERE t.EDate > '" + objPJ.StartDate + "' and  \n");
                varname1.Append(" t.EDate <= '" + objPJ.EndDate + "'            \n");
                //varname1.Append("Inner Join PRWage pw On t.WageC = pw.ID \n");

                ds = SqlHelper.ExecuteDataset(objPJ.ConnConfig, CommandType.Text, varname1.ToString());
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //api
        public DataSet GetTicketList(PJ objPJ,string ConnectionString)
        {
            try
            {
                DataSet ds = new DataSet();
                StringBuilder varname1 = new StringBuilder();
                varname1.Append("Select t.ID As TID,t.WorkOrder As TWorkOrder,t.CDate As TCDATE,t.DDate As TDDATE,t.EDate As TEDate, \n");
                varname1.Append("jt.Type As JType,w.fDesc As WfDesc,t.fDesc As TfDesc,t.Total As TTotal,pr.Reg As WIReg,     \n");
                varname1.Append(" t.Loc, r.Name as LocName   \n");
                varname1.Append("from TicketD t                                 \n");
                varname1.Append("Inner Join tblWork w On t.fWork = w.ID         \n");
                varname1.Append("Inner Join Emp e On t.fWork = e.fWork          \n");
                varname1.Append("Inner Join JobType jt On t.Type = jt.ID        \n");
                varname1.Append("Inner Join PRWage pw On t.WageC = pw.ID        \n");
                varname1.Append("Inner Join PRWageItem pr On (e.ID = pr.EMP And pw.GL = pr.GL And t.WageC =pr.Wage) \n");
                //varname1.Append("LEFT JOIN Job j ON j.ID = t.Job                \n");
                //varname1.Append("INNER JOIN Jobt jobt ON j.Template = jobt.ID   \n");
                //varname1.Append("LEFT JOIN PRWage pw1 ON pw1.ID = jobt.Wage     \n");
                varname1.Append("LEFT JOIN Loc l ON l.Loc = t.Loc                \n");
                varname1.Append("LEFT JOIN Rol r ON l.Rol = r.ID                \n");
                varname1.Append("WHERE t.EDate > '" + objPJ.StartDate + "' and  \n");
                varname1.Append(" t.EDate <= '" + objPJ.EndDate + "'            \n");
                //varname1.Append("Inner Join PRWage pw On t.WageC = pw.ID \n");

                ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, varname1.ToString());
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet getEquipmentInspection(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetEquipDetails");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet getEquipmentInspection(GetEquipmentInspectionParam _GetEquipmentInspection, string ConnectionString)
        {
            try
            {
                return _GetEquipmentInspection.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetEquipDetails");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetLocationReportFiltersValue(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spLocationFilterValue", objPropUser.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetLocationReportFiltersValue(GetLocationReportFiltersValueParam _GetLocationReportFiltersValue, string ConnectionString)
        {
            try
            {
                return _GetLocationReportFiltersValue.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, "spLocationFilterValue", _GetLocationReportFiltersValue.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetLocationDetails(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetLocationDetails");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //API
        public DataSet GetLocationDetails(GetLocationDetailsParam _GetLocationDetails, string ConnectionString)
        {
            try
            {
                return _GetLocationDetails.DsUserAuthorization = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "spGetLocationDetails");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetLocationReport(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetLocationsReport");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
