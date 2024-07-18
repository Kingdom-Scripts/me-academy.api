using LazyCache;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace Saharaviewpoint.Core.Utilities;
internal static class CacheUtil
{
    internal static void ClearCaches(this IAppCache cache, params string[] cacheKeys)
    {
        foreach (string key in cacheKeys)
        {
            // Attempt to get the key as if it points to a list of cache keys
            var listKeys = cache.Get<List<string>>(key);
            if (listKeys != null)
            {
                // If it does, clear each cache key in the list
                foreach (var listKey in listKeys)
                {
                    cache.Remove(listKey);
                }
            }
            // Whether it was a list of keys or a single key, remove the key itself
            cache.Remove(key);
        }
    }

    internal static string GenerateCacheKey<TModel>(TModel model, params object[] additionalValues)
    {
        var stringBuilder = new StringBuilder();

        // Use reflection to get properties of the model
        PropertyInfo[] properties = typeof(TModel).GetProperties();

        bool isFirst = true;
        foreach (var property in properties)
        {
            if (property.GetValue(model) == null)
                continue;
            AppendValueToStringBuilder(ref isFirst, stringBuilder, property.GetValue(model));
        }

        // Handle additional values
        foreach (var value in additionalValues)
        {
            if (value == null)
                continue;
            AppendValueToStringBuilder(ref isFirst, stringBuilder, value);
        }

        return stringBuilder.ToString();
    }

    internal static string GenerateCacheKey(params object[] values)
    {
        var stringBuilder = new StringBuilder();

        bool isFirst = true;
        foreach (var value in values)
        {
            if (value == null)
                continue;
            AppendValueToStringBuilder(ref isFirst, stringBuilder, value);
        }

        return stringBuilder.ToString();
    }

    private static void AppendValueToStringBuilder(ref bool isFirst, StringBuilder stringBuilder, object value)
    {
        if (!isFirst)
        {
            stringBuilder.Append('-');
        }
        isFirst = false;

        // Handle DateTime values
        if (value is DateTime dateTimeValue)
        {
            stringBuilder.Append(dateTimeValue.ToString("o"));
        }
        // Handle Enum values
        else if (value is Enum enumValue)
        {
            stringBuilder.Append(enumValue.ToString());
        }
        // Handle Collections (Arrays, Lists, etc.)
        else if (value is IEnumerable enumerableValue && !(value is string))
        {
            stringBuilder.Append(string.Join(",", enumerableValue.Cast<object>().Select(e => e?.ToString())));
        }
        // Handle primitive types, strings, and Guids
        else if (value != null && value.GetType().IsPrimitive || value is decimal || value is string || value is Guid)
        {
            stringBuilder.Append(Convert.ToString(value, CultureInfo.InvariantCulture));
        }
        // Handle complex objects, potentially recursively or by a unique identifier
        else if (value != null)
        {
            stringBuilder.Append(value.GetType().Name);
        }
    }
}
