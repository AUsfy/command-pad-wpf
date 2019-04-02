using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.IO;
using Models;

namespace Infrastructure
{
    public interface ISqliteDbConnector : IDisposable
    {
        List<CommandModel> GetCommands();
        bool InsertOrUpdateCommand(CommandModel cmd);
    }

    internal static class CmdColumnNames
    {
        public const string Id = "id";
        public const string Name = "name";
        public const string Cmd = "cmd";
        public const string TableName = "Commands";
    }

    public class SqliteDbConnector: ISqliteDbConnector
    {
        private readonly SQLiteConnection _sqliteConnection;

        public SqliteDbConnector(string connectionString)
        {

            if (string.IsNullOrEmpty(connectionString))
            {
                var dbFile = "Commands.sqlite";
                if (!File.Exists(dbFile))
                {
                    SQLiteConnection.CreateFile(dbFile);
                }
                connectionString = $"Data Source={dbFile};Version=3;";
            }
            _sqliteConnection = new SQLiteConnection(connectionString);
            CreateCommandsTableIfNotExist();
        }

        public void CreateCommandsTableIfNotExist() {
            _sqliteConnection.Open();
            string sql = $"CREATE TABLE IF NOT EXISTS {CmdColumnNames.TableName} (id Text PRIMARY KEY,name VARCHAR(100), cmd TEXT)";
            using (SQLiteCommand command = new SQLiteCommand(sql, _sqliteConnection))
            {
                var res = command.ExecuteNonQuery();
            }

            _sqliteConnection.Close();
        }

        public bool CreateCommand()
        {
            return false;
        }

        public bool SaveCommandList(IList<CommandModel> commands)
        {
            return false;
        }

        public bool InsertOrUpdateCommand(CommandModel cmd)
        {
            var sql = $"INSERT INTO {CmdColumnNames.TableName} VALUES(@{CmdColumnNames.Id},@{CmdColumnNames.Name}, @{CmdColumnNames.Cmd}) ON CONFLICT({CmdColumnNames.Id}) DO UPDATE SET {CmdColumnNames.Name}=excluded.{CmdColumnNames.Name} , {CmdColumnNames.Cmd}=excluded.{CmdColumnNames.Cmd}";
            try
            {
                _sqliteConnection.Open();
                using (SQLiteCommand dbCommand = new SQLiteCommand(sql, _sqliteConnection))
                {
                    dbCommand.Parameters.AddWithValue($"@{CmdColumnNames.Id}", cmd.Id.ToString());
                    dbCommand.Parameters.AddWithValue($"@{CmdColumnNames.Name}", cmd.CommandName);
                    dbCommand.Parameters.AddWithValue($"@{CmdColumnNames.Cmd}", cmd.Command);

                    dbCommand.ExecuteNonQuery();
                }
                _sqliteConnection.Close();
                return true;
            }
            catch (SqlException e)
            {
                //todo log exception
                //log
                Console.WriteLine(e.InnerException);
                return false;
            }
        }

        public bool DeleteCommand()
        {
            return false;
        }

        public List<CommandModel> GetCommands()
        {
           
            var commandList = new List<CommandModel>();
            _sqliteConnection.Open();
            using (SQLiteCommand sqlRequest =
                new SQLiteCommand($"SELECT * from {CmdColumnNames.TableName}", _sqliteConnection))
            {
                using (SQLiteDataReader reader = sqlRequest.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        NameValueCollection raw = reader.GetValues();
                        var command = MapToModel(raw);
                        commandList.Add(command);
                    }

                    reader.Close();
                }
            }

            _sqliteConnection.Close();
            return commandList;
            }

        private CommandModel MapToModel(NameValueCollection cmdRaw)
        {
            return new CommandModel
            {
                Id = Guid.Parse(cmdRaw.Get("id")),
                CommandName = cmdRaw.Get("name"),
                Command = cmdRaw.Get("cmd")
            };
        }

        public void Dispose()
        {
            _sqliteConnection?.Dispose();
        }
    }
}
