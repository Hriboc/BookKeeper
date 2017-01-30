
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
	[Activity(Label = "AccountsReportActivity")]
	public class AccountsReportActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "ReportsLayout" layout resource
			SetContentView(Resource.Layout.ReportsLayout);

			var etTaxReport = FindViewById<EditText>(Resource.Id.et_tax_report);
			etTaxReport.Text = BookkeeperManager.Instance.GetDetailedReportForAllAccounts();
		}
	}
}
