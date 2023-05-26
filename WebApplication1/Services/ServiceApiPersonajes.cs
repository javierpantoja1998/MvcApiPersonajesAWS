using System.Net.Http.Headers;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class ServiceApiPersonajes
    {
        private string UrlApi;
        private MediaTypeWithQualityHeaderValue Header;

        public ServiceApiPersonajes(IConfiguration configuration)
        {
            this.UrlApi = configuration.GetValue<string>("ApiUrls:ApiPersonajes");
            this.Header =
                new MediaTypeWithQualityHeaderValue("application/json");
        }

        private async Task<T> CallApiAsync<T>(string request)
        {
            using(HttpClientHandler handler = new HttpClientHandler())
            {
                handler.ServerCertificateCustomValidationCallback =
                    (message, cert, chain, sslPolicyErrors) =>
                {
                        return true;
                };

                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(this.UrlApi);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(this.Header);
                    HttpResponseMessage response =
                        await client.GetAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        T data = await response.Content.ReadFromJsonAsync<T>();
                        return data;
                    }
                    else
                    {
                        return default(T);
                    }
                }
            }
            
        }

        public async Task<List<Personaje>> GetPersonajesAsync()
        {
            string request = "api/personajes";
            List<Personaje> personajes =
                await this.CallApiAsync<List<Personaje>>(request);
            return personajes;
        }

        public async Task<string> TestApiAsync()
        {
            string request = "/api/personajes";
            var handler = new HttpClientHandler();



            handler.ServerCertificateCustomValidationCallback =
            (message, cert, chain, sslPolicyErrors) =>
            {
                return true;
            };
            HttpClient client = new HttpClient(handler);



            client.BaseAddress = new Uri(this.UrlApi);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(this.Header);
            HttpResponseMessage response =
            await client.GetAsync(request);
            return "Respuesta: " + response.StatusCode;
        }

    }
}
