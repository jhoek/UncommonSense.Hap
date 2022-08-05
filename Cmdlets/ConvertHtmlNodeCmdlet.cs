namespace UncommonSense.Hap;

[Cmdlet(VerbsData.Convert, "HtmlNode")]
public class ConvertHtmlNodeCmdlet : PSCmdlet
{
    public enum QueryMode
    {
        XPath,
        CssSelector
    }

    [Parameter(Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
    public HtmlNode[] InputObject { get; set; }

    [Parameter(Mandatory = true, Position = 0)]
    public IDictionary Property { get; set; }

    [Parameter()]
    public QueryMode Mode { get; set; } = QueryMode.XPath;

    [Parameter()]
    [ValidateNotNull()]
    public string[] TypeName { get; set; } = Array.Empty<string>();

    [Parameter()]
    public SwitchParameter SkipTrim { get; set; }

    [Parameter()]
    public SwitchParameter SkipRemoveLineBreaks { get; set; }

    [Parameter()]
    public SwitchParameter SkipFlattenWhitespace { get; set; }

    protected override void ProcessRecord()
    {
        WriteObject(
            InputObject.Select(i => ProcessInputObject(i)),
            true
        );
    }

    protected PSObject ProcessInputObject(HtmlNode inputObject)
    {
        var result = new PSObject();

        result
            .Properties
            .AddRange(
                Property
                    .Keys
                    .Cast<string>()
                    .Select(p => GetProperty(inputObject, p, Property[p].ToString()))
            );

        TypeName
            .Reverse()
            .ToList()
            .ForEach(t => result.TypeNames.Insert(0, t));

        return result;
    }

    protected PSNoteProperty GetProperty(HtmlNode inputObject, string propertyName, string query)
    {
        WriteVerbose($"Building property '{propertyName}' using query '{query}'");

        var propertyValue = GetPropertyValue(inputObject, query);
        WriteVerbose($"Raw value is '{FormatPropertyValue(propertyValue)}'");

        propertyValue = propertyValue.Select(s => HtmlEntity.DeEntitize(s));
        WriteVerbose($"Value with converted entities is '{FormatPropertyValue(propertyValue)}'");

        if (!SkipTrim)
        {
            propertyValue = propertyValue.Select(s => s.Trim());
            WriteVerbose($"Trimmed value is '{FormatPropertyValue(propertyValue)}'");
        }

        if (!SkipRemoveLineBreaks)
        {
            propertyValue = propertyValue.Select(s => s.RegexReplace(@"\n", ""));
            WriteVerbose($"Value with line breaks removed is '{FormatPropertyValue(propertyValue)}'");
        }

        if (!SkipFlattenWhitespace)
        {
            propertyValue = propertyValue.Select(s => s.RegexReplace(@"\s{2,}", " "));
            WriteVerbose($"Value with whitespace flattened is '{FormatPropertyValue(propertyValue)}'");
        }

        switch (propertyValue.Skip(1).Any())
        {
            case true:
                WriteVerbose("Value is a list");
                return new(propertyName, propertyValue);

            case false:
                WriteVerbose("Value is a single string");
                return new(propertyName, propertyValue.FirstOrDefault());
        }
    }

    protected IEnumerable<string> GetPropertyValue(HtmlNode inputObject, string query) => Mode switch
    {
        QueryMode.XPath => inputObject.SelectNodes(query).Select(n => n.InnerText),
        QueryMode.CssSelector => inputObject.QuerySelectorAll(query).Select(n => n.InnerText),
        _ => null,
    };

    protected static string FormatPropertyValue(IEnumerable<string> propertyValue) =>
        string.Join(", ", propertyValue);
}