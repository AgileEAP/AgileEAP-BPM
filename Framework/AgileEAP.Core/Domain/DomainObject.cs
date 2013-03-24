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
using System.Text;

namespace AgileEAP.Core.Domain
{
    public class DomainObject<TId> : IDomainWithId<TId>
    {
        [Newtonsoft.Json.JsonProperty]
        public virtual TId ID { get; set; }

        public virtual bool Equals(DomainObject<TId> other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return other != null && this.ID.Equals(other.ID);
        }
    }
}
