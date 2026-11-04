using NUnit.Framework;
using MyGame.Core;

namespace Tests;

[TestFixture]
public class ConnectivityRulesTests
{
    [TestCase(204, false, false, ExpectedResult = true)]
    [TestCase(200, false, false, ExpectedResult = true)]
    [TestCase(404, false, false, ExpectedResult = false)]
    [TestCase(500, false, false, ExpectedResult = false)]
    [TestCase(200, true, false, ExpectedResult = false)]
    [TestCase(200, false, true, ExpectedResult = false)]
    public bool EvaluateConnectivity_ValidatesResponseProperly(int responseCode, bool isNetworkError, bool isHttpError)
    {
        return ConnectivityRules.EvaluateConnectivity(responseCode, isNetworkError, isHttpError);
    }

    [TestCase(true, true, ExpectedResult = false)]
    [TestCase(false, true, ExpectedResult = true)]
    [TestCase(false, false, ExpectedResult = false)]
    [TestCase(true, false, ExpectedResult = false)]
    public bool ShouldPauseGame_CorrectlyDeterminesPauseState(bool hasInternet, bool requireInternet)
    {
        return ConnectivityRules.ShouldPauseGame(hasInternet, requireInternet);
    }
}
