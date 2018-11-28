using System;
using SQLite;

namespace TidePredictorDataAccess_Library
{
    [Table("Tides")]
    public class TideDataModel
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Item { get; set; }   // this element holds the ones below
        public string Location { get; set; }
        public DateTime Date { get; set; }
        public string Day { get; set; }
        public string Time { get; set; }
        public string Height { get; set; }
        public string HI_LOW { get; set; }
    }
}
