using SQLite;

namespace SmartCooking.Models
{
    public class RecipeDB
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; } // Auto-increment primary key for SQLite

        public string RecipeName { get; set; } // Recipe Name
        public string Category { get; set; } // Food Category
        public string Spice1 { get; set; } // Spice 1
        public double Spice1Quantity { get; set; } // Spice 1 Quantity in teaspoons
        public string Spice2 { get; set; } // Spice 2
        public double Spice2Quantity { get; set; } // Spice 2 Quantity in teaspoons
        public string Spice3 { get; set; } // Spice 3
        public double Spice3Quantity { get; set; } // Spice 3 Quantity in teaspoons
        public string Ingredient1 { get; set; } // Ingredient 1
        public double Ingredient1Quantity { get; set; } // Ingredient 1 Quantity in grams
        public string Ingredient2 { get; set; } // Ingredient 2
        public double Ingredient2Quantity { get; set; } // Ingredient 2 Quantity in grams
        public string Ingredient3 { get; set; } // Ingredient 3
        public double Ingredient3Quantity { get; set; } // Ingredient 3 Quantity in grams
        public string Ingredient4 { get; set; } // Ingredient 4
        public double Ingredient4Quantity { get; set; } // Ingredient 4 Quantity in grams
        public double WaterAmount { get; set; } // Water Amount in ml
        public double CoconutMilkAmount { get; set; } // Coconut Milk Amount in ml
        public int DurationHours { get; set; } // Duration Hours
        public int DurationMinutes { get; set; } // Duration Minutes
    }
}

