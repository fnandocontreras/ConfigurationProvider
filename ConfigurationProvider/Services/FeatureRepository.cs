using Microsoft.Extensions.Caching.Memory;
using Shared;

namespace ConfigurationProvider.Services
{
    public class FeatureRepository : IFeatureRepository
    {
        private readonly IMemoryCache _cache;
        public FeatureRepository(IMemoryCache cache)
        {
            _cache = cache;
        }

        public IEnumerable<FeatureToggle> GetFeatureToggles(string appName)
        {
            if (_cache.TryGetValue<IEnumerable<FeatureToggle>>(appName, out var featureToggles))
                return featureToggles;

            return Enumerable.Empty<FeatureToggle>();
        }

        public void SaveFeatureToggles(string appName, IEnumerable<FeatureToggle> featureToggles)
        {
            _cache.Set(appName, featureToggles);
        }
        
    }
}
