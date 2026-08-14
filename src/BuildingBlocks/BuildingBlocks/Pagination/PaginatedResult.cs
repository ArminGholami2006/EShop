namespace BuildingBlocks.Pagination;

public class PaginatedResult<TEntity>(int PageIndex, int PageSize, long Count, IEnumerable<TEntity> Data) where TEntity : class
{
    public int PageIndex { get; } = PageIndex;
    public int PageSize { get; } = PageSize;
    public long Count { get; } = Count;
    public IEnumerable<TEntity> Data { get; } = Data;
}
