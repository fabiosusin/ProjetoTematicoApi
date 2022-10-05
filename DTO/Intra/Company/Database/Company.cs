using DTO.General.Base.Database;

namespace DTO.Intra.Company.Database
{
    public class Company : BaseData
    {
        public string Name { get; set; }
        public string Cnpj { get; set; }
    }
}
