using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace AgileEAP.Core.Web
{
    public class FakeHeaderModule : IHttpModule
    {
        /// <summary>
        /// List of Headers to remove
        /// </summary>
        private List<string> removedHeaders = new List<string>
                                      {
                                              "Server",
                                              "X-AspNet-Version",
                                              "X-AspNetMvc-Version",
                                              "X-Powered-By"
                                      };

        /// <summary>
        /// Initializes a new instance of the <see cref="CloakHttpHeaderModule"/> class.
        /// </summary>
        public FakeHeaderModule()
        {
        }

        /// <summary>
        /// Dispose the Custom HttpModule.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Handles the current request.
        /// </summary>
        /// <param name="context">
        /// The HttpApplication context.
        /// </param>
        public void Init(HttpApplication context)
        {
            context.PreSendRequestHeaders += this.OnPreSendRequestHeaders;
        }

        /// <summary>
        /// Remove all headers from the HTTP Response.
        /// </summary>
        /// <param name="sender">
        /// The object raising the event
        /// </param>
        /// <param name="e">
        /// The event data.
        /// </param>
        private void OnPreSendRequestHeaders(object sender, EventArgs e)
        {
            if (HttpContext.Current != null)
                this.removedHeaders.ForEach(h => HttpContext.Current.Response.Headers.Remove(h));
        }
    }
}
