using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Content;

namespace BookKeeper
{
	[Activity(Label = "BookKeeper", MainLauncher = true)]
	public class MainActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			Button btnNewEntry = FindViewById<Button>(Resource.Id.btn_new_entry);
			btnNewEntry.Click += delegate
			{
				StartActivity(typeof(EntryActivity));
			};

			Button btnShowAllEntries = FindViewById<Button>(Resource.Id.btn_show_all_entries);
			btnShowAllEntries.Click += delegate
			{
				StartActivity(typeof(EntryListActivity));
			};
		}
	}
}

