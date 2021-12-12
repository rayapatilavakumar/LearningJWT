using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace JWT.Web.Controllers
{
    public class HomeController : Controller
    {
        public static string WebApiURL = "https://localhost:44396/";
        public async Task<ActionResult> Index()

        {
            var tokenbased = string.Empty;
            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Clear();
                client.BaseAddress = new Uri(WebApiURL);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var responseMessage = await client.GetAsync("api/Account/validateLogin?userName=admin&password=admin");

                if (responseMessage.IsSuccessStatusCode)

                {
                    var resultMessage = responseMessage.Content.ReadAsStringAsync().Result;

                    tokenbased = JsonConvert.DeserializeObject<string>(resultMessage);
                    Session["Token"]=tokenbased;
                    Session["UserName"]="admin";

                }
                return Content(tokenbased);
            }

            
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public async Task<ActionResult> GetData()
        {


            var returnMessage = string.Empty;
            using (var client = new HttpClient())
            {

                client.DefaultRequestHeaders.Clear();
                client.BaseAddress = new Uri(WebApiURL);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Session["Token"].ToString()+":"+Session["UserName"].ToString());
                var responseMessage = await client.GetAsync("api/Account/GetData");

                if (responseMessage.IsSuccessStatusCode)

                {
                    var resultMessage = responseMessage.Content.ReadAsStringAsync().Result;

                    returnMessage = JsonConvert.DeserializeObject<string>(resultMessage);
                }
                else
                {
                    returnMessage =responseMessage.ReasonPhrase;
                }
                return Content(returnMessage);
            }
        }
    }
}
