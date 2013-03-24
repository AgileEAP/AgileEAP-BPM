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
using System.IO;
using System.Configuration;

using System.Security.Cryptography;

namespace AgileEAP.Core.Security
{
    public sealed class HashAlgorithmProvider : ICryptographyProvider
    {
        #region Private Fields
        private AlgorithmFormat format = AlgorithmFormat.MD5;
        private HashAlgorithm provider = new MD5CryptoServiceProvider();
        private int hashLength = 32;
        static readonly string symmetricAlgo = ConfigurationManager.AppSettings["AgileEAP.AlgorithmType"];

        private ILogger logger = LogManager.GetLogger(typeof(HashAlgorithmProvider));
        #endregion

        #region Constructor

        public HashAlgorithmProvider(AlgorithmFormat format)
        {
            this.format = format;
            initProvider(format);
        }

        public HashAlgorithmProvider()
        {
            try
            {
                if (!string.IsNullOrEmpty(symmetricAlgo))
                    format = (AlgorithmFormat)Enum.Parse(typeof(AlgorithmFormat), symmetricAlgo);

                initProvider(format);
            }
            catch (Exception ex)
            {
                logger.Error("加密格式配置不正确", ex);
            }
        }

        #endregion

        #region Public Properties

        public AlgorithmFormat Format
        {
            get { return format; }
            set
            {
                format = value;
                initProvider(format);
            }
        }

        public int HashLength
        {
            get { return hashLength; }
            set { hashLength = value; }
        }

        #endregion

        #region Public Methods

        public string Encrypt(string source)
        {
            return Encrypt(source, hashLength);
        }

        public string Decrypt(string source)
        {
            return Encrypt(source);
        }

        #endregion

        #region Private Methods
        private void initProvider(AlgorithmFormat format)
        {
            provider = CryptoConfig.CreateFromName(format.ToString().ToUpper()) as HashAlgorithm;
        }

        /// <summary>
        /// MD5加密,和动网上的16/32位MD5加密结果相同
        /// </summary>
        /// <param name="strSource">待加密字串</param>
        /// <param name="length">16或32值之一,其它则采用.net默认MD5加密算法</param>
        /// <returns>加密后的字串</returns>
        public string Encrypt(string source, int length)
        {
            if (provider == null)
            {
                logger.Error("加密算法配置错误！");
                return source;
            }

            if (string.IsNullOrEmpty(source)) return string.Empty;

            byte[] bytes = Encoding.ASCII.GetBytes(source);
            byte[] hashValue = provider.ComputeHash(bytes);
            StringBuilder sb = new StringBuilder();
            switch (length)
            {
                case 16:
                    for (int i = 4; i < 12; i++)
                        sb.Append(hashValue[i].ToString("x2"));
                    break;
                case 32:
                    for (int i = 0; i < 16; i++)
                    {
                        sb.Append(hashValue[i].ToString("x2"));
                    }
                    break;
                default:
                    for (int i = 0; i < hashValue.Length; i++)
                    {
                        sb.Append(hashValue[i].ToString("x2"));
                    }
                    break;
            }
            return sb.ToString();
        }

        #endregion
    }
}
