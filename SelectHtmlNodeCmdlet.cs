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

    [Parameter(Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
    [Alias("DocumentNode")]
    public HtmlNode[] InputObject { get; set; }

    [Parameter(Mandatory = true, ParameterSetName = ParameterSets.SelectSingleNode)]
    public string SelectSingleNode { get; set; }

    [Parameter(Mandatory = true, ParameterSetName = ParameterSets.SelectNodes)]
    public string SelectNodes { get; set; }

    protected override void ProcessRecord() =>
        InputObject
            .SelectMany(i => ProcessInputObject(i))
            .ToList()
            .ForEach(i => WriteObject(i));

    protected IEnumerable<HtmlNode> ProcessInputObject(HtmlNode inputObject)
    {
        switch (ParameterSetName)
        {
            case ParameterSets.SelectSingleNode:
                yield return inputObject.SelectSingleNode(SelectSingleNode);
                break;

            case ParameterSets.SelectNodes:
                foreach (var node in inputObject.SelectNodes(SelectNodes))
                    yield return node;
                break;

            default:
                yield break;
        }
    }
}