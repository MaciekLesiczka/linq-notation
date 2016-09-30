using System;
using System.Threading.Tasks;

namespace LinqNotation
{
    public static class TaskMonad
    {

        public static Task<U> Select<T, U>(this Task<T> task, Func<T, U> func)
        {
            return Task.Run(() =>
            {
                return func(task.Result);
            });
        }

        public static Task<U> SelectMany<T, U>(this Task<T> task, Func<T, Task<U>> f)
        {
            return Task.Run(() =>
            {
                var t = task.Result;
                var ut = f(t);
                return ut.Result;
            });
        }

        public static Task<V> SelectMany<T, U, V>(this Task<T> task, Func<T, Task<U>> f, Func<T, U, V> c)
        {
            return Task.Run(() =>
            {                
                var t = task.Result;
                var ut = f(t);                
                var utr = ut.Result;
                return c(t, utr);
            });
        }
    }
}