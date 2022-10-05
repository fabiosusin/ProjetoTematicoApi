using DTO.General.Base.Api.Output;
using System.Collections.Generic;

namespace DTO.Intra.Employee.Output
{
    public class EmployeeListOutput : BaseApiOutput
    {
        public EmployeeListOutput(string msg) : base(msg) { }
        public EmployeeListOutput(IEnumerable<Database.Employee> allys) : base(true) => Employees = allys;
        public IEnumerable<Database.Employee> Employees { get; set; }
    }
}
