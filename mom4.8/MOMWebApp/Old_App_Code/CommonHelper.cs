using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Data;
using BusinessEntity;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for CommonHelper
/// </summary>
public class CommonHelper
{
    //public DateTime GetDataByFrequency(DateTime _date, int _frequencyId)
    //{
    //    DateTime _processedDate;
    //    switch (_frequencyId)
    //    {
    //        case 0:  //Monthly
    //            _processedDate = _date.AddMonths(1);
    //            break;
    //        case 1:  //BiMonthly
    //            _processedDate = _date.AddMonths(2); // 2 months
    //            break;
    //        case 2:  //Quarterly
    //            _processedDate = _date.AddMonths(3); // 3 months
    //            break;
    //        case 3:  //3 Times A Year
    //            //_processedDate = 
    //            System.DateTime.IsLeapYear(System.DateTime.Now.Year);
    //            if (System.DateTime.IsLeapYear(System.DateTime.Now.Year)) //366 days
    //            {
    //                int _days = 366;
    //                int _numDays = _days / 3;
    //                DateTime _date1 = _date.AddDays(_numDays);
    //                DateTime _date2 = _date1.AddDays(_numDays);
    //                DateTime _date3 = _date2.AddDays(_numDays);

    //                if(_date.Date <= _date1.Date)
    //                {
    //                    return _date1;
    //                }
    //                else if(_date1.Date <= _date2.Date)
    //                {
    //                    return _date2;
    //                }
    //                else if(_date2.Date <= _date3)
    //                {
    //                    return _date2;
    //                }
    //            }
    //            break;
    //        case 4:  //SemiAnnually
    //            _processedDate = _date.AddMonths(6); // 6 months
    //            break;
    //        case 5:  //Annually
    //            _processedDate = _date.AddYears(1);
    //            break;
    //        case 6:  //Weekly
    //            _processedDate = _date.AddDays(7); // 1 week (1 week = 7 days)
    //            break;
        

    //        default:
    //            throw new ArgumentException("Payment frequency is not initialized to valid value!", "paymentFrequency");
    //    }
    //    return _processedDate;
    //}
    public static bool GetPeriodDetails(DateTime _transDate)
    {
        bool _flag = false;
        if (HttpContext.Current.Session["PeriodClose"] != null)
        {
            DataTable _dt = (DataTable)HttpContext.Current.Session["PeriodClose"];
            if (_dt.Rows.Count > 0)
            {
                if ((!string.IsNullOrEmpty(_dt.Rows[0]["fStart"].ToString())) && (!string.IsNullOrEmpty(_dt.Rows[0]["fEnd"].ToString())))
                {
                    DateTime _startDate = Convert.ToDateTime(_dt.Rows[0]["fStart"]);
                    DateTime _endDate = Convert.ToDateTime(_dt.Rows[0]["fEnd"]);

                    if (_startDate <= _transDate && _endDate >= _transDate)
                    {
                        _flag = true;
                    }
                }
                else
                {
                    _flag = true;
                }
            }
        }
        return _flag;
    }
    public enum Months
    {
        January = 0,
        February = 1,
        March = 2,
        April = 3,
        May = 4,
        June = 5,
        July = 6,
        August = 7,
        September = 8,
        October = 9,
        November = 10,
        December = 11
    }
    public enum Tables
    {
        Trans,
        Rol
    }
    public enum BankReconItem
     {
         Journal = 0,
         Deposit = 1,
         Check = 2
     }

    public static List<YearEndMonth> lstMonths = new List<YearEndMonth>
    {
        new YearEndMonth {ID = 0, Month = "January"},
        new YearEndMonth {ID = 1, Month = "February"},
        new YearEndMonth {ID = 2, Month = "March"},
        new YearEndMonth {ID = 3, Month = "April"},
        new YearEndMonth {ID = 4, Month = "May"},
        new YearEndMonth {ID = 5, Month = "June"},
        new YearEndMonth {ID = 6, Month = "July"},
        new YearEndMonth {ID = 6, Month = "July"}
    };

    public static List<YearEndMonth> GetAll()
    {
        return lstMonths.ToList();
    }

    /// <summary>
    /// CustomField contains fixed custom field control's ID. 
    /// </summary>
    public enum CustomField     
    {
        Currency = 1,
        Date = 2,
        Text = 3,
        Dropdown = 4,
        Checkbox = 5
    }
    public enum CustomFieldFormat
    {
        Currency = 1,
        Date = 2,
        Text = 3,
        Dropdown = 4,
        Checkbox = 5,
        Notes = 6,
        CheckboxWithComment = 7
    }
}
public static class EnumExtensions
{
    public static string Description(this Enum value)
    {
        var enumType = value.GetType();
        var field = enumType.GetField(value.ToString());
        var attributes = field.GetCustomAttributes(typeof(DisplayAttribute),
                                                    false);
        return attributes.Length == 0
            ? value.ToString()
            : ((DisplayAttribute)attributes[0]).Description;
    }

}
public static class TypeHelper
{
    public static List<TypeName> ListTypes = new List<TypeName>
        {
            new TypeName{ID=1,Name="GLAdj"},
            new TypeName{ID=2,Name="AP Item"},
            new TypeName{ID=3,Name="Check"},
            new TypeName{ID=4,Name="Deposit"},
            new TypeName{ID=5,Name="Payment"},
            new TypeName{ID=6,Name="Invoice"},
            new TypeName{ID=7,Name="Bank Adj"},
            new TypeName{ID=8,Name="Bill"},    
            new TypeName{ID=9,Name="Recevied Payment"}
        };
    public static TypeName GetByID(int _ID)
    {
           return ListTypes.Single(t => t.ID == _ID);
    }
    public static TypeName GetGLAj()
    {
        return ListTypes.Single(t => t.ID == 1);        
    }
    public static TypeName GetAPItem()
    {
        return ListTypes.Single(t => t.ID == 2);
    }
    public static TypeName GetCheck()
    {
        return ListTypes.Single(t => t.ID == 3);
    }
    public static TypeName GetDeposit()
    {
        return ListTypes.Single(t => t.ID == 4);
    }
    public static TypeName GetPayment()
    {
        return ListTypes.Single(t => t.ID == 5);
    }
    public static TypeName GetBill()
    {
        return ListTypes.Single(t => t.ID == 8);
    }
    public static TypeName GetReceviedPayment()
    {
        return ListTypes.Single(t => t.ID == 9);
    }
    public static TypeName GetBankAdj()
    {
        return ListTypes.Single(t => t.ID == 7);
    }
}
public static class FrequencyHelper
{
    public static List<Frequency> Frequencies = new List<Frequency>
    {
        new Frequency {ID = 0, Name = "Monthly"},
        new Frequency {ID = 1, Name = "Bi-Monthly"},
        new Frequency {ID = 2, Name = "Quarterly"},
        new Frequency {ID = 3, Name = "3 Times A Year"},
        new Frequency {ID = 4, Name = "Semi-Annually"},
        new Frequency {ID = 5, Name = "Annually"},
        new Frequency {ID = 6, Name = "Weekly"}
    };

