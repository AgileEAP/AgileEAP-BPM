using System.ComponentModel;

namespace AgileEAP.Plugin.OpenApi.HostSample.Models {
    public class LogOnModel {
		[DisplayName("OpenID")]
		public string UserSuppliedIdentifier { get; set; }

		[DisplayName("Remember me?")]
		public bool RememberMe { get; set; }
	}
}
