using UnityEngine;
using System.Collections.Generic;

namespace Bitzawolf
{
    public class PotionBottle : MonoBehaviour
    {
        public int maxIngredientsPerBottle = 3;

        private List<Ingredient> contents;
        private bool dead = false;

        void Start()
        {
            contents = new List<Ingredient>();
            dead = false;
        }

        public void AddIngredient(Ingredient ingredient)
        {
            if (IsDead() || ingredient == Ingredient.None)
                return;

            if (IsAtCapacity())
            {
                KillPotion();
                return;
            }

            contents.Add(ingredient);
        }

        public bool IsAtCapacity()
        {
            return (contents.Count >= maxIngredientsPerBottle);
        }

        public bool IsEmpty()
        {
            return (contents.Count <= 0);
        }

        public bool IsDead()
        {
            return dead;
        }

        public void KillPotion()
        {
            dead = true;
        }

        public void DumpBottle()
        {
            contents.Clear();
            dead = false;
        }

        public string GetContentsAsString()
        {
            if (IsEmpty())
                return "<Empty Bottle>";

            if (IsDead())
                return "<Toxic Bottle>";

            string symbols = "";
            foreach (Ingredient ing in contents)
            {
                symbols = symbols + ing.GetSymbol() + " ";
            }
            return symbols;
        }

        public List<Ingredient> GetContents()
        {
            List<Ingredient> contentsCopy = new List<Ingredient>();
            for (int i = 0; i < contents.Count; ++i)
            {
                contentsCopy.Add(contents[i]);
            }
            return contentsCopy;
        }
    }
}