using System;
namespace CivilianPopulation.Domain
{
	public class ContractorMission
	{
		private readonly double missionEnd;
		private readonly CelestialBodyType missionTo;

		public ContractorMission(
			double missionEnd,
			CelestialBodyType missionTo
		)
		{
			this.missionEnd = missionEnd;
			this.missionTo = missionTo;
		}
		public ContractorMission(
			string missionEnd,
			CelestialBodyType missionTo
		) : this(Convert.ToDouble(missionEnd), missionTo)
		{
		}

		public CelestialBodyType getBody()
		{
			return this.missionTo;
		}
		public double getEndDate()
		{
			return this.missionEnd;
		}

		public override bool Equals(object obj)
		{
			var that = obj as ContractorMission;

			if (that == null)
			{
				return false;
			}

			return this.missionEnd.Equals(that.missionEnd) && this.missionTo.Equals(that.missionTo);
		}

		public override int GetHashCode()
		{
			return this.missionEnd.GetHashCode();
		}

	}
}
