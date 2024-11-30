using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen_Grupo2.Controllers
{
    public class SitiosController
    {
        //Crud
        //Create
        public async static Task<int> Create(Models.Sitios sitio)
        {
            try
            {
                String jsonObject = JsonConvert.SerializeObject(sitio);
                StringContent contenido = new StringContent(jsonObject, Encoding.UTF8, "application/json");

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.PostAsync(Config.Config.EndPointCreate, contenido);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Respuesta de la API: {result}");
                        return 1;
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Error: {response.ReasonPhrase}, Contenido del error: {errorContent}");
                        return -1;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ha Ocurrido un Error: {ex.ToString()}");
                return -1; // Indica error
            }
        }


        //Read
        public async static Task<List<Models.Sitios>> Get()
        {
            List<Models.Sitios> sitioList = new List<Models.Sitios>();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = null;
                    response = await client.GetAsync(Config.Config.EndPointList);
                    if (response != null)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var result = response.Content.ReadAsStringAsync().Result;
                            try
                            {
                                sitioList = JsonConvert.DeserializeObject<List<Models.Sitios>>(result);
                            }
                            catch (JsonException jex)
                            {

                            }
                        }
                    }
                    return sitioList;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //Update
        public static async Task<int> Update(Models.Sitios sitio)
        {
            try
            {
                String jsonObject = JsonConvert.SerializeObject(sitio);
                StringContent contenido = new StringContent(jsonObject, Encoding.UTF8, "application/json");

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.PutAsync($"{Config.Config.EndPointUpdate}/{sitio.id}", contenido);

                    if (response != null && response.IsSuccessStatusCode)
                    {
                        return 1;
                    }
                    else
                    {
                        Console.WriteLine($"Error al actualizar: {response?.ReasonPhrase}");
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar: {ex}");
                return -1;
            }
        }

        //Delete
        public async static Task<int> Delete(int id)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = null;

                    response = await client.DeleteAsync($"{Config.Config.EndPointDelete}/{id}");

                    if (response != null)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var result = response.Content.ReadAsStringAsync().Result;
                        }
                        else
                        {
                            Console.WriteLine($"Ha Ocurrido un Error: {response.ReasonPhrase}");
                            return -1;
                        }
                    }
                }
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ha Ocurrido un Error: {ex.ToString()}");
                return -1;
            }
        }
    }
}
