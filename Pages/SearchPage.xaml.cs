using SmartCooking.Helpers;  // Add this line to reference DatabaseHelper
using SmartCooking.Models;  // Ensure the Models namespace is referenced
using Microsoft.Maui.Controls;
using System;
using System.Linq;

#if ANDROID
using SmartCooking.Helpers; // Needed if BluetoothService is inside this namespace
using System.Collections.Generic;
#endif

namespace SmartCooking.Pages
{
    public partial class SearchPage : ContentPage
    {
        private DatabaseHelper _databaseHelper;
        private RecipeDB _selectedRecipe;
        private bool _isWaitingForMass = false;
#if ANDROID
        private List<double> _massReadings = new List<double>();
        private const int RequiredReadings = 5;
        private const double MinMassThreshold = 1.0;
#endif


        public SearchPage()
        {
            InitializeComponent();
            _databaseHelper = new DatabaseHelper();

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

        // Event handler for Select Recipe button
        private async void OnselectrecipeClicked(object sender, EventArgs e)
        {
            string recipeName = recipeSearchBar.Text;
            var recipe = _databaseHelper.GetRecipeByName(recipeName);

            if (recipe == null)
            {
                await DisplayAlert("Error", "Recipe not found in the database!", "OK");
                return;
            }

#if ANDROID
            if (!BluetoothService.Instance.IsConnected)
            {
                await DisplayAlert("Bluetooth Not Connected", "Please connect to the device from Settings first.", "OK");
                return;
            }

            _selectedRecipe = recipe;
            _massReadings.Clear();
            _isWaitingForMass = true;


            BluetoothService.Instance.OnDataReceived += OnMassReceived;

            await DisplayAlert("Waiting for Mass", "Waiting for mass...", "OK");
#endif
        }

#if ANDROID
        private async void OnMassReceived(string data)
{
    if (!_isWaitingForMass) return;

    try
    {
        int wIndex = data.IndexOf("W=");
        if (wIndex == -1) return; // No "W=" found

        // Extract substring after "W="
        string weightString = data.Substring(wIndex + 2);

        // Optional: trim spaces or newline characters
        weightString = weightString.Trim();

        if (double.TryParse(weightString, out double receivedMass))
        {
            _massReadings.Add(receivedMass);

            if (_massReadings.Count >= RequiredReadings)
            {
                _isWaitingForMass = false;
                BluetoothService.Instance.OnDataReceived -= OnMassReceived;

                double averageMass = _massReadings.Average();

                if (averageMass > MinMassThreshold)
                {
                    BluetoothService.Instance.SendCommandToDevice("0");
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await Navigation.PushAsync(new ShowDetails(_selectedRecipe, averageMass));
                    });
                }
                else
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await DisplayAlert("Mass Not Detected", "Detected mass is too low. Please try again.", "OK");
                    });
                }
            }
        }
    }
    catch (Exception)
    {
        // Ignore malformed data and wait for next reading
    }
}

#endif
    }
}