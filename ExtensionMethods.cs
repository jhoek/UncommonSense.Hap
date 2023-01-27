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
        string separator = " "
    ) =>
        nodes
            .Select(n => directInnerTextOnly ? n.GetDirectInnerText() : n.InnerText)
            .Select(t => skipDeentitize ? t : HttpUtility.HtmlDecode(t))
            .Select(t => skipTrim ? t : t.Trim())
            .Select(t => skipRemoveLineBreaks ? t : t.RegexReplace(@"\n", ""))
            .Select(t => SkipFlattenWhitespace ? t : t.RegexReplace(@"\s{2,}", " "))
            .Where(t => allowEmptyStrings || !string.IsNullOrEmpty(t))
            .JoinString(separator);
}