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

			return Math.Round(totalAmountExcTax,2).ToString();
		}

		internal static double CalculateTaxFromTotalAmont(double totalAmount, double taxRate)
		{
			taxRate *= 0.01;
			double marginTax = taxRate / (1 + taxRate);
			double tax = Math.Round(totalAmount * marginTax, 2);

			return tax;
		}

		internal static double ParseTaxRate(string rate)
		{
			return double.Parse(rate.Remove(rate.IndexOf('%')));
		}

		/*
		internal static double ParseEntryAccountNumber(string account)
		{
			int idxStart = account.IndexOf('(') + 1;
			int idxEnd = account.IndexOf(')');
			string accountNumber = account.Substring(idxStart, idxEnd - idxStart);

			return double.Parse(accountNumber);
		}
		*/
	}
}
