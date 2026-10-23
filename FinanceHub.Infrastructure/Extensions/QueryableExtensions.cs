using FinanceHub.Application.Common;
using Microsoft.EntityFrameworkCore;

namespace FinanceHub.Infrastructure.Extensions;

public static class QueryableExtensions
{
    public const int MinPageSize = 1;
    public const int MaxPageSize = 50;

    public static async Task<PagedResult<T>> ToPagedListAsync<T>(
        this IQueryable<T> source,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        NormalizePaging(ref pageIndex, ref pageSize);

        var count = await source.CountAsync(cancellationToken);
        var items = await source
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<T>(items, count, pageIndex, pageSize);
    }

    public static async Task<PagedResult<T>> ToPagedListAsync<T>(
        this IQueryable<T> source,
        int pageIndex,
        int pageSize,
        int totalCount,
        CancellationToken cancellationToken = default)
    {
        NormalizePaging(ref pageIndex, ref pageSize);

        var items = await source
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<T>(items, totalCount, pageIndex, pageSize);
    }

    private static void NormalizePaging(ref int pageIndex, ref int pageSize)
    {
        pageIndex = Math.Max(1, pageIndex);
        pageSize = Math.Clamp(pageSize, MinPageSize, MaxPageSize);
    }
}
