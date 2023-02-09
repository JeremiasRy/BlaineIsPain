public static class Extension
{
    public static int IndexOf<TSource>(this IEnumerable<TSource> source, Predicate<TSource> predicate)
    {
        int i = 0;
        foreach (TSource item in source)
        {
            if (predicate(item))
            {
                return i;
            }
            i++;
        }
        return -1;
    }
}

