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
    public class SymmetricAlgorithmProvider : ICryptographyProvider
    {
        #region Private Fields
        private AlgorithmFormat format = AlgorithmFormat.DES;
        private SymmetricAlgorithm provider = new DESCryptoServiceProvider();
        static readonly string symmetricAlgo = ConfigurationManager.AppSettings["AgileEAP.AlgorithmType"];
        private ILogger logger = LogManager.GetLogger(typeof(HashAlgorithmProvider));
        //TODO: Consider about how to get Keys and IVs
        private byte[] bKeys = Encoding.ASCII.GetBytes("EAFrame&");
        private byte[] bIVs = Encoding.ASCII.GetBytes("EAFrame&");

        #endregion

        #region Constructor

        public SymmetricAlgorithmProvider(AlgorithmFormat format)
        {
            this.format = format;
            initProvider(format);
        }

        public SymmetricAlgorithmProvider()
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

        public SymmetricAlgorithmProvider(byte[] keys, byte[] ivs)
        {
            bKeys = keys;
            bIVs = ivs;
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

        public byte[] Keys
        {
            get { return bKeys; }
            set { bKeys = value; }
        }
        public byte[] IVs
        {
            get { return bIVs; }
            set { bIVs = value; }
        }

        #endregion

        #region Public Methods

        public string Encrypt(string source)
        {
            return Encrypt(source, Keys, IVs);
        }

        public string Decrypt(string source)
        {
            return Decrypt(source, Keys, IVs);
        }

        public string Encrypt(string source, byte[] keys, byte[] ivs)
        {
            if (provider == null) return source;

            byte[] bEncrypted = cryptoTransform(provider.CreateEncryptor(keys, ivs),
                Encoding.UTF8.GetBytes(source));

            return Convert.ToBase64String(bEncrypted);
        }

        public string Decrypt(string source, byte[] keys, byte[] ivs)
        {
            if (provider == null) return source;

            byte[] bDecrypted = cryptoTransform(provider.CreateDecryptor(keys, ivs),
                Convert.FromBase64String(source));

            return Encoding.UTF8.GetString(bDecrypted);
        }

        #endregion

        #region Private Methods

        private void initProvider(AlgorithmFormat format)
        {
            switch (format)
            {
                case AlgorithmFormat.DES:
                    provider = new DESCryptoServiceProvider();
                    break;
                case AlgorithmFormat.RC2:
                    provider = new RC2CryptoServiceProvider();
                    break;
                default:
                    provider = null;
                    break;
            }
        }

        private byte[] cryptoTransform(ICryptoTransform cryptoTransform, byte[] bSrcs)
        {
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write);

            try
            {
                cs.Write(bSrcs, 0, bSrcs.Length);
                cs.FlushFinalBlock();

                ms.Position = 0;
            }
            catch (CryptographicException ex)
            {
                logger.Error("加密出错！", ex);
            }
            finally
            {
                ms.Close();
                cs.Close();
            }

            return ms.ToArray();
        }

        #endregion
    }
}
