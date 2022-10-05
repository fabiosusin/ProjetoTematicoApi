using DTO.General.Pagination.Input;

namespace DTO.Intra.User.Input
{
    public class UserListInput : PaginatorInput
    {
        public UserListInput() { }
        public UserListInput(int page, int result) => Paginator = new(page, result);
        public UserListInput(UserFiltersInput input, int page, int result)
        {
            Filters = input;
            Paginator = new(page, result);
        }

        public UserFiltersInput Filters { get; set; }
        public PaginatorInput Paginator { get; set; }
    }
}
