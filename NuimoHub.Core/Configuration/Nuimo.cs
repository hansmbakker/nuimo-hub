namespace NuimoHub.Core.Configuration
{
    public class Nuimo
    {
        public string DeviceId { get; set; }
        public string Name { get; set; }

        public override bool Equals(object other)
        {
            if (other == null || other.GetType() != typeof(Nuimo))
            {
                return false;
            }

            var otherOptions = (Nuimo)other;

            return DeviceId.Equals(otherOptions.DeviceId)
                   && Name.Equals(otherOptions.Name);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((DeviceId != null ? DeviceId.GetHashCode() : 0) * 397) ^ (Name != null ? Name.GetHashCode() : 0);
            }
        }
    }
}