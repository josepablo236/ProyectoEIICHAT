using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace FrontMVC
{
    public static class GlobalVariables
    {
        public static HttpClient WebApiClient = new HttpClient();
        public static HttpClient WebApiClientJWT = new HttpClient();
        static GlobalVariables()
        {
            WebApiClient.BaseAddress = new Uri("http://localhost:59190//api/");
            WebApiClient.DefaultRequestHeaders.Clear();
            WebApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            WebApiClientJWT.BaseAddress = new Uri("http://localhost:59191/api/");
            WebApiClientJWT.DefaultRequestHeaders.Clear();
            WebApiClientJWT.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
