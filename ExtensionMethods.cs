namespace UncommonSense.Hap;

public static class ExtensionMethods
{
    public static IEnumerable<T> ToEnumerable<T>(this T item)
    {
        yield return item;
    }
}