using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;



namespace MVC.Models
{
    public class Income 
    {



        [Key]
        public int Id { get; set; }
      
        [DisplayName("Income")]
        [Required]
        public string IncomeName { get; set; }
        
       [Required]
       [Range(1,int.MaxValue,ErrorMessage ="Amount must be greater than 0")]
        public int Amount { get; set; }


        public Budget budget { get; set; }
    }
}