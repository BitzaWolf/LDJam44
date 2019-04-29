using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Bitzawolf
{
    public class BottleUI : MonoBehaviour
    {
        public Image img_order1, img_order2, img_order3;
        public Sprite spr_red, spr_green, spr_blue, spr_dead;
        private PotionBottle bottle;

        private void Start()
        {
            bottle = GameManager.GetInstance().bottle;
        }

        public void SetSymbols()
        {
            if (bottle.IsDead())
            {
                img_order1.sprite = spr_dead;
                img_order2.enabled = false;
                img_order3.enabled = false;
                return;
            }
            img_order1.enabled = false;
            img_order2.enabled = false;
            img_order3.enabled = false;

            List<Ingredient> ingredients = bottle.GetContents();
            for (int i = 0; i < ingredients.Count; ++i)
            {
                Sprite spr = null;
                switch (ingredients[i])
                {
                    case Ingredient.Red: spr = spr_red; break;
                    case Ingredient.Green: spr = spr_green; break;
                    case Ingredient.Blue: spr = spr_blue; break;
                }
                if (i == 0)
                {
                    img_order1.sprite = spr;
                    img_order1.enabled = true;
                }
                else if (i == 1)
                {
                    img_order2.sprite = spr;
                    img_order2.enabled = true;
                }
                else
                {
                    img_order3.sprite = spr;
                    img_order3.enabled = true;
                }
            }
        }

        private void Update()
        {
            SetSymbols();
        }
    }
}
