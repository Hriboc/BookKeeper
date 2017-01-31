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
				db.Insert(new Account("Försäljning tjänster", 3310, Account.INCOME));
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

		internal IList<Entry> GetAllEntries()
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
			var expenseAccounts = ExpenseAccounts.Select(a => a.ToString());

			foreach (Entry entry in GetAllEntries())
			{
				taxes += entry.Description + "  ";
				double tax = Helper.CalculateTaxFromTotalAmont(entry.TotalAmount, entry.TaxRate);

				if (expenseAccounts.Contains(entry.IncomeOrExpanseAccount))
					taxes += "-";

				taxes += tax + "\n";
			}
			return taxes.Remove(taxes.LastIndexOf('\n'));
		}

		internal string GetDetailedReportForAllAccounts()
		{
			string report = "";
			IList<Entry> entries = GetAllEntries();

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
				double sum = 0;
				report += string.Format("*** {0} (total: [sum] kr)\n", account);
				foreach (var entry in entries)
				{
					if (entry.IncomeOrExpanseAccount.Equals(account.ToString()) || entry.MoneyAccount.Equals(account.ToString()))
					{
						if (account.Type == Account.EXPANSE)
							entry.TotalAmount = -entry.TotalAmount;
						report += string.Format("{0} - {1}, {2} kr\n", entry.Date.ToShortDateString(), 
							                    entry.Description, entry.TotalAmount);
						sum += entry.TotalAmount;
					}
				}
				report = report.Replace("[sum]", sum.ToString());  
				report += "***\n";
			}
			return report + "\n\n";
		}

		internal void DeleteAllEntries()
		{
			db.DeleteAll<Entry>();
		}
	}
}
