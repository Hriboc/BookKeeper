
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

namespace BookKeeper
{
	[Activity(Label = "EntryListActivity")]
	public class EntryListActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "EntryList" layout resource
			SetContentView(Resource.Layout.EntryList);

			var adapter = new EntriesAdapter(this);
			var lvEntries = FindViewById<ListView>(Resource.Id.lv_entries);
			lvEntries.Adapter = adapter;
		}
	}
}
