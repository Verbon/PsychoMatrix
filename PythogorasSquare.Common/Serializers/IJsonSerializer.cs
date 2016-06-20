namespace PythogorasSquare.Common.Serializers
{
    public interface IJsonSerializer
    {
        string Serialize(object obj);

        T Deserialize<T>(string json);
    }
}