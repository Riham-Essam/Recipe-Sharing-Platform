using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recipe_Sharing_Platform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recipe_Sharing_Platform.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRecipeRepository recipeRepository;

        public HomeController(IRecipeRepository recipeRepository)
        {
            this.recipeRepository = recipeRepository;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            IEnumerable<Recipe> recipe = recipeRepository.GetAllRecipes();

            return View(recipe);
        }

    }
}
