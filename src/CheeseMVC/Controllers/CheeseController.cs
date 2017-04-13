using Microsoft.AspNetCore.Mvc;
using CheeseMVC.Models;
using System.Collections.Generic;
using CheeseMVC.ViewModels;
using CheeseMVC.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CheeseMVC.Controllers
{
    public class CheeseController : Controller
    {
        private readonly CheeseDbContext context;

        public CheeseController(CheeseDbContext dbContext)
        {
            this.context = dbContext;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            IList<Cheese> cheeses = context.Cheeses.Include(c => c.Category).ToList();
            return View(cheeses);
        }

        public IActionResult Add()
        {
            AddCheeseViewModel addCheeseViewModel = new AddCheeseViewModel(context.Categories.ToList());
            return View(addCheeseViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddCheeseViewModel addCheeseViewModel)
        {
            if (ModelState.IsValid)
            {
                CheeseCategory newCheeseCategory = context.Categories.Single(c => c.ID == addCheeseViewModel.CategoryID);

                // Add the new cheese to my existing cheeses
                Cheese newCheese = new Cheese
                {
                    Name = addCheeseViewModel.Name,
                    Description = addCheeseViewModel.Description,
                    Category = newCheeseCategory
                };

                context.Cheeses.Add(newCheese);
                context.SaveChanges();

                return Redirect("/Cheese");
            }

            return View(addCheeseViewModel);
        }

        public IActionResult Remove()
        {
            ViewBag.title = "Remove Cheeses";
            ViewBag.cheeses = context.Cheeses.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Remove(int[] cheeseIds)
        {
            foreach (int cheeseId in cheeseIds)
            {
                Cheese theCheese = context.Cheeses.Single(c => c.ID == cheeseId);
                context.Cheeses.Remove(theCheese);
            }

            context.SaveChanges();

            return Redirect("/");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Cheese cheese = context.Cheeses.Single(c => c.ID == id);
            
            EditCheeseViewModel editCheeseViewModel = new EditCheeseViewModel(cheese, context.Categories.ToList());
            return View(editCheeseViewModel);
        }

        [HttpPost]
        public IActionResult EditPost(int cheeseId, EditCheeseViewModel editCheeseViewModel)
        {
            Cheese editedCheese = context.Cheeses.Single(c => c.ID == cheeseId);

            editedCheese.Name = editCheeseViewModel.Cheese.Name;
            editedCheese.Description = editCheeseViewModel.Cheese.Description;

            CheeseCategory newCheeseCategory = context.Categories.Single(c => c.ID == editCheeseViewModel.Cheese.CategoryID);
            editedCheese.Category = newCheeseCategory;

            context.Cheeses.Update(editedCheese);
            context.SaveChanges();

            return Redirect("Index");
        }
    }
}
