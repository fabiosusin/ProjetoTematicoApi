using System;
using System.Net;
using System.Net.Sockets;

namespace Services.Service
{
    public static class EnvironmentService
    {
        private const string VariableName = "ASPNETCORE_ENVIRONMENT";
        public const string DocumentBasePath = BaseFilesPath + "\\Documents";
        public const string EmailTemplateBasePath = BaseFilesPath + "\\MailTemplates";
        public const string BaseFilesPath = "C:/XDataFiles";
        public const string Dev = "Development";
        public const string Staging = "Staging";
        public const string Prod = "Production";
        public const string LibreOfficePath = @"C:\Program Files\LibreOffice\program\soffice.exe";

        public static string Get() =>
            Environment.GetEnvironmentVariable(VariableName);
    }
}
