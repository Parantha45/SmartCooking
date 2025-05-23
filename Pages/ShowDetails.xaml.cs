using Microsoft.Maui.Controls;
using SmartCooking.Models;
using System;
using System.Threading.Tasks;

#if ANDROID //if the platform is Android
using SmartCooking.Helpers; // BluetoothService - Includes your custom BluetoothService helper class for Bluetooth communications
using Microsoft.Maui.Dispatching; // Lets you run code on the main UI thread via MainThread
#endif


namespace SmartCooking.Pages
{
    public partial class ShowDetails : ContentPage
    {
        private RecipeDB _recipe;  // Store the recipe data
        private double _mass;


        public ShowDetails(RecipeDB recipe, double mass)
        {
            InitializeComponent();
            _recipe = recipe;
            _mass = mass;
            UpdateQuantities(mass);
        }

        private void UpdateQuantities(double mass)
        {
            double factor = mass / 1000.0;


            spice1Label.Text = $"{_recipe.Spice1} - {_recipe.Spice1Quantity * factor:F2} tsp";
            spice2Label.Text = $"{_recipe.Spice2} - {_recipe.Spice2Quantity * factor:F2} tsp";
            spice3Label.Text = $"{_recipe.Spice3} - {_recipe.Spice3Quantity * factor:F2} tsp";

            ingredient1Label.Text = $"{_recipe.Ingredient1} - {_recipe.Ingredient1Quantity * factor:F2} g";
            ingredient2Label.Text = $"{_recipe.Ingredient2} - {_recipe.Ingredient2Quantity * factor:F2} g";
            ingredient3Label.Text = $"{_recipe.Ingredient3} - {_recipe.Ingredient3Quantity * factor:F2} g";
            ingredient4Label.Text = $"{_recipe.Ingredient4} - {_recipe.Ingredient4Quantity * factor:F2} g";

            waterAmountLabel.Text = $"{_recipe.WaterAmount * factor:F2} ml";
            coconutMilkAmountLabel.Text = $"{_recipe.CoconutMilkAmount * factor:F2} ml";
            durationHrsLabel.Text = $"{_recipe.DurationHours} hrs";
            durationMinsLabel.Text = $" mins";
        }

#if ANDROID
           
        private async void OnnextClicked(object sender, EventArgs e)
        {
            double factor = _mass / 1000.0;
            double spice1Amount = _recipe.Spice1Quantity * factor;
            double spice2Amount = _recipe.Spice2Quantity * factor;
            double spice3Amount = _recipe.Spice3Quantity * factor;
            double In1Amount = _recipe.Ingredient1Quantity * factor;
            double In2Amount = _recipe.Ingredient2Quantity * factor;
            double In3Amount = _recipe.Ingredient3Quantity * factor;
            double In4Amount = _recipe.Ingredient4Quantity * factor;
            double water = _recipe.WaterAmount * factor;
            double cocomilk = _recipe.CoconutMilkAmount * factor;

            // Show the popMSg to Put the Spices and Ingredients .
            await DisplayAlert(
                "Place Ingredients",
                "Please store the spices and other ingredients in their correct containers before continuing.",
                "OK");


            // Format message with spice amounts and send to Arduino
            string spiceMessage = $"S1:{spice1Amount:0.00};S2:{spice2Amount:0.00};S3:{spice3Amount:0.00},I1:{In1Amount:0.00},I2:{In2Amount:0.00},I3:{In3Amount:0.00},I4:{In4Amount:0.00},W:{water:0.00},C:{cocomilk:0.00},T:{_recipe.DurationMinutes:0.00}";
            BluetoothService.Instance.SendCommandToDevice(spiceMessage);


            // Go to ShowProcess page with recipe and spice amounts
            await Navigation.PushAsync(new ShowProcess(_recipe,_recipe.DurationHours,
                    _recipe.DurationMinutes));
        }
#endif

    } // end of ShowDetails class
} // end of namespace