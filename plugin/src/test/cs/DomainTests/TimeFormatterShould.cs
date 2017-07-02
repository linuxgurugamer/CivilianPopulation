using System;
using NUnit.Framework;

namespace CivilianPopulation.Domain
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
            Assert.AreEqual("1 year(s) 74 day(s) 00:00:00", formatter.format(10800000));
		}
	}
}
