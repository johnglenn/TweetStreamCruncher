using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlennDemo.JsonParser
{
    public class NewtonsoftJsonParser<ModelType> : BaseParser<ModelType>
    {
        private JsonSerializerSettings _settings;
        public NewtonsoftJsonParser(Action<double>? timeInMillisecondsReportingAction) : base(timeInMillisecondsReportingAction)
        {
            _settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = new DefaultContractResolver()
            };
        }

        public override ModelType DeserializeJsonStringToModel(string json)
        {
            var start = DateTime.UtcNow;

            ModelType modelDeserializedFromString = 
                Newtonsoft.Json.JsonConvert.DeserializeObject<ModelType>(json, _settings)
                ?? throw new JsonParserException("deserialized model is unexpectedly null");

            var timeInMilliseconds = (DateTime.UtcNow - start).TotalMilliseconds;
            ReportCompletionTimeNonBlocking(timeInMilliseconds);

            return modelDeserializedFromString;
        }
    }
}
