using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlennDemo.JsonParser
{
    public interface IJsonParser<ModelType>
    {
        ModelType DeserializeJsonStringToModel(string json);
    }
}
