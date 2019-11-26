using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProyectoEII.Models;
using Laboratorio2.Controllers;
using Laboratorio2.Models;
using System.Net.Http;
using FrontMVC;

namespace ProyectoEII.Controllers
{
    public class UserController : Controller
    {
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
            if(newuser.User_ == null || newuser.Password == null)
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
                var cadena = newuser.User_.ToString() + "|" + newuser.Password.ToString();
                HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("User/" + cadena.ToString()).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Chat");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "404. Usuario no encontrado");
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

    }
}