    public static Frequency GetById(int _id)
    {
        return Frequencies.Single(f => f.ID == _id);
    }
    public static List<Frequency> GetAll()
    {
        return Frequencies.ToList();
    }
    public static DateTime GetRecurProcessDate(DateTime _date, int _frequency)
    {
        switch (_frequency)
        {
            case 0:                         //Monthly
                _date = _date.AddMonths(1); //1 Month
                break;
            case 1:                         //Bi-Monthly
                _date = _date.AddMonths(2); //2 Months
                break;
            case 2:                         //Quarterly
                _date = _date.AddMonths(3); //3 Months
                break;
            case 3:                         //3 Times A Year
                _date = _date.AddMonths(4); //4 Months
                break;
            case 4:                         //Semi-Annually
                _date = _date.AddMonths(6); //6 Months
                break;
            case 5:                         //Annually
                _date = _date.AddYears(1);  //1 Year
                break;
            case 6:                         //Weekly
                _date = _date.AddDays(7);   //7 Days
                break;
          
        }
        return _date;

    }
}
public static class APStatusHelper
{
    public static List<APStatus> listAPStatus = new List<APStatus>
    {
        new APStatus {ID = 0, Name = "Input Only"},
        new APStatus {ID = 1, Name = "Hold - No Invoices"},
        new APStatus {ID = 2, Name = "Hold - No Materials"},
        new APStatus {ID = 3, Name = "Hold - Other"},
        new APStatus {ID = 4, Name = "Verified"},
        new APStatus {ID = 5, Name = "Selected"}
    };
    public static APStatus GetById(int _id)
    {
        return listAPStatus.Single(s => s.ID == _id);
    }
    public static List<APStatus> GetAll()
    {
        return listAPStatus.ToList();
    }
}
public static class ContractBilling
{
    public static List<ContractBill> listBill = new List<ContractBill>
    {
        new ContractBill {ID = 0, Name = "Separate per Contract"},
        new ContractBill {ID = 1, Name = "Combined on One Invoice"}
    };
    public static List<ContractBill> GetAll()
    {
        return listBill.ToList();
    }
}
//public static class PhaseHelper
//{
//    public static List<Phase> Phases = new List<Phase>
//    {
//        new Phase {ID = 1, Description = "Labor"},
//        new Phase {ID = 2, Description = "Materials"},
//        new Phase {ID = 3, Description = "Shipping"},
//        new Phase {ID = 4, Description = "Permits"},
//        new Phase {ID = 5, Description = "Mileage"},
//        new Phase {ID = 6, Description = "NIS"}
//    };
//    public static Phase GetById(int _id)
//    {
//        return Phases.Single(f => f.ID == _id);
//    }
//    public static List<Phase> GetAll()
//    {
//        return Phases.ToList();
//    }
//}
public class CustomTextBox : TextBox
{
    protected override void LoadViewState(object savedState)
    {
        TrackViewState();
        base.LoadViewState(savedState);
    }
    protected override object SaveViewState()
    {
        TrackViewState();
        return base.SaveViewState();
    }
    //public string Text
    //{
    //    get
    //    {
    //        //return (string)(ViewState["Value"] ?? (ViewState["Value"] = string.Empty));
    //        //return (string)ViewState["Text"];
    //        return ViewState["Text"] == null ?
    //        "" :
    //         (string)ViewState["Text"];
    //    }
    //    set
    //    {
    //        ViewState["Text"] = value;
    //        //ViewState["Value"] = value;
    //    }
    //}
}
public class CustomCheckBox : CheckBox
{
    protected override void LoadViewState(object savedState)
    {
        base.LoadViewState(savedState);
    }
    protected override object SaveViewState()
    {
        return base.SaveViewState();
    }
}
public class CustomDropDownList : DropDownList
{
    protected override void LoadViewState(object savedState)
    {
        base.LoadViewState(savedState);
    }
    protected override object SaveViewState()
    {
        return base.SaveViewState();
    }
}
public static class DateTimeExtensions
{
    public static DateTime StartOfWeek(this DateTime today, DayOfWeek startOfWeek)
    {
        int diff = today.DayOfWeek - startOfWeek;
        if (diff < 0)
        {
            diff += 7;
        }
        return today.AddDays(-1 * diff).Date;
    }
}