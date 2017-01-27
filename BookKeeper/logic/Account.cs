using System;
using SQLite;

namespace BookKeeper
{
	public class Account
	{
		// Account types
		internal const int INCOME = 1;
		internal const int EXPANSE = 2;
		internal const int MONEY = 3;

		public Account() { }

		internal Account(string name, int number, int type)
		{
			Name = name;
			Number = number;
			Type = type;
		}

		[PrimaryKey]
		public int Number { get; set; }

		public string Name { get; set; }

		public int Type { get; set; }

		public override string ToString()
		{
			return string.Format("{0} ({1})", Name, Number);;
		}
	}
}
