using UnityEngine;

namespace Bitzawolf
{
    public class IngredientDump : MonoBehaviour
    {
        private BoxCollider boxCollider;

        private void Start()
        {
            boxCollider = GetComponent<BoxCollider>();
        }

        private void OnDrawGizmos()
        {
            boxCollider = GetComponent<BoxCollider>();
            Gizmos.color = new Color(0.3f, 0.3f, 0.3f, 0.4f);
            Gizmos.DrawCube(boxCollider.bounds.center, boxCollider.bounds.size);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                GameManager.GetInstance().bottle.DumpBottle();
            }
        }
    }
}
