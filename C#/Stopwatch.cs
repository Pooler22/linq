using System;
using System.Collections.Generic;
using System.Linq;

namespace QuerySamples
{
    public class Stopwatch
    {
        public Stopwatch()
        {
            TestTime(Enumerable.Range(1, 10).Where(x => x/2 == 0) as IEnumerable<object>,10);
        }

        public static TimeSpan TestTime(IEnumerable<object> enumerable, int i)
        {
            var tmp = Enumerable.Range(0, i).Select(x =>new {time = TestTime(enumerable)}).ToArray();
            return tmp.OrderBy(x => x.time).Skip(tmp.Count() / 2).First().time;
        }

        public static TimeSpan TestTime(IEnumerable<object> enumerable)
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            var tmp = enumerable.ToArray();
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }
    }
}
