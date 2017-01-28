
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

			lvEntries.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
			{
				// int id = (int) lvEntries.GetItemIdAtPosition(e.Position);
				int id = (int)lvEntries.Adapter.GetItemId(e.Position);

				// Bättre att skicka med Id:t till entryactivity och hämna entry där
				// Entry entry = BookkeeperManager.Instance.GetEntry(id);

				Intent entryActivity = new Intent(this, typeof(EntryActivity));
				entryActivity.PutExtra("entryId", id);
				StartActivity(entryActivity);
			};
		}
	}
}
