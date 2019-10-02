using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Team7MVC.ViewModels
{
    public class OrderDetailViewModel
    {
        public CustomerDetail customerDetails { get; set; }

        public List<ProductDetail> productDetails { get; set; }
    }
    public class CustomerDetail
    {
        public string BillName { get; set; }
        public string Email { get; set; }
        public string BillCity { get; set; }
        public string BillAddress { get; set; }
        public string BillPhone { get; set; }
        public string ShipName { get; set; }
        public string ShipCity { get; set; }
        public string ShipAddress { get; set; }
        public string ShipPhone { get; set; }
        public string PayWay { get; set; }
        public int Freight { get; set; }
    }

    public class ProductDetail
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
        public string Picture { get; set; }
    }
}