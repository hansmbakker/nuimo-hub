using System.Collections.Generic;
using System.Linq;

namespace NuimoHub.Core.Configuration
{
    public class NuimoOptions
    {
        public NuimoOptions()
        {
            WhitelistedNuimos = new List<Nuimo>();
        }

        public HueOptions HueOptions { get; set; }
        public ChromecastOptions ChromecastOptions { get; set; }
        public List<Nuimo> WhitelistedNuimos { get; set; }

        public override bool Equals(object other)
        {
            if (other == null || other.GetType() != typeof(NuimoOptions))
            {
                return false;
            }

            var otherOptions = (NuimoOptions)other;

            var whitelistSet = new HashSet<Nuimo>(WhitelistedNuimos);
            var otherWhitelistSet = new HashSet<Nuimo>(otherOptions.WhitelistedNuimos);

            return HueOptions.Equals(otherOptions.HueOptions)
                   && ChromecastOptions.Equals(otherOptions.ChromecastOptions)
                   && whitelistSet.SetEquals(otherWhitelistSet);
        }
        
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (HueOptions != null ? HueOptions.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ChromecastOptions != null ? ChromecastOptions.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (WhitelistedNuimos != null ? GetHashCodeForNuimos(WhitelistedNuimos) : 0);
                return hashCode;
            }
        }

        private static int GetHashCodeForNuimos(List<Nuimo> nuimos)
        {
            unchecked
            {
                int hash = 19;
                foreach (var nuimo in nuimos)
                {
                    hash = hash * 31 + nuimo.GetHashCode();
                }
                return hash;
            }
        }
    }
}