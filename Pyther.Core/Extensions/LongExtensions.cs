using System.Globalization;

namespace Pyther.Core.Extensions
{
    public static class LongExtensions
    {
        public enum ByteSystem
        {
            SI,   // SI-System  
            IEC   // IEC-System
        }

        private static readonly string[] byteSuffix = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };

        public static string FormatBytes(this long bytes, int digits = 2, ByteSystem byteSystem = ByteSystem.SI, string separator = " ", CultureInfo? cultureInfo = null)
        {
            if (bytes == 0) return "0" + byteSuffix[0];
            int factor = byteSystem == ByteSystem.SI ? 1000 : 1024;
            long absBytes = bytes < 0 ? -bytes : bytes;
            int place = Convert.ToInt32(Math.Floor(Math.Log(absBytes, factor)));
            double num = Math.Round(absBytes / Math.Pow(factor, place), digits);
            return (bytes < 0 ? -num : num).ToString(cultureInfo) + separator + byteSuffix[place];

        }
    }
}
