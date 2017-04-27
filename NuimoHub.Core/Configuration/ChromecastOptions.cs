namespace NuimoHub.Core.Configuration
{
    public class ChromecastOptions
    {
        public string Ip { get; set; }
        public string Name { get; set; }

        public override bool Equals(object other)
        {
            if (other == null || other.GetType() != typeof(ChromecastOptions))
            {
                return false;
            }

            var otherOptions = (ChromecastOptions)other;

            return Ip.Equals(otherOptions.Ip)
                   && Name.Equals(otherOptions.Name);
        }
        
        public override int GetHashCode()
        {
            unchecked
            {
                return ((Ip != null ? Ip.GetHashCode() : 0) * 397) ^ (Name != null ? Name.GetHashCode() : 0);
            }
        }
    }
}