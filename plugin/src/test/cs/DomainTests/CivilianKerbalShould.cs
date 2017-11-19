using System;
using CivilianPopulation.Domain;
using NUnit.Framework;

namespace CivilianPopulation.DomainTests
{
	[TestFixture()]
	public class CivilianKerbalShould
    {
		[Test()]
        public void turn_into_string() 
        {
            CivilianKerbal kerbal = new CivilianKerbal("Jeb", "Pilot", true, false, -1, -1);
			Assert.AreEqual(
                "{\"vesselId\":null, \"birthdate\":-1, \"name\":\"Jeb\", \"expectingBirthAt\":-1, \"male\":true, \"dead\":false, \"trait\":\"Pilot\"}",
                JSON.JsonEncode(kerbal.toTable())
			);
		}
	}
}
