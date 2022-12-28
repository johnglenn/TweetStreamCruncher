using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlennDemo.TweetSampler
{
    public interface ITweetSamplerService
    {
        Action<Exception> ErrorHandler { get; set; }

        int StatisticsReportingIntervalInSeconds { get; set; }

        void StartSampling();

        void StopSampling();

        bool IsStreamTaskRunning();
    }
}
