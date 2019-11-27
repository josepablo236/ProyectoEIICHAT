using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ProyectoEII.Models;
using System.Net.Http;

namespace FrontMVC.Controllers
{
    public class ChatController : Controller
    {
        public static string receptorglobal;
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Chat(string emisor)
        {
            List<UserViewModel> userslist;
            MessagesUsersViewModel usermessages = new MessagesUsersViewModel();
            MessagesViewModel messages = new MessagesViewModel();
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("User").Result;
            if (response.IsSuccessStatusCode)
            {
                userslist = response.Content.ReadAsAsync<List<UserViewModel>>().Result;
                usermessages.Users = userslist;
                usermessages.ActualUser = emisor;
            }
            else
            {
                ModelState.AddModelError(string.Empty, "400. No se obtuvo ningún mensaje");
            }
            return View(usermessages);
        }
        public IActionResult Chatear(string emisor, string receptor)
        {
            receptorglobal = receptor;
            MessagesUsersViewModel messagemodel = new MessagesUsersViewModel();
            var cadena = emisor + "|" + receptor;
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Message/" + cadena.ToString()).Result;
            if(response.IsSuccessStatusCode)
            {
                messagemodel.Messages = response.Content.ReadAsAsync<List<MessagesViewModel>>().Result;
            }
            else
            {
                ModelState.AddModelError(string.Empty, "400. No se obtuvo ningún mensaje");
            }
            return RedirectToAction("Chat", new { emisor = emisor });
        }
        public IActionResult SendMessage(string emisor, string Message)
        {
            MessagesViewModel messagemodel = new MessagesViewModel();
            messagemodel.Emisor = emisor;
            messagemodel.Receptor = receptorglobal;
            messagemodel.Message = Message;
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Message", messagemodel).Result;
            if (response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "200. Mensaje enviado exitosamente");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error del servidor, contacte al administrador");
            }
            return (RedirectToAction("Chatear", "Chat", new { emisor = messagemodel.Emisor , receptor = messagemodel.Receptor}));
        }
    }
}