using UnityEngine;

namespace Bitzawolf
{
    public class IngredientSource : MonoBehaviour
    {
        public Ingredient ingredient = Ingredient.Red;

        private BoxCollider boxCollider;

        private void Start()
        {
            boxCollider = GetComponent<BoxCollider>();
        }

        private void OnDrawGizmos()
        {
            boxCollider = GetComponent<BoxCollider>();
            Color c = ingredient.GetColor();
            c.a = 0.4f;
            Gizmos.color = c;
            Gizmos.DrawCube(boxCollider.bounds.center, boxCollider.bounds.size);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                GameManager.GetInstance().bottle.AddIngredient(ingredient);
                GameManager.GetInstance().aud_ingredient.Play();
            }
        }
    }
}
