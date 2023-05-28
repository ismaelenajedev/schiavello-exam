namespace Server.Extensions;

public static class StringExtensions
{
    public static string SanitizePath(this string str)
    {
        if (string.IsNullOrWhiteSpace(str))
        return string.Empty;

        str = str.Replace("%2E", ".").Replace("%2F", "/");

        if (str.Contains("..") || str.Contains("//"))
        throw new ApplicationException("Invalid directory path");

        return str;
    }
}
