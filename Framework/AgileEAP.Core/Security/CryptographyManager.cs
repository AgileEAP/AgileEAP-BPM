#region Description
/*==============================================================================
 *  Copyright (c) trenhui.  All rights reserved.
 * ===============================================================================
 * This code and information is provided "as is" without warranty of any kind,
 * either expressed or implied, including but not limited to the implied warranties
 * of merchantability and fitness for a particular purpose.
 * ===============================================================================
 * This code is only for study
 * ==============================================================================*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using AgileEAP.Core.Extensions;

namespace AgileEAP.Core.Security
{
    public sealed class CryptographyManager
    {
        private readonly static EncryptFormat encryptFormat = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["EncryptFormat"]) ? ConfigurationManager.AppSettings["EncryptFormat"].Cast<EncryptFormat>(EncryptFormat.Hashed) : EncryptFormat.Hashed;

        public static string EncodePassowrd(string plainText)
        {
            return LoadProvider().Encrypt(plainText);
        }

        public static string DecodePassword(string cypherText)
        {
            return LoadProvider().Decrypt(cypherText);
        }

        public static string DesEncode(string plainText)
        {
            ICryptographyProvider cryptographyProvider = new SymmetricAlgorithmProvider(AlgorithmFormat.DES);
            return cryptographyProvider.Encrypt(plainText);
        }

        public static string DesDecode(string cypherText)
        {
            ICryptographyProvider cryptographyProvider = new SymmetricAlgorithmProvider(AlgorithmFormat.DES);
            return cryptographyProvider.Decrypt(cypherText);
        }

        private static ICryptographyProvider LoadProvider()
        {
            ICryptographyProvider result = null;

            switch (encryptFormat)
            {
                case EncryptFormat.Encrypted:
                    result = new SymmetricAlgorithmProvider();
                    break;
                case EncryptFormat.None:
                    result = new NonCryptoProvider();
                    break;
                default:
                    result = new HashAlgorithmProvider();
                    break;
            }

            return result;
        }
    }
}
