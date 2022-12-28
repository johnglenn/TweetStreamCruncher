using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlennDemo.Twitter.Models
{
    public class TwitterClientConfig
    {
        public TwitterClientConfig(string bearerToken)
        {
            BearerToken = bearerToken;
        }

        public string BearerToken { get; set; }
    }
}
