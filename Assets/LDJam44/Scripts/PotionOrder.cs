using System.Collections.Generic;

namespace Bitzawolf
{
    public class PotionOrder
    {
        private List<Ingredient> requestedIngredients;

        public PotionOrder(params Ingredient[] ingredients)
        {
            requestedIngredients = new List<Ingredient>();
            requestedIngredients.AddRange(ingredients);
        }

        public bool IsOrderFilled()
        {
            PotionBottle bottle = GameManager.GetInstance().bottle;
            if (bottle.IsEmpty() || bottle.IsDead())
                return false;
            
            // Does the contents of the bottle match the order's request regardless
            // of ingredient order, and also no duplicates.
            List<Ingredient> bottleContents = bottle.GetContents();
            foreach (Ingredient ing in requestedIngredients)
            {
                if (!bottleContents.Contains(ing))
                    return false;

                bottleContents.Remove(ing);
            }
            return (bottleContents.Count == 0);
        }

        public string GetOrderAsString()
        {
            string symbols = "";
            foreach (Ingredient ing in requestedIngredients)
            {
                symbols = symbols + ing.GetSymbol() + " ";
            }
            return symbols;
        }

        public List<Ingredient> GetIngredients()
        {
            List<Ingredient> contentsCopy = new List<Ingredient>();
            for (int i = 0; i < requestedIngredients.Count; ++i)
            {
                contentsCopy.Add(requestedIngredients[i]);
            }
            return contentsCopy;
        }
    }
}