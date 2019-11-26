using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ProyectoEII.Models;
using System.Net.Http;

namespace FrontMVC.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Chat()
        {
            List<UserViewModel> userslist;
            MessagesUsersViewModel usermessages;
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("User").Result;
            if (response.IsSuccessStatusCode)
            {
                userslist = response.Content.ReadAsAsync<List<UserViewModel>>().Result;
                usermessages = new MessagesUsersViewModel();
                usermessages.Users = userslist;
            }
            else
            {
                return View();
            }
            return View(usermessages);
        }
        public IActionResult Chatear(string user)
        {
            return RedirectToAction("Chat");
        }
    }
}