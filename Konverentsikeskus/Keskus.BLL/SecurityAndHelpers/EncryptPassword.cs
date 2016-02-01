using System;
using System.Security.Cryptography;
using System.Text;

namespace Keskus.BLL.Security
{
    /// <remarks>
    /// class handles password encryption for app users
    /// </remarks>
    public static class EncryptPassword
    {

       public static string computeHash(string password, HashAlgorithm algorithm)
        {
            Byte[] inputBytes = Encoding.UTF8.GetBytes(password);
            Byte[] hashedBytes = algorithm.ComputeHash(inputBytes);

            return BitConverter.ToString(hashedBytes);
        }

    }
}
