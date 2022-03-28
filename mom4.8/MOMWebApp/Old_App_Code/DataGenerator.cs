using System;
using System.Data;

/// <summary>
/// Summary description for DataGenerator
/// </summary>
public class DataGenerator
{
    public static DataTable GetData()
    {
        DataTable dt;
        dt = new DataTable();
        dt.Columns.Add("start", typeof(DateTime));
        dt.Columns.Add("end", typeof(DateTime));
        dt.Columns.Add("name", typeof(string));
        dt.Columns.Add("id", typeof(string));
        dt.Columns.Add("column", typeof(string));
        dt.Columns.Add("allday", typeof(bool));
        dt.Columns.Add("color", typeof (string));

        DataRow dr;

        dr = dt.NewRow();
        dr["id"] = 1;
        dr["start"] = Convert.ToDateTime("15:00");
        dr["end"] = Convert.ToDateTime("15:00");
        dr["name"] = "Event 1 (backslash test: TEST\\jname)";
        dr["column"] = "1";
        dt.Rows.Add(dr);
        
        dr = dt.NewRow();
        dr["id"] = 2;
        dr["start"] = Convert.ToDateTime("16:00");
        dr["end"] = Convert.ToDateTime("17:00");
        dr["name"] = "Event 2";
        dr["column"] = "4";
        dr["color"] = "green";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 3;
        dr["start"] = Convert.ToDateTime("14:15").AddDays(1);
        dr["end"] = Convert.ToDateTime("18:45").AddDays(1);
        dr["name"] = "Event 3";
        dr["column"] = "5";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 4;
        dr["start"] = Convert.ToDateTime("16:30");
        dr["end"] = Convert.ToDateTime("17:30");
        dr["name"] = "Sales Dept. Meeting Once Again";
        dr["column"] = "6";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 5;
        dr["start"] = Convert.ToDateTime("8:00");
        dr["end"] = Convert.ToDateTime("9:00");
        dr["name"] = "Event 4";
        dr["column"] = "7";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 6;
        dr["start"] = Convert.ToDateTime("14:00");
        dr["end"] = Convert.ToDateTime("20:00");
        dr["name"] = "Event 6";
        dr["column"] = "4";
        dt.Rows.Add(dr);


        dr = dt.NewRow();
        dr["id"] = 7;
        dr["start"] = Convert.ToDateTime("11:00");
        dr["end"] = Convert.ToDateTime("13:14");
        dr["name"] = "Unicode test: 公曆 (requires Unicode fonts on the client side)";
        dr["color"] =  "orange";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 8;
        dr["start"] = Convert.ToDateTime("13:14").AddDays(-1);
        dr["end"] = Convert.ToDateTime("14:05").AddDays(-1);
        dr["name"] = "Event 8";
        dr["column"] = "4";
        dr["color"] = "green";
        dt.Rows.Add(dr);  
        
        
        dr = dt.NewRow();
        dr["id"] = 9;
        dr["start"] = Convert.ToDateTime("13:14").AddDays(7);
        dr["end"] = Convert.ToDateTime("14:05").AddDays(7);
        dr["name"] = "Event 9";
        dr["column"] = "4";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 10;
        dr["start"] = Convert.ToDateTime("13:14").AddDays(-7);
        dr["end"] = Convert.ToDateTime("14:05").AddDays(-7);
        dr["name"] = "Event 10";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 11;
        dr["start"] = Convert.ToDateTime("00:00").AddDays(8);
        dr["end"] = Convert.ToDateTime("00:00").AddDays(15);
        dr["name"] = "Event 11";
        dr["column"] = "4";
        dr["allday"] = true;
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 12;
        dr["start"] = Convert.ToDateTime("00:00").AddDays(-2);
        dr["end"] = Convert.ToDateTime("00:00").AddDays(-1);
        dr["name"] = "Event 12";
        dr["column"] = "5";
        dr["allday"] = true;
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 13;
        dr["start"] = DateTime.Now.AddDays(-7);
        dr["end"] = DateTime.Now.AddDays(14);
        dr["name"] = "Event 13";
        dr["column"] = "7";
        dr["allday"] = true;
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 14;
        dr["start"] = Convert.ToDateTime("7:45:00");
        dr["end"] = Convert.ToDateTime("8:30:00");
        dr["name"] = "Event 14";
        dr["column"] = "5";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 15;
        dr["start"] = Convert.ToDateTime("23:30:00");
        dr["end"] = Convert.ToDateTime("00:15:00").AddDays(2);
        dr["name"] = "Event 15";
        dr["column"] = "6";
        dt.Rows.Add(dr);
        
        dr = dt.NewRow();
        dr["id"] = 16;
        dr["start"] = Convert.ToDateTime("8:30:00").AddDays(1);
        dr["end"] = Convert.ToDateTime("9:00:00").AddDays(1);
        dr["name"] = "Event 16";
        dr["column"] = "5";
        dt.Rows.Add(dr);


        dr = dt.NewRow();
        dr["id"] = 17;
        dr["start"] = Convert.ToDateTime("8:00:00").AddDays(1);
        dr["end"] = Convert.ToDateTime("8:00:01").AddDays(1);
        dr["name"] = "Event 17";
        dr["column"] = "7";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 18;
        dr["start"] = Convert.ToDateTime("8:00:00").AddDays(1);
        dr["end"] = Convert.ToDateTime("8:00:01").AddDays(1);
        dr["name"] = "tttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttttt";
        dr["column"] = "5";
        dr["color"] = "yellow";

        dt.Rows.Add(dr);


        for (int i = 0; i < 10; i++ )
        {
            dr = dt.NewRow();
            dr["id"] = 1000 + i;
            dr["start"] = Convert.ToDateTime("2009-12-30T08:00:00");
            dr["end"] = Convert.ToDateTime("2009-12-30T19:00:00");
            dr["name"] = "Event " + (1000 + i);
            dr["column"] = "D";
            dt.Rows.Add(dr);
            
        }

        dt.PrimaryKey = new DataColumn[] { dt.Columns["id"] };

        return dt;

    }
    
