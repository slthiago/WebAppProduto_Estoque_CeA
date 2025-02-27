using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAppProduto.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace WebAppProduto.Pages.Produtos
{
    public class IndexModel : PageModel
    {
        public List<Produto> Produtos { get; private set; }
        string baseUrl = "https://localhost:5001/";

        public async Task<IActionResult> OnGetAsync()
        {
            //verifica autenticação
            if (!HttpContext.User.Identity.IsAuthenticated)
                return Redirect("/Login");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("api/Produtos");
                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().Result;
                    Produtos = JsonConvert.DeserializeObject<List<Produto>>(result);
                }
            }
            return Page();
        }
    }
}
