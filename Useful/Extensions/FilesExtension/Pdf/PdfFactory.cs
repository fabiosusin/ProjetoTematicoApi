using DTO.General.MimeType.Output;
using System;
using System.IO;
using System.Text.RegularExpressions;
using Useful.Service;

namespace Useful.Extensions.FilesExtension.Pdf
{
    public class PdfFactory
    {
        private const string PdfPath = "/Pdf";

        public static string SavePdfFromBase64(string base64) => GetImageConverted(base64);

        private static string GetImageConverted(string base64)
        {
            var pdfByteArray = ConvertBase64ToByteArray(base64);
            if (pdfByteArray == null)
                return null;

            var result = SavePdfOnServer(new PdfInput { PdfId = Guid.NewGuid().ToString(), Pdf = pdfByteArray });

            return result;
        }

        private static byte[] ConvertBase64ToByteArray(string base64) => string.IsNullOrEmpty(base64) ? null : Convert.FromBase64String(new Regex("data:application/pdf;base64,").Replace(base64, ""));

        private static string SavePdfOnServer(PdfInput input)
        {
            if (string.IsNullOrEmpty(input?.PdfId) || input.Pdf == null)
                return null;

            var path = EnvironmentService.BaseFilesPath + PdfPath;

            var folder = PdfPath;
            var extension = ".pdf";

            var result = path + "/" + input.PdfId + extension;

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (!Directory.Exists(path + folder))
                Directory.CreateDirectory(path + folder);

            try
            {
                File.WriteAllBytes(Path.Combine(path, input.PdfId + extension), input.Pdf);
            }
            catch
            {
                return null;
            }

            return $"{EnvironmentService.BaseURLFilesController}/Pdf/get-pdf?filePath={result}&contentType={MimeTypeOutput.Pdf}";
        }

        private class PdfInput
        {
            public byte[] Pdf { get; set; }
            public string PdfId { get; set; }
        }
    }
}
