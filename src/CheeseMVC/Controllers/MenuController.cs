﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CheeseMVC.ViewModels;
using CheeseMVC.Data;
using Microsoft.EntityFrameworkCore;
using CheeseMVC.Models;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CheeseMVC.Controllers
{
    public class MenuController : Controller
    {
        private readonly CheeseDbContext context;

        public MenuController(CheeseDbContext dbContext)
        {
            context = dbContext;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            IList<Menu> menus = context.Menus.Include(c => c.CheeseMenus).ToList();
            return View(menus);
        }

        public IActionResult Add()
        {
            AddMenuViewModel addMenuViewModel = new AddMenuViewModel();
            return View(addMenuViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddMenuViewModel addMenuViewModel)
        {
            if (ModelState.IsValid)
            {
                Menu newMenu = new Menu
                {
                    Name = addMenuViewModel.Name
                };

                context.Menus.Add(newMenu);
                context.SaveChanges();

                return Redirect("/Menu/ViewMenu/" + newMenu.ID);
            }

            return View(addMenuViewModel);
        }

        public IActionResult ViewMenu(int id)
        {
            Menu menu = context.Menus.Single(c => c.ID == id);

            List<CheeseMenu> items = context.CheeseMenus.Include(item => item.Cheese).Where(cm => cm.MenuID == id).ToList();

            ViewMenuViewModel viewMenuViewModel = new ViewMenuViewModel(menu, items);

            return View(viewMenuViewModel);
        }

        public IActionResult AddItem(int id)
        {
            Menu menu = context.Menus.Single(c => c.ID == id);
            IList<Cheese> cheeses = context.Cheeses.Include(c => c.Category).ToList();
            AddMenuItemViewModel addMenuItemViewModel = new AddMenuItemViewModel(menu, cheeses);
            return View(addMenuItemViewModel);
        }

        [HttpPost]
        public IActionResult AddItem(AddMenuItemViewModel addMenuItemViewModel)
        {
            if (ModelState.IsValid)
            {
                IList<CheeseMenu> existingItems = context.CheeseMenus
                    .Where(c => c.CheeseID == addMenuItemViewModel.CheeseID)
                    .Where(c => c.MenuID == addMenuItemViewModel.MenuID).ToList();

                if (existingItems.Count < 1)
                {
                    CheeseMenu newCheeseMenu = new CheeseMenu
                    {
                        MenuID = addMenuItemViewModel.MenuID,
                        Menu = context.Menus.Single(c => c.ID == addMenuItemViewModel.MenuID),
                        CheeseID = addMenuItemViewModel.CheeseID,
                        Cheese = context.Cheeses.Single(c => c.ID == addMenuItemViewModel.CheeseID)
                    };

                    context.CheeseMenus.Add(newCheeseMenu);
                    context.SaveChanges();

                    return Redirect("/Menu/ViewMenu/" + newCheeseMenu.MenuID);
                }

                return Redirect("/Menu/ViewMenu/" + addMenuItemViewModel.MenuID);
            }

            return View(addMenuItemViewModel);
        }
    }
}
