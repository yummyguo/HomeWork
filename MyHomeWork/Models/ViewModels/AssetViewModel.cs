using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyHomeWork.Models.ViewModels
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    public class AssetViewModel
    {
        [Key] 
        [DisplayName("類別")]
        [Required(ErrorMessage = "請選取類別")]
        public int Type { get; set; }
        [DisplayName("金額")]
        [Required(ErrorMessage = "請填寫金額")]
        public int Amt { get; set; }
        [DisplayName("日期")]
        [Required(ErrorMessage = "請填寫日期")]
        public string Date { get; set; }

        [DisplayName("備註")]
        [Required(ErrorMessage = "請填寫備註")]
        public string Remark { get; set; }

        public bool IsEdit { get; set; }
    }
}