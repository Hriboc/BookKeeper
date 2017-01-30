
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
	[Activity(Label = "CreateReportsActivity")]
	public class CreateReportsActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "CreateReports" layout resource
			SetContentView(Resource.Layout.CreateReportsLayout);

			var btnTaxReport = FindViewById<Button>(Resource.Id.btn_tax_report);
			btnTaxReport.Click += delegate {
				StartActivity(typeof(TaxReportActivity));
			};

			var btnAccountsReport = FindViewById<Button>(Resource.Id.btn_accounts_report);
			btnAccountsReport.Click += delegate
			{
				StartActivity(typeof(AccountsReportActivity));
			};
		}
	}
}
