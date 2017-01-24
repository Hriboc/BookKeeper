using System;
namespace BookKeeper
{
	class TaxRate
	{
		internal TaxRate(double rate)
		{
			Rate = rate;	
		}

		internal double Rate { get; private set; }

		public override string ToString()
		{
			return Rate + "%";
		}
	}
}
