using DTO.General.Base.Api.Output;
using System.Collections.Generic;

namespace DTO.Intra.Frequency.Output
{
    public class FrequencyListOutput : BaseApiOutput
    {
        public FrequencyListOutput(string msg) : base(msg) { }
        public FrequencyListOutput(IEnumerable<FrequencyDB.Database.Frequency> frequencies) : base(true) => Frequencies = frequencies;
        public IEnumerable<FrequencyDB.Database.Frequency> Frequencies { get; set; }
    }
}
