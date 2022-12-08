using DTO.General.Base.Database;
using System;

namespace DTO.Intra.FrequencyDB.Database
{
    public class Frequency : BaseData
    {
        public Guid PersonId { get; set; }
        public string PersonDocument { get; set; }
        public string Activity { get; set; }
        public DateTime EntryTime { get; set; }
        public DateTime ExitTime { get; set; }
        public int ActivityTotalTime { get; set; }
        public int FulfilledHours { get; set; }
        public int RemainingHours { get; set; }
        public bool Appear { get; set; }
    }
}
