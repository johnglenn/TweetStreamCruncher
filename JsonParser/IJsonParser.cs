namespace GlennDemo.JsonParser
{
    public interface IJsonParser<ModelType>
    {
        ModelType DeserializeJsonStringToModel(string json);
    }
}
