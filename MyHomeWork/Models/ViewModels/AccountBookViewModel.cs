namespace MyHomeWork.Models
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class AccountBookViewModel
    {
        [DisplayName("次序")]
        public string Id { get; set; }
        [DisplayName("種類")]
        public int? Categoryyy { get; set; }
        [DisplayName("數目")]
        public int? Amounttt { get; set; }
        [DisplayName("日期")]
        public DateTime? Dateee { get; set; }
        [DisplayName("註記")]
        [Required]
        [StringLength(500)]
        public string Remarkkk { get; set; } 

        public int PageIndex { get; set; }
    }
}