using DTO.General.Pagination.Input;

namespace DTO.Intra.Person.Input
{
    public class PersonListInput : PaginatorInput
    {
        public PersonFiltersInput Filters { get; set; }
        public PaginatorInput Paginator { get; set; }
    }
}
