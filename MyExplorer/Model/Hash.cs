using System.Security.Cryptography;
using System.Text;

namespace MyExplorer.Model
{
    public static class Hash
    {
        static SHA256 sha = SHA256.Create();

        public static string CreateHash(string data)
        {
            var byteHash = sha.ComputeHash(Encoding.UTF8.GetBytes(data));
            return ByteArrayToString(byteHash);
        }

        static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }
}
