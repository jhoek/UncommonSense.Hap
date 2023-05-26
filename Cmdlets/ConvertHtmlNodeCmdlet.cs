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
    public SwitchParameter DirectInnerTextOnly { get; set; }

    [Parameter()]
    public SwitchParameter SkipDeentitize { get; set; }

    [Parameter()]
    public SwitchParameter SkipTrim { get; set; }

    [Parameter()]
    public SwitchParameter SkipRemoveLineBreaks { get; set; }

    [Parameter()]
    public SwitchParameter SkipFlattenWhitespace { get; set; }

    [Parameter()]
    [ValidateNotNull()]
    public string Separator { get; set; } = " ";

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

        var nodes = Mode switch
        {
            QueryMode.XPath => inputObject.SelectNodes(query),
            QueryMode.CssSelector => inputObject.QuerySelectorAll(query),
            _ => null,
        };

        var value = nodes.GetHtmlNodeText(
            DirectInnerTextOnly,
            SkipDeentitize,
            SkipTrim,
            SkipRemoveLineBreaks,
            SkipFlattenWhitespace,
            separator: Separator,
            writeVerbose: s => WriteVerbose(s)
        );

        return new(propertyName, value);
    }
}