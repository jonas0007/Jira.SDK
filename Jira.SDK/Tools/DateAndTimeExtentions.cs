using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jira.SDK.Tools
{
	public static class DateAndTimeExtentions
	{
		public enum DaysBetweenOptions
		{
			IgnoreWeekends
		}
		public static List<DateTime> GetDaysUntil(this DateTime from, DateTime until, params DaysBetweenOptions[] options)
		{
			List<DayOfWeek> daysToIgnore = new List<DayOfWeek>();
			if (options.Contains(DaysBetweenOptions.IgnoreWeekends))
			{
				daysToIgnore.Add(DayOfWeek.Saturday);
				daysToIgnore.Add(DayOfWeek.Sunday);
			}

			from = from.Date;
			until = until.Date;

			List<DateTime> days = new List<DateTime>();
			for (DateTime date = from.Date; date.CompareTo(until) < 0; date = date.AddDays(1))
			{
				if (!daysToIgnore.Contains(date.DayOfWeek))
				{
					days.Add(date);
				}
			}

			return days;
		}

		public static String ToDetailedString(this TimeSpan time, Int32 hoursInDay)
		{
			StringBuilder output = new StringBuilder("0h");
			if (time.TotalMinutes > 0)
			{
				output = new StringBuilder();
				Int32 days = (int)(time.TotalHours / hoursInDay);
				Int32 hours = (((int)time.TotalHours) % hoursInDay);
				Int32 minutes = (((int)time.TotalMinutes) % 60);
				Int32 weeks = (int)days / 5;
				days = days % 5;

				if (weeks > 0)
					output.Append(String.Format("{0}w ", weeks));
				if (days > 0)
					output.Append(String.Format("{0}d ", days));
				if (hours > 0)
					output.Append(String.Format("{0}h ", hours));
				if (minutes > 0)
					output.Append(String.Format("{0}m ", minutes));
			}

			return output.ToString();
		}
	}
}
