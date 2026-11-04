namespace MyGame.Core;

public static class ConnectivityRules
{
    public static bool EvaluateConnectivity(int responseCode, bool isNetworkError, bool isHttpError)
    {
        if (isNetworkError || isHttpError)
        {
            return false;
        }

        return responseCode == 204 || responseCode == 200;
    }

    public static bool ShouldPauseGame(bool hasInternet, bool requireInternet)
    {
        return !hasInternet && requireInternet;
    }
}
