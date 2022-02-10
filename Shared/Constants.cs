namespace Shared
{
    public class Constants
    {
        public static string DatabaseName = "FeatureTogglesDb";
        public static string TogglesPartitionName = "/FeatureTogglesPartition";
        public static string TogglesContainerName = "FeatureToggles";
        public static string GetDocumentId(string appName, string feature) => $"toggles:{appName}:{feature}";
    }
}