    public static DataTable GetDataLarge()
    {
        DataTable dt;
        dt = new DataTable();
        dt.Columns.Add("start", typeof(DateTime));
        dt.Columns.Add("end", typeof(DateTime));
        dt.Columns.Add("name", typeof(string));
        dt.Columns.Add("id", typeof(string));
        dt.Columns.Add("column", typeof(string));
        dt.Columns.Add("allday", typeof(bool));

        DataRow dr;

        for (int i = 0; i < 300; i++ )
        {
            dr = dt.NewRow();
            dr["id"] = i + 1000;
            dr["start"] = Convert.ToDateTime("15:50").AddDays(i);
            dr["end"] = Convert.ToDateTime("15:50").AddDays(i);
            dr["name"] = "Event 1";
            dr["column"] = "D";
            dt.Rows.Add(dr);
        }

        for (int i = 0; i < 300; i++ )
        {
            dr = dt.NewRow();
            dr["id"] = i + 2000;
            dr["start"] = Convert.ToDateTime("15:50").AddDays(i);
            dr["end"] = Convert.ToDateTime("15:50").AddDays(i);
            dr["name"] = "Event 1";
            dr["column"] = "G";
            dt.Rows.Add(dr);
        }

        for (int i = 0; i < 300; i++ )
        {
            dr = dt.NewRow();
            dr["id"] = i + 3000;
            dr["start"] = Convert.ToDateTime("15:50").AddDays(i);
            dr["end"] = Convert.ToDateTime("15:50").AddDays(i);
            dr["name"] = "Event 1";
            dr["column"] = "I";
            dt.Rows.Add(dr);
        }

        dr = dt.NewRow();
        dr["id"] = 2;
        dr["start"] = Convert.ToDateTime("16:00");
        dr["end"] = Convert.ToDateTime("17:00");
        dr["name"] = "Event 2";
        dr["column"] = "A";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 3;
        dr["start"] = Convert.ToDateTime("14:15").AddDays(1);
        dr["end"] = Convert.ToDateTime("18:45").AddDays(1);
        dr["name"] = "Event 3";
        dr["column"] = "A";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 4;
        dr["start"] = Convert.ToDateTime("16:30");
        dr["end"] = Convert.ToDateTime("17:30");
        dr["name"] = "Sales Dept. Meeting Once Again";
        dr["column"] = "B";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 5;
        dr["start"] = Convert.ToDateTime("8:00");
        dr["end"] = Convert.ToDateTime("9:00");
        dr["name"] = "Event4asdfasdfasdfadfasdfasdf";
        dr["column"] = "B";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 6;
        dr["start"] = Convert.ToDateTime("14:00");
        dr["end"] = Convert.ToDateTime("10:00").AddDays(1);
        dr["name"] = "Event 6";
        dr["column"] = "C";
        dt.Rows.Add(dr);


        dr = dt.NewRow();
        dr["id"] = 7;
        dr["start"] = Convert.ToDateTime("11:00");
        dr["end"] = Convert.ToDateTime("13:14");
        dr["name"] = "Unicode test: 公曆";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 8;
        dr["start"] = Convert.ToDateTime("13:14").AddDays(-1);
        dr["end"] = Convert.ToDateTime("14:05").AddDays(-1);
        dr["name"] = "Event 8";
        dr["column"] = "C";
        dt.Rows.Add(dr);  
        
        
        dr = dt.NewRow();
        dr["id"] = 9;
        dr["start"] = Convert.ToDateTime("13:14").AddDays(7);
        dr["end"] = Convert.ToDateTime("14:05").AddDays(7);
        dr["name"] = "Event 9";
        dr["column"] = "C";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 10;
        dr["start"] = Convert.ToDateTime("13:14").AddDays(-7);
        dr["end"] = Convert.ToDateTime("14:05").AddDays(-7);
        dr["name"] = "Event 10";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 11;
        dr["start"] = Convert.ToDateTime("00:00").AddDays(8);
        dr["end"] = Convert.ToDateTime("00:00").AddDays(15);
        dr["name"] = "Event 11";
        dr["column"] = "D";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 12;
        dr["start"] = Convert.ToDateTime("00:00");
        dr["end"] = Convert.ToDateTime("00:00").AddDays(1);
        dr["name"] = "Event 12";
        dr["column"] = "D";
        dr["allday"] = true;
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 13;
        dr["start"] = DateTime.Now;
        dr["end"] = DateTime.Now.AddDays(1);
        dr["name"] = "Event 13.";
        dr["column"] = "B";
        dr["allday"] = true;
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 14;
        dr["start"] = Convert.ToDateTime("7:45:00");
        dr["end"] = Convert.ToDateTime("8:30:00");
        dr["name"] = "Event 14";
        dr["column"] = "D";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr["id"] = 15;
        dr["start"] = Convert.ToDateTime("23:30:00");
        dr["end"] = Convert.ToDateTime("00:15:00").AddDays(1);
        dr["name"] = "Event 15";
        dr["column"] = "D";
        dt.Rows.Add(dr);
        
        dr = dt.NewRow();
        dr["id"] = 16;
        dr["start"] = Convert.ToDateTime("8:30:00").AddDays(1);
        dr["end"] = Convert.ToDateTime("9:00:00").AddDays(3);
        dr["name"] = "Event 16";
        dr["column"] = "D";
        dt.Rows.Add(dr);


        dr = dt.NewRow();
        dr["id"] = 17;
        dr["start"] = Convert.ToDateTime("8:00:00").AddDays(1);
        dr["end"] = Convert.ToDateTime("8:00:01").AddDays(1);
        dr["name"] = "Event 17";
        dr["column"] = "D";
        dt.Rows.Add(dr);

        dt.PrimaryKey = new DataColumn[] { dt.Columns["id"] };

        return dt;

    }
}
