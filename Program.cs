using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

namespace LinqNotation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("start..");
            Console.WriteLine(InteractiveExample().First());
            Console.WriteLine(Callback().Result);
            Console.WriteLine(AsyncExample().Result);
            Console.WriteLine(LinqExample().Result);
            Console.WriteLine(RxExample().ToTask().Result);
        }

        public static Task<Tuple<char, double>> Callback()
        {
            return GetInt().ContinueWith(
                i =>
                {
                    var idx = i.Result;
                    var dateTime = DateTime.UtcNow;
                    return GetString().ContinueWith(s =>
                    {
                        var diff = DateTime.UtcNow - dateTime;
                        var str = s.Result;
                        return Tuple.Create(str[idx], diff.TotalMilliseconds);
                    }).Result;
                }                
            );
        }

        public static Task<Tuple<char, double>> LinqExample() => 
            from idx in GetInt()
            let dateTime = DateTime.UtcNow
            from str in GetString()
            let diff = DateTime.UtcNow - dateTime
            select Tuple.Create(str[idx], diff.TotalMilliseconds);

        public static IObservable<Tuple<char, double>> RxExample()
        {
            return from idx in GetInt().ToObservable()
                   let dateTime = DateTime.UtcNow
                   from str in GetString().ToObservable()
                   let diff = DateTime.UtcNow - dateTime
                   select Tuple.Create(str[idx], diff.TotalMilliseconds);
        }

        public static IEnumerable<Tuple<char, double>> InteractiveExample()
        {
            foreach (var idx in GetInt().ToObservable().ToEnumerable())
            {
                var dateTime = DateTime.UtcNow;
                foreach (var str in GetString().ToObservable().ToEnumerable())
                {
                    var diff = DateTime.UtcNow - dateTime;
                    yield return Tuple.Create(str[idx], diff.TotalMilliseconds);
                }
            }            
        }

        public static IEnumerable<Tuple<char, double>> InteractiveExampleLinq()
        {
            return GetInt().ToObservable().ToEnumerable().Select(idx => new {idx, dateTime = DateTime.UtcNow}).SelectMany(@t => GetString().ToObservable().ToEnumerable(), (@t, str) => new {@t, str}).Select(@t => new {@t, diff = DateTime.UtcNow - @t.@t.dateTime}).Select(@t => Tuple.Create(@t.@t.str[@t.@t.@t.idx], @t.diff.TotalMilliseconds));
        }

        public static async Task<Tuple<char, double>> AsyncExample()
        {
            var idx = await GetInt();
            var dateTime = DateTime.UtcNow;
            var str = await GetString();
            var diff = DateTime.UtcNow - dateTime;
            return Tuple.Create(str[idx], diff.TotalMilliseconds);
        }

        public static Task<int> GetInt()
        {            
            return Task.Run(() =>
            {
                Thread.Sleep(3000);
                return 1;
            });
        }

        public static Task<string> GetString()
        {
            return Task.Run(() =>
            {
                Thread.Sleep(1500);
                return "abc";
            });
        }
    }


    public class Process
    {
        public virtual DateTime? StartTime { get; set; }
        public virtual DateTime? FinishTime { get; set; }

        public virtual TimeSpan? Duration
        {
            get
            {
                if (StartTime.HasValue == false)
                {
                    return null;
                }

                var finishTime = FinishTime.HasValue
                    ? FinishTime.Value
                    : DateTime.UtcNow;

                return finishTime - StartTime.Value;
            }
        }
    }



    public class Process2
    {
        public DateTime? StartTime { get; set; }
        public DateTime? FinishTime { get; set; }

        public TimeSpan? Duration
        {
            get
            {
                foreach (var startTime in StartTime.AsEnumerable())
                {
                    var finishTime = FinishTime.AsEnumerable().DefaultIfEmpty(DateTime.UtcNow);
                    foreach (var endTime in finishTime)
                    {
                        return endTime - startTime;
                    }
                }
                return null;
            }
        }

        public IEnumerable<TimeSpan> Duration2
        {
            get
            {
                return 
                    from startTime in StartTime.AsEnumerable()
                       from finishTime in FinishTime.AsEnumerable().DefaultIfEmpty(DateTime.UtcNow)
                       select finishTime - startTime;                
            }
        }
    }

    

    public class Process3
    {
        public virtual Maybe<DateTime> StartTime { get; set; }
        public virtual Maybe<DateTime> FinishTime { get; set; }
        
        public virtual Maybe<TimeSpan> Duration =>
            from startTime in StartTime
            from finishTime in FinishTime.DefaultIfEmpty(DateTime.UtcNow)
            select finishTime - startTime;
    }
    
}



