using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Bitzawolf
{
    public class OrderUI : MonoBehaviour
    {
        public Text text;
        public Image img_order1, img_order2, img_order3;
        public Sprite spr_red, spr_green, spr_blue;
        private GameManager gm;

        private void Start()
        {
            gm = GameManager.GetInstance();
        }

        public void SetSymbols()
        {
            img_order1.enabled = false;
            img_order2.enabled = false;
            img_order3.enabled = false;

            if (gm == null)
            {
                gm = GameManager.GetInstance();
            }
            PotionOrder order = gm.GetOrder();
            List<Ingredient> ingredients = order.GetIngredients();
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
            if (! gm.HasOrders())
            {
                text.text = "";
                return;
            }

            PotionOrder order = gm.GetOrder();
            if (order.IsOrderFilled())
                text.text = "Order Ready!";
            else
                text.text = "Order Not Ready";
        }
    }
}
