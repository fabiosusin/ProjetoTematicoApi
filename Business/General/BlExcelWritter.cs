using DTO.General.Api.Output;
using DTO.General.Excel.Input;
using Services.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using ClosedXML.Excel;

namespace Business.General
{
    public class BlExcelWritter 
    {
        public static GenerateDocOutput GetExcel(string fileName, ExcelValues excelValues)
        {
            if (!(excelValues?.Collumns?.Any() ?? false) || !(excelValues.Values?.Any() ?? false))
                return null;

            var file = $"{EnvironmentService.DocumentBasePath}\\{Guid.NewGuid()}.xlsx";
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Relatório");
                var mainLineControl = 1;
                var totalLineControl = 0;

                #region HEADER TABELA
                for (var i = 0; i < excelValues.Collumns.Count; i++)
                {
                    var col = excelValues.Collumns[i];
                    worksheet.Cell($"{Letters(i)}{mainLineControl}").Value = col;
                }

                mainLineControl++;
                #endregion

                #region DADOS DA TABELA
                for (var i = 0; i < excelValues.Values.Count; i++)
                {
                    var line = excelValues.Values[i];
                    for (int j = 0; j < line.Count; j++)
                    {
                        var val = line[j];
                        worksheet.Cell($"{Letters(j)}{mainLineControl}").Value = val;
                    }

                    mainLineControl++;
                }

                //bordas da tabela e alinhamento
                IXLRange range = worksheet.Range(worksheet.Cell(1, 1).Address, worksheet.Cell(mainLineControl - 1, excelValues.Collumns.Count).Address);
                range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                range.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                #endregion

                workbook.SaveAs(file);
            }

            return new(fileName, file);
        }

        private static string Letters(int idx)
        {
            string[] letters = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            return letters[idx];
        }
    }
}
