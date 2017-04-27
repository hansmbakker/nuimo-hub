namespace NuimoHub.Core.Configuration
{
    public class HueOptions
    {
        public HueBridge Bridge { get; set; }

        public string SceneId1 { get; set; }
        public string SceneId2 { get; set; }
        public string SceneId3 { get; set; }

        public override bool Equals(object other)
        {
            if (other == null || other.GetType() != typeof(HueOptions))
            {
                return false;
            }

            var otherOptions = (HueOptions) other;

            return Bridge.Equals(otherOptions.Bridge)
                   && SceneId1.Equals(otherOptions.SceneId1)
                   && SceneId2.Equals(otherOptions.SceneId2)
                   && SceneId3.Equals(otherOptions.SceneId3);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Bridge != null ? Bridge.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (SceneId1 != null ? SceneId1.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (SceneId2 != null ? SceneId2.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (SceneId3 != null ? SceneId3.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}