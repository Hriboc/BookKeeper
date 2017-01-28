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

			// Get data for the adapter and populate spnIncomeOrExpanseAccount with data
			var accounts = (rbIncome.Checked) ? manager.IncomeAccounts : manager.ExpenseAccounts;
			var items = accounts.Select(a => a.ToString()).ToList();
			InitSpinner(spnIncomeOrExpanseAccount, items);

			// Get data for the adapter and populate spnMoneyAccount with data
			items = manager.MoneyAccounts.Select(a => a.ToString()).ToList();
			InitSpinner(spnMoneyAccount, items);

			// Get data for the adapter and populate spnTaxRate with data
			items = manager.TaxRates.Select(t => t.ToString()).ToList();
			InitSpinner(spnTaxRate, items);

			// Add event handlers
			rbIncome.Click += RadioButtonClick;
			rbExpense.Click += RadioButtonClick;
			btnAddEntry.Click += ButtonAddEntryClick;
			etDate.Click += EditTextDateClick;
			etTotalAmountIncTax.TextChanged += EditTextTotalAmountIncTaxTextChanged;
			spnTaxRate.ItemSelected += SpinnerTaxRateItemSelected;

			bool editable = Intent.GetBooleanExtra(Helper.EXTRA_EDIT_MODE, false);
			if (editable)
			{
				int id = Intent.GetIntExtra(Helper.EXTRA_ENTRY_ID, -1);
				Entry entry = manager.GetEntry(id);

				rbExpense.Checked = (manager.ExpenseAccounts
				                    .Select(a => a.ToString()))
									.Contains(entry.IncomeOrExpanseAccount);

				etDate.Text = entry.Date.ToShortDateString();
				etDesc.Text = entry.Description;
				etTotalAmountIncTax.Text = entry.TotalAmount.ToString();

				// Set the item to be displayed in spnIncomeOrExpanseAccount
				spnIncomeOrExpanseAccount.SetSelection(((ArrayAdapter<string>)spnIncomeOrExpanseAccount
				                                        .Adapter)
													   	.GetPosition(entry.IncomeOrExpanseAccount));

				// Set the item to be displayed in spnMoneyAccount 
				spnMoneyAccount.SetSelection(((ArrayAdapter<string>)spnMoneyAccount
				                              	.Adapter)
											 	.GetPosition(entry.MoneyAccount));

				// Set the item to be displayed in spnTaxRate 
				spnTaxRate.SetSelection(((ArrayAdapter<string>)spnTaxRate
				                         .Adapter)
										 .GetPosition(new TaxRate(entry.TaxRate).ToString()));
				// Change label of button to update
				btnAddEntry.Text = GetString(Resource.String.btn_update_entry);
			}
		}

		// - Event handlers

		// Creates an entry
		void ButtonAddEntryClick(object sender, EventArgs e)
		{
			Entry entry;

			bool editable = Intent.GetBooleanExtra(Helper.EXTRA_EDIT_MODE, false);
			if (editable)
			{
				// Need Entry Id for updating
				int id = Intent.GetIntExtra(Helper.EXTRA_ENTRY_ID, -1);
				entry = manager.GetEntry(id);
			}
			else entry = new Entry();

			entry.Date = DateTime.Parse(etDate.Text);
			entry.Description = etDesc.Text;
			entry.TotalAmount = double.Parse(etTotalAmountIncTax.Text);

			entry.IncomeOrExpanseAccount = (string)spnIncomeOrExpanseAccount.SelectedItem;
			entry.MoneyAccount = (string)spnMoneyAccount.SelectedItem;
			entry.TaxRate = Helper.ParseTaxRate((string)spnTaxRate.SelectedItem);

			string toastMsg;
			if (editable)
			{
				manager.UpdateEntry(entry);
				toastMsg = GetString(Resource.String.toast_entry_updated);
				// Poppa stacken

				// uppdatera Entry List Activity / EntriesAdapter
			}
			else
			{
				manager.AddEntry(entry);
				toastMsg = GetString(Resource.String.toast_entry_added);
				ClearInputTextFields();
			}
			// Notify user
			Toast.MakeText(this, toastMsg, ToastLength.Short).Show();

			this.Finish();
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
