using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace Pyther.Core.Ext;

public static class StringExt
{
    #region Manipulation

    public static string Repeat(this string text, uint n)
    {
        var spanSrc = text.AsSpan();
        int length = spanSrc.Length;
        var spanDst = new Span<char>(new char[length * (int)n]);
        for (var i = 0; i < n; i++)
        {
            spanSrc.CopyTo(spanDst.Slice(i * length, length));
        }
        return spanDst.ToString();
    }

    public static string Reverse(this string text)
    {
        char[] c = text.ToCharArray();
        Array.Reverse(c);
        return new string(c);
    }

    public static string Clip(this string text, int length)
    {
        return text[..Math.Min(text.Length, length)];
    }

    #endregion

    #region Transform/Encode/Decode

    public static string ToCamelCase(this string text)
    {
        return System.Text.Json.JsonNamingPolicy.CamelCase.ConvertName(text);
    }

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

    /// <summary>
    /// Check if the string is an integer.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static bool IsInteger(this string text)
    {
        return long.TryParse(text, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out _);
    }

    /// <summary>
    /// Check if the string is an email address.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static bool IsEmail(this string text)
    {
        var email = text.Trim();
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return !email.EndsWith(".") && addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public static bool Contains(this string str, string needle, StringComparison comparer = StringComparison.Ordinal)
    {
        return str?.IndexOf(needle, comparer) >= 0;
    }

    public static bool ContainsIgnoreCase(this string str, string needle)
    {
        return str?.IndexOf(needle, StringComparison.OrdinalIgnoreCase) >= 0;
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
    public static int CountWords(this string text)
    {
        return text.Split(new char[] { ' ', '\r', '\n', '.', '?', '!' }, StringSplitOptions.RemoveEmptyEntries).Length;
    }

    public static void Throw(this string text)
    {
        throw new Exception(text);
    }

}
