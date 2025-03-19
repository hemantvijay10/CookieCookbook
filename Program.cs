using System.Text.RegularExpressions;

namespace CookieCookbook
{
    enum IngredientID
    {
        WHEAT_FLOUR = 1,
        COCONUT_FLOUR,
        BUTTER,
        CHOCOLATE,
        SUGAR,
        CARDAMOM,
        CINNAMON,
        COCOA_POWDER,
        SAVE_RECIPE
    }

    internal class Program
    {
        private const string PatternStr = "*****";

        // The available ingredients are hard-coded as per the requirements.
        public static List<Ingredients> AvailableIngredients = new List<Ingredients>
        {
            new Ingredients(1, "Wheat flour", "Take wheat flour. Sieve. Add to other ingredients."),
            new Ingredients(2, "Coconut flour", "Take coconut flour. Sieve. Add to other ingredients."),
            new Ingredients(3, "Butter", "Take butter. Melt on low heat. Add to other ingredients."),
            new Ingredients(4, "Chocolate", "Take chocolate. Melt in a water bath. Add to other ingredients."),
            new Ingredients(5, "Sugar", "Take sugar. Add to other ingredients."),
            new Ingredients(6, "Cardamom", "Take cardamom. Take half a teaspoon. Add to other ingredients."),
            new Ingredients(7, "Cinnamon", "Take cinnamon. Take half a teaspoon. Add to other ingredients."),
            new Ingredients(8, "Cocoa powder", "Take cocoa powder. Add to other ingredients.")
        };

        static void PrintIngredients()
        {
            Console.WriteLine("Select the any of the following ingredients (enter number 1 to 9): ");
            Console.WriteLine("1. Wheat flour");
            Console.WriteLine("2. Coconut flour");
            Console.WriteLine("3. Butter");
            Console.WriteLine("4. Chocolate");
            Console.WriteLine("5. Sugar");
            Console.WriteLine("6. Cardamom");
            Console.WriteLine("7. Cinnamon");
            Console.WriteLine("8. Cocoa powder");
            Console.WriteLine("9. Save recipe");
        }

        static void Main()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            ushort recipeCount = 0;
            string CookbookFileName = $"{AppDomain.CurrentDomain.BaseDirectory}Cookbook.txt";

            if (!File.Exists(CookbookFileName))
            {
                File.Create(CookbookFileName).Dispose();
            }

            string recipeList = File.ReadAllText(CookbookFileName);

            if(recipeList.Length == 0)
            {
                Console.WriteLine("There are no recipes at the moment.");
            }
            else
            {
                Console.WriteLine("List of available recipes\n" + recipeList);
                recipeCount = (ushort)(Regex.Matches(recipeList, Regex.Escape(PatternStr)).Count / 2);
            }

            while (true)
            {
                Console.WriteLine("Would you like to create a new recipe? [y/n]");
                string? inputStr = Console.ReadLine();
                if(inputStr == null)
                {
                    Console.WriteLine("Invalid input.");
                    continue;
                }
                else if (inputStr.ToLower().Equals("y"))
                {
                    recipeCount++;
                    ushort stepCount = 0;
                    string finalRecipe = $"{PatternStr} {recipeCount} {PatternStr}\n";
                    while (true)
                    {
                        PrintIngredients();
                        byte ingId = 0;
                        if (byte.TryParse(Console.ReadLine(), out ingId))
                        {
                            if ((ingId == 0) || (ingId > (byte)IngredientID.SAVE_RECIPE))
                            {
                                Console.WriteLine("Invalid input.");
                            }
                            else if (ingId == (byte)IngredientID.SAVE_RECIPE)
                            {
                                Console.WriteLine("Recipe to store: \n" + finalRecipe);
                                File.AppendAllText(CookbookFileName, finalRecipe);
                                Console.WriteLine("Recipe is saved!");
                                break;
                            }
                            else
                            {
                                stepCount++;
                                finalRecipe += $"{stepCount}. " + AvailableIngredients[ingId - 1].PreparationInstructions + "\n";
                                Console.WriteLine("Ingredient added!\n");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid input.");
                        }
                    } //while(true)
                }
                else if (inputStr.ToLower().Equals("n"))
                {
                    Console.WriteLine("Exiting...");
                    Thread.Sleep(2000);
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("Invalid input.");
                    continue;
                }
            }
        }
    }

    public class Ingredients
    {
        public byte ID { get; set; } = 0;
        public string Name { get; set; } = "";
        public string PreparationInstructions { get; set; } = "";

        public Ingredients(byte id, string name, string prepInstructions) 
        {
            ID = id;
            Name = name;
            PreparationInstructions = prepInstructions;
        }
    }
}