using SQLite;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SmartCooking.Helpers
{
    public class DatabaseHelper
    {
        private SQLiteConnection _connection;

        public DatabaseHelper()
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "recipes.db3");
            _connection = new SQLiteConnection(dbPath);
            _connection.CreateTable<Models.RecipeDB>(); // Create table if it doesn't exist
        }

        // Save a recipe to the database
        public int SaveRecipe(Models.RecipeDB recipe)
        {
            return _connection.Insert(recipe); // Insert the recipe into the database
        }

        // Get all recipes from the database
        public List<Models.RecipeDB> GetAllRecipes()
        {
            return _connection.Table<Models.RecipeDB>().ToList(); // Return a list of all recipes
        }

        // Get a recipe by its name
        public Models.RecipeDB GetRecipeByName(string name)
        {
            return _connection.Table<Models.RecipeDB>().FirstOrDefault(r => r.RecipeName == name);
        }

        // Delete a recipe by its ID
        public int DeleteRecipe(int id)
        {
            return _connection.Delete<Models.RecipeDB>(id); // Delete the recipe by its ID
        }

        // Update an existing recipe
        public int UpdateRecipe(Models.RecipeDB recipe)
        {
            return _connection.Update(recipe); // Update the recipe
        }
    }
}

