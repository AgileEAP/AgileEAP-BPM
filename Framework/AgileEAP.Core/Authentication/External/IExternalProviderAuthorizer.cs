//Contributor:  Nicholas Mayne


namespace AgileEAP.Core.Authentication.External
{
    public partial interface IExternalProviderAuthorizer
    {
        AuthorizeState Authorize(string returnUrl);
    }
}