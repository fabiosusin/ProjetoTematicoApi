using DTO.General.Base.Api.Output;
using System.Collections.Generic;

namespace DTO.Intra.Situation.Output
{
    public class SituationListOutput : BaseApiOutput
    {
        public SituationListOutput(string msg) : base(msg) { }
        public SituationListOutput(IEnumerable<Database.Situation> situations) : base(true) => Situations = situations;
        public IEnumerable<Database.Situation> Situations { get; set; }
    }
}
