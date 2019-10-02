using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7MVC.Models;
using Team7MVC.Repositories;

namespace Team7MVC.Controllers
{
    public class CustomerController : Controller
    {
        public readonly CustomerRepository _repo;
       

        public CustomerController()
        {
            _repo = new CustomerRepository();
        }

        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CustomerEdit()
        {
            var customers = _repo.GetCustomersId(User.Identity.Name);
            return View(customers);
        }

        [HttpPost]
        public ActionResult CustomerEdit(string CustomerName, string Account, string Gender, string Email, string Phone, string Address, DateTime Birthday, string VIP)
        {
            Customers customers = new Customers()
            {
                CustomerName = CustomerName,
                Account = User.Identity.Name,
                Gender = Gender,
                Birthday = Birthday,
                Email = Email,
                Phone = Phone,
                Address = Address
            };

            _repo.UpdateCustomer(customers);

            return RedirectToAction("CustomerEdit");
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            var customers = _repo.GetCustomersId(User.Identity.Name);
            return View(customers);
        }

        [HttpPost]
        public ActionResult ChangePassword(Customers customers)
        {

            if (!ModelState.IsValid)
            {
                if(customers.NewPassword != customers.ConfirmPassword)
                {
                    return View(customers);

                }
                return View(customers);
            }

            //if (customers.NewPassword != customers.ConfirmPassword)
            //{
            //    return View(customers);
            //}


            if (customers.NewPassword == customers.ConfirmPassword)
            {
                _repo.UpdatePassword(customers);
            }

            return RedirectToAction("ChangePassword");

        }

        [HttpGet]
        public ActionResult PurchaseList()
        {
            var orders = _repo.OrderQuery(User.Identity.Name);
            return View(orders);
        }
        [HttpGet]
        public ActionResult Order_Details(int Id)
        {
            var orders_Details = _repo.QueryOrderDetails(Id);
            return View(orders_Details);
        }

        [HttpGet]
        public ActionResult PaywayInfo()
        {
            var PaywayInfo = _repo.QueryPaywayInfo(User.Identity.Name);
            return View(PaywayInfo);
        }
    }
}