namespace UncommonSense.Hap;

[Cmdlet(VerbsCommon.Select, "HtmlNode")]
[OutputType(typeof(HtmlNode), ParameterSetName = new[] { ParameterSets.SelectSingleNode })]
public class SelectHtmlNodeCmdlet : PSCmdlet
{
    public static class ParameterSets
    {
        public const string SelectSingleNode = nameof(SelectSingleNode);
        public const string SelectNodes = nameof(SelectNodes);
    }

    [Parameter(Mandatory = true, ValueFromPipeline = true)]
    [HtmlNodeArgumentTransformation()]
    public HtmlNode[] InputObject { get; set; }

    [Parameter(Mandatory = true, ParameterSetName = ParameterSets.SelectSingleNode)]
    public string SelectSingleNode { get; set; }

    [Parameter(Mandatory = true, ParameterSetName = ParameterSets.SelectNodes)]
    public string SelectNodes { get; set; }

    protected override void ProcessRecord() =>
        WriteObject(
            InputObject.Select(i => ProcessInputObject(i)),
            true
        );

    protected IEnumerable<HtmlNode> ProcessInputObject(HtmlNode inputObject)
    {
        switch (ParameterSetName)
        {
            case ParameterSets.SelectSingleNode:
                return inputObject.SelectSingleNode(SelectSingleNode).ToEnumerable();
            case ParameterSets.SelectNodes:
                return inputObject.SelectNodes(SelectNodes);
            default:
                return Enumerable.Empty<HtmlNode>();
        }
    }
}