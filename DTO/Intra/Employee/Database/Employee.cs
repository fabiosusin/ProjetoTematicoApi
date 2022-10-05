using DTO.General.Base.Database;

namespace DTO.Intra.Employee.Database
{
    public class Employee : BaseData
    {
        public string Name { get; set; }
        public string CpfCnpj { get; set; }
    }
}
