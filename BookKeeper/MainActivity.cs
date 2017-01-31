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
			SetContentView(Resource.Layout.MainLayout);

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

			Button btnShowReports = FindViewById<Button>(Resource.Id.btn_show_reports);
			btnShowReports.Click += delegate
			{
				StartActivity(typeof(CreateReportsActivity));
			};

			Button btnDeleteAllEntries = FindViewById<Button>(Resource.Id.btn_delete_all_entries);
			btnDeleteAllEntries.Click += delegate
			{
				BookkeeperManager.Instance.DeleteAllEntries();
				Toast.MakeText(this, Resource.String.toast_all_entries_deleted, ToastLength.Short).Show();
			};
		}
	}
}

