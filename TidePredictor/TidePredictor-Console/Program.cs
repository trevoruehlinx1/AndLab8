using System;
using TidePredictorDataAccess_Library;
using SQLite;
using System.IO;
using System.Collections.Generic;
using TidePredictor;

namespace ConsoleApp1
{
    class Program
    {
        static string currentDir;
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // Use a db file in the Android project's Assets folder
            currentDir = Directory.GetCurrentDirectory();
            // Console.WriteLine(currentDir);
            string dbPath = currentDir + @"/../../../../TidePredictor/Assets/tides.db3";
            var db = new SQLiteConnection(dbPath);

            // Create a Stocks table
            db.DropTable<TideDataModel>();
            if (db.CreateTable<TideDataModel>() == 0)
            {
                // A table already exixts, delete any data it contains
                db.DeleteAll<TideDataModel>();
            }
            AddTidesToDbFromXML(db, "Florence", "AnnualTidePredictions.xml");
            AddTidesToDbFromXML(db, "Charleston", "CharlestonTides.xml");
            AddTidesToDbFromXML(db, "Toledo", "ToledoTides.xml");
            AddTidesToDbFromXML(db, "Umpqua", "UmpquaTides.xml");

        }
        private static void AddTidesToDbFromXML(SQLiteConnection db, string location, string file)
        {
            //Add tides to the database
            XmlTideFileParser reader = new XmlTideFileParser(File.Open(currentDir + @"/../../../../TidePredictor/Assets/" + file , FileMode.Open));
            List<string> tideData = new List<string>();

            //Add the tidelist to the db file.
            int count = 0;
            reader.TideList.ForEach(tide =>
            {
                count++;
                db.Insert(new TideDataModel()
                {
                    //Item = tide["item"].ToString(),
                    Location = location,
                    Date = Convert.ToDateTime(tide["date"]),
                    Day = tide["day"].ToString(),
                    Time = tide["time"].ToString(),
                    Height = tide["pred_in_ft"].ToString(),
                    HI_LOW = tide["highlow"].ToString(),
                });
                Console.WriteLine(location);
            });

        }
    }
}
