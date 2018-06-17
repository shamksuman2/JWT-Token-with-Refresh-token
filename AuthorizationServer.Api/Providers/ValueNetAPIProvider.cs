using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthorizationServer.Api.Entities;

namespace AuthorizationServer.Api.Providers
{
    public class ValueNetApiProvider : OAuthAuthorizationServerProvider
    {
        //IAuthorizeUserLogic _authorizeUser;
        //APIClientsBL clientsBL = new APIClientsBL();

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            if (!context.TryGetBasicCredentials(out string clientId, out string clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if (clientId == null)
            {
                context.SetError("invalid_clientId/Secret", "client_Id/Secret is not set");
                return Task.FromResult<object>(null);
            }

            var client = new UserInfos();// clientsBL.GetClient(clientId);

            if (client == null)
            {
                context.SetError("invalid_clientId", $"Invalid client_id '{context.ClientId}'");
                return Task.FromResult<object>(null);
            }

            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            if (!String.IsNullOrEmpty(context.UserName) && !String.IsNullOrEmpty(context.Password))
            {
                //if (_authorizeUser == null)
                //{
                //    _authorizeUser = new AuthorizeUserLogic(new LoginProvider(new UserManagement(), new Audit()), null);
                //}

                //if (_authorizeUser.AuthorizeUser(context.UserName, context.Password))
                //{
                    string fetaureId = ConfigurationManager.AppSettings["APIFeatureId"];
                    UserInfos userInfoObj =  new UserInfos(); //_authorizeUser.GetUserFromCache(context.UserName);
                    if (userInfoObj != null)
                    {
                        var identity = new ClaimsIdentity("JWT");

                        identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
                        identity.AddClaim(new Claim("CEID", userInfoObj.CEID.ToString()));
                        identity.AddClaim(new Claim("canAccessAPI", userInfoObj.FeatureList.Split(',').Any(x => x == fetaureId) ? true.ToString() : false.ToString()));
                        var props = new AuthenticationProperties(new Dictionary<string, string>
                        {
                            {
                                    "client", context.ClientId ?? string.Empty
                            }
                        });
                        var ticket = new AuthenticationTicket(identity, props);
                        context.Validated(ticket);
                        return Task.FromResult<object>(null);
                    }
                    else
                    {
                        context.SetError("invalid_grant", "The user name or password is incorrect.");
                        context.Response.Write("userInfoObj is null!!");
                    }
                //}
                //else
                //{
                //    context.SetError("invalid_grant", "The user name or password is incorrect..");
                //    context.Response.Write(string.Format("Authorization failed for user: {0}!!", context.UserName));
                //}
            }
            else
            {
                context.SetError("invalid_grant", "The user name or password is incorrect...");
                context.Response.Write("Username / Password is empty!!");
            }

            return Task.FromResult<object>(null);
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            // Change authentication ticket for refresh token requests  
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);
            newIdentity.AddClaim(new Claim("newClaim", "newValue"));

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            string userName = context.Identity.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            context.AdditionalResponseParameters.Add("userName", userName);

            return Task.FromResult<object>(null);
        }
    }
}