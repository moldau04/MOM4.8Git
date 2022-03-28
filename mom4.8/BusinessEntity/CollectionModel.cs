using System;

namespace BusinessEntity
{
    public class CollectionModel
    {
        public string ConnConfig { get; set; }

        public DateTime Date { get; set; }

        public int CustomDay { get; set; }

        public int OwnerID { get; set; }

        public String CustomerIDs { get; set; }

        public String LocationIDs { get; set; }

        public String DepartmentIDs { get; set; }

        public int EN { get; set; }

        public int UserID { get; set; }

        public string CustomerName { get; set; }

        public string LocationName { get; set; }

        public string PrintEmail { get; set; }

        public bool HidePartial { get; set; }
        public bool isDBTotalService { get; set; }
    }
    public class GetDashboardCollectionParam
    {
        public string ConnConfig { get; set; }

        public DateTime Date { get; set; }

        public int CustomDay { get; set; }

        public int OwnerID { get; set; }

        public String CustomerIDs { get; set; }

        public String LocationIDs { get; set; }

        public String DepartmentIDs { get; set; }

        public int EN { get; set; }

        public int UserID { get; set; }

        public string CustomerName { get; set; }

        public string LocationName { get; set; }

        public string PrintEmail { get; set; }

        public bool HidePartial { get; set; }
        public bool isDBTotalService { get; set; }
    }

    public class CollectionResponseModel
    {
        public string ThirtyDay { get; set; }
        public string SixtyDay { get; set; }
        public string NinetyDay { get; set; }
        public string OneTwentyDay { get; set; }
    }

    public class GetCollectionsParam
    {
        public string ConnConfig { get; set; }
        public DateTime Date { get; set; }
        public int CustomDay { get; set; }
        public String LocationIDs { get; set; }
        public String CustomerIDs { get; set; }
        public String DepartmentIDs { get; set; }
        public int EN { get; set; }
        public int UserID { get; set; }
        public string PrintEmail { get; set; }
        public bool HidePartial { get; set; }
        public bool isDBTotalService { get; set; }
    }

    public class GetGLAccountParam
    {
        public string ConnConfig { get; set; }
        public string acct { get; set; }
    }
}
