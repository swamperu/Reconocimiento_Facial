using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Security.Cryptography;

namespace SCONT.Presentacion.Web
{
    public partial class Startup
    {       
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            app.Use((context, next) =>
            {
                var rng = new RNGCryptoServiceProvider();
                var nonceBytes = new byte[32];
                rng.GetBytes(nonceBytes);
                var nonce = System.Convert.ToBase64String(nonceBytes);
                context.Set("ScriptNonce", nonce);
                context.Response.Headers.Add("Content-Security-Policy",
                    new[] { string.Format("script-src 'strict-dynamic'  'nonce-{0}' 'unsafe-inline' 'self' http: https:; style-src 'self' 'unsafe-inline';  object-src 'none'; base-uri 'none';", nonce) });
                return next();
            });

            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Presentacion")
            });
            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            
        }


    }
}