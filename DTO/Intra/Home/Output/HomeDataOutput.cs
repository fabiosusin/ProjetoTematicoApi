using DTO.General.Base.Api.Output;
using DTO.General.Home;

namespace DTO.Intra.Home.Output
{
    public class HomeDataOutput : BaseApiOutput
    {
        public HomeDataOutput(decimal PersonQuantity) : base(true)
        {
            Person = new(PersonQuantity);
        }

        public HomeDataItemInfo Person { get; set; }
    }
}
