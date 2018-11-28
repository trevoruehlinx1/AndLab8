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
using Android.Gms.Common.Apis;
using Android.Gms.Common;
using Android.Gms.Location;
using Android.Locations;
using Android.Support.Design.Widget;
using Android;
using Android.Content.PM;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Util;
using System.Threading.Tasks;

namespace TidePredictor
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : Activity, GoogleApiClient.IConnectionCallbacks,
        GoogleApiClient.IOnConnectionFailedListener,
        Android.Gms.Location.ILocationListener
    {

        List<string> tideData;
        TextView locationTextView;
        TextView conStatusTextView;
        GoogleApiClient apiClient;
        Location thisLocation;
        City closestCity2;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
           
            //Set content view
            SetContentView(Resource.Layout.activity_main);
            locationTextView = FindViewById<TextView>(Resource.Id.locationTextView);
            Button getLocationButton = FindViewById<Button>(Resource.Id.locationButton);
            Button viewNearestLocationButton = FindViewById<Button>(Resource.Id.NearestButton);
            viewNearestLocationButton.Enabled = false;
            Button viewNearLocationsTidesButton = FindViewById<Button>(Resource.Id.ViewNearestTidesButton);
            viewNearLocationsTidesButton.Enabled = false;
            Button lastLocationButton = FindViewById<Button>(Resource.Id.lastLocationButton);

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


            //SET UP GEOLOCATION WITH GOOGLE PLAY SERVICES
            apiClient = new GoogleApiClient.Builder(this, this, this)
                .AddApi(LocationServices.API).Build();
            apiClient.Connect();




            getLocationButton.Click += async delegate
            {
                viewNearestLocationButton.Enabled = true;
                if (apiClient.IsConnected)
                {
                    locationTextView = FindViewById<TextView>(Resource.Id.locationTextView);
                    locationTextView.Text = "Requesting Location Updates";
                    var locRequest = new LocationRequest();

                    // Setting location priority to PRIORITY_HIGH_ACCURACY (100)
                    locRequest.SetPriority(100);
                    await LocationServices.FusedLocationApi.RequestLocationUpdates(apiClient, locRequest, this);
                }
                else
                    locationTextView.Text = "Client API not connected";
            };



            viewNearestLocationButton.Click += delegate
            {
                viewNearLocationsTidesButton.Enabled = true;
                var locationTextView = FindViewById<TextView>(Resource.Id.locationTextView);
                City city = new City();
                List<City> cities = new List<City>();
                cities = city.AddCities();
                City closestCity = null;
                Location closestCityLocation = new Location("");
                var currentLocation = new Location(thisLocation);

                foreach (City c in cities)
                {
                    if (closestCity == null)
                    {
                        closestCity = c;
                        closestCityLocation = new Location("") { Longitude = closestCity.Longitude, Latitude = closestCity.Latitude };
                    }
                    else
                    {
                        Location nextLocation = new Location("") { Longitude = city.Longitude, Latitude = city.Latitude };
                        if (currentLocation.DistanceTo(nextLocation) < currentLocation.DistanceTo(closestCityLocation))
                        {
                            closestCity = city;
                            closestCityLocation.Longitude = closestCity.Longitude;
                            closestCityLocation.Latitude = closestCity.Latitude;
                        }
                    }
                };
                closestCity2 = closestCity;
                locationTextView.Text = "Your closest city is " + closestCity.Name;
            };

            
            viewNearLocationsTidesButton.Click += delegate
            {
                DateTime today = DateTime.Now;
                DateTime date = today.Date;
                string location = closestCity2.Name;
                var tides = (from t in db.Table<TideDataModel>()
                             where (t.Location == location)
                             && (t.Date == date)
                             select t).ToList();

                int count = tides.Count;
                string[] tidesArray = new string[count];
                for (int i = 0; i < count; i++)
                {
                    tidesArray[i] =
                        tides[i].Location + "\t\t" + tides[i].Date.ToShortDateString() + "\t\t" + tides[i].Day + "\t\t" + tides[i].Time +
                                           "\t\t" + Convert.ToDouble(tides[i].Height) * 12 + "in.\t\t" + tides[i].HI_LOW;
                }

                Intent intent = new Intent(this, typeof(SecondActivity));
                intent.PutExtra("array", tidesArray);
                StartActivity(intent);

            };

            //Get last location

            lastLocationButton.Click += delegate {
                Android.Locations.Location location = LocationServices.FusedLocationApi.GetLastLocation(apiClient);
                var locationTextView = FindViewById<TextView>(Resource.Id.locationTextView);
                DisplayLocation(location);
            };


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

        //Methods and Overrides for the interfaces
        public void OnConnected(Bundle connectionHint)
        {
            conStatusTextView = FindViewById<TextView>(Resource.Id.connectionStatusTextView);
            conStatusTextView.Text = "You are now connected";
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            conStatusTextView = FindViewById<TextView>(Resource.Id.connectionStatusTextView);
            conStatusTextView.Text = "Connection Failed!";
        }

        public void OnConnectionSuspended(int cause)
        {
            throw new NotImplementedException();
        }

        public void OnLocationChanged(Location location)
        {
            thisLocation = location;
            var locationTextView = FindViewById<TextView>(Resource.Id.locationTextView);
            locationTextView.Text = "Your Location:\n";
            DisplayLocation(location);
        }
        private void DisplayLocation(Android.Locations.Location location)
        {
            
            if (location != null)
            {
                locationTextView.Text += "Latitude: " + location.Latitude.ToString() + "\n";
                locationTextView.Text += "Longitude: " + location.Longitude.ToString() + "\n";
                locationTextView.Text += "Provider: " + location.Provider.ToString();
            }
            else
            {
                locationTextView.Text = "No location info available";
            }

        }
    }
}

