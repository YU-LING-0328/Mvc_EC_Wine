using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7MVC.Repositories;
using Team7MVC.Models;
using Team7MVC.ViewModels;
using System.Net.Mail;

namespace Team7MVC.Controllers
{
    [Authorize(Users = "Admin")]
    public class AdminController : Controller
    {
        public readonly AdminRepository _repo;

        public AdminController()
        {
            _repo = new AdminRepository();
        }
        
        //Dashboard
        public ActionResult Index()
        {
            MonthSaleViewModel monthSaleViewModels;
            monthSaleViewModels = _repo.GetMonthSale();
            monthSaleViewModels.Year_Sale = _repo.GetYearSale();
            monthSaleViewModels.Customer_Count = _repo.GetCustomer_Count();
            monthSaleViewModels.Order_Count = _repo.GetOrder_Count();
            monthSaleViewModels.Origin_Count = _repo.GetOrigin_Count();


            return View(monthSaleViewModels);
        }

        [HttpGet]
        public ActionResult Product()
        {
            var products = _repo.GetAllProducts();
            

            return View(products);
        }

        [HttpPost]
        public ActionResult Product(string SortBy)
        {
            var products = _repo.GetAllProducts(SortBy);

            return View(products);
        }

        [HttpGet]
        public ActionResult ProductCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ProductCreate(string ProductName, string Origin, int Year, int Capacity, decimal UnitPrice, int Stock, string Grade, string Variety, string Area, string Picture, string Introduction, int CategoryId, string Status)
        {
            Products product = new Products()
            {
                ProductName = ProductName,
                Origin = Origin,
                Year = Year,
                Capacity = Capacity,
                UnitPrice = UnitPrice,
                Stock = Stock,
                Grade = Grade,
                Variety = Variety,
                Area = Area,
                Picture = Picture,
                Introduction = Introduction,
                CategoryID = CategoryId,
                Status= Status
            };

            _repo.CreateProduct(product);

            return RedirectToAction("Product");
        }

        [HttpGet]
        public ActionResult ProductReadOnly(int id)
        {
            var productreadonly = _repo.GetProductById(id);
            return View(productreadonly);
        }

        [HttpGet]
        public ActionResult ProductEdit(int Id)
        {
            var product = _repo.GetProduct(Id);
            return View(product);
        }

        [HttpPost]
        public ActionResult ProductEdit(int Id, string ProductName, string Origin, int Year, int Capacity, decimal UnitPrice, int Stock, string Grade, string Variety, string Area, string Picture, string Introduction, int CategoryId, string Status)
        {
            Products product = new Products()
            {
                ProductID = Id,
                ProductName = ProductName,
                Origin = Origin,
                Year = Year,
                Capacity = Capacity,
                UnitPrice = UnitPrice,
                Stock = Stock,
                Grade = Grade,
                Variety = Variety,
                Area = Area,
                Picture = Picture,
                Introduction = Introduction,
                CategoryID = CategoryId,
                Status= Status
            };

            _repo.UpdateProduct(product);

            return RedirectToAction("Product");
        }

        [HttpGet]
        public ActionResult ProductDelete(int Id)
        {
            _repo.DeleteProduct(Id);

            return RedirectToAction("Product");
        }

        [HttpGet]
        public ActionResult Order()
        {
            var orders = _repo.GetAllOrders();

            return View(orders);
        }

        [HttpPost]
        public ActionResult Order(string SortBy)
        {
            var orders = _repo.GetAllOrders(SortBy);

            return View(orders);
        }

        [HttpGet]
        public ActionResult OrderCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult OrderCreate( int CustomerID, DateTime OrderDate, DateTime RequiredDate, DateTime ShippedDate, int ShipperID, string ShipName, string ShipAddress, decimal Freight, string PayWay, DateTime PayDate, string ShipPhone, string ShipCity, string BillAddress, string BillName, string BillCity, string BillPhone, string Status)
        {
            Orders order  = new Orders()
            {
                CustomerID=CustomerID,
                OrderDate= OrderDate,
                RequiredDate= RequiredDate,
                ShippedDate= ShippedDate,
                ShipperID= ShipperID,
                ShipName= ShipName,
                ShipAddress= ShipAddress,
                Freight= Freight,
                PayWay= PayWay,
                PayDate= PayDate,
                ShipPhone= ShipPhone,
                ShipCity= ShipCity,
                BillAddress= BillAddress,
                BillName= BillName,
                BillCity= BillCity,
                BillPhone= BillPhone,
                Status= Status
                //TotalAmount= TotalAmount
            };

            _repo.CreateOrder(order);

            return RedirectToAction("Order");
        }

        [HttpGet]
        public ActionResult OrderReadOnly(int id)
        {
            var orderreadonly = _repo.GetOrderById(id);
            return View(orderreadonly);
        }

