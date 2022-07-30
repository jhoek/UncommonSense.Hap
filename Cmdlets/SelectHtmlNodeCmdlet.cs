namespace UncommonSense.Hap;

[Cmdlet(VerbsCommon.Select, "HtmlNode")]
[OutputType(typeof(HtmlNode), ParameterSetName = new[] { ParameterSets.SelectSingleNode })]
public class SelectHtmlNodeCmdlet : PSCmdlet
{
    public static class ParameterSets
    {
        public const string SelectSingleNode = nameof(SelectSingleNode);
        public const string SelectNodes = nameof(SelectNodes);
        public const string CssSelector = nameof(CssSelector);
    }

    [Parameter(Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
    [Alias("DocumentNode")]
    public HtmlNode[] InputObject { get; set; }

    [Parameter(Mandatory = true, ParameterSetName = ParameterSets.SelectSingleNode)]
    public string SelectSingleNode { get; set; }

    [Parameter(Mandatory = true, ParameterSetName = ParameterSets.SelectNodes)]
    public string SelectNodes { get; set; }

    [Parameter(Mandatory = true, ParameterSetName = ParameterSets.CssSelector)]
    public string CssSelector { get; set; }

    protected override void ProcessRecord() =>
        WriteObject(
            InputObject.SelectMany(i => ProcessInputObject(i)),
            true
        );

    protected IEnumerable<HtmlNode> ProcessInputObject(HtmlNode inputObject)
    {
        switch (ParameterSetName)
        {
            case ParameterSets.SelectSingleNode:
                yield return inputObject.SelectSingleNode(SelectSingleNode);
                break;

            case ParameterSets.SelectNodes:
                foreach (var node in inputObject.SelectNodes(SelectNodes) ?? Enumerable.Empty<HtmlNode>())
                    yield return node;
                break;

            case ParameterSets.CssSelector:
                var result = inputObject.QuerySelector(CssSelector);
                if (result is not null) yield return result;
                break;

            default:
                yield break;
        }
    }
}