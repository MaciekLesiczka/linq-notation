using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqNotation
{
    public class Maybe<T>
    {
        private readonly T _value;

        public Maybe()
        {            
        }
        public Maybe(T value)
        {
            if (Object.ReferenceEquals(value, null))
            {
                throw new NullReferenceException();
            }
            HasValue = true;
            _value = value;
        }

        public T Value
        {
            get
            {
                if (HasValue)
                {
                    return _value;
                }
                throw new InvalidOperationException();
            }
        }

        public bool HasValue { get; }
    }

    public static class MaybeMonad
    {
        public static Maybe<TResult> SelectMany<TSource, TTemp, TResult>(this Maybe<TSource> _this, Func<TSource, Maybe<TTemp>> optionSelector, Func<TSource, TTemp, TResult> resultSelector)
        {
            if (_this.HasValue)
            {
                var temp = optionSelector(_this.Value);
                if (temp.HasValue)
                {
                    return new Maybe<TResult>(resultSelector(_this.Value, temp.Value));
                }
            }
            return new Maybe<TResult>();
        }

        public static Maybe<TSource> DefaultIfEmpty<TSource>(this Maybe<TSource> _this, TSource defaultValue)
        {
            return _this.HasValue ? _this : new Maybe<TSource>(defaultValue);
        }

        public static IEnumerable<T> AsEnumerable<T>(this Nullable<T> _this)
            where T : struct
        {
            if (_this.HasValue)
            {
                yield return _this.Value;
            }
        }

        public static IEnumerable<T> AsEnumerable<T>(this Maybe<T> _this)
        {
            if (_this.HasValue)
            {
                yield return _this.Value;
            }
        }


        public static Maybe<TR> Select<TS, TR>(this Maybe<TS> _this, Func<TS, TR> func)
               where TS : struct
        {
            var seq = from t in _this.AsEnumerable()
                      select func(t);

            foreach (var r in seq)
            {
                return new Maybe<TR>(r);
            }
            return new Maybe<TR>();
        }
    }
}