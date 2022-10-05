namespace DTO.General.Home
{

    public class HomeDataItemInfo
    {
        public HomeDataItemInfo(decimal quantity) => Quantity = quantity;

        public decimal Quantity { get; set; }
    }
}
