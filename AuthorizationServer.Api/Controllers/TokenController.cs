using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using AuthorizationServer.Api.Formats;
using AuthorizationServer.Api.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;

namespace AuthorizationServer.Api.Controllers
{
    [RoutePrefix("api/Token")]
    public class TokenController : ApiController
    {
        [Route("Token")]
        public IHttpActionResult Token()
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                //For Dev enviroment only (on production should be AllowInsecureHttp = false)
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/oauth2/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                Provider = new CustomOAuthProvider(),
                AccessTokenFormat = new CustomJwtFormat("http://jwtauthzsrv.azurewebsites.net")
            };
            return null;
        }
    }
}
