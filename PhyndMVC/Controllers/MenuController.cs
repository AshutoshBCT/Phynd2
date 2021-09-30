using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace PhyndMVC.Controllers
{
    [Authorize]
    public class MenuController : Controller
    {

        public MenuController()
        {

            //_httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            //if (_httpContextAccessor.HttpContext.Session.GetString("name") == null)
            //{
            //  return RedirectToAction("Index", "Login");
            //}
            //name = HttpContext.Session.GetString("name");
            return View();
        }

    }
}
