namespace GraduationProjectAPI.DataTransferObject
{
    public class PaginatedListModel<T>
    {
        public List<T>? Items { get; set; } = null;
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public PaginatedListModel(List<T>? items, int pageNumber, int pageSize, int totalCount)
        {
            Items = items;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;
        }
    }
}
