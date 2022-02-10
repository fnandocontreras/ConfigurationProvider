namespace Shared
{
    public class FeatureToggles
    {
        public IEnumerable<FeatureToggle> Toggles { get; set; }
    }

    public class FeatureToggle
    {
        public string Name { get; set; }
        public IEnumerable<int> CustomerIds { get; set; }
    }
}
