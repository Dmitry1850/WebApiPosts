using System.Security.Cryptography;
using System.Text;

namespace MainProgram.Common
{
    public static class Hash
    {
        public static async Task<string> GetHash(string str)
        {
            var hash = MD5.HashData(Encoding.ASCII.GetBytes(str));
            var output = new StringBuilder(hash.Length);
            foreach (var b in hash)
            {
                output.Append(b.ToString("X2"));
            }

            return await Task.Run(() => (output.ToString()));
        }
    }
}