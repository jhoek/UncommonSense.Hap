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
        return new PSNoteProperty(
            propertyName,
            GetPropertyValue(inputObject, query)
        );
    }

    protected object GetPropertyValue(HtmlNode inputObject, string query) => Mode switch
    {
        QueryMode.XPath => inputObject.SelectNodes(query).Select(n => n.GetDirectInnerText()),
        QueryMode.CssSelector => inputObject.QuerySelectorAll(query).Select(n => n.GetDirectInnerText()),
        _ => null,
    };
}