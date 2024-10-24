

using Infrastructure.shared.CustomExceptions;
using Infrastructure.shared.Wrapper;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.shared.Extensions;

public static class QueryableExtensions
{
    public static async Task<PaginatedResult<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, int pageNumber,
        int pageSize)
        where T : class
    {
        if (source == null)
        {
            throw new ApiException("List for pagination cannot be null");
        }

        pageNumber = pageNumber == 0 ? 1 : pageNumber;
        pageSize = pageSize == 0 ? 10 : pageSize;
        int count = await source.CountAsync();
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        List<T> items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        return PaginatedResult<T>.Success(items, count, pageNumber, pageSize);
    }

    public static IQueryable<T> GetPage<T>(this IQueryable<T> list, int pageNumber, int pageSize)
    {
        return list
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
    }

    public static IQueryable<T> TrackChanges<T>(this IQueryable<T> list, bool trackChanges = false)
        where T : class
    {
        if (trackChanges)
        {
            return list;
        }

        return list
            .AsNoTracking();
    }
}
