
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

		RadioButton rbIncome, rbExpense;
		EditText etDate, etDesc, etTotalAmount;
		Spinner spnIncomeOrExpanseAccount, spnMoneyAccount, spnTaxRate;



		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Entry);

			// Get references to widgets
			rbIncome = FindViewById<RadioButton>(Resource.Id.rb_income);
			rbExpense = FindViewById<RadioButton>(Resource.Id.rb_expense);
			etDate = FindViewById<EditText>(Resource.Id.et_date);
			etDesc = FindViewById<EditText>(Resource.Id.et_description);
			spnIncomeOrExpanseAccount = FindViewById<Spinner>(Resource.Id.spn_income_expanse_account);
			spnMoneyAccount = FindViewById<Spinner>(Resource.Id.spn_money_account);
			etTotalAmount = FindViewById<EditText>(Resource.Id.et_total_amount);
			spnTaxRate = FindViewById<Spinner>(Resource.Id.spn_taxe_rate);
			var btnAddEntry = FindViewById<Button>(Resource.Id.btn_add_entry);

			// Populate spinner spnIncomeOrExpanseAccount with data...
			// Get income or expense accounts, depending on selected radio button
			var accounts = (rbIncome.Checked) ? manager.IncomeAccounts : manager.ExpenseAccounts;

			// Get data for the adapter
			var items = accounts.Select(a => a.ToString()).ToList();

			// Create adapter and bind data
			var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, items);

			// Feel and look, tror jag =)
			adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);

			// Bind adapter to spinner
			spnIncomeOrExpanseAccount.Adapter = adapter;


			// Populate spinner spnMoneyAccount with data...
			// Get data for the adapter
			items = manager.MoneyAccounts.Select(a => a.ToString()).ToList();

			// Create adapter and bind data
			adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, items);

			// Feel and look
			adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);

			// Bind adapter to spinner
			spnMoneyAccount.Adapter = adapter;


			// Populate spinner spnTaxRate with data...
			// Get data for the adapter
			items = manager.TaxRates.Select(a => a.ToString()).ToList();

			// Create adapter and bind data
			adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, items);

			// Feel and look
			adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);

			// Bind adapter to spinner
			spnTaxRate.Adapter = adapter;

			// Add listener for buttons
			rbIncome.Click += RadioButtonClick;
			rbExpense.Click += RadioButtonClick;
			btnAddEntry.Click += ButtonAddEntryClick;
		}

		void ButtonAddEntryClick(object sender, EventArgs e)
		{
			// Create model for entry
			var entry = new Entry();

			entry.Date = etDate.Text;
			entry.Description = etDesc.Text;
			entry.TotalAmount = etTotalAmount.Text;

			// Add income/expanse account
			string account = (string)spnIncomeOrExpanseAccount.SelectedItem;
			entry.setIncomeOrExpanseAccount(account);
			entry.IncomeAccount = rbIncome.Checked;
			entry.ExpanseAccount = rbExpense.Checked;

			// Add money account
			account = (string)spnMoneyAccount.SelectedItem;
			entry.setMoneyAccount(account);

			// Add tax rate
			string rate = (string)spnTaxRate.SelectedItem;
			entry.setTaxRate(rate);

			// Add entry to Entry list
			manager.AddEntry(entry);
		}

		private void RadioButtonClick(object sender, EventArgs e)
		{
			RadioButton rb = (RadioButton)sender;
			IList<string> items;

			// Get income or expense accounts, depending on radio button selected
			if (rb.Id == Resource.Id.rb_income)
			{
				// Get data for the adapter
				items = manager.IncomeAccounts.Select(a => a.ToString()).ToList();
			}
			else items = manager.ExpenseAccounts.Select(a => a.ToString()).ToList();
				
			// Create adapter and bind data
			var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, items);

			// Feel and look, tror jag =)
			adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);

			// Bind adapter to spinner
			spnIncomeOrExpanseAccount.Adapter = adapter;
		}
	}
}
