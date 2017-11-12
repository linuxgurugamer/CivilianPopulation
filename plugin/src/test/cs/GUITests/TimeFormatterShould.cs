using System;
using NUnit.Framework;

namespace CivilianPopulation.GUI
{
	[TestFixture()]
	public class TimeFormatterShould
    {
		[Test()]
		public void Format_0_to_0_sec()
		{
			TimeFormatter formatter = new TimeFormatter();
			Assert.AreEqual("00:00:00", formatter.format(0));
		}
		[Test()]
		public void Format_double_to_int_sec()
		{
			TimeFormatter formatter = new TimeFormatter();
			Assert.AreEqual("00:00:02", formatter.format(2.43));
		}
		[Test()]
		public void Format_60_into_1_m_0_s()
		{
			TimeFormatter formatter = new TimeFormatter();
			Assert.AreEqual("00:01:00", formatter.format(60));
		}
		[Test()]
		public void Format_time_counting_hours()
		{
			TimeFormatter formatter = new TimeFormatter();
			Assert.AreEqual("04:03:43", formatter.format(14623));
		}
		[Test()]
		public void Format_time_counting_days()
		{
			TimeFormatter formatter = new TimeFormatter();
            Assert.AreEqual("37 day(s) 04:03:43", formatter.format(813823));
		}
		[Test()]
		public void Format_100_days()
		{
			TimeFormatter formatter = new TimeFormatter();
            Assert.AreEqual("100 day(s) 00:00:00", formatter.format(2160000));
		}
		[Test()]
		public void Format_500_days()
		{
			TimeFormatter formatter = new TimeFormatter();
            Assert.AreEqual("1 year 74 day(s) 00:00:00", formatter.format(10800000));
		}
        [Test()]
        public void Format_age_for_100_days()
        {
            TimeFormatter formatter = new TimeFormatter();
            Assert.AreEqual("Young (100 days)", formatter.format(2160000, TimeFormat.AGE));
        }

        [Test()]
        public void Format_age_for_10_years()
        {
            TimeFormatter formatter = new TimeFormatter();
            Assert.AreEqual("Young (10 years)", formatter.format(21600 * 426 * 10, TimeFormat.AGE));
        }

        [Test()]
        public void Format_age_for_20_years()
        {
            TimeFormatter formatter = new TimeFormatter();
            Assert.AreEqual("Young adult (20 years)", formatter.format(21600 * 426 * 20, TimeFormat.AGE));
        }

        [Test()]
        public void Format_age_for_35_years()
        {
            TimeFormatter formatter = new TimeFormatter();
            Assert.AreEqual("Adult (35 years)", formatter.format(21600 * 426 * 35, TimeFormat.AGE));
        }

        [Test()]
        public void Format_age_for_35_years_and_few_seconds()
        {
            TimeFormatter formatter = new TimeFormatter();
            Assert.AreEqual("Adult (35 years)", formatter.format(21600 * 426 * 35+1, TimeFormat.AGE));
        }

        [Test()]
        public void Format_age_for_50_years()
        {
            TimeFormatter formatter = new TimeFormatter();
            Assert.AreEqual("Senior (50 years)", formatter.format(21600 * 426 * 50, TimeFormat.AGE));
        }

        [Test()]
        public void Format_age_for_65_years()
        {
            TimeFormatter formatter = new TimeFormatter();
            Assert.AreEqual("Ancient (65 years)", formatter.format(21600 * 426 * 65, TimeFormat.AGE));
        }

    }
}
