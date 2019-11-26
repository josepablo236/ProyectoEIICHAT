using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProyectoEII.Models;
using Laboratorio2.Controllers;
using Laboratorio2.Models;
using System.Net.Http;
using FrontMVC;
using System.Threading;
using System.Threading.Tasks;

namespace ProyectoEII.Controllers
{
    public class UserController : Controller
    {
        public string currenttoken;
        Random rn = new Random();
        public string cadena;
        private static readonly HttpClient client = new HttpClient();
        private readonly ILogger<UserController> _logger;
        //. static HttpClient client = new HttpClient();
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
                EspiralController espiral = new EspiralController();
                EspiralViewModel modelespiral = new EspiralViewModel();

                modelespiral.TamañoM = 5;
                modelespiral.TamañoN = 5;
                modelespiral.DireccionRecorrido = "vertical";
                List<string> passencoded1 = espiral.Cifrado(modelespiral, newuser.Password);
                string passencoded = string.Join(",", passencoded1);
                newuser.Password = passencoded;

                cadena = newuser.User_.ToString() + "|" + newuser.Password.ToString() + "|currenttoken" + rn.Next(2, 99);

                HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("User/" + cadena.ToString()).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Chat");
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
            UserViewModel newuser = new UserViewModel();
            newuser.User_ = user;
            newuser.Password = pass;

            return View(newuser);
        }

        [HttpPost]
        public ViewResult CreateAccount(UserViewModel newuser)
        {
            EspiralController espiral = new EspiralController();
            EspiralViewModel modelespiral = new EspiralViewModel();

            modelespiral.TamañoM = 5;
            modelespiral.TamañoN = 5;
            modelespiral.DireccionRecorrido = "vertical";
            List<string> passencoded1 = espiral.Cifrado(modelespiral, newuser.Password);
            string passencoded = string.Join(",", passencoded1);
            newuser.Password = passencoded;

            cadena = newuser.User_.ToString() + "|" + newuser.Password.ToString() + "currenttoken" + rn.Next(2, 99);

            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("User", newuser).Result;
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
            UserViewModel newuser = new UserViewModel();
            newuser.User_ = user;
            newuser.Password = pass;
            return View(newuser);
        }

        [HttpPost]
        public ViewResult Forgot(UserViewModel newuser)
        {
            EspiralController espiral = new EspiralController();
            EspiralViewModel modelespiral = new EspiralViewModel();

            modelespiral.TamañoM = 5;
            modelespiral.TamañoN = 5;
            modelespiral.DireccionRecorrido = "vertical";
            List<string> passencoded1 = espiral.Cifrado(modelespiral, newuser.Password);
            string passencoded = string.Join(",", passencoded1);
            newuser.Password = passencoded;

            cadena = newuser.User_.ToString() + "|" + newuser.Password.ToString() + "currenttoken" + rn.Next(2, 99);

            HttpResponseMessage response = GlobalVariables.WebApiClient.PutAsJsonAsync("User", newuser).Result;
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

        public IActionResult Chat()
        {

            return View();
        }
        public async Task StartTimer(CancellationToken cancellationToken)
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
                                    //matamos la sesión jajajaja                                 
                                }
                            }
                        }
                    }
                    await Task.Delay(10000, cancellationToken);
                    if (cancellationToken.IsCancellationRequested)
                        break;
                }
            });
        }
    }
}
