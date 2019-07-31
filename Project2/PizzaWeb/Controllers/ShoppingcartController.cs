using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PizzaAPI.Model;
using PizzaWeb.catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace PizzaWeb.Controllers
{
    [Authorize]

    public class ShoppingcartController : Controller
    {
        private CatalogContext _context;
        

        public ShoppingcartController(CatalogContext context)
        {
            _context = context;
        }
        // GET: Shoppingcart
        public ActionResult Index()
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

            //List<Pizza> pizza = _context.Orders.Where(i=>i.Customer.UserId==Convert.ToInt32(userId)).FirstOrDefault().Pizza as List<Pizza>;
            //var pizza = _context.Pizza.FirstOrDefault();
            //return Content(JsonConvert.SerializeObject(pizza));
            List<Pizza> myPizzas = new List<Pizza>();
            foreach (var item in _context.TempPizzas.ToList())
            {
                //OrderId is used as UserID, so check userID against it
                if (item.OrderId == userId)
                {
                    myPizzas.Add(item);
                }
            }
            return View(myPizzas);
        }

        // GET: Shoppingcart/Details/5
        //public ActionResult Details(int id)
        //{
           // return View();
       // }

        // GET: Shoppingcart/Create
        //public ActionResult Create()
        //{
            //return View();
        //}

        // POST: Shoppingcart/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Pizza p)
        {
            try
            {
                p.Price = p.Total();
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                //for shoppingcart purposes orderId is set to UserID
                p.OrderId = Convert.ToInt32(userId);
                _context.TempPizzas.Add(p);
                _context.SaveChanges();
                //return RedirectToAction(nameof(Index));
                if (p.PizzaName.ToString().Equals("customPizza"))
                {
                    return RedirectToAction("Index", "Shoppingcart");
                }
                return RedirectToAction("TopChoice", "Home");
            }
            catch
            {
            
                return View();
            }

        }

        // GET: Shoppingcart/Edit/5
        //public ActionResult Edit(int id)
        //{
        //   return View();
        //}

        // POST: Shoppingcart/Edit/5
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }*/

        //GET: Shoppingcart/Delete/5
        public IActionResult Delete(int id)
        {
            var item = _context.TempPizzas.Find(id);
            _context.TempPizzas.Remove(item);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
            
        }

        
        // POST: Shoppingcart/Delete/5
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete_post(int id)
        {
            try
            {
                // TODO: Add delete logic here
                //var item = _context.TempPizzas.Where(i => i.PizzaId == id).FirstOrDefault();
                //_context.TempPizzas.Remove(_context.TempPizzas.Find(id));
                var item = _context.TempPizzas.Find(id);
                _context.TempPizzas.Remove(item);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }*/
    }
}