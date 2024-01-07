using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Recipe_Sharing_Platform.Models;
using Recipe_Sharing_Platform.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Recipe_Sharing_Platform.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRecipeRepository recipeRepository;

        public IHostingEnvironment HostingEnv;

        public HomeController(IRecipeRepository recipeRepository, IHostingEnvironment hostingEnv)
        {
            this.recipeRepository = recipeRepository;
            HostingEnv = hostingEnv;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            IEnumerable<Recipe> recipe = recipeRepository.GetAllRecipes();

            return View(recipe);
        }

        [HttpGet]
        public IActionResult AddRecipe()
        {
            return View();
        }

        private string ProcessUploadedFile(AddRecipeViewModel model)
        {
            string uniqueFileName = null;
            if (model.Image != null)
            {

                string uploadsFolder = Path.Combine(HostingEnv.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Image.CopyTo(fileStream);
                }


            }

            return uniqueFileName;
        }


        [HttpPost]
        public IActionResult AddRecipe(AddRecipeViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(model);
                var recipe = new Recipe
                {
                    Title = model.Title,
                    Description = model.Description,
                    Ingredients = model.Ingredients,
                    Steps = model.Steps,
                    Photo = uniqueFileName
                };

                recipeRepository.AddRecipe(recipe);

                return RedirectToAction("Index");

            }

            return View(model);
        }


    }
}
