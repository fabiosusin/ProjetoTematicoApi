using DTO.General.Base.Database;

namespace DTO.Intra.Situation.Database
{
    public class Situation : BaseData
    {
        public int ProcessNumber { get; set; }
        public string VaraOrigem { get; set; }
        public int ConvictionQuantity { get; set; }
        public int ConvictionType { get; set; }
        public int PrdType { get; set; }
        public int FinePrice { get; set; }
        public int PrdToDo { get; set; }
        public int CrimeType { get; set; }
        public bool NonCriminalProsecutionAgreement { get; set; }
        public int OriginalPenalty { get; set; }
        public int TotalDays { get; set; }
        public int FulfilledHours { get; set; }
        public int RemainingHours { get; set; }
    }
}
