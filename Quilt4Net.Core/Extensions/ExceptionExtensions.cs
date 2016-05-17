using System;

// ReSharper disable once CheckNamespace
namespace Quilt4Net
{
    public static class ExceptionExtensions
    {
        public static T AddData<T>(this T item, object key, object value) where T : Exception
        {
            if(item == null) return null;
            if (item.Data.Contains(key)) item.Data.Remove(key);
            item.Data.Add(key, value);
            return item;
        }
    }
}