using System;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for SQLNotifier
/// </summary>
public class SQLNotifier : IDisposable
{
    public SqlCommand CurrentCommand { get; set; }
    private SqlConnection connection;
    public SqlConnection CurrentConnection
    {
        get
        {
            this.connection = this.connection ?? new SqlConnection(this.ConnectionString);
            return this.connection;
        }
    }
    private string _ConnectionString;
    public string ConnectionString
    {
        set
        {
            _ConnectionString = value;
        }
        get
        {
            return _ConnectionString;
        }
    }

    public SQLNotifier(string connectionString)
    {
        this.ConnectionString = connectionString;
        SqlDependency.Start(this.ConnectionString);

    }
    private event EventHandler<SqlNotificationEventArgs> _newMessage;

    public event EventHandler<SqlNotificationEventArgs> NewMessage
    {
        add
        {
            this._newMessage += value;
        }
        remove
        {
            this._newMessage -= value;
        }
    }

    public virtual void OnNewMessage(SqlNotificationEventArgs notification)
    {
        if (this._newMessage != null)
            this._newMessage(this, notification);
    }

    public DataTable RegisterDependency(string CommandToRegister)
    {
        this.CurrentCommand = new SqlCommand(CommandToRegister, this.CurrentConnection);
        this.CurrentCommand.Notification = null;
        
        SqlDependency dependency = new SqlDependency(this.CurrentCommand);
        dependency.OnChange += this.dependency_OnChange;
        
        if (this.CurrentConnection.State == ConnectionState.Closed)
            this.CurrentConnection.Open();
        try
        {

            DataTable dt = new DataTable();
            dt.Load(this.CurrentCommand.ExecuteReader(CommandBehavior.CloseConnection));
            return dt;
        }
        catch { return null; }

    }

    void dependency_OnChange(object sender, SqlNotificationEventArgs e)
    {
        SqlDependency dependency = sender as SqlDependency;

        dependency.OnChange -= new OnChangeEventHandler(dependency_OnChange);

        this.OnNewMessage(e);
    }
    
    #region IDisposable Members

    public void Dispose()
    {
        SqlDependency.Stop(this.ConnectionString);
    }

    #endregion
}