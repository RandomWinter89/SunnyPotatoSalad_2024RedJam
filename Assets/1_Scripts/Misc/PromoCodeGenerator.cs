using System;
using System.Security.Cryptography;
using System.Text;

public class PromoCodeGenerator
{
    public static string GeneratePromoCode(string input)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            string hashString = BitConverter.ToString(hashBytes).Replace("-", "").Substring(0, 10);
            string promoCode = FormatPromoCode(hashString);

            return promoCode;
        }
    }

    private static string FormatPromoCode(string hashString)
    {
        return string.Join("-", hashString.Substring(0, 4), hashString.Substring(4, 4), hashString.Substring(8, 2));
    }
}
