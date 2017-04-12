using CheeseMVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CheeseMVC.ViewModels
{
    public class EditCheeseViewModel
    {
        [Required]
        public int CheeseID { get; set; }

        public Cheese Cheese { get; set; }

        
    }
}
