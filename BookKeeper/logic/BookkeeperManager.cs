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
				db.Insert(new Account("Försäljning varor", 3010));
				db.Insert(new Account("Försäljning tjänster varor", 3310));

				// Create expence accounts
				db.Insert(new Account("Inköp", 4010));
				db.Insert(new Account("Förbrukningsmaterial", 5460));
				db.Insert(new Account("Mobiltelefon", 6212));

				// Create money accounts
				db.Insert(new Account("Kassa", 1910));
				db.Insert(new Account("Placeringskonto", 1940));
				db.Insert(new Account("Egna Insättningar", 2018));
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
					     .Where(a => a.Number >= 3000 && a.Number < 4000)
						 .ToList();
			}
		}

		internal IList<Account> ExpenseAccounts
		{
			get
			{
				return db.Table<Account>()
					     .Where(a => a.Number >= 4000 && a.Number < 8000)
						 .ToList();
			}
		}

		internal IList<Account> MoneyAccounts
		{
			get
			{
				return db.Table<Account>()
					     .Where(a => a.Number >= 1000 && a.Number < 3000)
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
		}

		internal string GetDetailedReportForAllAccounts()
		{
			string report = "";
			IList<Entry> entries = GetEntries();

			report += GetDetailedReportForAccounts(IncomeAccounts, entries);
			report += GetDetailedReportForAccounts(ExpenseAccounts, entries);
			report += GetDetailedReportForAccounts(MoneyAccounts, entries);

			int idxNewLinesEnd = report.LastIndexOf("\n\n\n", StringComparison.CurrentCulture);
			return report.Remove(idxNewLinesEnd);
		}

		string GetDetailedReportForAccounts(IList<Account> accounts, IList<Entry> entries)
		{
			string report = "";
			foreach (Account account in accounts)
			{
				
				report += "*** " + account + "\n"; //TODO: lägg till total belopp för hela kontot
				foreach (var entry in entries)
				{
					if(entry.IncomeOrExpanseAccount.Equals(account.ToString()) || entry.MoneyAccount.Equals(account.ToString()))
						report += string.Format("{0} - {1}, {2} kr\n", entry.Date.ToShortDateString(), entry.Description, entry.TotalAmount); // TODO: +/- på belopp
				}
				report += "***\n";
			}
			return report + "\n\n";
		}
	}
}
