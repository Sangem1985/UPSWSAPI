namespace UPSWSWebAPIs.Models
{
    public class MasterModel
    {
        public class Countries
        {
            public string CountryId { get; set; }
            public string CountryName { get; set; }
        }
        public class State
        {
            public string StateId { get; set; }
            public string StateName { get; set; }
        }
        public class    District
        {
            public string districtcode { get; set; }
            public string districtname { get; set; }
        }
        public class Mandal
        {
            public string MandalId { get; set; }
            public string MandalName { get; set; }
        }
        public class Village
        {
            public string VillageId { get; set; }
            public string VillageName { get; set; }
        }

    }
}
