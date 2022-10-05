using System;
using System.Net;
using System.Net.Sockets;

namespace Useful.Service
{
    public static class EnvironmentService
    {
        private const string VariableName = "ASPNETCORE_ENVIRONMENT";
        public const string DocumentBasePath = BaseFilesPath + "\\Documents";
        public const string BaseFilesPath = "C:/XDataFiles";
        public const string Dev = "Development";
        public const string Staging = "Staging";
        public const string Prod = "Production";
        public const string LibreOfficePath = @"C:\Program Files\LibreOffice\program\soffice.exe";

        public static string Get() =>
            Environment.GetEnvironmentVariable(VariableName);

        // TODO Implementar salvamento de imagens na AWS
        // Esse código é temporário até não salvarmos as imagens na AWS
        public static string BaseURLFilesController
        {
            get
            {
                if (Get() == Dev)
                    return $"http://{GetLocalIPAddress()}:80/v1/General/Files";
                else if (Get() == Staging)
                    return $"http://{GetLocalIPAddress()}:80/v1/General/Files";
                else
                    return "https://xapi-sagu.xplay.digital:443/v1/General/Files";
            }
        }

        private static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip.ToString();

            return "192.168.1.22";
        }
    }
}
