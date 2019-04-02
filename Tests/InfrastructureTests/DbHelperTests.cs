using NUnit.Framework;
using Infrastructure;
using System;
using System.Data.SQLite;
using System.Reflection;
using System.IO;

namespace InfrastructureTests
{
    [TestFixture]
    public class DbHelperTest
    {

        [Test]
        public void TestConnectionStringValidation()
        {
            string connectString = @"Data Source=D:\fold1\fold2\db.sqlite;Version=3;";

            Assert.IsTrue(DbHelper.ValidateConnectionString(connectString));
            Assert.IsFalse(DbHelper.ValidateConnectionString(@"C:\a\b"));
        }

    }
}
