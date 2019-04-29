using UnityEngine;
using System.Collections.Generic;

namespace Bitzawolf
{
    public class CustomerOrder : BitzawolfGameObject
    {
        public OrderUI orderUI;

        private BoxCollider boxCollider;
        private bool hasGivenOrder = false;
        private PotionOrder myOrder;
        private float difficuluty, timeToFinish;

        public override void OnStart()
        {
            base.OnStart();
            boxCollider = GetComponent<BoxCollider>();
            timeToFinish = 0;
            GameManager.GetInstance().AddOrder(GetRandomOrder());
            orderUI.SetSymbols();
        }

        public override void UpdateInLevel()
        {
            base.UpdateInLevel();
            if (hasGivenOrder)
            {
                timeToFinish += Time.deltaTime;
            }
        }

        private void OnDrawGizmos()
        {
            boxCollider = GetComponent<BoxCollider>();
            Gizmos.color = new Color(1.0f, 1.0f, 1.0f, 0.4f);
            Gizmos.DrawCube(boxCollider.bounds.center, boxCollider.bounds.size);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                GameManager gm = GameManager.GetInstance();

                if (hasGivenOrder)
                {
                    if (myOrder.IsOrderFilled())
                    {
                        gm.CompleteOrder(myOrder);
                        gm.bottle.DumpBottle();
                        gm.AwardPoints(CalculatePoints());
                        hasGivenOrder = false;
                        timeToFinish = 0;
                        gm.AddOrder(GetRandomOrder());
                        orderUI.SetSymbols();
                    }
                }
                else
                {
                    gm.AddOrder(GetRandomOrder());
                    orderUI.SetSymbols();
                }
            }
        }

        private PotionOrder GetRandomOrder()
        {
            int numIngredients = 1;
            difficuluty = Random.Range(0.0f, 1.0f) * 10;
            if (difficuluty <= 3)
                numIngredients = 1;
            else if (difficuluty <= 6)
                numIngredients = 2;
            else
                numIngredients = 3;

            Ingredient[] ingredients = new Ingredient[numIngredients];
            for (int i = 0; i < numIngredients; ++i)
            {
                int ingredientIndex = (int)(Random.Range(0.0f, 3.0f));
                switch (ingredientIndex)
                {
                    default:
                    case 0: ingredients[i] = Ingredient.Red; break;
                    case 1: ingredients[i] = Ingredient.Green; break;
                    case 2: ingredients[i] = Ingredient.Blue; break;
                }
            }

            myOrder = new PotionOrder(ingredients);
            hasGivenOrder = true;
            return myOrder;
        }

        private int CalculatePoints()
        {
            List<Ingredient> orderIngredients = myOrder.GetIngredients();

            int difficultyPoints = (int)(difficuluty * 100);

            // reduce points if the potion has duplicate ingredients
            int duplicatePenalty = 0;
            if (orderIngredients.Count > 1)
            {
                int red = 0, green = 0, blue = 0;
                foreach (Ingredient i in orderIngredients)
                {
                    switch (i)
                    {
                        case Ingredient.Red: ++red; break;
                        case Ingredient.Green: ++green; break;
                        case Ingredient.Blue: ++blue; break;
                    }
                }
                red = Mathf.Max(red - 1, 0);
                green = Mathf.Max(green - 1, 0);
                blue = Mathf.Max(blue - 1, 0);
                int duplicateCount = red + green + blue;
                duplicatePenalty = -100 * duplicateCount;
            }

            // Time bonus ends at 1 minute
            // Time bonus fastest is 10 seconds
            // So we create a factor between 10 and 60 seconds
            // The factor ranges between 0 and 1. 1 being the max time bonus, 0 being the least.
            float fastestTime = 5;
            float slowestTime = 10;
            float timeBonusFactor = Mathf.Abs(Mathf.Min(Mathf.Max((timeToFinish - fastestTime), 0) / slowestTime, 1) - 1);
            int timeBonus = (int)(timeBonusFactor * 500);
            //Debug.Log("My time: " + timeToFinish + " | factor: " + timeBonusFactor + " | Bonus: " + timeBonus);

            int basePoints = 250;
            int netPoints = basePoints + difficultyPoints + timeBonus + duplicatePenalty;
            netPoints = Mathf.Max(netPoints, basePoints);
            int debugPoints = netPoints;
            // Make the net points a multiple of 10 for nice even numbers
            netPoints /= 10;
            netPoints *= 10;

            //Debug.Log("Diff: " + difficultyPoints + " | Dupe: " + duplicatePenalty + " | Time: " + timeBonus + " | Base: " + basePoints);
            //Debug.Log("Net Points: " + netPoints + " | (really: " + debugPoints + ")");

            return netPoints;
        }
    }
}
