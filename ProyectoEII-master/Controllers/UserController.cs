using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProyectoEII.Models;
using Laboratorio2.Models;
using System.Net.Http;
using FrontMVC;
using System.Threading;
using System.Threading.Tasks;
using Laboratorio2.Controllers;

namespace ProyectoEII.Controllers
{
    public class UserController : Controller
    {
        public string currenttoken;
        Random rn = new Random();
        public string cadena;
        private static readonly HttpClient client = new HttpClient();
        private readonly ILogger<UserController> _logger;
        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserViewModel newuser)
        {
            if (newuser.User_ == null || newuser.Password == null)
            {
                ModelState.AddModelError("User_", "Los datos no pueden ser nulos");
            }
            else
            {
               
                var espiral = new EspiralController();
                var modelespiral = new EspiralViewModel();
                modelespiral.TamañoM = 8;
                modelespiral.TamañoN = 8;
                modelespiral.DireccionRecorrido = "vertical";
                newuser.Password = espiral.Cifrado(modelespiral, newuser.Password);
                cadena = newuser.User_.ToString() + "|" + newuser.Password.ToString() + "|currenttoken" + rn.Next(2, 99);
                var response = GlobalVariables.WebApiClient.GetAsync("User/" + cadena.ToString()).Result;
                if (response.IsSuccessStatusCode)
                {
                    return (RedirectToAction("Chat", "Chat", new { emisor = newuser.User_}));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "404. Credenciales incorrectas");
                }
            }
            return View(newuser);
        }

        public IActionResult CreateAccount(string user, string pass)
        {
            var newuser = new UserViewModel();
            newuser.User_ = user;
            newuser.Password = pass;

            return View(newuser);
        }

        [HttpPost]
        public ViewResult CreateAccount(UserViewModel newuser)
        {
            var espiral = new EspiralController();
            var modelespiral = new EspiralViewModel();
            modelespiral.TamañoM = 8;
            modelespiral.TamañoN = 8;
            modelespiral.DireccionRecorrido = "vertical";
            newuser.Password = espiral.Cifrado(modelespiral, newuser.Password);
            cadena = newuser.User_.ToString() + "|" + newuser.Password.ToString() + "currenttoken" + rn.Next(2, 99);
            var response = GlobalVariables.WebApiClient.PostAsJsonAsync("User", newuser).Result;
            if (response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "200. Usuario creado exitosamente");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error del servidor, contacte al administrador");
            }
            return View(newuser);
        }

        public IActionResult Forgot(string user, string pass)
        {
            var newuser = new UserViewModel();
            newuser.User_ = user;
            newuser.Password = pass;
            return View(newuser);
        }

        [HttpPost]
        public ViewResult Forgot(UserViewModel newuser)
        {
            var espiral = new EspiralController();
            var modelespiral = new EspiralViewModel();
            modelespiral.TamañoM = 8;
            modelespiral.TamañoN = 8;
            modelespiral.DireccionRecorrido = "vertical";
            newuser.Password = espiral.Cifrado(modelespiral, newuser.Password);
            cadena = newuser.User_.ToString() + "|" + newuser.Password.ToString() + "currenttoken" + rn.Next(2, 99);
            var response = GlobalVariables.WebApiClient.PutAsJsonAsync("User", newuser).Result;
            if (response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "200. Contraseña actualizada exitosamente");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "404. Usuario no encontrado");
            }
            return View(newuser);
        }

       
        public async Task StartTimer(CancellationToken cancellationToken)
        {

            await Task.Run(async () =>
            {

                while (true)
                {
                    if (cadena != null)
                    {
                        var response = GlobalVariables.WebApiClientJWT.GetAsync("JWT/" + cadena).Result;
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
                    await Task.Delay(1000, cancellationToken);
                    if (cancellationToken.IsCancellationRequested)
                        break;
                }
            });
        }
    }
}
