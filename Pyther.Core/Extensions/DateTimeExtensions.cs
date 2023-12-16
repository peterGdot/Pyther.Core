namespace Pyther.Core.Extensions
{
    public static class DateTimeExtensions
    {
        static readonly DateTime unixEpoch = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public static long Timestamp(this DateTime date)
        {
            TimeSpan diff = date.ToUniversalTime() - unixEpoch;
            return (long)Math.Floor(diff.TotalSeconds);
        }
    }
}
