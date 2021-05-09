using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;



namespace MVC.Models
{
    public class Item 
    {
       [Key]
        public int Id { get; set; }

        [Required]
        public string Borrower { get; set; }

        [Required]
        public string Lender { get; set; }

        [Required]
        [DisplayName("Item Name")]
        public string ItemName { get; set; }
    }
}