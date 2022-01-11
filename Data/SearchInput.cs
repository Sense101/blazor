namespace Blazor.Data;

// NOTE: this is not under models because it isn't saved to the database, I thought it best to keep them seperate

public class SearchInput
{
    public string NameFilter { get; set; } = "";
    public string DescFilter { get; set; } = "";
    public Dictionary<string, string> Query { get; set; } = new();
}