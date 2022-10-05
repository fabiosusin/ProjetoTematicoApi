using DTO.General.Base.Database;
using DTO.General.Log.Enum;
using System;

namespace DTO.General.Log.Database
{
    public class AppLogHistory : BaseData
    {
        public AppLogTypeEnum Type { get; set; }
        public string Message { get; set; }
        public string ExceptionMessage { get; set; }
        public string StackTrace { get; set; }
        public string Controller { get; set; }
        public string Route { get; set; }
        public string Method { get; set; }
        public string Username { get; set; }
        public string Data { get; set; }
        public DateTime Date { get; set; }
    }
}
