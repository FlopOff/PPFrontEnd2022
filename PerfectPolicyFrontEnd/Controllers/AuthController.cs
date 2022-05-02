using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PerfectPolicyFrontEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PerfectPolicyFrontEnd.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserInfo user)
        {
            string token = "";

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44362/api/");

                var response = client.PostAsJsonAsync("Auth/GenerateToken", user).Result;

                if (response.IsSuccessStatusCode)
                {
                    // logged in
                    token = response.Content.ReadAsStringAsync().Result;

                    // Store the token in the session
                    HttpContext.Session.SetString("Token", token);

                    //!!!
                    //return RedirectToAction("Index", "Company");

                }
                else
                {
                    // there was an issue logging in
                    ViewBag.Error = "The provided credentials were incorrect";
                    // potentially save a message to ViewBag and render in the view
                    return View();
                }
            }
            return RedirectToAction("Index", "Quiz");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
