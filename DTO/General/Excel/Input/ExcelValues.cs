using System.Collections.Generic;

namespace DTO.General.Excel.Input
{
    public class ExcelValues
    {
        public ExcelValues()
        {
            Collumns = new List<string>();
            Values = new List<List<string>>();
        }
        public List<string> Collumns { get; set; }
        public List<List<string>> Values { get; set; }
    }
}
