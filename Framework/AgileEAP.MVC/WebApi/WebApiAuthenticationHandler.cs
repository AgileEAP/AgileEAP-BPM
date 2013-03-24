using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.Web.Security;

namespace AgileEAP.MVC.WebApi
{
    public class WebApiAuthenticationHandler : DelegatingHandler
    {
        private readonly ITokenAuthentication authentication;

        public WebApiAuthenticationHandler(ITokenAuthentication tokenAuthentication)
        {
            this.authentication = tokenAuthentication;
        }


        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            IEnumerable<string> cookie = request.Headers.GetValues("Cookie");
            if (cookie != null && cookie.Count() > 0)
            {
                foreach (var cookieValue in cookie)
                {
                    foreach (var cookieItem in cookieValue.Split(';'))
                    {
                        var values = cookieItem.Split('=');
                        if (values != null && values[0] == FormsAuthentication.FormsCookieName)
                        {
                            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(values[1]);
                            if (ticket.Expiration > DateTime.Now)
                            {
                                return base.SendAsync(request, cancellationToken);
                            }
                        }
                    }
                }
            }

            if (request.Headers.Authorization != null && request.Headers.Authorization.Scheme == "Bearer")
            {
                if (authentication.AuthorizeToken(request.Headers.Authorization.Parameter))
                {
                    return base.SendAsync(request, cancellationToken);
                }
            }

            return FobiddenResponse();
        }

        public Task<HttpResponseMessage> FobiddenResponse()
        {
            return Task<HttpResponseMessage>.Factory.StartNew(
                () =>
                new HttpResponseMessage(HttpStatusCode.Forbidden));
        }


        //protected override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(
        //HttpRequestMessage request,
        //CancellationToken cancellationToken)
        //{
        //    AuthenticationHeaderValue authValue = request.Headers.Authorization;
        //    if (authValue != null && !String.IsNullOrWhiteSpace(authValue.Parameter))
        //    {
        //        Credentials parsedCredentials = ParseAuthorizationHeader(authValue.Parameter);
        //        if (parsedCredentials != null)
        //        {
        //            Thread.CurrentPrincipal = PrincipalProvider
        //                .CreatePrincipal(parsedCredentials.Username, parsedCredentials.Password);
        //        }
        //    }
        //    return base.SendAsync(request, cancellationToken)
        //       .ContinueWith(task =>
        //       {
        //           var response = task.Result;
        //           if (response.StatusCode == HttpStatusCode.Unauthorized
        //               && !response.Headers.Contains(BasicAuthResponseHeader))
        //           {
        //               response.Headers.Add(BasicAuthResponseHeader
        //                   , BasicAuthResponseHeaderValue);
        //           }
        //           return response;
        //       });
        //}

        //private Credentials ParseAuthorizationHeader(string authHeader)
        //{
        //    string[] credentials = Encoding.ASCII.GetString(Convert
        //                                                    .FromBase64String(authHeader))
        //                                                    .Split(
        //                                                    new[] { ':' });
        //    if (credentials.Length != 2 || string.IsNullOrEmpty(credentials[0])
        //        || string.IsNullOrEmpty(credentials[1])) return null;
        //    return new Credentials()
        //    {
        //        Username = credentials[0],
        //        Password = credentials[1],
        //    };
        //}
    }
}
