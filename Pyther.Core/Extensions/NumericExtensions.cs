namespace Pyther.Core.Extensions;

public static class NumericExtensions
{
    #region float

    /// <summary>
    /// Compare two float values using a tolerance.
    /// </summary>
    /// <param name="a">The first value</param>
    /// <param name="b">The second value</param>
    /// <param name="tolerance">An optional tolerance (0.00001f by default)</param>
    /// <returns>Returns true, if both values are equal within a tolerance, false otherwise.</returns>
    public static bool CloseTo(this float? a, float? b, float tolerance = 0.00001f)
    {
        if (a == null && b == null) return true;
        if (a == null || b == null) return false;
        return Math.Abs(a.Value - b.Value) < tolerance;
    }

    #endregion

    #region double

    /// <summary>
    /// Compare two double values using a tolerance.
    /// </summary>
    /// <param name="a">The first value</param>
    /// <param name="b">The second value</param>
    /// <param name="tolerance">An optional tolerance (0.00001 by default)</param>
    /// <returns>Returns true, if both values are equal within a tolerance, false otherwise.</returns>
    public static bool CloseTo(this double? a, double? b, double tolerance = 0.00001)
    {
        if (a == null && b == null) return true;
        if (a == null || b == null) return false;
        return Math.Abs(a.Value - b.Value) < tolerance;
    }

    #endregion

    #region decimal

    /// <summary>
    /// Compare two decmal values using a tolerance.
    /// </summary>
    /// <param name="a">The first value</param>
    /// <param name="b">The second value</param>
    /// <param name="tolerance">An optional tolerance (0.00001m by default)</param>
    /// <returns>Returns true, if both values are equal within a tolerance, false otherwise.</returns>
    public static bool CloseTo(this decimal? a, decimal? b, decimal tolerance = 0.00001m)
    {
        if (a == null && b == null) return true;
        if (a == null || b == null) return false;
        return Math.Abs(a.Value - b.Value) < tolerance;
    }

    #endregion
}
