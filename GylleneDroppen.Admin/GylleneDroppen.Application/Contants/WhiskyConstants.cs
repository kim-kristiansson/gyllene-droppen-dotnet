namespace GylleneDroppen.Core.Constants;

public static class WhiskyConstants
{
    public static readonly List<string> WhiskyTypes = new()
    {
        "Single Malt",
        "Blended Scotch",
        "Blended Malt",
        "Single Grain",
        "Bourbon",
        "Rye",
        "Irish Single Malt",
        "Irish Blend",
        "Japanese Single Malt",
        "Japanese Blend",
        "Canadian",
        "Other"
    };

    public static readonly List<string> ScotchRegions = new()
    {
        "Speyside",
        "Highlands",
        "Lowlands",
        "Islay",
        "Campbeltown",
        "Islands"
    };

    public static readonly List<string> Countries = new()
    {
        "Scotland",
        "Ireland",
        "United States",
        "Japan",
        "Canada",
        "India",
        "Taiwan",
        "Australia",
        "France",
        "Germany",
        "Sweden",
        "Other"
    };

    public static readonly List<string> CommonRegions = new()
    {
        // Scotland
        "Speyside", "Highlands", "Lowlands", "Islay", "Campbeltown", "Islands",
        // Ireland
        "Northern Ireland", "Republic of Ireland",
        // USA
        "Kentucky", "Tennessee", "Pennsylvania", "New York",
        // Japan
        "Honshu", "Hokkaido",
        // Other
        "N/A", "Various", "Other"
    };
}