using CheeseMVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public List<SelectListItem> Categories { get; set; }

        public EditCheeseViewModel() : base() { }

        public EditCheeseViewModel(Cheese cheese, IEnumerable<CheeseCategory> categories)
        {
            Cheese = cheese;

            Categories = new List<SelectListItem>();

            foreach (CheeseCategory cat in categories)
            {
                Categories.Add(new SelectListItem
                {
                    Value = ((int)cat.ID).ToString(),
                    Text = cat.Name.ToString()
                });
            }
        }
    }
}
