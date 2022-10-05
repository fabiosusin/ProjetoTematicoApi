using DTO.General.Base.Api.Output;
using DTO.General.Home;

namespace DTO.Intra.Home.Output
{
    public class HomeDataOutput : BaseApiOutput
    {
        public HomeDataOutput(decimal personsQuantity, decimal companiesQuantity) : base(true)
        {
            Person = new(personsQuantity);
            Company = new(companiesQuantity);
        }

        public HomeDataItemInfo Person { get; set; }
        public HomeDataItemInfo Company{ get; set; }
    }
}
