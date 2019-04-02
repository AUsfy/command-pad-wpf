using System.IO;
using System.Reflection;
using NUnit.Framework;
using Core;
using Models;

namespace InfrastructureTests
{
    [TestFixture]
    public class CoreTests
    {
        private const string dbPath = @"/Data/dbTest.sqlite";
        private const string emptyDbPath = @"/Data/empty.sqlite";

        public static string AssemblyDirectory
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
        }

        [Test]
        public void TestReadCommands()
        {
            var connectionString = DbHelper.CreateStringConnectionFromPath(AssemblyDirectory + dbPath);
            var sqliteDbConnector = new SqliteDbConnector(connectionString);
            var listCommands = sqliteDbConnector.GetCommands();
            int i = 1;
            foreach (var command in listCommands)
            {
                Assert.AreEqual(i, command.Id);
                Assert.AreEqual("name" + i, command.CommandName);
                Assert.AreEqual("cmd" + i, command.Command);
                i++;
            }
        }

        [Test]
        public void TestInsertCommand()
        {
            var connectionString = DbHelper.CreateStringConnectionFromPath(AssemblyDirectory + emptyDbPath);
            using (var sqliteDbConnector = new SqliteDbConnector(connectionString))
            {
                var cmd = new CommandModel {Command = "NameInserted", CommandName = "CmdInserted"};
                sqliteDbConnector.InsertOrUpdateCommand(cmd);
                var resultCmd = sqliteDbConnector.GetCommands()[0];
                Assert.AreEqual(cmd.CommandName, resultCmd.CommandName);
                Assert.AreEqual(cmd.Command, resultCmd.Command);
            }
        }
    }
}
