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
<<<<<<< HEAD
            WebApiClient.BaseAddress = new Uri("http://localhost:50859//api/");
            WebApiClient.DefaultRequestHeaders.Clear();
            WebApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            WebApiClientJWT.BaseAddress = new Uri("http://localhost:50858/api/");
=======
            WebApiClient.BaseAddress = new Uri("http://localhost:57224//api/");
            WebApiClient.DefaultRequestHeaders.Clear();
            WebApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            WebApiClientJWT.BaseAddress = new Uri("http://localhost:57226/api/");
>>>>>>> f21117e400782dc5234135987522b3749add4c4b
            WebApiClientJWT.DefaultRequestHeaders.Clear();
            WebApiClientJWT.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
