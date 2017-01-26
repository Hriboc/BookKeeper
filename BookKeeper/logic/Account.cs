using System;
using SQLite;

namespace BookKeeper
{
	class Account
	{
		internal Account(string name, int number)
		{
			Name = name;
			Number = number;
		}

		[PrimaryKey]
		internal int Number { get; set; }

		internal string Name { get; private set; }

		public override string ToString()
		{
			return Name + " (" + Number + ")";
		}
	}
}
