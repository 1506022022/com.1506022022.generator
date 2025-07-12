using System.Collections.Generic;
using System.Linq;

public static class JsonFormatUtility
{
    public static string ReplaceFormat(object @object, JsonFormat jsonFormat)
    {
        string baseJson = jsonFormat.Format;

        var type = @object.GetType();
        var fields = type.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var properties = FindProperties(jsonFormat);
        for (int i = 0; i < properties.Count; i++)
        {
            var property = properties[i];
            var changeValue = fields.Where(x => x.Name == property).FirstOrDefault()?.GetValue(@object)?.ToString();
            if (changeValue == null) continue;

            var oldValue = '{' + property + '}';
            baseJson = baseJson.Replace(oldValue, changeValue);
        }

        return baseJson;
    }
    static List<string> FindProperties(JsonFormat jsonFormat)
    {
        var format = jsonFormat.Format;
        var properties = new List<string>();
        int startIndex = 0;
        if (string.IsNullOrEmpty(jsonFormat.Format)) return properties;

        for (int i = 0; i < format.Length; i++)
        {
            if (format[i] == '{')
            {
                startIndex = i + 1;
            }
            else if (format[i] == '}' && -1 < startIndex)
            {
                var property = format.Substring(startIndex, i - startIndex);
                properties.Add(property);
                startIndex = -1;
            }
        }

        return properties;
    }
}
