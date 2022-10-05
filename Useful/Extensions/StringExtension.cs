using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Useful.Extensions
{
    public static class StringExtension
    {
        public static bool ZipCodeIsValid(string self) => self.GetDigits()?.Length == 8;

        public static string GetDigits(this string self) =>
            self == null ? null : new Regex(@"[^\d]").Replace(self, "");

        public static bool IsValidEmail(this string email)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                static string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public static string RandomString(int size, bool lowerCase)
        {
            var builder = new StringBuilder();
            var random = new Random();
            for (int i = 0; i < size; i++)
                builder.Append(Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65))));

            if (lowerCase)
                return builder.ToString().ToLower();

            return builder.ToString();
        }

        public static bool IsCnpjOrCpf(this string data) => data?.Length == 11 ? IsCpf(data) : IsCnpj(data);

        public static bool IsCnpj(this string cnpj)
        {
            if (string.IsNullOrEmpty(cnpj))
                return false;

            var multiplier1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplier2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;

            var tempCnpj = cnpj[..12];
            var sum = 0;
            for (int i = 0; i < 12; i++)
                sum += int.Parse(tempCnpj[i].ToString()) * multiplier1[i];

            var remainder = (sum % 11);
            remainder = remainder < 2 ? 0 : 11 - remainder;

            var digit = remainder.ToString();
            tempCnpj += digit;
            sum = 0;
            for (int i = 0; i < 13; i++)
                sum += int.Parse(tempCnpj[i].ToString()) * multiplier2[i];

            remainder = (sum % 11);
            remainder = remainder < 2 ? 0 : 11 - remainder;
            digit += remainder.ToString();
            return cnpj.EndsWith(digit);
        }

        public static bool IsCpf(this string cpf)
        {
            if (string.IsNullOrEmpty(cpf))
                return false;

            var multiplier1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplier2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;

            var tempCpf = cpf[..9];
            var sum = 0;
            for (int i = 0; i < 9; i++)
                sum += int.Parse(tempCpf[i].ToString()) * multiplier1[i];

            var remainder = (sum % 11);
            remainder = remainder < 2 ? 0 : 11 - remainder;

            var digit = remainder.ToString();
            tempCpf += digit;
            sum = 0;
            for (int i = 0; i < 10; i++)
                sum += int.Parse(tempCpf[i].ToString()) * multiplier2[i];

            remainder = (sum % 11);
            remainder = remainder < 2 ? 0 : 11 - remainder;
            digit += remainder.ToString();
            return cpf.EndsWith(digit);
        }

        public static string FirstCharacterToLower(this string str) =>
            string.IsNullOrEmpty(str) || char.IsLower(str, 0) ? str : char.ToLowerInvariant(str[0]) + str[1..];

        public static string GetMsisdn(this string data)
        {
            if (string.IsNullOrEmpty(data))
                return null;

            var dataFormatted = data.GetDigits();
            return (dataFormatted.Length == 11 ? "55" : "") + dataFormatted;
        }

        public async static Task<string> GetImageAsBase64Url(string url)
        {
            if (string.IsNullOrEmpty(url))
                return null;

            using var handler = new HttpClientHandler { };
            using var client = new HttpClient(handler);
            var bytes = await client.GetByteArrayAsync(url);
            return "data:image/jpeg;base64," + Convert.ToBase64String(bytes);
        }

        public static bool CellphoneIsValid(this string number) => number?.Length switch
        {
            11 => Regex.IsMatch(number, @"^\(?\d{2}\)?[\s-]?[\s9]?\d{4}-?\d{4}$"),
            13 => Regex.IsMatch(number, @"^\d{2}?\(?\d{2}\)?[\s-]?[\s9]?\d{4}-?\d{4}$"),
            _ => false,
        };

        public static string FormatCpfCnpj(this string cpfCnpj) => string.IsNullOrEmpty(cpfCnpj) ? "" : cpfCnpj.Length == 11 ? FormatCPF(cpfCnpj) : FormatCNPJ(cpfCnpj);

        /// <summary>
        /// Formatar uma string CNPJ
        /// </summary>
        /// <param name="CNPJ">string CNPJ sem formatacao</param>
        /// <returns>string CNPJ formatada</returns>
        /// <example>Recebe '99999999999999' Devolve '99.999.999/9999-99'</example>

        private static string FormatCNPJ(string CNPJ)
        {
            return Convert.ToUInt64(CNPJ).ToString(@"00\.000\.000\/0000\-00");
        }

        /// <summary>
        /// Formatar uma string CPF
        /// </summary>
        /// <param name="CPF">string CPF sem formatacao</param>
        /// <returns>string CPF formatada</returns>
        /// <example>Recebe '99999999999' Devolve '999.999.999-99'</example>

        private static string FormatCPF(string CPF)
        {
            return Convert.ToUInt64(CPF).ToString(@"000\.000\.000\-00");
        }

        public static string ToMoney(this decimal number) => string.Format("{0:c" + 2 + "}", number);

        public static string CompleteWithZerosLeft(string value, int length)
        {
            if (string.IsNullOrEmpty(value))
                value = "";

            if (value.Length > length)
                return value[..length];

            while (value.Length < length)
                value = "0" + value;

            return value;
        }

        public static string RemoveSpecialCharacter(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            var symbolTable = new Dictionary<char, char[]>
            {
                { 'e', new char[] { '&' } },
                { '-', new char[] { '\\', '/', '|' } }
            };

            foreach (var key in symbolTable.Keys)
                foreach (var symbol in symbolTable[key])
                    text = text.Replace(symbol, key);

            var validchars = "1234567890.,;:?!(){}[]%@_-+*=$#";
            validchars += "ABCDEFGHIJKLMNOPQRSTUVWXYZÇ abcdefghijklmnopqrstuvwxyzç";
            validchars += "ÁÀÃÂáàãâÉÈÊéèêÍÌíìÓÒÕÔóòõôÚÙÜúùü";
            validchars += "º°ª";

            return new string(text.Where(c => validchars.Contains(c)).ToArray());
        }

        public static string FormatWhitouthCharacters(this string value) => value == null ?
            string.Empty : value.Replace(".", string.Empty).Replace(",", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty).Trim();

        public static string FormatNumber(this decimal value) => string.Format("{0:0.00}", value).Replace(",", ".").Trim();

        public static string FormatAliquotaNumber(this decimal value) => string.Format("{0:0.000}", value).Replace(",", ".").Trim();

        public static string ObjectToXML(object input)
        {
            using var stringwriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(input.GetType());
            serializer.Serialize(stringwriter, input);
            return stringwriter.ToString();
        }
      
        public static string GetPortugueseWrittenDate(DateTime date) => date.ToString(new CultureInfo("pt-PT", false).DateTimeFormat.LongDatePattern, new CultureInfo("pt-PT"));
    }
}
