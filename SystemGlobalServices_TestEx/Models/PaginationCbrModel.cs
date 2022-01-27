using System;
using SystemGlobalServices_TestEx.Interfaces;

namespace SystemGlobalServices_TestEx.Models
{
    public class PaginationCbrModel : IPaginationCbrModel
    {
        public int PageNumber { get; private set; }
        public int TotalPages { get; private set; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;

        public PaginationCbrModel(int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }

    }
}
