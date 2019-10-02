using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Team7MVC.ViewModels
{
    public class ShopListsViewModel
    {
        [Display(Name = "商品編號")]
        public int ProductId { get; set; }
        [Display(Name = "商品圖片")]
        public string Picture { get; set; }
        [Display(Name = "商品名稱")]
        public string ProductName { get; set; }
        [Display(Name = "年份")]
        public int Year { get; set; }
        [Display(Name = "產地")]
        public string Origin { get; set; }
        [Display(Name = "單價")]
        public decimal Price { get; set; }
        [Display(Name = "數量")]
        public int Quantity { get; set; }
        [Display(Name = "商品庫存量")]
        public int Stock { get; set; }
        [Display(Name = "小計")]
        public decimal TotalCost { get; set; }
    }
}