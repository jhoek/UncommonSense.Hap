namespace UncommonSense.Hap;

[Cmdlet(VerbsData.ConvertTo, "HtmlDocument", DefaultParameterSetName = ParameterSets.Uri)]
[OutputType(typeof(HtmlDocument))]
public class ConvertToHtmlDocumentCmdlet : PSCmdlet
{
    public static class ParameterSets
    {
        public const string Uri = nameof(Uri);
        public const string Text = nameof(Text);
    }

    [Parameter(Mandatory = true, Position = 0, ParameterSetName = ParameterSets.Uri)]
    public string[] Uri { get; set; }

    [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = ParameterSets.Text)]
    public string[] Text { get; set; }

    protected override void ProcessRecord()
    {
        var htmlWeb = new HtmlWeb();

        switch (ParameterSetName)
        {
            case ParameterSets.Text:
                WriteObject(
                    Text.Select(t =>
                    {
                        var result = new HtmlDocument();
                        result.LoadHtml(t);
                        return result;
                    }),
                    true);
                break;

            case ParameterSets.Uri:
                WriteObject(
                    Uri.Select(u => htmlWeb.Load(u)),
                    true
                );
                break;
        }
    }
}
