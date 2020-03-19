using CivilianPopulation.Domain;
using System;

namespace CivilianPopulation.GUI
{
    public enum TimeFormat
    {
        TIME,
        AGE
    }

    public class TimeFormatter
    {
        public string format(double time, TimeFormat format)
        {
            string res;
            if (format == TimeFormat.AGE)
            {
                res = this.formatAge(time);
            }
            else
            {
                res = this.format(time);
            }
            return res;
        }

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
            if (hour >= TimeUnit.HOURS_PER_DAYS)
            {
                days = hour / TimeUnit.HOURS_PER_DAYS;
                hour = hour - days * TimeUnit.HOURS_PER_DAYS;
            }
            if (days >= TimeUnit.DAYS_PER_YEARS)
            {
                years = days / TimeUnit.DAYS_PER_YEARS;
                days = days - years * TimeUnit.DAYS_PER_YEARS;
            }

            string secAsString = sec.ToString("D2");
            string minAsString = min.ToString("D2");
            string hourAsString = hour.ToString("D2");

            String res = hourAsString + ":" + minAsString + ":" + secAsString;
            if (days > 0 || years > 0)
            {
                if (days > 1)
                {
                    res = days + " day(s) " + res;
                }
                else
                {
                    res = days + " day " + res;
                }
            }
            if (years > 0)
            {
                if (years > 1)
                {
                    res = years + " year(s) " + res;
                }
                else
                {
                    res = years + " year " + res;
                }
            }
            return res;
        }

        public string formatAge(double time)
        {
            string res;
            if (time < 1 * TimeUnit.YEAR)
            {
                res = "Young (" + (int)(time / TimeUnit.DAY) + " days)";
            }
            else if (time < 15 * TimeUnit.YEAR)
            {
                res = "Young (" + (int)(time / TimeUnit.YEAR) + " years)";
            }
            else if (time < 30 * TimeUnit.YEAR)
            {
                res = "Young adult (" + (int)(time / TimeUnit.YEAR) + " years)";
            }
            else if (time < 45 * TimeUnit.YEAR)
            {
                res = "Adult (" + (int)(time / TimeUnit.YEAR) + " years)";
            }
            else if (time < 60 * TimeUnit.YEAR)
            {
                res = "Senior (" + (int)(time / TimeUnit.YEAR) + " years)";
            }
            else
            {
                res = "Ancient (" + (int)(time / TimeUnit.YEAR) + " years)";
            }
            return res;
        }
    }
}
