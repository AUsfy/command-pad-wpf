using NUnit.Framework;
using Core;

namespace CoreTests
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
