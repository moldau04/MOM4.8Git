namespace BusinessEntity
{
    public class Alerts
    {

        public string ConnConfig { get; set; }

        public string SearchValue { get; set; }

        public int loc { get; set; }
    }

    public class GetAlertTypeParam
    {
        public string ConnConfig { get; set; }
    }

    public class GetAlertsParam
    {
        public string ConnConfig { get; set; }
        public int loc { get; set; }
    }
}
