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
			FillEntries();
		}

		void FillEntries()
		{
			/*
			var uri = ContactsContract.Contacts.ContentUri;

			string[] projection = {
				ContactsContract.Contacts.InterfaceConsts.Id,
				ContactsContract.Contacts.InterfaceConsts.DisplayName,
				ContactsContract.Contacts.InterfaceConsts.PhotoId
			};

			var cursor = activity.Q(uri, projection, null,
				null, null);
			*/
			entries = BookkeeperManager.Instance.GetEntries();
			/*
			if (cursor.MoveToFirst())
			{
				do
				{
					_contactList.Add(new Contact
					{
						Id = cursor.GetLong(
					cursor.GetColumnIndex(projection[0])),
						DisplayName = cursor.GetString(
					cursor.GetColumnIndex(projection[1])),
						PhotoId = cursor.GetString(
					cursor.GetColumnIndex(projection[2]))
					});
				} while (cursor.MoveToNext());
			}
			*/
		}

		public override int Count
		{
			get { return entries.Count; }
		}

		public override Java.Lang.Object GetItem(int position)
		{
			// could wrap an Entry in a Java.Lang.Object
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

			var date = view.FindViewById<TextView>(Resource.Id.tv_item1);
			var description = view.FindViewById<TextView>(Resource.Id.tv_item2);
			var amount = view.FindViewById<TextView>(Resource.Id.tv_item3);

			date.Text = entries[position].Date;
			description.Text = entries[position].Description;
			amount.Text = entries[position].TotalAmount + " kr";

			return view;
		}

	}
}
