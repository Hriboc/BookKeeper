using System;
using SQLite;

namespace BookKeeper
{
	class Entry
	{
		/*
		Guid id = new Guid();
		internal long Id
		{
			get {return ((long)id.GetHashCode()); }
		}
		*/
		[PrimaryKey, AutoIncrement]
		internal int Id { get; private set; }

		Account incomeOrExpanseAccount;
		Account moneyAccount;
		TaxRate taxRate;

		// Account types
		//internal bool IncomeAccount { get; set; }
		//internal bool ExpanseAccount { get; set; }

		internal string Date { get; set;}
		internal string Description { get; set;}
		internal string TotalAmount { get; set; }

		// Get and Set income/expanse account
		internal Account IncomeOrExpanseAccount
		{
			get { return incomeOrExpanseAccount; }
		}

		internal void setIncomeOrExpanseAccount(string account)
		{
			incomeOrExpanseAccount = parseAccount(account);
		}

		// Get and Set money account
		internal Account MoneyAccount
		{
			get { return moneyAccount; }
		}

		internal void setMoneyAccount(string account)
		{
			moneyAccount = parseAccount(account);
		}

		// Get and Set taxe rate
		internal TaxRate TaxRate 
		{
			get { return taxRate; }
		}

		internal void setTaxRate(string rate)
		{
			taxRate = parseTaxRate(rate);
		}

		// Help methonds for parsing account and tax rate
		private Account parseAccount(string account)
		{
			string[] nameAndNumber = account.Split('(');
			string name = nameAndNumber[0].TrimEnd();
			string number = nameAndNumber[1].Substring(0, nameAndNumber[1].Length - 1);
			return new Account(name, int.Parse(number));
		}

		private TaxRate parseTaxRate(string rate)
		{
			return new TaxRate(double
			                   .Parse(rate
	                           .Remove(rate
                               .IndexOf('%'))));
		}


	}
}
