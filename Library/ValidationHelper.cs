using System.Text.RegularExpressions;

namespace Locator;

public partial class ValidationHelper
{
    [GeneratedRegex(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$")]
    private static partial Regex EmailRegex();

    public bool IsValidEmail(string email)
    {
        return EmailRegex().IsMatch(email);
    }

    [GeneratedRegex(@"^\d{10}$")]
    private static partial Regex PhoneNumberRegex();

    public bool IsValidPhoneNumber(string phoneNumber)
    {
        return PhoneNumberRegex().IsMatch(phoneNumber);
    }
}
