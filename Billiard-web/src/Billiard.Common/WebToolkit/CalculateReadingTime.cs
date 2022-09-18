﻿using System;

namespace Billiard.Common.WebToolkit
{
    public static class CalculateReadingTime
    {
        public static string MinReadTime(this string text, int wordsPerMinute = 180)
        {
            var wordsCount = text.WordsCount();
            var minutes = wordsCount / wordsPerMinute;
            return minutes == 0 ? "کمتر از یک دقیقه" : TimeSpan.FromMinutes(minutes).toReadableString();
        }

        private static string toReadableString(this TimeSpan span)
        {
            var formatted = string.Format("{0}{1}{2}{3}",
                span.Duration().Days > 0 ? string.Format("{0:0} روز و ", span.Days) : string.Empty,
                span.Duration().Hours > 0 ? string.Format("{0:0} ساعت و ", span.Hours) : string.Empty,
                span.Duration().Minutes > 0 ? string.Format("{0:0} دقيقه و ", span.Minutes) : string.Empty,
                span.Duration().Seconds > 0 ? string.Format("{0:0} ثانيه", span.Seconds) : string.Empty);

            if (formatted.EndsWith("و "))
            {
                formatted = formatted.Substring(0, formatted.Length - 2);
            }

            if (string.IsNullOrEmpty(formatted))
            {
                formatted = "0 ثانيه";
            }
            return formatted.Trim();
        }
    }
}