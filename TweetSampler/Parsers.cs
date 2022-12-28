using GlennDemo.JsonParser;
using GlennDemo.Twitter.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlennDemo.TweetSampler
{
    internal static class Parsers
    {
        private static ConcurrentBag<double> _nativeTimes = new ConcurrentBag<double>();
        private static ConcurrentBag<double> _newtonTimes = new ConcurrentBag<double>();

        private static NativeJsonParser<TweetStreamDataWrapper> _nativeParser = new (
            time => _nativeTimes.Add(time)
            );

        private static NewtonsoftJsonParser<TweetStreamDataWrapper> _newtonParser = new (
            time => _newtonTimes.Add(time)
            );

        internal static IJsonParser<TweetStreamDataWrapper> NativeParser => _nativeParser;

        internal static IJsonParser<TweetStreamDataWrapper> NewtonsoftParser => _newtonParser;

        // TODO: remove after parser downselection
        internal static void LogTimesAndResetTimeCollections()
        {
#if DEBUG
            Console.WriteLine("Average JSON Deserialization Times:");
            Console.WriteLine("-- Native    : {0} ms", _nativeTimes.Any() ? _nativeTimes.Average() : -1);
            Console.WriteLine("-- Newtonsoft: {0} ms", _newtonTimes.Any() ? _newtonTimes.Average() : -1);
            Console.WriteLine();

            _nativeTimes.Clear();
            _newtonTimes.Clear();
#endif
        }
    }
}
