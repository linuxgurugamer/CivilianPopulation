using System;
namespace CivilianPopulation.Domain
{
    public class TimeFormatter
    {
		private const int HOURS_PER_DAYS = 6;
		private const int DAYS_PER_YEARS = 426;

		public string format(double time)
        {
            int sec = (int)time;
			int min = 0;
			int hour = 0;
			int days = 0;
			int years = 0;
			if (sec >= 60)
			{
				min = sec / 60;
				sec = sec - min * 60;
			}
			if (min >= 60)
			{
				hour = min / 60;
				min = min - hour * 60;
			}
			if (hour >= HOURS_PER_DAYS)
			{
				days = hour / HOURS_PER_DAYS;
				hour = hour - days * HOURS_PER_DAYS;
			}
            if (days >= DAYS_PER_YEARS)
            {
                years = days / DAYS_PER_YEARS;
                days = days - years * DAYS_PER_YEARS;
            }

			string secAsString = sec.ToString("D2");
			string minAsString = min.ToString("D2");
			string hourAsString = hour.ToString("D2");

            String res = hourAsString + ":" + minAsString + ":" + secAsString;
            if (days > 0 || years > 0) {
                res = days + " day(s) " + res;
            }
			if (years > 0)
			{
				res = years + " year(s) " + res;
			}
			return res;
        }
    }
}
