using System;
using System.Collections.Generic;
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
			entries = BookkeeperManager.Instance.GetEntries();
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
			etAmount.Text = string.Format("{0} kr",entries[position].TotalAmount);

			return view;
		}

	}
}
