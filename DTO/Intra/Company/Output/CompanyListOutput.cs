using DTO.General.Base.Api.Output;
using System.Collections.Generic;

namespace DTO.Intra.Company.Output
{
    public class CompanyListOutput : BaseApiOutput
    {
        public CompanyListOutput(string msg) : base(msg) { }
        public CompanyListOutput(IEnumerable<Database.Company> companies) : base(true) => Companies = companies;
        public IEnumerable<Database.Company> Companies { get; set; }
    }
}
