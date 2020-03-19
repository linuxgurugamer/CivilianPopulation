namespace CivilianPopulation.Domain
{
    public class CelestialBody
    {
        private readonly string name;
        private readonly CelestialBodyType type;

        public CelestialBody(string name, CelestialBodyType type)
        {
            this.name = name;
            this.type = type;
        }

        public string getName()
        {
            return this.name;
        }

        public CelestialBodyType getType()
        {
            return this.type;
        }

        public override bool Equals(object obj)
        {
            var that = obj as CelestialBody;
            if (that == null)
            {
                return false;
            }
            return this.name.Equals(that.name);
        }

        public override int GetHashCode()
        {
            return this.name.GetHashCode();
        }
    }
}
