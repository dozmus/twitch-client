using System;

namespace TwitchClient.Util
{
    /// <summary>
    ///     A collection of date-related utility functions.
    /// 
    ///     This class is provided from the azeron-ircd open-source
    ///     IRC server software, under the MIT license.
    /// </summary>
    public class DateHelper
    {
        /// <summary>
        ///     The UNIX timestamp base date.
        /// </summary>
        private static readonly DateTime UnixTimestampBaseDate = new DateTime(1970, 1, 1);

        /// <summary>
        ///     Calculates the UNIX timestamp of now.
        /// </summary>
        /// <returns>UNIX timestamp of now</returns>
        public static int UnixTimestampNow()
        {
            return (Int32) (DateTime.UtcNow.Subtract(UnixTimestampBaseDate)).TotalSeconds;
        }

        /// <summary>
        ///     Converts the given UNIX timestamp to a date-time format.
        /// </summary>
        /// <param name="unixTimestamp">UNIX timestamp</param>
        /// <returns>UNIX timestamp as DateTime</returns>
        public static DateTime GetDateTime(int unixTimestamp)
        {
            return UnixTimestampBaseDate.Add(new TimeSpan(0, 0, 0, unixTimestamp));
        }
    }
}