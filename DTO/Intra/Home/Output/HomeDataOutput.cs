using DTO.General.Base.Api.Output;
using DTO.General.Home;

namespace DTO.Intra.Home.Output
{
    public class HomeDataOutput : BaseApiOutput
    {
        public HomeDataOutput(decimal employeeQuantity) : base(true)
        {
            Employee = new(employeeQuantity);
        }

        public HomeDataItemInfo Employee { get; set; }
    }
}
