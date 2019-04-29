using UnityEngine;

namespace Bitzawolf
{
    public enum Ingredient
    {
        None,
        Red,
        Green,
        Blue
    }

    public static class IngredientMethods
    {
        public static string GetSymbol(this Ingredient ingredient)
        {
            switch (ingredient)
            {
                default:
                case Ingredient.None:
                    return "";
                case Ingredient.Red:
                    return "R";
                case Ingredient.Green:
                    return "G";
                case Ingredient.Blue:
                    return "B";
            }
        }

        public static Color GetColor(this Ingredient ingredient)
        {
            switch (ingredient)
            {
                default:
                case Ingredient.None:
                    return Color.white;
                case Ingredient.Red:
                    return Color.red;
                case Ingredient.Green:
                    return Color.green;
                case Ingredient.Blue:
                    return Color.blue;
            }
        }
    }
}
