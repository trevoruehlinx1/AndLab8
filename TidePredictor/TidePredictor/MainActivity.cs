using Android.App;
using Android.Widget;
using Android.OS;
using System.Data;
using Android.Support.V7.App;
using System;
using System.Linq;
using System.Collections.Generic;
using Android.Views;
using SQLite;
using System.IO;
using TidePredictorDataAccess_Library;
using Android.Content;

namespace TidePredictor
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : Activity
    {
        List<string> tideData;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            /* ------ copy and open the dB file using the SQLite-Net ORM ------ */

            string dbPath = "";
            SQLiteConnection db = null;

            // Get the path to the database that was deployed in Assets
            dbPath = Path.Combine(
                System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "tides.db3");

            // It seems you can read a file in Assets, but not write to it
            // so we'll copy our file to a read/write location
            using (Stream inStream = Assets.Open("tides.db3"))
            using (Stream outStream = File.Create(dbPath))
                inStream.CopyTo(outStream);

            // Open the database
            db = new SQLiteConnection(dbPath);

            /* ------ Spinner initialization ------ */

            // Initialize the adapter for the spinner with city names
            //List<string> locationNames = new List<string>();

            var distinctLocations = db.Table<TideDataModel>().GroupBy(t => t.Location).Select(t => t.First());
            var locationNames = distinctLocations.Select(t => t.Location).ToList();
            var adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem,locationNames);

            var locationSpinner = FindViewById<Spinner>(Resource.Id.citySpinner);
            locationSpinner.Adapter = adapter;

            // Event handler for selected spinner item
            string selectedLocation = "";
            locationSpinner.ItemSelected += delegate (object sender, AdapterView.ItemSelectedEventArgs e)
            {
                Spinner spinner = (Spinner)sender;
                selectedLocation = (string)spinner.GetItemAtPosition(e.Position);
            };

            /* ------- DatePicker initialization ------- */

            var datePicker = FindViewById<DatePicker>(Resource.Id.datePicker);

            TideDataModel selectedDatePredictioin =
                db.Get<TideDataModel>((from t in db.Table<TideDataModel>() select t).Min(s => s.ID));
            DateTime selectedDate = selectedDatePredictioin.Date;
            datePicker.DateTime = selectedDate;

            /* ------- Query for selected date and location -------- */

            Button submitButton = FindViewById<Button>(Resource.Id.button1 );
            ListView tidesListView = FindViewById<ListView>(Resource.Id.tideListView);
            submitButton.Click += delegate
            {
                DateTime date = datePicker.DateTime;
                string location = selectedLocation;
                var tides = (from t in db.Table<TideDataModel>()
                              where (t.Date == date)
                              && (t.Location == location)
                              select t).ToList();

                int count = tides.Count;
                string[] tidesArray = new string[count];
                for (int i = 0; i < count; i++)
                {
                    tidesArray[i] =
                        tides[i].Location +"\t\t" + tides[i].Date.ToShortDateString() + "\t\t" + tides[i].Day + "\t\t" + tides[i].Time +
                                           "\t\t" + Convert.ToDouble(tides[i].Height) * 12 + "in.\t\t" + tides[i].HI_LOW;
                }

                Intent intent = new Intent(this, typeof(SecondActivity));
                intent.PutExtra("array",tidesArray);
                StartActivity(intent);

            };
        }
    }
}

