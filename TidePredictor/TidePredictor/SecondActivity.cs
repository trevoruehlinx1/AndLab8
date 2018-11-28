using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TidePredictor
{
    [Activity(Label = "SecondActivity")]
    public class SecondActivity : ListActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            string[] tideData = Intent.GetStringArrayExtra("array");

            // Create your application here
            ListAdapter = new ArrayAdapter<string>(this, Resource.Layout.SimpleListItem1, tideData.ToArray());

            ListView.TextFilterEnabled = true;

            ListView.FastScrollEnabled = true;
        }
    }
}