using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace QuerySamples
{
    public class Stopwatch
    {
        public static double TestTime(IEnumerable enumerable, int i)
        {
            var results = Enumerable.Range(0, i).Select(x => TestTime(enumerable));
            double result = 0;
            try
            {
                if (i < 1)
                {
                    throw new ArgumentOutOfRangeException();
                }
                if (i == 1)
                {
                    result = results.First();

                }
                else switch (i % 2)
                {
                    case 0:
                        result = results.OrderBy(x => x).Skip(i / 2).First();
                        break;
                    case 1:
                        result = results.OrderBy(x => x).Skip(i / 2 - 1).Take(2).Average();
                        break;
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Exception: number of repetitions is out of range");
            }
            
            return result;
        }

        public static double TestTime(IEnumerable enumerable)
        {
            var i = 0;
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            foreach (var VARIABLE in enumerable)
            {
                i++;
            }
            stopwatch.Stop();
            //Console.WriteLine(i);
            return stopwatch.Elapsed.TotalMilliseconds;
        }
    }
}