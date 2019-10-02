using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Team7MVC.Models
{
    public class Orders
    {
        [Display(Name = "訂單ID")]
        public int OrderID { get; set; }
        [Display(Name = "客戶ID")]
        public int CustomerID { get; set; }
        [Display(Name = "訂單日期")]
        public DateTime OrderDate { get; set; }
        [Display(Name = "送達日期")]
        [DataType(DataType.Date)]
        public DateTime RequiredDate { get; set; }
        [Display(Name = "發貨日期")]
        public DateTime ShippedDate { get; set; }
        [Display(Name = "發貨ID")]
        public int ShipperID { get; set; }
        [Display(Name = "訂購人")]
        public string ShipName { get; set; }
        [Display(Name = "訂購人地址")]
        public string ShipAddress { get; set; }
        [Display(Name = "運費")]
        public decimal Freight { get; set; }
        [Display(Name = "付款方式")]
        public string PayWay { get; set; }
        [Display(Name = "付款日期")]
        public DateTime PayDate { get; set; }

        [Display(Name = "訂購人電話")]
        public string ShipPhone { get; set; }
        [Display(Name = "訂購人城市")]
        public string ShipCity { get; set; }
        [Display(Name = "收件人地址")]
        public string BillAddress { get; set; }
        [Display(Name = "收件人名字")]
        public string BillName { get; set; }
        [Display(Name = "收件人城市")]
        public string BillCity { get; set; }
        [Display(Name = "收件人電話")]
        public string BillPhone { get; set; }
        [Display(Name = "訂單狀態")]
        public string Status { get; set; }
        [Display(Name = "訂單總金額")]
        public string TotalAmount { get; set; }
    }
}