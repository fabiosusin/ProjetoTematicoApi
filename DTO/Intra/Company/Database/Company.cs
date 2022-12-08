using DTO.General.Base.Database;
using System;

namespace DTO.Intra.Company.Database
{
    public class Company : BaseData
    {
        public string Name { get; set; }
        public string Cnpj { get; set; }
        public string SocialReason { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime RegisterDate { get; set; }
        public DateTime DisqualificationDate { get; set; }
    }
}
