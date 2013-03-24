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

namespace AgileEAP.Core.Security
{
    public sealed class NonCryptoProvider : ICryptographyProvider
    {
        public string Encrypt(string plainText)
        { return plainText; }

        public string Decrypt(string cypherText)
        { return cypherText; }
    }
}
