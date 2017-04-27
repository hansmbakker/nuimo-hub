namespace NuimoHub.Core.Configuration
{
    public class HueBridge
    {
        public string Ip { get; set; }
        public string AppKey { get; set; }

        public override bool Equals(object other)
        {
            if (other == null || other.GetType() != typeof(HueBridge))
            {
                return false;
            }

            var otherOptions = (HueBridge)other;

            return Ip.Equals(otherOptions.Ip)
                   && AppKey.Equals(otherOptions.AppKey);
        }
        
        public override int GetHashCode()
        {
            unchecked
            {
                return ((Ip != null ? Ip.GetHashCode() : 0) * 397) ^ (AppKey != null ? AppKey.GetHashCode() : 0);
            }
        }
    }
}