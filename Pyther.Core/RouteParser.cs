using Pyther.Core.Extensions;
using System.Globalization;

namespace Pyther.Core
{
    /// <summary>
    /// Helper class to parse routes like "orders/{id}/clone".
    /// </summary>
    public class RouteParser
    {
        private readonly Dictionary<string, string> parameters = new();

        public RouteParser()
        {
        }

        /// <summary>
        /// Get the value of a route argument (if available).
        /// </summary>
        /// <param name="key">the argument key like "id".</param>
        /// <returns>The value or `NULL` if key doesn't exists.</returns>
        public string? this[string key]
        {
            get => parameters.ContainsKey(key) ? parameters[key] : null;
        }

        /// <summary>
        /// Returns `true`, if the route match. If it matches, the value of the argument can be retrieved afterward.
        /// </summary>
        /// <param name="mask">Syntax of the route to test (like "orders/{id}/clone").</param>
        /// <param name="route">Concrete route to test (like "orders/123/clone").</param>
        /// <returns>Reeturns true, if the route matches, false otherwise.</returns>
        public bool Is(string mask, string route)
        {
            parameters.Clear();

            int mIdx = 0;
            for (int rIdx = 0; rIdx < route.Length; rIdx++)
            {
                if (mIdx >= mask.Length) break;

                char m = mask[mIdx];
                char r = route[rIdx];

                // start of variable
                if (m == '{')
                {
                    int end = mask.IndexOf('}', mIdx);
                    // no ending } => false
                    if (end == -1) break;

                    // variable name between '{' and '}'
                    string varName = mask.Substring(mIdx + 1, end - mIdx - 1);

                    // next char after '}' or null if EOS
                    char? nextChar = end + 1 < mask.Length ? mask[end + 1] : null;
                    mIdx = end + 1;

                    // no '{' after '}' allowed!
                    if (nextChar == '{') break;

                    // advance route until nextChar or '/'
                    int rEnd1 = nextChar != null ? route.IndexOf(nextChar.Value, rIdx) : route.Length;
                    int rEnd2 = route.IndexOf('/', rIdx);
                    if (rEnd2 == -1) rEnd2 = route.Length;
                    int rEnd = Math.Min(rEnd1, rEnd2);

                    string varValue = route[rIdx..rEnd];
                    rIdx = rEnd;

                    parameters.Add(varName, varValue);

                    if (mIdx == mask.Length && rIdx == route.Length) return true;
                    mIdx++;

                    continue;
                }

                if (mIdx + 1 == mask.Length && rIdx + 1 == route.Length) return true;
                if (m != r) break;

                mIdx++;
            }

            parameters.Clear();
            return false;
        }

        /// <summary>
        /// Get the value of a route argument (if available) parsed to the given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="cultureInfo"></param>
        /// <param name="supressException"></param>
        /// <returns></returns>
        public T? Get<T>(string key, CultureInfo? cultureInfo = null, bool supressException = true) where T: struct
        {
            string? text = this[key];
            if (text == null) return null;
            return text.Parse<T>(cultureInfo, supressException);
        }
    }
}
