using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using System.IO;

namespace BookKeeper
{
	class BookkeeperManager
	{
		// Singelton
		static BookkeeperManager instance;

		SQLiteConnection db;

		private BookkeeperManager()
		{
			// Create connection to database
			string dirPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			db = new SQLiteConnection(dirPath + Path.DirectorySeparatorChar + "bookkeeper.db");

			// Create tables
			db.CreateTable<TaxRate>();
			db.CreateTable<Account>();
			db.CreateTable<Entry>();

			// Create tax rates
			if (db.Table<TaxRate>().Count() == 0)
			{
				db.Insert(new TaxRate(6));
				db.Insert(new TaxRate(12));
				db.Insert(new TaxRate(25));
			}

			if (db.Table<Account>().Count() == 0)
			{
				// Create income accounts
				db.Insert(new Account("Försäljning varor", 3010, Account.INCOME));
				db.Insert(new Account("Försäljning tjänster varor", 3310, Account.INCOME));

				// Create expence accounts
				db.Insert(new Account("Inköp", 4010, Account.EXPANSE));
				db.Insert(new Account("Förbrukningsmaterial", 5460, Account.EXPANSE));
				db.Insert(new Account("Mobiltelefon", 6212, Account.EXPANSE));

				// Create money accounts
				db.Insert(new Account("Kassa", 1910, Account.MONEY));
				db.Insert(new Account("Placeringskonto", 1940, Account.MONEY));
				db.Insert(new Account("Egna Insättningar", 2018, Account.MONEY));
			}
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
				return db.Table<Account>()
					     .Where(a => a.Type == Account.INCOME)
						 .ToList();
			}
		}

		internal IList<Account> ExpenseAccounts
		{
			get
			{
				return db.Table<Account>()
					     .Where(a => a.Type == Account.EXPANSE)
						 .ToList();
			}
		}

		internal IList<Account> MoneyAccounts
		{
			get
			{
				return db.Table<Account>()
					     .Where(a => a.Type == Account.MONEY)
					     .ToList();
			}
		}

		internal IList<TaxRate> TaxRates
		{
			get
			{
				return db.Table<TaxRate>().ToList();
			}
		}

		internal void AddEntry(Entry e)
		{
			db.Insert(e);
		}

		internal void UpdateEntry(Entry e)
		{
			db.Update(e);
		}

		internal IList<Entry> GetEntries()
		{
			return db.Table<Entry>().ToList();
		}

		internal Entry GetEntry(int id)
		{
			return db.Get<Entry>(id);
		}

		// A row in tax report looks like this "Description  (+/-)Tax\n"
		internal string GetTaxReport()
		{
			string taxes = "";
			var incomeAccounts = IncomeAccounts.Select(a => a.ToString());

			foreach (Entry e in GetEntries())
			{
				taxes += e.Description + "  ";
				double tax = Helper.CalculateTaxFromTotalAmont(e.TotalAmount, e.TaxRate);

				if (incomeAccounts.Contains(e.IncomeOrExpanseAccount))
					taxes += "+";
				else taxes += "-";

				taxes += tax + "\n";
			}
			return taxes.Remove(taxes.LastIndexOf('\n'));

			/*
			var taxes = GetEntries()
				.Select(e => e.TotalAmount * e.TaxRate * 0.01)
				.Select(t => t.ToString());
			
			return string.Join(Environment.NewLine, taxes);
			*/
		}

		internal string GetDetailedReportForAllAccounts()
		{
			string report = "";
			var entries = GetEntries();

			report += AppendDetailedReportForAccount(report, IncomeAccounts, entries);
			report += AppendDetailedReportForAccount(report, ExpenseAccounts, entries);
			report += AppendDetailedReportForAccount(report, MoneyAccounts, entries);

			return report;
		}

		// passing by reference on report
		string AppendDetailedReportForAccount(string report, IList<Account> accounts, IList<Entry> entries)
		{
			foreach (Account account in accounts)
			{
				// get all income entries for current account
				var incomeEntries = entries.Where(entry => entry.IncomeOrExpanseAccount
												  .Equals(account.ToString())).ToList();

				if (incomeEntries != null && incomeEntries.Count > 0)
					report += "*** " + account + "\n"; //TODO: lägg till total belopp för hela kontot
				foreach (var ie in incomeEntries)
				{
					report += string.Format("{0} - {1}, {2} kr\n", ie.Date.ToShortDateString(), ie.Description, ie.TotalAmount); // TODO: +/- på belopp
				}
			}
			return report;
		}
	}
}
