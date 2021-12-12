using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace JWT.API.Controllers
{
    public class AccountController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage ValidateLogin(string userName, string password)
        {
            if (userName =="admin" && password == "admin")
            {
                return Request.CreateResponse(HttpStatusCode.OK, TokenManager.GenerateToken(userName));

            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "User Name / Password  is invalid");
        }
    }
}
