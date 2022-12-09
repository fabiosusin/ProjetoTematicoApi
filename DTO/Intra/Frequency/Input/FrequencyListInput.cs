using DTO.General.Pagination.Input;

namespace DTO.Intra.Frequency.Input
{
    public class FrequencyListInput : PaginatorInput
    {
        public FrequencyFiltersInput Filters { get; set; }
        public PaginatorInput Paginator { get; set; }
    }
}
