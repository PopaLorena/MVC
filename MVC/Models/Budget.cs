using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace MVC.Models
{
    public class Budget  
    {
        [Key]
        public int Id { get; set; }

      
        public int Amount { get; set; }
    }
}