        //[HttpGet]
        //public ActionResult OrderReadOnly()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult OrderReadOnly(int CustomerID, DateTime OrderDate, DateTime RequiredDate, DateTime ShippedDate, int ShipperID, string ShipName, string ShipAddress, decimal Freight, string PayWay, DateTime PayDate, string ShipPhone, string ShipCity, string BillAddress, string BillName, string BillCity, string BillPhone, string Status)
        //{
        //    Orders order = new Orders()
        //    {
        //        CustomerID = CustomerID,
        //        OrderDate = OrderDate,
        //        RequiredDate = RequiredDate,
        //        ShippedDate = ShippedDate,
        //        ShipperID = ShipperID,
        //        ShipName = ShipName,
        //        ShipAddress = ShipAddress,
        //        Freight = Freight,
        //        PayWay = PayWay,
        //        PayDate = PayDate,
        //        ShipPhone = ShipPhone,
        //        ShipCity = ShipCity,
        //        BillAddress = BillAddress,
        //        BillName = BillName,
        //        BillCity = BillCity,
        //        BillPhone = BillPhone,
        //        Status = Status
        //        //TotalAmount= TotalAmount
        //    };

        //    _repo.GetOrder(order);

        //    return RedirectToAction("Order");
        //}

        [HttpGet]
        public ActionResult OrderEdit(int Id)
        {
            var order = _repo.GetOrder(Id);
            return View(order);
        }

        [HttpPost]
        public ActionResult OrderEdit(int CustomerID, DateTime OrderDate, DateTime RequiredDate, DateTime ShippedDate, int ShipperID, string ShipName, string ShipAddress, decimal Freight, string PayWay, DateTime PayDate, string ShipPhone, string ShipCity, string BillAddress, string BillName, string BillCity, string BillPhone, string Status)
        {
            Orders order = new Orders()
            {
                CustomerID = CustomerID,
                OrderDate = OrderDate,
                RequiredDate = RequiredDate,
                ShippedDate = ShippedDate,
                ShipperID = ShipperID,
                ShipName = ShipName,
                ShipAddress = ShipAddress,
                Freight = Freight,
                PayWay = PayWay,
                PayDate = PayDate,
                ShipPhone = ShipPhone,
                ShipCity = ShipCity,
                BillAddress = BillAddress,
                BillName = BillName,
                BillCity = BillCity,
                BillPhone = BillPhone,
                Status = Status
                //TotalAmount = TotalAmount
            };

            _repo.UpdateOrder(order);

            return RedirectToAction("Order");
        }

        [HttpGet]
        public ActionResult OrderDelete(int Id)
        {
            _repo.DeleteOrder(Id);

            return RedirectToAction("Order");
        }

        // GET: Customers
        [HttpGet]
        public ActionResult Customer()
        {
            var customers = _repo.GetAllCustomers();

            return View(customers);
        }

        [HttpPost]
        public ActionResult Customer(string SortBy)
        {
            var customers = _repo.GetAllCustomers(SortBy);

            return View(customers);
        }

        [HttpGet]
        public ActionResult CustomerCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CustomerCreate(string Account, string Password, string CustomerName, string Gender, DateTime Birthday, string Email, string Address, string Phone, bool VIP, string Picture, string City)
        {
            Customers customer = new Customers()
            {
                Account= Account,
                Password= Password,
                CustomerName = CustomerName,
                Gender= Gender,
                Birthday= Birthday,
                Email = Email,
                Address= Address,
                Phone= Phone,
                VIP= VIP,
                Picture= Picture,
                City= City
            };

            _repo.CreateCustomer(customer);

            return RedirectToAction("Customer");
        }

        [HttpGet]
        public ActionResult CustomerReadOnly(int id)
        {
            var customerreadonly = _repo.GetCustomerById(id);
            return View(customerreadonly);
        }

        [HttpGet]
        public ActionResult CustomerEdit(int Id)
        {
            var customer = _repo.GetCustomer(Id);
            return View(customer);
        }

        [HttpPost]
        public ActionResult CustomerEdit(int Id, string Account, string Password, string CustomerName, string Gender, DateTime Birthday, string Email, string Address, string Phone, bool VIP, string Picture, string City)
        {
            Customers customer = new Customers()
            {
                CustomerID = Id,
                Account = Account,
                Password = Password,
                CustomerName = CustomerName,
                Gender = Gender,
                Birthday = Birthday,
                Email = Email,
                Address = Address,
                Phone = Phone,
                VIP = VIP,
                Picture = Picture,
                City = City
            };

            _repo.UpdateCustomer(customer);

            return RedirectToAction("Customer");
        }

        [HttpGet]
        public ActionResult CustomerDelete(int Id)
        {
            _repo.DeleteCustomer(Id);

            return RedirectToAction("Customer");
        }

        [HttpGet]
        public ActionResult Messages()
        {
            var messages = _repo.GetAllMessages();

            return View(messages);
        }

        [HttpPost]
        public ActionResult Messages(string SortBy)
        {
            var messages = _repo.GetAllMessages(SortBy);

            return View(messages);
        }

        [HttpGet]
        public ActionResult ReplyMessage(int id)
        {
            var message = _repo.GetMessageById(id);
            return View(message);
        }

        [HttpPost]
        public ActionResult ReplyMessage(int id, string replyContent, string Email)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new System.Net.NetworkCredential("z116017z@gmail.com", "Galazy928136+/-"),
                EnableSsl = true
            };

            if (replyContent != "")
            {
                client.Send("z116017z@gmail.com", Email, "THE MACALLAN", replyContent);
                return RedirectToAction("Messages");
            }
            else
            {
                return View();
            }
        }
    }
}