using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace QuerySamples
{
    public class Stopwatch
    {
        public Stopwatch()
        {
            TestTime(Enumerable.Range(1, 10).Where(x => x/2 == 0) as IEnumerable,10);
        }

        public static double TestTime(IEnumerable enumerable, int i)
        {
            var tmp = Enumerable.Range(0, i).Select(x =>new {time = TestTime(enumerable)}).ToArray();
            return tmp.OrderBy(x => x.time).Skip(tmp.Count() / 2).First().time;
        }

        public static double TestTime(IEnumerable enumerable)
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            //int i = 0;

            stopwatch.Start();
            foreach (var a in enumerable) {
            //    i++;
            }
            stopwatch.Stop();

            //Console.WriteLine(i);
            return stopwatch.Elapsed.TotalMilliseconds;
        }
    }
}
