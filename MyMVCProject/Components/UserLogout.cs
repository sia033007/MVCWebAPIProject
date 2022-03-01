using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyMVCProject.Models;
using Microsoft.AspNetCore.Http;

namespace MyMVCProject.Components
{
    public class UserLogout : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User"));
            //TempData["success"] = $"Hello {user.UserName}";
            return View(user);
        }
    }
}
