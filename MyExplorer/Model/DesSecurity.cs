using MyExplorer.Properties;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MyExplorer.Model
{
    internal static class DesSecurity
    {
        static Services.FileLogger fl = Services.FileLogger.GetInstance(ViewModel.Settings.GetInstance().LogFile);

        static byte[] GetP()
        {
            byte[] bytes = Encoding.UTF8.GetBytes(Resources.pickles);
            for (int i = 0; i < bytes.Length; i++) bytes[i] ^= bytes[bytes.Length - 1 - i];
            Array.Resize(ref bytes, 8);
            return bytes;
        }
        public static string Encrypt(string originalString)
        {
            if (String.IsNullOrEmpty(originalString))
                fl.Write("Строка пользователя пуста", Enums.LogType.Error);
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                cryptoProvider.CreateEncryptor(GetP(), GetP()), CryptoStreamMode.Write);
            StreamWriter writer = new StreamWriter(cryptoStream);
            writer.Write(originalString);
            writer.Flush();
            cryptoStream.FlushFinalBlock();
            writer.Flush();
            return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
        }

        public static string Decrypt(string cryptedString)
        {
            if (String.IsNullOrEmpty(cryptedString))
                fl.Write("Закрытая строка пользователя пуста", Enums.LogType.Error);
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(cryptedString));
            CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateDecryptor(GetP(), GetP()), CryptoStreamMode.Read);
            StreamReader reader = new StreamReader(cryptoStream);
            return reader.ReadToEnd();
        }
    }
}
