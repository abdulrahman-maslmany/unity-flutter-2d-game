using System.Collections.Generic;

namespace MyGame.Services;

// تم تغييرها إلى internal لحل CS-R1035
internal interface ISaveStore
{
    void SetInt(string key, int value);
    int GetInt(string key, int defaultValue = 0);
    bool HasKey(string key);
    void DeleteKey(string key);
}

// تم تغييرها إلى internal لحل CS-R1035
internal class InMemorySaveStore : ISaveStore
{
    private readonly Dictionary<string, int> _storage = new();

    public void SetInt(string key, int value) => _storage[key] = value;
    public int GetInt(string key, int defaultValue = 0) => _storage.TryGetValue(key, out var val) ? val : defaultValue;
    public bool HasKey(string key) => _storage.ContainsKey(key);
    public void DeleteKey(string key) => _storage.Remove(key);
}

// الكلاس العام الوحيد في الملف
public class ProgressionManager
{
    private readonly ISaveStore _store;
    private const string LevelKeyPrefix = "Level_Unlocked_";
    private const string HighScoreKey = "High_Score";

    internal ProgressionManager(ISaveStore store)
    {
        _store = store;
    }

    public void UnlockLevel(int level) => _store.SetInt(LevelKeyPrefix + level, 1);
    public bool IsLevelUnlocked(int level) => level == 1 || _store.GetInt(LevelKeyPrefix + level, 0) == 1;

    public void SaveHighScore(int score)
    {
        if (score > GetHighScore())
        {
            _store.SetInt(HighScoreKey, score);
        }
    }

    public int GetHighScore() => _store.GetInt(HighScoreKey, 0);
}
