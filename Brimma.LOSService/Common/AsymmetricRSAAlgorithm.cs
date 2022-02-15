using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Brimma.LOSService.Common
{
    public class AsymmetricRSAAlgorithm
    {
        private static string privateKey;
        private static string publicKey;
        public readonly static IConfiguration configuration;
        static AsymmetricRSAAlgorithm()
        {
            configuration = Startup.StaticConfig;
            publicKey = configuration["publickey"];
            privateKey = configuration["privatekey"];
        }
        public static string Decrypt(dynamic Encrytext)
        {
            string strEncrytext = Encrytext;
            if (!string.IsNullOrEmpty(strEncrytext))
            {
                var cipher = new RSACryptoServiceProvider();
                cipher.FromXmlString(privateKey);
                byte[] ciphterText = Convert.FromBase64String(strEncrytext);
                byte[] plainText = cipher.Decrypt(ciphterText, false);
                return Encoding.UTF8.GetString(plainText);
            }
            return null;
        }
        public static string Encrypt(dynamic UnEncryText)
        {
            string strUnEncryText = UnEncryText;
            if (!string.IsNullOrEmpty(strUnEncryText))
            {
                var cipher = new RSACryptoServiceProvider();
                cipher.FromXmlString(publicKey);
                byte[] data = Encoding.UTF8.GetBytes(strUnEncryText);
                byte[] cipherText = cipher.Encrypt(data, false);
                return Convert.ToBase64String(cipherText);
            }
            return null;
        }
    }
}
