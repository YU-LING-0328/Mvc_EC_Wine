using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Team7MVC.Models;
using Team7MVC.Repositories;
using Team7MVC.ViewModels;

namespace Team7MVC.Controllers
{
    public class ProductController : Controller
    {
        public readonly ProductRepository _repo;

        public ProductController()
        {
            _repo = new ProductRepository();
        }
        // GET: Product
        [HttpGet]
        public ActionResult Index()
        {
            List<Products> products;

            products = _repo.GetProducts();

            return View(products);
        }



        #region 產品搜尋

        [HttpPost]
        public ActionResult SearchProductName(string search)
        {
            List<Products> products;

            if (search != null)
            {
                products = _repo.GetProducts(search);
            }
            else
            {
                products = _repo.GetProducts();
            }

            ViewData.Model = products;
            return View("Index");
        }

        [HttpPost]
        public ActionResult SearchProductYear(int? Year_s, int? Year_e)
        {
            List<Products> products;

            if (Year_e == null && Year_s != null)
            {
                products = _repo.GetProducts(Year_s);
            }
            else if (Year_s != null)
            {
                products = _repo.GetProducts(Year_s, Year_e);
            }
            else
            {
                products = _repo.GetProducts();
            }

            ViewData.Model = products;
            return View("Index");
        }

        [HttpPost]
        public ActionResult SearchProductPrice(decimal? Price_s, decimal? Price_e)
        {
            List<Products> products;

            if (Price_e == null && Price_s != null)
            {
                products = _repo.GetProducts(Price_s);
            }
            else if (Price_s != null)
            {
                products = _repo.GetProducts(Price_s, Price_e);
            }
            else
            {
                products = _repo.GetProducts();
            }

            ViewData.Model = products;
            return View("Index");
        }

        #endregion

        #region 產品分類

        [HttpPost]
        public ActionResult ProductOrigin(string[] Origin)
        {
            List<Products> products;

            if (Origin != null)
            {
                products = _repo.GetProducts(Origin, 1);
            }
            else
            {
                products = _repo.GetProducts();
            }

            ViewData.Model = products;
            return View("Index");
        }

        [HttpPost]
        public ActionResult ProductCategory(string[] Category)
        {
            List<Products> products;

            if (Category != null)
            {
                products = _repo.GetProducts(Category, 2);
            }
            else
            {
                products = _repo.GetProducts();
            }

            ViewData.Model = products;
            return View("Index");
        }

        public ActionResult NewProduct()
        {
            List<Products> products;


            products = _repo.GetNewProducts();

            ViewData.Model = products;
            return View("Index");
        }

        public ActionResult HotProduct()
        {
            List<Products> products;


            products = _repo.GetHotProducts();

            ViewData.Model = products;
            return View("Index");
        }

        public ActionResult ExpensiveProduct()
        {
            List<Products> products;


            products = _repo.GetExpensiveProducts();

            ViewData.Model = products;
            return View("Index");
        }

        #endregion

        [HttpGet]
        public ActionResult ProductDetail(int Id)
        {
            var product = _repo.GetProductById(Id);
            return View(product);
        }

        [Authorize]
        [HttpPost]
        public ActionResult ProductDetail(int ProductId, int buyQty)
        {
            //var product = _repo.GetProductById(Id);

            //ShopListsViewModel shopLists = new ShopListsViewModel
            //{
            //    ProductId = product.ProductID,
            //    Price = product.UnitPrice,
            //    Quantity = buyQty
            //};

            _repo.CreateShoppingCartData(User.Identity.Name, ProductId, buyQty);

            return RedirectToAction("ShoppingCart");
        }

        [Authorize]
        public ActionResult ShoppingCart()
        {
            List<ShopListsViewModel> shopLists;
            shopLists = _repo.ShopList(User.Identity.Name);

            return View(shopLists);
        }

        [HttpPost]
        public ActionResult ShoppingCart(string nothing)
        {
            return RedirectToAction("Payment");
        }

        [HttpGet]
        public ActionResult DeleteShoppingCartProduct(int ProductID)
        {
            var CustomerID = _repo.GetCustomerID(User.Identity.Name);
            _repo.DeleteShopListProduct(CustomerID, ProductID);

            return RedirectToAction("ShoppingCart");
        }

        [Authorize]
        [HttpGet]
        public ActionResult Payment()
        {
            PaymentViewModel paymentViewModel = new PaymentViewModel();
            List<ShopListsViewModel> shopList;
            shopList = _repo.ShopList(User.Identity.Name);
            paymentViewModel.shopList = shopList;

            return View(paymentViewModel);
        }

