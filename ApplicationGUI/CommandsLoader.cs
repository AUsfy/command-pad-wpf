using Core;
using Models;
using System.Collections.Generic;
using System.Linq;

namespace CmdProject
{
    internal static class DbWrapper 
    {
        private static ISqliteDbConnector dbFactory = new DbFactory().CreateSqliteDbConnector(""); // sqliteDbConnector handle the creation of db

        public static IList<CommandWrapper> Commands { get; set; }

        static DbWrapper()
        {
            Commands = dbFactory.GetCommands()?.Select(c=>new CommandWrapper(c)).ToList()?? new List<CommandWrapper>();
        }

        public static bool SaveCommands(IList<CommandWrapper> commandWrappers)
        {
            try
            {
                foreach (var cmd in commandWrappers)
                {
                    dbFactory.InsertOrUpdateCommand(cmd.CommandModel);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void InsertCommand(CommandWrapper cmdWrapper)
        {
            dbFactory.InsertOrUpdateCommand(cmdWrapper.CommandModel);
        }

        public static void InsertOrUpdateCommand(CommandModel cmd)
        {
            dbFactory.InsertOrUpdateCommand(cmd);
        }
    }
}
