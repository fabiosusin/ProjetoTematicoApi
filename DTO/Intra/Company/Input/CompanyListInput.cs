using DTO.General.Pagination.Input;

namespace DTO.Intra.Company.Input
{
    public class CompanyListInput : PaginatorInput
    {
        public CompanyFiltersInput Filters { get; set; }
        public PaginatorInput Paginator { get; set; }
    }
}
