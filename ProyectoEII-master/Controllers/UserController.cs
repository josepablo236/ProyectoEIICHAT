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

        public IActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public ViewResult Login(UserViewModel newuser)
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
            //Hacer un get de mongo y validar si los datos coinciden 
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("User/" + cadena.ToString()).Result;
            return View();
        }

        public IActionResult CreateAccount()
        {
            return View();
        }

        [HttpPost]
        public ViewResult CreateAccount(string user, string pass)
        {
            EspiralController espiral = new EspiralController();
            EspiralViewModel modelespiral = new EspiralViewModel();

            modelespiral.TamañoM = 5;
            modelespiral.TamañoN = 5;
            modelespiral.DireccionRecorrido = "vertical";
            List<string> passencoded1 = espiral.Cifrado(modelespiral, pass);
            string passencoded = string.Join(",", passencoded1);
            UserViewModel newuser = new UserViewModel();
            newuser.User_ = user;
            newuser.Password = passencoded;
           
            //Mandar datos a API
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("User", newuser).Result;
            TempData["SuccessMessage"] = "Saved Successfully";
            return View();
        }

        public IActionResult Forgot()
        {
            return View();
        }

    }
}