        [HttpPost]
        public ActionResult Payment(/*DateTime RequiredDate,*/ string BillFirstName, string BillLastName, string BillPhone, string BillCity, string BillAddress, string ShipFirstName, string ShipLastName, string ShipPhone, string ShipCity, string ShipAddress, string PayWay, string CreditCardNo4, string CreditCardNo8, string CreditCardNo12, string CreditCardNo16, string CreditCardMM, string CreditCardYY, int? CreditCardCSC, string IdentityCard)
        {
            CustomerPayment customerPayment;

            if (string.IsNullOrEmpty(CreditCardNo4) && string.IsNullOrEmpty(CreditCardNo8) && string.IsNullOrEmpty(CreditCardNo12) && string.IsNullOrEmpty(CreditCardNo16) && string.IsNullOrEmpty(CreditCardMM) && string.IsNullOrEmpty(CreditCardYY) && CreditCardCSC == null && string.IsNullOrEmpty(IdentityCard))
            {
                if (string.IsNullOrEmpty(BillFirstName) && string.IsNullOrEmpty(BillLastName) && string.IsNullOrEmpty(BillCity) && string.IsNullOrEmpty(BillAddress) && string.IsNullOrEmpty(BillPhone))
                {
                    customerPayment = new CustomerPayment()
                    {
                        OrderDate = DateTime.Now,
                        //RequiredDate = DateTime.Now,
                        BillName = ShipFirstName + ShipLastName,
                        BillPhone = ShipPhone,
                        BillCity = ShipCity,
                        BillAddress = ShipAddress,
                        ShipperID = 1,
                        ShipName = ShipFirstName + ShipLastName,
                        ShipPhone = ShipPhone,
                        ShipAddress = ShipAddress,
                        ShipCity = ShipCity,
                        PayWay = "ATM",
                        PayDate = DateTime.Now,
                        Freight= 100
                    };
                }
                else
                {
                    customerPayment = new CustomerPayment()
                    {
                        OrderDate = DateTime.Now,
                        //RequiredDate = DateTime.Now,
                        BillName = BillFirstName + BillLastName,
                        BillPhone = BillPhone,
                        BillCity = BillCity,
                        BillAddress = BillAddress,
                        ShipperID = 1,
                        ShipName = ShipFirstName + ShipLastName,
                        ShipPhone = ShipPhone,
                        ShipAddress = ShipAddress,
                        ShipCity = ShipCity,
                        PayWay = "ATM",
                        PayDate = DateTime.Now,
                        Freight = 100
                    };
                }

            }
            else
            {
                if (string.IsNullOrEmpty(BillFirstName) && string.IsNullOrEmpty(BillLastName) && string.IsNullOrEmpty(BillCity) && string.IsNullOrEmpty(BillAddress) && string.IsNullOrEmpty(BillPhone))
                {
                    customerPayment = new CustomerPayment()
                    {
                        OrderDate = DateTime.Now,
                        //RequiredDate = DateTime.Now,
                        BillName = ShipFirstName + ShipLastName,
                        BillPhone = ShipPhone,
                        BillCity = ShipCity,
                        BillAddress = ShipAddress,
                        ShipperID = 1,
                        ShipName = ShipFirstName + ShipLastName,
                        ShipPhone = ShipPhone,
                        ShipAddress = ShipAddress,
                        ShipCity = ShipCity,
                        PayWay = "CreditCard",
                        CreditCardNo = CreditCardNo4 + CreditCardNo8 + CreditCardNo12 + CreditCardNo16,
                        CreditCardCSC = CreditCardCSC,
                        CreditCardDate = CreditCardMM + CreditCardYY,
                        IdentityCard = IdentityCard,
                        PayDate = DateTime.Now,
                        Freight = 100
                    };
                }
                else
                {
                    customerPayment = new CustomerPayment()
                    {
                        OrderDate = DateTime.Now,
                        //RequiredDate = DateTime.Now,
                        BillName = BillFirstName + BillLastName,
                        BillPhone = BillPhone,
                        BillCity = BillCity,
                        BillAddress = BillAddress,
                        ShipperID = 1,
                        ShipName = ShipFirstName + ShipLastName,
                        ShipPhone = ShipPhone,
                        ShipAddress = ShipAddress,
                        ShipCity = ShipCity,
                        PayWay = "CreditCard",
                        CreditCardNo = CreditCardNo4 + CreditCardNo8 + CreditCardNo12 + CreditCardNo16,
                        CreditCardCSC = CreditCardCSC,
                        CreditCardDate = CreditCardMM + CreditCardYY,
                        IdentityCard = IdentityCard,
                        PayDate = DateTime.Now,
                        Freight = 100
                    };
                }



            }

            PaymentViewModel paymentViewModel = new PaymentViewModel()
            {
                customerPayment = customerPayment
            };
            var CustomerID = _repo.GetCustomerID(User.Identity.Name);
            _repo.Payment(paymentViewModel, CustomerID);

            return RedirectToAction("OrderDetail", "OrderDetail");
        }

        public ActionResult GetRedWine()
        {
            List<Products> products;

            products = _repo.GetProducts(new string[] { "紅酒"}, 2);

            ViewData.Model = products;
            return View("Index");
        }

        public ActionResult GetWhiteSpirit()
        {
            List<Products> products;

            products = _repo.GetProducts(new string[] { "白酒" }, 2);

            ViewData.Model = products;
            return View("Index");
        }

        public ActionResult GetMulledWine()
        {
            List<Products> products;

            products = _repo.GetProducts(new string[] { "甜酒/貴腐酒" }, 2);

            ViewData.Model = products;
            return View("Index");
        }

        public ActionResult GetChampagne()
        {
            List<Products> products;

            products = _repo.GetProducts(new string[] { "氣泡酒 / 微氣泡 / 香檳" }, 2);

            ViewData.Model = products;
            return View("Index");
        }

        public ActionResult GetPinkWine()
        {
            List<Products> products;

            products = _repo.GetProducts(new string[] { "粉紅酒" }, 2);

            ViewData.Model = products;
            return View("Index");
        }
    }
}