using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ProyectoEII.Models;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Laboratorio_1.Controllers;
using Laboratorio_1.Models;

namespace FrontMVC.Controllers
{
    public class ChatController : Controller
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly ILogger<ChatController> _logger;
        public ChatController(ILogger<ChatController> logger)
        {
            _logger = logger;
        }
        public IActionResult IndexMessages(IndexMessagesViewModel modelView)
        {
            var model = new IndexMessagesViewModel();
            model.listMessage = new List<MessagesViewModel>();
            if (string.IsNullOrEmpty(modelView.Busqueda))
            {
                return View(model);
            }
            else
            {
                HttpResponseMessage responseMessages = GlobalVariables.WebApiClient.GetAsync("Message").Result;
                if (responseMessages.IsSuccessStatusCode)
                {
                    var listMessage = responseMessages.Content.ReadAsAsync<List<MessagesViewModel>>().Result;
                    foreach (var item in listMessage)
                    {
                        if (item.Message_.Contains(modelView.Busqueda) && (modelView.Usuario == item.Emisor || modelView.Usuario == item.Receptor))
                        {
                            model.listMessage.Add(item);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "400. No se obtuvo ningún mensaje");
                }
                model.Busqueda = modelView.Busqueda;
                return View(model);
            }
        }
        public IActionResult Chat(string emisor, string receptor, string Message)
        {
            List<UserViewModel> userslist;
            MessagesUsersViewModel usermessages = new MessagesUsersViewModel();
            MessagesViewModel messages = new MessagesViewModel();
            if (Message == null)
            {
                HttpResponseMessage responseUsers = GlobalVariables.WebApiClient.GetAsync("User").Result;
                if (responseUsers.IsSuccessStatusCode)
                {
                    userslist = responseUsers.Content.ReadAsAsync<List<UserViewModel>>().Result;
                    usermessages.Users = userslist;
                    usermessages.ActualUser = emisor;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "400. No se obtuvo ningún usuario");
                }
                if (receptor != null)
                {
                    var cadena = emisor + "|" + receptor;
                    HttpResponseMessage responseMessages = GlobalVariables.WebApiClient.GetAsync("Message/" + cadena.ToString()).Result;
                    if (responseMessages.IsSuccessStatusCode)
                    {
                        usermessages.Messages = responseMessages.Content.ReadAsAsync<List<MessagesViewModel>>().Result;
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "400. No se obtuvo ningún mensaje");
                    }
                }
                return View(usermessages);
            }
            else
            {
                var status = SendMessage(Message, emisor, receptor);
                if (status == true)
                {
                    return RedirectToAction("Chat", new { emisor = emisor, receptor = receptor });
                }
                else
                { return View(usermessages); 
                }
            }
          
        }

        public IActionResult UploadFile()
        {
            HomeController home = new HomeController();
            home.Compresion_Descompresion_LZW();
            return View();
        }

       [HttpPost] 
       public IActionResult UploadFile(string file)
        {
            return View();
        }
        public bool SendMessage(string Message, string emisor, string receptor)
        {
            MessagesViewModel messagemodel = new MessagesViewModel();
            messagemodel.Emisor = emisor;
            messagemodel.Receptor = receptor;
            //Cifrar mensaje y asignarlo a la variable del modelo
            messagemodel.Message_ = Message;
            messagemodel.Date = DateTime.Now;
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Message", messagemodel).Result;
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
           
        }
    }
}