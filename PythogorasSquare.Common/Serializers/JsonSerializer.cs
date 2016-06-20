using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace PythogorasSquare.Common.Serializers
{
    [UsedImplicitly]
    public class JsonSerializer : IJsonSerializer
    {
        public string Serialize(object obj)
        {
            using (var ms = new MemoryStream())
            {
                var jsonSerializer = new DataContractJsonSerializer(obj.GetType());
                jsonSerializer.WriteObject(ms, obj);

                return Encoding.UTF8.GetString(ms.ToArray(), 0, (int)ms.Length);
            }
        }

        public T Deserialize<T>(string json)
        {
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                var jsonSerializer = new DataContractJsonSerializer(typeof(T));

                return (T) jsonSerializer.ReadObject(ms);
            }
        }
    }
}