namespace UncommonSense.Hap;

[Cmdlet(VerbsCommon.Select, "HtmlNode")]
[OutputType(typeof(HtmlNode), ParameterSetName = new[] { ParameterSets.XPath })]
public class SelectHtmlNodeCmdlet : PSCmdlet
{
    public static class ParameterSets
    {
        public const string XPath = nameof(XPath);
        public const string CssSelector = nameof(CssSelector);
    }

    [Parameter(Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
    [Alias("DocumentNode")]
    public HtmlNode[] InputObject { get; set; }

    [Parameter(Mandatory = true, ParameterSetName = ParameterSets.XPath)]
    public string XPath { get; set; }

    [Parameter(Mandatory = true, ParameterSetName = ParameterSets.CssSelector)]
    public string CssSelector { get; set; }

    [Parameter()]
    public SwitchParameter All { get; set; }

    protected override void ProcessRecord() =>
        WriteObject(
            InputObject.SelectMany(i => ProcessInputObject(i)),
            true
        );

    protected IEnumerable<HtmlNode> ProcessInputObject(HtmlNode inputObject)
    {
        switch (ParameterSetName)
        {
            case ParameterSets.XPath when !All:
                yield return inputObject.SelectSingleNode(XPath);
                break;

            case ParameterSets.XPath when All:
                foreach (var node in inputObject.SelectNodes(XPath) ?? Enumerable.Empty<HtmlNode>())
                    yield return node;
                break;

            case ParameterSets.CssSelector when !All:
                var result = inputObject.QuerySelector(CssSelector);
                if (result is not null) yield return result;
                break;

            case ParameterSets.CssSelector when All:
                foreach (var node in inputObject.QuerySelectorAll(CssSelector))
                    yield return node;
                break;

            default:
                yield break;
        }
    }
}