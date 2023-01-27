namespace UncommonSense.Hap;

[Cmdlet(VerbsCommon.Get, "HtmlNodeText")]
[OutputType(typeof(string))]
public class GetHtmlNodeTextCmdlet : Cmdlet
{
    [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0)]
    public HtmlNode[] HtmlNode { get; set; }

    [Parameter()] public SwitchParameter DirectInnerTextOnly { get; set; }
    [Parameter()] public SwitchParameter SkipDeentitize { get; set; }
    [Parameter()] public SwitchParameter SkipTrim { get; set; }
    [Parameter()] public SwitchParameter SkipRemoveLineBreaks { get; set; }
    [Parameter()] public SwitchParameter SkipFlattenWhitespace { get; set; }
    [Parameter()] public SwitchParameter AllowEmptyStrings { get; set; }
    [Parameter()] public string Separator { get; set; } = " ";

    protected List<HtmlNode> innerList;

    protected override void BeginProcessing()
    {
        innerList = new List<HtmlNode>();
    }

    protected override void ProcessRecord()
    {
        innerList.AddRange(HtmlNode);
    }

    protected override void EndProcessing()
    {
        WriteObject(
            innerList.GetHtmlNodeText(
                DirectInnerTextOnly,
                SkipDeentitize,
                SkipTrim,
                SkipRemoveLineBreaks,
                SkipFlattenWhitespace,
                AllowEmptyStrings,
                Separator
            )
        );
    }
}