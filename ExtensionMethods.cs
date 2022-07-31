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
}