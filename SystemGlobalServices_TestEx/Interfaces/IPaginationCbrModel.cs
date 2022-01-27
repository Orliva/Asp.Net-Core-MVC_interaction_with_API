namespace SystemGlobalServices_TestEx.Interfaces
{
    public interface IPaginationCbrModel
    {
        int PageNumber { get; }
        int TotalPages { get; }
        bool HasPreviousPage => PageNumber > 1;
        bool HasNextPage => PageNumber < TotalPages;
    }
}
