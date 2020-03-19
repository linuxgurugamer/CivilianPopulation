namespace CivilianPopulation.Domain
{
    public class TimeUnit
    {
        public const int HOURS_PER_DAYS = 6;
        public const int DAYS_PER_YEARS = 426;
        public const int HOUR = 60 * 60;
        public const int DAY = HOURS_PER_DAYS * HOUR;
        public const int YEAR = DAYS_PER_YEARS * DAY;

        public TimeUnit()
        {
        }
    }
}
