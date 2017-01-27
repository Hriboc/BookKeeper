using System;
using SQLite;

namespace BookKeeper
{
	public class TaxRate
	{
		public TaxRate() {}

		internal TaxRate(double rate)
		{
			Rate = rate;	
		}

		[PrimaryKey, AutoIncrement]
		public int Id { get; private set; }

		public double Rate { get; set; }

		public override string ToString()
		{
			return Rate + "%";
		}
	}
}
