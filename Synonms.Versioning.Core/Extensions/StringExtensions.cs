namespace Synonms.Versioning.Core.Extensions
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return string.Empty;

            if (char.IsUpper(text[0]))
            {
                return char.ToLower(text[0]) + text.Remove(0, 1);
            }

            return text;
        }
    }
}
