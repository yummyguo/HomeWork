namespace MyHomeWork.Models
{
    using System;  
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
  

    [Table("AccountBook")]///���w�s����Ʈw����ƪ�W��
    public partial class AccountBook
    { 
        public Guid Id { get; set; }

        public int Categoryyy { get; set; }

        public int Amounttt { get; set; }

        public DateTime Dateee { get; set; }

        [Required]
        public string Remarkkk { get; set; }
    }
}
