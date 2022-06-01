using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartAppWS.Utilities
{
    public class CardTool
    {
        public static bool IsValid(string CardNumber)
        {
            StringBuilder digitsOnly = new StringBuilder();
            foreach (char c in CardNumber.Where(c => char.IsDigit(c)))
            {
                digitsOnly.Append(c);
            }

            if (digitsOnly.Length > 18 || digitsOnly.Length < 15) return false;

            int sum = 0;
            int digit = 0;
            int addend = 0;
            bool timesTwo = false;

            for (int i = digitsOnly.Length - 1; i >= 0; i--)
            {
                digit = Int32.Parse(digitsOnly.ToString(i, 1));
                if (timesTwo)
                {
                    addend = digit * 2;
                    if (addend > 9)
                        addend -= 9;
                }
                else
                    addend = digit;

                sum += addend;

                timesTwo = !timesTwo;

            }
            return (sum % 10) == 0;
        }

        public static string CardType(string CardNumber)
        {
            string s = CardNumber.Trim().Replace("-", "").Replace(" ", "");
            if (s.StartsWith("34") || s.StartsWith("37"))
                return "American Express";
            if ((int.Parse(s.Substring(0, 2)) >= 51 && int.Parse(s.Substring(0, 2)) <= 55)
                || (int.Parse(s.Substring(0, 6)) >= 222100 && int.Parse(s.Substring(0, 6)) <= 272099))
                return "MasterCard";
            if (int.Parse(s.Substring(0, 2)) == 65 || int.Parse(s.Substring(0, 4)) == 6011
                || (int.Parse(s.Substring(0, 6)) >= 622126 && int.Parse(s.Substring(0, 6)) <= 622925)
                || (int.Parse(s.Substring(0, 3)) >= 644 && int.Parse(s.Substring(0, 3)) <= 649))
                return "Discover";
            if ((int.Parse(s.Substring(0, 6)) >= 500000 && int.Parse(s.Substring(0, 6)) <= 509999)
                || (int.Parse(s.Substring(0, 6)) >= 560000 && int.Parse(s.Substring(0, 6)) <= 699999))
                return "Maestro";
            if (s.StartsWith("4026") || s.StartsWith("417500") || s.StartsWith("4405") || s.StartsWith("4508") || s.StartsWith("4844") || s.StartsWith("4913") || s.StartsWith("4917"))
                return "Visa Electron";
            if ((s.StartsWith("4")) && !(s.StartsWith("4903") || s.StartsWith("4905") || s.StartsWith("4911") || s.StartsWith("4936")))
                return "Visa";
            return "Other";
        }
    }
}