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
using System.Collections.Specialized;
using System.Configuration;

namespace AgileEAP.Core.Utility
{
    public class AppSettingsUtil
    {
        private readonly NameValueCollection appSettings;

        public AppSettingsUtil(NameValueCollection appSettings)
        {
            this.appSettings = appSettings;
        }

        public string GetString(string name)
        {
            return getValue(name, true, null);
        }

        public string GetString(string name, string defaultValue)
        {
            return getValue(name, false, defaultValue);
        }

        public string[] GetStringArray(string name, string separator)
        {
            return getStringArray(name, separator, true, null);
        }

        public string[] GetStringArray(string name, string separator, string[] defaultValue)
        {
            return getStringArray(name, separator, false, defaultValue);
        }

        private string[] getStringArray(string name, string separator, bool requireValue, string[] defaultValue)
        {
            string value = getValue(name, requireValue, null);

            if (value != null)
                return value.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);

            return defaultValue;
        }

        //INFO: (erikpo) As other types of fields are needed, add strongly typed methods like "GetBoolean"

        private string getValue(string name, bool requireValue, string defaultValue)
        {
            string value = appSettings[name];

            if (value != null)
                return value;

            if (requireValue)
                throw new InvalidOperationException(string.Format("Could not find required app setting '{0}'", name));

            return defaultValue;
        }
    }
}