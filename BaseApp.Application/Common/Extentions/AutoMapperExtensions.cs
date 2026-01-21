using AutoMapper;

namespace BaseApp.Application.Common.Extentions
{
    public static class AutoMapperExtensions
    {
        public static void MapCollection<TSource, TDest, TKey>(
            this IMapper mapper,
            IEnumerable<TSource> sources,
            ICollection<TDest> destinations,
            Func<TSource, TKey> sourceKeySelector,
            Func<TDest, TKey> destKeySelector,
            Action<TSource, TDest> customMapping = null)
            where TDest : class, new()
        {
            var destDict = destinations.ToDictionary(destKeySelector);

            foreach (var source in sources)
            {
                var key = sourceKeySelector(source);
                if (destDict.TryGetValue(key, out var dest))
                {
                    mapper.Map(source, dest);
                    customMapping?.Invoke(source, dest);
                }
                else
                {
                    var newItem = mapper.Map<TDest>(source);
                    destinations.Add(newItem);
                }
            }
        }
    }
}
