using System;
using SQLite;

namespace BookKeeper
{
	public class Account
	{
		public Account() { }

		internal Account(string name, int number)
		{
			Name = name;
			Number = number;
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
