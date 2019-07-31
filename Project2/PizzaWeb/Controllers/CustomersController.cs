using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PizzaAPI.Model;

namespace PizzaWeb.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private static string _url = "https://project-2-pizza-2.azurewebsites.net/api/";

        // GET: Customers
        public ActionResult Index()
        {
            IEnumerable<Customer> customers = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_url);
                //HTTP GET
                // PizzaAPI.Controllers.CustomerController c = new PizzaAPI.Controllers.CustomerController(_context);
                var responseTask = client.GetAsync("Customers");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Customer>>();
                    readTask.Wait();

                    customers = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    customers = Enumerable.Empty<Customer>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            //Customer c = SearchCustomerId(User.Claims.First().Value);
            //customers = customers.Where(x => x.CustomerId == c.CustomerId);

            
            return View(customers);
        }

        // GET: Customers/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            Customer customers = SearchCustomerId(User.Claims.First().Value);
            return View(customers);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Create([Bind("CustomerId,UserId, Address,PhoneNo")] Customer customer)
        {
            using (var client = new HttpClient())
            {
                customer.UserId = Convert.ToInt32(User.Claims.First().Value);
                client.BaseAddress = new Uri(_url);
                Customer cust = SearchCustomerId(User.Claims.First().Value);
                if (cust != null)
                {

                    ModelState.AddModelError(string.Empty, "Customer already created. You are only allow to create one customer per account.");
                }
                else
                {
                    //HTTP GET
                    var postTask = client.PostAsJsonAsync("Customers", customer);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                }
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Customer customers = SearchCustomerId(User.Claims.First().Value);
            return View(customers);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("id,Address,Firstname,Lastname,PhoneNumber")] Customer customer)
        {

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(_url);
                //HTTP GET
                // PizzaAPI.Controllers.CustomerController c = new PizzaAPI.Controllers.CustomerController(_context);
                var responseTask = client.PutAsJsonAsync("Customers/" + id, customer);
                responseTask.Wait();
                var res = responseTask.Result;
                //customers = JsonConvert.DeserializeObject<List<Customer>>(res).Where(s => s.id == id).FirstOrDefault<Customer>();
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Customer customers = SearchCustomer(id);

            return View(customers);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(_url);

                //HTTP DELETE
                var deleteTask = client.DeleteAsync("Customers/" + id);
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index");
                }
                else //web api sent error response 
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return RedirectToAction(nameof(Index));
        }

        //private bool CustomerExists(int id)
        //{
        //    return _context.Customer.Any(e => e.CustomerId == id);
        //}

        private Customer SearchCustomer(int? id)
        {
            Customer customers = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_url);

                var responseTask = client.GetAsync("Customers/" + id);
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
                }
            }

            return customers;
        }

        private Customer SearchCustomerId(string id)
        {
            Customer customers = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_url);

                var responseTask = client.GetAsync("Customers/" + id);
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
                }
            }

            return customers;
        }
    }
}
