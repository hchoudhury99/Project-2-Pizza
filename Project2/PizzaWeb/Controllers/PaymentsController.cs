using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PizzaAPI.Model;

namespace PizzaWeb.Controllers
{
    [Authorize]
    public class PaymentsController : Controller
    {

        private static string _url = "https://project-2-pizza-2.azurewebsites.net/api/";
        // GET: Payments
        public async Task<IActionResult> Index()
        {
            Customer cust = SearchCustomerId(User.Claims.First().Value);
            IEnumerable<Payment> payments = GetAllPayment();
            //payments = payments.Where(x => x.CustomerId == cust.CustomerId);
            //if (payments == null)
            //{
            //    payments = Enumerable.Empty<Payment>();
            //}

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

        // GET: Payments/Details/5
        public IActionResult Confirmation(int? id)
        {
            Order order = null;
            if (id != null)
            {
                order = SearchOrder(id);
            }

            //ViewData["CustomerID"] = new SelectList(_context.Customers, "id", "id");
            return View(order);
        }

        // GET: Payments/Create
        public IActionResult Create(int? id)
        {
            OrderPaymentVM orderPayment = new OrderPaymentVM();
            if(id != null)
            {
                orderPayment.order = SearchOrder(id);
            }
            
            //ViewData["CustomerID"] = new SelectList(_context.Customers, "id", "id");
            return View(orderPayment);
        }



        // POST: Payments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("payment, order")]  OrderPaymentVM orderPayment)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_url);
                //HTTP GET
                // PizzaAPI.Controllers.CustomerController c = new PizzaAPI.Controllers.CustomerController(_context);
                int id = Convert.ToInt32(User.Claims.First().Value);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
 

                Customer cust = SearchCustomerId(User.Claims.First().Value);
                //Payment payments = GetAllPayment().FirstOrDefault(x => x.CustomerId == cust.CustomerId);
                Order order= SearchOrder(Convert.ToInt32(this.RouteData.Values.Values.Last()));
                orderPayment.order = order;

                if (cust == null || GetAllPayment().FirstOrDefault(x => x.CustomerId == cust.CustomerId) != null)
                {

                    ModelState.AddModelError(string.Empty, "Customer or payment information no exist! Please use the create one or update it.");
                }
                else
                {
                    Payment payment = orderPayment.payment;
                    var postTask = client.PostAsJsonAsync("Payments/" + cust.CustomerId, payment);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Confirmation", new { id = order.OrderId, });
                    }
                    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                }
            }
            //ViewData["CustomerID"] = new SelectList(_context.Customers, "id", "id", payment.CustomerID);
            return View(orderPayment);
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
        public async Task<IActionResult> Edit(int id, [Bind("PaymentId,CardNo,CustomerId")] Payment payment)
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
                Customer cust = SearchCustomerId(User.Claims.First().Value);
                Order order = SearchAllOrder().FirstOrDefault(x => x.Customer.CustomerId == cust.CustomerId);
                //customers = JsonConvert.DeserializeObject<List<Customer>>(res).Where(s => s.id == id).FirstOrDefault<Customer>();
                if (res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Confirmation", new { id = order.OrderId, });
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


        private IEnumerable<Payment> GetAllPayment()
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
            return payments;
        }


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

        private IEnumerable<Order> SearchAllOrder()
        {
            IEnumerable<Order> Orders = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_url);
                //int CurrentUserId = Convert.ToInt32(User.Claims.First().Value);

                // Customer c = Customer.FirstOrDefault(x => x.UserId == CurrentUserId);
                //HTTP GET
                //Request.ContentType = User.Claims.First().Value;

                var responseTask = client.GetAsync("Orders");
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
                return (Orders);
            }
        }
    }
}
