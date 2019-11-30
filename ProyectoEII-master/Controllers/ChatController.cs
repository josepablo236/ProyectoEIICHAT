using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ProyectoEII.Models;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Laboratorio2.Models;
using Laboratorio2.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using Laboratorio_1.Models;
using Laboratorio_1.Controllers;
using Microsoft.AspNetCore.Hosting;

namespace FrontMVC.Controllers
{
    public class ChatController : Controller
    {
        Random rn = new Random();
        public string currenttoken;
        private readonly IHostingEnvironment _env;
        private static readonly HttpClient client = new HttpClient();
        private readonly ILogger<ChatController> _logger;
        public string cadena;
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
                var espiral = new EspiralController();
                var modelespiral = new EspiralViewModel();
                modelespiral.TamañoM = 8;
                modelespiral.TamañoN = 8;
                modelespiral.DireccionRecorrido = "vertical";
                var responseMessages = GlobalVariables.WebApiClient.GetAsync("Message").Result;
                if (responseMessages.IsSuccessStatusCode)
                {
                    var listMessage = responseMessages.Content.ReadAsAsync<List<MessagesViewModel>>().Result;
                    foreach (var item in listMessage)
                    {
                        item.Message_ = espiral.Descifrado(modelespiral, item.Message_.ToString());
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
            var list = new List<string>();
            var list2 = new List<string>();
            var espiral = new EspiralController();
            var modelespiral = new EspiralViewModel();
            modelespiral.TamañoM = 15;
            modelespiral.TamañoN = 15;
            modelespiral.DireccionRecorrido = "vertical";
            var usermessages = new MessagesUsersViewModel();
            var messages = new MessagesViewModel();
            if (Message == null)
            {
                HttpResponseMessage responseUsers = GlobalVariables.WebApiClient.GetAsync("User").Result;
                if (responseUsers.IsSuccessStatusCode)
                {
                    usermessages.Users = responseUsers.Content.ReadAsAsync<List<UserViewModel>>().Result;
                    usermessages.ActualUser = emisor;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "400. No se obtuvo ningún usuario");
                }
                if (receptor != null)
                {
                    var cadena = emisor + "|" + receptor;
                    var responseMessages = GlobalVariables.WebApiClient.GetAsync("Message/" + cadena.ToString()).Result;
                    if (responseMessages.IsSuccessStatusCode)
                    {
                        var listMessage = responseMessages.Content.ReadAsAsync<List<MessagesViewModel>>().Result;
                        foreach (var item in listMessage)
                        {
                            item.Message_ = espiral.Descifrado(modelespiral, item.Message_.ToString());
                        }
                        usermessages.Messages = listMessage;
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
                { 
                    return View(usermessages); 
                }
            }
          
        }

        public IActionResult UploadFile()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(List<IFormFile> files)
        {
            var file = files[0];
            foreach (var formFile in files)
            {
                using (var stream = new FileStream(file.FileName, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }
            }
            string[] nombreArchivo = file.FileName.Split('.');

            if (nombreArchivo[1] == "txt")
            {
                CompresionLZW H = new CompresionLZW();
                string path = _env.WebRootPath +"/ArchivosTemp/";
                string pathPrueba = path + nombreArchivo[0];
                path = path + file.FileName;
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }


                H.Compresion(path, pathPrueba);
                return File(pathPrueba, "lzw", (nombreArchivo[0] + ".lzw"));
            }
            else if (nombreArchivo[1] == "lzw")
            {
                DescompresionLZW H = new DescompresionLZW();
                string path = _env.WebRootPath + "/ArchivosTemp/";
                string pathPrueba = path + nombreArchivo[0];
                path = path + file.FileName;
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                H.Descompresion(pathPrueba, path);
                ViewBag.ok = "Proceso completado :)";
                return File(pathPrueba, "txt", (nombreArchivo[0] + ".txt"));
            }
            return View();

        }

        public bool SendMessage(string Message, string emisor, string receptor)
        {
            if(receptor != null)
            {
                var messagemodel = new MessagesViewModel();
                messagemodel.Emisor = emisor;
                messagemodel.Receptor = receptor;
                var espiral = new EspiralController();
                var modelespiral = new EspiralViewModel();
                modelespiral.TamañoM = 15;
                modelespiral.TamañoN = 15;
                modelespiral.DireccionRecorrido = "vertical";
                messagemodel.Message_ = espiral.Cifrado(modelespiral, Message);
                messagemodel.Date = DateTime.Now;
                cadena = emisor.ToString() + "|" + receptor.ToString() + "|currenttoken" + rn.Next(2, 99);
                StartTimer();
                var response = GlobalVariables.WebApiClient.PostAsJsonAsync("Message", messagemodel).Result;
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public async Task StartTimer()
        {
            await Task.Run(async () =>
            {

                while (true)
                {
                    if (cadena != null)
                    {
                        HttpResponseMessage response = GlobalVariables.WebApiClientJWT.GetAsync("JWT/" + cadena).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            if (currenttoken == null)
                            {
                                currenttoken = (response.Content.ReadAsAsync<string>().Result);

                            }
                            else
                            {
                                if (currenttoken != (response.Content.ReadAsAsync<string>().Result))
                                {
                                    RedirectToAction("Login", "User");
                                }
                            }
                        }
                    }
                    await Task.Delay(1000);
                    break;
                }
            });
        }
    }
}