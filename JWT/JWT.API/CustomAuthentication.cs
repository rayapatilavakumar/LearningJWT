using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Results;

namespace JWT.API
{
    public class CustomAuthentication : AuthorizeAttribute, IAuthenticationFilter
    {
        public bool AllowMultiple { get { return false; } }

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var authParameter = string.Empty;
            var request = context.Request;
            var authorization = request.Headers.Authorization;

            string[] tokenanduser = null;
            if(authorization == null)
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing authorization Header", request);

                return;
            }

           
            if (authorization.Scheme != "Bearer")
            {
                context.ErrorResult = new AuthenticationFailureResult("Invalid Authorization Scheme", request);
                return;
            }

            tokenanduser = authorization.Parameter.Split(':');
            string token = tokenanduser[0];
            string un = tokenanduser[1];
            if (string.IsNullOrEmpty(token))
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing Token", request);
                return;
            }


            string validusername = TokenManager.GetClaim(token);

            if(un != validusername)
            {
                context.ErrorResult = new AuthenticationFailureResult("Invalid Token for user", request);
                return;

            }
            context.Principal = TokenManager.GetPrincipal(token);
        }

        public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            var result = await context.Result.ExecuteAsync(cancellationToken);

            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                result.Headers.WwwAuthenticate.Add(new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", "realm=localhost"));
            }
            context.Result = new ResponseMessageResult(result);
        }
    }
    public class AuthenticationFailureResult : IHttpActionResult
    {
        public string ReasonPhrase;
        public HttpRequestMessage Request { get; set; }
        public AuthenticationFailureResult(string reasonPhrase, HttpRequestMessage requset)
        {
            ReasonPhrase = reasonPhrase;
            Request = requset;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        public HttpResponseMessage Execute()
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);

            responseMessage.RequestMessage = Request;
            responseMessage.ReasonPhrase = ReasonPhrase;

            return responseMessage;
        }
    }
}