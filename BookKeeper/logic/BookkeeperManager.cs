﻿using System;
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

		// Formel för marginalmoms: M = P/(1+P)
		internal string GetTaxReport()
		{
			string taxes = "";
			var incomeAccounts = IncomeAccounts.Select(a => a.ToString());

			foreach (Entry e in GetEntries())
			{
				taxes += e.Description + "  ";
				double marginTax = e.TaxRate * 0.01 / (1 + e.TaxRate * 0.01);
				double tax = Math.Round(e.TotalAmount * marginTax, 2);
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
	}
}
