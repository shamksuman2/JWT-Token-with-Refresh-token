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
    [RoutePrefix("api/Values")]
    [Authorize]
    public class ValuesController : ApiController
    {
        [Route("get")]
        public IHttpActionResult Get()
        {
            return Ok();
        }
    }
}
