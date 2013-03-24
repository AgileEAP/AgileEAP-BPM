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
    public enum EncryptFormat
    {
        None = 0,
        Encrypted = 1,
        Hashed = 2
    }

    public enum AlgorithmFormat
    {
        None = 0,
        RC2 = 1,
        DES = 2,
        SHA1 = 3,
        MD5 = 4,
        Hash3DES = 5
    }
}
