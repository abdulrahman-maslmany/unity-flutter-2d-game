using System;

namespace MyGame.Services;

    public enum ConnectivityStatus
    {
        Connected,
        Disconnected,
        Checking
    }

    public interface IConnectivityService
    {
        ConnectivityStatus CurrentStatus { get; }
        bool IsConnected { get; }
        void UpdateStatus(ConnectivityStatus status);
    }

    public class ConnectivityService : IConnectivityService
    {
        public ConnectivityStatus CurrentStatus { get; private set; } = ConnectivityStatus.Connected;

        public bool IsConnected => CurrentStatus == ConnectivityStatus.Connected;

        public void UpdateStatus(ConnectivityStatus status)
        {
            CurrentStatus = status;
        }

        public static bool EvaluateHttpStatus(long responseCode)
        {
            return responseCode >= 200 && responseCode < 300;
        }
    }

