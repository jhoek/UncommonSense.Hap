using System.Text.RegularExpressions;

namespace UncommonSense.Hap;

public static class ExtensionMethods
{
    public static IEnumerable<T> ToEnumerable<T>(this T item)
    {
        yield return item;
    }

    public static void AddRange<T>(this PSMemberInfoCollection<T> collection, IEnumerable<T> items) where T : PSMemberInfo
    {
        foreach (var item in items)
            collection.Add(item);
    }

    public static string RegexReplace(this string input, string pattern, string replacement) =>
        Regex.Replace(input, pattern, replacement);

    public static string JoinString<T>(this IEnumerable<T> items, string separator) =>
        string.Join(separator, items);

    public static string GetHtmlNodeText(
        this IEnumerable<HtmlNode> nodes,
        bool directInnerTextOnly = false,
        bool skipDeentitize = false,
        bool skipTrim = false,
        bool skipRemoveLineBreaks = false,
        bool SkipFlattenWhitespace = false,
        bool allowEmptyStrings = false,
        string separator = " ",
        Action<string> writeVerbose = null
    ) =>
        nodes
            .Select(n => directInnerTextOnly ? n.GetDirectInnerText() : n.InnerText)
            .WriteVerbose(writeVerbose, "Raw values")
            .Select(t => skipDeentitize ? t : HttpUtility.HtmlDecode(t))
            .WriteVerbose(writeVerbose, "Deentitized")
            .Select(t => skipTrim ? t : t.Trim())
            .WriteVerbose(writeVerbose, "Trimmed")
            .Select(t => skipRemoveLineBreaks ? t : t.RegexReplace(@"\n", ""))
            .WriteVerbose(writeVerbose, "Without line breaks")
            .Select(t => SkipFlattenWhitespace ? t : t.RegexReplace(@"\s{2,}", " "))
            .WriteVerbose(writeVerbose, "Flatted whitespace")
            .Where(t => allowEmptyStrings || !string.IsNullOrEmpty(t))
            .WriteVerbose(writeVerbose, "Omitted empty values")
            .JoinString(separator);

    public static IEnumerable<T> WriteVerbose<T>(this IEnumerable<T> items, Action<string> writeVerbose, string message)
    {
        writeVerbose?.Invoke($"{message}: {items.JoinString(", ")}");
        return items;
    }
}