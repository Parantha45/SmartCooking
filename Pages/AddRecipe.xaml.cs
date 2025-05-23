using SmartCooking.Helpers;
using SmartCooking.Models;
using Microsoft.Maui.Controls;
using System;

namespace SmartCooking.Pages
{
    public partial class AddRecipe : ContentPage
    {
        private DatabaseHelper _databaseHelper;

        public AddRecipe()
        {
            InitializeComponent();
            _databaseHelper = new DatabaseHelper();
        }

        // Event handler for Save Button
        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            // Validate input fields (check if any required field is null or empty)
            if (string.IsNullOrEmpty(recipeNameEntry.Text) ||
                categoryPicker.SelectedIndex == -1 ||
                spice1Picker.SelectedIndex == -1 ||
                spice1QuantityPicker.SelectedIndex == -1 ||
                spice2Picker.SelectedIndex == -1 ||
                spice2QuantityPicker.SelectedIndex == -1 ||
                ingredient1Entry.Text == string.Empty ||
                ingredient1QuantityPicker.SelectedIndex == -1 ||
                ingredient2Entry.Text == string.Empty ||
                ingredient2QuantityPicker.SelectedIndex == -1 ||
                Hrs.SelectedIndex == -1 ||
                Mins.SelectedIndex == -1)
            {
                // Show an error if any required field is empty
                await DisplayAlert("Error", "Please fill all required fields!", "OK");
                return;
            }

            // Create a new Recipe object
            var newRecipe = new RecipeDB
            {
                RecipeName = recipeNameEntry.Text,
                Category = categoryPicker.SelectedItem?.ToString(), // Add null check for SelectedItem
                Spice1 = spice1Picker.SelectedItem?.ToString(),
                Spice1Quantity = Convert.ToDouble(spice1QuantityPicker.SelectedItem),
                Spice2 = spice2Picker.SelectedItem?.ToString(),
                Spice2Quantity = Convert.ToDouble(spice2QuantityPicker.SelectedItem),
                Spice3 = spice3Picker.SelectedItem?.ToString(),
                Spice3Quantity = Convert.ToDouble(spice3QuantityPicker.SelectedItem),
                Ingredient1 = ingredient1Entry.Text,
                Ingredient1Quantity = Convert.ToDouble(ingredient1QuantityPicker.SelectedItem),
                Ingredient2 = ingredient2Entry.Text,
                Ingredient2Quantity = Convert.ToDouble(ingredient2QuantityPicker.SelectedItem),
                Ingredient3 = ingredient3Entry.Text,
                Ingredient3Quantity = Convert.ToDouble(ingredient3QuantityPicker.SelectedItem),
                Ingredient4 = ingredient4Entry.Text,
                Ingredient4Quantity = Convert.ToDouble(ingredient4QuantityPicker.SelectedItem),
                WaterAmount = Convert.ToDouble(waterAmountPicker.SelectedItem),
                CoconutMilkAmount = Convert.ToDouble(coconutMilkPicker.SelectedItem),
                DurationHours = Convert.ToInt32(Hrs.SelectedItem),
                DurationMinutes = Convert.ToInt32(Mins.SelectedItem)
            };

            // Check if the recipe already exists
            var existingRecipe = _databaseHelper.GetRecipeByName(newRecipe.RecipeName);
            if (existingRecipe != null)
            {
                await DisplayAlert("Error", "Recipe name already exists!", "OK");
                return;
            }

            // Save the new recipe
            _databaseHelper.SaveRecipe(newRecipe);

            // Now check if the recipe has been saved
            var savedRecipe = _databaseHelper.GetRecipeByName(newRecipe.RecipeName);
            if (savedRecipe != null)
            {
                // If the recipe is saved successfully, navigate to the Edit page
                await DisplayAlert("Success", "Recipe saved successfully!", "OK");
                await Navigation.PushAsync(new Edit());
            }
            else
            {
                // If the recipe could not be found after saving, show an error
                await DisplayAlert("Error", "Recipe saving failed, please try again.", "OK");
            }
        }


        // Event handler for Reset Button (Clear all fields)
        private void OnResetButtonClicked(object sender, EventArgs e)
        {
            recipeNameEntry.Text = string.Empty;
            categoryPicker.SelectedIndex = -1;
            spice1Picker.SelectedIndex = -1;
            spice1QuantityPicker.SelectedIndex = -1;
            spice2Picker.SelectedIndex = -1;
            spice2QuantityPicker.SelectedIndex = -1;
            spice3Picker.SelectedIndex = -1;
            spice3QuantityPicker.SelectedIndex = -1;
            ingredient1Entry.Text = string.Empty;
            ingredient1QuantityPicker.SelectedIndex = -1;
            ingredient2Entry.Text = string.Empty;
            ingredient2QuantityPicker.SelectedIndex = -1;
            ingredient3Entry.Text = string.Empty;
            ingredient3QuantityPicker.SelectedIndex = -1;
            ingredient4Entry.Text = string.Empty;
            ingredient4QuantityPicker.SelectedIndex = -1;
            waterAmountPicker.SelectedIndex = -1;
            coconutMilkPicker.SelectedIndex = -1;
            Hrs.SelectedIndex = -1;
            Mins.SelectedIndex = -1;
        }
    }
}

