using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.Storage;
using PythogorasSquare.Common;
using PythogorasSquare.Common.Configuration;
using PythogorasSquare.Common.Serializers;

namespace PythogorasSquare.Clients.Store.Common.Configuration
{
    [UsedImplicitly]
    public class AppConfigService : IAppConfigService, IAppConfigServiceInitializer
    {
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IPropertySet _localSettings;


        public string AppVersion { get; private set; }


        public AppConfigService(IJsonSerializer jsonSerializer)
        {
            _jsonSerializer = jsonSerializer;
            _localSettings = ApplicationData.Current.LocalSettings.Values;
        }


        public async Task InitializeAsync(Uri appConfigUri)
        {
            var appConfig = await ReadConfigAsync(appConfigUri);
            AppVersion = appConfig.Version;
            InitializeLocalSettings(appConfig.Entries);
        }

        public bool HasValue(string key)
            => _localSettings.ContainsKey(key);

        public bool GetBoolean(string key)
            => GetValue<bool>(key);

        public int GetInt32(string key)
            => GetValue<int>(key);

        public string GetString(string key)
            => GetValue<string>(key);

        public TValue GetValue<TValue>(string key, bool shouldDeserialize = false)
        {
            var settingsValue = _localSettings[key];

            return shouldDeserialize ? _jsonSerializer.Deserialize<TValue>((string) settingsValue) : (TValue) settingsValue;
        }

        public void SetValue(string key, object value, bool shouldSerialize = false)
        {
            _localSettings[key] = shouldSerialize ? _jsonSerializer.Serialize(value) : value;
        }

        public void RemoveValue(string key)
        {
            _localSettings.Remove(key);
        }


        private async Task<AppConfig> ReadConfigAsync(Uri appConfigUri)
        {
            var appConfigFile = await StorageFile.GetFileFromApplicationUriAsync(appConfigUri);
            var serializedAppConfig = await FileIO.ReadTextAsync(appConfigFile);

            return _jsonSerializer.Deserialize<AppConfig>(serializedAppConfig);
        }

        private void InitializeLocalSettings(IEnumerable<AppConfigEntry> configEntries)
        {
            foreach (var configEntry in configEntries.Where(configEntry => configEntry.IsApplicationLevel || !_localSettings.ContainsKey(configEntry.Name)))
            {
                switch (configEntry.SerializeAs)
                {
                    case AppConfigEntryType.Boolean:
                        _localSettings[configEntry.Name] = Boolean.Parse(configEntry.Value);
                        break;
                    case AppConfigEntryType.Int32:
                        _localSettings[configEntry.Name] = Int32.Parse(configEntry.Value, CultureInfo.InvariantCulture);
                        break;
                    case AppConfigEntryType.String:
                        _localSettings[configEntry.Name] = configEntry.Value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("configEntry.SerializeAs");
                }
            }
        }
    }
}