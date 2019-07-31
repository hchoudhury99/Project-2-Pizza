using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PizzaAPI;
using PizzaAPI.Model;

namespace PizzaWeb.Controllers
{
    public class HomeController : Controller
    {
        private static string _url = "https://project-2-pizza-2.azurewebsites.net/api/";
        public IActionResult Index()
        {
            Customer customers = null;
            if (User.Claims.Count() != 0)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_url);

                    var responseTask = client.GetAsync("Customers/" + Convert.ToInt32(User.Claims.First().Value));
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<Customer>();
                        readTask.Wait();
                        customers = readTask.Result;

                    }
                    else //web api sent error response 
                    {

                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                        return RedirectToAction("create", "customers");
                    }
                }
            }
            return View();
        }


        public IActionResult TopChoice()
        {
            Customer customers = null;
            if (User.Claims.Count() != 0)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_url);

                    var responseTask = client.GetAsync("Customers/" + Convert.ToInt32(User.Claims.First().Value));
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<Customer>();
                        readTask.Wait();
                        customers = readTask.Result;

                    }
                    else //web api sent error response 
                    {

                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                        return RedirectToAction("create", "customers");
                    }
                }
            }
            return View();
        }



        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
