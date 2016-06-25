namespace PythogorasSquare.Common.Configuration
{
    public interface IAppConfigService
    {
        string AppVersion { get; }


        bool HasValue(string key);

        bool GetBoolean(string key);

        int GetInt32(string key);

        string GetString(string key);

        TValue GetValue<TValue>(string key, bool shouldDeserialize = false);

        void SetValue(string key, object value, bool shouldSerialize = false);

        void RemoveValue(string key);
    }
}