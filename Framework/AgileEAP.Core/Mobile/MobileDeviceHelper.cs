using System.Web;
using AgileEAP.Core;
using AgileEAP.Core.Domain;

namespace AgileEAP.Core.Mobile
{
    /// <summary>
    /// Mobile device helper
    /// </summary>
    public partial class MobileDeviceHelper : IMobileDeviceHelper
    {
        #region Fields

        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="workContext">Work context</param>
        public MobileDeviceHelper(IWorkContext workContext)
        {
            this._workContext = workContext;
        }

        #endregion

        #region Methods


        /// <summary>
        /// Returns a value indicating whether request is made by a mobile device
        /// </summary>
        /// <param name="httpContext">HTTP context</param>
        /// <returns>Result</returns>
        public virtual bool IsMobileDevice(HttpContextBase httpContext)
        {
            return httpContext.Request.Browser.IsMobileDevice;
        }

        /// <summary>
        /// Returns a value indicating whether mobile devices support is enabled
        /// </summary>
        public virtual bool MobileDevicesSupported()
        {
            return false;// _storeInformationSettings.MobileDevicesSupported;
        }

        /// <summary>
        /// Returns a value indicating whether current customer prefer to use full desktop version (even request is made by a mobile device)
        /// </summary>
        public virtual bool DontUseMobileVersion()
        {
            return true;// _workContext.CurrentUser.GetAttribute<bool>(SystemCustomerAttributeNames.DontUseMobileVersion);
        }

        #endregion
    }
}