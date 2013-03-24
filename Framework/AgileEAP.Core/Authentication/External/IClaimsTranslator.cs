//Contributor:  Nicholas Mayne


namespace AgileEAP.Core.Authentication.External
{
    public partial interface IClaimsTranslator<T>
    {
        UserClaims Translate(T response);
    }
}