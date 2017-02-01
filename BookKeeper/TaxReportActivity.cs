
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
	[Activity(Label = "TaxReportActivity")]
	public class TaxReportActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "TaxReport" layout resource
			SetContentView(Resource.Layout.ReportsLayout);

			var etReport = FindViewById<TextView>(Resource.Id.tv_report);
			etReport.MovementMethod = new Android.Text.Method.ScrollingMovementMethod();
			etReport.Text = BookkeeperManager.Instance.GetTaxReport();
		}
	}
}
