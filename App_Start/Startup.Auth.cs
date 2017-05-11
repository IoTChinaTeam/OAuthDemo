using System.IdentityModel.Tokens;
using Microsoft.Owin.Security.Jwt;
using Owin;

namespace OAuthDemo
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidAudiences = new string[]
                        {
                            "957cfb32-7907-4fb2-90a0-fafcf3afc56c",
                            "349887504252-4q03i53j0bvcu3uorokc39e1fcd2q1r7.apps.googleusercontent.com"
                        },

                        ValidIssuers = new string[]
                        {
                            "https://sts.windows.net/9e88becc-48d3-42b4-9766-543fd455ad51/",
                            "https://accounts.google.com"
                        },

                        IssuerSigningKeyResolver = IssuerSigningKeyResolver.Resolve
                    }
                });
        }
    }
}