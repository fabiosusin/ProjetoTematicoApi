using DTO.General.Base.Database;
using System;

namespace DTO.Intra.Interview.Database
{
    public class Interview : BaseData
    {
        public Guid PersonId { get; set; }
        public string PersonDocument { get; set; }
        public decimal FamilyIncome { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Complement { get; set; }
        public string Phone { get; set; }
        public string StreetNumber { get; set; }
        public string EducationDegree { get; set; }
        public bool ConsumesAlcohol { get; set; }
        public int WorkSkills { get; set; }
        public int ServiceHours { get; set; }
    }
}
