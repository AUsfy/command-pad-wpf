namespace Core
{
    public class DbFactory
    {
        public ISqliteDbConnector CreateSqliteDbConnector(string connectionString)
        {
            return new SqliteDbConnector(connectionString);
        }
    }
}
