using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Microsoft.Owin.Security;
using System.Configuration;

namespace AuthorizationServer.Api.Providers
{
    public class RefreshTokenProvider : IAuthenticationTokenProvider
    {
        private static readonly ConcurrentDictionary<string, AuthenticationTicket> RefreshTokens = new ConcurrentDictionary<string, AuthenticationTicket>();

        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }


        public async Task CreateAsync(AuthenticationTokenCreateContext context)

        {
            var guid = Guid.NewGuid().ToString();

            if (!Int32.TryParse(ConfigurationManager.AppSettings["RefreshTokenExpireTimeInDays"], out int refreshTokenExpireTimeInDays))
            {
                refreshTokenExpireTimeInDays = 1;
            }

            // copy all properties and set the desired lifetime of refresh token  
            var refreshTokenProperties = new AuthenticationProperties(context.Ticket.Properties.Dictionary)
            {
                IssuedUtc = context.Ticket.Properties.IssuedUtc,
                ExpiresUtc = DateTime.UtcNow.AddDays(refreshTokenExpireTimeInDays)
            };
            var refreshTokenTicket = new AuthenticationTicket(context.Ticket.Identity, refreshTokenProperties);

            RefreshTokens.TryAdd(guid, refreshTokenTicket);

            // consider storing only the hash of the handle  
            context.SetToken(guid);

            //Task.FromResult(0);
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            //string header = context.OwinContext.Request.Headers["Authorization"];

            if (RefreshTokens.TryRemove(context.Token, out AuthenticationTicket ticket))
            {
                context.SetTicket(ticket);
            }

            //Task.FromResult(0);
        }
    }
}