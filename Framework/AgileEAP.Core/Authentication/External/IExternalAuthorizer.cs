//Contributor:  Nicholas Mayne


namespace AgileEAP.Core.Authentication.External
{
    public partial interface IExternalAuthorizer
    {
        AuthorizationResult Authorize(OpenAuthenticationParameters parameters);
    }
}