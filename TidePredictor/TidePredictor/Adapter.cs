using System;
using System.Collections.Generic;
using System.Linq;
using Android.Views;
using Android.Widget;
using Android.App;

namespace TidePredictor
{
    public class Adapter : BaseAdapter<string>, ISectionIndexer
    {
        List<string> tideList;
        Activity context;

        //This is the constructor
        public Adapter(List<string> theTideList, Activity context)
        {
            this.tideList = theTideList;
            this.context = context;
            BuildSectionIndex();
        }

        //Code that overrides BaseAdapter methods follows

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count
        {
            get { return tideList.Count; }
        }

        public override string this[int position]
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
                view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = tideList[position].ToString();
            return view;
        }

        //ISectionIndexer implementation

        String[] sections;
        Java.Lang.Object[] sectionsObjects;
        Dictionary<string, int> alphaIndex;

        public int GetPositionForSection(int section)
        {
            return alphaIndex[sections[section]];
        }

        public int GetSectionForPosition(int position)
        {
            return 1;
        }

        public Java.Lang.Object[] GetSections()
        {
            return sectionsObjects;
        }

        private void BuildSectionIndex()
        {
            alphaIndex = new Dictionary<string, int>();
            for (var i = 0; i < tideList.Count; i++)
            {
                // Use the first character in the name as a key.
                var key = tideList[i].Substring(4,2);
                if (!alphaIndex.ContainsKey(key))
                {
                    alphaIndex.Add(key, i);
                }
            }

            sections = new string[alphaIndex.Keys.Count];
            alphaIndex.Keys.CopyTo(sections, 0);
            sectionsObjects = new Java.Lang.Object[sections.Length];
            for (var i = 0; i < sections.Length; i++)
            {
                sectionsObjects[i] = new Java.Lang.String(sections[i]);
            }
        }
        

    }
}