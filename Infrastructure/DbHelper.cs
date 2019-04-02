using System.Text.RegularExpressions;

namespace Infrastructure
{
    public class DbHelper
    {
        private static string pattern = @"^Data Source=[/\w\.\-\\\s\:]+.sqlite;Version=\d;$";

        public static string CreateStringConnectionFromPath(string path, string version = "3")
        {
            return $"Data Source={path};Version={version}";
        }

        public static bool ValidateConnectionString(string connectionString)
        {
            Regex rx = new Regex(pattern, RegexOptions.IgnoreCase);

            return rx.IsMatch(connectionString);
        }
    }
}
