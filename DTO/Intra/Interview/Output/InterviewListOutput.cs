using DTO.General.Base.Api.Output;
using System.Collections.Generic;

namespace DTO.Intra.Interview.Output
{
    public class InterviewListOutput : BaseApiOutput
    {
        public InterviewListOutput(string msg) : base(msg) { }
        public InterviewListOutput(IEnumerable<Database.Interview> interviews) : base(true) => Interviews = interviews;
        public IEnumerable<Database.Interview> Interviews { get; set; }
    }
}
