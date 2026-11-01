using NUnit.Framework;
using MyGame.Services;

namespace Tests
{
    [TestFixture]
    public class ConnectivityServiceTests
    {
        [Test]
        public void ConnectivityService_InitialStatus_IsConnectedByDefault()
        {
            var service = new ConnectivityService();
            Assert.IsTrue(service.IsConnected);
            Assert.AreEqual(ConnectivityStatus.Connected, service.CurrentStatus);
        }

        [Test]
        public void ConnectivityService_UpdateStatus_ChangesConnectionState()
        {
            var service = new ConnectivityService();
            service.UpdateStatus(ConnectivityStatus.Disconnected);

            Assert.IsFalse(service.IsConnected);
            Assert.AreEqual(ConnectivityStatus.Disconnected, service.CurrentStatus);
        }

        [TestCase(200, true)]
        [TestCase(204, true)]
        [TestCase(404, false)]
        [TestCase(500, false)]
        public void EvaluateHttpStatus_ReturnsAccurateConnectionResult(long statusCode, bool expected)
        {
            bool isSuccess = ConnectivityService.EvaluateHttpStatus(statusCode);
            Assert.AreEqual(expected, isSuccess);
        }
    }
}
