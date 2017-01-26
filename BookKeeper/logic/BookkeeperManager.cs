using System;
using System.Collections.Generic;

namespace BookKeeper
{
	class BookkeeperManager
	{
		// Singelton
		private static BookkeeperManager instance = null;

		IList<Account> incomeAccounts = new List<Account>();
		IList<Account> expenseAccounts = new List<Account>();
		IList<Account> moneyAccounts = new List<Account>();
		IList<TaxRate> taxRates = new List<TaxRate>();
		IList<Entry> entries = new List<Entry>();

		private BookkeeperManager()
		{
			// Add income accounts
			incomeAccounts.Add(new Account("Försäljning varor", 3010));
			incomeAccounts.Add(new Account("Försäljning tjänster varor", 3310));

			// Add expence accounts
			expenseAccounts.Add(new Account("Inköp", 4010));
			expenseAccounts.Add(new Account("Förbrukningsmaterial", 5460));
			expenseAccounts.Add(new Account("Mobiltelefon", 6212));

			// Add money accounts
			moneyAccounts.Add(new Account("Kassa", 1910));
			moneyAccounts.Add(new Account("Placeringskonto", 1940));
			moneyAccounts.Add(new Account("Egna Insättningar", 2018));

			// Add tax rates
			taxRates.Add(new TaxRate(6));
			taxRates.Add(new TaxRate(12));
			taxRates.Add(new TaxRate(25));
		}

		internal static BookkeeperManager Instance
		{
			get
			{
				if (instance == null)
					instance = new BookkeeperManager();
				return instance;
			}
		}

		internal IList<Account> IncomeAccounts
		{
			get
			{
				return incomeAccounts;
			}
		}

		internal IList<Account> ExpenseAccounts
		{
			get
			{
				return expenseAccounts;
			}
		}

		internal IList<Account> MoneyAccounts
		{
			get
			{
				return moneyAccounts;
			}
		}

		internal IList<TaxRate> TaxRates
		{
			get
			{
				return taxRates;
			}
		}

		internal void AddEntry(Entry e)
		{
			entries.Add(e);
		}

		internal IList<Entry> GetEntries()
		{
			return entries;
		}
	}
}
