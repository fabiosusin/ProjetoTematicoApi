using DTO.General.Base.Api.Output;
using DTO.Intra.User.Database;
using System.Collections.Generic;

namespace DTO.Intra.User.Output
{
    public class UserListOutput : BaseApiOutput
    {
        public UserListOutput(string msg) : base(msg) { }
        public UserListOutput(IEnumerable<Database.AppUser> allys) : base(true) => Users = allys;
        public IEnumerable<Database.AppUser> Users { get; set; }
    }
}
