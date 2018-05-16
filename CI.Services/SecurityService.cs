using System.Security.Cryptography;
using System.Text;

namespace Savchin.Services
{
    public interface ISecurityService
    {
        /// <summary>
        /// Computes the hash MD5.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        byte[] ComputeHashMd5(string input);
    }

    public class SecurityService : ISecurityService
    {
        public byte[] ComputeHashMd5(string input)
        {
            using (var md5 = MD5.Create())
            {
               return md5.ComputeHash(Encoding.Default.GetBytes(input));
            }
        }
    }
}
