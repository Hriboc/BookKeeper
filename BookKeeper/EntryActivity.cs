
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
	[Activity(Label = "EntryActivity")]
	public class EntryActivity : Activity
	{
		BookkeeperManager manager = BookkeeperManager.Instance;

		Spinner spnIncomeOrExpanseAccount;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Entry);

			// Create your application here
			spnIncomeOrExpanseAccount = FindViewById<Spinner>(Resource.Id.spn_type);
			//Adapter adpIncomeOrExpanse = new ArrayAdapter(this,
		}
	}
}
