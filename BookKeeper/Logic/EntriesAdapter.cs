using System;
using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.Content;
using Android.Provider;
using Android.Views;
using Android.Widget;

namespace BookKeeper
{
	public class EntriesAdapter : BaseAdapter
	{
		IList<Entry> entries;
		Activity activity;

		public EntriesAdapter(Activity activity)
		{
			this.activity = activity;
			LoadDataSource();
		}

		public override int Count
		{
			get { return entries.Count; }
		}

		// Its not used
		public override Java.Lang.Object GetItem(int position)
		{
			// could wrap an entry in a Java.Lang.Object
			// to return it here if needed
			return null;
		}

		public override long GetItemId(int position)
		{
			return entries[position].Id;
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var view = convertView ?? activity.LayoutInflater.Inflate(
				Resource.Layout.EntryListItem, parent, false);

			var etDate = view.FindViewById<TextView>(Resource.Id.tv_item1);
			var etDesc = view.FindViewById<TextView>(Resource.Id.tv_item2);
			var etAmount = view.FindViewById<TextView>(Resource.Id.tv_item3);

			etDate.Text = entries[position].Date.ToShortDateString();
			etDesc.Text = entries[position].Description;

			string currency = activity.GetString(Resource.String.currency);
			etAmount.Text = string.Format("{0} {1}",entries[position].TotalAmount, currency);

			return view;
		}

		internal void LoadDataSource()
		{
			entries = BookkeeperManager.Instance
									   .GetEntries()
			                           .OrderBy(e => e.Date)
									   .ToList();
		}

	}
}
