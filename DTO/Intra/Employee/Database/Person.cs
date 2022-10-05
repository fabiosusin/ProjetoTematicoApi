using DTO.General.Base.Database;

namespace DTO.Intra.Person.Database
{
    public class Person : BaseData
    {
        public string Name { get; set; }
        public string CpfCnpj { get; set; }
    }
}
