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

		// Formel för marginalmoms: M = P/(1+P)
		internal static string CalculateTotalAmountBeforeTax(string totalAmount, string applyingTax)
		{
			double totalAmountIncTax = double.Parse(totalAmount);
			double applTax = double.Parse(applyingTax.Remove(applyingTax.IndexOf('%'))) * 0.01;
			double marginTax = applTax / (1 + applTax);
			double totalAmountExcTax = totalAmountIncTax * (1 - marginTax);

			return totalAmountExcTax.ToString();
		}
	}
}
