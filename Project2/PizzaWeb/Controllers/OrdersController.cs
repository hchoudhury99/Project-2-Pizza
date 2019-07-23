using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PizzaAPI.Model;
using System.Threading;
using System.Net;

namespace PizzaWeb.Controllers
{
    public class OrdersController : Controller
    {
        private static string _url = "http://localhost:63875/api/";
        // GET: Orders
        public IActionResult Index()
        {
            //var pizzaDBContext = _context.Orders.Include(o => o.Customer);
            //return View(await pizzaDBContext.ToListAsync());
            IEnumerable<Order> Orders = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_url);
                //int CurrentUserId = Convert.ToInt32(User.Claims.First().Value);

                // Customer c = Customer.FirstOrDefault(x => x.UserId == CurrentUserId);
                //HTTP GET
                //Request.ContentType = User.Claims.First().Value;
                
                var responseTask = client.GetAsync("Orders" );       
                responseTask.Wait();
                
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Order>>();
                    readTask.Wait();


                    Orders = readTask.Result;
                    JsonConvert.SerializeObject(Orders, Formatting.Indented,
                                 new JsonSerializerSettings
                                 {
                                     DateFormatHandling = DateFormatHandling.IsoDateFormat
                                 });
                }
                else //web api sent error response 
                {
                    //log response status here..

                    Orders = Enumerable.Empty<Order>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
                //int userid = Convert.ToInt32(User.Claims.First().Value);
                Customer c = SearchCustomerId(User.Claims.First().Value);
                //Orders = Orders.Where(x => x.Customer.CustomerId == c.CustomerId);
                //ViewBag.CustomerID = c.CustomerId;
                if (Orders == null)
                {
                    Orders = Enumerable.Empty<Order>();
                }
                return View(Orders);
            }
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order order = SearchOrder(id);
            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            //ViewData["CustomerId"] = new SelectList(_context.Customer, "id", "id");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,Customer, CustomerId, OrderDate,Duetime,TotalPrice")] Order order)
        {
            order.OrderDate = DateTime.Now;
            order.Duetime = DateTime.Now.AddMinutes(20);
            //order.Customer = c;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                JsonConvert.SerializeObject(order, Formatting.Indented,
                 new JsonSerializerSettings
                 {
                     DateFormatHandling = DateFormatHandling.IsoDateFormat
                 });
                //int id = Convert.ToInt32(User.Claims.First().Value);
                client.BaseAddress = new Uri(_url);
       
                Customer c = SearchCustomerId(User.Claims.First().Value);
                order.CustomerId = c.CustomerId;

                var postTask = client.PostAsJsonAsync("Orders", order);

                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                ////
            }

            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order order = SearchOrder(id);
           // ViewData["CustomerId"] = new SelectList(_context.Customer, "id", "id", order.Customer.CustomerId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,CustomerId,OrderTime,DeliveryTime,TotalPrice,OrderType")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }
            Order orders = null;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(_url);
                //HTTP GET
                // PizzaAPI.Controllers.CustomerController c = new PizzaAPI.Controllers.CustomerController(_context);
                var responseTask = client.PutAsJsonAsync("Orders/" + id, order);
                responseTask.Wait();
                var res = responseTask.Result;
                //customers = JsonConvert.DeserializeObject<List<Customer>>(res).Where(s => s.id == id).FirstOrDefault<Customer>();
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }


           // ViewData["CustomerId"] = new SelectList(_context.Customer, "id", "id", order.Customer.CustomerId);
            return View(orders);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Order order = SearchOrder(id);

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(_url);

                //HTTP DELETE
                var deleteTask = client.DeleteAsync("Orders/" + id);
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

        //private bool OrderExists(int id)
        //{
        //    return _context.Order.Any(e => e.OrderId == id);
        //}

        private Order SearchOrder(int? id)
        {
            Order order = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_url);

                var responseTask = client.GetAsync("Orders/" + id);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Order>();
                    readTask.Wait();
                    order = readTask.Result;
                    //customers = readTask.Result.Where(s => s.id == id).FirstOrDefault<Customer>();
                }
                else //web api sent error response 
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return (order);
        }

        private Customer SearchCustomerId(string id)
        {
            Customer customers = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_url);

                var responseTask = client.GetAsync("Customers/"+  id);
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
