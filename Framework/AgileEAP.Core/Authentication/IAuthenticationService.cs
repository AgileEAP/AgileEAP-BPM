
namespace AgileEAP.Core.Authentication
{
    /// <summary>
    /// Authentication service interface
    /// </summary>
    public partial interface IAuthenticationService : IAuthorize, IFormsAuthentication
    {
        void SignIn(IUser user, bool createPersistentCookie);
        void SignOut();
        IUser GetAuthenticatedUser();
    }
}