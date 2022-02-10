using Shared;

namespace ConfigurationProvider.Services
{
    public interface IFeatureRepository
    {
        IEnumerable<FeatureToggle> GetFeatureToggles(string appName);
        
        void SaveFeatureToggles(string appName, IEnumerable<FeatureToggle> featuretoggles);
    }
}
