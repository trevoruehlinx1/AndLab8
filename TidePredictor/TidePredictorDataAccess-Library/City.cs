using System;
using System.Collections.Generic;
using System.Text;

namespace TidePredictorDataAccess_Library
{
    public class City
    {
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        private List<City> cities = new List<City>();



        public List<City> AddCities()
        {
            cities.Add(AddCity("Florence", -124.330117, 43.340111));
            cities.Add(AddCity("Charleston", -124.103142, 43.974659));
            cities.Add(AddCity("Toledo", -123.938197, 44.621688));
            cities.Add(AddCity("Umpqua", -124.198472, 43.662296));

            return cities;
        }
        public City AddCity(string name, double lon, double lat)
        {
            City city = new City { Name = name, Longitude = lon, Latitude = lat };
            return city;
        }
    }
}
