using DTO.General.Pagination.Input;

namespace DTO.Intra.Employee.Input
{
    public class EmployeeListInput : PaginatorInput
    {
        public EmployeeFiltersInput Filters { get; set; }
        public PaginatorInput Paginator { get; set; }
    }
}
