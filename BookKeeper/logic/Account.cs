using System;
namespace BookKeeper
{
	class Account
	{
		internal Account(string name, int number)
		{
			Name = name;
			Number = number;
		}

		internal string Name { get; private set; }

		internal int Number { get; private set; }

		public override string ToString()
		{
			return Name + " (" + Number + ")";
		}
	}
}
