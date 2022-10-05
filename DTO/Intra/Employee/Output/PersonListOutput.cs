using DTO.General.Base.Api.Output;
using System.Collections.Generic;

namespace DTO.Intra.Person.Output
{
    public class PersonListOutput : BaseApiOutput
    {
        public PersonListOutput(string msg) : base(msg) { }
        public PersonListOutput(IEnumerable<Database.Person> persons) : base(true) => Persons = persons;
        public IEnumerable<Database.Person> Persons { get; set; }
    }
}
