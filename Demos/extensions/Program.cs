using System.Globalization;
using Pyther.Core.Extensions;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("===== String Extensions ===== \n");

        foreach (var value in new[] { 321L, 1020L, 87_654_321L, 10_987_654_321L, long.MaxValue, long.MinValue + 1 })
        {
            Console.WriteLine($"{value} = " + value.FormatBytes(1));
            Console.WriteLine($"{value} = " + value.FormatBytes(3, LongExtensions.ByteSystem.IEC));
            Console.WriteLine($"{value} = " + value.FormatBytes(2, LongExtensions.ByteSystem.IEC, ""));
            Console.WriteLine($"{value} = " + value.FormatBytes(2, LongExtensions.ByteSystem.SI, " ", CultureInfo.InvariantCulture));
            Console.WriteLine("");
        }

    }
}