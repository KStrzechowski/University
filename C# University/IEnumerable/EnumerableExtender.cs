using System;
using System.Collections.Generic;

namespace Lab10a
{
    public delegate void deleg1<T>(T item);
    public delegate T deleg2<T>();
    public delegate bool deleg3<T>(T item);
    public delegate T1 deleg4<T, T1>(T item);
    public delegate T1 deleg5<T, T1>(T1 item1, T item2);
    public static class EnumerableExtender
    {
        /* Uzupełnić */
        public static void ForEach<T>(this IEnumerable<T> obj, deleg1<T> func)
        {
            foreach(var x in obj)
            {
                func(x);
            }
        }

        public static void Print<T>(this IEnumerable<T> obj)
        {
            obj.ForEach(obj => Console.Write(obj + ";"));
            Console.WriteLine();
        }

        public static IEnumerable<T> Where<T>(this IEnumerable<T> obj, deleg3<T> func)
        {
            foreach(var x in obj)
            {
                if(func(x)) { yield return x; }
            }
        }
        public static IEnumerable<T> GenerateN<T>(int count, deleg2<T> func)
        {
            for (int i = 0; i < count; i++)
                yield return func();
        }

        public static IEnumerable<T1> Transform<T, T1>(this IEnumerable<T> obj, Func<T, T1> func)
        {
            foreach (var x in obj)
            {
                yield return func(x);
            }
        }

        public static T1 Accumulate<T, T1>(this IEnumerable<T> obj, T1 initValue, deleg5<T, T1> func = null)
        {
            if (func != null)
            {
                foreach (var x in obj)
                {
                    initValue = func(initValue, x);

                }
            }
            return initValue;
        }
        public static int Accumulate(this IEnumerable<int> t, int initValue)
        {
            int s = initValue;
            foreach (var v in t)
            {
                s = s + v;
            }
            return s;
        }

        public static T FindFirstIfOrDefault<T>(this IEnumerable<T> obj, deleg3<T> func)
        {
            foreach(var x in obj)
            {
                if(func(x)) { return x; }
            }
            return default(T);
        }

        public static T[] ToArray<T>(this IEnumerable<T> obj)
        {
            int i = 0;
            foreach (var x in obj)
                i++;
            T[] NewArray = new T[i];
            i = 0;
            foreach (var x in obj)
            {
                NewArray[i] = x;
                i++;
            }
            return NewArray;
        }

        public static IEnumerable<T> Unique<T>(this IEnumerable<T> obj, Func<T,T,bool> func) where T : IComparable<T>
        {
            T[] TempArray = obj.ToArray();
            if (TempArray.Length > 0)
                yield return TempArray[0];
            for(int i = 1; i < TempArray.Length; i++)
                if (func(TempArray[i - 1], TempArray[i])) { yield return TempArray[i]; }

        }

        public static Func<T,T> MinFunc<T>(params Func<T,T>[] FuncArray) where T : IComparable<T>
        {
            if(FuncArray.Length == 0) 
                return x => x;
            return delegate (T x)
            {
                T min = FuncArray[0](x);
                for (int i = 1; i < FuncArray.Length; i++)
                    if (FuncArray[i](x).CompareTo(min) < 0) { min = FuncArray[i](x); }
                return min;
            };
        }
    }


}
