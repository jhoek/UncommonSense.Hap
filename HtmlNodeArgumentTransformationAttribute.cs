namespace UncommonSense.Hap;

public class HtmlNodeArgumentTransformationAttribute : ArgumentTransformationAttribute
{
    public override object Transform(EngineIntrinsics engineIntrinsics, object inputData) => inputData switch
    {
        HtmlNode htmlNode => htmlNode,
        HtmlDocument htmlDocument => htmlDocument.DocumentNode,
        _ => inputData,
    };
}