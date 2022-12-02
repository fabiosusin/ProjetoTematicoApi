using DTO.General.Base.Database;
using System;

namespace DTO.Intra.Interview.Database
{
    public class Interview : BaseData
    {
        public decimal FamilyIncome { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Complement { get; set; }
        public string Phone { get; set; }
        public string StreetNumber { get; set; }
        public string EducationDegree { get; set; }
        public int WorkSkills { get; set; }
        public bool ConsumesAlcohol { get; set; }
        public DateTime ServiceHours { get; set; }
    }
}
