namespace FleetManager.Common.Extension
{
    public static class StringExtensions
    {
        public static string ToKey(this string txt)
        {
            return string.IsNullOrEmpty(txt) ? txt : txt.Replace(" ", string.Empty).ToLower();
        }
    }
}
