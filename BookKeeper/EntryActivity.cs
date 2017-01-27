using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;

namespace BookKeeper
{
	[Activity(Label = "EntryActivity")]
	public class EntryActivity : Activity
	{
		BookkeeperManager manager = BookkeeperManager.Instance;

		RadioButton rbIncome, rbExpense;
		EditText etDate, etDesc, etTotalAmountIncTax, etTotalAmountExcTax;
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
			etTotalAmountIncTax = FindViewById<EditText>(Resource.Id.et_total_amount_inc_tax);
			etTotalAmountExcTax = FindViewById<EditText>(Resource.Id.et_total_amount_exc_tax);
			spnTaxRate = FindViewById<Spinner>(Resource.Id.spn_tax_rate);
			Button btnAddEntry = FindViewById<Button>(Resource.Id.btn_add_entry);

			// Get income or expense accounts, depending on selected radio button
			var accounts = (rbIncome.Checked) ? manager.IncomeAccounts : manager.ExpenseAccounts;
			// Get data for the adapter
			var items = accounts.Select(a => a.ToString()).ToList();
			// Populate spinner with data
			InitSpinner(spnIncomeOrExpanseAccount, items);

			// Get data for the adapter
			items = manager.MoneyAccounts.Select(a => a.ToString()).ToList();
			// Populate spinner with data
			InitSpinner(spnMoneyAccount, items);

			// Get data for the adapter
			items = manager.TaxRates.Select(t => t.ToString()).ToList();
			// Populate spinner spnTaxRate with data
			InitSpinner(spnTaxRate, items);

			// Add event handlers
			rbIncome.Click += RadioButtonClick;
			rbExpense.Click += RadioButtonClick;
			btnAddEntry.Click += ButtonAddEntryClick;
			etDate.Click += EditTextDateClick;
			etTotalAmountIncTax.TextChanged += EditTextTotalAmountIncTaxTextChanged;
			spnTaxRate.ItemSelected += SpinnerTaxRateItemSelected;
		}

		// - Event handlers

		// Creates an entry
		void ButtonAddEntryClick(object sender, EventArgs e)
		{
			// Create model for entry
			Entry entry = new Entry();

			entry.Date = DateTime.Parse(etDate.Text);
			entry.Description = etDesc.Text;
			entry.TotalAmount = double.Parse(etTotalAmountIncTax.Text);

			// Add income/expanse account
			entry.IncomeOrExpanseAccount = (string)spnIncomeOrExpanseAccount.SelectedItem;

			// Add money account
			entry.MoneyAccount = (string)spnMoneyAccount.SelectedItem;

			// Add tax rate
			entry.TaxRate = Helper.ParseTaxRate((string)spnTaxRate.SelectedItem);

			// Add entry to database
			manager.AddEntry(entry);

			ClearInputTextFields();

			// Notify user
			Toast.MakeText(this, Resource.String.toast_entry_added, ToastLength.Short).Show();
		}

		void RadioButtonClick(object sender, EventArgs e)
		{
			RadioButton rb = (RadioButton)sender;
			List<string> accounts;

			// Get income or expense accounts, depending on radio button selected
			if (rb.Id == Resource.Id.rb_income)
			{
				accounts = manager.IncomeAccounts.Select(a => a.ToString()).ToList();
			}
			else accounts = manager.ExpenseAccounts.Select(a => a.ToString()).ToList();

			InitSpinner(spnIncomeOrExpanseAccount, accounts);
		}

		void EditTextDateClick(object sender, EventArgs e)
		{
			// Note: today.Mount returns a value between 1 and 12, but arg monthOfYear is a value between 0 and 11
			DateTime today = DateTime.Today;
			DatePickerDialog dialog = new DatePickerDialog(this, OnDateSet, today.Year, today.Month - 1, today.Day);
			dialog.Show();
		}

		void OnDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
		{
			etDate.Text = e.Date.ToShortDateString();
		}

		void SpinnerTaxRateItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
		{
			SetEditTextTotalAmountExcTax();
		}

		void EditTextTotalAmountIncTaxTextChanged(object sender, TextChangedEventArgs e)
		{
			SetEditTextTotalAmountExcTax();
		}

		// - Help methods

		void InitSpinner(Spinner spinner, List<string> items)
		{
			// Create adapter and bind data
			var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, items);

			// Feel and look, tror jag =)
			adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);

			// Bind adapter to spinner
			spinner.Adapter = adapter;
		}

		void SetEditTextTotalAmountExcTax()
		{
			if (!string.IsNullOrEmpty(etTotalAmountIncTax.Text))
			{
				string appyingTax = (string)spnTaxRate.SelectedItem;
				etTotalAmountExcTax.Text = Helper.CalculateTotalAmountBeforeTax(etTotalAmountIncTax.Text, appyingTax);
			}
			else etTotalAmountExcTax.Text = "-";
		}

		void ClearInputTextFields()
		{
			etDate.Text = "";
			etDesc.Text = "";
			etTotalAmountIncTax.Text = "";
			etTotalAmountExcTax.Text = "";
		}
	}
}
