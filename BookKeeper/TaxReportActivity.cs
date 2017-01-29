
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
			SetContentView(Resource.Layout.TaxReport);

			var etTaxReport = FindViewById<EditText>(Resource.Id.et_tax_report);
			//etTaxReport.Text = BookkeeperManager.Instance.GetTaxReport();
		}
	}
}
