using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Team7MVC.ViewModels
{
    public class PaymentViewModel
    {
        public CustomerPayment customerPayment { get; set; }

        public List<ShopListsViewModel> shopList { get; set; }
    }
   public class CustomerPayment
    {
        public DateTime RequiredDate { get; set; }
        public DateTime PayDate { get; set; }
        public DateTime OrderDate { get; set; }
        public string BillName { get; set; }
        public string BillCity { get; set; }
        public string BillAddress { get; set; }
        public string BillPhone { get; set; }
        public int ShipperID { get; set; }

        [Required]
        public string ShipName { get; set; }

        [RegularExpression(@"^\d{4}\-?\d{3}\-?\d{3}$", ErrorMessage = "需為09xx-xxx-xxx格式")]
        public string ShipPhone { get; set; }

        [Required]
        public string ShipCity { get; set; }

        [Required]
        public string ShipAddress { get; set; }
        public string PayWay { get; set; }
        public decimal Freight { get; set; }
        public string CreditCardNo { get; set; }
        public string CreditCardDate { get; set; }
        public int? CreditCardCSC { get; set; }
        public string IdentityCard { get; set; }
    }
    
}