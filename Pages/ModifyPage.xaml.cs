using SmartCooking.Helpers;  // Add this line to reference DatabaseHelper
using SmartCooking.Models;  // Ensure the Models namespace is referenced
using Microsoft.Maui.Controls;
using System;
using System.Linq;

namespace SmartCooking.Pages
{
    public partial class ModifyPage : ContentPage
    {
        private DatabaseHelper _databaseHelper;

        public ModifyPage()
        {
            InitializeComponent();
            _databaseHelper = new DatabaseHelper();
        }
        private async void OnAddButtonClicked(object sender, EventArgs e)
        {
            // Navigate to AddRecipe page
            await Navigation.PushAsync(new AddRecipe());
        }

        // Event handler for SearchBar TextChanged (suggest recipes)
        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = e.NewTextValue?.ToLower();
            string selectedCategory = categoryPicker.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(searchText) && !string.IsNullOrEmpty(selectedCategory))
            {
                // Get recipe suggestions from the database that match the category and starting letters
                var suggestions = _databaseHelper.GetAllRecipes()
                    .Where(r => r.RecipeName.ToLower().StartsWith(searchText) && r.Category == selectedCategory)
                    .ToList();

                recipeSuggestionsList.ItemsSource = suggestions;
                recipeSuggestionsList.IsVisible = suggestions.Any();
            }
            else
            {
                recipeSuggestionsList.IsVisible = false;
            }
        }

        // Event handler when a recipe is selected from the suggestions list
        private void OnRecipeSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedRecipe = e.SelectedItem as Models.RecipeDB;
            if (selectedRecipe != null)
            {
                recipeSearchBar.Text = selectedRecipe.RecipeName; // Pre-fill the recipe name field
            }
        }

        // Event handler for the Edit button
        private async void OnEditButtonClicked(object sender, EventArgs e)
        {
            string recipeName = recipeSearchBar.Text;

            // Validate if the recipe name is empty or not found in the database
            if (string.IsNullOrEmpty(recipeName))
            {
                await DisplayAlert("Error", "Please enter a recipe name to search for.", "OK");
                return;
            }

            // Check if the recipe exists in the database
            var recipe = _databaseHelper.GetRecipeByName(recipeName);

            if (recipe == null)
            {
                // If recipe doesn't exist, show an error message
                await DisplayAlert("Error", "Recipe not found in the database.", "OK");
            }
            else
            {
                // Navigate to the EditRecipePage with the found recipe
                await Navigation.PushAsync(new EditPage(recipe));
            }
        }
    }
}
