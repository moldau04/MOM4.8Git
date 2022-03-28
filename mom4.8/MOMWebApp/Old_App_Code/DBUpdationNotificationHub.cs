using Microsoft.AspNet.SignalR;

/// <summary>
/// Summary description for DBUpdationNotificationHub
/// </summary>
/// 
namespace MSWeb
{
    public class DBUpdationNotificationHub : Hub
    {
        public DBUpdationNotificationHub()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static void Send(string chartToUpdate)
        {
            //IN ORDER TO INVOKE SIGNALR FUNCTIONALITY DIRECTLY FROM SERVER SIDE WE MUST USE THIS
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<DBUpdationNotificationHub>();

            //PUSHING DATA TO ALL CLIENTS
            hubContext.Clients.All.Send(chartToUpdate);
        }
    }

    
}