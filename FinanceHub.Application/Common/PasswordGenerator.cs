namespace FinanceHub.Application.Common;

public static class PasswordGenerator
{
    private const string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string Lowercase = "abcdefghijklmnopqrstuvwxyz";
    private const string Digits = "0123456789";
    private const string Special = "!@#$%&*";

    public static string GenerateRandomPassword(int length = 6)
    {
        var passwordChars = new[]
        {
            Uppercase[System.Security.Cryptography.RandomNumberGenerator.GetInt32(Uppercase.Length)],
            Lowercase[System.Security.Cryptography.RandomNumberGenerator.GetInt32(Lowercase.Length)],
            Digits[System.Security.Cryptography.RandomNumberGenerator.GetInt32(Digits.Length)],
            Special[System.Security.Cryptography.RandomNumberGenerator.GetInt32(Special.Length)]
        }.ToList();

        var allAllowed = Uppercase + Lowercase + Digits + Special;
        for (var i = passwordChars.Count; i < length; i++)
        {
            passwordChars.Add(allAllowed[System.Security.Cryptography.RandomNumberGenerator.GetInt32(allAllowed.Length)]);
        }

        return new string(passwordChars.OrderBy(_ => System.Security.Cryptography.RandomNumberGenerator.GetInt32(int.MaxValue)).ToArray());
    }
}