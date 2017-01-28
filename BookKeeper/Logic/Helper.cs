using System;
namespace BookKeeper
{
	internal class Helper
	{
		internal const string EXTRA_ENTRY_ID = "entryId";
		internal const string EXTRA_EDIT_MODE = "editMode";

		private Helper() {}

		// Formel för marginalmoms: M = P/(1+P)
		internal static string CalculateTotalAmountBeforeTax(string totalAmount, string applyingTax)
		{
			double totalAmountIncTax = double.Parse(totalAmount);
			double applTax = ParseTaxRate(applyingTax) * 0.01;
			double marginTax = applTax / (1 + applTax);
			double totalAmountExcTax = totalAmountIncTax * (1 - marginTax);

			return totalAmountExcTax.ToString();
		}

		internal static double ParseTaxRate(string rate)
		{
			return double.Parse(rate.Remove(rate.IndexOf('%')));
		}
	}
}
