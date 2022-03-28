using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DataLayer;
using BusinessEntity;

namespace BusinessLayer
{
    public class BL_Budgets
    {
        DL_Budgets obj_budgets = new DL_Budgets();
        public DataSet GetIncomeStatement12PeriodForBudgets(Chart objChart)
        {
            return obj_budgets.GetIncomeStatement12PeriodForBudgets(objChart);
        }
        public int AddBudget(Budget budget)
        {
            return obj_budgets.AddBudget(budget);
        }
        public void UpdateBudgetName(Budget budget)
        {
            obj_budgets.UpdateBudgetName(budget);
        }
        public int AddAccount(Account account)
        {
            return obj_budgets.AddAccount(account);
        }
        public void AddAccountDetails(AccountDetail accountDetail)
        {
            obj_budgets.AddAccountDetails(accountDetail);
        }

        public void AddBudgetAccountDetails(AccountDetail accountDetail)
        {
            obj_budgets.AddBudgetAccountDetails(accountDetail);
        }

        public DataSet GetBudgetsData(string connString, string budgetName, string types)
        {
            return obj_budgets.GetBudgetsData(connString, budgetName, types);
        }

        public DataSet GetAccountID(string connString,string acct, string fDesc, string typeName)
        {
            return obj_budgets.GetAccountID(connString, acct, fDesc, typeName);
        }

        public int GetBudgetID(string connString, string budgetName)
        {
            return obj_budgets.GetBudgetID(connString, budgetName);
        }

        public DataSet GetBudget(string connString, int? year)
        {
            return obj_budgets.GetBudgets(connString, year);
        }

        public DataSet GetBudgetData(string connString, string budgetName)
        {
            return obj_budgets.GetBudgetData(connString, budgetName);
        }

        public DataSet GetSummaryBudgetData(Chart objChart, string budgetName)
        {
            return obj_budgets.GetSummaryBudgetData(objChart, budgetName);
        }

        public DataSet GetBudgetsByYear(string connString, int startDate, int endDate)
        {
            return obj_budgets.GetBudgetsByYear(connString, startDate, endDate);
        }

        public void DeleteBudget(string connString, string budgetID)
        {
            obj_budgets.DeleteBudget(connString, budgetID);
        }

        public DataSet GetTotalBudget(string connString, DateTime startDate, DateTime endDate, string budgetName)
        {
            return obj_budgets.GetTotalBudget(connString, startDate, endDate, budgetName);
        }

        public DataSet GetTotalActual(string connString, DateTime startDate, DateTime endDate)
        {
            return obj_budgets.GetTotalActual(connString, startDate, endDate);
        }

        public DataSet GetBudgetVSActual(string connString, DateTime startDate, DateTime endDate, string budgetName, bool includeZero)
        {
            return obj_budgets.GetBudgetVSActual(connString, startDate, endDate, budgetName, includeZero);
        }

        public DataSet Get12MonthBudgetVSActual(string connString, DateTime startDate, DateTime endDate, string budgetName, string centers)
        {
            return obj_budgets.Get12MonthBudgetVSActual(connString, startDate, endDate, budgetName, centers);
        }

        public DataSet GetChildAccounts(string connString, string acct)
        {
            return obj_budgets.GetChildAccounts(connString, acct);
        }

        public DataSet GetGLAccounts(string connString)
        {
            return obj_budgets.GetGLAccounts(connString);
        }

        public DataSet GetAllActiveAccounts(string connString)
        {
            return obj_budgets.GetAllActiveAccounts(connString);
        }

    }
}
