using Core;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace CoreTests
{
    [TestFixture]
    public class CommandRunnerTest
    {
        [Test]
        public async Task TestRunCommand()
        {
            string msg = "hello";
            string expectedMsg = msg + "\r\n";
            string cmd = "echo "+msg;
            ProcessResult result = await (new CommandRunner()).RunCommandAsync(cmd, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.IsEmpty(result.StandardError[0]);
            Assert.AreEqual(expectedMsg, result.StandardOutput[0]);
        }
    }
}
