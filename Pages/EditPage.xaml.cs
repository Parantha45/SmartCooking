using SmartCooking.Helpers;
using SmartCooking.Models;
using Microsoft.Maui.Controls;
using System;

namespace SmartCooking.Pages
{
    public partial class EditPage : ContentPage
    {
        private RecipeDB _recipe;
        private DatabaseHelper _databaseHelper;

        public EditPage(RecipeDB recipe)
        {
            InitializeComponent();
            _recipe = recipe;  // Store the recipe passed from ModifyPage
            _databaseHelper = new DatabaseHelper();

            // Populate the fields with the existing recipe details
            PopulateFields();
        }

        private void PopulateFields()
        {
            recipeNameEntry.Text = _recipe.RecipeName;
            categoryPicker.SelectedItem = _recipe.Category;
            spice1Picker.SelectedItem = _recipe.Spice1;
            spice1QuantityPicker.SelectedItem = _recipe.Spice1Quantity;
            spice2Picker.SelectedItem = _recipe.Spice2;
            spice2QuantityPicker.SelectedItem = _recipe.Spice2Quantity;
            spice3Picker.SelectedItem = _recipe.Spice3;
            spice3QuantityPicker.SelectedItem = _recipe.Spice3Quantity;
            ingredient1Entry.Text = _recipe.Ingredient1;
            ingredient1QuantityPicker.SelectedItem = _recipe.Ingredient1Quantity;
            ingredient2Entry.Text = _recipe.Ingredient2;
            ingredient2QuantityPicker.SelectedItem = _recipe.Ingredient2Quantity;
            ingredient3Entry.Text = _recipe.Ingredient3;
            ingredient3QuantityPicker.SelectedItem = _recipe.Ingredient3Quantity;
            ingredient4Entry.Text = _recipe.Ingredient4;
            ingredient4QuantityPicker.SelectedItem = _recipe.Ingredient4Quantity;
            waterAmountPicker.SelectedItem = _recipe.WaterAmount;
            coconutMilkAmountPicker.SelectedItem = _recipe.CoconutMilkAmount;
            durationHrsPicker.SelectedItem = _recipe.DurationHours;
            durationMinsPicker.SelectedItem = _recipe.DurationMinutes;
        }

        // Event handler for the Save button
        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            // Update the recipe with the new data from the UI
            _recipe.RecipeName = recipeNameEntry.Text;
            _recipe.Category = categoryPicker.SelectedItem?.ToString();
            _recipe.Spice1 = spice1Picker.SelectedItem?.ToString();
            _recipe.Spice1Quantity = Convert.ToDouble(spice1QuantityPicker.SelectedItem?.ToString());
            _recipe.Spice2 = spice2Picker.SelectedItem?.ToString();
            _recipe.Spice2Quantity = Convert.ToDouble(spice2QuantityPicker.SelectedItem?.ToString());
            _recipe.Spice3 = spice3Picker.SelectedItem?.ToString();
            _recipe.Spice3Quantity = Convert.ToDouble(spice3QuantityPicker.SelectedItem?.ToString());
            _recipe.Ingredient1 = ingredient1Entry.Text;
            _recipe.Ingredient1Quantity = Convert.ToDouble(ingredient1QuantityPicker.SelectedItem?.ToString());
            _recipe.Ingredient2 = ingredient2Entry.Text;
            _recipe.Ingredient2Quantity = Convert.ToDouble(ingredient2QuantityPicker.SelectedItem?.ToString());
            _recipe.Ingredient3 = ingredient3Entry.Text;
            _recipe.Ingredient3Quantity = Convert.ToDouble(ingredient3QuantityPicker.SelectedItem?.ToString());
            _recipe.Ingredient4 = ingredient4Entry.Text;
            _recipe.Ingredient4Quantity = Convert.ToDouble(ingredient4QuantityPicker.SelectedItem?.ToString());
            _recipe.WaterAmount = Convert.ToDouble(waterAmountPicker.SelectedItem?.ToString());
            _recipe.CoconutMilkAmount = Convert.ToDouble(coconutMilkAmountPicker.SelectedItem?.ToString());
            _recipe.DurationHours = Convert.ToInt32(durationHrsPicker.SelectedItem?.ToString());
            _recipe.DurationMinutes = Convert.ToInt32(durationMinsPicker.SelectedItem?.ToString());

            // Save the updated recipe to the database
            _databaseHelper.UpdateRecipe(_recipe);

            // Navigate back to the HomePage after saving
            await Navigation.PushAsync(new HomePage());
        }

        // Event handler for the Delete button
        private async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            // Confirm the deletion
            var result = await DisplayAlert("Delete", "Are you sure you want to delete this recipe?", "Yes", "No");

            if (result)
            {
                // Delete the recipe from the database
                _databaseHelper.DeleteRecipe(_recipe.Id);

                // Navigate back to the HomePage after deletion
                await Navigation.PushAsync(new HomePage());
            }
        }

        // Event handler for the Reset button
        private void OnResetButtonClicked(object sender, EventArgs e)
        {
            // Reset all fields to the original values (same as PopulateFields)
            PopulateFields();
        }
    }
}
