using System;
using SQLite;

namespace BookKeeper
{
	public class Entry
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; private set; }

		public DateTime Date { get; set;}

		public string Description { get; set;}

		public double TotalAmount { get; set; }

		public string IncomeOrExpanseAccount { get; set; }

		public string MoneyAccount { get; set; }

		public double TaxRate { get; set; }
	}
}
