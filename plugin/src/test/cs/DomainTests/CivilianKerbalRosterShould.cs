using System;
using System.Collections;
using CivilianPopulation.Domain;
using NUnit.Framework;

namespace CivilianPopulation.DomainTests
{
    [TestFixture()]
    public class CivilianKerbalRosterShould
    {
        [Test()]
        public void encode_empty_hashtable()
        {
            CivilianKerbalRoster roster = new CivilianKerbalRoster();
            Assert.AreEqual(
                "{}",
                JSON.JsonEncode(new Hashtable())
            );
        }

        [Test()]
        public void encode_empty_roster()
        {
            CivilianKerbalRoster roster = new CivilianKerbalRoster();
            Assert.AreEqual(
                "{}",
                roster.toString()
            );
        }

		[Test()]
		public void decode_null_roster()
		{
			CivilianKerbalRoster roster = new CivilianKerbalRoster(null);
			Assert.AreEqual(0, roster.count());
		}

		[Test()]
		public void decode_empty_roster()
		{
			CivilianKerbalRoster roster = new CivilianKerbalRoster("");
			Assert.AreEqual(0, roster.count());
		}

		[Test()]
		public void decode_empty_roster_without_content()
		{
            CivilianKerbalRoster roster = new CivilianKerbalRoster("[]");
			Assert.AreEqual(0, roster.count());
		}

		[Test()]
		public void encode_non_empty_roster()
		{
			CivilianKerbalRoster roster = new CivilianKerbalRoster();
            roster.add(new CivilianKerbal("Jeb", "Pilot", true, false, -1, -1));
			Assert.AreEqual(
                "{\"Jeb\":{\"vesselId\":null, \"birthdate\":-1, \"name\":\"Jeb\", \"expectingBirthAt\":-1, \"male\":true, \"dead\":false, \"trait\":\"Pilot\"}}",
				roster.toString()
			);
		}

		[Test()]
		public void decode_non_empty_roster()
		{
			CivilianKerbalRoster roster = new CivilianKerbalRoster();
            roster.add(new CivilianKerbal("Jeb", "Pilot", true, false, -1, -1));
            roster.add(new CivilianKerbal("Val", "Pilot", false, false, -1, 123456));

            string json = roster.toString();

            CivilianKerbalRoster copy = new CivilianKerbalRoster(json);
            Assert.AreEqual(2, copy.count());
			Assert.False(copy.exists("Bob"));
			Assert.True(copy.exists("Jeb"));
			Assert.True(copy.exists("Val"));

			CivilianKerbal val = copy.get("Val");
			Assert.AreEqual("Val", val.getName());
			Assert.AreEqual("Pilot", val.getTrait());
			Assert.AreEqual(false, val.isMale());
			Assert.AreEqual(123456, val.getExpectingBirthAt());
		}

		[Test()]
        public void list_1_member()
        {
            CivilianKerbalRoster roster = new CivilianKerbalRoster();
            roster.add(new CivilianKerbal("Jeb", "Pilot", true, false, -1, -1));
            int count = 0;
            foreach (CivilianKerbal kerbal in roster.list())
            {
                count++;
            }
            Assert.AreEqual(1, count);
            Assert.AreEqual(1, roster.count());
        }

        [Test()]
        public void list_2_members()
        {
            CivilianKerbalRoster roster = new CivilianKerbalRoster();
            roster.add(new CivilianKerbal("Jeb", "Pilot", true, false, -1, -1));
            roster.add(new CivilianKerbal("Val", "Pilot", false, false, -1, -1));
            int count = 0;
            foreach (CivilianKerbal kerbal in roster.list())
            {
                count++;
            }
            Assert.AreEqual(2, count);
            Assert.AreEqual(2, roster.count());

        }

        [Test()]
        public void check_crew_exists()
        {
            CivilianKerbalRoster roster = new CivilianKerbalRoster();
            roster.add(new CivilianKerbal("Jeb", "Pilot", true, false, -1, -1));
            roster.add(new CivilianKerbal("Val", "Pilot", false, false, -1, -1));
            Assert.False(roster.exists("Bob"));
            Assert.True(roster.exists("Jeb"));
        }

        [Test()]
        public void remove_crew()
        {
            CivilianKerbalRoster roster = new CivilianKerbalRoster();
            roster.add(new CivilianKerbal("Jeb", "Pilot", true, false, -1, -1));
            roster.add(new CivilianKerbal("Val", "Pilot", false, false, -1, -1));
            roster.remove("Jeb");
            Assert.False(roster.exists("Jeb"));
        }

        [Test()]
        public void retreive_crew_by_name()
        {
            CivilianKerbalRoster roster = new CivilianKerbalRoster();
            roster.add(new CivilianKerbal("Jeb", "Pilot", true, false, -1, -1));
            roster.add(new CivilianKerbal("Val", "Pilot", false, false, -1, 123456));

            CivilianKerbal val = roster.get("Val");
			Assert.AreEqual("Val", val.getName());
            Assert.AreEqual("Pilot", val.getTrait());
            Assert.AreEqual(false, val.isMale());
            Assert.AreEqual(123456, val.getExpectingBirthAt());
		}
    }
}
