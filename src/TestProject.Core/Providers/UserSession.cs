using Xamarin.Essentials;
using TestProject.Core.Common;
using BuildApps.Core.Mobile.Configurations;

namespace TestProject.Core.Providers
{
    public class UserSession : IUserSession
    {
        private const string TokenKey = nameof(TokenKey);
        private Token? token;

        public UserSession()
        {
        }

        public Token GetToken()
        {
            if (token != null)
            {
                return token;
            }

            var json = Preferences.Get(TokenKey, null);
            if (json is null)
            {
                return null;
            }

            var newToken = Json.Deserialize<Token>(json);
            token = newToken;

            return newToken;
        }

        public void SetToken(Token token)
        {
            this.token = token;

            if (token is null)
            {
                Preferences.Set(TokenKey, null);
            }
            else
            {
                var json = Json.Serialize(token);
                Preferences.Set(TokenKey, json);
            }
        }
    }
}
