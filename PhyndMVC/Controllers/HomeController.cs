using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PhyndMVC.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using System.Web;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace PhyndMVC.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IHttpClientFactory httpClientFactory;
        // private JsonSerializer JsonSerializer;
        private IHttpContextAccessor _httpContext;
        //private  IHttpContextAccessor _httpContextAccessor;
        public HomeController(IHttpClientFactory clientFactory, IHttpContextAccessor httpContext, ILogger<HomeController> logger)
        {
            httpClientFactory = clientFactory;
            // JsonSerializer = jsonSerializer;
            _httpContext = httpContext;
            _logger = logger;
            //_httpContextAccessor = httpContextAccessor;
        }


        public IActionResult Index()
        {
            return View();
        }

   
        public IActionResult Login() {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(Models.LoginUser user)
        {
            HttpContent httpContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(user), System.Text.Encoding.UTF8);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var message = new HttpRequestMessage();
            message.Content = httpContent;
            message.Method = HttpMethod.Post;
            message.RequestUri = new Uri("https://localhost:44342/authenticate");

            HttpClient client = httpClientFactory.CreateClient();
            HttpResponseMessage responseMessage = await client.SendAsync(message);
            var result = await responseMessage.Content.ReadAsStringAsync();

            Token resultfinal = JsonConvert.DeserializeObject<Token>(result);
            Console.WriteLine(result);

            //Cookies
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,resultfinal.email)
            };
            var claimIdentity = new ClaimsIdentity(claims, "Login");

            //var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name);
            //var claimIdentity = new ClaimsIdentity(new[] {
            //    new Claim(ClaimTypes.Name,resultfinal.email)
            //}, "Cookies");


            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true, 
                ExpiresUtc = DateTimeOffset.Now.AddDays(1),
                IsPersistent = true,
            };
            await _httpContext.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity));

            //Console.WriteLine("HIiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii");
            //_httpContextAccessor.HttpContext.Session.SetString("name",result);
            //return Redirect(ReturnUrl == null ?"/Homepage":ReturnUrl);
            return RedirectToAction("Index", "Menu");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _httpContext.HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}
