using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PizzaAPI.Model;

namespace PizzaWeb.Controllers
{
    public class PaymentsController : Controller
    {
        private static string _url = "http://localhost:63875/api/";
        // GET: Payments
        public async Task<IActionResult> Index()
        {
            IEnumerable<Payment> payments = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_url);
                //HTTP GET
                // PizzaAPI.Controllers.CustomerController c = new PizzaAPI.Controllers.CustomerController(_context);
                var responseTask = client.GetAsync("Payments");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<Payment>>();
                    readTask.Wait();

                    payments = readTask.Result;
                }
                else //web api sent error response 
                {
                    //log response status here..

                    payments = Enumerable.Empty<Payment>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            Customer c = SearchCustomerId(User.Claims.First().Value);
            payments = payments.Where(x => x.CustomerId == c.CustomerId);
            return View(payments);
        }

        // GET: Payments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var payment = SearchPayments(id);
            return View(payment);
        }

        // GET: Payments/Create
        public IActionResult Create()
        {
            //ViewData["CustomerID"] = new SelectList(_context.Customers, "id", "id");
            return View();
        }

        // GET: Payments/Create
        public IActionResult CreatePayment()
        {
            //ViewData["CustomerID"] = new SelectList(_context.Customers, "id", "id");
            return View();
        }

        // POST: Payments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PaymentId,CardNo,CustomerId")] Payment payment)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_url);
                //JsonConvert.SerializeObject(payment, Formatting.Indented,
                //     new JsonSerializerSettings
                //     {
                //         DateFormatHandling = DateFormatHandling.IsoDateFormat
                //     });
                //HTTP GET
                // PizzaAPI.Controllers.CustomerController c = new PizzaAPI.Controllers.CustomerController(_context);
                int id = Convert.ToInt32(User.Claims.First().Value);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));


                //var data = JsonConvert.SerializeObject(payment);
                //var jsonData = new StringContent(data, Encoding.UTF8, "application/json");
                Customer c = SearchCustomerId(User.Claims.First().Value);
                payment.CustomerId = c.CustomerId;

                var postTask = client.PostAsJsonAsync("Payments/" +id, payment);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            }
            //ViewData["CustomerID"] = new SelectList(_context.Customers, "id", "id", payment.CustomerID);
            return View(payment);
        }

        // GET: Payments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = SearchPayments(id);
            //ViewData["CustomerID"] = new SelectList(_context.Customers, "id", "id", payment.CustomerID);
            return View(payment);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PaymentId,CardNumber,ExpireDate,CVV,CustomerID")] Payment payment)
        {
            if (id != payment.PaymentId)
            {
                return NotFound();
            }
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(_url);
                //HTTP GET
                // PizzaAPI.Controllers.CustomerController c = new PizzaAPI.Controllers.CustomerController(_context);
                var responseTask = client.PutAsJsonAsync("Payments/" + id, payment);
                responseTask.Wait();
                var res = responseTask.Result;
                //customers = JsonConvert.DeserializeObject<List<Customer>>(res).Where(s => s.id == id).FirstOrDefault<Customer>();
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }



            //ViewData["CustomerID"] = new SelectList(_context.Customers, "id", "id", payment.CustomerID);
            //return View(payment);
            return RedirectToAction(nameof(Index));
        }

        // GET: Payments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = SearchPayments(id);

            return View(payment);
        }

        // POST: Payments/Delete/5
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
                var deleteTask = client.DeleteAsync("Payments/" + id);
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

        //private bool PaymentExists(int id)
        //{
        //    return _context.Payments.Any(e => e.PaymentId == id);
        //}

        private Payment SearchPayments(int? id)
        {
            Payment payment = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_url);

                var responseTask = client.GetAsync("Payments/" + id);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Payment>();
                    readTask.Wait();
                    payment = readTask.Result;
                }
                else //web api sent error response 
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }

            return payment;
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
