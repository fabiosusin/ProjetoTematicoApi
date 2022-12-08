using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using DTO.Intra.FrequencyDB.Database;
using DTO.Intra.Person.Database;

namespace Business.General
{
    public class CryptographyService
    {
        public static readonly string CryptographyKey = "8091326668571953";
        public static readonly string IV = "8091326668571953";

        public static string Decrypt(string key)
        {
            if (string.IsNullOrEmpty(key))
                return null;

            var ret = string.Empty;
            var keybytes = Encoding.UTF8.GetBytes(CryptographyKey);
            var iv = Encoding.UTF8.GetBytes(IV);

            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = keybytes;
                rijAlg.IV = iv;
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
                using var msDecrypt = new MemoryStream(Convert.FromBase64String(key));
                using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                using var srDecrypt = new StreamReader(csDecrypt);
                ret = srDecrypt.ReadToEnd();
            }

            return ret;
        }

        public static string Encrypt(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            var ret = string.Empty;
            var keybytes = Encoding.UTF8.GetBytes(CryptographyKey);
            var iv = Encoding.UTF8.GetBytes(IV);

            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = keybytes;
                rijAlg.IV = iv;

                var encryptor = rijAlg.CreateEncryptor(keybytes, iv);

                using var msEncrypt = new MemoryStream();
                using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
                var toEncrypt = Encoding.ASCII.GetBytes(value);

                csEncrypt.Write(toEncrypt, 0, toEncrypt.Length);
                csEncrypt.FlushFinalBlock();
                ret = Convert.ToBase64String(msEncrypt.ToArray());
            }

            return ret;
        }

        public static Frequency DecryptFrequency(Frequency input) => input == null ? null : new Frequency
        {
            PersonId = input.PersonId,
            Id = input.Id,
            PersonDocument = Decrypt(input.PersonDocument),
            Activity = Decrypt(input.Activity),
            EntryTime = input.EntryTime,
            ExitTime = input.ExitTime,
            ActivityTotalTime = input.ActivityTotalTime,
            FulfilledHours = input.FulfilledHours,
            RemainingHours = input.RemainingHours,
            Appear = input.Appear
        };

        public static Person DecryptPerson(Person input) => input == null ? null : new Person
        {
            Id = input.Id,
            Name = Decrypt(input.Name),
            CpfCnpj = Decrypt(input.CpfCnpj),
            Naturally = Decrypt(input.Naturally),
            MotherName = Decrypt(input.MotherName),
            MaritalStatus = Decrypt(input.MaritalStatus),
            BirthDay = input.BirthDay
        };

        public static Frequency EncryptFrequency(Frequency input) => input == null ? null : new Frequency
        {
            Id = input.Id,
            PersonId = input.PersonId,
            PersonDocument = Encrypt(input.PersonDocument),
            Activity = Encrypt(input.Activity),
            EntryTime = input.EntryTime,
            ExitTime = input.ExitTime,
            ActivityTotalTime = input.ActivityTotalTime,
            FulfilledHours = input.FulfilledHours,
            RemainingHours = input.RemainingHours,
            Appear = input.Appear
        };

        public static Person EncryptPerson(Person input) => input == null ? null : new Person
        {
            Id = input.Id,
            Name = Encrypt(input.Name),
            CpfCnpj = Encrypt(input.CpfCnpj),
            Naturally = Encrypt(input.Naturally),
            MotherName = Encrypt(input.MotherName),
            MaritalStatus = Encrypt(input.MaritalStatus),
            BirthDay = input.BirthDay
        };
    }
}
