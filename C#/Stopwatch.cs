using System;
using System.Collections;
using System.Linq;

namespace QuerySamples
{
    public class Stopwatch
    {
        public Stopwatch()
        {
//            TestTime(Enumerable.Range(1, 10).Where(x => x/2 == 0),10);
        }

        public static double TestTime(IEnumerable enumerable, int i)
        {
            var tmp = Enumerable.Range(0, i).Select(x => TestTime(enumerable));
            var tmp1 = tmp.OrderBy(x => x).Skip(tmp.Count()/2);
            return tmp1.First();
        }

        public static double TestTime(IEnumerable enumerable)
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            int i = 0;
            foreach (var VARIABLE in enumerable)
            {
                i++;
            }
//            var i = enumerable.Cast<object>().Count();
            stopwatch.Stop();

            //Console.WriteLine(i);
            return stopwatch.Elapsed.TotalMilliseconds;
        }
    }
}
