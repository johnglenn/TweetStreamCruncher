using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlennDemo.JsonParser
{
    public class JsonParserException : Exception
    {
        public JsonParserException(string message) : base(message) { }
    }
}
