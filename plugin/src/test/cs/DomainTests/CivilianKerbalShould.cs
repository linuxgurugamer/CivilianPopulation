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
            CivilianKerbal kerbal = new CivilianKerbal("Jeb", "Pilot", true, -1);
			Assert.AreEqual(
                "{\"expectingBirthAt\":-1, \"name\":\"Jeb\", \"male\":true, \"trait\":\"Pilot\"}",
                JSON.JsonEncode(kerbal.toTable())
			);
		}
	}
}
