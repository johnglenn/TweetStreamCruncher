using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GlennDemo.JsonParser
{
    public class NativeJsonParser<ModelType> : BaseParser<ModelType>
    {
        private JsonSerializerOptions _serializerConfig;
        public NativeJsonParser(Action<double>? timeInMillisecondsReportingAction) : base(timeInMillisecondsReportingAction)
        {
            _serializerConfig = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        }

        public override ModelType DeserializeJsonStringToModel(string json)
        {
            var start = DateTime.UtcNow;

            ModelType modelDeserializedFromString = JsonSerializer.Deserialize<ModelType>(json, _serializerConfig)
                ?? throw new JsonParserException("deserialized model is unexpectedly null");

            var timeInMilliseconds = (DateTime.UtcNow - start).TotalMilliseconds;
            ReportCompletionTimeNonBlocking(timeInMilliseconds);

            return modelDeserializedFromString;
        }
    }
}
