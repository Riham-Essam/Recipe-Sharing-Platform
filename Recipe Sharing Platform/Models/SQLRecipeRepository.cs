﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Recipe_Sharing_Platform.Models
{
    public class SQLRecipeRepository : IRecipeRepository
    {
        private readonly AppDbContext context;

        public SQLRecipeRepository(AppDbContext context)
        {
            this.context = context;
        }


        public Recipe AddRecipe(Recipe recipe)
        {
            context.Add(recipe);
            context.SaveChanges();
            return recipe;
        }

        public Recipe DeleteRecipe(int id)
        {
            var recipe = context.Recipes.Find(id);
            if (recipe != null)
            {
                context.Remove(recipe);
                context.SaveChanges();
            }

            return recipe;
        }

        public IEnumerable<Recipe> GetAllRecipes()
        {
            return context.Recipes;
        }

        public Recipe GetRecipe(string id)
        {
            return context.Recipes
                                 .Where(a => a.RecipeId == id)
                                 .Include(a => a.Comments)
                                 .FirstOrDefault();

        }

        public Recipe UpdateRecipe(Recipe recipe)
        {
            var recipeToBeUpdated = context.Recipes.Attach(recipe);
            recipeToBeUpdated.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return recipe;
        }
    }
}
