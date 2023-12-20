using System.ComponentModel;
using System.Dynamic;
using System.Globalization;
using System.Net;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace Pyther.Core.Extensions;

public static class StringExtensions
{
    private static readonly Regex rexEmail = new(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
    private static readonly Regex rexRemoveTags = new(@"(?></?\w+)(?>(?:[^>'""]+|'[^']*'|""[^""]*"")*)>");
    private static readonly Regex rexRemoveHtmlComments = new("<!--.*?-->", RegexOptions.Singleline);

    #region Manipulation

    public static string? Repeat(this string? text, uint n)
    {
        if (text == null) return null;
        var spanSrc = text.AsSpan();
        int length = spanSrc.Length;
        var spanDst = new Span<char>(new char[length * (int)n]);
        for (var i = 0; i < n; i++)
        {
            spanSrc.CopyTo(spanDst.Slice(i * length, length));
        }
        return spanDst.ToString();
    }

    public static string? Reverse(this string? text)
    {
        if (text == null) return null;
        char[] c = text.ToCharArray();
        Array.Reverse(c);
        return new string(c);
    }

    public static string? Clip(this string? text, int length)
    {
        if (text == null) return null;
        return text[..Math.Min(text.Length, length)];
    }

    /// <summary>
    /// Remmove html/xml tags from the given string.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="decode"></param>
    /// <returns></returns>
    public static string? RemoveTags(this string? text, bool decode = false)
    {
        if (text == null)
        {
            return null;
        }

        string? result = rexRemoveTags.Replace(text, string.Empty);

        return decode ? WebUtility.HtmlDecode(result) : result;
    }

    /// <summary>
    /// Remove Html comments from the given string.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string? RemoveHtmlComments(this string? text)
    {
        return text != null ? rexRemoveHtmlComments.Replace(text, string.Empty) : null;
    }

    #endregion

    #region Transform/Encode/Decode

    public static string Base64Encode(this string text, Encoding? encoding = null)
    {
        encoding ??= Encoding.UTF8;
        return Convert.ToBase64String(encoding.GetBytes(text));
    }

    public static string Base64Decode(this string text, Encoding? encoding = null)
    {
        encoding ??= Encoding.UTF8;
        return encoding.GetString(Convert.FromBase64String(text));
    }

    public static string? LowerFirst(this string? text)
    {
        if (text == null) return null;
        if (string.IsNullOrEmpty(text)) return text;
        if (text.Length > 1)
            return char.ToLower(text[0]) + text[1..];
        else
            return char.ToLower(text[0]).ToString();
    }

    public static string? UpperFirst(this string? text)
    {
        if (text == null) return null;
        if (string.IsNullOrEmpty(text)) return text;
        if (text.Length > 1)
            return char.ToUpper(text[0]) + text[1..];
        else
            return char.ToUpper(text[0]).ToString();
    }

    #endregion

    #region Naming Policy

    public static string? SnakeToTitleCase(this string? name) => name != null ? string.Concat(name.Split('_').Select(CultureInfo.InvariantCulture.TextInfo.ToTitleCase)) : null;
    public static string? SnakeToCamelCase(this string? name) => name != null ? string.Concat(name.Split('_').Select(CultureInfo.InvariantCulture.TextInfo.ToTitleCase)).LowerFirst() : null;
    public static string? SnakeToKebabCase(this string? name) => name != null ? name.Replace('_', '-') : null;

    public static string? TitleToSnakeCase(this string? name)
    {
        if (name == null) return null;
        string[] parts = string.Concat(
            string.Join("_", name.Split(new char[] { },
            StringSplitOptions.RemoveEmptyEntries))
            .Select(c => char.IsLower(c) ? $"{c}" : $"_{c}".ToLower()))
            .Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
        return string.Join("_", parts);
    }
    public static string? TitleToKebabCase(this string? name)
    {
        if (name == null) return null;
        string[] parts = string.Concat(
            string.Join("-", name.Split(new char[] { },
            StringSplitOptions.RemoveEmptyEntries))
            .Select(c => char.IsLower(c) ? $"{c}" : $"-{c}".ToLower()))
            .Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
        return string.Join("-", parts);
    }
    public static string? TitleToCamelCase(this string? name) => name != null ? System.Text.Json.JsonNamingPolicy.CamelCase.ConvertName(name) : null;

    public static string? KebabToTitleCase(this string? name) => name != null ? string.Concat(name.Split('-').Select(CultureInfo.InvariantCulture.TextInfo.ToTitleCase)) : null;
    public static string? KebabToCamelCase(this string? name) => name != null ? string.Concat(name.Split('-').Select(CultureInfo.InvariantCulture.TextInfo.ToTitleCase)).LowerFirst() : null;
    public static string? KebabToSnakeCase(this string? name) => name?.Replace('-', '_');

    public static string? CamelToSnakeCase(this string? name) => name?.UpperFirst().TitleToSnakeCase();
    public static string? CamelToKebabCase(this string? name) => name?.UpperFirst().TitleToKebabCase();
    public static string? CamelToTitleCase(this string? name) => name?.UpperFirst();

    #endregion

    #region Conversation

    /// <summary>
    /// Generic text converter.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="text"></param>
    /// <param name="cultureInfo"></param>
    /// <param name="supressException"></param>
    /// <returns></returns>
    public static T? Parse<T>(this string text, CultureInfo? cultureInfo = null, bool supressException = true) where T : struct
    {
        T? result = null;
        try
        {
            // try to convert to type T
            TypeConverter tc = TypeDescriptor.GetConverter(typeof(T));
            if (tc.IsValid(text)) return (T?)tc.ConvertFromString(null, cultureInfo ?? CultureInfo.InvariantCulture, text);
            // try to convert to enum value
            if (typeof(T).IsEnum) return Enum.TryParse(text, out T res) ? res : null;
        }
        catch (Exception)
        {
            if (!supressException)
            {
                throw;
            }
        }
        return result;
    }

    /// <summary>
    /// Try to convert the string to an enum text.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="text"></param>
    /// <returns></returns>
    public static T? ToEnum<T>(this string text) where T : struct
    {
        return Enum.TryParse(text, out T myStatus) ? myStatus : null;
    }

    public static DateTime? ToDateTime(this string text)
    {
        return DateTime.TryParse(text, out DateTime result) ? result : null;
    }

    public static TimeSpan? ToTimeSpan(this string text)
    {
        return TimeSpan.TryParse(text, out TimeSpan result) ? result : null;
    }

    #endregion

    #region Checks

    public static bool IsEmpty(this string? text)
    {
        return string.IsNullOrWhiteSpace(text);
    }

    /// <summary>
    /// Check if the string is an integer.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static bool IsInteger(this string? text)
    {
        return text != null && long.TryParse(text, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out _);
    }

    /// <summary>
    /// Check if the string is an email address.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static bool IsEmail(this string? text)
    {
        if (text == null) return false;
        return rexEmail.Match(text.Trim()).Success && !text.EndsWith('.');
    }

    public static bool Contains(this string? text, string? needle, StringComparison comparer = StringComparison.Ordinal)
    {
        if (text == null || needle == null) return false;
        return text?.IndexOf(needle, comparer) >= 0;
    }

    public static bool ContainsIgnoreCase(this string? text, string? needle)
    {
        if (text == null || needle == null) return false;
        return text?.IndexOf(needle, StringComparison.OrdinalIgnoreCase) >= 0;
    }

    #endregion

    #region En/De-cryption

    [SupportedOSPlatform("windows")]
    public static string? EncryptRSA(this string? text, string key)
    {
        if (text == null) return null;
        var parameter = new CspParameters { KeyContainerName = key };
        var provider = new RSACryptoServiceProvider(parameter) { PersistKeyInCsp = true };
        byte[] bytes = provider.Encrypt(Encoding.UTF8.GetBytes(text), true);
        return BitConverter.ToString(bytes);
    }

    [SupportedOSPlatform("windows")]
    public static string? DecryptRSA(this string text, string key)
    {
        if (text == null) return null;
        var parameter = new CspParameters { KeyContainerName = key };
        var provider = new RSACryptoServiceProvider(parameter) { PersistKeyInCsp = true };
        string[] decryptArray = text.Split(new[] { "-" }, StringSplitOptions.None);
        byte[] decryptByteArray = Array.ConvertAll(decryptArray, (s => Convert.ToByte(byte.Parse(s, NumberStyles.HexNumber))));
        byte[] bytes = provider.Decrypt(decryptByteArray, true);
        string result = Encoding.UTF8.GetString(bytes);
        return result;
    }

    #endregion

    /// <summary>
    /// Create an instance with sthe string using as full qualified class name.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="className">The full qualified class name.</param>
    /// <param name="assembly">The assembly the class is defined (null means current assembly).</param>
    /// <param name="args">Optional class constructor arguments</param>
    /// <returns>The instance of T on success, null otherwise.</returns>
    public static T? ToInstance<T>(this string className, System.Reflection.Assembly? assembly = null, object?[]? args = null) where T: class
    {
        Type? type = assembly != null ? assembly.GetType(className) : Type.GetType(className);
        if (type != null)
        {
            return Activator.CreateInstance(type, args) as T;
        }
        return null;
    }

    /// <summary>
    /// Create an instance with sthe string using as full qualified class name from unknown assembly.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="className">The full qualified class name.</param>
    /// <param name="args">Optional class constructor arguments</param>
    /// <returns>The instance of T on success, null otherwise.</returns>
    public static T? ToInstanceUnknownAssembly<T>(this string className, object?[]? args = null) where T: class
    {
        Type? type = Type.GetType(className);
        if (type != null)
        {
            return Activator.CreateInstance(type) as T;
        }
        foreach (System.Reflection.Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            type = assembly.GetType(className);
            if (type != null)
                return Activator.CreateInstance(type, args) as T;
        }
        return null;
    }

    public static void SaveToFile(this string text, string path)
    {
        using StreamWriter writer = new(path);
        writer.Write(text);
    }

    /// <summary>
    /// Count the words of the string.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static int CountWords(this string? text)
    {
        return text != null ? text.Split(new char[] { ' ', '\r', '\n', '.', '?', '!' }, StringSplitOptions.RemoveEmptyEntries).Length : 0;
    }

    public static void Throw(this string? text)
    {
        throw new Exception(text);
    }

    public static IEnumerable<T>? SplitToType<T>(this string? text, char separator, StringSplitOptions splitOptions = StringSplitOptions.RemoveEmptyEntries) where T : IConvertible
    {
        if (text == null) return null;
        return text?.Split(separator, splitOptions).Select(s => (T)Convert.ChangeType(s, typeof(T)));
    }

    public static IEnumerable<T>? SplitToType<T>(this string? text, string separator, StringSplitOptions splitOptions = StringSplitOptions.RemoveEmptyEntries) where T : IConvertible
    {
        if (text == null) return null;
        return text?.Split(separator, splitOptions).Select(s => (T)Convert.ChangeType(s, typeof(T)));
    }

    public static IEnumerable<T>? SplitToType<T>(this string? text, StringSplitOptions splitOptions = StringSplitOptions.RemoveEmptyEntries, params char[] separators) where T : IConvertible
    {
        if (text == null) return null;
        return text?.Split(separators, splitOptions).Select(s => (T)Convert.ChangeType(s, typeof(T)));
    }

    public static dynamic? ParseJson(this string? text, JsonSerializerOptions? options = null, bool suppressExceptions = false)
    {
        if (string.IsNullOrWhiteSpace(text)) return null;
        try
        {
            return JsonSerializer.Deserialize<ExpandoObject>(text, options);
        }
        catch (Exception)
        {
            if (suppressExceptions)
            {
                return null;
            }
            throw;
        }
    }

    public static T? ParseJson<T>(this string? text, JsonSerializerOptions? options = null, bool suppressExceptions = false) where T : class
    {
        if (string.IsNullOrWhiteSpace(text)) return null;
        try
        {
            return JsonSerializer.Deserialize<T>(text, options);
        }
        catch (Exception)
        {
            if (suppressExceptions)
            {
                return null;
            }
            throw;
        }
    }
}
