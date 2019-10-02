using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Team7MVC.ViewModels
{
    public class AdminMessagesViewModel
    {
        [Display(Name = "訊息編號")]
        public int Id { get; set; }
        [Display(Name = "姓名")]
        public String Name { get; set; }
        [Display(Name = "問題種類")]
        public String QuestionCategory { get; set; }
        [Display(Name = "意見內容")]
        public String Comments { get; set; }
        [Display(Name = "訊息日期")]
        public DateTime Datetime { get; set; }
        [Display(Name = "狀態")]
        public String Status { get; set; }
        
    }
}