using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2019.Day14
{
    public class Day14 : IMDay
    {
        public const string Ore = "ORE";
        public const string Fuel = "FUEL";
        public const long MaxOre = 1000000000000L;

        private readonly List<Recipe> _orderedRecipes;

        public Day14()
        {
            _orderedRecipes = GetOrderedRecipes(File.ReadAllLines("Day14\\input.txt").Select(Recipe.Parse));
        }

        public string GetAnswerPart1()
        {
            return GetAmountOfOreRequired(1).ToString();
        }

        public string GetAnswerPart2()
        {
            var oreRequiredForOneFuelWithoutWaste = Math.Ceiling(_orderedRecipes.Single(r => r.Product == Fuel).AmountOfOreWithoutWaste(_orderedRecipes));
            var guesstimateOfFuelRequired = (long)(MaxOre / oreRequiredForOneFuelWithoutWaste);

            var oreRequired = GetAmountOfOreRequired(guesstimateOfFuelRequired);
            var fuelAddition = oreRequired > MaxOre ? -1 : 1;

            while ((oreRequired > MaxOre && fuelAddition == -1) || (oreRequired < MaxOre && fuelAddition == 1))
            {
                guesstimateOfFuelRequired += fuelAddition;
                oreRequired = GetAmountOfOreRequired(guesstimateOfFuelRequired);
            }

            if (oreRequired > MaxOre)
            {
                guesstimateOfFuelRequired--;
            }

            return guesstimateOfFuelRequired.ToString();
        }

        private long GetAmountOfOreRequired(long amountOfFuel)
        {
            var totalProducts = GetTotalProductAmounts(amountOfFuel);
            var recipesThatUseOre = _orderedRecipes.Where(r => r.Ingredients.ContainsKey(Ore));

            return recipesThatUseOre.Sum(r => (totalProducts[r.Product] / r.Amount) * r.Ingredients[Ore]);
        }

        private Dictionary<string, long> GetTotalProductAmounts(long amountOfFuel)
        {
            Dictionary<string, long> productAmounts = new();

            var newFuel = _orderedRecipes.First().CloneWithAmountMultiplier(amountOfFuel);
            var newRecipeOrder = new List<Recipe> { newFuel };
            newRecipeOrder.AddRange(_orderedRecipes.Skip(1));

            foreach (var recipe in newRecipeOrder)
            {
                long amount = recipe.Amount;
                if (productAmounts.ContainsKey(recipe.Product))
                {
                    if (productAmounts[recipe.Product] % recipe.Amount != 0)
                    {
                        productAmounts[recipe.Product] += recipe.Amount - (productAmounts[recipe.Product] % recipe.Amount);
                    }
                    amount = productAmounts[recipe.Product];
                }

                var amountOfRecipes = amount / recipe.Amount;

                foreach (var ingredient in recipe.Ingredients.Keys)
                {
                    var amountToAdd = amountOfRecipes * recipe.Ingredients[ingredient];
                    if (productAmounts.ContainsKey(ingredient))
                    {
                        productAmounts[ingredient] += amountToAdd;
                    }
                    else
                    {
                        productAmounts.Add(ingredient, amountToAdd);
                    }
                }
            }

            return productAmounts;
        }

        private static List<Recipe> GetOrderedRecipes(IEnumerable<Recipe> recipes)
        {
            List<Recipe> recipeOrder = new();
            var remaining = recipes.ToList();

            while (remaining.Any())
            {
                recipeOrder.AddRange(remaining.Where(r => remaining.All(or => !or.Ingredients.Keys.Contains(r.Product))));
                remaining = remaining.Where(r => !recipeOrder.Contains(r)).ToList();
            }

            return recipeOrder;
        }

        private class Recipe
        {
            public Dictionary<string, long> Ingredients { get; private init; }
            public string Product { get; private init; }
            public long Amount { get; private init; }

            public static Recipe Parse(string line)
            {
                Dictionary<string, long> ingredients = new();

                var parts = line.Split(" => ");
                foreach (var ingredient in parts[0].Split(","))
                {
                    var ingredientParts = ingredient.Trim().Split(" ");
                    ingredients.Add(ingredientParts[1], int.Parse(ingredientParts[0]));
                }

                var resultingIngredientParts = parts[1].Split(" ");

                return new Recipe
                {
                    Ingredients = ingredients,
                    Amount = int.Parse(resultingIngredientParts[0]),
                    Product = resultingIngredientParts[1]
                };
            }

            public double AmountOfOreWithoutWaste(List<Recipe> allRecipes)
            {
                var totalAmountOfOre = 0d;
                foreach (var product in Ingredients.Keys)
                {
                    totalAmountOfOre += (allRecipes.SingleOrDefault(r => r.Product == product)?.AmountOfOreWithoutWaste(allRecipes) ?? 1d) * Ingredients[product];
                }

                return totalAmountOfOre / Amount;
            }

            public Recipe CloneWithAmountMultiplier(long amountMultiplier)
            {
                return new Recipe
                {
                    Product = Product,
                    Amount = amountMultiplier * Amount,
                    Ingredients = Ingredients.ToDictionary(kvp => kvp.Key, kvp => kvp.Value * amountMultiplier)
                };
            }
        }
    }
}
