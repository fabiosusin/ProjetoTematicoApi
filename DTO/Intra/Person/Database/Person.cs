using DTO.General.Base.Database;
using System;

namespace DTO.Intra.Person.Database
{
    public class Person : BaseData
    {
        public string Name { get; set; }
        public string CpfCnpj { get; set; }
        public string Naturally { get; set; }
        public string MotherName { get; set; }
        public string MaritalStatus { get; set; }
        public DateTime BirthDay { get; set; }
    }
}
